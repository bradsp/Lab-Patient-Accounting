using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabBilling.Core.Models;


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

    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }

    [Ignore]
    public Pat Pat { get; set; } = new Pat();
    [Ignore]
    public List<Ins> Insurances { get; set; } = new List<Ins>();
    [Ignore]
    public List<Chrg> Charges { get; set; } = new List<Chrg>();
    [Ignore]
    public List<Chrg> BillableCharges { get => this.Charges.Where(x => x.Status != "CBILL" && x.Status != "CAP" && x.Status != "N/A").ToList(); }
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
    public List<PatientStatementAccount> PatientStatements { get; set; }
    [Ignore]
    public List<ChrgDiagnosisPointer> ChrgDiagnosisPointers { get; set; }

    [Ignore]
    public List<Cdm> Cdms
    {
        get
        {
            List<Cdm> cdms = new();
            Log.Instance.Debug($"Compiling distinct list of CDMs on account {this.AccountNo}");
            Charges.Where(x => x.IsCredited == false).ToList().ForEach(c =>
            {
                Log.Instance.Debug($"CDM {c.CDMCode}, iteration {cdms.Count}");
                if (cdms.Count > 0)
                {
                    if (!cdms.Any(d => d.ChargeId == c.CDMCode) && c.Cdm != null)
                    {
                        cdms.Add(c.Cdm);
                    }
                }
                else
                {
                    cdms.Add(c.Cdm);
                }
            });
            return cdms;
        }
    }

    [Ignore]
    public Client Client { get; set; }

    public Guid rowguid { get; set; }

    [Ignore]
    public double InvoiceBalance { get; set; }
    [Ignore]
    public double ClaimBalance
    {
        get
        {
            if (this.FinCode == "CLIENT")
            {
                return 0.00;
            }
            else
            {
                return this.BillableCharges.Where(x => x.FinancialType == "M").Sum(x => x.Quantity * x.NetAmount)
                - (this.TotalPayments + this.TotalWriteOff + this.TotalContractual);
            }
        }
    }
    [Ignore]
    public List<(string client, double balance)> ClientBalance
    {
        get
        {
            if (this.FinCode == "CLIENT")
            {
                var balance = this.BillableCharges.Sum(y => y.Quantity * y.NetAmount)
                 - (this.TotalPayments + this.TotalContractual + this.TotalWriteOff);
                var list = new List<(string client, double balance)>
                {
                    (this.ClientMnem, balance)
                };
                return list;
            }
            else
            {
                var list = new List<(string client, double balance)>();
                var results = this.BillableCharges
                    .Where(x => x.FinancialType == "C")
                    .GroupBy(x => x.ClientMnem, (client, balance) => new { Client = client, Balance = balance.Sum(c => c.Quantity * c.NetAmount) });
                foreach (var result in results)
                    list.Add((result.Client, result.Balance));
                return list;
            }
        }
    }


    [Ignore]
    public double Balance { get => this.TotalCharges - (this.TotalPayments + this.TotalContractual + this.TotalWriteOff); }
    [Ignore]
    public double TotalCharges
    {
        get
        {
            if (this.FinCode == "CLIENT")
            {
                return this.Charges.Where(x => x.Status != "CBILL").Sum(x => x.Quantity * x.NetAmount);
            }
            else
            {
                return this.Charges.Where(x => x.Status != "CBILL" && x.Status != "CAP" && x.Status != "N/A")
                    .Sum(x => x.Quantity * x.NetAmount);
            }
        }
    }
    [Ignore]
    public double TotalPayments { get => this.Payments.Where(x => x.Status != "CBILL").Sum(x => x.PaidAmount); }
    [Ignore]
    public double TotalContractual { get => this.Payments.Where(x => x.Status != "CBILL").Sum(x => x.ContractualAmount); }
    [Ignore]
    public double TotalWriteOff { get => this.Payments.Where(x => x.Status != "CBILL").Sum(x => x.WriteOffAmount); }
    [Ignore]
    public double TotalBadDebt
    {
        get => this.Payments.Where(x => x.IsCollectionPmt).Sum(x => x.WriteOffAmount);
    }

    [Ignore]
    public string FullInfo => $"{AccountNo} {PatFullName} {ClientMnem} {TransactionDate}";
    [Ignore]
    public string ClientName { get => this.Client.Name; }
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
            return InsurancePrimary?.InsCode;
        }
    }

    [Ignore]
    public Ins InsurancePrimary
    {
        get
        {
            return Insurances.Where(i => i.Coverage == InsCoverage.Primary).FirstOrDefault();
        }
    }
    [Ignore]
    public Ins InsuranceSecondary
    {
        get
        {
            return Insurances.Where(i => i.Coverage == InsCoverage.Secondary).FirstOrDefault();
        }
    }
    [Ignore]
    public Ins InsuranceTertiary
    {
        get
        {
            return Insurances.Where(i => i.Coverage == InsCoverage.Tertiary).FirstOrDefault();
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
            List<DateTime> dates = new()
            {
                UpdatedDate,
                Pat.UpdatedDate
            };

            if (Charges.Any())
                dates.Add(Charges.Max(x => x.UpdatedDate));

            if (Insurances.Any())
                dates.Add(Insurances.Max(x => x.UpdatedDate));

            if (Payments.Any())
                dates.Add(Payments.Max(x => x.UpdatedDate));

            if (Notes.Any())
                dates.Add(Notes.Max(x => x.UpdatedDate));

            if (BillingActivities.Any())
                dates.Add(BillingActivities.Max(x => x.UpdatedDate));

            return dates.Max();
        }
    }

    [Ignore]
    public AccountLock AccountLockInfo { get; set; }

    public override string ToString()
    {
        return FullInfo;
    }
}