-- =============================================
-- Author:		Rick and David
-- Create date: 06/29/2011
-- Description:	Get AR by General Ledger Account code
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_acct_recievables_by_cost_center] 
	-- Add the parameters for the stored procedure here
    @startDate DATETIME = 0 ,
    @endDate DATETIME = 0
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

    -- Insert statements for procedure here
        WITH    cteBalance
                  AS ( SELECT   account ,
                                ISNULL(fin.type,'M') AS fin_type ,
                                balance
                       FROM     aging_history
                                LEFT OUTER JOIN fin ON fin.fin_code = aging_history.fin_code
                       WHERE    datestamp BETWEEN @startDate AND @endDate
                     )

	SELECT  [GL ACCOUNT] ,
            [C] AS [client] ,
            [M] AS [Third Party]
    FROM    ( SELECT    ISNULL(b.balance, 0) AS [balance] ,
                        fin_type --,count(b.account)
                        ,
                        CASE WHEN b.account = 'BADDEBT' THEN 'RECOVERY'
                             WHEN bad_debt.account_no IS NOT NULL
                             THEN CASE WHEN date_sent IS NULL
                                       THEN 'COLLECTIONS'
                                       ELSE 'BAD DEBT'
                                  END
                             WHEN client.type = '7' THEN '7001'
                             WHEN cli_mnem IN ( 'DMG', 'DMGH', 'MSH', 'MSHH' )
                             THEN '7002'
                             WHEN cli_mnem = 'TUC' THEN '7000'
                             WHEN cli_mnem = 'PCC' THEN '7003'
                             WHEN cli_mnem = 'RMC' THEN '7005'
                             WHEN cli_mnem = 'RCL' THEN '7004'
                             WHEN cli_mnem = 'WC' THEN '7006'
                             WHEN acc_location.location = 'LIFT' THEN '7007'
                             ELSE isnull(client.gl_code,'7009')
                        END AS [GL Account]
              FROM      cteBalance b 
                        INNER JOIN acc ON acc.account = b.account
						LEFT OUTER JOIN dbo.acc_location ON acc.account = acc_location.account
                        LEFT OUTER JOIN client ON client.cli_mnem = acc.cl_mnem
                        LEFT OUTER JOIN bad_debt ON bad_debt.account_no = b.account
            ) a PIVOT
		( SUM(balance) FOR fin_type IN ( [C], [M] ) ) pvt
    ORDER BY [GL Account]
    END
