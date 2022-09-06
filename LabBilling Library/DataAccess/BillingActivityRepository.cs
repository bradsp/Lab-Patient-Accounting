using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class BillingActivityRepository : RepositoryBase<BillingActivity>
    {
        public BillingActivityRepository(string connection) : base(connection)
        {
        }

        public BillingActivityRepository(PetaPoco.Database db) : base(db)
        {
        }

        public List<BillingActivity> GetByAccount(string account)
        {
            Log.Instance.Debug($"Entering");

            var record = dbConnection.Fetch<BillingActivity>("where account = @0", account);

            return record;
        }

        public override BillingActivity GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
