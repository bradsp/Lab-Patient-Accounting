using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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

        public InvoiceHistory GetByInvoice(string invoiceNo)
        {
            return dbConnection.SingleOrDefault<InvoiceHistory>($"where {GetRealColumn(nameof(InvoiceHistory.InvoiceNo))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = invoiceNo });
        }

        public IEnumerable<InvoiceHistory> GetWithSort(string clientMnem = null, DateTime? fromDate = null, DateTime? throughDate = null, string invoice = null)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Select($"{_tableName}.*, client.cli_nme as 'ClientName'")
                .From(_tableName)
                .LeftJoin("client").On($"{_tableName}.cl_mnem = client.cli_mnem");

            if(clientMnem != null)
            {
                sql.Where($"cl_mnem = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem });
            }

            if (fromDate != null || throughDate != null)
            {
                if (fromDate != null && throughDate != null)
                    sql.Where($"{_tableName}.mod_date between @0 and @1",
                        new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = fromDate},
                        new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = throughDate});
            }
            if(!string.IsNullOrEmpty(invoice))
            {
                sql.Where($"{GetRealColumn(nameof(InvoiceHistory.InvoiceNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = invoice });
            }
            sql.OrderBy($"{_tableName}.mod_date DESC");
            Log.Instance.Debug(sql);
            return dbConnection.Fetch<InvoiceHistory>(sql);
        }
        
    }
}
