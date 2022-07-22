using System;
using System.Collections.Generic;
using PetaPoco;


namespace LabBilling.Core.Models
{
    [TableName("acc")]
    [PrimaryKey("account", AutoIncrement = false)]
    public class Account : IBaseEntity
    {
        public bool deleted { get; set; }
        public string account { get; set; }
        public string meditech_account { get; set; }
        public string HNE_NUMBER { get; set; }
        public string ssn { get; set; }
        public string mri { get; set; }

        public string pat_name { get; set; }
        [Ignore]
        public string pat_name_last { get; set; }
        [Ignore]
        public string pat_name_first { get; set; }
        [Ignore]
        public string pat_name_middle { get; set; }
        [Ignore]
        public string pat_name_suffix { get; set; }

        public string cl_mnem { get; set; }
        public string fin_code { get; set; }
        public string original_fincode { get; set; }

        public DateTime? trans_date { get; set; }
        public DateTime? cbill_date { get; set; }
        public DateTime? post_date { get; set; }
        public DateTime? trans_date_time { get; set; }

        public int bill_priority { get; set; }
        public string oereqno { get; set; }
        public string ov_order_id { get; set; }
        public string ov_pat_id { get; set; }
        public string guarantorID { get; set; }

        public string status { get; set; }
        public int num_comments { get; set; }

        [ResultColumn]
        public DateTime mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

        [Ignore]
        public Pat Pat { get; set; } = new Pat();
        [Ignore]
        public List<Ins> Insurances { get; set; } = new List<Ins>();
        [Ignore]
        public List<Chrg> Charges { get; set; } = new List<Chrg>();
        [Ignore]
        public List<Chk> Payments { get; set; } = new List<Chk>();
        [Ignore]
        public List<AccountNote> Notes { get; set; } = new List<AccountNote>();
        [Ignore]
        public List<BillingActivity> BillingActivities { get; set; } = new List<BillingActivity>();
        [Ignore]
        public AccountValidationStatus AccountValidationStatus { get; set; } = new AccountValidationStatus();
        [Ignore]
        public Guid rowguid { get; set; }

        [Ignore]
        public double Balance { get; set; }
        [Ignore]
        public double TotalCharges { get; set; }
        [Ignore]
        public double TotalPayments { get; set; }
        [Ignore]
        public double TotalContractual { get; set; }
        [Ignore]
        public double TotalWriteOff { get; set; }
        [Ignore]
        public double TotalBadDebt { get; set; }

        [Ignore]
        public string FullInfo => $"{account} {pat_name} {cl_mnem} {trans_date}";
        [Ignore]
        public string ClientName { get; set; }

        [Ignore]
        public List<string> cpt4List
        {
            get
            {
                List<string> cpt4List = new List<string>();
                foreach(var chrg in Charges)
                {
                    foreach(var detail in chrg.ChrgDetails)
                    {
                        cpt4List.Add(detail.cpt4);
                    }
                }
                return cpt4List;
            }
        }

        [Ignore]
        public string PrimaryInsuranceCode
        {
            get
            {
                return this.Insurances.Find(x => x.Coverage == "A").InsCode;
            }
        }
     }

}
