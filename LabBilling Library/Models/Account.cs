using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("acc")]
    [PrimaryKey("account", AutoIncrement = false)]
    public class Account : IBaseEntity
    {
        [Column("deleted")]
        public bool IsDeleted { get; set; }
        [Column("account")]
        public string AccountNo { get; set; }
        [Column("meditech_account")]
        public string MeditechAccount { get; set; }
        [Column("HNE_NUMBER")]
        public string EMPINumber { get; set; }
        [Column("ssn")]
        public string SocSecNo { get; set; }
        [Column("mri")]
        public string MRN { get; set; }
        [Column("birthdate")]
        public DateTime? BirthDate { get; set; }
        [Column("sex")]
        public string Sex { get; set; }

        /// <summary>
        /// Patient Full name as stored in database. This will be deprecated.
        /// </summary>
        [Column("pat_name")]
        public string PatFullName { get; set; }
        [Column("last_name")]
        public string PatLastName { get; set; }
        [Column("first_name")]
        public string PatFirstName { get; set; }
        [Column("middle_name")]
        public string PatMiddleName { get; set; }
        [Column("name_suffix")]
        public string PatNameSuffix { get; set; }

        [Column("cl_mnem")]
        public string ClientMnem { get; set; }
        [Column("fin_code")]
        public string FinCode { get; set; }
        [Column("original_fincode")]
        public string OriginalFinCode { get; set; }

        [Column("trans_date")]
        public DateTime TransactionDate { get; set; }
        [Column("cbill_date")]
        public DateTime? ClientBillDate { get; set; }
        [Column("post_date")]
        public DateTime? PostingDate { get; set; }
        [Column("trans_date_time")]
        public DateTime? TransactionDateTime { get; set; }

        [Column("bill_priority")]
        public int BillPriority { get; set; }
        [Column("oereqno")]
        public string OEReqNo { get; set; }
        [Column("ov_order_id")]
        public string OVOrderId { get; set; }
        [Column("ov_pat_id")]
        public string OVPatId { get; set; }
        [Column("guarantorID")]
        public string GuarantorId { get; set; }

        [Column("status")]
        public string Status { get; set; }
        [Column("num_comments")]
        public int CommentCount { get; set; }

        [ResultColumn]
        [Column("mod_date")]
        public DateTime mod_date { get; set; }
        [ResultColumn]
        [Column("mod_date")]
        public string mod_user { get; set; }
        [ResultColumn]
        [Column("mod_prg")]
        public string mod_prg { get; set; }
        [ResultColumn]
        [Column("mod_host")]
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
        public Client Client { get; set; }
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
        public string FullInfo => $"{AccountNo} {PatFullName} {ClientMnem} {TransactionDate}";
        [Ignore]
        public string ClientName { get; set; }
        [Ignore]
        public string DOBSex => $"{BirthDate.GetValueOrDefault().ToShortDateString()} - {Sex}";



        /// <summary>
        /// Generates full name display from name parts.
        /// </summary>
        [Ignore]
        public string PatNameDisplay
        {
            get
            {
                if(string.IsNullOrEmpty(PatNameSuffix))
                {
                    return $"{PatLastName},{PatFirstName} {PatMiddleName}".TrimEnd();
                }
                else
                {
                    return $"{PatLastName} {PatNameSuffix},{PatFirstName} {PatMiddleName}".TrimEnd();
                }
            }
        }

        [Ignore]
        public List<string> cpt4List
        {
            get
            {
                List<string> cpt4List = new List<string>();
                foreach (var chrg in Charges.Where(c => c.IsCredited == false))
                {
                    foreach (var detail in chrg.ChrgDetails)
                    {
                        cpt4List.Add(detail.Cpt4);
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

        [Ignore]
        public Ins InsurancePrimary
        {
            get
            {
                return Insurances.Where(i => i.Coverage == "A").FirstOrDefault();
            }
        }
        [Ignore]
        public Ins InsuranceSecondary
        {
            get
            {
                return Insurances.Where(i => i.Coverage == "B").FirstOrDefault();
            }
        }
        [Ignore]
        public Ins InsuranceTertiary
        {
            get
            {
                return Insurances.Where(i => i.Coverage == "C").FirstOrDefault();
            }
        }

        [Ignore]
        public List<string> LmrpErrors { get; set; } = new List<string>();

        [Ignore]
        public Fin Fin { get; set; } = new Fin();

        [Ignore]
        public string BillForm { get; set; }
        [Ignore]
        public string BillingType { get; set; }
        [Ignore]
        public bool SentToCollections 
        { 
            get
            {
                if (this.Pat.SentToCollectionsDate != null)
                    return true;
                else
                    return false;
            }
        }
 
    }

}