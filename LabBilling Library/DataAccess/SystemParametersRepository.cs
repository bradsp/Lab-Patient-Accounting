using System;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class SystemParametersRepository : RepositoryBase<SysParameter>
    {
        public SystemParametersRepository(string connection) : base(connection)
        {
                
        }

        public SystemParametersRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override SysParameter GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string GetProductionEnvironment()
        {
            string env = GetByKey("dbenvironment");
            if (env == "Production")
                return "P";
            else
                return "T";
        }

        public string GetByKey(string key)
        {
            Log.Instance.Debug($"Entering");

            SysParameter record;

            record = dbConnection.SingleOrDefault<SysParameter>("where key_name = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = key });
  
            if(string.IsNullOrEmpty(record.Value))
                throw new InvalidParameterValueException("Parameter not defined", key);
  
            return record.Value;
        }
    }
}
