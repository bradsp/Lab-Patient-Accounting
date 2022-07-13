
-- =============================================
-- Author:		David
-- Create date: 12/23/2014
-- Description:	modified to Pull the amounts paid by insurance
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_amt_paid_by_selected_insurance] 
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
                                 THEN 'THIRD PARTY PAID'
                                 ELSE 'CLIENT PAID'
                            END AS [pay_type] ,
                            
                            CASE WHEN chk.account = 'BADDEBT' THEN 'RECOVERY'
                                 WHEN chk.status = 'REFUND' THEN 'REFUND'
							ELSE ISNULL(NULLIF(chk.ins_code,''),'UNKNOWN') END AS [INSURANCE],
--	WHEN 
--	chk.ins_code IN('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
--	THEN ins_code
--	ELSE 'OTHER'
--  END AS [Insurance] ,
                            ISNULL(SUM(amt_paid), 0.00) AS [paid]
                  FROM      chk

                  WHERE     chk.mod_date BETWEEN @startDate AND @endDate
                            AND chk.status <> 'REFUND'
                            AND chk.account <> 'BADDEBT'
                  GROUP BY  --ISNULL(NULLIF(chk.ins_code,''),'UNKNOWN'),
							chk.ins_code,
                            chk.status ,
                            bad_debt ,
                            chk.account
                ) s PIVOT
	( SUM(paid) FOR pay_type IN ( [CLIENT PAID], [THIRD PARTY PAID] ) )
AS pvt
       ORDER BY pvt.Insurance

    END

