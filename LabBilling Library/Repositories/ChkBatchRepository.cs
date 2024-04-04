using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace LabBilling.Core.DataAccess;

public sealed class ChkBatchRepository : RepositoryBase<ChkBatch>
{
    public ChkBatchRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {
    }

    public ChkBatch GetById(int id)
    {
        Log.Instance.Trace("Entering");
        var batch = Context.SingleOrDefault<ChkBatch>((object)id);
        return batch;
    }

    public int UpdatePostedDate(int id, DateTime postedDate)
    {
        Log.Instance.Trace("Entering");

        return Context.Update<ChkBatch>($"set {GetRealColumn(nameof(ChkBatch.PostedDate))} = @0 where {GetRealColumn(nameof(ChkBatch.BatchNo))} = @1",
            new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = postedDate },
            new SqlParameter() { SqlDbType = SqlDbType.Int, Value = id });

    }

    public List<ChkBatch> GetOpenBatches()
    {
        Log.Instance.Trace("Entering");
        return Context.Fetch<ChkBatch>($"where {GetRealColumn(nameof(ChkBatch.PostedDate))} is null");
    }

}
