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
        public BillingActivityRepository(string connection) : base("vw_billing_activity", connection)
        {

        }

        public IEnumerable<BillingActivity> GetByAccount(string account)
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
