-- =============================================
-- Author:		DAVID
-- Create date: 08/21/2014
-- Description:	Returns the checks before last DataMailer
-- =============================================
CREATE FUNCTION [dbo].[GetDaysSinceLastPayment] 
(
	-- Add the parameters for the function here
	@account varchar(15)
	
)
RETURNS INT -- -1 means no checks 
AS
BEGIN
	-- Declare the return variable here
	DECLARE @DaysSinceLastPayment INT;

	--get total of payment records this should be done after checks are posted
	--schedule daily at 8 pm 
	WITH cteChk ([account], [last_payment])
	AS
	(
	select [chk].[account]
		, MAX(chk.mod_date) AS [last_payment]
		from chk
	--	INNER JOIN pat ON dbo.pat.account = dbo.chk.account
		WHERE  [chk].[account] = @account
		GROUP BY [chk].[account]
	)

	SELECT @DaysSinceLastPayment =  
		ISNULL(DATEDIFF(DAY,cteChk.last_payment,GETDATE()), -1)
	FROM  cteChk 

	-- Return the result of the function
	RETURN @DaysSinceLastPayment

END
