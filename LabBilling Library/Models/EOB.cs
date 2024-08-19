using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[PetaPoco.TableName("data_EOB")]
[PetaPoco.PrimaryKey("uid", AutoIncrement = true)]
public class Eob : IBaseEntity
{
    [Column("rowguid")]
    public Guid rowguid { get; set; }
    [Column("deleted")]
    public bool IsDeleted { get; set; }
    [Column("payor")]
    public string Payor { get; set; }
    [Column("account")]
    public string AccountNo { get; set; }
    [Column("subscriberID")]
    public string SubscriberID { get; set; }
    [Column("subscriberName")]
    public string SubscriberName { get; set; }
    [Column("date_of_service")]
    public DateTime DateOfService { get; set; }
    [Column("ICN")]
    public string ICN { get; set; }
    [Column("PatStat")]
    public string PatStat { get; set; }
    [Column("claim_status")]
    public string ClaimStatus { get; set; }
    [Column("claim_type")]
    public string ClaimType { get; set; }
    [Column("charges_reported")]
    public double ChargesReported { get; set; }
    [Column("charges_noncovered")]
    public double ChargesNoncovered { get; set; }
    [Column("charges_denied")]
    public double ChargesDenied { get; set; }
    [Column("pat_lib_coinsurance")]
    public double PatLibCoinsurance { get; set; }
    [Column("pat_lib_noncovered")]
    public double PatLibNoncovered { get; set; }
    [Column("pay_data_reimb_rate")]
    public string PayDataReimbRate { get; set; }
    [Column("pay_data_msp_prim_pay")]
    public double PayDataMspPrimPay { get; set; }
    [Column("pay_data_hcpcs_amt")]
    public double PayDataHcpcsAmt { get; set; }
    [Column("pay_data_cont_adj_amt")]
    public double pay_data_cont_adj_amt { get; set; }
    [Column("pay_data_pat_refund")]
    public double PayDataPatRefund { get; set; }
    [Column("pay_data_per_diem_rate")]
    public string PayDataPerDiemRate { get; set; }
    [Column("pay_data_net_reimb_amt")]
    public double PayDataNetReimbAmt { get; set; }
    [Column("claim_forwarded_to")]
    public string ClaimForwardedTo { get; set; }
    [Column("claim_forwarded_id")]
    public string ClaimForwardedId { get; set; }
    [Column("eft_file")]
    public string EftFile { get; set; }
    [Column("eft_number")]
    public string EftNumber { get; set; }
    [Column("eft_date")]
    public DateTime EftDate { get; set; }
    [Column("eob_print_date")]
    public DateTime EobPrintDate { get; set; }
    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }
    [Column("bill_cycle_date")]
    public DateTime BillCycleDate { get; set; }
    [Column("check_no")]
    public string CheckNo { get; set; }
    [Column("provider_id")]
    public string ProviderId { get; set; }
    [Column("uid")]
    public int Id { get; set; }
}

[PetaPoco.TableName("data_EOB_Detail")]
[PetaPoco.PrimaryKey("uid", AutoIncrement = true)]
public class EobDetail : IBaseEntity
{
    [Column("rowguid")] public Guid rowguid { get; set; }
    [Column("deleted")] public bool IsDeleted { get; set; }
    [Column("account")] public string AccountNo { get; set; }
    [Column("claim_status")] public string ClaimStatus { get; set; }
    [Column("ServiceCode")] public string ServiceCode { get; set; }
    [Column("rev_code")] public string RevenueCode { get; set; }
    [Column("units")] public int Units { get; set; }
    [Column("apc_nr")] public string ApcNR { get; set; }
    [Column("allowed_amt")] public double AllowedAmt { get; set; }
    [Column("stat")] public string Stat { get; set; }
    [Column("wght")] public string Wght { get; set; }
    [Column("date_of_service")] public DateTime DateOfService { get; set; }
    [Column("charge_amt")] public double ChargeAmt { get; set; }
    [Column("paid_amt")] public double PaidAmt { get; set; }
    [Column("reason_type")] public string ReasonType { get; set; }
    [Column("reason_code")] public string ReasonCode { get; set; }
    [Column("adj_amt")] public double AdjAmt { get; set; }
    [Column("other_adj_amt")] public double OtherAdjAmt { get; set; }
    [Column("mod_date")] public DateTime UpdatedDate { get; set; }
    [Column("mod_user")] public string UpdatedUser { get; set; }
    [Column("mod_prg")] public string UpdatedApp { get; set; }
    [Column("mod_host")] public string UpdatedHost { get; set; }
    [Column("uid")] public int Id { get; set; }
    [Column("bill_cycle_date")] public DateTime BillCycleDate { get; set; }
    [Column("check_no")] public string CheckNo { get; set; }
}
