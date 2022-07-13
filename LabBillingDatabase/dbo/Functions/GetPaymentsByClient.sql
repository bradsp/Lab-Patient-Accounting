-- =============================================
-- Author:		David Kelly
-- Create date: 10/08/2014
-- Description:	Returns the totals between a date range
-- =============================================
CREATE FUNCTION [dbo].[GetPaymentsByClient] 
(
	-- Add the parameters for the function here
	@client varchar(15),
	@startDate DATETIME,
	@endDate DATETIME
)
RETURNS @tableVar TABLE
(
	colClient varchar(100),
	colAmtPaid numeric(10,2),
	colAmtContractual numeric(10,2),
	colAmtWriteOff NUMERIC(10,2),
	colTotalPaid numeric(10,2)
)
AS
BEGIN

	; WITH cte AS
	(
	SELECT 	COALESCE(dbo.chk.account ,@client) AS [account]
			, ISNULL(SUM(amt_paid),0.00) AS [Paid]
			, ISNULL(SUM(dbo.chk.contractual),0.00) AS [Contractual]
			, ISNULL(SUM(dbo.chk.write_off),0.00) AS [WriteOff]
			, ISNULL(SUM(amt_paid+dbo.chk.contractual+dbo.chk.write_off),0.00) AS [TOTAL PAID]
			
	FROM dbo.chk
	WHERE dbo.chk.account = @client
	AND chk.mod_date BETWEEN @startDate AND @endDate
	
	GROUP BY account
	)
	INSERT INTO @tableVar
			(
				colClient ,
				colAmtPaid ,
				colAmtContractual,
				colAmtWriteOff ,
				colTotalPaid 
			)
	SELECT 	
	 COALESCE(cte.account,@client),ISNULL(cte.Paid,0.00)
	 ,ISNULL(cte.Contractual,0.00)
	 ,ISNULL (cte.WriteOff,0.00) 
	 ,ISNULL(cte.[TOTAL PAID],0.00)
	FROM cte
	

	-- Return the result of the function	
	RETURN 

END
