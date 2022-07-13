-- =============================================
-- Author:		David Kelly
-- Create date: 12/22/2014
-- Description:	patterned after Bradleys usp_accounting_summary_cumulative
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_summary_cumulative_by_selected_insurance] 
	-- Add the parameters for the stored procedure here
	@startdate DATETIME, 
	@enddate DATETIME,
	@ins VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE #icharges
	(
		insurance VARCHAR(20),
		client MONEY,
		thirdparty MONEY
	) 
	CREATE TABLE #ipayments
	(
		insurance VARCHAR(20),
		client MONEY DEFAULT '0.00',
		thirdparty MONEY DEFAULT '0.00'
	)
	CREATE TABLE #icontractual
	(
		insurance VARCHAR(20),
		client MONEY,
		thirdparty MONEY
	) 
	CREATE TABLE #iwriteoff
	(
		insurance VARCHAR(20),
		client MONEY,
		thirdparty MONEY
	) 

-- create the master table the other tables can get insurance info from
	; WITH cteAccIns
	AS
	(
	SELECT acc.account AS [ACCOUNT], acc.fin_code, acc.trans_date, acc.status
	, CASE WHEN acc.fin_code IN ('CLIENT','W','X','Y','Z') 
			OR ins.ins_code IN ('CLIENT','W','X','Y','Z') then 'CLIENT'
		   WHEN COALESCE(NULLIF(ins.ins_code,''),acc.fin_code) = 'E' 
			OR ins.ins_code = 'SP' THEN 'SP'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'B' 
			OR ins.ins_code = 'BC' THEN 'BC'		   
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'A' 
		    OR ins.ins_code = 'MC' THEN 'MC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'D' 
			OR ins.ins_code  = 'TNBC' THEN 'TNBC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'M' 
			OR ins.ins_code = 'AM' THEN 'AM'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'S'
		    OR ins.ins_code = 'MISC' THEN 'MISC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'H' 
			OR ins.ins_code = 'COMM.H' THEN 'COMM.H'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'L' 
			OR ins.ins_code = 'COMM.L' THEN 'COMM.L'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'AETNA' THEN 'AETNA'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'UHC' THEN 'UHC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'AM' THEN 'AM'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'CIGNA' THEN 'CIGNA'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'TNBC' THEN 'TNBC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'WIN' THEN 'WIN'
		   --else 'OTHER' END AS ins_code
		   ELSE COALESCE(NULLIF(ins.ins_code,''),acc.fin_code) END AS ins_code
		   , CASE WHEN bd.account_no IS NOT NULL AND bd.date_sent IS NOT NULL
			THEN 'true' 
			ELSE NULL END AS [bad debt]
			, CASE WHEN bd.account_no IS NOT NULL AND bd.date_sent IS NULL
			THEN 'true' 
			ELSE NULL END AS [pre bad debt]
			
		FROM dbo.acc
		LEFT OUTER JOIN ins ON dbo.ins.account = dbo.acc.account AND ins.ins_a_b_c = 'A'
		LEFT OUTER JOIN dbo.bad_debt bd ON bd.account_no = acc.account 
			--AND dbo.bd.date_sent IS NOT null
		WHERE acc.trans_date >= DATEADD(year,-5,@startDate) 
	)
	SELECT * 
	INTO ##cteAccIns
	FROM cteAccIns
	--ORDER BY cteAccIns.ins_code

	-- query to pull recovery amount by insurance

SELECT  COALESCE(cteAccIns.ins_code,'UNKNOWN') AS [Insurance],

	 SUM(dbo.chk.amt_paid) AS [RECOVERY]
	INTO #irecovery
	FROM chk 
	LEFT outer JOIN ##cteAccIns AS cteAccIns ON cteAccIns.account = dbo.chk.comment
	WHERE chk.account = 'baddebt' AND dbo.chk.status <> 'refund'
	AND dbo.chk.mod_date BETWEEN @startDate AND @endDate
	GROUP BY COALESCE(cteAccIns.ins_code,'UNKNOWN')


