using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

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

        public override List<Cdm> GetAll()
        {
            return GetAll(false);
        }

        public List<Cdm> GetAll(bool includeDeleted = false)
        {
            Log.Instance.Debug($"Entering");
            Sql sql = PetaPoco.Sql.Builder
                .Select("*")
                .From(_tableName);

            if (includeDeleted == false)
                sql.Where($"{this.GetRealColumn(typeof(Cdm), nameof(Cdm.IsDeleted))} = 0");

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
            string isDeletedRealName = this.GetRealColumn(typeof(Cdm), nameof(Cdm.IsDeleted));

            var result = dbConnection.SingleOrDefault<Cdm>($"where {cdmRealName} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });
            if(result != null)
            {
                string cdmColName = this.GetRealColumn(typeof(CdmFeeSchedule1), nameof(CdmFeeSchedule1.ChargeItemId));
                string isDeletedColName = this.GetRealColumn(typeof(CdmFeeSchedule1), nameof(CdmFeeSchedule1.IsDeleted));
                result.CdmFeeSchedule1 = dbConnection.Fetch<CdmFeeSchedule1>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });
                result.CdmFeeSchedule2 = dbConnection.Fetch<CdmFeeSchedule2>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });
                result.CdmFeeSchedule3 = dbConnection.Fetch<CdmFeeSchedule3>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });
                result.CdmFeeSchedule4 = dbConnection.Fetch<CdmFeeSchedule4>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });
                result.CdmFeeSchedule5 = dbConnection.Fetch<CdmFeeSchedule5>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });
            }

            return result;
        }
    }
}
