using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;

namespace LabBilling.Core.DataAccess
{
    public class InsCompanyRepository : RepositoryBase<InsCompany>
    {
        public InsCompanyRepository(string connection) : base(connection)
        {

        }

        public InsCompanyRepository(PetaPoco.Database db) : base(db)
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

            if(record != null)
            {
                Str.ParseCityStZip(record.citystzip, out string strCity, out string strState, out string strZip);
                record.City = strCity;
                record.State = strState;
                record.Zip = strZip;
            }

            return record;
        }

        public override List<InsCompany> GetAll()
        {
            return GetAll(true);
        }

        public List<InsCompany> GetAll(bool excludeDeleted)
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
