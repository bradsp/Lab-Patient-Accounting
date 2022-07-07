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
        public BillingBatchRepository(string connection) : base("data_billing_batch", connection)
        {

        }

        public BillingBatchRepository(string connection, PetaPoco.Database db) : base("data_billing_batch", connection, db)
        {

        }

        public override BillingBatch GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
