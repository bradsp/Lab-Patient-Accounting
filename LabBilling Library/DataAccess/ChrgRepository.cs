using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Logging;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class ChrgRepository : RepositoryBase<Chrg>
    {
        private CdmRepository cdmRepository;
        //private FinRepository finRepository;
        private readonly ChrgDetailRepository amtRepository;

        public ChrgRepository(string connection) : base("chrg", connection)
        {
            amtRepository = new ChrgDetailRepository(connection);
            cdmRepository = new CdmRepository(connection);
            //finRepository = new FinRepository(connection);
        }

        public ChrgRepository(string connection, PetaPoco.Database db) : base("chrg", connection, db)
        {
            amtRepository = new ChrgDetailRepository(connection, db);
            cdmRepository = new CdmRepository(connection, db);
            //finRepository = new FinRepository(connection, db);
        }

        public override Chrg GetById(int id)
        {
            var sql = PetaPoco.Sql.Builder
                .Select("chrg.*, cdm.descript as 'cdm_desc', amt.*")
                .From("chrg")
                .LeftJoin("cdm").On("chrg.cdm = cdm.cdm")
                .InnerJoin("amt").On("amt.chrg_num = chrg.chrg_num")
                .Where("chrg.chrg_num = @0", id);

            var result = dbConnection.Fetch<Chrg, ChrgDetail, Chrg>(new ChrgChrgDetailRelator().MapIt, sql);

            Chrg chrg = result.First<Chrg>();

            //load the amt records
            //result.ChrgDetails = amtRepository.GetByCharge(result.chrg_num).ToList();
            
            return chrg;
        }

        public List<Chrg> GetByAccount(string account, bool showCredited = true, bool includeInvoiced = true)
        {
            Log.Instance.Debug($"Entering");

            var sql = PetaPoco.Sql.Builder
                .Select("chrg.*, cdm.descript as 'cdm_desc', amt.*")
                .From("chrg")
                .LeftJoin("cdm").On("chrg.cdm = cdm.cdm")
                .InnerJoin("amt").On("amt.chrg_num = chrg.chrg_num")
                .Where("account = @0", account);
            
            if(!showCredited)
                sql.Where("credited = 0");

            if (!includeInvoiced)
                sql.Where("invoice is null");

            sql.OrderBy("chrg.chrg_num");

            var result = dbConnection.Fetch<Chrg, ChrgDetail, Chrg>(new ChrgChrgDetailRelator().MapIt, sql);

            return result;
        }

        public int CreditCharge(int chrgNum, string comment = "")
        {
            // usp_prg_ReverseCharge            
            int retVal = dbConnection.ExecuteNonQueryProc("usp_prg_ReverseChargeOnly", new { chrgNum, comment });

            return retVal;
        }

        /// <summary>
        /// Use to add a new charge to an account. This will likely be called from the AccountRepository class.
        /// </summary>
        /// <param name="chrg"></param>
        /// <returns></returns>
        public int AddCharge(Chrg chrg)
        {
            Log.Instance.Trace("Entering");
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
                Log.Instance.Fatal("Error in AddCharge", ex);
                throw new ApplicationException("Error in AddCharge", ex);
            }

        }

        public List<InvoiceChargeView> GetInvoiceCharges(string account)
        {
            Log.Instance.Debug($"Entering");

            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * FROM InvoiceChargeView ")
                .Append("WHERE account = @0 and cdm <> 'CBILL'", account);

            List<InvoiceChargeView> results = dbConnection.Fetch<InvoiceChargeView>(sql);

            return results;

        }

        public void SetChargeInvoiceStatus(string account, string invoiceNo)
        {
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
            Log.Instance.Trace($"Entering");
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
