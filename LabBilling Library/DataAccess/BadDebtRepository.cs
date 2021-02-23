using LabBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.DataAccess
{
    public class BadDebtRepository : RepositoryBase<BadDebt>
    {
        public BadDebtRepository(string connection) : base("bad_debt", connection)
        {

        }

        public override BadDebt GetById(int id)
        {
            throw new NotImplementedException();
        }

        public BadDebt GetRecord(string rowguid)
        {
            BadDebt badDebt = new BadDebt();

            badDebt = dbConnection.SingleOrDefault<BadDebt>("where rowguid = @0", rowguid);

            badDebt.State = badDebt.state_zip.Substring(0, 2);
            badDebt.Zip = badDebt.state_zip.Substring(3);

            return badDebt;
        }

        public override bool Update(BadDebt table)
        {
            table.state_zip = string.Format("{0} {1}", table.State, table.Zip);

            return base.Update(table);
        }

        public IEnumerable<BadDebt> GetRecords(bool sent)
        {
            List<BadDebt> records = new List<BadDebt>();

            return records;
        }
    }
}
