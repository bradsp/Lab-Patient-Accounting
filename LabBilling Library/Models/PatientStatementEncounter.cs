using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dbo.patbill_enctr")]
    public class PatientStatementEncounter : IBaseEntity
    {
        [Column("record_type")] 
        public string RecordType { get; set; }
        [Column("record_cnt")] 
        public int RecordCount { get; set; }
        [Column("statement_number")] 
        public float StatementNumber { get; set; }
        [Column("enctr_nbr")] 
        public string EncntrNumber { get; set; }
        [Column("pft_encntr_id")] 
        public string PFTEncntrId { get; set; }
        [Column("place_of_service")] 
        public string PlaceOfService { get; set; }
        [Column("pft_encntr_dates_of_service")] 
        public string PFTEncntrDatesOfService { get; set; }
        [Column("pft_encntr_amt_due")] 
        public string PFTEncntrAmtDue { get; set; }
        [Column("pft_encntr_prov_name")] 
        public string PFTEncntrProvName { get; set; }
        [Column("pft_encntr_prov_org_name")] 
        public string PFTEncntrProvOrgName { get; set; }
        [Column("pft_encntr_prov_org_str_addr_")] 
        public string PFTEncntrProvOrgStreetAddr { get; set; }
        [Column("pft_encntr_prov_org_str_addr_2")] 
        public string PFTEncntrProvOrgStreetAddr2 { get; set; }
        [Column("pft_encntr_prov_org_str_addr_3")] 
        public string PFTEncntrProvOrgStreetAddr3 { get; set; }
        [Column("pft_encntr_prov_org_str_addr_4")] 
        public string PFTEncntrProvOrgStreetAddr4 { get; set; }
        [Column("pft_encntr_prov_org_city")] 
        public string PFTEncntrProvOrgCity { get; set; }
        [Column("pft_encntr_prov_org_state")] 
        public string PFTEncntrProvOrgState { get; set; }
        [Column("pft_encntr_prov_org_zip")] 
        public string PFTEncntrProvOrgZip { get; set; }
        [Column("pft_encntr_prov_org_phone")] 
        public string PFTEncntrProvOrgPhone { get; set; }
        [Column("pft_encntr_prov_hrs")] 
        public string PFTEncntrProvHrs { get; set; }
        [Column("pft_encntr_unpaid_bal")] 
        public string PFTEncntrUnpaidBalance { get; set; }
        [Column("pft_encntr_patient_bal")] 
        public double PFTEncntrPatientBalance { get; set; }
        [Column("pft_encntr_paid_since_last_stmt")] 
        public double PFTEncntrPaidSinceLastStmt { get; set; }
        [Column("pft_encntr_ins_discount")] 
        public string PFTEncntrInsDiscount { get; set; }
        [Column("pft_encntr_ord_mgmt_act_type")] 
        public string PFTEncntrOrdMgmtActType { get; set; }
        [Column("pft_encntr_ord_mgmt_cat_type")] 
        public string PFTEncntrOrgMgmtCatType { get; set; }
        [Column("pft_encntr_health_plan_name")] 
        public string PFTEncntrHealthPlanName { get; set; }
        [Column("pft_encntr_in_pending")] 
        public double PFTEncntrInPending { get; set; }
        [Column("pft_encntr_total")] 
        public string PFTEncntrTotal { get; set; }
        [Column("encntr_admit_dt_tm")] 
        public string EncntrAdmitDateTime { get; set; }
        [Column("encntr_discharge_dt_tm")] 
        public string EncntrDischargeDateTime { get; set; }
        [Column("encntr_medical_service")] 
        public string EncntrMedicalService { get; set; }
        [Column("encntr_type")] 
        public string EncntrType { get; set; }
        [Column("encntr_financial_class")] 
        public string EncntrFinancialClass { get; set; }
        [Column("encntr_vip")] 
        public string EncntrVIP { get; set; }
        [Column("pft_encntr_qualifier")] 
        public string PFTEncntrQualifier { get; set; }
        [Column("pft_encntr_total_charges")] 
        public string PFTEncntrTotalCharges { get; set; }
        [Column("total_patient_payments")] 
        public double TotalPatientPayments { get; set; }
        [Column("total_patient_adjustments")] 
        public double TotalPatientAdjustments { get; set; }
        [Column("total_insurance_payments")] 
        public string TotalInsurancePayments { get; set; }
        [Column("total_insurance_adjustments")] 
        public string TotalInsuranceAdjustments { get; set; }
        [Column("pft_encntr_assigned_agency")] 
        public string PFTEncntrAssignedAgency { get; set; }
        [Column("pft_encntr_pay_plan_flag")] 
        public string PFTEncntrPayPlanFlag { get; set; }
        [Column("pft_encntr_pay_plan_status")] 
        public string PFTEncntrPayPlanStatus { get; set; }
        [Column("pft_encntr_pay_plan_orig_amt")] 
        public string PFTEncntrPayPlanOrigAmt { get; set; }
        [Column("pft_encntr_pay_plan_pay_amt")] 
        public string PFTEncntrPayPlanPayAmt { get; set; }
        [Column("pft_encntr_pay_plan_begin_dttm")] 
        public string PFTEncntrPayPlanBeginDateTime { get; set; }
        [Column("pft_encntr_pay_plan_delinq_amt")] 
        public string PFTEncntrPayPlanDelinqAmt { get; set; }
        [Column("pftectr_pri_clm_orig_trans_dttm")] 
        public string PFTEncntrPriClmOrigTransDateTime { get; set; }
        [Column("pftectr_pri_clm_cur_trans_dttm")] 
        public string PFTEncntrPriClmCurTransDateTime { get; set; }
        [Column("pftectr_sec_clm_orig_trans_dttm")] 
        public string PFTEncntrSecClmOrigTransDateTime { get; set; }
        [Column("pftectr_sec_clm_cur_trans_dttm")] 
        public string PFTSecClmCurTransDateTime { get; set; }
        [Column("pftectr_ter_clm_orig_trans_dttm")] 
        public string PFTEncntrTerClmOrigTransDateTime { get; set; }
        [Column("pftectr_ter_clm_cur_trans_dttm")] 
        public string PFTEncntrTerClmCurTransDateTime { get; set; }
        [Column("pft_ectr_prim_insr_balance")] 
        public string PFTEncntrPrimInsuranceBalance { get; set; }
        [Column("pft_ectr_sec_insr_balance")] 
        public string PFTEncntrSecInsuranceBalance { get; set; }
        [Column("pft_ectr_tert_insr_balance")] 
        public string PFTEncntrTerInsuranceBalance { get; set; }
        [Column("pft_ectr_self_pay_balance")] 
        public string PFTEncntrSelfPayBalance { get; set; }
        [Column("attending_physician_name")] 
        public string AttendingPhysicianName { get; set; }
        [Column("includes_est_pat_liab")] 
        public int IncludesEstPatLiab { get; set; }
        [Column("total_charge_amount")] 
        public int TotalChargeAmount { get; set; }
        [Column("non_covered_charge_amt")] 
        public int NonCoveredChargeAmt { get; set; }
        [Column("ABN_charge_amt")] 
        public int ABNChargeAmt { get; set; }
        [Column("est_contract_allowance_amt_ind")] 
        public int EstContractAllowanceAmtInd { get; set; }
        [Column("est_contract_allowance_amt")] 
        public int EstContractAllowanceAmt { get; set; }
        [Column("encntr_deductible_rem_amt_ind")] 
        public int EncntrDeductibleRemAmtInd { get; set; }
        [Column("encntr_deductible_rem_amt")] 
        public int EncntrDeductibleRemAmt { get; set; }
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
        public int MaximumOutOfPocketAmtInd { get; set; }
        [Column("maximum_out_of_pocket_amt")] 
        public int MaximumOutOfPocketAmt { get; set; }
        [Column("amt_over_max_out_of_pocket")] 
        public int AmtOverMaxOutOfPocket { get; set; }
        [Column("est_patient_liab_amt")] 
        public int EstPatientLiabAmt { get; set; }
        [Column("batch_id")] 
        public string BatchId { get; set; }


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
