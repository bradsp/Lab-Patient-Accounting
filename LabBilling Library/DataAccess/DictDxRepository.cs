using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;

namespace LabBilling.Core.DataAccess
{
    public sealed class DictDxRepository : RepositoryBase<DictDx>
    {
        public DictDxRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public DictDx GetByCode(string dxCode, DateTime transDate)
        {
            Log.Instance.Trace($"Entering");

            var record = GetByCode(dxCode, FunctionRepository.GetAMAYear(transDate));

            return record;
        }

        public DictDx GetByCode(string dxCode, string AMA_year)
        {
            Log.Instance.Trace($"Entering");

            var record = dbConnection.SingleOrDefault<DictDx>($"where {GetRealColumn(nameof(DictDx.DxCode))} = @0 and {GetRealColumn(nameof(DictDx.AmaYear))} = @1", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = dxCode },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AMA_year });

            return record;
        }

        /// <summary>
        /// Returns count of codes for an AMA year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool AMAYearExists(string year)
        {
            Log.Instance.Trace("Entering");

            var records = dbConnection.Exists<DictDx>($"where {GetRealColumn(nameof(DictDx.AmaYear))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = year });

            return records;
        }

        /// <summary>
        /// Query diagnosis records from the database based on provided search text. Function searches for an exact match on the diagnosis code
        /// OR a LIKE search on description.
        /// </summary>
        /// <param name="searchText">Text to be queried</param>
        /// <param name="transDate">Date of service of account to determine the correct AMA year code set.</param>
        /// <returns></returns>
        public IEnumerable<DictDx> Search(string searchText, DateTime transDate)
        {
            Log.Instance.Trace($"Entering - searchtext {searchText} date {transDate}");

            var sql = PetaPoco.Sql.Builder
                .Append($"WHERE ({GetRealColumn(nameof(DictDx.DxCode))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = searchText })
                .Append($"OR {GetRealColumn(nameof(DictDx.Description))} like @0)", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "%" + searchText + "%" })
                .Append($"AND {GetRealColumn(nameof(DictDx.AmaYear))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = FunctionRepository.GetAMAYear(transDate) });

            List<DictDx> records = dbConnection.Fetch<DictDx>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);
            return records;
        }
    }
}
