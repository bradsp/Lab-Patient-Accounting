using Microsoft.Data.SqlClient;
using System.Data;

using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class AccountLmrpErrorRepository : RepositoryBase<AccountLmrpError>
    {
        public AccountLmrpErrorRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

        }

        public override AccountLmrpError Save(AccountLmrpError model)
        {
            //lookup record by account
            var record = GetByAccount(model.AccountNo);

            if(record == null)
            {                
                return this.Add(model);
            }
            else
            {
                model.uri = record.uri;
                return this.Update(model);
            }

        }

        public AccountLmrpError GetByAccount(string accountNo)
        {
            var record = Context.SingleOrDefault<AccountLmrpError>($"where {this.GetRealColumn(typeof(AccountLmrpError), nameof(AccountLmrpError.AccountNo))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });

            return record;
        }

    }
}
