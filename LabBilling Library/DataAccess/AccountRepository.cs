using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.DataAccess
{
    public class AccountRepository : RepositoryBase<Account>
    {
        private string _connection;
        private readonly PatRepository patRepository;

        public AccountRepository(string connectionString) : base("acc", connectionString)
        {
            _connection = connectionString;
            patRepository = new PatRepository(_connection);
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

        public Account GetByAccount(string account)
        {
            Log.Instance.Debug($"Entering");

            var record = dbConnection.SingleOrDefault<Account>("where account = @0", account);

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
            patRepository.Add(acc.Pat);
        }

        public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
        {
            Log.Instance.Trace("Entering");

            return dbConnection.Fetch<InvoiceSelect>("where cl_mnem = @0 and trans_date <= @1", clientMnem, thruDate);

        }
    }
}
