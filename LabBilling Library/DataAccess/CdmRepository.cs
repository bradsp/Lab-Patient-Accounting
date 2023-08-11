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
    public sealed class CdmRepository : RepositoryBase<Cdm>
    {
        public CdmRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
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
                    new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = 0 });

            sql.Append($"order by {_tableName}.{this.GetRealColumn(nameof(Cdm.Description))}");

            var queryResult = dbConnection.Fetch<Cdm>(sql);

            return queryResult;
        }

        public override bool Save(Cdm table)
        {
            var record = GetCdm(table.ChargeId, true);

            if (record != null)
                return Update(table);
            else
            {
                Add(table);
                return true;
            }
        }

        public override bool Update(Cdm table)
        {
            //update all fee schedules as well

            foreach(var cd in table.CdmFeeSchedule1)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule2)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule3)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule4)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule5)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }


            return base.Update(table);
        }

        public override object Add(Cdm table)
        {
            //add all fee schedules as well
            foreach (var cd in table.CdmFeeSchedule1)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule2)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule3)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule4)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule5)
            {
                AppEnvironment.Context.CdmDetailRepository.Save(cd);
            }

            return base.Add(table);
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
            if (result != null)
            {
                result.CdmDetails = AppEnvironment.Context.CdmDetailRepository.GetByCdm(cdm);

                result.CdmFeeSchedule1 = result.CdmDetails.Where(x => x.FeeSchedule == "1").ToList();
                result.CdmFeeSchedule2 = result.CdmDetails.Where(x => x.FeeSchedule == "2").ToList();
                result.CdmFeeSchedule3 = result.CdmDetails.Where(x => x.FeeSchedule == "3").ToList();
                result.CdmFeeSchedule4 = result.CdmDetails.Where(x => x.FeeSchedule == "4").ToList();
                result.CdmFeeSchedule5 = result.CdmDetails.Where(x => x.FeeSchedule == "5").ToList();
            }

            return result;
        }

        public List<Cdm> GetByCpt(string cptId)
        {
            List<CdmDetail> cdmDetails = new List<CdmDetail>();


            cdmDetails = AppEnvironment.Context.CdmDetailRepository.GetByCpt(cptId);

            List<string> cdms = new List<string>();

            List<string> distinctCdms = cdmDetails.Select(c => c.ChargeItemId).Distinct().ToList();

            if(distinctCdms.Count > 0)
            {
                PetaPoco.Sql cmd = PetaPoco.Sql.Builder;
                cmd.From(_tableName);
                cmd.Where($"{GetRealColumn(nameof(Cdm.ChargeId))} in (@cdms)", new { cdms = distinctCdms });
                List<Cdm> results = dbConnection.Fetch<Cdm>(cmd);

                return results;
            }
            return new List<Cdm>();
        }

    }
}
