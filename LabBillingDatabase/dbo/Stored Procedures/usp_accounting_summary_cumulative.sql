-- =============================================
-- Author:		Bradley Powers
-- Create date: 9/10/2014
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_summary_cumulative] 
	-- Add the parameters for the stored procedure here
	@startdate DATETIME, 
	@enddate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE #charges
	(
		glaccount VARCHAR(20),
		client MONEY,
		thirdparty MONEY
	) 
	CREATE TABLE #payments
	(
		glaccount VARCHAR(20),
		client MONEY,
		thirdparty MONEY
	)
	CREATE TABLE #contractual
	(
		glaccount VARCHAR(20),
		client MONEY,
		thirdparty MONEY
	) 
	CREATE TABLE #writeoff
	(
		glaccount VARCHAR(20),
		client MONEY,
		thirdparty MONEY
	) 

	-- query to pull recovery amount by cost center
	SELECT  *
	INTO #recovery
	FROM    ( SELECT    CASE WHEN client.type IS NULL
							 THEN 'thirdparty'
							 ELSE 'client'
						END AS [pay_type] ,
						CASE WHEN acc_location.location = 'LIFT' THEN '7007'
							 ELSE COALESCE(client2.gl_code, '7009')
						END AS [glaccount] ,
						SUM(ISNULL(amt_paid, 0)) AS [paid]
			  FROM      chk
						LEFT OUTER JOIN client ON client.cli_mnem = chk.account
						LEFT OUTER JOIN acc ON chk.comment = acc.account
						LEFT OUTER JOIN dbo.acc_location ON acc_location.account = acc.account
						LEFT OUTER JOIN client client2 ON acc.cl_mnem = client2.cli_mnem
			  WHERE     chk.mod_date BETWEEN @startDate AND @endDate
						AND chk.status <> 'REFUND' AND chk.account = 'BADDEBT'
			  GROUP BY  client.type ,
						client2.type ,
						acc.cl_mnem ,
						dbo.acc_location.location, 
						client2.gl_code ,
						acc.account ,
						chk.status ,
						bad_debt ,
						chk.source ,
						chk.account
			) s PIVOT
	( SUM(paid) FOR pay_type IN ( [client], [thirdparty] ) )
	AS pvt
	ORDER BY glaccount

	INSERT INTO #charges
	EXEC usp_prg_charges_by_cost_centers @startdate, @enddate

	INSERT INTO #payments
	EXEC dbo.usp_prg_amt_paid_by_cost_center @startdate, @enddate

	INSERT INTO #contractual
	EXEC dbo.usp_prg_contractual_by_cost_center @startdate, @enddate

	INSERT INTO #writeoff
	EXEC dbo.usp_prg_write_off_by_cost_center @startdate, @enddate

	SELECT 
		COALESCE(#charges.glaccount,#payments.glaccount,#contractual.glaccount,#writeoff.glaccount) AS glaccount,
		dbo.dict_general_ledger_codes.description,
		ISNULL(#charges.thirdparty,0.00) AS [Third Party Charges],
		ISNULL(#contractual.thirdparty,0.00) AS [Deductions from Charges],
		ISNULL(#payments.thirdparty,0.00) AS [Payments],
		ISNULL(#recovery.thirdparty,0.00) AS [Recovery],
		ISNULL(#contractual.thirdparty,0.00)/ISNULL(#charges.thirdparty,0.00) AS [Percent Deduct from Charges],
		ISNULL(#charges.client,0.00) AS [Client Charges],
		ISNULL(#payments.client,0.00) AS [Client Payments],
		ISNULL(#charges.thirdparty,0.00)+ISNULL(#charges.client,0.00) AS [Total Revenue],
		(ISNULL(#payments.thirdparty,0.00)+ISNULL(#payments.client,0.00)+ISNULL(#recovery.thirdparty,0.00)+ISNULL(#recovery.client,0.00))/(ISNULL(#charges.thirdparty,0.00)+ISNULL(#charges.client,0.00)) AS [Percent Coll of All Charges]
	FROM #charges
	FULL OUTER JOIN #payments ON #payments.glaccount = #charges.glaccount
	FULL OUTER JOIN #contractual ON #contractual.glaccount = #charges.glaccount
	FULL OUTER JOIN #writeoff ON #writeoff.glaccount = #charges.glaccount
	FULL OUTER JOIN #recovery ON #recovery.glaccount = #charges.glaccount
	LEFT OUTER JOIN dbo.dict_general_ledger_codes ON COALESCE(#charges.glaccount,#payments.glaccount,#contractual.glaccount,#writeoff.glaccount) = dbo.dict_general_ledger_codes.level_1
	WHERE dbo.dict_general_ledger_codes.description IS NOT NULL 
	ORDER BY COALESCE(#charges.glaccount,#payments.glaccount,#contractual.glaccount,#writeoff.glaccount)

	DROP TABLE #charges
	DROP TABLE #payments
	DROP TABLE #contractual
	DROP TABLE #writeoff
	DROP TABLE #recovery
END
