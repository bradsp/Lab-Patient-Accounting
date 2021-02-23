using System;
using System.Collections.Generic;
using LabBilling.Models;

namespace LabBilling.DataAccess
{
    public class AmtRepository : RepositoryBase<Amt>
    {
        public AmtRepository(string connection) : base("amt", connection)
        {

        }

        public override Amt GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Amt> GetByCharge(int chrg_num)
        {
            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * ")
                .Append("FROM amt ")
                .Append("WHERE chrg_num = @0", chrg_num);

            var result = dbConnection.Fetch<Amt>(sql);

            return result;
        }
    }
}
