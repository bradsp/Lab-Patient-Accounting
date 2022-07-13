-- =============================================
-- Author:		David
-- Create date: 07/07/2014
-- Description:	Pad text to a specific length
-- =============================================
CREATE FUNCTION PadText
(
	-- Add the parameters for the function here
	@text varchar(1024),
	@len INT
	
)
RETURNS VARCHAR(1024)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(1024)
	-- Add the T-SQL statements to compute the return value here
	SET @text = LTRIM(RTRIM(@text))
	DECLARE @padLen INT
	SET @padLen = CASE WHEN LEN(@text) > ABS(@len) 
			THEN 0
			ELSE ABS(@len)-LEN(@text) end
	
	SELECT @Result = CASE WHEN @len >= 0 THEN LEFT(@text+SPACE(@padlen),@len) 
		ELSE RIGHT(SPACE(@padlen)+@text, ABS(@len))
		END

	-- Return the result of the function
	RETURN @Result -- testing use for placement(REPLACE(@Result,' ','*'))

END
