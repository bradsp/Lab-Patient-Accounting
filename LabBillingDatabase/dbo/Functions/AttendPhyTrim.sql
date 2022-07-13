-- =============================================
-- Author:		David
-- Create date: 06/09/2014
-- Description:	Removes the leading 'z' from a client/phy
-- =============================================
CREATE FUNCTION AttendPhyTrim 
(
	-- Add the parameters for the function here
	@name varchar(50)
)
RETURNS varchar(10)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(10)
	SET @name = REPLACE(@name,'"','')
	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = @name
	IF ( ASCII(LEFT(@name,1)) = 122)
	BEGIN
		SET @Result = LTRIM(RTRIM(UPPER(SUBSTRING(@name,2,LEN(@name)) )))	
	END

	-- Return the result of the function
	RETURN @Result

END
