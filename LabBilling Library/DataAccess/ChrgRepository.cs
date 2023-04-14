using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;
using NPOI.XSSF.Model;
using System.CodeDom.Compiler;

namespace LabBilling.Core.DataAccess
{
    public class ChrgRepository : RepositoryBase<Chrg>
    {
        private CdmRepository cdmRepository;
        private readonly ChrgDetailRepository amtRepository;
        private readonly ChrgDiagnosisPointerRepository chrgDiagnosisPointerRepository;
        private const string invoiceCode = "CBILL";

        public ChrgRepository(string connection) : base(connection)
        {
            amtRepository = new ChrgDetailRepository(connection);
            cdmRepository = new CdmRepository(connection);
            chrgDiagnosisPointerRepository = new ChrgDiagnosisPointerRepository(connection);
        }

        public ChrgRepository(PetaPoco.Database db) : base(db)
        {
            amtRepository = new ChrgDetailRepository(db);
            cdmRepository = new CdmRepository(db);
            chrgDiagnosisPointerRepository = new ChrgDiagnosisPointerRepository(db);
        }

        /// <summary>
        /// Get a single charge record by the charge number.
        /// </summary>
        /// <param name="id">Charge number</param>
        /// <returns></returns>
        public Chrg GetById(int id)
        {
            var sql = PetaPoco.Sql.Builder
                .Select("chrg.*, cdm.descript as 'cdm_desc', chrg_details.*")
                .From("chrg")
                .LeftJoin("cdm").On("chrg.cdm = cdm.cdm")
                .InnerJoin("chrg_details").On("chrg_details.chrg_num = chrg.chrg_num")
                .Where("chrg.chrg_num = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = id });

            var result = dbConnection.Fetch<Chrg, ChrgDetail, Chrg>(new ChrgChrgDetailRelator().MapIt, sql);

            Chrg chrg = result.First<Chrg>();

            //load the cdm record
            CdmRepository cdmRepository = new CdmRepository(dbConnection);
            chrg.Cdm = cdmRepository.GetCdm(chrg.CDMCode);
            Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
            return chrg;
        }

        public List<ClaimChargeView> GetClaimCharges(string account)
        {
            Log.Instance.Debug($"Entering - account {account}");

            if(string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            var sql = PetaPoco.Sql.Builder
                .From("vw_chrg_bill")
                .Where("account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            var results = dbConnection.Fetch<ClaimChargeView>(sql);

            CdmRepository cdmRepository = new CdmRepository(dbConnection);
            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(dbConnection);

            foreach(var chrg in results)
            {
                chrg.RevenueCodeDetail = revenueCodeRepository.GetByCode(chrg.RevenueCode);
                chrg.Cdm = cdmRepository.GetCdm(chrg.ChargeId, true);
                Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
            }

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

            if(string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            var sql = PetaPoco.Sql.Builder
                .Select("chrg.*, chrg_details.*")
                .From("chrg")
                .InnerJoin("chrg_details").On("chrg_details.chrg_num = chrg.chrg_num")
                .Where("account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            if (asOfDate != null)
            {
                sql.Where($"{_tableName}.{GetRealColumn(nameof(Chrg.mod_date))} > @0",
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = asOfDate});
            }

            if (!showCredited)
                sql.Where($"{GetRealColumn(nameof(Chrg.IsCredited))} = 0");

            if (!includeInvoiced)
                sql.Where($"{GetRealColumn(nameof(Chrg.Invoice))} is null or {GetRealColumn(nameof(Chrg.Invoice))} = ''");

            if (excludeCBill)
                sql.Where($"{GetRealColumn(nameof(Chrg.CDMCode))} <> @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = invoiceCode});

            sql.OrderBy($"{_tableName}.{GetRealColumn(nameof(Chrg.ChrgId))}");

            var result = dbConnection.Fetch<Chrg, ChrgDetail, Chrg>(new ChrgChrgDetailRelator().MapIt, sql);

            CdmRepository cdmRepository = new CdmRepository(dbConnection);
            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(dbConnection);
            
            foreach(Chrg chrg in result)
            {
                chrg.Cdm = cdmRepository.GetCdm(chrg.CDMCode, true);
                foreach(ChrgDetail detail in chrg.ChrgDetails)
                {
                    detail.RevenueCodeDetail = revenueCodeRepository.GetByCode(detail.RevenueCode);
                    detail.DiagnosisPointer = chrgDiagnosisPointerRepository.GetById(detail.uri);
                    Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
                }
            }

            return result;
        }

        /// <summary>
        /// Credit a charge.
        /// </summary>
        /// <param name="chrgNum"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int CreditCharge(int chrgNum, string comment = "")
        {
            Log.Instance.Trace($"Entering - chrg number {chrgNum} comment {comment}");

            if (chrgNum <= 0)
                throw new ArgumentOutOfRangeException(nameof(chrgNum));

            // usp_prg_ReverseCharge            
            int retVal = dbConnection.ExecuteNonQueryProc("usp_prg_ReverseChargeOnly", 
                new SqlParameter() { ParameterName="chrgNum", SqlDbType = SqlDbType.Decimal, Value = chrgNum }, 
                new SqlParameter() { ParameterName="comment", SqlDbType = SqlDbType.VarChar, Value = comment });
            
            Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
            return retVal;
        }

        /// <summary>
        /// Use to add a new charge to an account. This will likely be called from the AccountRepository class.
        /// </summary>
        /// <param name="chrg"></param>
        /// <returns></returns>
        public int AddCharge(Chrg chrg)
        {
            Log.Instance.Trace($"Entering - {chrg.AccountNo}");
            
            if(chrg == null)
                throw new ArgumentNullException(nameof(chrg));

            //function will add charge
            try
            {
                int chrg_num = Convert.ToInt32(this.Add(chrg));

                foreach (ChrgDetail amt in chrg.ChrgDetails)
                {
                    amt.ChrgNo = chrg_num;

                    amtRepository.Add(amt);
                }
                Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
                return chrg_num;
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

            if(string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            if(string.IsNullOrEmpty(clientMnem))
                throw new ArgumentNullException(nameof(clientMnem));

            var sql = PetaPoco.Sql.Builder
                .From("InvoiceChargeView")
                .Where($"{GetRealColumn(typeof(InvoiceChargeView), nameof(InvoiceChargeView.AccountNo))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
                .Where($"{GetRealColumn(typeof(InvoiceChargeView), nameof(InvoiceChargeView.ChargeItemId))} <> @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = invoiceCode })
                .Where($"{GetRealColumn(typeof(InvoiceChargeView), nameof(InvoiceChargeView.ClientMnem))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem })
                .Where($"{GetRealColumn(typeof(InvoiceChargeView), nameof(InvoiceChargeView.FinancialType))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "C" });

            List<InvoiceChargeView> results = dbConnection.Fetch<InvoiceChargeView>(sql);
            Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
            return results;

        }

        public bool SetCredited(int chrgId, bool isCredited = true)
        {
            Log.Instance.Trace($"Entering ChrgId {chrgId}");

            if(chrgId <= 0)
                throw new ArgumentOutOfRangeException(nameof(chrgId));

            try
            {
                var chrg = GetById(chrgId);
                if (chrg != null)
                {
                    chrg.IsCredited = isCredited;
                    return Update(chrg, new List<string> { nameof(Chrg.IsCredited) });
                }
                return false;
            }
            catch(Exception ex)
            {
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

            if(string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            if(string.IsNullOrEmpty(clientMnem))
                throw new ArgumentNullException(nameof(clientMnem));

            if(string.IsNullOrEmpty(invoiceNo))
                throw new ArgumentNullException(nameof(invoiceNo));

            List<Chrg> chrgs = GetByAccount(account, true, false).ToList();

            foreach (Chrg chrg in chrgs.Where(x => x.ClientMnem == clientMnem && x.FinancialType == "C"))
            {
                if(chrg.CDMCode != invoiceCode && (chrg.Invoice == "" || chrg.Invoice == null))
                {
                    chrg.Invoice = invoiceNo;

                    try
                    {
                        Update(chrg, new List<string> { nameof(Chrg.Invoice) });
                        Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
                    }
                    catch(Exception ex)
                    {
                        Log.Instance.Error(ex);
                        throw new ApplicationException("Error in SetChargeInvoiceStatus", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Reprocess all open charges on an account. Usually done as a result of a financial code or client change to reprice charges.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>Number of charges reprocessed.</returns>
        public int ReprocessCharges(string account)
        {
            Log.Instance.Trace($"Entering {account}");
            int chrgCount = 0;

            if(string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            //call stored procedure [dbo].[usp_prg_ReCharge_Acc_Transaction]
            try
            {
                chrgCount = dbConnection.ExecuteNonQueryProc("dbo.usp_prg_ReCharge_Acc_Transaction", 
                    new SqlParameter() { ParameterName = "@acc", SqlDbType = SqlDbType.VarChar, Value = account });
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                throw new ApplicationException("Error reprocessing charges.", ex);
            }

            Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
            return chrgCount;
        }

        public bool UpdateDxPointers(IEnumerable<Chrg> chrgs)
        {
            bool returnVal = true;

            foreach(var chrg in chrgs)
            {
                foreach(var detail in chrg.ChrgDetails)
                {
                    returnVal = chrgDiagnosisPointerRepository.Save(detail.DiagnosisPointer) && returnVal;
                }
            }

            return returnVal;
        }
    }
}
