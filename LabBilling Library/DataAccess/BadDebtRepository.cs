using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class BadDebtRepository : RepositoryBase<BadDebt>
    {
        public BadDebtRepository(string connection) : base(connection)
        {

        }

        public BadDebtRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override BadDebt GetById(int id)
        {
            throw new NotImplementedException();
        }

        public BadDebt GetRecord(string rowguid)
        {
            BadDebt badDebt = new BadDebt();

            badDebt = dbConnection.SingleOrDefault<BadDebt>("where rowguid = @0", new SqlParameter() { SqlDbType = SqlDbType.UniqueIdentifier, Value = rowguid });

            badDebt.State = badDebt.StateZip.Substring(0, 2);
            badDebt.Zip = badDebt.StateZip.Substring(3);

            return badDebt;
        }

        public override bool Update(BadDebt table)
        {
            table.StateZip = string.Format("{0} {1}", table.State, table.Zip);

            return base.Update(table);
        }

        public override bool Update(BadDebt table, IEnumerable<string> columns)
        {
            table.StateZip = string.Format("{0} {1}", table.State, table.Zip);

            return base.Update(table, columns);
        }

        public IEnumerable<BadDebt> GetRecords(bool sent)
        {
            List<BadDebt> records = new List<BadDebt>();

            return records;
        }
    }
}
