using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace LabBilling.Core.DataAccess
{
    public sealed class InsCompanyRepository : RepositoryBase<InsCompany>
    {
        public InsCompanyRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public InsCompany GetByCode(string code)
        {
            Log.Instance.Debug($"Entering");

            if (code == null)
            {
                Log.Instance.Error("Null value passed to InsCompanyRepository GetByCode.");
                return new InsCompany();
            }
            var record = dbConnection.SingleOrDefault<InsCompany>("where code = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = code });

            if(record != null)
            {
                //Str.ParseCityStZip(record.CityStateZip, out string strCity, out string strState, out string strZip);
                //record.City = strCity;
                //record.State = strState;
                //record.Zip = strZip;

                MappingRepository mappingRepository = new MappingRepository(_appEnvironment);

                record.Mappings = mappingRepository.GetMappingsBySendingValue("INS_CODE", record.InsuranceCode).ToList();
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
