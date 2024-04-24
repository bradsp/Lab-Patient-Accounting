using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System;
using System.Data;

namespace LabBilling.Core.DataAccess;

public class AccountLockRepository : RepositoryBase<AccountLock>
{
    public AccountLockRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

    public AccountLock GetLock(string accountNo)
    {
        Log.Instance.Trace("Entering");

        var sql = Sql.Builder
            .Where($"{GetRealColumn(nameof(AccountLock.AccountNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });

        return Context.SingleOrDefault<AccountLock>(sql);
    }
    
    /// <summary>
    /// Set lock on the provided account no. Other details will be obtained from the system.
    /// </summary>
    /// <param name="accountNo"></param>
    /// <returns></returns>
    public AccountLock Add(string accountNo)
    {
        AccountLock alock = new()
        {
            AccountNo = accountNo,
            UpdatedUser = AppEnvironment.User,
            UpdatedDate = DateTime.Now,
            LockDateTime = DateTime.Now,
            UpdatedHost = Utilities.OS.GetMachineName(),
            UpdatedApp = Utilities.OS.GetAppName()
        };

        return Add(alock);
    }

    public bool Delete(int id)
    {
        var alock = Context.SingleOrDefault<AccountLock>((object)id);

        if (alock != null)
            return Delete(alock);

        return false;
    }

    public bool DeleteByUserHost(string username, string hostname)
    {
        var sql = Sql.Builder
            .Where($"{GetRealColumn(nameof(AccountLock.UpdatedUser))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = username })
            .Where($"{GetRealColumn(nameof(AccountLock.UpdatedHost))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = hostname });
        try
        {
            var retval = Context.Delete<AccountLock>(sql);
            return retval >= 0;
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal(ex);
            return false;
        }
    }

}
