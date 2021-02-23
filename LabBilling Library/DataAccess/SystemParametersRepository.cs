using System;
using LabBilling.Logging;
using LabBilling.Models;

namespace LabBilling.DataAccess
{
    public class SystemParametersRepository : RepositoryBase<SystemParameters>
    {
        public SystemParametersRepository(string connection) : base("system", connection)
        {
                
        }

        public override SystemParameters GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string GetByKey(string key)
        {
            Log.Instance.Debug($"Entering");

            var record = dbConnection.SingleOrDefault<SystemParameters>("where key_name = @0", key);
                       
            return record.value;
        }
    }
}
