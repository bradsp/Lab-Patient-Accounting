using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class InvoiceHistoryRepository : RepositoryBase<InvoiceHistory>
    {
        public InvoiceHistoryRepository(string connection) : base(connection)
        {

        }

        public InvoiceHistoryRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override InvoiceHistory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvoiceHistory> GetWithSort(string clientMnem = null, DateTime? fromDate = null, DateTime? throughDate = null)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Append($"SELECT {_tableName}.*, client.cli_nme as 'ClientName' ")
                .Append($"FROM {_tableName} left outer join client on {_tableName}.cl_mnem = client.cli_mnem ");

            if (clientMnem != null || fromDate != null || throughDate != null)
            {
                sql.Append("WHERE ");

                if (clientMnem != null)
                    sql.Append($"cl_mnem = '{clientMnem}'");

                if (fromDate != null && throughDate != null)
                    sql.Append($"{_tableName}.mod_date between '{fromDate}' and '{throughDate}'");
            }
            sql.Append($"order by {_tableName}.mod_date DESC");

            return dbConnection.Fetch<InvoiceHistory>(sql);
        }
        
    }
}
