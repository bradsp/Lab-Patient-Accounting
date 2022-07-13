
-- =============================================
-- Author:		David
-- Create date: 1/21/2015
-- Description:	modified to Pull the amounts paid by insurance
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_amt_paid_by_client] 
	-- Add the parameters for the stored procedure here
    @startDate DATETIME = 0 ,
    @endDate DATETIME = 0
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
        SET NOCOUNT ON;

    -- Insert statements for procedure here
        SELECT  *
--        s.pay_type ,
--        		s.cl_mnem ,
--        		ISNULL(s.paid,0) AS [paid]
        FROM    ( SELECT    CASE WHEN UPPER(chk.ins_code) <> 'CLIENT'
                                 THEN 'THIRD PARTY PAID'
                                 ELSE 'CLIENT PAID'
                            END AS [pay_type] ,
                            acc.cl_mnem,
--                            CASE WHEN chk.account = 'BADDEBT' THEN 'RECOVERY'
--                                -- WHEN chk.bad_debt = 1 THEN 'BAD DEBT ADJUSTMENTS'
--                                 WHEN chk.status = 'REFUND' THEN 'REFUND'
--								WHEN chk.ins_code IN('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
--							THEN ins_code
--							ELSE 'OTHER'
--                            END AS [Insurance] ,
                            ISNULL(SUM(amt_paid), 0.00) AS [paid]
                  FROM      chk
							INNER JOIN acc ON dbo.acc.account = dbo.chk.account
                            --LEFT OUTER JOIN client ON client.cli_mnem = chk.account
                           -- LEFT OUTER JOIN acc ON acc.account = chk.account
                           -- LEFT OUTER JOIN dbo.acc_location ON acc_location.account = acc.account
                           -- LEFT OUTER JOIN client client2 ON acc.cl_mnem = client2.cli_mnem
                  WHERE     chk.mod_date BETWEEN @startDate AND @endDate
                            AND chk.status <> 'REFUND'
                            AND chk.account <> 'BADDEBT'
                  GROUP BY  acc.cl_mnem,
					chk.ins_code,
                  
                            chk.status ,
                            bad_debt ,
--                           
                            chk.account
                ) s PIVOT
	( SUM(paid) FOR pay_type IN ( [CLIENT PAID], [THIRD PARTY PAID] ) )
AS pvt
        

    
    
    END

