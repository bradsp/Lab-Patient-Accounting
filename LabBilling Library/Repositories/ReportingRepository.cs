using LabBilling.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace LabBilling.Core.DataAccess;

public sealed class ReportingRepository
{
    readonly IAppEnvironment _appEnvironment;
    public ReportingRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context)
    {
        Log.Instance.Trace("Entering");
        _appEnvironment = appEnvironment;
    }

    public DataTable GetARByFinCode()
    {
        // TODO Phase 23-03: SQL text is T-SQL (DATEADD/DATEDIFF/GetDate, quoted aliases). Rewrite to PG dialect.
        string sql = @"select fin_code as 'Financial Class', sum(ah.balance) as 'Balance'
                    from aging_history ah
                    where ah.datestamp = DATEADD(Day, -1, DATEDIFF(Day, 0, GetDate()))
                    group by fin_code";

        using (NpgsqlConnection conn = new NpgsqlConnection(_appEnvironment.ConnectionString))
        {
            NpgsqlCommand cmd = new(sql, conn);
            NpgsqlDataAdapter da = new()
            {
                SelectCommand = cmd
            };
            conn.Open();
            DataTable dt = new();
            da.Fill(dt);
            return dt;
        }
    }

    public List<(string FinancialClass, double Balance)> GetARByFinCodeList()
    {
        // TODO Phase 23-03: SQL text is T-SQL (DATEADD/DATEDIFF/GetDate, quoted aliases). Rewrite to PG dialect.
        string sql = @"select fin_code as 'Financial Class', sum(ah.balance) as 'Balance'
                    from aging_history ah
                    where ah.datestamp = DATEADD(Day, -1, DATEDIFF(Day, 0, GetDate()))
                    group by fin_code
                    order by fin_code";

        using NpgsqlConnection conn = new(_appEnvironment.ConnectionString);
        NpgsqlCommand cmd = new(sql, conn);
        NpgsqlDataAdapter da = new();
        da.SelectCommand = cmd;
        conn.Open();
        DataTable dt = new();
        da.Fill(dt);

        List<(string, double)> list = new();
        foreach (DataRow dr in dt.Rows)
        {
            list.Add((dr["Financial Class"].ToString(), Convert.ToDouble(dr["Balance"].ToString())));
        }

        return list;
    }

}
