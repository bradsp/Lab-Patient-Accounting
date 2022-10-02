using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class ChrgDetailRepository : RepositoryBase<ChrgDetail>
    {
        public ChrgDetailRepository(string connection) : base(connection)
        {

        }

        public ChrgDetailRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override ChrgDetail GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChrgDetail> GetByCharge(int chrg_num)
        {
            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(dbConnection);
            var sql = PetaPoco.Sql.Builder
                .From($"{_tableName}")
                .Where($"{this.GetRealColumn(typeof(ChrgDetail), nameof(ChrgDetail.ChrgNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = chrg_num });

            var results = dbConnection.Fetch<ChrgDetail>(sql);

            foreach(var result in results)
            {
                result.RevenueCodeDetail = revenueCodeRepository.GetByCode(result.RevenueCode);
            }

            return results;
        }
    }
}
