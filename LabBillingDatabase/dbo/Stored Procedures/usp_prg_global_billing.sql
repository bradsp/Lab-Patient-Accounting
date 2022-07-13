
-- =============================================
-- Author:		David
-- Create date: 20110120
-- Description:	Retrieve Global billing accounts
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_global_billing] 
	-- Add the parameters for the stored procedure here
	@startDate datetime = 0,--'01/01/2012 00:00:00.000', 
	@endDate datetime = 0
AS
BEGIN
	RETURN;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

-- accounts that have cdm's that contain the global billing cpt4s
select acc.cl_mnem, chrg.account, chrg.chrg_num as [chrg_num], cdm, cpt4, sum(qty) as [qty], sum(qty*amount) as [charges]
	, convert(datetime,convert(varchar(10),service_date,101)) as [DOS]
	, convert(datetime,convert(varchar(10),amt.mod_date,101)) as [Date Chrg Entered]
	, acc.fin_code, client.type
from chrg
inner join amt on amt.chrg_num = chrg.chrg_num AND amt.type <> 'PC'
inner join acc on acc.account = chrg.account
inner join client on client.cli_mnem = acc.cl_mnem
where cdm IN (select cdm from dict_global_billing_cdms
WHERE @startDate BETWEEN effective_date AND COALESCE(expiration_date,GETDATE() ))
--and service_date >= @startDate
and service_date between @startDate and @endDate
and credited = 0
and (invoice is null or invoice = '')
and ((client.type in ('0','1','2') and (not acc.fin_code in ('D','A','M','X','Y')))
		or (not client.type in ('0','1','2') and (not acc.fin_code in ('D'))))
and (not chrg.account like '[Q]%')
--and client.cli_mnem <> 'LEW'
and client.cli_mnem not in ('HC','JPG', 'LEW','TPG','TPG2','TPG3','BMUC','MGPS') -- wdk 20190603 added 'HC'
group by acc.cl_mnem, convert(datetime,convert(varchar(10),service_date,101)), chrg.account, cdm, cpt4
	, chrg.chrg_num, chrg.credited,  convert(datetime,convert(varchar(10),amt.mod_date,101))
	, acc.fin_code, client.type
having sum(qty*amount) <> 0
order by convert(datetime,convert(varchar(10),service_date,101)), acc.account, cdm, cpt4

END
