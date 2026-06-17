using LabBilling.Core.Models;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace LabBilling.Core.DataAccess;

public sealed class CdmDetailRepository : RepositoryBase<CdmDetail>
{
    public CdmDetailRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {

    }

    public List<CdmDetail> GetByCdm(string cdm)
    {
        Log.Instance.Trace("Entering");
        List<CdmDetail> cdmDetails = new List<CdmDetail>();

        cdmDetails = Context.Fetch<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.ChargeItemId))} = @0",
            cdm);

        return cdmDetails;
    }

    public List<CdmDetail> GetByCdm(string cdm, string feeSched)
    {
        Log.Instance.Trace("Entering");
        List<CdmDetail> cdmDetails;

        cdmDetails = Context.Fetch<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.ChargeItemId))} = @0 and {GetRealColumn(nameof(CdmDetail.FeeSchedule))} = @1",
            cdm,
            feeSched);

        return cdmDetails;
    }

    public List<CdmDetail> GetByCpt(string cpt)
    {
        Log.Instance.Trace("Entering");
        List<CdmDetail> cdmDetails;

        cdmDetails = Context.Fetch<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.Cpt4))} = @0",
            cpt);

        return cdmDetails;
    }

    public CdmDetail GetByRowguid(Guid rowguid)
    {
        Log.Instance.Trace("Entering");

        CdmDetail cdmDetail;

        cdmDetail = Context.FirstOrDefault<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.rowguid))} = @0",
            rowguid);

        return cdmDetail;
    }

    public override CdmDetail Add(CdmDetail table)
    {
        return base.Add(table);
    }

    public int Delete(string cdm)
    {
        try
        {
            var result = Context.Delete<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.ChargeItemId))} = @0",
                cdm);

            return result;
        }
        catch (Exception ex)
        {
            return -1;
        }
    }

    public override CdmDetail Save(CdmDetail table)
    {
        var existing = GetByRowguid(table.rowguid);
        if (existing != null)
        {
            return base.Update(table);
        }
        else
        {

            return Add(table);
        }
    }
}
