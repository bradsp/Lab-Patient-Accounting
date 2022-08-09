using System;
using System.Collections.Generic;
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
            var sql = PetaPoco.Sql.Builder
                .From($"{_tableName}")
                .Where($"{this.GetRealColumn(typeof(ChrgDetail), nameof(ChrgDetail.ChrgNo))} = @0", chrg_num);

            var result = dbConnection.Fetch<ChrgDetail>(sql);

            return result;
        }
    }
}
