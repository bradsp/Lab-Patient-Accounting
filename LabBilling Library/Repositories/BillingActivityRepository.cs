using LabBilling.Core.Models;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace LabBilling.Core.DataAccess;

public sealed class BillingActivityRepository : RepositoryBase<BillingActivity>
{
    public BillingActivityRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {
    }

    public List<BillingActivity> GetByAccount(string account)
    {
        Log.Instance.Debug($"Entering");

        var record = Context.Fetch<BillingActivity>($"where {GetRealColumn(nameof(BillingActivity.AccountNo))} = @0",
            account);

        return record;
    }

    public List<BillingActivity> GetByRunDate(DateTime fromDate, DateTime thruDate)
    {
        Log.Instance.Trace("Entering");

        if (fromDate == DateTime.MinValue || thruDate == DateTime.MinValue)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (fromDate > thruDate)
        {
            throw new ArgumentException("fromDate must be less than thruDate");
        }

        return Context.Fetch<BillingActivity>($"where {GetRealColumn(nameof(BillingActivity.RunDate))} between @0 and @1",
            fromDate,
            thruDate);

    }

    public List<BillingActivity> GetBatch(string batch)
    {
        Log.Instance.Debug("Entering");
        var records = Context.Fetch<BillingActivity>($"where {GetRealColumn(nameof(BillingActivity.Batch))} = @0",
            batch);

        return records;
    }

    public override BillingActivity Save(BillingActivity table)
    {
        Log.Instance.Debug($"Entering");
        var record = Context.SingleOrDefault<BillingActivity>("where account=@0 and run_date = @1",
            table.AccountNo,
            table.RunDate);

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
