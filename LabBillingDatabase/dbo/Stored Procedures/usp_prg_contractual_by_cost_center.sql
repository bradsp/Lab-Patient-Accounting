-- =============================================
-- Author:		Rick and David
-- Create date: 06/21/2011
-- Description:	Collects the contractual amount paid during the daterange
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_contractual_by_cost_center] 
	-- Add the parameters for the stored procedure here
    @startDate DATE = getdate ,
    @endDate DATE = getdate
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

    -- Insert statements for procedure here
        SELECT  *
        FROM    ( SELECT    CASE WHEN client.type IS NULL
                                 THEN 'THIRD PARTY CONTRACTUAL'
                                 ELSE 'CLIENT CONTRACTUAL'
                            END AS [pay_type] ,
                            CASE WHEN chk.account = 'BADDEBT' THEN 'RECOVERY'
                                 WHEN chk.bad_debt = 1
                                 THEN 'BAD DEBT ADJUSTMENTS'
                                 WHEN chk.status = 'REFUND' THEN 'REFUND'
                                 WHEN acc_location.location = 'LIFT'
                                 THEN '7007'
                                 --WHEN client2.type = '7' THEN '7001'
                                 --WHEN client.type = '7' THEN '7001'
                                 --WHEN acc.cl_mnem IN ( 'DMG', 'DMGH', 'MSH','MSHH' ) THEN '7002'
                                 --WHEN acc.cl_mnem = 'PCC' THEN '7003'
                                 --WHEN acc.cl_mnem = 'RCL' THEN '7004'
                                 --WHEN acc.cl_mnem = 'RMC' THEN '7005'
                                 --WHEN acc.cl_mnem = 'WC' THEN '7006'
                                 ELSE COALESCE(client2.gl_code, '7009')
                            END AS [GL] ,
                            SUM(ISNULL(contractual, 0)) AS [contractual]
                  FROM      chk
                            LEFT OUTER JOIN client ON client.cli_mnem = chk.account
                            INNER JOIN acc ON acc.account = chk.account
                            LEFT OUTER JOIN dbo.acc_location ON acc.account = dbo.acc_location.account
                            LEFT OUTER JOIN client client2 ON client2.cli_mnem = acc.cl_mnem
                  WHERE     chk.mod_date >= @startDate AND chk.mod_date < DATEADD(dd,1,@endDate)
                            --AND chk.account <> 'BADDEBT'
                            AND chk.status <> 'REFUND'
                  GROUP BY  client.type ,
                            client2.type ,
                            acc.cl_mnem ,
                            acc_location.location ,
                            acc.account ,
                            client2.gl_code , 
                            --cli_mnem ,
                            chk.status ,
                            bad_debt ,
                            chk.source ,
                            chk.account
                ) s PIVOT
	( SUM([CONTRACTUAL]) FOR pay_type IN ( [CLIENT CONTRACTUAL],
                                           [THIRD PARTY CONTRACTUAL] ) )
AS pvt
        ORDER BY [GL]

    END
