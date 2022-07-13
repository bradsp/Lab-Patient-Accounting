-- =============================================
-- Author:		David
-- Create date: 07/02/2013
-- Description:	Run the out of balance report
-- =============================================
CREATE PROCEDURE [dbo].[sp_job_out_of_balance] 
	-- Add the parameters for the stored procedure here
	@startDate datetime = 0, 
	@endDate datetime = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT @startDate, @endDate
IF  OBJECT_ID('dbo.tempDaily') IS NOT NULL
        DROP TABLE dbo.tempDaily

create table tempDaily (account varchar(15) primary key
, startBal numeric(18,2)
, charges numeric(18,2)
, payments numeric(18,2)
, endBal numeric(18,2)
, error as (isnull(startBal,0)+isnull(charges,0)-isnull(payments,0)-isnull(endBal,0)))

-- 1. get the starting balance

;with cteBalPrior as
(
	select account, convert(datetime,convert(varchar(10),datestamp,101)) as datestamp,
	balance
	from aging_history
	where datestamp between @startDate and @endDate
) 
insert into tempDaily (account, startBal)
select account, balance from cteBalPrior

-- 2. get the charges for accounts that do not exist in the prior balance
; with cteCharge as
(
	select account, sum(qty*net_amt) as [charges]
	from chrg
	where chrg.mod_date between @startDate and @endDate
	and service_date <= @endDate
	group by account
)
insert into tempDaily (account, startBal, charges) 
select cteCharge.account, 0, cteCharge.charges 
from cteCharge
left outer join tempDaily on tempDaily.account = cteCharge.account
where tempDaily.account is null 


-- 3. get the charges for accounts that DO exist in the prior balance
; with cteCharge2 as
(
	select account, sum(qty*net_amt) as [charges]
	from chrg
	where chrg.mod_date between @startDate and @endDate
	and service_date <= @endDate
	group by account

) 
update tempDaily 
set  charges = cteCharge2.charges 
from cteCharge2
left outer join tempDaily on tempDaily.account = cteCharge2.account
where tempDaily.account is not null
and tempDaily.account = cteCharge2.account 

-- 4 get the checks that do not have a beginning balance. second coming of the check
; with  cteChk as
(
	select account, sum(amt_paid+contractual+write_off) as [payments]
	from chk
	where mod_date between @startDate and @endDate 
	group by account
) 
insert into tempDaily (account, startBal, payments)
select cteChk.account, 0, cteChk.payments
from cteChk
left outer join tempDaily on tempDaily.account = cteChk.account
where tempDaily.account is null 

-- 4a. Get the checks 
; with  cteChk as
(
	select account, sum(amt_paid+contractual+write_off) as [payments]
	from chk
	where mod_date between @startDate and @endDate 
	group by account
) 
update tempDaily
set payments = cteChk.payments from cteChk 
left outer join tempDaily on tempDaily.account = cteChk.account
where cteChk.account is not null 


-- 5. insert accounts that do not yet exist in tempDaily (this is an error indicator)
; with cteBalCurrent as
(
select account, convert(datetime,convert(varchar(10),datestamp,101)) as datestamp,
sum(balance) as balance
from aging_history
where datestamp between @startDate+1 and @endDate+1
group by account, convert(datetime,convert(varchar(10),datestamp,101))
)
insert into tempDaily (account, endBal)
select cteBalCurrent.account, cteBalCurrent.balance from cteBalCurrent
left outer join tempDaily on tempDaily.account = cteBalCurrent.account
where tempDaily.account is null

-- 6. update the accounts that do have starting balances
; with cteBalCurrent as
(
select account, convert(datetime,convert(varchar(10),datestamp,101)) as datestamp,
sum(balance) as balance
from aging_history
where datestamp between @startDate+1 and @endDate+1
group by account, convert(datetime,convert(varchar(10),datestamp,101))
)
update tempDaily
set endBal = cteBalCurrent.balance
from cteBalCurrent
left outer join tempDaily on cteBalCurrent.account = tempDaily.account
where cteBalCurrent.account is not null  and tempDaily.endBal is null
and cteBalCurrent.account = tempDaily.account

select sum(startBal) as [Starting Balance]
,sum(charges) as [charges]
, sum(payments) as [payments]
, sum(endBal) as [ending balance]
, sum(error) as [error] from tempDaily

select account,convert(varchar(10),@startDate,101) as [Date]
,startBal
,isnull(charges,0) as [charges] 
,isnull(payments,0) as [payments] 
,isnull(endBal,0) as [endBal]
,isnull(error,0) as [error]
from tempDaily where error 	<> 0.00
order by account


exec msdb.dbo.sp_send_dbmail
@profile_name = 'WTHMCLBILL',
@recipients = 'david.kelly@wth.org',
@subject = 'Daily Errors',
@attach_query_result_as_file = 0,
@execute_query_database = 'MCLLIVE',
@query = 'select account, convert(varchar(10),getdate(),101) as [Date]
,startBal
,isnull(charges,0) as [charges] 
,isnull(payments,0) as [payments] 
,isnull(endBal,0) as [endBal]
,isnull(error,0) as [error]
from tempDaily 
where error 	<> 0.00
order by account';


END
