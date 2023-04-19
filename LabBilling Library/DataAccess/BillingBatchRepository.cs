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
        AccountRepository accountRepository;
        PatRepository patRepository;

        public BillingBatchRepository(string connection) : base(connection)
        {
            accountRepository = new AccountRepository(connection);
            patRepository = new PatRepository(connection);
        }

        public BillingBatchRepository(PetaPoco.Database db) : base(db)
        {
            accountRepository = new AccountRepository(db);
            patRepository = new PatRepository(db);
        }

        public bool ClearBatch(double batch)
        {
            var data = GetBatch(batch);

            try
            {
                dbConnection.BeginTransaction();
                foreach (var detail in data.BillingActivities)
                {
                    var account = accountRepository.GetByAccount(detail.AccountNo);
                    if (account != null)
                    {

                        accountRepository.ClearClaimStatus(account);

                        // -- pat - clear h1500_date, ub_date, ssi_batch
                        //List<string> columns = new List<string>();
                        //if (detail.ElectronicBillStatus == "UB")
                        //{
                        //    account.Pat.InstitutionalClaimDate = null;
                        //    columns.Add(nameof(Pat.InstitutionalClaimDate));
                        //}
                        //if (detail.ElectronicBillStatus == "1500")
                        //{
                        //    account.Pat.ProfessionalClaimDate = null;
                        //    columns.Add(nameof(Pat.ProfessionalClaimDate));
                        //}
                        //account.Pat.EBillBatchDate = null;
                        //account.Pat.SSIBatch = null;
                        //columns.Add(nameof(Pat.EBillBatchDate));
                        //columns.Add(nameof(Pat.SSIBatch));
                        //// acc - reset status to form status
                        //account.Status = "NEW";
                        //patRepository.Update(account.Pat, columns);
                        //accountRepository.UpdateStatus(account.AccountNo, "NEW");
                    }
                    // dbh - delete history record
                    dbConnection.Delete(detail);
                }
                // batch - delete batch
                dbConnection.Delete(data);

                dbConnection.CompleteTransaction();
                return true;
            }
            catch(Exception ex)
            {
                dbConnection.AbortTransaction();
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
