
-- =============================================
-- Author:		David
-- Create date: 12/23/2014
-- Description:	modified to Pull the contractuals by insurance
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_contractual_by_selected_Insurance] 
	-- Add the parameters for the stored procedure here
    @startDate DATETIME = 0 ,
    @endDate DATETIME = 0
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

    -- Insert statements for procedure here
        SELECT  *
        FROM    ( SELECT    CASE WHEN UPPER(chk.ins_code) <> 'CLIENT'
                                 THEN 'THIRD PARTY CONTRACTUAL'
                                 ELSE 'CLIENT CONTRACTUAL'
                            END AS [pay_type] ,
                            CASE WHEN chk.account = 'BADDEBT' THEN 'RECOVERY'
                                 WHEN chk.status = 'REFUND' THEN 'REFUND'
ELSE ISNULL(NULLIF(chk.ins_code,''),'UNKNOWN') END AS [INSURANCE],

                            ISNULL(SUM(dbo.chk.contractual), 0.00) AS [contractual]
                  FROM      chk
 
                  WHERE     chk.mod_date BETWEEN @startDate AND @endDate
                            AND chk.status <> 'REFUND'
                  GROUP BY  chk.ins_code,
                            chk.status ,
                            bad_debt ,
                            chk.account
                ) s PIVOT
	( SUM(contractual) FOR pay_type IN ( [CLIENT CONTRACTUAL], [THIRD PARTY CONTRACTUAL] ) )
AS pvt
        ORDER BY pvt.Insurance

    END

