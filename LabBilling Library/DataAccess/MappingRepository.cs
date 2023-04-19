using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public sealed class MappingRepository : RepositoryBase<Mapping>
    {
        public MappingRepository(string connection) : base(connection)
        {
                
        }

        public MappingRepository(PetaPoco.Database db) : base(db)
        {

        }

        public IList<string> GetReturnTypeList()
        {
            Log.Instance.Debug($"Entering");

            string sql = null;
            sql = $"select DISTINCT return_value_type from {_tableName}";

            var queryResult = dbConnection.Fetch<string>(sql);

            return queryResult;
        }

        public IList<string> GetSendingSystemList()
        {
            Log.Instance.Debug($"Entering");

            string sql = null;
            sql = $"select DISTINCT sending_system from {_tableName}";

            var queryResult = dbConnection.Fetch<string>(sql);

            return queryResult;

        }

        public IEnumerable<Mapping> GetMappings(string codeSet, string sendingSystem)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Where("return_value_type = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = codeSet })
                .Where("sending_system = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sendingSystem });

            var records = dbConnection.Fetch<Mapping>(sql);

            return records;
        }

        public string GetMappedValue(string codeSet, string sendingSystem, string sendingValue)
        {
            Log.Instance.Trace("Entering");

            if (codeSet == null)
                throw new ArgumentNullException(codeSet);
            if (sendingSystem == null)
                throw new ArgumentNullException(sendingSystem);
            if (sendingValue == null)
                throw new ArgumentNullException(sendingValue);

            if (string.IsNullOrEmpty(codeSet))
                throw new ArgumentOutOfRangeException(codeSet);
            if (string.IsNullOrEmpty(sendingSystem))
                throw new ArgumentOutOfRangeException(sendingSystem);
            if (string.IsNullOrEmpty(sendingValue))
                throw new ArgumentOutOfRangeException(sendingValue);

            var sql = PetaPoco.Sql.Builder
                .Where("return_value_type = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = codeSet })
                .Where("sending_system = @0 ", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sendingSystem })
                .Where("sending_value = @0 ", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sendingValue });

            var record = dbConnection.FirstOrDefault<Mapping>(sql);

            string retVal = string.Empty;
            if(record != null)
                retVal = record.return_value;

            return retVal;

        }
    }
}
