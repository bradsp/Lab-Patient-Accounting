
-- =============================================
-- Author:		David
-- Create date: 5/6/2015
-- Description:	inserts account into the table dbo.patbill_acc
-- for the statements.
-- work file P:\wkelly\SQL Server Management Studio\Projects\PAT_BILL\patbill_master.sql
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_pat_bill_acct] 
	-- Add the parameters for the stored procedure here
	@batchDate datetime = 0, -- this is the date the patbill was run 
	@endDate datetime = 0, -- this is the last day of the month the batch was for
	@batch VARCHAR(50) = '', -- this is the YYYYMM the batch was for.
	@batchCount INT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
; WITH cteAcc
AS
(
	SELECT  TOP(100) PERCENT
	
	/*statement fields
	'STMT as [record_type]
	[record_cnt] get outside cteAcc*/
	

	(CAST(DATEPART(YEAR,@endDate) AS FLOAT)*10000000 ) +
		(CAST (DATEPART(MONTH,@endDate) AS FLOAT)*100000) +
		(@batchCount*10000)+ -- if nothing is passed this is 0
		(CAST(DENSE_RANK() 
		OVER (ORDER BY acc.pat_name, acc.dob_yyyy, acc.ssn ) AS FLOAT))
		
	AS [statement_number]


	/*account fields*/ 
	,CONVERT(VARCHAR,
			ROW_NUMBER() 
			OVER (PARTITION BY acc.pat_name, acc.dob_yyyy
				, acc.ssn ,acc.sex
			ORDER BY acc.pat_name, acc.trans_date )
		) AS [record_cnt_acct] 
		
	, ISNULL(COALESCE( 
		NULLIF(acc.HNE_NUMBER,''), 
		NULLIF(acc.mri,''),
		NULLIF(acc.guarantorID,'')),acc.account) as [patient_account_number]

	, acc.account AS [account_id]
	, acc.pat_name
	,'SUBTOTAL' AS [account_subtotal]
	, dbo.GetAccBalByDate(acc.account, GETDATE() )  AS [total_account_subtotal]
	, dbo.GetAccBalByDate(acc.account, GETDATE() )  AS [acct_amt_due]

	, 0.00 AS [acct_ins_pending]
	, CONVERT(VARCHAR,acc.trans_date,101) AS [acct_dates_of_service]
	, dbo.GetAccBalByDate(acc.account, GETDATE() )  AS [acct_unpaid_bal]
	, dbo.GetAccBalByDate(acc.account, GETDATE() )  AS [acct_patient_bal]
	, dbo.GetPaymentsSinceLastDataMailer(acc.account) AS [acct_paid_since_last_stmt]
	, dbo.GetContractualByAccount(acc.account) AS [acct_ins_discount]
	, CONVERT(VARCHAR,DATEADD(MONTH,1,GETDATE()),101) AS [acct_date_due]
	, (SELECT dbo.GetInsurance.colPlanName FROM dbo.GetInsurance(acc.account) 
		WHERE dbo.GetInsurance.colPriority = 'A' ) AS [acct_health_plan_name]
	, CONVERT(VARCHAR,acc.dob_yyyy,101) AS [patient_date_of_birth]
	, '' AS [patient_date_of_death]
	, CASE WHEN UPPER(acc.sex) IN ('M','MALE') THEN 'MALE'
		   WHEN UPPER(acc.sex) IN ('F','FEMALE') THEN 'FEMALE'
		   ELSE NULL 
	   END AS [patient_sex]
	, '' AS [patient_vip]
	, 0 AS [includes_est_pat_lib]
	, dbo.GetAccTotalCharges(acc.account) AS [total_charge_amt]
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

	, dbo.GetDaysSinceLastPayment(acc.account) AS [acc_msg]	
		
	, acc.mailer
	, acc.first_dm AS [first_data_mailer]
	, acc.last_dm AS [last_data_mailer]
	, DATEDIFF(MONTH,acc.first_dm,acc.last_dm) AS [mailer_count]
	, CONVERT(DATETIME,convert(varchar,GETDATE(),101)) AS [processed_date]
	, CONVERT(DATETIME,null,101) AS [date_sent]
	, CONVERT(VARCHAR(4),YEAR(@endDate ))+
              CONVERT(VARCHAR(2),@endDate,101 ) AS [batch_id]
    , CASE WHEN DATEDIFF(DAY,acc.trans_date,GETDATE()) <= 30 
		   THEN ISNULL( dbo.GetAccBalByDate(acc.account, GETDATE() ),0.00)
		   ELSE 0.00
		END AS [stmt_aging_bucket_current]
	
	, CASE WHEN DATEDIFF(DAY,acc.trans_date,GETDATE()) BETWEEN 31 AND 60 
		   THEN ISNULL(dbo.GetAccBalByDate(acc.account, GETDATE() ),0.00)
		   ELSE 0.00
		END AS [stmt_aging_bucket_30_day]
	, CASE WHEN DATEDIFF(DAY,acc.trans_date,GETDATE()) BETWEEN 61 AND 90 
		   THEN ISNULL(dbo.GetAccBalByDate(acc.account, GETDATE() ),0.00)
		   ELSE 0.00
		END AS [stmt_aging_bucket_60_day]
	
	, CASE WHEN DATEDIFF(DAY,acc.trans_date,GETDATE()) > 91
		   THEN ISNULL(dbo.GetAccBalByDate(acc.account, GETDATE() ),0.00)
		   ELSE 0.00
		END AS [stmt_aging_bucket_90_day]
	FROM dbo.vw_pat_bill_cerner2 AS acc
	WHERE acc.last_dm = @batchDate	
	AND acc.[Current Balance] > 0.00
	

	
)  --select * from cteAcc
INSERT INTO dbo.patbill_acc
	(
		statement_number ,
		record_cnt_acct ,
		patient_account_number ,
		account_id ,
		pat_name ,
		account_subtotal ,
		total_account_subtotal ,
		acct_amt_due ,
		acct_ins_pending ,
		acct_dates_of_service ,
		acct_unpaid_bal ,
		acct_patient_bal ,
		acct_paid_since_last_stmt ,
		acct_ins_discount ,
		acct_date_due ,
		acct_health_plan_name ,
		patient_date_of_birth ,
		patient_date_of_death ,
		patient_sex ,
		patient_vip ,
		includes_est_pat_lib ,
		total_charge_amt ,
		non_covered_charge_amt ,
		ABN_charge_amt ,
		est_contract_allowance_amt_ind ,
		est_contract_allowance_amt ,
		encntr_deductible_rem_amt_ind ,
		deductible_applied_amt ,
		encntr_copay_amt_ind ,
		encntr_copay_amt ,		
		encntr_coinsurance_pct_ind ,
		encntr_coinsurance_pct ,
		encntr_coinsurance_amt ,
		maximum_out_of_pocket_amt_ind ,
		amt_over_max_out_of_pocket ,
		est_patient_liab_amt ,
		acc_msg ,
		mailer ,
		first_data_mailer ,
		last_data_mailer ,
		mailer_count ,
		processed_date ,
		date_sent ,
		--statement_id,
		aging_bucket_current,
		aging_bucket_30,
		aging_bucket_60,
		aging_bucket_90,
		batch_id
	)
