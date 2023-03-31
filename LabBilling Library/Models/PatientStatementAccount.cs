using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dbo.patbill_acc")]
    [PrimaryKey("uid", AutoIncrement = true)]
    public class PatientStatementAccount : IBaseEntity
    {
        [Column("statement_number")] 
        public Int64 StatementNumber { get; set; }
        [Column("record_cnt_acct")] 
        public string RecordCountAcct { get; set; }
        [Column("patient_account_number")] 
        public string PatientAccountNumber { get; set; }
        [Column("account_id")] 
        public string AccountId { get; set; }
        [Column("pat_name")] 
        public string PatientName { get; set; }
        [Column("account_subtotal")] 
        public string AccountSubtotal { get; set; }
        [Column("total_account_subtotal")] 
        public double TotalAccountSubtotal { get; set; }
        [Column("acct_amt_due")] 
        public double AccountAmtDue { get; set; }
        [Column("acct_ins_pending")] 
        public double AccountInsPending { get; set; }
        [Column("acct_dates_of_service")] 
        public string AccountDatesOfService { get; set; }
        [Column("acct_unpaid_bal")] 
        public double AccountUnpaidBalance { get; set; }
        [Column("acct_patient_bal")] 
        public double AccountPatientBalance { get; set; }
        [Column("acct_paid_since_last_stmt")] 
        public double AccountPaidSinceLastStatement { get; set; }
        [Column("acct_ins_discount")] 
        public double AccountInsDiscount { get; set; }
        [Column("acct_date_due")] 
        public string AccountDateDue { get; set; }
        [Column("acct_health_plan_name")] 
        public string AccountHealthPlanName { get; set; }
        [Column("patient_date_of_birth")] 
        public string PatientDateOfBirth { get; set; }
        [Column("patient_date_of_death")] 
        public string PatientDateOfDeath { get; set; }
        [Column("patient_sex")] 
        public string PatientSex { get; set; }
        [Column("patient_vip")] 
        public string PatientVip { get; set; }
        [Column("includes_est_pat_lib")] 
        public int IncludesEstPatLib { get; set; }
        [Column("total_charge_amt")] 
        public double TotalChargeAmt { get; set; }
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
        [Column("amt_over_max_out_of_pocket")] 
        public int AmtOverMaxOutOfPocket { get; set; }
        [Column("est_patient_liab_amt")] 
        public int EstPatientLiabAmt { get; set; }
        [Column("acc_msg")] 
        public string AccountMsg { get; set; }
        [Column("mailer")] 
        public string Mailer { get; set; }
        [Column("first_data_mailer")] 
        public DateTime FirstDataMailer { get; set; }
        [Column("last_data_mailer")] 
        public DateTime LastDataMailer { get; set; }
        [Column("mailer_count")] 
        public int MailerCount { get; set; }
        [Column("processed_date")] 
        public DateTime ProcessedDate { get; set; }
        [Column("date_sent")] 
        public DateTime DateSent { get; set; }
        [Column("aging_bucket_current")] 
        public double AgingBucketCurrent { get; set; }
        [Column("aging_bucket_30")] 
        public double AgingBucket30 { get; set; }
        [Column("aging_bucket_60")] 
        public double AgingBucket60 { get; set; }
        [Column("aging_bucket_90")] 
        public double AgingBucket90 { get; set; }
        [Column("batch_id")] 
        public string BatchId { get; set; }
        [Column("uid")] 
        public int Uid { get; set; }


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
