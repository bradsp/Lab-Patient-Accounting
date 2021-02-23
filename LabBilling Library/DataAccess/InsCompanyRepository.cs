using LabBilling.Logging;
using LabBilling.Models;
using System;
using System.Collections.Generic;

namespace LabBilling.DataAccess
{
    public class InsCompanyRepository : RepositoryBase<InsCompany>
    {
        public InsCompanyRepository(string connection) : base("insc", connection)
        {

        }

        public override InsCompany GetById(int id)
        {
            throw new NotImplementedException();
        }

        public InsCompany GetByCode(string code)
        {
            Log.Instance.Debug($"Entering");

            var record = dbConnection.SingleOrDefault<InsCompany>("where code = @0", code);

            return record;
        }

        public override IEnumerable<InsCompany> GetAll()
        {
            return GetAll(true);
        }

        public IEnumerable<InsCompany> GetAll(bool excludeDeleted)
        {
            Log.Instance.Debug($"Entering");

            string sql = null;
            if(excludeDeleted)
                sql = $"SELECT * FROM {_tableName} where deleted = 0";
            else
                sql = $"SELECT * FROM {_tableName}";

            var queryResult = dbConnection.Fetch<InsCompany>(sql);

            return queryResult;
        }
    }
}
