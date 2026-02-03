using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{

    public sealed class PatientStatementRepository : RepositoryBase<PatientStatement>
    {

        public PatientStatementRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        { 
        }
        
        public PatientStatement GetByStatement(long statementNo)
        {
            Log.Instance.Trace("Entering");
            var sql = Sql.Builder
                .Where($"{GetRealColumn(nameof(PatientStatement.StatementNumber))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.BigInt, Value = statementNo });

            var result = Context.SingleOrDefault<PatientStatement>(sql);
            return result;
        }

        public List<PatientStatement> GetByStatement(IEnumerable<string> statementNos)
        {
            Log.Instance.Trace("Entering");
            var sql = Sql.Builder
                .Where($"{GetRealColumn(nameof(PatientStatement.StatementNumber))} in (@statementNos)");

            var statements = Context.Query<PatientStatement>(sql.ToString(), statementNos.ToArray());

            return statements.ToList();
        }

        public int GetStatementCount(string batch)
        {
            Log.Instance.Trace("Entering");
            var sql = PetaPoco.Sql.Builder
                .Select("count(*) as 'Cnt'")
                .From("dbo.patbill_stmt")
                .Where($"{GetRealColumn(nameof(PatientStatement.BatchId))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = batch });

            int cnt = Context.ExecuteScalar<int>(sql);

            return cnt;
        }

        public List<PatientStatement> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatement.BatchId))} = @0", 
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch});
            sql.Where($"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} is null or " +
                $"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = new DateTime(1900, 1, 1) });

            var results = Context.Fetch<PatientStatement>(sql);

            Log.Instance.Debug(Context.LastSQL);
            return results;
        }

    }
}
