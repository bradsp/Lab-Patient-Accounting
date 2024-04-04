using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("patbill_enctr_actv")]
public sealed class PatientStatementEncounterActivity : IBaseEntity
{
    [Column("statement_number")]
    public Int64 StatementNumber { get; set; }
    [Column("record_type")]
    public string RecordType { get; set; }
    [Column("record_cnt")]
    public int RecordCount { get; set; }
    [Column("enctr_nbr")]
    public string EncntrNumber { get; set; }
    [Column("activity_id")]
    public string ActivityId { get; set; }
    [Column("activity_type")]
    public string ActivityType { get; set; }
    [Column("activity_date")]
    public string ActivityDate { get; set; }
    [Column("activity_description")]
    public string ActivityDescription { get; set; }
    [Column("activity_code")]
    public string ActivityCode { get; set; }
    [Column("activity_amount")]
    public string ActivityAmount { get; set; }
    [Column("units")]
    public double Units { get; set; }
    [Column("cpt_code")]
    public string CptCode { get; set; }
    [Column("cpt_description")]
    public string CptDescription { get; set; }
    [Column("drg_code")]
    public string DrgCode { get; set; }
    [Column("revenue_code")]
    public string RevenueCode { get; set; }
    [Column("revenue_code_description")]
    public string RevenueCodeDescription { get; set; }
    [Column("hcpcs_code")]
    public string HCPCSCode { get; set; }
    [Column("hcpcs_description")]
    public string HPCPSDescription { get; set; }
    [Column("order_mgmt_activity_type")]
    public string OrderMgmtActivityType { get; set; }
    [Column("activity_amount_due")]
    public double ActivityAmountDue { get; set; }
    [Column("activity_date_of_service")]
    public string ActivityDateOfService { get; set; }
    [Column("activity_patient_bal")]
    public double ActivityPatientBalance { get; set; }
    [Column("activity_ins_discount")]
    public double ActivityInsDiscount { get; set; }
    [Column("activity_trans_type")]
    public string ActivityTransType { get; set; }
    [Column("activity_trans_sub_type")]
    public string ActivityTransSubType { get; set; }
    [Column("activity_trans_amount")]
    public double ActivityTransAmount { get; set; }
    [Column("activity_health_plan_name")]
    public string ActivityHealthPlanName { get; set; }
    [Column("activity_ins_pending")]
    public double ActivityInsPending { get; set; }
    [Column("activity_dr_cr_flag")]
    public int ActivityDrCrFlag { get; set; }
    [Column("parent_activity_id")]
    public int ParentActivityId { get; set; }
    [Column("batch_id")]
    public string BatchId { get; set; }


    [Ignore]
    public DateTime UpdatedDate { get; set; }
    [Ignore]
    public string UpdatedUser { get; set; }
    [Ignore]
    public string UpdatedApp { get; set; }
    [Ignore]
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }
}
