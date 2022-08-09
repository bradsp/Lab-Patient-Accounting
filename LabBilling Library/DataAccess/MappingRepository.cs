using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Logging;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class MappingRepository : RepositoryBase<Mapping>
    {
        public MappingRepository(string connection) : base(connection)
        {
                
        }

        public MappingRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override Mapping GetById(int id)
        {
            throw new NotImplementedException();
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
            Log.Instance.Trace("$Entering");

            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * ")
                .Append($"FROM {_tableName} ")
                .Append("WHERE return_value_type = @0 ", codeSet)
                .Append("AND sending_system = @0 ", sendingSystem);

            var records = dbConnection.Fetch<Mapping>(sql);

            return records;
        }

        public string GetMappedValue(string codeSet, string sendingSystem, string sendingValue)
        {
            Log.Instance.Trace("$Entering");

            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * ")
                .Append($"FROM {_tableName} ")
                .Append("WHERE return_value_type = @0 ", codeSet)
                .Append("AND sending_system = @0 ", sendingSystem)
                .Append("AND sending_value = @0 ", sendingValue);

            var record = dbConnection.SingleOrDefault<Mapping>(sql);

            string retVal = record.return_value;

            return retVal;

        }
    }
}
