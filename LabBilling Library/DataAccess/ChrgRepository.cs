using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public sealed class ChrgRepository : RepositoryBase<Chrg>
    {
        private CdmRepository cdmRepository;
        private readonly ChrgDetailRepository amtRepository;
        private const string invoiceCode = "CBILL";
        private const string patientFinType = "M";
        private const string clientFinType = "C";
        private const string zFinType = "Z";

        public ChrgRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            amtRepository = new ChrgDetailRepository(appEnvironment);
            cdmRepository = new CdmRepository(appEnvironment);
        }

        /// <summary>
        /// Get a single charge record by the charge number.
        /// </summary>
        /// <param name="id">Charge number</param>
        /// <returns></returns>
        public Chrg GetById(int id)
        {
            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where($"{GetRealColumn(nameof(Chrg.ChrgId))} = @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = id });

            Chrg chrg = dbConnection.SingleOrDefault<Chrg>(sql);

            //load the cdm record
            CdmRepository cdmRepository = new CdmRepository(_appEnvironment);
            chrg.Cdm = cdmRepository.GetCdm(chrg.CDMCode);
            Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
            return chrg;
        }

        public List<ClaimChargeView> GetClaimCharges(string account)
        {
            Log.Instance.Debug($"Entering - account {account}");

            if(string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            //var sql = PetaPoco.Sql.Builder
            //    .From("vw_chrg_bill")
            //    .Where("account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            var sql = PetaPoco.Sql.Builder;
            sql.From(_tableName);
            sql.Where($"{GetRealColumn(nameof(ChrgDetail.AccountNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
            sql.Where($"{GetRealColumn(nameof(ChrgDetail.FinancialType))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = patientFinType });
            sql.Where($"{GetRealColumn(nameof(ChrgDetail.IsCredited))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = false });
            sql.Where($"{GetRealColumn(nameof(ChrgDetail.Type))} <> @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = ChrgDetailStatus.Invoice });
            sql.Where($"{GetRealColumn(nameof(ChrgDetail.Type))} <> @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = ChrgDetailStatus.NA });

            var results = dbConnection.Fetch<ChrgDetail>(sql);

            CdmRepository cdmRepository = new CdmRepository(_appEnvironment);
            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(_appEnvironment);

            foreach(var chrg in results)
            {
                chrg.RevenueCodeDetail = revenueCodeRepository.GetByCode(chrg.RevenueCode);
                Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
            }

            List<ClaimChargeView> list = new List<ClaimChargeView>();
            
            list = results.GroupBy(c => new {c.BillingCode, c.RevenueCode, c.Cpt4, c.Modifier, c.Modifer2, Amount = c.Quantity * c.Amount })
                .Select(g => new ClaimChargeView { Amount = g.Key.Amount, ChargeId = g.Key.BillingCode, CptCode = g.Key.Cpt4, Modifier = g.Key.Modifier, Modifier2 = g.Key.Modifer2, RevenueCode = g.Key.RevenueCode, Qty = g.Sum(c => c.Quantity) }).ToList();


            return list;

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
                .Where($"{GetRealColumn(nameof(Chrg.AccountNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            if (asOfDate != null)
            {
                sql.Where($"chrg_detail.{GetRealColumn(nameof(ChrgDetail.mod_date))} > @0",
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = asOfDate});
            }

            if (!showCredited)
                sql.Where($"chrg_detail.{GetRealColumn(nameof(ChrgDetail.IsCredited))} = 0");

            if (!includeInvoiced)
                sql.Where($"{GetRealColumn(nameof(ChrgDetail.Invoice))} is null or {GetRealColumn(nameof(ChrgDetail.Invoice))} = ''");

            if (excludeCBill)
                sql.Where($"{GetRealColumn(nameof(Chrg.CDMCode))} <> @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = invoiceCode});

            sql.OrderBy($"{_tableName}.{GetRealColumn(nameof(Chrg.ChrgId))}");

            var result = dbConnection.Fetch<Chrg, ChrgDetail, Chrg>(new ChrgChrgDetailRelator().MapIt, sql);

            CdmRepository cdmRepository = new CdmRepository(_appEnvironment);
            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(_appEnvironment);
            
            foreach(Chrg chrg in result)
            {
                chrg.Cdm = cdmRepository.GetCdm(chrg.CDMCode, true);
                foreach(ChrgDetail detail in chrg.ChrgDetails)
                {
                    detail.RevenueCodeDetail = revenueCodeRepository.GetByCode(detail.RevenueCode);
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
        /// <returns>Charge ID of credit charge record</returns>
        public List<ChrgDetail> CreditCharge(int chrgNum, string comment = "")
        {
            Log.Instance.Trace($"Entering - chrg number {chrgNum} comment {comment}");
            try
            {
                List<ChrgDetail> chrgDetailsPosted = new List<ChrgDetail>();
                if (chrgNum <= 0)
                    throw new ArgumentOutOfRangeException(nameof(chrgNum));
                var chrg = GetById(chrgNum) ?? throw new ApplicationException($"Charge number {chrgNum} not found.");
                var chrgDetails = amtRepository.GetByCharge(chrgNum);

                foreach (var detail in chrgDetails.Where(x => x.IsCredited == false))
                {
                    detail.Quantity *= -1;
                    detail.uri = 0;
                    detail.IsCredited = true;
                    detail.PostedDate = DateTime.Now;
                    chrgDetailsPosted.Add((ChrgDetail)amtRepository.Add(detail));
                }
                SetCredited(chrgNum);

                Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
                return chrgDetailsPosted;
            }
            catch(Exception ex)
            {
                Log.Instance.Error("Error in CreditCharge", ex);
                throw new ApplicationException("Error in CreditCharge", ex);
            }
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

            try
            {
                int chrg_num = Convert.ToInt32(this.Add(chrg));

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

            List<ChrgDetail> chrgs = amtRepository.GetByAccount(account);

            foreach (ChrgDetail chrg in chrgs.Where(x => x.ClientMnem == clientMnem && x.FinancialType == "C"))
            {
                if(chrg.Type != ChrgDetailStatus.Invoice && (chrg.Invoice == "" || chrg.Invoice == null))
                {
                    chrg.Invoice = invoiceNo;

                    try
                    {
                        amtRepository.Update(chrg, new List<string> { nameof(ChrgDetail.Invoice) });
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

        //public bool UpdateDxPointers(IEnumerable<Chrg> chrgs)
        //{
        //    bool returnVal = true;

        //    foreach(var chrg in chrgs)
        //    {
        //        foreach(var detail in chrg.ChrgDetails)
        //        {
        //            returnVal = chrgDiagnosisPointerRepository.Save(detail.DiagnosisPointer) && returnVal;
        //        }
        //    }

        //    return returnVal;
        //}
    }
}
