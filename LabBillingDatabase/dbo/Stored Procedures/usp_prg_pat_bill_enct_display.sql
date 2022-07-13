
-- =============================================
-- Author:		David
-- Create date: 5/6/2015
-- Description:	inserts account into the table dbo.patbill_acc
-- for the statements.
-- work file P:\wkelly\SQL Server Management Studio\Projects\PAT_BILL\patbill_master.sql
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_pat_bill_enct_display] 
	-- Add the parameters for the stored procedure here
--	@startDate datetime = 0, 
--	@endDate datetime = 0
	@batch VARCHAR(6) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
 -- Insert statements for procedure here
 SELECT @batch = (
 SELECT COALESCE(@batch,  CONVERT(VARCHAR(4),YEAR(GETDATE() ))+
                       CONVERT(VARCHAR(2),GETDATE(),101) ))
 
; WITH cteEnctr
AS
(
	SELECT DISTINCT TOP(100) PERCENT
		E.statement_number ,
		E.record_cnt_acct ,
		E.patient_account_number AS [enctr_nbr],
		E.account_id,
		chrg.location,
		CONVERT(VARCHAR(10),chrg.service_date,101) AS [service_date],
		SUM(chrg.qty*dbo.chrg.net_amt) 
			--OVER (PARTITION BY chrg.account,chrg.cdm) AS [chrg_amt]
			--OVER (PARTITION BY chrg.account) 
		AS [chrg_amt]
	
	--,	chrg.cdm
	FROM dbo.patbill_acc E	
	INNER JOIN chrg ON chrg.account = E.account_id
	WHERE E.batch_id = @batch
	GROUP BY e.statement_number, e.record_cnt_acct
		,E.patient_account_number, E.account_id, chrg.location,
		CONVERT(VARCHAR(10),chrg.service_date,101)
	ORDER BY E.statement_number, E.record_cnt_acct
	
)--SELECT * FROM cteEnctr
--INSERT INTO dbo.patbill_enctr
--		(
--			record_type ,
--			record_cnt ,
--			statement_number,
--			enctr_nbr ,
--			pft_encntr_id ,
--			place_of_service ,
--			pft_encntr_dates_of_service ,
--			pft_encntr_amt_due ,
--			pft_encntr_prov_name ,
--			pft_encntr_prov_org_name ,
--			pft_encntr_prov_org_str_addr_ ,
--			pft_encntr_prov_org_str_addr_2 ,
--			pft_encntr_prov_org_str_addr_3 ,
--			pft_encntr_prov_org_str_addr_4 ,
--			pft_encntr_prov_org_city ,
--			pft_encntr_prov_org_state ,
--			pft_encntr_prov_org_zip ,
--			pft_encntr_prov_org_phone ,
--			pft_encntr_prov_hrs ,
--			pft_encntr_unpaid_bal ,
--			pft_encntr_patient_bal ,
--			pft_encntr_paid_since_last_stmt ,
--			pft_encntr_ins_discount ,
--			pft_encntr_ord_mgmt_act_type ,
--			pft_encntr_ord_mgmt_cat_type ,
--			pft_encntr_health_plan_name ,
--			pft_encntr_in_pending ,
--			pft_encntr_total ,
--			encntr_admit_dt_tm ,
--			encntr_discharge_dt_tm ,
--			encntr_medical_service ,
--			encntr_type ,
--			encntr_financial_class ,
--			encntr_vip ,
--			pft_encntr_qualifier ,
--			pft_encntr_total_charges ,
--			total_patient_payments ,
--			total_patient_adjustments ,
--			total_insurance_payments ,
--			total_insurance_adjustments ,
--			pft_encntr_assigned_agency ,
--			pft_encntr_pay_plan_flag ,
--			pft_encntr_pay_plan_status ,
--			pft_encntr_pay_plan_orig_amt ,
--			pft_encntr_pay_plan_pay_amt ,
--			pft_encntr_pay_plan_begin_dttm ,
--			pft_encntr_pay_plan_delinq_amt ,
--			pftectr_pri_clm_orig_trans_dttm ,
--			pftectr_pri_clm_cur_trans_dttm ,
--			pftectr_sec_clm_orig_trans_dttm ,
--			pftectr_sec_clm_cur_trans_dttm ,
--			pftectr_ter_clm_orig_trans_dttm ,
--			pftectr_ter_clm_cur_trans_dttm ,
--			pft_ectr_prim_insr_balance ,
--			pft_ectr_sec_insr_balance ,
--			pft_ectr_tert_insr_balance ,
--			pft_ectr_self_pay_balance ,
--			attending_physician_name ,
--			includes_est_pat_liab ,
--			total_charge_amount ,
--			non_covered_charge_amt ,
--			ABN_charge_amt ,
--			est_contract_allowance_amt_ind ,
--			est_contract_allowance_amt ,
--			encntr_deductible_rem_amt_ind ,
--			encntr_deductible_rem_amt ,
--			deductible_applied_amt ,
--			encntr_copay_amt_ind ,
--			encntr_copay_amt ,
--			encntr_coinsurance_pct_ind ,
--			encntr_coinsurance_pct ,
--			encntr_coinsurance_amt ,
--			maximum_out_of_pocket_amt_ind ,
--			maximum_out_of_pocket_amt ,
--			amt_over_max_out_of_pocket ,
--			est_patient_liab_amt,
--			batch_id 
--		)--SELECT * FROM cteEnctr
SELECT 
'ENCT' AS [record_type]
,cteEnctr.record_cnt_acct 
--,ROW_NUMBER() OVER 
--		(PARTITION BY cteEnctr.statement_number,cteEnctr.account_id
--		ORDER BY cteEnctr.statement_number, cteEnctr.account_id,cteEnctr.service_date) 
--	AS [record_cnt]
,cteEnctr.statement_number
,cteEnctr.enctr_nbr
,cteEnctr.account_id AS [pft_encntr_id]
,cteEnctr.location AS [place_of_service]
,cteEnctr.service_date AS [pft_encntr_dates_of_service]
,STR(cteEnctr.chrg_amt,10,2) AS [pft_encntr_amt_due]
,'' AS [pft_encntr_prov_name]
,'' AS [pft_encntr_prov_org_name]
,'' AS [pft_encntr_prov_org_str_addr_]
,'' AS [pft_encntr_prov_org_str_addr_2]
,'' AS [pft_encntr_prov_org_str_addr_3]
,'' AS [pft_encntr_prov_org_str_addr_4]
,'' AS [pft_encntr_prov_org_city]
,'' AS [pft_encntr_prov_org_state]
,'' AS [pft_encntr_prov_org_zip]
,'' AS [pft_encntr_prov_org_phone]
,'' AS [pft_encntr_prov_hrs]
,'' AS [pft_encntr_unpaid_bal]
,0.00 AS [pft_encntr_patient_bal]
,0.00 AS [pft_encntr_paid_since_last_stmt]
,'' AS [pft_encntr_ins_discount]
,'' AS [pft_encntr_ord_mgmt_act_type]
,'' AS [pft_encntr_ord_mgmt_cat_type]
,'' AS [pft_encntr_health_plan_name]
,0.00 AS [pft_encntr_in_pending]
,STR(cteEnctr.chrg_amt,10,2) AS [pft_encntr_total]
,cteEnctr.service_date AS [encntr_admit_dt_tm]
,cteEnctr.service_date AS [encntr_discharge_dt_tm]
,'' AS [encntr_medical_service]
,'REF LAB OUTREACH' AS [encntr_type]
,'' AS [encntr_financial_class]
,'' AS [encntr_vip]
,'' AS [pft_encntr_qualifier]
,STR(cteEnctr.chrg_amt,10,2) AS [pft_encntr_total_charges]
,0.00 AS [total_patient_payments]
,0.00 AS [total_patient_adjustments]
,'' AS [total_insurance_payments]
,'' AS [total_insurance_adjustments]
,'' AS [pft_encntr_assigned_agency]
,'' AS [pft_encntr_pay_plan_flag]
,'' AS [pft_encntr_pay_plan_status]
,'' AS [pft_encntr_pay_plan_orig_amt]
,'' AS [pft_encntr_pay_plan_pay_amt]
,'' AS [pft_encntr_pay_plan_begin_dttm]
,'' AS [pft_encntr_pay_plan_delinq_amt]
,'' AS [pftectr_pri_clm_orig_trans_dttm]
,'' AS [pftectr_pri_clm_cur_trans_dttm]
,'' AS [pftectr_sec_clm_orig_trans_dttm]
,'' AS [pftectr_sec_clm_cur_trans_dttm]
,'' AS [pftectr_ter_clm_orig_trans_dttm]
,'' AS [pftectr_ter_clm_cur_trans_dttm]
,'' AS [pft_ectr_prim_insr_balance]
,'' AS [pft_ectr_sec_insr_balance]
,'' AS [pft_ectr_tert_insr_balance]
,'' AS [pft_ectr_self_pay_balance]
,'' AS [attending_physician_name]
,0 AS [includes_est_pat_liab]
,0 AS [total_charge_amount]
,0 AS [non_covered_charge_amt]
,0 AS [ABN_charge_amt]
,0 AS [est_contract_allowance_amt_ind]
,0 AS [est_contract_allowance_amt]
,0 AS [encntr_deductible_rem_amt_ind]
,0 AS [encntr_deductible_rem_amt]
,0 AS [deductible_applied_amt]
,0 AS [encntr_copay_amt_ind]
,0 AS [encntr_copay_amt]
,0 AS [encntr_coinsurance_pct_ind]
,0 AS [encntr_coinsurance_pct]
,0 AS [encntr_coinsurance_amt]
,0 AS [maximum_out_of_pocket_amt_ind]
,0 AS [maximum_out_of_pocket_amt]
,0 AS [amt_over_max_out_of_pocket]
,0 AS [est_patient_liab_amt]

,@batch

FROM cteEnctr
LEFT OUTER JOIN dbo.patbill_enctr pe ON pe.statement_number
	= cteEnctr.statement_number 
	AND pe.enctr_nbr = cteEnctr.enctr_nbr
	AND pe.record_cnt = cteEnctr.record_cnt_acct 
--WHERE pe.statement_number IS null
ORDER BY cteEnctr.statement_number, cteEnctr.record_cnt_acct


END

