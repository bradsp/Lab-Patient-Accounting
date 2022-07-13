-- =============================================
-- Author:		David Kelly
-- Create date: 1/21/2015
-- Description:	patterned after Bradleys usp_accounting_summary_cumulative
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_summary_cumulative_by_client] 
	-- Add the parameters for the stored procedure here
	@startdate DATETIME, 
	@enddate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE #icharges
	(
		client VARCHAR(20),
		clientAmt MONEY DEFAULT '0.00',
		thirdpartyAmt MONEY DEFAULT '0.00'
	) 
	CREATE TABLE #ipayments
	(
		client VARCHAR(20),
		clientAmt MONEY DEFAULT '0.00',
		thirdpartyAmt MONEY DEFAULT '0.00'
	)
	CREATE TABLE #icontractual
	(
		client VARCHAR(20),
		clientAmt MONEY DEFAULT '0.00',
		thirdpartyAmt MONEY DEFAULT '0.00'
	) 
	CREATE TABLE #iwriteoff
	(
		client VARCHAR(20),
		clientAmt MONEY  DEFAULT '0.00',
		thirdpartyAmt MONEY  DEFAULT '0.00'
	) 

-- create the master table the other tables can get insurance info from
	; WITH cteAccIns
	AS
	(
	SELECT acc.account AS [ACCOUNT], acc.cl_mnem AS [CLIENT], acc.fin_code, acc.trans_date, acc.status
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
		   else 'OTHER' END AS ins_code
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
		WHERE acc.trans_date >= DATEADD(year,-12,@startDate) 
	)
	SELECT * 
	INTO ##cteAccIns
	FROM cteAccIns
	-- query to pull recovery amount by client
--SELECT * FROM ##cteAccIns
--RETURN;

SELECT
	cteAccIns.CLIENT,  
	CASE WHEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		IN ('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
		THEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		ELSE 'OTHER' END AS [INSURANCE],
	 SUM(dbo.chk.amt_paid) AS [RECOVERY]
	INTO #irecovery
	FROM chk 
	LEFT outer JOIN ##cteAccIns AS cteAccIns ON cteAccIns.account = dbo.chk.comment
	WHERE chk.account = 'baddebt' AND dbo.chk.status <> 'refund'
	AND dbo.chk.mod_date BETWEEN @startDate AND @endDate
	GROUP BY cteAccIns.CLIENT,
	CASE WHEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		IN ('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
		THEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		ELSE 'OTHER' END
	HAVING CASE WHEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		IN ('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
		THEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		ELSE 'OTHER' END IS NOT NULL

--SELECT * FROM #irecovery
--return

	INSERT INTO #icharges (#icharges.client, #icharges.clientAmt, #icharges.thirdpartyAmt)
	EXEC usp_prg_charges_by_client @startdate, @enddate
	
--SELECT * FROM #icharges	
--RETURN;

INSERT INTO #ipayments
	EXEC dbo.usp_prg_amt_paid_by_client @startdate, @enddate

--SELECT * FROM #ipayments
--RETURN;
	

	INSERT INTO #icontractual
	EXEC dbo.usp_prg_contractual_by_client @startdate, @enddate
--SELECT * FROM #icontractual
--return

	INSERT INTO #iwriteoff
	EXEC dbo.usp_prg_write_off_by_client @startdate, @enddate
--SELECT * FROM #iwriteoff
--RETURN;

	SELECT 
		COALESCE(#icharges.client,#ipayments.client,#icontractual.client,#iwriteoff.client,'UNK') AS [CLIENT]
		,ISNULL(#icharges.thirdpartyAmt,0.00) AS [Third Party Charges]
		,ISNULL(#icontractual.thirdpartyAmt,0.00) AS [Deductions from Charges]
		,ISNULL(#ipayments.thirdpartyAmt,0.00) AS [Payments]
		,ISNULL(#irecovery.RECOVERY,0.00) AS [Recovery]
		
		,CASE WHEN NULLIF(#icontractual.thirdpartyAmt,0.00) IS NOT NULL 
		AND NULLIF(#icharges.thirdpartyAmt,0.00) IS NOT NULL
		THEN		 
			#icontractual.thirdpartyAmt
			/	#icharges.thirdpartyAmt
			else 0.00 end 
			AS [Percent Deduct from Charges]
		
			
		,ISNULL(#icharges.clientAmt,0.00) AS [Client Charges],
		ISNULL(#ipayments.clientAmt,0.00) AS [Client Payments],
		ISNULL(#icharges.thirdpartyAmt,0.00)+ISNULL(#icharges.clientAmt,0.00) AS [Total Revenue]
		
--		,(ISNULL(#irecovery.RECOVERY,0.00)+ISNULL(#ipayments.client,0.00)+ISNULL(#ipayments.thirdParty,0.00))
--		/ (ISNULL(NULLIF( ISNULL(#icharges.client,0.00)+ISNULL(#icharges.thirdParty,0.00),0.00),1)) AS [Percent Coll of All Charges]
		
		, (ISNULL(#irecovery.RECOVERY,0.00)+ISNULL(#ipayments.clientAmt,0.00)+ISNULL(#ipayments.thirdPartyAmt,0.00))
		/SUM(ISNULL(#icharges.thirdpartyAmt,0.00)+ISNULL(#icharges.clientAmt,0.00))
			OVER (PARTITION BY 1) AS [Percent Coll of All Charges]
		
	FROM #icharges
	FULL OUTER JOIN #ipayments ON #ipayments.client = #icharges.client
	FULL OUTER JOIN #icontractual ON #icontractual.client = #icharges.client
	FULL OUTER JOIN #iwriteoff ON #iwriteoff.client = #icharges.client
	FULL OUTER JOIN #irecovery ON #irecovery.client = #icharges.client
	ORDER BY 
	COALESCE(#icharges.client,#ipayments.client,#icontractual.client,#iwriteoff.client,'UNK')

	DROP TABLE #icharges
	DROP TABLE #ipayments
	DROP TABLE #icontractual
	DROP TABLE #iwriteoff
	DROP TABLE #irecovery
	
	DROP TABLE ##cteAccIns
END
