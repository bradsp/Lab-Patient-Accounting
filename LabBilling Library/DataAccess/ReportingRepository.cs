﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class ReportingRepository
    {
        private PetaPoco.Database dbConnection;
        private string _connectionString;

        public ReportingRepository(string connectionString)
        {
            Log.Instance.Trace("Entering");
            _connectionString = connectionString;
            //dbConnection = new PetaPoco.Database(connectionString, new CustomSqlDatabaseProvider());
        }

        public DataTable GetARByFinCode()
        {
            //string sql = @";with cteAccBal
            //        as
            //        (
            //            select acc.account, acc.fin_code, dbo.GetAccBalByDate(acc.account, getdate()) as Balance
            //            from acc
            //            where status not in ('PAID_OUT', 'CLOSED')
            //        )
            //        select fin_code as 'Financial Class', sum(Balance) as 'Balance'
            //        from cteAccBal
            //        group by fin_code
            //        order by fin_code";

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