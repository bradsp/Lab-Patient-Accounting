using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LabBilling.Core.Services;
public interface IClaimGeneratorService
{
    string PropProductionEnvironment { get; set; }

    bool ClearBatch(double batch);
    int CompileBillingBatch(ClaimType claimType, IProgress<ProgressReportModel> progress, CancellationToken cancellationToken);
    void CompileClaim(string accountNo);
    ClaimData GenerateClaim(string account, bool reprint = false);
    List<BillingActivity> GetBillingBatchActivity(string batch);
    List<BillingBatch> GetBillingBatches();
    void RegenerateBatch(double batchNo);
}