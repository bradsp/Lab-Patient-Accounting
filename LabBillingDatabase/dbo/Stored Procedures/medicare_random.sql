CREATE PROCEDURE [dbo].[medicare_random]
@fromdate DATETIME,
@thrudate DATETIME
AS

select identity(int,1,1) as [ID],account,pat_name,trans_date 
into #tmp
from acc
where fin_code = 'A' and trans_date between @fromdate and @thrudate

--work fines on win2k os
select top 20 *
from #tmp
order by newid()
