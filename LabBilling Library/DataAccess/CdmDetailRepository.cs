using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.CodeDom.Compiler;

namespace LabBilling.Core.DataAccess
{
    public sealed class CdmDetailRepository : RepositoryBase<CdmDetail>
    {
        public CdmDetailRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            
        }

        public List<CdmDetail> GetByCdm(string cdm)
        {
            Log.Instance.Trace("Entering");
            List<CdmDetail> cdmDetails = new List<CdmDetail>();

            cdmDetails = dbConnection.Fetch<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.ChargeItemId))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm});

            return cdmDetails;
        }

        public List<CdmDetail> GetByCdm(string cdm, string feeSched)
        {
            Log.Instance.Trace("Entering");
            List<CdmDetail> cdmDetails = new List<CdmDetail>();

            cdmDetails = dbConnection.Fetch<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.ChargeItemId))} = @0 and {GetRealColumn(nameof(CdmDetail.FeeSchedule))} = @1",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = feeSched});

            return cdmDetails;
        }

        public List<CdmDetail> GetByCpt(string cpt)
        {
            Log.Instance.Trace("Entering");
            List<CdmDetail> cdmDetails = new List<CdmDetail>();

            cdmDetails = dbConnection.Fetch<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.Cpt4))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cpt });

            return cdmDetails;
        }

        public CdmDetail GetByRowguid(Guid rowguid)
        {
            Log.Instance.Trace("Entering");

            CdmDetail cdmDetail;

            cdmDetail = dbConnection.FirstOrDefault<CdmDetail>($"where {GetRealColumn(nameof(CdmDetail.rowguid))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.UniqueIdentifier, Value = rowguid });

            return cdmDetail;
        }

        public override object Add(CdmDetail table)
        {
            return base.Add(table);
        }

        public override bool Update(CdmDetail table)
        {
            return base.Update(table);
        }

        public override bool Save(CdmDetail table)
        {
            var existing = GetByRowguid(table.rowguid);
            if(existing != null)
            {
                return base.Update(table);
            }
            else
            {
                Add(table);
                return true;
            }
        }
    }
}
