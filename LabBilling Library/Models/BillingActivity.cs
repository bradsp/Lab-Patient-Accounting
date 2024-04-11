using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("data_billing_history")]
[PrimaryKey("rowguid", AutoIncrement = false)]
public sealed class BillingActivity : IBaseEntity
{
    public Guid rowguid { get; set; }
    [Column("deleted")]
    public bool IsDeleted { get; set; }

    [Column("account")]
    public string AccountNo { get; set; }

    [Column("ins_abc")]
    public string InsuranceOrder { get; set; }

    [Column("pat_name")]
    public string PatientName { get; set; }

    [Column("fin_code")]
    public string FinancialCode { get; set; }

    [Column("ins_code")]
    public string InsuranceCode { get; set; }

    [Column("trans_date")]
    public DateTime? TransactionDate { get; set; }

    [Column("run_date")]
    public DateTime RunDate { get; set; }

    [Column("printed")]
    public bool IsPrinted { get; set; }

    [Column("run_user")]
    public string RunUser { get; set; }

    [Column("batch")]
    public double Batch { get; set; }

    [Column("ebill_status")]
    public string ElectronicBillStatus { get; set; }

    [Column("ebill_batch")]
    public double ElectronicBillBatch { get; set; }

    [Column("text")]
    public string Text { get; set; }

    [Column("ins_complete")]
    public DateTime? InsComplete { get; set; }

    [Column("claim_amount")]
    public double ClaimAmount { get; set; }

    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }

}
