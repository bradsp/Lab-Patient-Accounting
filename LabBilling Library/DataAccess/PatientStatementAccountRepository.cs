using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatientStatementAccountRepository : RepositoryBase<PatientStatementAccount>
    {
        public PatientStatementAccountRepository(IAppEnvironment appEnvironment) : base(appEnvironment) { }

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

        public List<PatientStatementAccount> GetByAccount(string account)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementAccount.AccountId))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = account });

            var results = dbConnection.Fetch<PatientStatementAccount>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return results;

        }

    }
}
