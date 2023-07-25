using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("acc")]
    [PrimaryKey("account", AutoIncrement = false)]
    public sealed class Account : IBaseEntity
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
        public List<ChrgDetail> ChargeDetails { get; set;} = new List<ChrgDetail>();
        [Ignore]
        public List<ChrgDetail> BillableCharges { get; set;} = new List<ChrgDetail>();
        [Ignore]
        public List<ClaimChargeView> ClaimCharges { get; set; } = new List<ClaimChargeView>();
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
        public double InvoiceBalance { get; set; }
        [Ignore]
        public double ClaimBalance { get; set; }
        [Ignore]
        public List<(string client, double balance)> ClientBalance { get; set; }


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

        [Ignore]
        public bool ReadyToBill
        {
            get
            {
                if (Status == AccountStatus.ReadyToBill || 
                    Status == AccountStatus.Institutional || 
                    Status == AccountStatus.InstSubmitted || 
                    Status == AccountStatus.Professional || 
                    Status == AccountStatus.ProfSubmitted || 
                    Status == AccountStatus.PaidOut || 
                    Status == AccountStatus.Closed || 
                    Status == AccountStatus.ClaimSubmitted || 
                    Status == AccountStatus.Statements)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                {
                    this.Status = AccountStatus.ReadyToBill;
                }
                else
                {
                    this.Status = AccountStatus.New;
                }
            }
        }

        /// <summary>
        /// Generates full name display from name parts.
        /// </summary>
        [Ignore]
        public string PatNameDisplay
        {
            get
            {
                if (FinCode != "CLIENT")
                {
                    if (string.IsNullOrEmpty(PatNameSuffix))
                    {
                        return $"{PatLastName},{PatFirstName} {PatMiddleName}".TrimEnd();
                    }
                    else
                    {
                        return $"{PatLastName} {PatNameSuffix},{PatFirstName} {PatMiddleName}".TrimEnd();
                    }
                }
                else
                {
                    return PatFullName;
                }
            }
        }

        [Ignore]
        public List<string> Cpt4List
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
                var ins = this.Insurances.Find(x => x.Coverage == "A");
                if (ins != null)
                    return this.Insurances.Find(x => x.Coverage == "A").InsCode;
                else
                    return string.Empty;
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

        [Ignore]
        public AccountAlert AccountAlert { get; set; } = new AccountAlert();

        [Ignore]
        public DateTime LastActivity 
        { 
            get
            {
                List<DateTime> dates = new List<DateTime>
                {
                    mod_date,
                    Pat.mod_date
                };

                if (Charges.Any())
                    dates.Add(Charges.Max(x => x.mod_date));

                if (Insurances.Any())
                    dates.Add(Insurances.Max(x => x.mod_date));

                if (Payments.Any())
                    dates.Add(Payments.Max(x => x.mod_date));

                if (Notes.Any())
                    dates.Add(Notes.Max(x => x.mod_date));

                if (BillingActivities.Any())
                    dates.Add(BillingActivities.Max(x => x.mod_date));

                return dates.Max();
            } 
        }
    }

    public static class AccountStatus
    {
        public static readonly string New = "NEW";
        public static readonly string ReadyToBill = "RTB";
        public static readonly string Professional = "1500";
        public static readonly string Institutional = "UB";
        public static readonly string ProfSubmitted = "SSI1500";
        public static readonly string InstSubmitted = "SSIUB";
        public static readonly string ClaimSubmitted = "CLAIM";
        public static readonly string Statements = "STMT";
        public static readonly string PaidOut = "PAID_OUT";
        public static readonly string Closed = "CLOSED";
        public static readonly string Hold = "HOLD";
        public static readonly string Client = "CLIENT";

        public static bool IsValid(string status)
        {
            if (status == New) return true;
            if (status == ReadyToBill) return true;
            if (status == Professional) return true;
            if (status == Institutional) return true;
            if (status == InstSubmitted) return true;
            if (status == ClaimSubmitted) return true;
            if (status == Statements) return true;
            if (status == ProfSubmitted) return true;
            if (status == PaidOut) return true;
            if (status == Closed) return true;
            if (status == Hold) return true;
            if (status == Client) return true;

            return false;
        }
    }
}