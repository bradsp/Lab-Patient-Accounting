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

            return result;
        }
    }
}
