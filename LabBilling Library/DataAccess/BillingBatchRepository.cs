using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using MCL;

namespace LabBilling.Core.DataAccess
{
    public sealed class BillingBatchRepository : RepositoryBase<BillingBatch>
    {


        public BillingBatchRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public bool ClearBatch(double batch)
        {
            var data = GetBatch(batch);

            try
            {
                BeginTransaction();
                foreach (var detail in data.BillingActivities)
                {
                    var account = AppEnvironment.Context.AccountRepository.GetByAccount(detail.AccountNo);
                    if (account != null)
                    {

                        AppEnvironment.Context.AccountRepository.ClearClaimStatus(account);
                    }
                    // dbh - delete history record
                    dbConnection.Delete(detail);
                }
                // batch - delete batch
                dbConnection.Delete(data);

                CompleteTransaction();
                return true;
            }
            catch(Exception ex)
            {
                AbortTransaction();
                return false;
            }
        }

        public BillingBatch GetBatch(double batch)
        {
            var data = dbConnection.SingleOrDefault<BillingBatch>($"where {GetRealColumn(nameof(BillingBatch.Batch))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.Decimal, Value = batch });

            data.BillingActivities = dbConnection.Fetch<BillingActivity>($"where {GetRealColumn(nameof(BillingActivity.Batch))} = @0",
                new SqlParameter() { SqlDbType = System.Data.SqlDbType.Decimal, Value = batch });

            return data;
        }
    }
}
