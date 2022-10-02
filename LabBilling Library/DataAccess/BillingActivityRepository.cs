using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

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

            var record = dbConnection.Fetch<BillingActivity>("where account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            return record;
        }

        public override bool Save(BillingActivity table)
        {
            Log.Instance.Debug($"Entering");
            var record = dbConnection.SingleOrDefault<BillingActivity>("where account=@0 and run_date = @1", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = table.AccountNo }, 
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = table.RunDate });

            if (record == null)
            {
                Add(table);
                return true;
            }
            else
            {
                if (table.InsComplete == DateTime.MinValue)
                    table.InsComplete = null;
                table.rowguid = record.rowguid;
                return Update(table);
            }
        }

        public override BillingActivity GetById(int id)
        {
            Log.Instance.Debug($"Entering");
            throw new NotImplementedException();
        }

        public override object Add(BillingActivity table)
        {
            Log.Instance.Debug($"Entering");
            if (table.InsComplete == DateTime.MinValue)
                table.InsComplete = null;

            return base.Add(table);
        }

    }
}
