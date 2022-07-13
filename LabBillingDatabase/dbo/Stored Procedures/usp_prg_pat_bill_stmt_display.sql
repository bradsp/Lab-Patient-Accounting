
-- =============================================
-- Author:		David
-- Create date: 5/6/2015
-- Description:	Selects the statements for Patient Bill by HL7
--		use usp_prg_pat_bill_acct to get the detail data
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_pat_bill_stmt_display] 
	-- Add the parameters for the stored procedure here
--	@startDate datetime = 0, 
--	@endDate datetime = 0
	@batch VARCHAR(6) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @batch = 
	(SELECT COALESCE(@batch,  CONVERT(VARCHAR(4),YEAR(GETDATE() ))+
	                                     CONVERT(VARCHAR(2),GETDATE() ,101) ))
    -- Insert statements for procedure here
; WITH cteStmt
AS
(
SELECT dbo.patbill_acc.statement_number ,

		SUM(dbo.patbill_acc.acct_amt_due) AS [stmt_amt_due] ,

		COALESCE(SUM(dbo.patbill_acc.acct_paid_since_last_stmt),0.00) AS [totat_paid_since_last_stmt] ,

		SUM(dbo.patbill_acc.includes_est_pat_lib) AS [stmt_est_pat_lib] ,
		SUM(dbo.patbill_acc.total_charge_amt) 
			AS [stmt_total_charge_amt]

		 ,SUM(dbo.patbill_acc.aging_bucket_current) AS [stmt_aging_bucket_current]
		 ,SUM(dbo.patbill_acc.aging_bucket_30) AS [stmt_aging_bucket_30_day]
		 ,SUM(dbo.patbill_acc.aging_bucket_60) AS [stmt_aging_bucket_60_day]
		 ,SUM(dbo.patbill_acc.aging_bucket_90) AS [stmt_aging_bucket_90_day]
		 
		FROM dbo.patbill_acc
		WHERE dbo.patbill_acc.batch_id = @batch		
		GROUP BY  dbo.patbill_acc.statement_number
		
)

--INSERT INTO dbo.patbill_stmt
--		(
--			record_type ,
--			record_cnt ,
--			statement_number ,
--			billing_entity_street ,
--			billing_entity_city ,
--			billing_entity_state ,
--			billing_entity_zip ,
--			billing_entity_federal_tax_id ,
--			billing_entity_name ,
--			billing_entity_phone_number ,
--			billing_entity_fax_number ,
--			remit_to_street ,
--			remit_to_street2 ,
--			remit_to_city ,
--			remit_to_state ,
--			remit_to_zip ,
--			remit_to_org_name ,
--			guarantor_street ,
--			guarantor_street_2 ,
--			guarantor_city ,
--			guarantor_state ,
--			guarantor_zip ,
--			guarantor_name ,
--			amount_due ,
--			date_due ,
--			balance_forward,
--			aging_bucket_current ,
--			aging_bucket_30_day ,
--			aging_bucket_60_day ,
--			aging_bucket_90_day ,
--			statement_total_amount ,
--			insurance_billed_amount ,
--			balance_forward_amount ,
--			balance_forward_date ,
--			primary_health_plan_name ,
--			prim_health_plan_policy_number ,
--			prim_health_plan_group_number ,
--			secondary_health_plan_name ,
--			sec_health_plan_policy_number ,
--			sec_health_plan_group_number ,
--			tertiary_health_plan_name ,
--			ter_health_plan_policy_number ,
--			ter_health_plan_group_number ,
--			statement_time ,
--			page_number ,
--			insurance_pending ,
--			unpaid_balance ,
--			patient_balance,
--			totat_paid_since_last_stmt ,
--			insurance_discount ,
--			[contact text] ,
--			transmission_dt_tm ,
--			guarantor_country ,
--			guarantor_ssn ,
--			guarantor_phone ,
--			statement_submitted_dt_tm ,
--			includes_est_pat_lib ,
--			total_charge_amount ,
--			non_covered_charge_amt ,
--			ABN_charge_amt ,
--			est_contract_allowance_amt_ind ,
--			est_contract_allowance_amt ,
--			encntr_deductible_rem_amt_ind ,
--			deductible_applied_amt ,
--			encntr_copay_amt_ind ,
--			encntr_copay_amt ,
--			encntr_coinsurance_pct_ind ,
--			encntr_coinsurance_pct ,
--			encntr_coinsurance_amt ,
--			maximum_out_of_pocket_amt_ind ,
--			amt_over_max_out_of_pocket ,
--			est_patient_liab_amt ,
--			online_billpay_url ,
--			guarantor_access_code
--			, dbo.patbill_stmt.batch_id
--		)
---------------
SELECT 'STMT' AS [record_type]
, ROW_NUMBER() OVER (ORDER BY cteStmt.statement_number) AS [record_cnt]
,cteStmt.statement_number 
-- Billing / remit to addresses
, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_street') 
	AS [billing_entity_street]
, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_city') 
	AS [billing_entity_city]
, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_state') 
	AS [billing_entity_state]
, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_zip') 
	AS [billing_entity_zip]
, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_fed_tax_id') 
	AS [billing_entity_federal_tax_id]
, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_name') 
	AS [billing_entity_name]
, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_phone') 
	AS [billing_entity_phone_number]
, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_fax') 
	AS [billing_entity_fax_number]
, (select value FROM system WHERE dbo.system.key_name = 'remit_to_street') 
	AS [remit_to_street]
, (select value FROM system WHERE dbo.system.key_name = 'remit_to_street2') 
	AS [remit_to_street2]
, (select value FROM system WHERE dbo.system.key_name = 'remit_to_city') 
	AS [remit_to_city]
, (select value FROM system WHERE dbo.system.key_name = 'remit_to_state') 
	AS [remit_to_state]
, (select value FROM system WHERE dbo.system.key_name = 'remit_to_zip') 
	AS [remit_to_zip]
, (select value FROM system WHERE dbo.system.key_name = 'remit_to_org_name') 
	AS [remit_to_org_name]
-- end of billing / remit to addresses
-- guarantor information
, COALESCE(NULLIF(pat.guar_addr,''), pat.pat_addr1) AS [guarantor_street]
, '' AS [guarantor_street_2]

, COALESCE(NULLIF(dbo.SplitCITY_ST_ZIP(pat.g_city_st,'C'),'')
	, dbo.SplitCITY_ST_ZIP(pat.city_st_zip,'C'))  AS [guarantor_city]
	
, COALESCE(NULLIF(dbo.SplitCITY_ST_ZIP(pat.g_city_st,'S'),'')
	, dbo.SplitCITY_ST_ZIP(pat.city_st_zip,'S')) AS [guarantor_state]

, COALESCE(NULLIF(dbo.SplitCITY_ST_ZIP(pat.g_city_st,'Z'),'')
	, dbo.SplitCITY_ST_ZIP(pat.city_st_zip,'Z')) AS [guarantor_zip]

, COALESCE(NULLIF(pat.guarantor,''), pat.pat_full_name) AS [guarantor_name] 
-- end of Guarantor information
-- Amt due and payment due date
,cteStmt.stmt_amt_due 
, CONVERT(VARCHAR,DATEADD(mm,DATEDIFF(m,0,GETDATE())+2, -.000003) ,101) AS [date_due]-- Last day Next Month
-- end of Amt due and payment due date
, '0.00' AS [balance_forward]
, cteStmt.stmt_aging_bucket_current
, cteStmt.stmt_aging_bucket_30_day
, cteStmt.stmt_aging_bucket_60_day
, cteStmt.stmt_aging_bucket_90_day
, cteStmt.stmt_amt_due 
,'' AS [insurance_billed_amount]
,'' AS [balance_forward_amount]
,'' AS [balance_forward_date]
,'' AS [primary_health_plan_name]
,'' AS [prim_health_plan_policy_number]
,'' AS [prim_health_plan_group_number]
,'' AS [secondary_health_plan_name]
,'' AS [sec_health_plan_policy_number]
,'' AS [sec_health_plan_group_number]
,'' AS [tertiary_health_plan_name]
,'' AS [ter_health_plan_policy_number]
,'' AS [ter_health_plan_group_number]
,'12:00' AS [statement_time]
,'' AS [page_number]
,0.00 AS [insurance_pending]
,cteStmt.stmt_amt_due  AS [unpaid_balance]
,cteStmt.stmt_amt_due  AS [patient_balance]
, cteStmt.totat_paid_since_last_stmt AS [totat_paid_since_last_stmt]
,0.00 AS [insurance_discount]
,'' AS [contact text]
,'' AS [transmission_dt_tm]
,'USA' AS [guarantor_country]
,'' AS [guarantor_ssn]
,COALESCE( pat.guar_phone, pat.pat_phone) AS [guarantor_phone]
,'' AS [statement_dt_tm]
, 0 AS [includes_est_pat_lib]
, cteStmt.stmt_total_charge_amt  AS [total_charge_amount]
, 0 AS [non_covered_charge_amt]
	, 0 AS [ABN_charge_amt]
	, 0 AS [est_contract_allowance_amt_ind]
	, 0 AS [est_contract_allowance_amt]
	,0 AS [encntr_deductible_rem_amt_ind]
	,0 AS [deductible_applied_amt]
	,0 AS [encntr_copay_amt_ind]
	,0 AS [encntr_copay_amt]
	,0 AS [encntr_coinsurance_pct_ind]
	,0 AS [encntr_coinsurance_pct]
	,0 AS [encntr_coinsurance_amt]
	,0 AS [maximum_out_of_pocket_amt_ind]
	,0 AS [amt_over_max_out_of_pocket]
	,0 AS [est_patient_liab_amt]
,'' AS [online_billpay_url]
,'' AS [guarantor_access_code]
, @batch
FROM cteStmt
inner JOIN dbo.patbill_acc ON 
	dbo.patbill_acc.statement_number = cteStmt.statement_number
	AND dbo.patbill_acc.record_cnt_acct = 1
INNER JOIN pat ON 
	pat.account = dbo.patbill_acc.account_id


END

