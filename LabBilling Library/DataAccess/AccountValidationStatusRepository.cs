using System.Data;
using Microsoft.Data.SqlClient;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public sealed class AccountValidationStatusRepository : RepositoryBase<AccountValidationStatus>
    {

        public AccountValidationStatusRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
        }

        public AccountValidationStatus GetByAccount(string account)
        {
            var record = dbConnection.SingleOrDefault<AccountValidationStatus>($"where {GetRealColumn(nameof(AccountValidationStatus.account))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account }) ?? new AccountValidationStatus();
            return record;
        }

        public override bool Save(AccountValidationStatus table)
        {
            //TODO: error catching
            bool retVal = true;
            if(dbConnection.Exists<AccountValidationStatus>((object)table.account))
            {
                this.Update(table);
            }
            else
            {
                this.Add(table);
            }

            return retVal;

        }

    }
}
