using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;

namespace LabBilling.Core.DataAccess
{
    public class AccountRepository : RepositoryBase<Account>
    {
        private string _connection;
        private readonly PatRepository patRepository;
        private readonly InsRepository insRepository;
        private readonly ChrgRepository chrgRepository;
        private readonly ChkRepository chkRepository;
        private readonly ClientRepository clientRepository;
        private readonly NotesRepository notesRepository;
        private readonly BillingActivityRepository billingActivityRepository;

        public AccountRepository(string connectionString) : base("acc", connectionString)
        {
            _connection = connectionString;
            patRepository = new PatRepository(_connection);
            insRepository = new InsRepository(_connection);
            chrgRepository = new ChrgRepository(_connection);
            chkRepository = new ChkRepository(_connection);
            clientRepository = new ClientRepository(_connection);
            notesRepository = new NotesRepository(_connection);
            billingActivityRepository = new BillingActivityRepository(_connection);
        }

        public AccountRepository(string connectionString, PetaPoco.Database db) : base("acc", connectionString, db)
        {
            _connection = connectionString;
            patRepository = new PatRepository(_connection, db);
            insRepository = new InsRepository(_connection, db);
            chrgRepository = new ChrgRepository(_connection, db);
            chkRepository = new ChkRepository(_connection, db);
            clientRepository = new ClientRepository(_connection, db);
            notesRepository = new NotesRepository(_connection, db);
            billingActivityRepository = new BillingActivityRepository(_connection, db);
        }

        public override Account GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AccountSummary GetAccountSummary(string account)
        {
            Log.Instance.Debug($"Entering");

            var records = dbConnection.FetchProc<AccountSummary>("GetAccountSummary", new { accno = account });

            return records[0];
        }

        public Account GetByAccount(string account, bool demographicsOnly = false)
        {
            Log.Instance.Debug($"Entering");

            var record = dbConnection.SingleOrDefault<Account>("where account = @0", account);

            if(!Str.ParseName(record.pat_name, out string strLastName, out string strFirstName, out string strMiddleName, out string strSuffix))
            {
                this.Errors = string.Format("Patient name could not be parsed. {0} {1}\n", record.pat_name, record.account);
            }
            else
            {
                record.pat_name_last = strLastName;
                record.pat_name_first = strFirstName;
                record.pat_name_middle = strMiddleName;
                record.pat_name_suffix = strSuffix;
            }

            if (!demographicsOnly)
            {
                record.Pat = patRepository.GetByAccount(account);
                record.Insurances = insRepository.GetByAccount(account);
                record.Charges = chrgRepository.GetByAccount(account);
                record.Payments = chkRepository.GetByAccount(account);
                record.Notes = notesRepository.GetByAccount(account);
                record.BillingActivities = billingActivityRepository.GetByAccount(account);
            }
            Client client;
            if (record.cl_mnem != null)
            {
                client = clientRepository.GetClient(record.cl_mnem);
                record.ClientName = client.cli_nme;
            }

            double? result;

            //populate properties
            result = dbConnection.ExecuteScalar<double>("SELECT dbo.GetAccBalByDate(@0, @1)", account, DateTime.Today);
            record.Balance = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetBadDebtByAccount(@0)", account);
            record.TotalBadDebt = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetAccTotalCharges(@0)", account);
            record.TotalCharges = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetContractualByAccount(@0)", account);
            record.TotalContractual = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetAmtPaidByAccount(@0)", account);
            record.TotalPayments = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetWriteOffByAccount(@0)", account);
            record.TotalWriteOff = result == null ? 0.00 : (double)result;
            
            return record;
        }

        public IEnumerable<AccountSearch> GetBySearch(string lastNameSearchText, string firstNameSearchText, string mrnSearchText, string ssnSearchText, string dobSearch, string sexSearch, string accountSearchText)
        {
            Log.Instance.Debug($"Entering");

            if ((lastNameSearchText == string.Empty) && (firstNameSearchText == string.Empty) && (mrnSearchText == string.Empty) 
                && (ssnSearchText == string.Empty)
                && (dobSearch == string.Empty) && (sexSearch == string.Empty) && (accountSearchText == string.Empty))
            {
                return new List<AccountSearch>();
            }

            try
            {

                string nameSearch = "";
                if(!(lastNameSearchText == "" && firstNameSearchText == ""))
                    nameSearch = string.Format("{0}%,{1}%", lastNameSearchText, firstNameSearchText);

                var command = PetaPoco.Sql.Builder
                    .Append("select acc.account, acc.trans_date, acc.pat_name, isnull(acc.ssn, pat.ssn) as 'SSN', pat.dob_yyyy, pat.sex, acc.mri")
                    .Append("from acc left outer join pat on acc.account = pat.account")
                    .Append("where acc.deleted = 0 ")
                    .Append("and (acc.pat_name like @0 or @1 = '')", nameSearch, nameSearch)
                    .Append("and (acc.account = @0 or @1 = '')", accountSearchText, accountSearchText)
                    .Append("and (acc.mri = @0 or @1 = '')", mrnSearchText, mrnSearchText)
                    .Append("and (pat.sex = @0 or @1 = '')", sexSearch, sexSearch)
                    .Append("and (acc.ssn = @0 or @1 = '')", ssnSearchText, ssnSearchText)
                    .Append("and (dob_yyyy = @0 or @1 = '')", dobSearch, dobSearch)
                    .Append("order by acc.trans_date desc");

                return dbConnection.Fetch<AccountSearch>(command);
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception in");
            }

            return new List<AccountSearch>();

        }

        public override object Add(Account table)
        {
            patRepository.Add(table.Pat);
            return base.Add(table);
        }

        public void AddAccount(Account acc)
        {
            this.Add(acc);
            //patRepository.Add(acc.Pat);
        }

        public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
        {
            Log.Instance.Trace("Entering");

            return dbConnection.Fetch<InvoiceSelect>("where cl_mnem = @0 and trans_date <= @1", clientMnem, thruDate);

        }

        public IEnumerable<ClaimItem> GetAccountsForClaims(ClaimType claimType)
        {
            Log.Instance.Trace("Entering");

            PetaPoco.Sql command;

            try
            {
                switch (claimType)
                {
                    case ClaimType.Institutional:
                        command = PetaPoco.Sql.Builder;
                        break;
                    case ClaimType.Professional:
                        command = PetaPoco.Sql.Builder
                            .Select("status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme")
                            .From("acc")
                            .InnerJoin("ins").On("ins.account = acc.account and ins_a_b_c = 'a'")
                            .Where("status = '1500'")
                            .Where("ins_code not in ('CHAMPUS')");
                        break;
                    default:
                        command = PetaPoco.Sql.Builder;
                        break;
                }

                var queryResult = dbConnection.Fetch<ClaimItem>(command);

                return queryResult;
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception in");
            }
            Log.Instance.Trace("Exiting");

            return new List<ClaimItem>();
        }

        public enum ClaimType
        {
            Institutional,
            Professional
        }

        public override bool Update(Account table)
        {
            //generate full name field from name parts
            table.pat_name = String.Format("{0},{1} {2} {3}",
                table.pat_name_last,
                table.pat_name_first,
                table.pat_name_middle,
                table.pat_name_suffix);

            table.pat_name = table.pat_name.Trim();

            return base.Update(table);
        }

    }
}
