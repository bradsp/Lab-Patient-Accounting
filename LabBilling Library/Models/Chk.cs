using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("chk")]
[PrimaryKey("pay_no", AutoIncrement = true)]
public sealed class Chk : IBaseEntity
{

    [Column("deleted")]
    public bool IsDeleted { get; set; }
    [Column("pay_no")]
    public double PaymentNo { get; set; }
    [Column("account")]
    public string AccountNo { get; set; }
    [Column("chk_date")]
    public DateTime? ChkDate { get; set; }
    [Column("date_rec")]
    public DateTime? DateReceived { get; set; }
    [Column("chk_no")]
    public string CheckNo { get; set; }
    [Column("amt_paid")]
    public double PaidAmount { get; set; }
    [Column("write_off")]
    public double WriteOffAmount { get; set; }
    [Column("contractual")]
    public double ContractualAmount { get; set; }
    [Column("status")]
    public string Status { get; set; } = "NEW";
    [Column("source")]
    public string Source { get; set; }
    [Column("fin_code")]
    public string FinCode { get; set; }
    [Column("w_off_date")]
    public DateTime? WriteOffDate { get; set; }
    [Column("invoice")]
    public string Invoice { get; set; }
    [Column("batch")]
    public double Batch { get; set; }
    [Column("comment")]
    public string Comment { get; set; }
    [Column("bad_debt")]
    public bool IsCollectionPmt { get; set; }
    [Column("cpt4Code")]
    public string Cpt4Code { get; set; }
    [Column("post_file")]
    public string PostingFile { get; set; }
    [Column("write_off_code")]
    public string WriteOffCode { get; set; }
    [Column("eft_date")]
    public DateTime? EftDate { get; set; }
    [Column("eft_number")]
    public string EftNumber { get; set; }
    [Column("post_date")]
    public DateTime? PostingDate { get; set; }
    [Column("ins_code")]
    public string InsCode { get; set; }
    [Column("claim_adj_code")]
    public string ClaimAdjCode { get; set; }
    [Column("claim_adj_group_code")]
    public string ClaimAdjGroupCode { get; set; }
    [Column("facility_code")]
    public string FacilityCode { get; set; }
    [Column("claim_no")]
    public string ClaimNo { get; set; }

    [ResultColumn]
    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    [ResultColumn]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    [ResultColumn]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    [ResultColumn]
    public string UpdatedHost { get; set; }
    [ResultColumn]
    public DateTime? mod_date_audit { get; set; }

    [Ignore]
    public bool IsRefund { get; set; } = false;

    public Guid rowguid { get; set; }
    public Guid chrg_rowguid { get; set; }
}
