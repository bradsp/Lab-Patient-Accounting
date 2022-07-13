-- =============================================
-- Author:		Bradley Powers
-- Create date: 12/20/2013
-- Description:	
-- =============================================
CREATE FUNCTION GetAmountTotal 
(
	-- Add the parameters for the function here
	@chrgnum int
)
RETURNS money
AS
BEGIN
	-- Declare the return variable here
	DECLARE @amttotal money;

	-- Add the T-SQL statements to compute the return value here
	SELECT @amttotal = SUM(amount) from amt where chrg_num = @chrgnum

	-- Return the result of the function
	RETURN @amttotal

END
