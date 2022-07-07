using System;
using LabBilling.Logging;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class SystemParametersRepository : RepositoryBase<SystemParameters>
    {
        public SystemParametersRepository(string connection) : base("system", connection)
        {
                
        }

        public SystemParametersRepository(string connection, PetaPoco.Database db) : base("system", connection, db)
        {

        }

        public override SystemParameters GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string GetByKey(string key)
        {
            Log.Instance.Debug($"Entering");

            SystemParameters record;

            record = dbConnection.SingleOrDefault<SystemParameters>("where key_name = @0", key);
  
            if(string.IsNullOrEmpty(record.value))
                throw new InvalidParameterValueException("Parameter not defined", key);
  
            return record.value;
        }
    }
}
