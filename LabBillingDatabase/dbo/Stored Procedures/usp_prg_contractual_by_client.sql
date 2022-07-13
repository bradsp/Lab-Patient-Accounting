
-- =============================================
-- Author:		David
-- Create date: 12/23/2014
-- Description:	modified to Pull the contractuals by insurance
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_contractual_by_client] 
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
        FROM    ( SELECT TOP(100) percent   acc.cl_mnem,
        CASE WHEN UPPER(chk.ins_code) <> 'CLIENT'
                                 THEN 'THIRD PARTY CONTRACTUAL'
                                 ELSE 'CLIENT CONTRACTUAL'
                            END AS [pay_type] ,
--                            CASE WHEN chk.account = 'BADDEBT' THEN 'RECOVERY'
--                                 WHEN chk.status = 'REFUND' THEN 'REFUND'
--								WHEN chk.ins_code IN('CLIENT','W','X','Y','Z') THEN 'CLIENT'
--								WHEN chk.ins_code IN('AETNA','AM','BC','CIGNA','MC','SP','TNBC','UHC','UNKNOWN')
--							THEN ins_code
--							ELSE 'OTHER'
--                            END AS [Insurance] ,
                            ISNULL(SUM(ISNULL(dbo.chk.contractual,0.00)), 0.00) AS [contractual]
                  FROM      chk
							INNER JOIN acc ON dbo.acc.account = dbo.chk.account
                            --LEFT OUTER JOIN client ON client.cli_mnem = chk.account
                           -- LEFT OUTER JOIN acc ON acc.account = chk.account
                           -- LEFT OUTER JOIN dbo.acc_location ON acc_location.account = acc.account
                           -- LEFT OUTER JOIN client client2 ON acc.cl_mnem = client2.cli_mnem
                  WHERE     chk.mod_date BETWEEN @startDate AND @endDate
                            AND chk.status <> 'REFUND'
                            --AND chk.account <> 'BADDEBT'
                  GROUP BY  acc.cl_mnem,
                  chk.ins_code,
 
                            chk.status ,
                            bad_debt ,
--                            
                            chk.account
                  ORDER BY acc.cl_mnem
                ) s PIVOT
	( SUM(contractual) FOR pay_type IN ( [CLIENT CONTRACTUAL], [THIRD PARTY CONTRACTUAL] ) )
AS pvt
ORDER BY pvt.cl_mnem 

    END

