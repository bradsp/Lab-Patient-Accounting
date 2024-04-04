using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
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
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });

        return cdmDetails;
    }

    public List<CdmDetail> GetByCdm(string cdm, string feeSched)
    {
        Log.Instance.Trace("Entering");
        List<CdmDetail> cdmDetails = new List<CdmDetail>();

        cdmDetails = Context.Fetch<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.ChargeItemId))} = @0 and {GetRealColumn(nameof(CdmDetail.FeeSchedule))} = @1",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm },
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = feeSched });

        return cdmDetails;
    }

    public List<CdmDetail> GetByCpt(string cpt)
    {
        Log.Instance.Trace("Entering");
        List<CdmDetail> cdmDetails = new List<CdmDetail>();

        cdmDetails = Context.Fetch<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.Cpt4))} = @0",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cpt });

        return cdmDetails;
    }

    public CdmDetail GetByRowguid(Guid rowguid)
    {
        Log.Instance.Trace("Entering");

        CdmDetail cdmDetail;

        cdmDetail = Context.FirstOrDefault<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.rowguid))} = @0",
            new SqlParameter() { SqlDbType = SqlDbType.UniqueIdentifier, Value = rowguid });

        return cdmDetail;
    }

    public override CdmDetail Add(CdmDetail table)
    {
        return base.Add(table);
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
