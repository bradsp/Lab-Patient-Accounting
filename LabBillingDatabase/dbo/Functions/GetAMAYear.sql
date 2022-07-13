-- =============================================
-- Author:		Bradley Powers
-- Create date: 11/5/2013
-- Description:	Returns the AMA year based on the supplied date
-- =============================================
CREATE FUNCTION GetAMAYear 
(
	-- Add the parameters for the function here
	@date datetime
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result INT;
	DECLARE @year INT;
	DECLARE @month INT;
	-- Add the T-SQL statements to compute the return value here
	--AMA year runs from October 1 until Sept 30th
	

	SET @year = DATEPART(year, @date)
	SET @month = DATEPART(month, @date)

	IF @month >= 10
		BEGIN
			SET @Result = @year + 1
		END
	ELSE
		BEGIN
			SET @Result = @year;		
		END
	
	-- Return the result of the function
	RETURN @Result

END
