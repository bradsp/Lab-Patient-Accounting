-- =============================================
-- Author:		David
-- Create date: 06/25/2012
-- Description:	Create Drill through for Cbills
-- =============================================
CREATE PROCEDURE usp_prg_cbill_client_orders 
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
	select  cli_mnem, cli_nme
	,acc.account,trans_date
	, sum(net_amt*qty) as [Total Charge]
	from client
	inner join acc on acc.cl_mnem = client.cli_mnem
	inner join chrg on chrg.account = acc.account
	where trans_date between @startDate and @endDate
	group by cli_mnem, cli_nme,acc.account, trans_date
	order by cli_mnem, cli_nme,acc.account, trans_date

END
