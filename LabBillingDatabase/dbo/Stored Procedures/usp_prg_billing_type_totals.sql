-- =============================================
-- Author:		David
-- Create date: 09/03/2013
-- Description:	Gets total number of 1500 and UB's for date range
-- =============================================
CREATE PROCEDURE usp_prg_billing_type_totals 
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

; with cteBilling
as
(
-- PAPER
	select --'1500' as [type],
	count(account) as [NO BILLED 1500]
	, 0 as [NO BILLED UB]
	, convert(datetime,convert(varchar(10),run_date,101)) as [cldate] 
	from h1500
	where printed = 1 and run_date between @startDate and @endDate
	group by convert(datetime,convert(varchar(10),run_date,101))
union 
	select --'UB' as [type],
		 0 as [NO BILLED 1500]
		,count(account) as [NO BILLED UB], convert(datetime, convert(varchar(10),run_date,101)) as [cldate] 
	from ub
	where printed = 1 and run_date between @startDate and @endDate
	group by convert(datetime,convert(varchar(10),run_date,101))
union 
-- SSI
	select --'UB' as [type],
	0 as [NO BILLED 1500]
	,count(account) as [NO BILLED UB], convert(datetime,convert(varchar(10),ub_date,101)) as [cldate]
	from pat
	where ub_date between @startDate and @endDate
	group by convert(datetime,convert(varchar(10),ub_date,101))
union 
	select --'1500' as [type],
	count(account) as [NO BILLED 1500],
	0 as [NO BILLED UB]
	, convert(datetime,convert(varchar(10),h1500_date,101)) as [cldate]

	from pat
	where h1500_date between @startDate and @endDate
	group by convert(datetime,convert(varchar(10),h1500_date,101))

)--select * from cteBilling
, cteBillingSum
as
(
	select sum([NO BILLED UB]) as [BILLED UB]
	,sum([NO BILLED 1500]) as [BILLED 1500]
	, isnull(convert(varchar(10),[cldate],101),'TOTALs') as [DATE]
	from cteBilling
	group by [cldate] with rollup
)
select * from cteBillingSum

END
