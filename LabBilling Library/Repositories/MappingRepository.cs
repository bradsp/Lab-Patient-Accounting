using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class MappingRepository : RepositoryBase<Mapping>
    {
        public MappingRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {
                
        }

        public IList<string> GetReturnTypeList()
        {
            Log.Instance.Debug($"Entering");

            var sql = PetaPoco.Sql.Builder
                .Select($"DISTINCT {GetRealColumn(nameof(Mapping.SystemType))}")
                .From(_tableName);

            var queryResult = Context.Fetch<string>(sql);

            return queryResult;
        }

        public IList<string> GetSendingSystemList()
        {
            Log.Instance.Debug($"Entering");

            var sql = PetaPoco.Sql.Builder
                .Select($"DISTINCT {GetRealColumn(nameof(Mapping.InterfaceName))}")
                .From(_tableName);

            var queryResult = Context.Fetch<string>(sql);

            return queryResult;
        }

        public IEnumerable<Mapping> GetMappings(string codeSet, string sendingSystem)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(Mapping.SystemType))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = codeSet })
                .Where($"{GetRealColumn(nameof(Mapping.InterfaceName))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sendingSystem });

            var records = Context.Fetch<Mapping>(sql);

            return records;
        }

        public IEnumerable<Mapping> GetMappingsBySendingValue(string codeSet, string sendingValue)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(Mapping.SystemType))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = codeSet })
                .Where($"{GetRealColumn(nameof(Mapping.SystemKey))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sendingValue });

            var records = Context.Fetch<Mapping>(sql);

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
                .Where($"{GetRealColumn(nameof(Mapping.SystemType))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = codeSet })
                .Where($"{GetRealColumn(nameof(Mapping.InterfaceName))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sendingSystem })
                .Where($"{GetRealColumn(nameof(Mapping.InterfaceAlias))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = sendingValue });

            var record = Context.FirstOrDefault<Mapping>(sql);

            string retVal = string.Empty;
            if(record != null)
                retVal = record.SystemKey;

            return retVal;

        }
    }
}
