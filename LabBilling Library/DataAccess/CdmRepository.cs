using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.DataAccess
{
    public class CdmRepository : RepositoryBase<Cdm>
    {
        public CdmRepository(string connection) : base(connection)
        {

        }

        public CdmRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override IEnumerable<Cdm> GetAll()
        {
            return GetAll(false);
        }

        public IEnumerable<Cdm> GetAll(bool includeDeleted = false)
        {
            Log.Instance.Debug($"Entering");

            Sql sql = PetaPoco.Sql.Builder
                .Select("*")
                .From(_tableName);

            if (includeDeleted == false)
                sql.Append($"where {this.GetRealColumn(typeof(Cdm), nameof(Cdm.IsDeleted))} = 0");

            sql.Append($"order by {_tableName}.{this.GetRealColumn(typeof(Cdm), nameof(Cdm.Description))}");

            var queryResult = dbConnection.Fetch<Cdm>(sql);

            return queryResult;
        }

        public override Cdm GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Cdm GetCdm(string cdm)
        {
            string cdmRealName = this.GetRealColumn(typeof(Cdm), nameof(Cdm.ChargeId));
            string isDeletedRealName = this.GetRealColumn(typeof(Cdm), nameof(Cdm.ChargeId));

            var result = dbConnection.SingleOrDefault<Cdm>($"where {cdmRealName} = @0", cdm);
            if(result != null)
            {
                result.CdmFeeSchedule1 = dbConnection.Fetch<CdmFeeSchedule1>($"where {cdmRealName} = @0 and {isDeletedRealName} = 0", cdm);
                result.CdmFeeSchedule2 = dbConnection.Fetch<CdmFeeSchedule2>($"where {cdmRealName} = @0 and {isDeletedRealName} = 0", cdm);
                result.CdmFeeSchedule3 = dbConnection.Fetch<CdmFeeSchedule3>($"where {cdmRealName} = @0 and {isDeletedRealName} = 0", cdm);
                result.CdmFeeSchedule4 = dbConnection.Fetch<CdmFeeSchedule4>($"where {cdmRealName} = @0 and {isDeletedRealName} = 0", cdm);
                result.CdmFeeSchedule5 = dbConnection.Fetch<CdmFeeSchedule5>($"where {cdmRealName} = @0 and {isDeletedRealName} = 0", cdm);
            }

            return result;
        }
    }
}
