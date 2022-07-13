CREATE PROCEDURE [dbo].[medicare_random_occ_bld]
@fromdate DATETIME,
@thrudate DATETIME
AS

select identity(int,1,1) as [ID],acc.account,pat_name,trans_date 
into #tmp
from acc JOIN chrg on acc.account = chrg.account
where fin_code = 'A' and trans_date between @fromdate and @thrudate and cdm = '6127008'

--works fine on win2k os
select top 20 *
from #tmp
order by newid()
