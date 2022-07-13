-- =============================================
-- Author:		Bradley Powers
-- Create date: 9/3/2013
-- Description:	Compile accounting report by GL code
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_report_by_gl] 
	-- Add the parameters for the stored procedure here
    @startDate DATETIME ,
    @endDate DATETIME
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

        WITH    cteChrg ( [GL], [Qty], [3rd Party Net], [Client Net], [Other Billed], [Total] )
                  AS ( SELECT   CASE WHEN acc_location.location = 'LIFT'
                                     THEN '7007'
                                     ELSE ISNULL(client.gl_code, '7009')
                                END AS 'GL' ,
                                qty AS 'Qty' ,
                                CASE WHEN fin_type = 'M' THEN qty * net_amt
                                     ELSE 0.00
                                END AS '3rd Party Net' ,
                                CASE WHEN fin_type = 'C' THEN qty * net_amt
                                     ELSE 0.00
                                END AS 'Client Net' ,
                                CASE WHEN fin_type NOT IN ( 'M', 'C' )
                                     THEN qty * net_amt
                                     ELSE 0.00
                                END AS 'Other Billed' ,
                                qty * net_amt AS 'Net Total'
                       FROM     chrg
                                LEFT OUTER JOIN chrg_pa ON chrg.chrg_num = chrg_pa.chrg_num
                                LEFT OUTER JOIN dbo.acc ON chrg.account = acc.account
                                LEFT OUTER JOIN dbo.acc_location ON acc.account = dbo.acc_location.account
                                LEFT OUTER JOIN client ON acc.cl_mnem = client.cli_mnem
	--LEFT OUTER JOIN fin on chrg.fin_code = fin.fin_code
                       WHERE    chrg.mod_date >= @startDate
                                AND chrg.mod_date < @endDate + 1
                                AND chrg.cdm <> 'CBILL'
                                AND chrg.status NOT IN ( 'N/A', 'CBILL', 'CAP' )
                     )
            SELECT  [GL] AS 'GL_ACCOUNT' ,
                    SUM([Qty]) AS 'QTY' ,
                    CONVERT(MONEY, SUM([3rd Party Net]), 1) AS 'THIRD_PARTY' ,
                    CONVERT(MONEY, SUM([Client Net]), 1) AS 'CLIENT' ,
                    CONVERT(MONEY, SUM([Total]), 1) AS 'TOTAL'
            FROM    cteChrg
            GROUP BY GL
            ORDER BY GL
    END
