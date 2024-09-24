using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;

namespace LabBilling.Core.DataAccess;

public class InvoiceSelectRepository : RepositoryBase<InvoiceSelect>
{

    /*
        SELECT        dbo.chrg.account, dbo.chrg.cl_mnem, dbo.acc.trans_date, dbo.acc.pat_name, dbo.chrg.fin_code
        FROM            dbo.chrg INNER JOIN
                                 dbo.acc ON dbo.chrg.account = dbo.acc.account
        WHERE        (dbo.chrg.status NOT IN ('CBILL', 'N/A')) AND (dbo.chrg.invoice IS NULL OR
                                 dbo.chrg.invoice = '') AND (dbo.chrg.fin_code IN ('APC', 'X', 'Y', 'W', 'Z')) AND (dbo.acc.status NOT LIKE '%HOLD%')
        GROUP BY dbo.chrg.account, dbo.chrg.cl_mnem, dbo.acc.trans_date, dbo.acc.pat_name, dbo.chrg.fin_code
     */

    public InvoiceSelectRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {
    }

    public List<InvoiceSelect> GetByClientAndDate(string clientMnem, DateTime throughDate)
    {

        var selectColumns = new[]
        {
            $"{Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.AccountNo))}",
            $"{Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.ClientMnem))}",
            $"{Account.TableName}.{Account.ColumnName(nameof(Account.TransactionDate))}",
            $"{Account.TableName}.{Account.ColumnName(nameof(Account.PatFullName))}",
            $"{Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.FinCode))}",
        };

        var sql = Sql.Builder
            .Select(selectColumns)
            .From(Chrg.TableName)
            .InnerJoin(Account.TableName)
            .On($"{Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.AccountNo))} = {Account.TableName}.{Account.ColumnName(nameof(Account.AccountNo))}")
            .Where($"{Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.ClientMnem))} = @0",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem })
            .Where($"{Account.TableName}.{Account.ColumnName(nameof(Account.TransactionDate))} <= @0",
            new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = throughDate })
            .Where($"{Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.Status))} NOT IN (@0,@1)",
             new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AppEnvironment.ApplicationParameters.ChargeInvoiceStatus },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AppEnvironment.ApplicationParameters.NotApplicableChargeStatus })
            .Where($"{Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.Invoice))} IS NULL or  {Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.Invoice))} = ''")
            .Where($"{Chrg.TableName}.{Chrg.ColumnName(nameof(Chrg.FinancialType))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AppEnvironment.ApplicationParameters.ClientFinancialTypeCode })
            .Where($"{Account.TableName}.{Account.ColumnName(nameof(Account.Status))} NOT LIKE @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = $"%{AccountStatus.Hold}%" })
            .GroupBy(selectColumns);

        return Context.Fetch<InvoiceSelect>(sql);
    }

}
