using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class ChrgRepository : RepositoryBase<Chrg>
    {
        private CdmRepository cdmRepository;
        private readonly ChrgDetailRepository amtRepository;

        public ChrgRepository(string connection) : base(connection)
        {
            amtRepository = new ChrgDetailRepository(connection);
            cdmRepository = new CdmRepository(connection);
        }

        public ChrgRepository(PetaPoco.Database db) : base(db)
        {
            amtRepository = new ChrgDetailRepository(db);
            cdmRepository = new CdmRepository(db);
        }

        /// <summary>
        /// Get a single charge record by the charge number.
        /// </summary>
        /// <param name="id">Charge number</param>
        /// <returns></returns>
        public override Chrg GetById(int id)
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

        /// <summary>
        /// Get charge records for an account.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="showCredited">True to include credited charges. False to include only active charges.</param>
        /// <param name="includeInvoiced"></param>
        /// <returns></returns>
        public List<Chrg> GetByAccount(string account, bool showCredited = true, bool includeInvoiced = true)
        {
            Log.Instance.Debug($"Entering - account {account}");

            var sql = PetaPoco.Sql.Builder
                .Select("chrg.*, chrg_details.*")
                .From("chrg")
                .InnerJoin("chrg_details").On("chrg_details.chrg_num = chrg.chrg_num")
                .Where("account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
            
            if(!showCredited)
                sql.Where("credited = 0");

            if (!includeInvoiced)
                sql.Where("invoice is null");

            sql.OrderBy("chrg.chrg_num");

            var result = dbConnection.Fetch<Chrg, ChrgDetail, Chrg>(new ChrgChrgDetailRelator().MapIt, sql);

            CdmRepository cdmRepository = new CdmRepository(dbConnection);
            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(dbConnection);
            
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
        /// <returns></returns>
        public int CreditCharge(int chrgNum, string comment = "")
        {
            Log.Instance.Trace($"Entering - chrg number {chrgNum} comment {comment}");
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
        public List<InvoiceChargeView> GetInvoiceCharges(string account)
        {
            Log.Instance.Trace($"Entering - {account}");

            var sql = PetaPoco.Sql.Builder
                .From("InvoiceChargeView")
                .Where("WHERE account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
                .Where("cdm <> 'CBILL'");

            List<InvoiceChargeView> results = dbConnection.Fetch<InvoiceChargeView>(sql);
            Log.Instance.Debug($"{dbConnection.LastSQL} {dbConnection.LastArgs}");
            return results;

        }

        /// <summary>
        /// Sets the invoice number on a charge
        /// </summary>
        /// <param name="account"></param>
        /// <param name="invoiceNo"></param>
        /// <exception cref="ApplicationException"></exception>
        public void SetChargeInvoiceStatus(string account, string invoiceNo)
        {
            Log.Instance.Trace($"Entering - account {account} invoice {invoiceNo}");
            List<Chrg> chrgs = GetByAccount(account, true, false).ToList();

            foreach (Chrg chrg in chrgs)
            {
                if(chrg.CDMCode != "CBILL" && (chrg.Invoice == "" || chrg.Invoice == null))
                {
                    //chrg.status = "CBILL";
                    chrg.Invoice = invoiceNo;

                    try
                    {
                        Update(chrg);
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

    }
}
