
-- =============================================
-- Author:		David
-- Create date: 5/6/2015
-- Description:	Selects the statements for Patient Bill by HL7
--		use usp_prg_pat_bill_acct to get the detail data
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_pat_bill_stmt_errors] 
	-- Add the parameters for the stored procedure here
--	@startDate datetime = 0, 
--	@endDate datetime = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	; WITH cte AS
(
	SELECT 
	ROW_NUMBER() OVER (PARTITION BY 
	ABS(CHECKSUM(QUOTENAME(UPPER(COALESCE(dbo.pat.guarantor,dbo.pat.pat_full_name))) )) 
	ORDER BY ABS(CHECKSUM(QUOTENAME(UPPER(COALESCE(dbo.pat.guarantor,dbo.pat.pat_full_name))) )) )AS [rn]

	,ABS(CHECKSUM(QUOTENAME(UPPER(COALESCE(dbo.pat.guarantor,dbo.pat.pat_full_name))) ))  AS [id]
--	, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_street') AS [billing_entity_street]
--	, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_city') AS [billing_entity_city]
--	, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_state') AS [billing_entity_state]
--	, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_zip') AS [billing_entity_zip]
--	, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_fed_tax_id') AS [billing_entity_federal_tax_id]
--	, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_name') AS [billing_entity_name]
--	, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_phone') AS [billing_entity_phone_number]
--	, (select value FROM system WHERE dbo.system.key_name = 'billing_entity_fax') AS [billing_entity_fax_number]
--	, (select value FROM system WHERE dbo.system.key_name = 'remit_to_street') AS [remit_to_street]
--	, (select value FROM system WHERE dbo.system.key_name = 'remit_to_street2') AS [remit_to_street2]
--	, (select value FROM system WHERE dbo.system.key_name = 'remit_to_city') AS [remit_to_city]
--	, (select value FROM system WHERE dbo.system.key_name = 'remit_to_state') AS [remit_to_state]
--	, (select value FROM system WHERE dbo.system.key_name = 'remit_to_zip') AS [remit_to_zip]
--	, (select value FROM system WHERE dbo.system.key_name = 'remit_to_org_name') AS [remit_to_org_name]

	, dbo.pat.guar_addr AS [guarantor_street] 
	, '' AS [guarantor_street2] 
	, guar_csz.cLNAME AS [guarantor_city]
	, substring(LTRIM(RTRIM(guar_csz.cFNAME)),0,3) AS [guarantor_state]
	, SUBSTRING(LTRIM(RTRIM(guar_csz.cFNAME)),4,5) AS [guarantor_zip]
	, dbo.ConvertName(COALESCE(dbo.pat.guarantor,dbo.pat.pat_full_name)) AS [guarantor_name]

	, dbo.GetAccBalByDate(acc.account,GETDATE()) AS [amount_due]

	, CONVERT(VARCHAR(10),DATEADD(MONTH,1,GETDATE()),101) AS [date_due]
	, dbo.GetAccBalByDate(acc.account, DATEADD(MONTH,-1,GETDATE())) AS [balance_forward]
	, dbo.GetAccBalByDate(acc.account,GETDATE()) AS [aging_bucket_current]
	, dbo.GetAccBalByDate(acc.account, DATEADD(MONTH,-2,GETDATE())) AS [30_Day]
	, dbo.GetAccBalByDate(acc.account, DATEADD(MONTH,-3,GETDATE())) AS [60_Day]
	, dbo.GetAccBalByDate(acc.account, DATEADD(MONTH,-4,GETDATE())) AS [90_Day]
	, dbo.GetAccBalByDate(acc.account,GETDATE()) AS [statement_total_amount] -- update totals when accounts are done.
	, '' AS [insurance_billed_amount]
	, dbo.GetAccBalByDate(acc.account,DATEADD(MONTH,-1,GETDATE())) AS [balance_forward_amount]
	, '' AS [balance_forward_date]
	, (SELECT dbo.GetInsurance.colPlanName FROM dbo.GetInsurance(acc.account) 
		WHERE dbo.GetInsurance.colPriority = 'A' ) AS [primary_health_plan_name]
--	, '' AS [prim_health_plan_policy_number]
--	, '' AS [prim_health_plan_group_number]
--	, '' AS [secondary_health_plan_name]
--	, '' AS [sec_health_plan_policy_number]
--	, '' AS [sec_health_plan_group_number]
--	, '' AS [tertiary_health_plan_name]
--	, '' AS [ter_health_plan_policy_number]
--	, '' AS [ter_health_plan_group_number]
	, CONVERT(VARCHAR,GETDATE(),108) AS [statement_time]
	, '' AS [page_number]
	, 0.00 AS [insurance_pending]
	, dbo.GetAccBalByDate(acc.account,GETDATE()) AS [unpaid_balance]
	, dbo.GetAccBalByDate(acc.account,GETDATE()) AS [patient_balance]-- should be the total of all the account balances on this statement?
	, dbo.GetPaymentsSinceLastDataMailer(acc.account) AS [total_pat_paid_since_last_stmt] -- should be the total of all account using dbo.GetPaymentsSinceLastDataMailer(acc.account) AS [total_pat_paid_since_last_stmt] ?
	, dbo.GetContractualByAccount(acc.account) AS [insurance_discount] -- should be the total of all insurance contractuals?
	, '' AS [contact_text]
	,convert(varchar,GETDATE(),101) + ' '+ convert(varchar,GETDATE(),108) AS [transmission_dt_tm]
	, '' AS [guarantor_country]
	, '' AS [guarantor_ssn]
	,NULLIF(REPLACE(COALESCE(REPLACE(REPLACE(REPLACE(REPLACE(dbo.pat.guar_phone,'(',''),')',''),'-',''),' ',''),
			  REPLACE(REPLACE(REPLACE(REPLACE(dbo.pat.pat_phone,'(',''),')',''),'-',''),' ','')) ,'0000000000',''),'')
			  AS [guarantor_phone]
	,convert(varchar,GETDATE(),101) + ' '+ convert(varchar,GETDATE(),108) AS [statement_submitted_dt_tm]
--	,0 AS [includes_est_pat_liab]
	, dbo.GetAccTotalCharges(acc.account) AS [total_charge_amount] -- sum of all patient accounts?
--	,0 AS [non_covered_charge_amt]
--	,0 AS [ABN_charge_amt]
--	,0 AS [est_contract_allowance_amt_ind]
--	,0 AS [est_contract_allowance_amt]
--	,0 AS [encntr_deductible_rem_amt_ind]
--	,0 AS [deductible_applied_amt]
--	,0 AS [encntr_copay_amt_ind]
--	,0 AS [encntr_copay_amt]
--	,0 AS [encntr_coinsurance_pct_ind]
--	,0 AS [encntr_coinsurance_pct]
--	,0 AS [encntr_coinsurance_amt]
--	,0 AS [maximum_out_of_pocket_amt_ind]
--	,0 AS [amt_over_max_out_of_pocket]
--	,0 AS [est_patient_liab_amt]
--	,'' AS [online_billpay_url]
--	,'' AS [guarantor_access_code]
	FROM         dbo.acc LEFT OUTER JOIN
						  dbo.pat ON dbo.acc.account = dbo.pat.account INNER JOIN
						  dbo.fin ON dbo.acc.fin_code = dbo.fin.fin_code
	CROSS APPLY dbo.ufn_Split_Name(COALESCE(pat.g_city_st,pat.city_st_zip)) AS guar_csz
	WHERE      (acc.status NOT IN ('PAID_OUT', 'CLOSED'))
	AND (acc.trans_date < CONVERT(varchar(10), GETDATE(), 101))
	AND pat.mailer <> 'N'
	AND dbo.GetAccBalByDate(acc.account, GETDATE()) <> 0.00
	--AND CONVERT(NUMERIC(18,2),dbo.GetAccBalByDate(acc.account, GETDATE())) BETWEEN '0.01' AND '10.00' -- HTML work queue on small balances eventually
	AND guar_csz.cLNAME  is NULL -- HTML work queue on this eventually
	--AND ABS(CHECKSUM(QUOTENAME(UPPER(COALESCE(dbo.pat.guarantor,dbo.pat.pat_full_name))) )) )	 is NULL
	

) --SELECT * FROM cte
SELECT 
'STMT' AS [record_type]
, ROW_NUMBER() OVER (ORDER BY cte.id) AS [record_cnt]
--, (DATEPART(MONTH,GETDATE())*10000)+ROW_NUMBER() OVER (ORDER BY cte.id) AS [statement_number]
--, SUM(cte.amount_due) OVER (PARTITION BY cte.id) AS [amt]
--,cte.billing_entity_street ,
--cte.billing_entity_city ,
--cte.billing_entity_state ,
--cte.billing_entity_zip ,
--cte.billing_entity_federal_tax_id ,
--cte.billing_entity_name ,
--cte.billing_entity_phone_number ,
--cte.billing_entity_fax_number ,
--cte.remit_to_street ,
--cte.remit_to_street2 ,
--cte.remit_to_city ,
--cte.remit_to_state ,
--cte.remit_to_zip ,
--cte.remit_to_org_name ,
,cte.guarantor_street ,
cte.guarantor_street2 ,
cte.guarantor_city ,
cte.guarantor_state ,
cte.guarantor_zip ,
cte.guarantor_name ,
SUM(cte.amount_due) OVER (PARTITION BY cte.id) AS [amount_due] ,
cte.date_due ,
SUM(cte.balance_forward) OVER (PARTITION BY cte.id) AS [balance_forward]  ,
SUM(cte.aging_bucket_current) OVER (PARTITION BY cte.id) AS [aging_bucket_current] ,
SUM(cte.[30_Day]) OVER (PARTITION BY cte.id) AS [30_Day],
SUM(cte.[60_Day]) OVER (PARTITION BY cte.id) AS [60_Day],
SUM(cte.[90_Day]) OVER (PARTITION BY cte.id) AS [90_Day] ,
SUM(cte.statement_total_amount) OVER (PARTITION BY cte.id) AS [ statement_total_amount] ,
cte.insurance_billed_amount ,
SUM(cte.balance_forward_amount)  OVER (PARTITION BY cte.id) AS [balance_forward_amount] ,
convert(varchar,GETDATE(), 101) as [balance_forward_date] ,
cte.primary_health_plan_name ,
--cte.prim_health_plan_policy_number ,
--cte.prim_health_plan_group_number ,
--cte.secondary_health_plan_name ,
--cte.sec_health_plan_policy_number ,
--cte.sec_health_plan_group_number ,
--cte.tertiary_health_plan_name ,
--cte.ter_health_plan_policy_number ,
--cte.ter_health_plan_group_number ,
--cte.statement_time ,
--cte.page_number ,
--cte.insurance_pending ,
		SUM(cte.unpaid_balance) OVER (PARTITION BY cte.id) AS [unpaid_balance],
		SUM(cte.patient_balance) OVER (PARTITION BY cte.id) AS [patient_balance] ,
		SUM(cte.total_pat_paid_since_last_stmt) OVER (PARTITION BY cte.id) AS [total_pat_paid_since_last_stmt] ,
		SUM(cte.insurance_discount) OVER (PARTITION BY cte.id) AS [insurance_discount] ,
--		cte.contact_text ,
--		cte.transmission_dt_tm ,
--		cte.guarantor_country ,
		cte.guarantor_ssn ,
		cte.guarantor_phone ,
--		cte.statement_submitted_dt_tm ,
--		cte.includes_est_pat_liab ,
		SUM(cte.total_charge_amount) OVER (PARTITION BY cte.id) AS [total_charge_amount] 
--		cte.non_covered_charge_amt ,
--		cte.ABN_charge_amt ,
--		cte.est_contract_allowance_amt_ind ,
--		cte.est_contract_allowance_amt ,
--		cte.encntr_deductible_rem_amt_ind ,
--		cte.deductible_applied_amt ,
--		cte.encntr_copay_amt_ind ,
--		cte.encntr_copay_amt ,
--		cte.encntr_coinsurance_pct_ind ,
--		cte.encntr_coinsurance_pct ,
--		cte.encntr_coinsurance_amt ,
--		cte.maximum_out_of_pocket_amt_ind ,
--		cte.amt_over_max_out_of_pocket ,
--		cte.est_patient_liab_amt ,
--		cte.online_billpay_url ,
--		cte.guarantor_access_code
		 
FROM cte
WHERE rn  = 1

END

