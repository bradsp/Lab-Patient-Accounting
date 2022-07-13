-- =============================================
-- Author:		David
-- Create date: 01/22/2016
-- Description:	trims leading zeros
-- =============================================
CREATE FUNCTION sfn_trim_zeros 
(
	-- Add the parameters for the function here
	@param VARCHAR(50)
)
RETURNS VARCHAR(35)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(35)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = @param
	WHILE (CHARINDEX('0',@param) = 1)
	BEGIN
		SELECT @param = STUFF(@param,1,1,'')
	END

	SET @Result = @param
	-- Return the result of the function
	RETURN @Result

END
