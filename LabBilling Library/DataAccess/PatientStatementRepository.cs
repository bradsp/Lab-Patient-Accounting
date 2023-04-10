using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{

    public class PatientStatementRepository : RepositoryBase<PatientStatement>
    {

        public PatientStatementRepository(string connectionString) : base(connectionString)  
        { 

        }

        public PatientStatementRepository(Database db) : base(db) 
        {

        }

        public int GetStatementCount(string batch)
        {
            var sql = PetaPoco.Sql.Builder
                .Select("count(*) as 'Cnt'")
                .From("dbo.patbill_stmt")
                .Where($"{GetRealColumn(nameof(PatientStatement.BatchId))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = batch });

            int cnt = dbConnection.ExecuteScalar<int>(sql);

            return cnt;
        }

        public List<PatientStatement> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            //SqlCommand cmdSelectStmt = new SqlCommand(
            //string.Format(
            //"select * from dbo.patbill_stmt " +
            //"WHERE dbo.patbill_stmt.batch_id = '{0}' " +
            //" and nullif(statement_submitted_dt_tm,'') is null " +
            //"order by statement_number, record_cnt", strBatchId)
            //, conn);

            var sql = PetaPoco.Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatement.BatchId))} = @0", 
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch});
            sql.Where($"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} is null or {GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} = '01/01/1900'");

            var results = dbConnection.Fetch<PatientStatement>(sql);

            //foreach ( var row in results )
            //{
            //    row.Accounts = _accountRepository.GetByBatch(batch);
            //    row.CernerStatements = _cernerRepository.GetByBatch(batch);
            //    row.Encounters = _encounterRepository.GetByBatch(batch);
            //    row.EncounterActivity = _activityRepository.GetByBatch(batch);
            //}

            Log.Instance.Debug(dbConnection.LastSQL);
            return results;
        }

    }
}
