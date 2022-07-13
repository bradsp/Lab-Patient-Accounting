-- =============================================
-- Author:		DAVID
-- Create date: 08/21/2014
-- Description:	Returns the checks before last DataMailer
-- =============================================
Create FUNCTION [dbo].[GetBadDebtByAccount] 
(
	-- Add the parameters for the function here
	@account varchar(15)
	
)
RETURNS NUMERIC(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @chkTotal NUMERIC(18,2);

	--get total of payment records this should be done after checks are posted
	--schedule daily at 8 pm 
	WITH cteChk ([account], [ChkTotal])
	AS
	(
	select [chk].[account]
	, CASE WHEN chk.bad_debt = 1
		THEN SUM(ISNULL(NULLIF([write_off],''),0.00)) 
		ELSE 0 
		END
		as 'chktotal'
		from chk
		INNER JOIN pat ON dbo.pat.account = dbo.chk.account
		WHERE 
			[chk].[status] <> 'CBILL' --AND [chk].[mod_date] > pat.last_dm
			and [chk].[account] = @account
		GROUP BY [chk].[account], chk.bad_debt
	)

	SELECT @chkTotal =  ISNULL(cteChk.ChkTotal,0.00)
	FROM  cteChk 

	-- Return the result of the function
	RETURN @chkTotal

END
