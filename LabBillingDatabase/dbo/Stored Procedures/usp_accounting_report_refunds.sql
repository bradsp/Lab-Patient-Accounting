-- =============================================
-- Author:		Bradley Powers
-- Create date: 09/04/2013
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_report_refunds] 
	-- Add the parameters for the stored procedure here
	@startDate DATETIME, 
	@endDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @results TABLE
	(
		[Description] varchar(50),
		[Amount] numeric(18,2)
	); 

	DECLARE @refunds MONEY;
	DECLARE @TotalCharges money;
	DECLARE @TotalPayments money;
	DECLARE @BeginAR money;
	DECLARE @EndAR money;
	DECLARE @ComputedEndAR money;
	DECLARE @MiscAdj money;

	select @BeginAR = SUM(balance)
	from aging_history
	where datestamp = @startDate-1

	select @EndAR = SUM(balance)
	from aging_history
	where datestamp = DATEADD(dd,0,DATEDIFF(dd,0,@EndDate))

	select @TotalCharges = SUM(qty*amount)
	from chrg LEFT OUTER JOIN amt on chrg.chrg_num = amt.chrg_num
	where amt.mod_date >= @startDate AND amt.mod_date < @endDate+1
		and chrg.cdm <> 'CBILL' AND chrg.status NOT IN ('CBILL','N/A','CAP')

	select @TotalPayments = COALESCE(SUM(amt_paid+contractual+write_off),0)
	from chk
	where mod_date >= @startDate and mod_date < @EndDate+1
		AND status NOT IN ('CBILL','REFUND')

	select @refunds = SUM(amt_paid)
	from chk where mod_date >= @startDate and mod_date < @endDate+1
	and status = 'REFUND' --AND account <> 'BADDEBT'
	--had to remove the account<>'BADDEBT' filter due to a refund being posted to the BADDEBT account
	--per Carol Plumlee 9/2/2014 bsp
	SET @ComputedEndAR = @BeginAR+@TotalCharges-@TotalPayments-@refunds;
	SET @MiscAdj = @ComputedEndAR - @EndAR;

    -- Insert statements for procedure here
	INSERT INTO @results VALUES ('Refunds',@refunds);
	INSERT INTO @results VALUES ('Misc Adjustment',@MiscAdj);
	INSERT INTO @results VALUES ('Total Other Adjustments', @refunds+@MiscAdj);

	select * from @results;

END
