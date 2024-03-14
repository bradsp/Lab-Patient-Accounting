using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class BillingActivityRepository : RepositoryBase<BillingActivity>
    {
        public BillingActivityRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {
        }

        public List<BillingActivity> GetByAccount(string account)
        {
            Log.Instance.Debug($"Entering");

            var record = Context.Fetch<BillingActivity>($"where {GetRealColumn(nameof(BillingActivity.AccountNo))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            return record;
        }

        public List<BillingActivity> GetByRunDate(DateTime fromDate, DateTime thruDate)
        {
            Log.Instance.Trace("Entering");

            if(fromDate == DateTime.MinValue || thruDate == DateTime.MinValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            if(fromDate > thruDate)
            {
                throw new ArgumentException("fromDate must be less than thruDate");
            }

            return Context.Fetch<BillingActivity>($"where {GetRealColumn(nameof(BillingActivity.RunDate))} between @0 and @1",
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = fromDate },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate });

        }

        public List<BillingActivity> GetBatch(string batch)
        {
            Log.Instance.Debug("Entering");
            var records = Context.Fetch<BillingActivity>($"where {GetRealColumn(nameof(BillingActivity.Batch))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = batch });

            return records;
        }

        public override BillingActivity Save(BillingActivity table)
        {
            Log.Instance.Debug($"Entering");
            var record = Context.SingleOrDefault<BillingActivity>("where account=@0 and run_date = @1", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = table.AccountNo }, 
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = table.RunDate });

            if (record == null)
            {
                return Add(table);
            }
            else
            {
                if (table.InsComplete == DateTime.MinValue)
                    table.InsComplete = null;
                table.rowguid = record.rowguid;
                return Update(table);
            }
        }

        public override BillingActivity Add(BillingActivity table)
        {
            Log.Instance.Debug($"Entering");
            if (table.InsComplete == DateTime.MinValue)
                table.InsComplete = null;

            return base.Add(table);
        }

    }
}
