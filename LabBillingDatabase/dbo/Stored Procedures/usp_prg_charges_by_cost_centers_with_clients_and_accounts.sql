-- =============================================
-- Author:		David
-- Create date: 06/10/2011
-- Description:	Returns Charges by cost centers
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_charges_by_cost_centers_with_clients_and_accounts] 
	-- Add the parameters for the stored procedure here
    @startDate DATETIME = 0 ,
    @endDate DATETIME = 0
    , @glCode VARCHAR(10)
    
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

    -- Insert statements for procedure here
        SELECT  perform_site AS [GL ACCOUNT] ,
				[Client],
				[ACCOUNT],
                [C] AS [CLIENT] ,
                [M] AS [THIRD PARTY]
        FROM    ( SELECT   chrg.account AS [ACCOUNT], CONVERT(NUMERIC(18, 2), SUM(qty * amount)) AS [chrg] ,
                            fin_type AS [fin_type] ,
                            CASE WHEN acc_location.location = 'LIFT' THEN '7007'
                                 ELSE COALESCE(gl_code, '7009')
                            END AS [perform_site]
                            ,acc.cl_mnem AS [Client]
                  FROM      chrg
                            LEFT OUTER JOIN amt ON amt.chrg_num = chrg.chrg_num
                            INNER JOIN acc ON acc.account = chrg.account
                            LEFT OUTER JOIN dbo.acc_location ON acc.account = acc_location.account
                            LEFT OUTER JOIN client ON client.cli_mnem = acc.cl_mnem
                  WHERE     NOT ( chrg.status IN ( 'CBILL', 'CAP', 'N/A' ) )
							AND chrg.cdm <> 'CBILL'
							--AND COALESCE(dbo.client.gl_code,'') = @glCode
                            AND amt.mod_date BETWEEN @startDate AND @endDate
                  GROUP BY  gl_code ,acc.cl_mnem, chrg.account,
                            dbo.acc_location.location ,
                            fin_type
                ) a PIVOT
	( SUM(chrg) FOR fin_type IN ( [C], [M] ) )
as pvt
        ORDER BY perform_site, pvt.Client, pvt.Account
	
    END
