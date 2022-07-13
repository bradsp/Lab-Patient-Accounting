-- =============================================
-- Author:		Bradley Powers
-- Create date: 7/24/2013
-- Description:	Query to reconcile aging / aging history
--		Formula: beginning A/R balance
--				+ charges for day
--				- payments for day
--				= ending A/R balance
-- =============================================
CREATE PROCEDURE [dbo].[usp_account_receivable_daily_reconciliation] 
	-- Add the parameters for the stored procedure here
	@BeginDate datetime, 
	@EndDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/* Query to reconcile aging / aging history
		Formula: beginning A/R balance
				+ charges for day
				- payments for day
				= ending A/R balance
	*/

	; with cteDailyBalance(datestamp, ARBalance)
	AS
	(
		select datestamp, SUM(balance) as ARBalance
		from aging_history
		where datestamp between @BeginDate-1 and @EndDate
		group by datestamp
	),
	cteDailyAR(ARDate, BegBalance, EndBalance)
	as
	(
		select c2.datestamp as ARDate, c1.ARBalance as BegARBalance, c2.ARBalance as EndARBalance
		from cteDailyBalance c1 join cteDailyBalance c2 on c1.datestamp = c2.datestamp-1

	),
	cteDailyCharge(ARDate, TotalCharges)
	as
	(
		select CONVERT(CHAR(8),amt.mod_date,10) as ARDAte, SUM(qty*amount) as 'TotalCharges'
		from chrg JOIN amt on chrg.chrg_num = amt.chrg_num
		where amt.mod_date >= @BeginDate and amt.mod_date < @EndDate+1
			and chrg.status NOT IN ('CBILL','N/A','CAP')
		group by CONVERT(CHAR(8),amt.mod_date,10)
	),
	cteDailyPayment(ARDate, TotalPayment)
	as
	(
		select CONVERT(CHAR(8),mod_date,10) as ARDate, COALESCE(SUM(amt_paid+contractual+write_off),0) as 'TotalPayments'
		from chk
		where mod_date >= @BeginDate and mod_date < @EndDate+1
			AND status <> 'CBILL'
		group by CONVERT(CHAR(8),mod_date,10)
	)

	select a.ARDate as [A/R Date]
		, convert(varchar,a.BegBalance,0) as [Beginning A/R Balance]
		, convert(varchar,convert(money,ISNULL(b.TotalCharges, 0.00)),0) as [Total Charges]
		, convert(varchar,convert(money,ISNULL(c.TotalPayment, 0.00)),0) as [Total Payments]
		, convert(varchar,convert(money,ISNULL(a.BegBalance,0.00) + ISNULL(b.TotalCharges,0.00) - ISNULL(c.TotalPayment,0.00)),0) as [Calculated Ending A/R]
		, convert(varchar,convert(money,a.EndBalance),0) as [Ending A/R Balance]
		, convert(varchar,convert(money,(ISNULL(a.BegBalance,0.00) + ISNULL(b.TotalCharges,0.00) - ISNULL(c.TotalPayment,0.00)) - a.EndBalance),0) as [Difference]
	from cteDailyAR a
	left outer join cteDailyCharge b on a.ARDate = b.ARDate
	left outer join cteDailyPayment c on a.ARDate = c.ARDate
	ORDER BY a.ARDate

END
