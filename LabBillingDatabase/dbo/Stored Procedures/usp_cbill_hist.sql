-- =============================================
-- Author:		David
-- Create date: 05/21/2013
-- Description:	Select clients charges and payments
-- =============================================
CREATE PROCEDURE [dbo].[usp_cbill_hist] 
	-- Add the parameters for the stored procedure here
	@client varchar(15) = '', 
	@startDate datetime = '',
	@endDate datetime = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT @client, @startDate, @endDate

;with cteDate
as
(
	select dateadd(ms,-0,dateadd(mm,datediff(m,0, StandardDate ),0)) as [startDate]
		 , dateadd(ms,-3,dateadd(mm,datediff(m,0, StandardDate )+1,0)) as [endDate]

	from dict_date
	where  StandardDate  = convert(varchar(10),dateadd(ms,-3,dateadd(mm,datediff(m,0, StandardDate )+1,0)),101)
	and convert(varchar(10),dateadd(ms,-3,dateadd(mm,datediff(m,0, StandardDate )+1,0)),101) < getdate()
)
,cteChrg
as
(
	select  invoice,
convert(datetime,convert(varchar(10),dateadd(ms,-3,dateadd(mm,datediff(m,0, chrg.mod_date )+1,0)),101)) as [cdate]
		,sum(qty*net_amt) as [Charge]
	from chrg
	inner join acc on acc.account = chrg.account
	where acc.cl_mnem = @client
	group by dateadd(ms,-3,dateadd(mm,datediff(m,0, chrg.mod_date )+1,0)) ,invoice

)  
, ctePay
as
(
	select 
	 convert(datetime,convert(varchar(10),dateadd(ms,-3,dateadd(mm,datediff(m,0,chk.mod_date)+1,0)),101)) as [pdate]
	, sum(contractual+amt_paid+write_off) 


		as [Paid]
	from chk
	where account = @client	
	group by dateadd(ms,-3,dateadd(mm,datediff(m,0,chk.mod_date)+1,0))

) 
select @client as [cl_mnem], invoice, endDate as [thru_date],
startDate, 0.00 as [Balance Forward]
,cdate
,[Charge]
, pdate
, [Paid]
, 0.00 as [Running Balance]
,endDate
from cteDate
left outer join cteChrg on cteChrg.cdate between cteDate.startDate and cteDate.endDate
left outer join ctePay on ctePay.pdate between cteDate.startDate and cteDate.endDate
where invoice is not null
order by startDate
END
