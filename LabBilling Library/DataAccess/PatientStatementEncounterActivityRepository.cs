using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatientStatementEncounterActivityRepository : RepositoryBase<PatientStatementEncounterActivity>
    {
        public PatientStatementEncounterActivityRepository(string connectionString) : base(connectionString) { }

        public PatientStatementEncounterActivityRepository(Database db) : base(db) { }

        public List<PatientStatementEncounterActivity> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            //SqlCommand cmdSelectEnctrActv = new SqlCommand(
            //    string.Format(
            //    "select * from dbo.patbill_enctr_actv  " +
            //    "WHERE batch_id = '{0}' and statement_number in ( " +
            //    "select statement_number from dbo.patbill_acc where batch_id = '{0}' " +
            //    "and nullif(date_sent,'') is null " +
            //    " ) " +
            //    "order by statement_number,  parent_activity_id, record_cnt", strBatchId)
            //    , conn);

            var sql = PetaPoco.Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounterActivity.BatchId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });
            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounterActivity.StatementNumber))} IN (select statement_number from dbo.patbill_acc where batch_id = @0 " +
                "and nullif(date_sent,'') is null)",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });


            var results = dbConnection.Fetch<PatientStatementEncounterActivity>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;

        }
    }
}
