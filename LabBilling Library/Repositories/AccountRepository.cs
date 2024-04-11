using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Log = LabBilling.Logging.Log;



namespace LabBilling.Core.DataAccess;

public sealed class AccountRepository : RepositoryBase<Account>, IRepositoryBase<Account>, IAccountRepository
{
    public AccountRepository(IAppEnvironment appEnvironment, IDatabase context) : base(appEnvironment, context)
    {

    }

    public async Task<Account> GetByAccountAsync(string account) => await Task.Run(() => GetByAccount(account));

    public Account GetByAccount(string account)
    {
        Logging.Log.Instance.Trace($"Entering - account {account}");

        var record = Context.SingleOrDefault<Account>((object)account);

        if (record == null)
            return null;

        return record;
    }

    public override async Task<Account> AddAsync(Account acc) => await Task.Run(() => Add(acc));

    public override Account Add(Account acc)
    {
        Log.Instance.Trace($"Entering - account {acc.AccountNo}");
        if (string.IsNullOrEmpty(acc.Status))
            acc.Status = AccountStatus.New;

        acc.PatFullName = acc.PatNameDisplay;

        return base.Add(acc);
    }


    public IEnumerable<string> GetByStatus(string status)
    {
        Log.Instance.Trace("Entering");

        var sql = PetaPoco.Sql.Builder
            .Select(GetRealColumn(nameof(Account.AccountNo)))
            .From(_tableName)
            .Where($"{GetRealColumn(nameof(Account.Status))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = status })
            .OrderBy(GetRealColumn(nameof(Account.AccountNo)));

        var results = Context.Fetch<string>(sql);

        return results;
    }

    public async Task<Account> UpdateAsync(Account table) => await Task.Run(() => Update(table));

    public override Account Update(Account table)
    {
        Log.Instance.Trace($"Entering - account {table.AccountNo}");

        if (table == null)
            throw new ArgumentNullException(nameof(table));

        if (string.IsNullOrEmpty(table.Status))
            table.Status = AccountStatus.New;
        table.PatFullName = table.PatNameDisplay;

        Log.Instance.Trace("Exiting");
        return base.Update(table);
    }

    public async Task<Account> UpdateAsync(Account table, IEnumerable<string> columns) => await Task.Run(() => Update(table, columns));

    public override Account Update(Account table, IEnumerable<string> columns)
    {
        Log.Instance.Trace($"Entering - account {table.AccountNo}");
        //generate full name field from name parts
        if (string.IsNullOrEmpty(table.Status))
            table.Status = AccountStatus.New;
        table.PatFullName = table.PatNameDisplay;

        Log.Instance.Trace($"Exiting");
        return base.Update(table, columns);
    }

    public async Task<int> UpdateStatusAsync(string accountNo, string status) => await Task.Run(() => UpdateStatus(accountNo, status));
    public async Task<Account> UpdateStatusAsync(Account model, string status) => await Task.Run(() => UpdateStatus(model, status));

    public Account UpdateStatus(Account model, string status)
    {
        Log.Instance.Trace($"Entering - account{model.AccountNo}, status {status}");
        if (!AccountStatus.IsValid(status))
            throw new ArgumentOutOfRangeException(nameof(status), "Invalid status");

        model.Status = status;
        return Update(model, new[] { nameof(Account.Status) });
    }

    public int UpdateStatus(string accountNo, string status)
    {
        Log.Instance.Trace($"Entering - account {accountNo} status {status}");

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(status))
            throw new ArgumentNullException(nameof(status));
        if (!AccountStatus.IsValid(status))
            throw new ArgumentOutOfRangeException(nameof(status), "Invalid status");

        var result = Context.Update<Account>($"set {GetRealColumn(nameof(Account.Status))} = @0, {GetRealColumn(nameof(Account.UpdatedDate))} = @1, " +
            $"{GetRealColumn(nameof(Account.UpdatedUser))} = @2, {GetRealColumn(nameof(Account.UpdatedApp))} = @3, {GetRealColumn(nameof(Account.UpdatedHost))} = @4 " +
            $"where {GetRealColumn(nameof(Account.AccountNo))} = @5",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = status },
            new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = DateTime.Now },
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.UserName.ToString() },
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Utilities.OS.GetAppName() },
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName },
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });

        return result;

    }

}
