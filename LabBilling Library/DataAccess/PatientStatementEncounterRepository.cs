using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatientStatementEncounterRepository : RepositoryBase<PatientStatementEncounter>
    {
        public PatientStatementEncounterRepository(IAppEnvironment appEnvironment) : base(appEnvironment) { }

        public List<PatientStatementEncounter> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            //SqlCommand cmdSelectEnctr = new SqlCommand(
            //    string.Format(
            //    "select * from dbo.patbill_enctr " +
            //    "WHERE batch_id = '{0}' and statement_number in ( " +
            //    "select statement_number from dbo.patbill_acc where batch_id = '{0}' " +
            //    "and nullif(date_sent,'') is null " +
            //    " ) " +
            //    "order by statement_number, record_cnt", strBatchId)
            //    , conn);

            var sql = PetaPoco.Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounter.BatchId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });
            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounter.StatementNumber))} IN (select statement_number from dbo.patbill_acc where batch_id = @0 " + 
                "and nullif(date_sent,'') is null)",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });


            var results = dbConnection.Fetch<PatientStatementEncounter>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;

        }
        
    }
}
