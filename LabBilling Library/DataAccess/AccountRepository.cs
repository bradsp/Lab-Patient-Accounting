using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using Utilities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Log = LabBilling.Logging.Log;
using PetaPoco;
using NPOI.SS.Formula.Functions;
using LabBilling.Core.UnitOfWork;


namespace LabBilling.Core.DataAccess
{
    public sealed class AccountRepository : RepositoryBase<Account>, IRepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(IAppEnvironment appEnvironment, IDatabase context) : base(appEnvironment, context)
        {

        }

        public async Task<Account> GetByAccountAsync(string account, bool demographicsOnly = false) => await Task.Run(() => GetByAccount(account, demographicsOnly));

        public Account GetByAccount(string account, bool demographicsOnly = false)
        {
            Logging.Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");

            var record = Context.SingleOrDefault<Account>(account);

            if (record == null)
                return null;

            return record;
        }

        public async Task AddAccountAsync(Account acc) => await Task.Run(() => AddAccount(acc));

        public void AddAccount(Account acc)
        {
            Log.Instance.Trace($"Entering - account {acc.AccountNo}");
            if (string.IsNullOrEmpty(acc.Status))
                acc.Status = AccountStatus.New;

            acc.PatFullName = acc.PatNameDisplay;

            this.Add(acc);
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

        public async Task<bool> UpdateAsync(Account table) => await Task.Run(() => Update(table));

        public override bool Update(Account table)
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

        public async Task<bool> UpdateAsync(Account table, IEnumerable<string> columns) => await Task.Run(() => Update(table, columns));

        public override bool Update(Account table, IEnumerable<string> columns)
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

        public int UpdateStatus(string accountNo, string status)
        {
            Log.Instance.Trace($"Entering - account {accountNo} status {status}");

            if (string.IsNullOrEmpty(accountNo))
                throw new ArgumentNullException(nameof(accountNo));
            if (string.IsNullOrEmpty(status))
                throw new ArgumentNullException(nameof(status));
            if (!AccountStatus.IsValid(status))
                throw new ArgumentOutOfRangeException(nameof(status), "Invalid status");

            return Context.Update<Account>($"set {GetRealColumn(nameof(Account.Status))} = @0, {GetRealColumn(nameof(Account.UpdatedDate))} = @1, " +
                $"{GetRealColumn(nameof(Account.UpdatedUser))} = @2, {GetRealColumn(nameof(Account.UpdatedApp))} = @3, {GetRealColumn(nameof(Account.UpdatedHost))} = @4 " +
                $"where {GetRealColumn(nameof(Account.AccountNo))} = @5",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = status },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = DateTime.Now },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.UserName.ToString() },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Utilities.OS.GetAppName() },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });
        }

    }
}
