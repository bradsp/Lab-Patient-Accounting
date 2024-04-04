using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System.Data;

namespace LabBilling.Core.DataAccess;

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

    public override AccountValidationStatus Save(AccountValidationStatus table)
    {
        //TODO: error catching
        if (Context.Exists<AccountValidationStatus>((object)table.Account))
        {
            return this.Update(table);
        }
        else
        {
            return this.Add(table);
        }
    }

}
