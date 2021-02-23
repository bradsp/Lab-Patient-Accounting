using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Models;

namespace LabBilling.DataAccess
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
