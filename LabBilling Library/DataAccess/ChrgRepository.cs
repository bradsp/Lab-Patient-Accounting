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
            
            return chrg;
        }

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
                chrg.Cdm = cdmRepository.GetCdm(chrg.CDMCode);
                foreach(ChrgDetail detail in chrg.ChrgDetails)
                {
                    detail.RevenueCodeDetail = revenueCodeRepository.GetByCode(detail.RevenueCode);
                }
            }

            return result;
        }

        public int CreditCharge(int chrgNum, string comment = "")
        {
            // usp_prg_ReverseCharge            
            int retVal = dbConnection.ExecuteNonQueryProc("usp_prg_ReverseChargeOnly", 
                new { 
                    parm1 = new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = chrgNum }, 
                    parm2 = new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = comment }
                });

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

                return chrg_num;
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal($"Error in AddCharge - {chrg.AccountNo}", ex);
                throw new ApplicationException("Error in AddCharge", ex);
            }

        }

        public List<InvoiceChargeView> GetInvoiceCharges(string account)
        {
            Log.Instance.Trace($"Entering - {account}");

            var sql = PetaPoco.Sql.Builder
                .From("InvoiceChargeView")
                .Where("WHERE account = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
                .Where("cdm <> 'CBILL'");

            List<InvoiceChargeView> results = dbConnection.Fetch<InvoiceChargeView>(sql);

            return results;

        }

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
                chrgCount = dbConnection.ExecuteNonQueryProc("dbo.usp_prg_ReCharge_Acc_Transaction", new { @account = account });
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                throw new ApplicationException("Error reprocessing charges.", ex);
            }

            Log.Instance.Trace($"Exiting");
            return chrgCount;
        }

    }
}
