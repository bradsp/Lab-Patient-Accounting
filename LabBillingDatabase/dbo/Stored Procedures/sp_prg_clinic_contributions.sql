-- =============================================
-- Author:		Rick and David
-- Create date: 03/11/2008
-- Description:	Stored procedure for running Ed's Clinic Report
-- =============================================
CREATE PROCEDURE [dbo].[sp_prg_clinic_contributions] 
	-- Add the parameters for the stored procedure here
	@DateFrom DateTime = '' , 
	@DateThru DateTime = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--- Charges
	SELECT     TOP (100) PERCENT dbo.acc.cl_mnem, CAST(SUM(dbo.chrg.inp_price * dbo.chrg.qty) AS float) AS [GROSS CHARGES], 
			CAST(SUM(dbo.chk.amt_paid) AS float) AS [COLLECTION AMT], 
			DATEPART(month, dbo.acc.trans_date) AS TMONTH, DATEPART(month, dbo.chk.date_rec) AS PMONTH
	FROM         dbo.acc 
			inner JOIN dbo.chrg ON dbo.acc.account = dbo.chrg.account
			inner Join dbo.chk on dbo.chk.account = dbo.acc.account
	WHERE     (dbo.chrg.credited = 0) AND (dbo.acc.trans_date between @DateFrom AND @DateThru) -- CONVERT(DATETIME, '2008-01-01 00:00:00', 102))
				OR (dbo.chk.date_rec  between @DateFrom AND @DateThru)
	GROUP BY dbo.acc.cl_mnem, DATEPART(month, dbo.acc.trans_date), DATEPART(month, dbo.chk.date_rec)
	HAVING      (dbo.acc.cl_mnem IN ('ejc', 'brmc', 'cfmc', 'dwl', 'goo', 'dmg', 'dmgh', 'dmgn')) --AND
			--	(DATEPART(month, dbo.acc.trans_date) IS NOT NULL OR  DATEPART(month, dbo.chk.date_rec) IS NOT NULL)
	ORDER BY dbo.acc.cl_mnem, TMONTH, PMONTH
	

	-- Payments
	--SELECT     TOP (100) PERCENT dbo.acc.cl_mnem, CAST(SUM(dbo.chk.amt_paid) AS float) AS [COLLECTION AMT], DATEPART(month, dbo.chk.date_rec) 
	--					  AS PMONTH
--	FROM         dbo.chk RIGHT OUTER JOIN
--						  dbo.acc ON dbo.chk.account = dbo.acc.account
	--WHERE     (dbo.chk.date_rec  between @DateFrom AND @DateThru)--> CONVERT(DATETIME, '2008-01-01 00:00:00', 102))
--	GROUP BY dbo.acc.cl_mnem, DATEPART(month, dbo.chk.date_rec)
--	HAVING      (dbo.acc.cl_mnem IN ('ejc', 'brmc', 'cfmc', 'dwl', 'goo', 'dmg', 'dmgh', 'dmgn'))
--	ORDER BY dbo.acc.cl_mnem, MONTH
	

END
