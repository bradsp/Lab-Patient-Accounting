-- =============================================
-- Author:		David
-- Create date: 07/07/2014
-- Description:	Pad text to a specific length
-- =============================================
CREATE FUNCTION PadTextSave 
(
	-- Add the parameters for the function here
	@text varchar(1024),
	@len	INT
)
RETURNS VARCHAR(1024)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(1024)
	-- Add the T-SQL statements to compute the return value here
	SET @text = LTRIM(RTRIM(@text))
	DECLARE @padLen INT
	SET @padLen = @len - LEN(@text)
	SELECT @Result = 
		CASE WHEN @padLen >= LEN(@text) THEN @text + SPACE(@padlen)
		ELSE LEFT(@text,@len)
		end
	

	-- Return the result of the function
	RETURN @Result

END
