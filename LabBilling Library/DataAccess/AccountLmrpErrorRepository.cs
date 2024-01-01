using Microsoft.Data.SqlClient;
using System.Data;

using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public sealed class AccountLmrpErrorRepository : RepositoryBase<AccountLmrpError>
    {
        public AccountLmrpErrorRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public override bool Save(AccountLmrpError table)
        {
            //lookup record by account
            var record = GetByAccount(table.AccountNo);

            if(record == null)
            {
                this.Add(table);
                return true;
            }
            else
            {
                table.uri = record.uri;
                return this.Update(table);
            }

        }

        public AccountLmrpError GetByAccount(string accountNo)
        {
            var record = dbConnection.SingleOrDefault<AccountLmrpError>($"where {this.GetRealColumn(typeof(AccountLmrpError), nameof(AccountLmrpError.AccountNo))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });

            return record;
        }

    }
}
