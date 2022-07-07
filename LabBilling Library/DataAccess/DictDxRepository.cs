using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.DataAccess
{
    public class DictDxRepository : RepositoryBase<DictDx>
    {
        public DictDxRepository(string connection) : base("icd9desc", connection)
        {

        }

        public DictDxRepository(string connection, PetaPoco.Database db) : base("icd9desc", connection, db)
        {

        }

        public override DictDx GetById(int id)
        {
            throw new NotImplementedException();
        }

        public DictDx GetByCode(string dxCode, DateTime transDate)
        {
            Log.Instance.Debug($"Entering");

            var record = GetByCode(dxCode, FunctionRepository.GetAMAYear(transDate));

            return record;
        }

        public DictDx GetByCode(string dxCode, string AMA_year)
        {
            Log.Instance.Debug($"Entering");

            var record = dbConnection.SingleOrDefault<DictDx>("where icd9_num = @0 and AMA_year = @1", dxCode, AMA_year);

            return record;
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
            Log.Instance.Debug($"Entering");

            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * FROM icd9desc")
                .Append("WHERE (icd9_num = @0", searchText)
                .Append("OR icd9_desc like @0)", "%"+searchText+"%")
                .Append("AND AMA_year = @0", FunctionRepository.GetAMAYear(transDate));

            List<DictDx> records = dbConnection.Fetch<DictDx>(sql);

            return records;
        }
    }
}
