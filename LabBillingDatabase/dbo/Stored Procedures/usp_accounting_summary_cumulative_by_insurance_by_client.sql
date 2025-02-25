﻿-- =============================================
-- Author:		David Kelly
-- Create date: 12/22/2014
-- Description:	patterned after Bradleys usp_accounting_summary_cumulative
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_summary_cumulative_by_insurance_by_client] 
	-- Add the parameters for the stored procedure here
	@startdate DATETIME, 
	@enddate DATETIME,
	@client VARCHAR(10)
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
-- because this one if for a particular client the left outer joins to the ##cteAccIns
-- have been converted to inner joins
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
		AND acc.cl_mnem = @client
	)
	SELECT * 
	INTO ##cteAccIns
	FROM cteAccIns

	-- query to pull recovery amount by insurance

SELECT  
	CASE WHEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		IN ('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
		THEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		ELSE 'OTHER' END AS [INSURANCE],
	 SUM(dbo.chk.amt_paid) AS [RECOVERY]
	INTO #irecovery
	FROM chk 
	INNER JOIN ##cteAccIns AS cteAccIns ON cteAccIns.account = dbo.chk.comment
	WHERE chk.account = 'baddebt' AND dbo.chk.status <> 'refund'
	AND dbo.chk.mod_date BETWEEN @startDate AND @endDate
	GROUP BY CASE WHEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		IN ('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
		THEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		ELSE 'OTHER' END
	HAVING CASE WHEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		IN ('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
		THEN COALESCE(NULLIF(chk.ins_code,''),cteAccIns.ins_code)
		ELSE 'OTHER' END IS NOT NULL

--SELECT * FROM #irecovery
--return
	
	INSERT INTO #icharges (#icharges.insurance, #icharges.client, #icharges.thirdparty)
	EXEC usp_prg_charges_by_Insurance_by_client @startdate, @enddate
--SELECT * FROM #icharges	
--RETURN;

INSERT INTO #ipayments
	EXEC dbo.usp_prg_amt_paid_by_insurance_by_client @startdate, @enddate
--SELECT * FROM #ipayments
--RETURN;
	

	INSERT INTO #icontractual
	EXEC dbo.usp_prg_contractual_by_insurance_by_client @startdate, @enddate
--SELECT * FROM #icontractual
--return

	INSERT INTO #iwriteoff
	EXEC dbo.usp_prg_write_off_by_insurance_by_client @startdate, @enddate
--SELECT * FROM #iwriteoff
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
	ORDER BY COALESCE(#icharges.insurance,#ipayments.insurance,#icontractual.insurance,#iwriteoff.insurance)

	DROP TABLE #icharges
	DROP TABLE #ipayments
	DROP TABLE #icontractual
	DROP TABLE #iwriteoff
	DROP TABLE #irecovery
	
	DROP TABLE ##cteAccIns
END
