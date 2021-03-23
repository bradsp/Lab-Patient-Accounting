using System;
using System.Collections.Generic;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class AmtRepository : RepositoryBase<ChrgDetails>
    {
        public AmtRepository(string connection) : base("amt", connection)
        {

        }

        public override ChrgDetails GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChrgDetails> GetByCharge(int chrg_num)
        {
            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * ")
                .Append("FROM amt ")
                .Append("WHERE chrg_num = @0", chrg_num);

            var result = dbConnection.Fetch<ChrgDetails>(sql);

            return result;
        }
    }
}
