using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class BillingBatchRepository : RepositoryBase<BillingBatch>
    {
        public BillingBatchRepository(string connection) : base(connection)
        {

        }

        public BillingBatchRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override BillingBatch GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