--SELECT * FROM #irecovery
--return
	
	INSERT INTO #icharges (#icharges.insurance, #icharges.client, #icharges.thirdparty)
	EXEC usp_prg_charges_by_selected_Insurance @startdate, @enddate
--SELECT * FROM #icharges	
--WHERE #icharges.insurance = @ins
--ORDER BY #icharges.insurance
--RETURN;

INSERT INTO #ipayments
	EXEC dbo.usp_prg_amt_paid_by_selected_insurance @startdate, @enddate
--SELECT * FROM #ipayments
--WHERE #ipayments.insurance = @ins
--RETURN;
	

	INSERT INTO #icontractual
	EXEC dbo.usp_prg_contractual_by_selected_insurance @startdate, @enddate
--SELECT * FROM #icontractual
--WHERE #icontractual.insurance = @ins
--RETURN	

	INSERT INTO #iwriteoff
	EXEC dbo.usp_prg_write_off_by_selected_insurance @startdate, @enddate
--SELECT * FROM #iwriteoff
--WHERE #iwriteoff.insurance = @ins
--RETURN;



	SELECT 
		COALESCE(#icharges.insurance,#ipayments.insurance,#icontractual.insurance,#iwriteoff.insurance) AS [INSURANCE]
		,ISNULL(#icharges.thirdparty,0.00) AS [Third Party Charges]
		,ISNULL(#icontractual.thirdparty,0.00) AS [Deductions from Charges]
		,ISNULL(#ipayments.thirdparty,0.00) AS [Payments]
		,ISNULL(#irecovery.RECOVERY,0.00) AS [Recovery]
		
		,CASE WHEN NULLIF(#icontractual.thirdparty,0.00) IS NOT NULL 
		AND NULLIF(#icharges.thirdparty,0.00) IS NOT NULL
		THEN		 
			#icontractual.thirdparty
			/	#icharges.thirdparty
			else 0.00 end 
			AS [Percent Deduct from Charges]
		
			
		,ISNULL(#icharges.client,0.00) AS [Client Charges],
		ISNULL(#ipayments.client,0.00) AS [Client Payments],
		ISNULL(#icharges.thirdparty,0.00)+ISNULL(#icharges.client,0.00) AS [Total Revenue]
		
--		,(ISNULL(#irecovery.RECOVERY,0.00)+ISNULL(#ipayments.client,0.00)+ISNULL(#ipayments.thirdParty,0.00))
--		/ (ISNULL(NULLIF( ISNULL(#icharges.client,0.00)+ISNULL(#icharges.thirdParty,0.00),0.00),1)) AS [Percent Coll of All Charges]
		
		, (ISNULL(#irecovery.RECOVERY,0.00)+ISNULL(#ipayments.client,0.00)+ISNULL(#ipayments.thirdParty,0.00))
		/SUM(ISNULL(#icharges.thirdparty,0.00)+ISNULL(#icharges.client,0.00))
			OVER (PARTITION BY 1) AS [Percent Coll of All Charges]
		
	FROM #icharges
	FULL OUTER JOIN #ipayments ON #ipayments.insurance = #icharges.insurance
	FULL OUTER JOIN #icontractual ON #icontractual.insurance = #icharges.insurance
	FULL OUTER JOIN #iwriteoff ON #iwriteoff.insurance = #icharges.insurance
	FULL OUTER JOIN #irecovery ON #irecovery.insurance = #icharges.insurance
	WHERE #icharges.insurance = @ins
	ORDER BY COALESCE(#icharges.insurance,#ipayments.insurance,#icontractual.insurance,#iwriteoff.insurance)

	DROP TABLE #icharges
	DROP TABLE #ipayments
	DROP TABLE #icontractual
	DROP TABLE #iwriteoff
	DROP TABLE #irecovery
	
	DROP TABLE ##cteAccIns
END
