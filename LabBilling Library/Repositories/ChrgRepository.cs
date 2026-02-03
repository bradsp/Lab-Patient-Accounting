using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LabBilling.Core.DataAccess;

public sealed class ChrgRepository : RepositoryBase<Chrg>
{
    public ChrgRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {
    }

    /// <summary>
    /// Get a single charge record by the charge number.
    /// </summary>
    /// <param name="id">Charge number</param>
    /// <returns></returns>
    public Chrg GetById(int id)
    {
        return Context.SingleOrDefault<Chrg>((object)id);
    }

    public List<ClaimChargeView> GetClaimCharges(string account)
    {
        Log.Instance.Debug($"Entering - account {account}");

        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));

        var sql = PetaPoco.Sql.Builder
            .From("vw_chrg_bill")
            .Where("account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

        var results = Context.Fetch<ClaimChargeView>(sql);

        return results;

    }


    /// <summary>
    /// Get charge records for an account.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="showCredited">True to include credited charges. False to include only active charges.</param>
    /// <param name="includeInvoiced">True to include all charges. False to exclude charges that have been on client invoice.</param>
    /// <param name="asOfDate"></param>
    /// <param name="excludeCBill">True to exclude the Invoice transfer charge record.</param>
    /// <returns></returns>
    public List<Chrg> GetByAccount(string account, bool showCredited = true, bool includeInvoiced = true, DateTime? asOfDate = null, bool excludeCBill = true)
    {
        Log.Instance.Debug($"Entering - account {account}");

        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));

        var sql = PetaPoco.Sql.Builder
            .Where("account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

        sql.Where($"(@0 IS NULL OR {_tableName}.{GetRealColumn(nameof(Chrg.UpdatedDate))} > @0)", asOfDate);

        sql.Where($"(@0 = 1 OR {GetRealColumn(nameof(Chrg.IsCredited))} = 0)",
            new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = showCredited });

        sql.Where($"(@0 = 1 OR {GetRealColumn(nameof(Chrg.Invoice))} is null OR {GetRealColumn(nameof(Chrg.Invoice))} = '')",
            new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = includeInvoiced });

        sql.Where($"(@0 = 0 OR {GetRealColumn(nameof(Chrg.CDMCode))} <> @1)",
            new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = excludeCBill },
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AppEnvironment.ApplicationParameters.ClientInvoiceCdm });

        sql.OrderBy($"{_tableName}.{GetRealColumn(nameof(Chrg.ChrgId))}");

        var result = Context.Fetch<Chrg>(sql);

        return result;
    }

    public List<Chrg> GetChargeByReferenceAndCdm(string refNumber, string cdm)
    {
        Log.Instance.Trace($"Entering refNumber {refNumber}, cdm {cdm}");

        if(string.IsNullOrEmpty(refNumber))
            throw new ArgumentNullException(nameof(refNumber));
        if (string.IsNullOrEmpty(cdm))
            throw new ArgumentNullException(nameof(cdm));

        var sql = PetaPoco.Sql.Builder
            .Where($"{GetRealColumn(nameof(Chrg.LISReqNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = refNumber })
            .Where($"{GetRealColumn(nameof(Chrg.Cdm))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm })
            .Where($"{GetRealColumn(nameof(Chrg.IsCredited))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = false });

        return Context.Fetch<Chrg>(sql);
    }

    /// <summary>
    /// Use to add a new charge to an account.
    /// </summary>
    /// <param name="chrg"></param>
    /// <returns></returns>
    public override Chrg Add(Chrg chrg)
    {
        Log.Instance.Trace($"Entering - {chrg.AccountNo}");

        if (chrg == null)
            throw new ArgumentNullException(nameof(chrg));

        try
        {
            chrg = base.Add(chrg);
            Log.Instance.Debug($"{Context.LastSQL} {Context.GetArgs()}");
            return chrg;
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal($"Error in AddCharge - {chrg.AccountNo}", ex);
            throw new ApplicationException("Error in AddCharge", ex);
        }
    }

    /// <summary>
    /// Gets charges to be included on a client invoice.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public List<InvoiceChargeView> GetInvoiceCharges(string account, string clientMnem)
    {
        Log.Instance.Trace($"Entering - {account}");

        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));

        if (string.IsNullOrEmpty(clientMnem))
            throw new ArgumentNullException(nameof(clientMnem));

        var sql = PetaPoco.Sql.Builder
            .From("InvoiceChargeView")
            .Where($"{GetRealColumn(typeof(InvoiceChargeView), nameof(InvoiceChargeView.AccountNo))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
            .Where($"{GetRealColumn(typeof(InvoiceChargeView), nameof(InvoiceChargeView.ChargeItemId))} <> @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AppEnvironment.ApplicationParameters.ClientInvoiceCdm })
            .Where($"{GetRealColumn(typeof(InvoiceChargeView), nameof(InvoiceChargeView.ClientMnem))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem })
            .Where($"{GetRealColumn(typeof(InvoiceChargeView), nameof(InvoiceChargeView.FinancialType))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "C" });

        List<InvoiceChargeView> results = Context.Fetch<InvoiceChargeView>(sql);
        Log.Instance.Debug($"{Context.LastSQL} {Context.GetArgs()}");
        return results;

    }

    public Chrg SetCredited(Chrg chrg, bool isCredited = true)
    {
        Log.Instance.Trace($"Entering ChrgId {chrg.ChrgId}");

        if (chrg.ChrgId <= 0)
            throw new ArgumentOutOfRangeException(nameof(Chrg.ChrgId));

        try
        {
            chrg.IsCredited = isCredited;
            return Update(chrg, new List<string> { nameof(Chrg.IsCredited) });
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error setting credited on charge {chrg.ChrgId}", ex);
            throw new ApplicationException($"Error setting credited on charge {chrg.ChrgId}", ex);
        }
    }

    public Chrg SetCredited(int chrgId, bool isCredited = true)
    {
        Log.Instance.Trace($"Entering ChrgId {chrgId}");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(chrgId);

        try
        {
            var chrg = GetById(chrgId);
            if (chrg.ChrgId <= 0)
                throw new ApplicationException($"Charge not found for chrgId {chrgId}");

            return SetCredited(chrg, isCredited);
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error setting credited on charge {chrgId}", ex);
            throw new ApplicationException($"Error setting credited on charge {chrgId}", ex);
        }

    }

    /// <summary>
    /// Sets the invoice number on a charge
    /// </summary>
    /// <param name="account"></param>
    /// <param name="invoiceNo"></param>
    /// <exception cref="ApplicationException"></exception>
    public void SetChargeInvoiceStatus(string account, string clientMnem, string invoiceNo)
    {
        Log.Instance.Trace($"Entering - account {account} invoice {invoiceNo}");

        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));

        if (string.IsNullOrEmpty(clientMnem))
            throw new ArgumentNullException(nameof(clientMnem));

        if (string.IsNullOrEmpty(invoiceNo))
            throw new ArgumentNullException(nameof(invoiceNo));

        List<Chrg> chrgs = GetByAccount(account, true, false).ToList();

        foreach (Chrg chrg in chrgs.Where(x => x.ClientMnem == clientMnem && x.FinancialType == "C"))
        {
            if (chrg.CDMCode != AppEnvironment.ApplicationParameters.ClientInvoiceCdm && (chrg.Invoice == "" || chrg.Invoice == null))
            {
                chrg.Invoice = invoiceNo;

                try
                {
                    Update(chrg, new List<string> { nameof(Chrg.Invoice) });
                    Log.Instance.Debug($"{Context.LastSQL} {Context.GetArgs()}");
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    throw new ApplicationException("Error in SetChargeInvoiceStatus", ex);
                }
            }
        }
    }
}
