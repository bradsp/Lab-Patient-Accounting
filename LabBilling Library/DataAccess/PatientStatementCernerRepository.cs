using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatientStatementCernerRepository : RepositoryBase<PatientStatementCerner>
    {
        public PatientStatementCernerRepository(IAppEnvironment appEnvironment) : base(appEnvironment) { }

        public List<PatientStatementCerner> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            //SqlCommand cmdSelectNotices = new SqlCommand(
            //string.Format("select * from pat_statements_cerner " +
            //"where batch_id = '{0}' " +
            //"order by statement_number,  activity_id, record_cnt ", strBatchId)
            //, conn);

            var sql = PetaPoco.Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementCerner.BatchId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });

            var results = dbConnection.Fetch<PatientStatementCerner>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;
        }

    }
}
