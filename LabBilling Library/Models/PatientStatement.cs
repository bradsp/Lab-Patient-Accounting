using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("dbo.patbill_stmt")]
    public sealed class PatientStatement : IBaseEntity
    {
        [Column("record_type")]
        public string RecordType { get; set; }
        [Column("record_cnt")]
        public int RecordCount { get; set; }
        [Column("statement_number")]
        public Int64 StatementNumber { get; set; }
        [Column("billing_entity_street")]
        public string BillingEntityStreet { get; set; }
        [Column("billing_entity_city")]
        public string BillingEntityCity { get; set; }
        [Column("billing_entity_state")]
        public string BillingEntityState { get; set; }
        [Column("billing_entity_zip")]
        public string BillingEntityZip { get; set; }
        [Column("billing_entity_federal_tax_id")]
        public string BillingEntityFedTaxId { get; set; }
        [Column("billing_entity_name")]
        public string BillingEntityName { get; set; }
        [Column("billing_entity_phone_number")]
        public string BillingEntityPhone { get; set; }
        [Column("billing_entity_fax_number")]
        public string BillingEntityFax { get; set; }
        [Column("remit_to_street")]
        public string RemitToStreet { get; set; }
        [Column("remit_to_street2")]
        public string RemitToStreet2 { get; set; }
        [Column("remit_to_city")]
        public string RemitToCity { get; set; }
        [Column("remit_to_state")]
        public string RemitToState { get; set; }
        [Column("remit_to_zip")]
        public string RemitToZip { get; set; }
        [Column("remit_to_org_name")]
        public string RemitToOrgName { get; set; }
        [Column("guarantor_street")]
        public string GuarantorStreet { get; set; }
        [Column("guarantor_street_2")]
        public string GuarantorStreet2 { get; set; }
        [Column("guarantor_city")]
        public string GuarantorCity { get; set; }
        [Column("guarantor_state")]
        public string GuarantorState { get; set; }
        [Column("guarantor_zip")]
        public string GuarantorZip { get; set; }
        [Column("guarantor_name")]
        public string GuarantorName { get; set; }
        [Column("amount_due")]
        public double AmountDue { get; set; }
        [Column("date_due")]
        public string DateDue { get; set; }
        [Column("balance_forward")]
        public double BalanceForward { get; set; }
        [Column("aging_bucket_current")]
        public double AgingBucketCurrent { get; set; }
        [Column("aging_bucket_30_day")]
        public double AgingBucket30Day { get; set; }
        [Column("aging_bucket_60_day")]
        public double AgingBucket60Day { get; set; }
        [Column("aging_bucket_90_day")]
        public double AgingBucket90Day { get; set; }
        [Column("statement_total_amount")]
        public double StatementTotalAmount { get; set; }
        [Column("insurance_billed_amount")]
        public string InsuranceBilledAmount { get; set; }
        [Column("balance_forward_amount")]
        public string BalanceForwardAmount { get; set; }
        [Column("balance_forward_date")]
        public string BalanceForwardDate { get; set; }
        [Column("primary_health_plan_name")]
        public string PrimaryHealthPlanName { get; set; }
        [Column("prim_health_plan_policy_number")]
        public string PrimaryHealthPlanPolicyNumber { get; set; }
        [Column("prim_health_plan_group_number")]
        public string PrimaryHealthPlanGroupNumber { get; set; }
        [Column("secondary_health_plan_name")]
        public string SecondaryHealthPlanName { get; set; }
        [Column("sec_health_plan_policy_number")]
        public string SecondaryHealthPlanPolicyNumber { get; set; }
        [Column("sec_health_plan_group_number")]
        public string SecondaryHealthPlanGroupNumber { get; set; }
        [Column("tertiary_health_plan_name")]
        public string TertiaryHealthPlanName { get; set; }
        [Column("ter_health_plan_policy_number")]
        public string TertiaryHealthPlanPolicyNumber { get; set; }
        [Column("ter_health_plan_group_number")]
        public string TertiaryHealthPlanGroupNumber { get; set; }
        [Column("statement_time")]
        public string StatementTime { get; set; }
        [Column("page_number")]
        public string PageNumber { get; set; }
        [Column("insurance_pending")]
        public double InsurancePending { get; set; }
        [Column("unpaid_balance")]
        public double UnpaidBalance { get; set; }
        [Column("patient_balance")]
        public double PatientBalance { get; set; }
        [Column("totat_paid_since_last_stmt")]
        public double TotalPaidSinceLastStatement { get; set; }
        [Column("insurance_discount")]
        public double InsuranceDiscount { get; set; }
        [Column("contact text")]
        public string ContactText { get; set; }
        [Column("transmission_dt_tm")]
        public string TransmissionDateTime { get; set; }
        [Column("guarantor_country")]
        public string GuarantorCountry { get; set; }
        [Column("guarantor_ssn")]
        public string GuarantorSSN { get; set; }
        [Column("guarantor_phone")]
        public string GuarantorPhone { get; set; }
        [Column("statement_submitted_dt_tm")]
        public DateTime StatementSubmittedDateTime { get; set; }
        [Column("includes_est_pat_lib")]
        public int IncludesEstPatLib { get; set; }
        [Column("total_charge_amount")]
        public double TotalChargeAmount { get; set; }
        [Column("non_covered_charge_amt")]
        public int NonCoveredChargeAmount { get; set; }
        [Column("ABN_charge_amt")]
        public int ABNChargeAmount { get; set; }
        [Column("est_contract_allowance_amt_ind")]
        public int EstContractAllowanceAmtInd { get; set; }
        [Column("est_contract_allowance_amt")]
        public int EstContractAllowanceAmt { get; set; }
        [Column("encntr_deductible_rem_amt_ind")]
        public int EncntrDeductibleRemAmtInd { get; set; }
        [Column("deductible_applied_amt")]
        public int DeductibleAppliedAmt { get; set; }
        [Column("encntr_copay_amt_ind")]
        public int EncntrCopayAmtInd { get; set; }
        [Column("encntr_copay_amt")]
        public int EncntrCopayAmt { get; set; }
        [Column("encntr_coinsurance_pct_ind")]
        public int EncntrCoinsurancePctInd { get; set; }
        [Column("encntr_coinsurance_pct")]
        public int EncntrCoinsurancePct { get; set; }
        [Column("encntr_coinsurance_amt")]
        public int EncntrCoinsuranceAmt { get; set; }
        [Column("maximum_out_of_pocket_amt_ind")]
        public int MaxOutOfPocketAmtInd { get; set; }
        [Column("amt_over_max_out_of_pocket")]
        public int AmtOverMaxOutOfPocket { get; set; }
        [Column("est_patient_liab_amt")]
        public int EstPatientLiabAmt { get; set; }
        [Column("online_billpay_url")]
        public string OnlineBillpayUrl { get; set; }
        [Column("guarantor_access_code")]
        public string GuarantorAccessCode { get; set; }
        [Column("batch_id")]
        public string BatchId { get; set; }

        [Ignore]
        public List<PatientStatementAccount> Accounts { get; set; }
        [Ignore] 
        public List<PatientStatementCerner> CernerStatements { get; set; }
        [Ignore] 
        public List<PatientStatementEncounter> Encounters { get; set; }
        [Ignore] 
        public List<PatientStatementEncounterActivity> EncounterActivity { get; set; }


        [Ignore]
        public DateTime mod_date { get; set; }
        [Ignore] 
        public string mod_user { get; set; }
        [Ignore] 
        public string mod_prg { get; set; }
        [Ignore] 
        public string mod_host { get; set; }
        [Ignore] 
        public Guid rowguid { get; set; }
    }
}
