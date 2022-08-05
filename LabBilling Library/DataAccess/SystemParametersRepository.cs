using System;
using LabBilling.Logging;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class SystemParametersRepository : RepositoryBase<SysParameter>
    {
        public SystemParametersRepository(string connection) : base("system", connection)
        {
                
        }

        public SystemParametersRepository(string connection, PetaPoco.Database db) : base("system", connection, db)
        {

        }

        public override SysParameter GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string GetByKey(string key)
        {
            Log.Instance.Debug($"Entering");

            SysParameter record;

            record = dbConnection.SingleOrDefault<SysParameter>("where key_name = @0", key);
  
            if(string.IsNullOrEmpty(record.Value))
                throw new InvalidParameterValueException("Parameter not defined", key);
  
            return record.Value;
        }
    }
}
