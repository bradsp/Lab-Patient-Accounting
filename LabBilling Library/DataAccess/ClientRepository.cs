using System;
using System.Data.SqlClient;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Linq;

namespace LabBilling.Core.DataAccess
{
    public sealed class ClientRepository : RepositoryBase<Client>
    {
        ClientDiscountRepository clientDiscountRepository;
        ClientTypeRepository clientTypeRepository;
        //AccountRepository accountRepository;

        public ClientRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            clientDiscountRepository = new ClientDiscountRepository(appEnvironment);
            clientTypeRepository = new ClientTypeRepository(appEnvironment);
        }

        public List<Client> GetAll(bool includeInactive)
        {
            Log.Instance.Trace("Entering");
            PetaPoco.Sql sql;

            if (includeInactive)
            {
                sql = PetaPoco.Sql.Builder
                    .From(_tableName);
            }
            else
            {
                sql = PetaPoco.Sql.Builder
                    .From(_tableName)
                    .Where($"{GetRealColumn(nameof(Client.IsDeleted))} = @0", false);
            }
            var queryResult = dbConnection.Fetch<Client>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);

            return queryResult;
        }

        public Client GetClient(string clientMnem)
        {
            Log.Instance.Debug($"Entering - {clientMnem}");
            MappingRepository mappingRepository = new MappingRepository(_appEnvironment);

            if (clientMnem == null)
            {
                throw new ArgumentNullException(nameof(clientMnem));
            }

            var record = dbConnection.SingleOrDefault<Client>("where cli_mnem = @0", new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });
            Log.Instance.Debug(dbConnection.LastSQL);
            if (record != null)
            {
                record.Discounts = clientDiscountRepository.GetByClient(clientMnem);
                record.ClientType = clientTypeRepository.GetByType(record.Type);
                record.Mappings = mappingRepository.GetMappingsBySendingValue("CLIENT", record.ClientMnem).ToList();
            }
            Log.Instance.Debug(dbConnection.LastSQL);
            return record;
        }

        public override object Add(Client table)
        {
            if (string.IsNullOrEmpty(table.BillProfCharges))
                table.BillProfCharges = "NO";

            if (string.IsNullOrEmpty(table.BillMethod))
                table.BillMethod = "PER ACCOUNT";
            AccountRepository accountRepository = new AccountRepository(_appEnvironment);

            var account = accountRepository.GetByAccount(table.ClientMnem, true);

            if(account == null)
            {
                //add account
                account = new Account();
                account.AccountNo = table.ClientMnem;
                account.PatFullName = table.Name;
                account.FinCode = "CLIENT";
                account.TransactionDate = DateTime.Today;
                account.Status = "NEW";
                account.ClientMnem = table.ClientMnem;
                account.MeditechAccount = table.ClientMnem;

                accountRepository.Add(account);
            }

            return base.Add(table);
        }

        public override bool Update(Client table)
        {



            return base.Update(table);
        }

        public override bool Save(Client table)
        {
            var record = this.GetClient(table.ClientMnem);

            bool success;
            if (record != null)
            {
                clientDiscountRepository.SaveAll(table.Discounts);
                success = this.Update(table);
            }
            else
            {
                try
                {
                    this.Add(table);
                    if(table.Discounts != null)
                        clientDiscountRepository.SaveAll(table.Discounts);
                    success = true;
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    success = false;
                }
            }
            Log.Instance.Debug(dbConnection.LastSQL);
            return success;
        }

        /// <summary>
        /// Compute the client balance. Uses the vw_chrg_bal_cbill and vw_chk_bal_cbill
        /// which is essentially the same as the vw_chrg_bal and vw_chk_bal
        /// except it excludes charges/checks that have not been on a cbill
        /// (by using the invoice field).
        /// </summary>
        /// <param name="clientMnem"></param>
        /// <returns></returns>
        public double Balance(string clientMnem)
        {
            Log.Instance.Debug($"Entering");

            if(clientMnem == null)
                throw new ArgumentNullException("clientMnem");

             var balanceReturn = dbConnection.ExecuteScalar<double>("select dbo.GetAccBalance(@0)",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });

            return balanceReturn;

        }

        public double Balance(string clientMnem, DateTime asOfDate)
        {
            if (clientMnem == null)
                throw new ArgumentNullException("clientMnem");
            if (asOfDate > DateTime.Now)
                throw new ArgumentOutOfRangeException("asOfDate");

            var balance = dbConnection.ExecuteScalar<double>("select dbo.GetAccBalByDate(@0, @1)",
                new SqlParameter() { ParameterName = "@account", SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem },
                new SqlParameter() { ParameterName = "@effDate", SqlDbType = System.Data.SqlDbType.DateTime, Value = asOfDate });

            return balance;
        }

        public List<ClientStatementDetailModel> GetStatementDetails(string clientMnem, DateTime asOfDate)
        {

            ChkRepository chkRepository = new ChkRepository(_appEnvironment);
            ChrgRepository chrgRepository = new ChrgRepository(_appEnvironment);

            var charges = chrgRepository.GetByAccount(clientMnem, true, true, asOfDate, false);

            var payments = chkRepository.GetByAccount(clientMnem, asOfDate);

            List<ClientStatementDetailModel> statementDetails = new List<ClientStatementDetailModel>();

            foreach(var chrg in charges)
            {
                if (chrg.NetAmount == 0 && chrg.CDMCode == "CBILL")
                    continue;

                var statementDetail = new ClientStatementDetailModel();

                statementDetail.ServiceDate = chrg.ServiceDate == null ? DateTime.MinValue : (DateTime)chrg.ServiceDate;
                statementDetail.Account = chrg.AccountNo;
                statementDetail.Invoice = chrg.Invoice;
                statementDetail.Amount = chrg.NetAmount * chrg.Quantity;
                if (chrg.CDMCode == "CBILL")
                {
                    statementDetail.Description = $"Invoice {chrg.Invoice}";
                    statementDetail.Reference = chrg.Invoice;
                }
                else
                {
                    //see if account is in comment and extract it for the line description
                    string pattern = "([A-Z_]*)\\[(\\w*)\\]";
                    Regex rg = new Regex(pattern);
                    Match match = rg.Match(chrg.Comment);
                    if(match.Success)
                    {
                        string account = match.Groups[1].Value;
                        statementDetail.Description = $"Adjustment: {chrg.CdmDescription} on {account}";
                    }
                    else
                    {
                        statementDetail.Description = $"Adjustment: {chrg.CdmDescription}";
                    }
                }

                statementDetails.Add(statementDetail);
            }

            foreach(var chk in payments)
            {
                var statementDetail = new ClientStatementDetailModel();

                if(chk.PaidAmount > 0)
                {
                    statementDetail.Description = $"Payment Received - {chk.Comment}";
                }
                else if(chk.WriteOffAmount > 0)
                {
                    statementDetail.Description = $"Adjustment - {chk.Comment}";
                }
                statementDetail.Amount = (chk.PaidAmount + chk.ContractualAmount + chk.WriteOffAmount) * -1;
                statementDetail.ServiceDate = chk.DateReceived == null ? DateTime.Today : (DateTime)chk.DateReceived;
                statementDetail.Reference = chk.CheckNo;
                statementDetails.Add(statementDetail);
            }

            statementDetails.Sort((x, y) => DateTime.Compare(x.ServiceDate, y.ServiceDate));

            return statementDetails;
        }

        /// <summary>
        /// Adds new client account if it does not exist
        /// </summary>
        public void NewClient(string clientMnem)
        {
            //check to see if client is valid and client exists
            Client client = GetClient(clientMnem);

            if (client == null)
            {
                throw new ArgumentException("Client mnemonic is not found in client table", "clientMnem");
            }

            Account account;

            //check to see if client account exists
            AccountRepository accdb = new AccountRepository(_appEnvironment);
            account = accdb.GetByAccount(clientMnem);

            if (account == null)
            {
                //account does not exist - add the account
                account = new Account();
                account.AccountNo = clientMnem;
                account.PatFullName = client.Name;
                account.MeditechAccount = clientMnem;
                account.FinCode = "CLIENT";
                account.TransactionDate = DateTime.Today;
                account.ClientMnem = clientMnem;

                accdb.Add(account);
            }

        }

        public List<UnbilledClient> GetUnbilledClients(DateTime thruDate)
        {
            //var cmd = Sql.Builder
            //    .Append("select vbs.cl_mnem as 'ClientMnem', client.cli_nme as 'ClientName', dictionary.clienttype.description as 'ClientType', ")
            //    .Append("sum(dbo.GetAccBalance(vbs.account)) as 'UnbilledAmount' ")
            //    .Append("from vw_cbill_select vbs join client on vbs.cl_mnem = client.cli_mnem ")
            //    .Append("join dictionary.clienttype on client.[type] = dictionary.clienttype.[type] ")
            //    .Append("where trans_date <= @0", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate })
            //    .Append("group by vbs.cl_mnem, client.cli_nme, dictionary.clienttype.description ")
            //    .Append("order by vbs.cl_mnem ");

            //cmd = Sql.Builder
            //    .Select("client.cli_mnem, client.cli_nme, ct.description, dbo.GetAccBalance(acc.account) as 'PriorBalance', sum(dbo.GetAccBalance(vbs.account)) as 'UnbilledAmount'")
            //    .From("client ")
            //    .InnerJoin("acc").On("client.cli_mnem = acc.account")
            //    .LeftJoin("vw_cbill_select vbs").On("vbs.cl_mnem = client.cli_mnem")
            //    .InnerJoin("dictionary.clienttype ct").On("ct.type = client.[type]")
            //    .Where("client.deleted = 0")
            //    .GroupBy("client.cli_mnem, client.cli_nme, ct.description, dbo.GetAccBalance(acc.account)")
            //    .OrderBy("client.cli_mnem");

            var results = dbConnection.FetchProc<UnbilledClient>("dbo.GetClientsToBill",
                new SqlParameter() { ParameterName = "@thruDate", SqlDbType = SqlDbType.DateTime, Value = thruDate });

            return results;
        }

        public List<UnbilledAccounts> GetUnbilledAccounts(string clientMnem, DateTime thruDate)
        {
            var cmd = Sql.Builder
                .Select(new[] 
                {
                    "vbs.cl_mnem",
                    "vbs.account",
                    "vbs.trans_date",
                    "vbs.pat_name",
                    "vbs.fin_code",
                    "dbo.GetAccClientBalance(vbs.account, vbs.cl_mnem) as 'UnbilledAmount'"
                })
                .From("vw_cbill_select vbs")
                .Where("cl_mnem = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem })
                .Where("trans_date <= @0 ", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate });

            return dbConnection.Fetch<UnbilledAccounts>(cmd);
        }

    }
}
