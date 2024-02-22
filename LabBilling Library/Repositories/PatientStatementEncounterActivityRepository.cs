using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatientStatementEncounterActivityRepository : RepositoryBase<PatientStatementEncounterActivity>
    {
        public PatientStatementEncounterActivityRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) { }

        public List<PatientStatementEncounterActivity> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounterActivity.BatchId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });
            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounterActivity.StatementNumber))} IN (select statement_number from dbo.patbill_acc where batch_id = @0 " +
                "and nullif(date_sent,'') is null)",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });


            var results = Context.Fetch<PatientStatementEncounterActivity>(sql);
            Log.Instance.Debug(Context.LastSQL);
            return results;

        }

        public List<PatientStatementEncounterActivity> GetByStatement(Int64 statementNo)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounterActivity.StatementNumber))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.Int, Value = statementNo });

            var results = Context.Fetch<PatientStatementEncounterActivity>(sql);
            Log.Instance.Debug(Context.LastSQL);
            return results;
        }
    }
}
