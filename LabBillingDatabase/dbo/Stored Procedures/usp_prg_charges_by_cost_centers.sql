-- =============================================
-- Author:		David
-- Create date: 06/10/2011
-- Description:	Returns Charges by cost centers
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_charges_by_cost_centers] 
	-- Add the parameters for the stored procedure here
    @startDate DATE = GETDATE ,
    @endDate DATE = GETDATE
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

    -- Insert statements for procedure here
        SELECT  perform_site AS [GL ACCOUNT] ,
                [C] AS [CLIENT] ,
                [M] AS [THIRD PARTY]
        FROM    ( SELECT    CONVERT(NUMERIC(18, 2), SUM(calc_amt)) AS [chrg] ,
                            ISNULL(fin_type,'M') AS [fin_type] ,
                            CASE WHEN acc_location.location = 'LIFT' THEN '7007'
                                 ELSE COALESCE(gl_code, '7009')
                            END AS [perform_site]
                  FROM      chrg
                            --LEFT OUTER JOIN amt ON amt.chrg_num = chrg.chrg_num
                            INNER JOIN acc ON acc.account = chrg.account
                            LEFT OUTER JOIN dbo.acc_location ON acc.account = acc_location.account
                            LEFT OUTER JOIN client ON client.cli_mnem = acc.cl_mnem
                  WHERE     NOT ( chrg.status IN ( 'CBILL', 'CAP', 'N/A' ) )
							AND chrg.cdm <> 'CBILL'
                            AND chrg.mod_date >= @startDate AND chrg.mod_date < DATEADD(dd,1,@endDate)
                  GROUP BY  gl_code ,
                            dbo.acc_location.location ,
                            ISNULL(fin_type,'M')
                ) a PIVOT
	( SUM(chrg) FOR fin_type IN ( [C], [M] ) )
as pvt
        ORDER BY perform_site
	
    END