SELECT cteAcc.statement_number ,
		cteAcc.record_cnt_acct ,
		cteAcc.patient_account_number ,
		cteAcc.account_id ,
		cteAcc.pat_name ,
		cteAcc.account_subtotal ,
		cteAcc.total_account_subtotal ,
		cteAcc.acct_amt_due ,
		cteAcc.acct_ins_pending ,
		cteAcc.acct_dates_of_service ,
		cteAcc.acct_unpaid_bal ,
		cteAcc.acct_patient_bal ,
		cteAcc.acct_paid_since_last_stmt ,
		cteAcc.acct_ins_discount ,
		cteAcc.acct_date_due ,
		cteAcc.acct_health_plan_name ,
		cteAcc.patient_date_of_birth ,
		cteAcc.patient_date_of_death ,
		cteAcc.patient_sex ,
		cteAcc.patient_vip ,
		cteAcc.includes_est_pat_lib ,
		cteAcc.total_charge_amt ,
		cteAcc.non_covered_charge_amt ,
		cteAcc.ABN_charge_amt ,		
		cteAcc.est_contract_allowance_amt_ind ,
		cteAcc.est_contract_allowance_amt ,
		cteAcc.encntr_deductible_rem_amt_ind ,
		cteAcc.deductible_applied_amt ,
		cteAcc.encntr_copay_amt_ind ,
		cteAcc.encntr_copay_amt ,		
		cteAcc.encntr_coinsurance_pct_ind ,
		cteAcc.encntr_coinsurance_pct ,
		cteAcc.encntr_coinsurance_amt ,
		cteAcc.maximum_out_of_pocket_amt_ind ,
		cteAcc.amt_over_max_out_of_pocket ,
		cteAcc.est_patient_liab_amt ,
		
		CASE WHEN cteAcc.acc_msg BETWEEN 0 AND 30	THEN 'IPPACCEPT' -- thanks for the check
			 WHEN cteAcc.acc_msg BETWEEN 31 AND 120 THEN 'INSSP1'
			 WHEN cteAcc.acc_msg  >= 121			THEN 'INSSP2'
			 ELSE 'INSSP1'
		END AS [acc_msg],
		cteAcc.mailer ,
		cteAcc.first_data_mailer ,
		cteAcc.last_data_mailer ,
		cteAcc.mailer_count ,
		cteAcc.processed_date ,
		cteAcc.date_sent ,
		--cteAcc.batch_id,
		cteAcc.stmt_aging_bucket_current,
		cteAcc.stmt_aging_bucket_30_day,
		cteAcc.stmt_aging_bucket_60_day,
		cteAcc.stmt_aging_bucket_90_day,
		@batch
FROM cteAcc
LEFT outer JOIN dbo.patbill_acc  pb 
	ON pb.batch_id = cteAcc.batch_id
	AND pb.account_id = cteAcc.account_id
WHERE pb.batch_id IS NULL AND pb.date_sent IS null
AND pb.account_id IS null
END

