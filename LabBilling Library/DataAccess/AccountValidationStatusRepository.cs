using System.Data;
using Microsoft.Data.SqlClient;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public sealed class AccountValidationStatusRepository : RepositoryBase<AccountValidationStatus>
    {

        public AccountValidationStatusRepository(IAppEnvironment appEnvironment, IDatabase context) : base(appEnvironment, context)
        {
        }

        public AccountValidationStatus GetByAccount(string account)
        {
            var record = Context.SingleOrDefault<AccountValidationStatus>($"where {GetRealColumn(nameof(AccountValidationStatus.Account))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account }) ?? new AccountValidationStatus();
            return record;
        }

        public override bool Save(AccountValidationStatus table)
        {
            //TODO: error catching
            bool retVal = true;
            if(Context.Exists<AccountValidationStatus>((object)table.Account))
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
