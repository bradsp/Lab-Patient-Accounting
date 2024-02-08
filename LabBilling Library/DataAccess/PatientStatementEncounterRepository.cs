using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatientStatementEncounterRepository : RepositoryBase<PatientStatementEncounter>
    {
        public PatientStatementEncounterRepository(IAppEnvironment appEnvironment) : base(appEnvironment) { }

        public List<PatientStatementEncounter> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounter.BatchId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });
            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounter.StatementNumber))} IN (select statement_number from dbo.patbill_acc where batch_id = @0 " + 
                "and nullif(date_sent,'') is null)",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });


            var results = dbConnection.Fetch<PatientStatementEncounter>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;

        }

        public List<PatientStatementEncounter> GetByStatement(Int64 statementNo)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounter.StatementNumber))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.Int, Value = statementNo });

            var results = dbConnection.Fetch<PatientStatementEncounter>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;
        }
    }
}
