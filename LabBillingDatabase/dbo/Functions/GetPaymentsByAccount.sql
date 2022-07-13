-- =============================================
-- Author:		David Kelly
-- Create date: 10/08/2014
-- Description:	Returns the totals between a date range
-- =============================================
CREATE FUNCTION [dbo].[GetPaymentsByAccount] 
(
	-- Add the parameters for the function here
	@account varchar(15)
)
RETURNS @tableVar TABLE
(
	colSource varchar(100),
	colAmtPaid numeric(10,2),
	colAmtContractual numeric(10,2),
	colAmtWriteOff NUMERIC(10,2),
	colTotalPaid numeric(10,2)
)
AS
BEGIN

	; WITH cte AS
	(
	SELECT 	dbo.chk.account 
			, SUM(amt_paid) AS [Paid], SUM(dbo.chk.contractual) AS [Contractual]
			, SUM(dbo.chk.write_off) AS [WriteOff]
			, SUM(amt_paid+dbo.chk.contractual+dbo.chk.write_off) AS [TOTAL PAID]
	FROM dbo.chk
	WHERE dbo.chk.account = @account 
	
	GROUP BY account
	)
	INSERT INTO @tableVar
			(
				colSource ,
				colAmtPaid ,
				colAmtContractual,
				colAmtWriteOff ,
				colTotalPaid 
			)
	SELECT 	
	 cte.account,cte.Paid,cte.Contractual,cte.WriteOff,cte.[TOTAL PAID]
	FROM cte
	

	-- Return the result of the function	
	RETURN 

END
