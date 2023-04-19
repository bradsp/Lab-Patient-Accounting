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
            CdmDetailRepository cdmDetailRepository = new CdmDetailRepository(dbConnection);

            foreach(var cd in table.CdmFeeSchedule1)
            {
                cdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule2)
            {
                cdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule3)
            {
                cdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule4)
            {
                cdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule5)
            {
                cdmDetailRepository.Save(cd);
            }


            return base.Update(table);
        }

        public override object Add(Cdm table)
        {
            CdmDetailRepository cdmDetailRepository = new CdmDetailRepository(dbConnection);
            //add all fee schedules as well
            foreach (var cd in table.CdmFeeSchedule1)
            {
                cdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule2)
            {
                cdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule3)
            {
                cdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule4)
            {
                cdmDetailRepository.Save(cd);
            }
            foreach (var cd in table.CdmFeeSchedule5)
            {
                cdmDetailRepository.Save(cd);
            }

            return base.Add(table);
        }

        public Cdm GetCdm(string cdm, bool includeDeleted = false)
        {
            CdmDetailRepository cdmDetailRepository = new CdmDetailRepository(dbConnection);

            string cdmRealName = this.GetRealColumn(nameof(Cdm.ChargeId));
            string isDeletedRealName = this.GetRealColumn(nameof(Cdm.IsDeleted));

            var cmd = PetaPoco.Sql.Builder;
            cmd.Where($"{cdmRealName} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });

            if (!includeDeleted)
                cmd.Where($"{isDeletedRealName} = 0");

            var result = dbConnection.SingleOrDefault<Cdm>(cmd);
            if (result != null)
            {
                result.CdmFeeSchedule1 = cdmDetailRepository.GetByCdm(cdm, "1");
                result.CdmFeeSchedule2 = cdmDetailRepository.GetByCdm(cdm, "2");
                result.CdmFeeSchedule3 = cdmDetailRepository.GetByCdm(cdm, "3");
                result.CdmFeeSchedule4 = cdmDetailRepository.GetByCdm(cdm, "4");
                result.CdmFeeSchedule5 = cdmDetailRepository.GetByCdm(cdm, "5");
            }

            return result;
        }

        public List<Cdm> GetByCpt(string cptId)
        {
            List<CdmDetail> cdmDetails = new List<CdmDetail>();

            CdmDetailRepository cdmDetailRepository = new CdmDetailRepository(dbConnection);

            cdmDetails = cdmDetailRepository.GetByCpt(cptId);

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
