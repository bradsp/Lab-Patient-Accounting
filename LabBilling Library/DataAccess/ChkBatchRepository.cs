using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class ChkBatchRepository : RepositoryBase<ChkBatch>
    {
        public ChkBatchRepository(string connection) : base("chk_batch", connection)
        {

        }

        public override ChkBatch GetById(int id)
        {
            return dbConnection.SingleOrDefault<ChkBatch>(id);
        }
    }
}
