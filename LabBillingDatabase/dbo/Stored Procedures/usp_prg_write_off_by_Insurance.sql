
-- =============================================
-- Author:		David
-- Create date: 12/23/2014
-- Description:	modified to Pull the contractuals by insurance
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_write_off_by_Insurance] 
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
                                 THEN 'THIRD PARTY WRITE OFF'
                                 ELSE 'CLIENT WRITE OFF'
                            END AS [pay_type] ,
                            CASE WHEN chk.account = 'BADDEBT' THEN 'RECOVERY'
                                -- WHEN chk.bad_debt = 1 THEN 'BAD DEBT ADJUSTMENTS'
                                 WHEN chk.status = 'REFUND' THEN 'REFUND'
--                                 WHEN acc_location.location = 'LIFT' THEN '7007'
--                                 --WHEN client2.type = '7' THEN '7001'
--                                 --WHEN client.type = '7' THEN '7001'
--                                 --WHEN acc.cl_mnem IN ( 'DMG', 'DMGH', 'MSH','MSHH' ) THEN '7002'
--                                 --WHEN acc.cl_mnem = 'PCC' THEN '7003'
--                                 --WHEN acc.cl_mnem = 'RCL' THEN '7004'
--                                 --WHEN acc.cl_mnem = 'RMC' THEN '7005'
--                                 --WHEN acc.cl_mnem = 'WC' THEN '7006'
--                                 ELSE COALESCE(client2.gl_code, '7009')
								WHEN chk.ins_code IN('CLIENT','W','X','Y','Z') THEN 'CLIENT'
								WHEN chk.ins_code IN('AETNA','AM','BC','CIGNA','MC','SP','TNBC','UHC','UNKNOWN')
							THEN ins_code
							ELSE 'OTHER'
                            END AS [Insurance] ,
                            ISNULL(SUM(dbo.chk.write_off), 0.00) AS [WRITE OFF]
                  FROM      chk
                            --LEFT OUTER JOIN client ON client.cli_mnem = chk.account
                           -- LEFT OUTER JOIN acc ON acc.account = chk.account
                           -- LEFT OUTER JOIN dbo.acc_location ON acc_location.account = acc.account
                           -- LEFT OUTER JOIN client client2 ON acc.cl_mnem = client2.cli_mnem
                  WHERE     chk.mod_date BETWEEN @startDate AND @endDate
                            AND chk.status <> 'REFUND'
                            --AND chk.account <> 'BADDEBT'
                  GROUP BY  chk.ins_code,
                  --client.type ,
                            --client2.type ,
                            --acc.cl_mnem ,
                            --dbo.acc_location.location, 
                            --client2.gl_code ,
                            --acc.account ,
                            --cli_mnem ,
                            chk.status ,
                            bad_debt ,
--                            chk.source ,
                            chk.account
                ) s PIVOT
	( SUM([write off]) FOR pay_type IN ( [CLIENT WRITE OFF], [THIRD PARTY WRITE OFF] ) )
AS pvt
        ORDER BY pvt.Insurance

    END

