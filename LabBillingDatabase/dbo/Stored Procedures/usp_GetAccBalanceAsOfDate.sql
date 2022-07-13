-- =============================================
-- Author:		David and Rick
-- Create date: 12/02/2009
-- Description:	Get Balance as of a Date
-- =============================================
CREATE PROCEDURE usp_GetAccBalanceAsOfDate 
	-- Add the parameters for the stored procedure here
	@mod_date varchar(10) = '' 
	--,@Account varchar(15) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if OBJECT_ID ('tempdb.dbo.accBal','U') is not null
		drop table tempdb.dbo.accBal
    -- Insert statements for procedure here
	--SELECT @mod_date, @Account
	select c.account, sum(c.qty*c.net_amt)as [balance], a.fin_code
	into tempdb.dbo.accBal
	from chrg c
	left outer join acc a on a.account = c.account
	--left outer join audit_acc aa on aa.account = c.account and status = 'NEW' and
	--		aa.mod_date between convert(varchar,convert(datetime,c.mod_date,101)-(day(convert(datetime,c.mod_date,101))-1),101) and c.mod_date
	where  c.mod_date < @mod_date -- and account = @Account
	group by a.fin_code, c.account
	order by a.fin_code, c.account;


	with cte(account, bal)
	as
	(
		select account, sum(amt_paid+write_off+contractual)
		from chk 
		where  mod_date < @mod_date
		group by account
		
	)
	update tempdb.dbo.accBal 
	set balance = balance-bal
	from cte
	where cte.account = tempdb.dbo.accBal.account
	
END
