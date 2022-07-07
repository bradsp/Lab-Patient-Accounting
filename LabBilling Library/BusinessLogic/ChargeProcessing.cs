using System;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;

namespace LabBilling.Core
{
    public class ChargeProcessing
    {
        private AccountRepository accountRepository;
        private ChrgRepository chrgRepository;
        private CdmRepository cdmRepository;
        private FinRepository finRepository;

        public ChargeProcessing(string connectionString)
        {
            accountRepository = new AccountRepository(connectionString);
            chrgRepository = new ChrgRepository(connectionString);
            cdmRepository = new CdmRepository(connectionString);
            finRepository = new FinRepository(connectionString);
        }

        public int CreditCharge(int chrgNum)
        {
            throw new NotImplementedException();

            if(chrgNum <= 0)
            {
                throw new ArgumentException("Charge Number is not greater than zero.", "chrgNum");
            }


        }

        /// <summary>
        /// Add a change to an account. The account must exist.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cdm"></param>
        /// <param name="qty"></param>
        /// <param name="comment"></param>
        /// <returns>Charge number of newly entered charge or < 0 if an error occurs.</returns>
        public int AddCharge(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null)
        {
            //verify the account exists - if not return -1
            Account accData = accountRepository.GetByAccount(account);
            if(accData == null)
            {
                throw new AccountNotFoundException("Account not found.", account);
            }
            //get the cdm number - if cdm number is not found - abort
            Cdm cdmData = cdmRepository.GetCdm(cdm);
            if(cdmData == null)
            {
                throw new CdmNotFoundException("CDM not found.", cdm);
            }

            Fin fin = finRepository.GetFin(accData.fin_code);

            Chrg chrg = new Chrg();

            //split the patient name
            RFClassLibrary.Str.ParseName(accData.pat_name, out string ln, out string fn, out string mn, out string suffix);

            //now build the charge & detail records
            chrg.account = account;
            chrg.action = "";
            chrg.bill_method = fin.form_type;
            chrg.cdm = cdm;
            chrg.comment = comment;
            chrg.credited = false;
            chrg.facility = "";
            chrg.fin_code = accData.fin_code;
            chrg.fin_type = fin.type;
            chrg.fname = fn;
            chrg.lname = ln;
            chrg.mname = mn;
            chrg.mt_mnem = cdmData.mnem;
            chrg.mt_req_no = refNumber;
            chrg.order_site = "";
            chrg.pat_dob = accData.Pat.dob_yyyy;
            chrg.pat_name = accData.pat_name;
            chrg.pat_ssn = accData.Pat.ssn;
            chrg.performing_site = "";
            chrg.post_date = DateTime.Today;
            chrg.qty = qty;
            chrg.service_date = serviceDate;
            chrg.status = "NEW";
            chrg.unitno = accData.HNE_NUMBER;
            chrg.responsiblephy = "";


            //need to determine the correct fee schedule - for now default to 1
            double ztotal = 0.0;
            double amtTotal = 0.0;
            double retailTotal = 0.0;

            foreach(CdmFeeSchedule1 fee in cdmData.cdmFeeSchedule1)
            {
                ChrgDetail amt = new ChrgDetail();
                amt.cpt4 = fee.cpt4;
                amt.type = fee.type;
                switch(fin.type)
                {
                    case "M":
                        amt.amount = fee.mprice;
                        retailTotal += fee.mprice;
                        ztotal += fee.zprice;
                        break;
                    case "C":
                        amt.amount = fee.cprice;
                        retailTotal += fee.cprice;
                        ztotal += fee.zprice;
                        break;
                    case "Z":
                        amt.amount = fee.zprice;
                        retailTotal += fee.zprice;
                        ztotal += fee.zprice;
                        break;
                    default:
                        amt.amount = fee.mprice;
                        retailTotal += fee.mprice;
                        ztotal += fee.zprice;
                        break;
                }

                amtTotal += amt.amount;

                amt.modi = fee.modi;
                amt.revcode = fee.rev_code;
                amt.mt_req_no = "";
                amt.order_code = fee.billcode;
                amt.bill_type = "";
                amt.bill_method = "";
                amt.diagnosis_code_ptr = "1:";

                chrg.ChrgDetails.Add(amt);
            }

            chrg.net_amt = amtTotal;
            chrg.inp_price = ztotal;
            chrg.retail = retailTotal;

            return chrgRepository.AddCharge(chrg);
                        
        }
    }
}
