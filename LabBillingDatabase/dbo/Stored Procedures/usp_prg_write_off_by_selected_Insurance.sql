﻿
-- =============================================
-- Author:		David
-- Create date: 12/23/2014
-- Description:	modified to Pull the contractuals by insurance
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_write_off_by_selected_Insurance] 
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
                                 WHEN chk.status = 'REFUND' THEN 'REFUND'
                            ELSE ISNULL(NULLIF(chk.ins_code,''),'UNKNOWN') END AS [INSURANCE],
--		WHEN chk.ins_code IN('CLIENT','W','X','Y','Z') THEN 'CLIENT'
--		WHEN chk.ins_code IN('AETNA','AM','BC','CIGNA','MC','SP','TNBC','UHC','UNKNOWN')
--	THEN ins_code
--	ELSE 'OTHER'
--    END AS [Insurance] ,
                            ISNULL(SUM(dbo.chk.write_off), 0.00) AS [WRITE OFF]
                  FROM      chk
                  WHERE     chk.mod_date BETWEEN @startDate AND @endDate
                            AND chk.status <> 'REFUND'
                  GROUP BY  chk.ins_code,
                            chk.status ,
                            bad_debt ,
                            chk.account
                ) s PIVOT
	( SUM([write off]) FOR pay_type IN ( [CLIENT WRITE OFF], [THIRD PARTY WRITE OFF] ) )
AS pvt
        ORDER BY pvt.Insurance

    END
