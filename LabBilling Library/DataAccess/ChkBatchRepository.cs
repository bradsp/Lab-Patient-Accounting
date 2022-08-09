using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class ChkBatchRepository : RepositoryBase<ChkBatch>
    {
        public ChkBatchRepository(string connection) : base(connection)
        {

        }

        public ChkBatchRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override ChkBatch GetById(int id)
        {
            return dbConnection.SingleOrDefault<ChkBatch>(id);
        }
    }
}
