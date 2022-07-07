using System;
using System.Collections.Generic;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class AmtRepository : RepositoryBase<ChrgDetail>
    {
        public AmtRepository(string connection) : base("amt", connection)
        {

        }

        public AmtRepository(string connection, PetaPoco.Database db) : base("amt", connection, db)
        {

        }

        public override ChrgDetail GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChrgDetail> GetByCharge(int chrg_num)
        {
            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * ")
                .Append("FROM amt ")
                .Append("WHERE chrg_num = @0", chrg_num);

            var result = dbConnection.Fetch<ChrgDetail>(sql);

            return result;
        }
    }
}
