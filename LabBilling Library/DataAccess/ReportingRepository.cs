using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public sealed class ReportingRepository
    {
        //private PetaPoco.Database dbConnection;
        private string _connectionString;

        public ReportingRepository(string connectionString)
        {
            Log.Instance.Trace("Entering");
            _connectionString = connectionString;
        }

        public DataTable GetARByFinCode()
        {
            string sql = @"select fin_code as 'Financial Class', sum(ah.balance) as 'Balance'
                    from aging_history ah
                    where ah.datestamp = DATEADD(Day, -1, DATEDIFF(Day, 0, GetDate())) 
                    group by fin_code";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                conn.Open();
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

    }
}
