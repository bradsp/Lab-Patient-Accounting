using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess;

public sealed class BillingBatchRepository : RepositoryBase<BillingBatch>
{

    public BillingBatchRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {
    }

    public BillingBatch GetBatch(double batch)
    {
        var data = Context.SingleOrDefault<BillingBatch>($"where {GetRealColumn(nameof(BillingBatch.Batch))} = @0",
            batch);

        data.BillingActivities = Context.Fetch<BillingActivity>($"where {GetRealColumn(nameof(BillingActivity.Batch))} = @0",
            batch);

        return data;
    }


}
