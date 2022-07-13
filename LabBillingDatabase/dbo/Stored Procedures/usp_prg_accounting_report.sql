-- =============================================
-- Author:		Rick and David
-- Create date: 02/01/2012
-- Description:	Accounting report for cdm totals
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_accounting_report] 
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
SELECT     dbo.chrg.chrg_num, dbo.chrg.status, dbo.acc.cl_mnem, dbo.acc.fin_code, dbo.chrg.cdm, dbo.chrg.qty
--, dbo.chrg.retail, dbo.chrg.inp_price 
,dbo.chrg.qty*dbo.amt.amount as [Charge]
, dbo.chrg.fin_type, dbo.chrg_pa.pa_amount, dbo.client.cli_nme, dbo.chrg_pa.perform_site, CONVERT(datetime, 
                      CONVERT(varchar(10), dbo.chrg.service_date, 101)) AS service_date, dbo.chrg.mod_date
FROM         dbo.acc INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account inner join
						amt on amt.chrg_num = chrg.chrg_num
	
						LEFT OUTER JOIN
                      dbo.chrg_pa ON dbo.chrg.chrg_num = dbo.chrg_pa.chrg_num LEFT OUTER JOIN
                      dbo.client ON dbo.acc.cl_mnem = dbo.client.cli_mnem
WHERE     (dbo.chrg.cdm <> 'cbill' and (not chrg.status in ('CBILL','N/A','CAP')) 
and amt.mod_date between @startDate and @endDate)
--service_date <= @startDate  and chrg.chrg_num in (select chrg_num from amt 
  --                   where mod_date between @startDate and @endDate )
    --                 or (service_date between @startDate and @endDate and chrg.mod_date < service_date) )
                    order by cdm, fin_type
END
