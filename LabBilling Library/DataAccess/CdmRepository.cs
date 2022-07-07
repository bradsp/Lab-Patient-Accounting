using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.DataAccess
{
    public class CdmRepository : RepositoryBase<Cdm>
    {
        public CdmRepository(string connection) : base("cdm", connection)
        {

        }

        public CdmRepository(string connection, PetaPoco.Database db) : base("cdm", connection, db)
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
                sql.Append("where deleted = 0");

            sql.Append("order by cdm.descript");

            var queryResult = dbConnection.Fetch<Cdm>(sql);

            return queryResult;
        }

        public override Cdm GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Cdm GetCdm(string cdm)
        {
            var result = dbConnection.SingleOrDefault<Cdm>("where cdm = @0", cdm);
            if(result != null)
            {
                result.cdmFeeSchedule1 = dbConnection.Fetch<CdmFeeSchedule1>("where cdm = @0 and deleted = 0", cdm);
                result.cdmFeeSchedule2 = dbConnection.Fetch<CdmFeeSchedule2>("where cdm = @0 and deleted = 0", cdm);
                result.cdmFeeSchedule3 = dbConnection.Fetch<CdmFeeSchedule3>("where cdm = @0 and deleted = 0", cdm);
                result.cdmFeeSchedule4 = dbConnection.Fetch<CdmFeeSchedule4>("where cdm = @0 and deleted = 0", cdm);
                result.cdmFeeSchedule5 = dbConnection.Fetch<CdmFeeSchedule5>("where cdm = @0 and deleted = 0", cdm);
            }

            return result;
        }
    }
}
