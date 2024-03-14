using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatientStatementAccountRepository : RepositoryBase<PatientStatementAccount>
    {
        public PatientStatementAccountRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) { }

        public List<PatientStatementAccount> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementAccount.BatchId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });
            sql.Where($"{GetRealColumn(nameof(PatientStatementAccount.DateSent))} is null");

            var results = Context.Fetch<PatientStatementAccount>(sql);
            Log.Instance.Debug(Context.LastSQL);
            return results;
        }

        public List<PatientStatementAccount> GetByAccount(string account)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementAccount.AccountId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = account });

            var results = Context.Fetch<PatientStatementAccount>(sql);
            Log.Instance.Debug(Context.LastSQL);
            return results;

        }

        public List<PatientStatementAccount> GetByStatement(Int64 statementNo)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementAccount.StatementNumber))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.Int, Value = statementNo });

            var results = Context.Fetch<PatientStatementAccount>(sql);
            Log.Instance.Debug(Context.LastSQL);
            return results;
        }

    }
}
