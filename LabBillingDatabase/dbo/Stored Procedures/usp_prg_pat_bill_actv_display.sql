
-- =============================================
-- Author:		David
-- Create date: 5/6/2015
-- Description:	inserts account into the table dbo.patbill_acc
-- for the statements.
-- work file P:\wkelly\SQL Server Management Studio\Projects\PAT_BILL\patbill_master.sql
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_pat_bill_actv_display] 
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
 
; WITH cteChrg
AS
(
	SELECT chrg.account, chrg_num, chrg.cdm, SUM(qty) AS [qty]
	FROM dbo.chrg
	INNER join dbo.patbill_acc pa ON pa.account_id
	= chrg.account
	GROUP BY chrg.account, chrg.cdm, chrg_num
	HAVING SUM(qty) <> 0
)
,cteActv
AS
(
	SELECT TOP(100) PERCENT
		
		E.statement_number ,
		'ACTV' AS [record_type],
		E.record_cnt_acct ,
		E.patient_account_number AS [enctr_nbr],
		E.account_id AS [activity_id],
		'' AS [activity_type],
		--chrg.location,
		CONVERT(VARCHAR(10),chrg.service_date,101) AS [activity_date],
		'LABORTORY' AS [activity_description],
		'' AS [activity_code],
		'' AS [activity_amount]
		,chrg.qty AS [units]
		--SUM(chrg.qty*dbo.chrg.net_amt) OVER (PARTITION BY chrg.account,chrg.cdm) AS [chrg_amt]
		, dbo.amt.cpt4 AS [cpt_code]
		, cpt4.descript AS [cpt_description]
		, '' AS [drg_code]
		, amt.revcode AS [revenue_code]
		,'' AS [revenue_code_description]
		, '' AS [hcpcs_code]
		, '' AS [hcpcs_description]
		, '' AS [order_mgmt_activity_type]
		, amt.amount AS [activity_amount_due]
		, CONVERT(VARCHAR(10), chrg.service_date,101) AS [activity_date_of_service]				
		, 0.00 AS [activity_patient_bal]
		, 0.00 AS [activity_ins_discount]
		, 'CHARGE' AS [activity_trans_type]
		, '' AS [activity_trans_sub_type]
		, chrg.qty*amt.amount AS [activity_trans_amount]
		, '' AS [activity_health_plan_name]	
		, 0.00 AS [activity_ins_pending]
		, CASE WHEN chrg.qty >= 0 THEN  1 
				ELSE -1 END AS [activity_dr_cr_flag]
		--, chrg.cdm AS [parent_activity_id]
		,E.record_cnt_acct AS [parent_activity_id]
	--,	chrg.cdm
	FROM dbo.patbill_acc E	
	INNER JOIN chrg ON chrg.account = E.account_id
		AND dbo.chrg.credited = 0
	INNER JOIN cteChrg 
		ON cteChrg.account = dbo.chrg.account
			AND cteChrg.cdm = dbo.chrg.cdm 
			AND cteChrg.chrg_num = dbo.chrg.chrg_num
	INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num
	INNER JOIN cpt4 ON dbo.cpt4.cdm = dbo.chrg.cdm AND dbo.cpt4.cpt4 = dbo.amt.cpt4
	WHERE E.batch_id = @batch
	ORDER BY E.statement_number, E.record_cnt_acct
	
)  --SELECT * FROM cteActv

--INSERT INTO dbo.patbill_enctr_actv
--		(
--			statement_number ,
--			record_type ,
--			record_cnt ,
--			enctr_nbr ,
--			activity_id ,
--			activity_type ,
--			activity_date ,
--			activity_description ,
--			activity_code ,
--			activity_amount ,
--			units ,
--			cpt_code ,
--			cpt_description ,
--			drg_code ,
--			revenue_code ,
--			revenue_code_description ,
--			hcpcs_code ,
--			hcpcs_description ,
--			order_mgmt_activity_type ,
--			activity_amount_due ,
--			activity_date_of_service ,
--			activity_patient_bal ,
--			activity_ins_discount ,
--			activity_trans_type ,
--			activity_trans_sub_type ,
--			activity_trans_amount ,
--			activity_health_plan_name ,
--			activity_ins_pending ,
--			activity_dr_cr_flag ,
--			parent_activity_id,
--			batch_id
--		)


SELECT cteActv.statement_number ,
		cteActv.record_type ,
		ROW_NUMBER() OVER (PARTITION BY 
			cteActv.statement_number, cteActv.parent_activity_id 
			ORDER BY 
			cteActv.statement_number, cteActv.parent_activity_id)
			AS [record_cnt],
		cteActv.enctr_nbr ,
		cteActv.activity_id ,
		cteActv.activity_type ,
		cteActv.activity_date ,
		cteActv.activity_description ,
		cteActv.activity_code ,
		cteActv.activity_amount ,
		cteActv.units ,
		cteActv.cpt_code ,
		cteActv.cpt_description ,
		cteActv.drg_code ,
		cteActv.revenue_code ,
		cteActv.revenue_code_description ,
		cteActv.hcpcs_code ,
		cteActv.hcpcs_description ,
		cteActv.order_mgmt_activity_type ,
		cteActv.activity_amount_due ,
		cteActv.activity_date_of_service ,
		cteActv.activity_patient_bal ,
		cteActv.activity_ins_discount ,
		cteActv.activity_trans_type ,
		cteActv.activity_trans_sub_type ,
		cteActv.activity_trans_amount ,
		cteActv.activity_health_plan_name ,
		cteActv.activity_ins_pending ,
		cteActv.activity_dr_cr_flag ,
		cteActv.parent_activity_id 

FROM cteActv
ORDER BY cteActv.statement_number





END

