using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Logging;
using LabBilling.Models;

namespace LabBilling.DataAccess
{
    public class ChrgRepository : RepositoryBase<Chrg>
    {
        private readonly AmtRepository amtRepository;

        public ChrgRepository(string connection) : base("chrg", connection)
        {
            amtRepository = new AmtRepository(connection);
        }

        public override Chrg GetById(int id)
        {
            
            var sql = PetaPoco.Sql.Builder
                .Append("SELECT chrg.*, cdm.descript as 'cdm_desc' ")
                .Append("FROM chrg left outer join cdm on chrg.cdm = cdm.cdm ")
                .Append("WHERE chrg_num = @0", id);

            var result = dbConnection.SingleOrDefault<Chrg>(sql);

            //load the amt records
            result.ChrgDetails = amtRepository.GetByCharge(result.chrg_num).ToList();
            
            return result;
        }

        public IEnumerable<Chrg> GetByAccount(string account, bool showCredited = true, bool includeInvoiced = true)
        {
            Log.Instance.Debug($"Entering");

            var sql = PetaPoco.Sql.Builder
                .Append("SELECT chrg.*, cdm.descript as 'cdm_desc' ")
                .Append("FROM chrg left outer join cdm on chrg.cdm = cdm.cdm ")
                .Append("WHERE account = @0 ", account);
            
            if(!showCredited)
                sql.Append("AND credited = 0 ");

            if (!includeInvoiced)
                sql.Append("AND invoice is null");

            sql.Append("order by chrg_num");

            List<Chrg> records = dbConnection.Fetch<Chrg>(sql);

            return records;
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
            //function will add charge
            int chrg_num = Convert.ToInt32(this.Add(chrg));
            
            foreach(Amt amt in chrg.ChrgDetails)
            {
                amt.chrg_num = chrg_num;

                amtRepository.Add(amt);
            }

            return chrg_num;
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
                if(chrg.cdm != "CBILL" && (chrg.invoice == "" || chrg.invoice == null))
                {
                    //chrg.status = "CBILL";
                    chrg.invoice = invoiceNo;

                    Update(chrg);

                }
            }

        }
    }
}
