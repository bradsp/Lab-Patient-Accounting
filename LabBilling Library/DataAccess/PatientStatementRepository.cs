using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{

    public class PatientStatementRepository : RepositoryBase<PatientStatement>
    {
        private PatientStatementAccountRepository _accountRepository;
        private PatientStatementCernerRepository _cernerRepository;
        private PatientStatementEncounterRepository _encounterRepository;
        private PatientStatementEncounterActivityRepository _activityRepository;

        public PatientStatementRepository(string connectionString) : base(connectionString)  
        { 
            _accountRepository = new PatientStatementAccountRepository(connectionString);
            _cernerRepository = new PatientStatementCernerRepository(connectionString);
            _encounterRepository = new PatientStatementEncounterRepository(connectionString);
            _activityRepository = new PatientStatementEncounterActivityRepository(connectionString);
        }

        public PatientStatementRepository(Database db) : base(db) 
        {
            _accountRepository = new PatientStatementAccountRepository(db);
            _cernerRepository = new PatientStatementCernerRepository(db);
            _encounterRepository = new PatientStatementEncounterRepository(db);
            _activityRepository = new PatientStatementEncounterActivityRepository(db);
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
            sql.Where($"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} is null");

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

    public class PatientStatementAccountRepository : RepositoryBase<PatientStatementAccount>
    {
        public PatientStatementAccountRepository(string connectionString) : base(connectionString) { }

        public PatientStatementAccountRepository(Database db) : base(db)  { }

        public List<PatientStatementAccount> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            //SqlCommand cmdSelectAcc = new SqlCommand(
            //string.Format("select account_id as [account], * from dbo.patbill_acc  " +
            //"where batch_id = '{0}' " +
            //" and nullif(date_sent,'') is null " +
            //"order by statement_number, record_cnt_acct ", strBatchId)
            //, conn);

            var sql = PetaPoco.Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementAccount.BatchId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });
            sql.Where($"{GetRealColumn(nameof(PatientStatementAccount.DateSent))} is null");

            var results = dbConnection.Fetch<PatientStatementAccount>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;
        }

    }

    public class PatientStatementCernerRepository : RepositoryBase<PatientStatementCerner>
    {
        public PatientStatementCernerRepository(string connectionString) : base (connectionString) { }

        public PatientStatementCernerRepository(Database db) : base(db) { }

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

    public class PatientStatementEncounterRepository : RepositoryBase<PatientStatementEncounter>
    {
        public PatientStatementEncounterRepository(string connectionString) : base(connectionString) { }


        public PatientStatementEncounterRepository(Database db) : base(db) {  }

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
            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounter.StatementNumber))} IN (select statement_number from dbo.pat_bill_acc where batch_id = @0 " + 
                "and nullif(date_sent,'') is null)",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });


            var results = dbConnection.Fetch<PatientStatementEncounter>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;

        }
        
    }

    public class PatientStatementEncounterActivityRepository : RepositoryBase<PatientStatementEncounterActivity>
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
            sql.Where($"{GetRealColumn(nameof(PatientStatementEncounterActivity.StatementNumber))} IN (select statement_number from dbo.pat_bill_acc where batch_id = @0 " +
                "and nullif(date_sent,'') is null)",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = batch });


            var results = dbConnection.Fetch<PatientStatementEncounterActivity>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;

        }
    }

}
