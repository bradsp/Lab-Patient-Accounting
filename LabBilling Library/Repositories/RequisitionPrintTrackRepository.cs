using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using System;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess;

/// <summary>
/// Repository for managing requisition form print tracking
/// </summary>
public sealed class RequisitionPrintTrackRepository : RepositoryBase<RequisitionPrintTrack>
{
    public RequisitionPrintTrackRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context)
        : base(appEnvironment, context)
    {
    }

    /// <summary>
    /// Adds a new print tracking record
    /// </summary>
    /// <param name="model">The print track record to add</param>
    /// <returns>The added record with generated Uri</returns>
    public override RequisitionPrintTrack Add(RequisitionPrintTrack model)
    {
        Log.Instance.Trace($"Adding print track record for client {model.ClientName}, form {model.FormPrinted}");

        // Set default values
        model.ModDate = DateTime.Now;
        model.ModHost = Environment.MachineName;

        return base.Add(model);
    }

    /// <summary>
    /// Adds a print tracking record asynchronously
    /// </summary>
    public async Task<RequisitionPrintTrack> AddAsync(RequisitionPrintTrack model)
    {
        return await Task.Run(() => Add(model));
    }
}
