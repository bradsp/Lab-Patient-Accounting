using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.CodeDom.Compiler;

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
                sql.Where($"{this.GetRealColumn(nameof(Cdm.IsDeleted))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = 0});

            sql.Append($"order by {_tableName}.{this.GetRealColumn(nameof(Cdm.Description))}");

            var queryResult = dbConnection.Fetch<Cdm>(sql);

            return queryResult;
        }

        public override bool Save(Cdm table)
        {
            var record = GetCdm(table.ChargeId);

            if (record != null)
            {
                return Update(table);
            }
            else
            {
                Add(table);
                return true;
            }
        }

        public Cdm GetCdm(string cdm, bool includeDeleted = false)
        {
            string cdmRealName = this.GetRealColumn(nameof(Cdm.ChargeId));
            string isDeletedRealName = this.GetRealColumn(nameof(Cdm.IsDeleted));

            var cmd = PetaPoco.Sql.Builder;
            cmd.Where($"{cdmRealName} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });

            if (!includeDeleted)
                cmd.Where($"{isDeletedRealName} = 0");

            var result = dbConnection.SingleOrDefault<Cdm>(cmd);
            if(result != null)
            {
                string cdmColName = this.GetRealColumn(typeof(CdmDetail), nameof(CdmDetail.ChargeItemId));
                string isDeletedColName = this.GetRealColumn(typeof(CdmDetail), nameof(CdmDetail.IsDeleted));
                result.CdmFeeSchedule1 = dbConnection.Fetch<CdmFeeSchedule1>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm }).ToList<ICdmDetail>();
                result.CdmFeeSchedule2 = dbConnection.Fetch<CdmFeeSchedule2>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm }).ToList<ICdmDetail>();
                result.CdmFeeSchedule3 = dbConnection.Fetch<CdmFeeSchedule3>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm }).ToList<ICdmDetail>();
                result.CdmFeeSchedule4 = dbConnection.Fetch<CdmFeeSchedule4>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm }).ToList<ICdmDetail>();
                result.CdmFeeSchedule5 = dbConnection.Fetch<CdmFeeSchedule5>($"where {cdmColName} = @0 and {isDeletedColName} = 0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm }).ToList<ICdmDetail>();
            }

            return result;
        }

        public List<Cdm> GetByCpt(string cptId)
        {
            List<CdmFeeSchedule1> cdmDetails = new List<CdmFeeSchedule1>();

            var cmd = PetaPoco.Sql.Builder;
            cmd.From("cpt4");
            cmd.Where($"{GetRealColumn(typeof(CdmFeeSchedule1), nameof(CdmFeeSchedule1.Cpt4))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar,SqlValue = cptId });

            cdmDetails = dbConnection.Fetch<CdmFeeSchedule1>(cmd);

            List<string> cdms = new List<string>();

            List<string> distinctCdms = cdmDetails.Select(c => c.BillCode).ToList();

            if(distinctCdms.Count > 0)
            {
                cmd = PetaPoco.Sql.Builder;
                cmd.From(_tableName);
                cmd.Where($"{GetRealColumn(nameof(Cdm.ChargeId))} in (@cdms)", new { cdms = distinctCdms });
                List<Cdm> results = dbConnection.Fetch<Cdm>(cmd);

                return results;
            }
            return new List<Cdm>();
        }

    }
}
