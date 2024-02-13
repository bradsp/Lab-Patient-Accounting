using System;
using Microsoft.Data.SqlClient;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using System.Runtime.CompilerServices;
using LabBilling.Core.UnitOfWork;
using LabBilling.Core.Services;

namespace LabBilling.Core.DataAccess
{
    public sealed class ClientRepository : RepositoryBase<Client>
    {

        private const string clientFinCode = "CLIENT";

        public ClientRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

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
            var queryResult = Context.Fetch<Client>(sql);

            foreach(var item in queryResult) 
            {
                item.ClientType = unitOfWork.ClientTypeRepository.GetByType(item.Type);    
            }

            Log.Instance.Debug(Context.LastSQL);

            return queryResult;
        }

        public Client GetClient(string clientMnem)
        {
            Log.Instance.Debug($"Entering - {clientMnem}");
            MappingRepository mappingRepository = new(AppEnvironment, unitOfWork);

            if (clientMnem == null)
            {
                throw new ArgumentNullException(nameof(clientMnem));
            }

            var record = Context.SingleOrDefault<Client>("where cli_mnem = @0", new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });
            Log.Instance.Debug(Context.LastSQL);
            if (record != null)
            {
                record.Discounts = unitOfWork.ClientDiscountRepository.GetByClient(clientMnem);
                record.ClientType = unitOfWork.ClientTypeRepository.GetByType(record.Type);
                record.Mappings = mappingRepository.GetMappingsBySendingValue("CLIENT", record.ClientMnem).ToList();
            }
            Log.Instance.Debug(Context.LastSQL);
            return record;
        }

        public override object Add(Client table)
        {
            if (string.IsNullOrEmpty(table.BillProfCharges))
                table.BillProfCharges = "NO";

            if (string.IsNullOrEmpty(table.BillMethod))
                table.BillMethod = "PER ACCOUNT";
            AccountRepository accountRepository = new(AppEnvironment, unitOfWork);

            var account = accountRepository.GetByAccount(table.ClientMnem, true);

            if(account == null)
            {
                //add account
                account = new Account
                {
                    AccountNo = table.ClientMnem,
                    PatFullName = table.Name,
                    FinCode = clientFinCode,
                    TransactionDate = DateTime.Today,
                    Status = AccountStatus.New,
                    ClientMnem = table.ClientMnem,
                    MeditechAccount = table.ClientMnem
                };

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
                unitOfWork.ClientDiscountRepository.SaveAll(table.Discounts);
                success = this.Update(table);
            }
            else
            {
                try
                {
                    this.Add(table);
                    if(table.Discounts != null)
                        unitOfWork.ClientDiscountRepository.SaveAll(table.Discounts);
                    success = true;
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    success = false;
                }
            }
            Log.Instance.Debug(Context.LastSQL);
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
                throw new ArgumentNullException(nameof(clientMnem));

             var balanceReturn = Context.ExecuteScalar<double>("select dbo.GetAccBalance(@0)",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });

            return balanceReturn;

        }

        public double Balance(string clientMnem, DateTime asOfDate)
        {
            if (clientMnem == null)
                throw new ArgumentNullException(nameof(clientMnem));
            if (asOfDate > DateTime.Now)
                throw new ArgumentOutOfRangeException(nameof(asOfDate));

            var balance = Context.ExecuteScalar<double>("select dbo.GetAccBalByDate(@0, @1)",
                new SqlParameter() { ParameterName = "@account", SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem },
                new SqlParameter() { ParameterName = "@effDate", SqlDbType = System.Data.SqlDbType.DateTime, Value = asOfDate });

            return balance;
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
            AccountRepository accdb = new AccountRepository(AppEnvironment, unitOfWork);
            account = accdb.GetByAccount(clientMnem);

            if (account == null)
            {
                //account does not exist - add the account
                account = new Account
                {
                    AccountNo = clientMnem,
                    PatFullName = client.Name,
                    MeditechAccount = clientMnem,
                    FinCode = clientFinCode,
                    TransactionDate = DateTime.Today,
                    ClientMnem = clientMnem
                };

                accdb.Add(account);
            }

        }

        public async Task<List<UnbilledClient>> GetUnbilledClientsAsync(DateTime thruDate, IProgress<int> progress) => await Task.Run(() => GetUnbilledClients(thruDate, progress));

        public event EventHandler<ProgressEventArgs> ProgressChanged;

        public List<UnbilledClient> GetUnbilledClients(DateTime thruDate, IProgress<int> progress)
        {
            AccountService accountService = new(AppEnvironment);

            List<UnbilledClient> unbilledClients = new();

            var unbilledAccounts = GetUnbilledAccounts(thruDate, progress);

            var clients = GetAll(false);

            int total = clients.Count;
            int processed = 0;

            foreach (var client in clients)
            {
                UnbilledClient unbilledClient = new()
                {
                    ClientMnem = client.ClientMnem,
                    ClientName = client.Name,
                    ClientType = client.ClientType.Description,
                    PriorBalance = accountService.GetBalance(client.ClientMnem),
                    UnbilledAccounts = unbilledAccounts.Where(x => x.ClientMnem == client.ClientMnem).ToList()
                };

                if(unbilledClient.PriorBalance != 0 && unbilledAccounts.Sum(x => x.UnbilledAmount) != 0)
                    unbilledClients.Add(unbilledClient);


                progress?.Report(HelperExtensions.ComputePercentage(++processed, total));
            }

            return unbilledClients;
        }

        private List<UnbilledAccounts> GetUnbilledAccounts(DateTime thruDate, IProgress<int> progress)
        {
            string chrgTableName = GetTableInfo(typeof(Chrg)).TableName;
            string accTableName = GetTableInfo(typeof(Account)).TableName;

            var cmd = Sql.Builder
                .Select(new[]
                {
                    chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.AccountNo)),
                    chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.ClientMnem)),
                    accTableName + "." + GetRealColumn(typeof(Account), nameof(Account.TransactionDate)),
                    accTableName + "." + GetRealColumn(typeof(Account), nameof(Account.PatFullName)),
                    chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.FinCode))
                })
                .From(chrgTableName)
                .InnerJoin(accTableName)
                .On($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.AccountNo))} = {accTableName}.{GetRealColumn(typeof(Account), nameof(Account.AccountNo))}")
                .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Status))} not in ('CBILL','N/A')")
                .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Invoice))} is null or {chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Invoice))} = ''")
                .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.FinCode))} in ('APC','X','Y','Z')")
                .Where($"{accTableName}.{GetRealColumn(typeof(Account), nameof(Account.Status))} not like '%HOLD%'")
                .Where($"{accTableName}.{GetRealColumn(typeof(Account), nameof(Account.TransactionDate))} <= @0 ", 
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate })
                .GroupBy(new[]
                {
                    chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.AccountNo)),
                    chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.ClientMnem)),
                    accTableName + "." + GetRealColumn(typeof(Account), nameof(Account.TransactionDate)),
                    accTableName + "." + GetRealColumn(typeof(Account), nameof(Account.PatFullName)),
                    chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.FinCode))
                });

            var results = Context.Fetch<UnbilledAccounts>(cmd);
            int processed = 0;
            foreach (var result in results)
            {
                if (!string.IsNullOrEmpty(result.ClientMnem))
                {
                    var unbilledBalance = Context.ExecuteScalar<double>("select dbo.GetAccClientBalance(@0, @1)",
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = result.Account },
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = result.ClientMnem });

                    result.UnbilledAmount = unbilledBalance;
                }
                else
                {
                    result.UnbilledAmount = 0.00;
                }

                progress?.Report(HelperExtensions.ComputePercentage(++processed, results.Count));
            }

            return results.Where(x => x.UnbilledAmount != 0.00).ToList();
        }
    }
}
