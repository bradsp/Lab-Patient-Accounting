-- =============================================
-- Author:		David
-- Create date: 04/23/2014
-- Description:	Formats a Phone number
-- =============================================
CREATE FUNCTION FormatPhoneNo 
(
	-- Add the parameters for the function here
	@phone varchar(23)
)
RETURNS varchar(13)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(13)
	
	-- Add the T-SQL statements to compute the return value here
	SET @phone = SUBSTRING(@phone,0,CHARINDEX('^',@phone,0))
	SET @phone = NULLIF(NULLIF(REPLACE(REPLACE(@phone,'^',''),'no',''),'U'),'UNK')
	
	SELECT @Result = 
	LTRIM(RTRIM(LEFT(REPLACE(replace(replace(replace(replace(replace(@phone,'-',''),'(',''),')',''),' ',''),'EXT',''),'"',''),13)))
	
	IF (LEFT(@Result,1) = 1 AND LEN(@Result) = 8)
	BEGIN
		SET @Result = RIGHT(@Result,7)
	END
	
	SELECT @Result = (SELECT CASE WHEN @Result IN ('0000000000','0000000','000','731','901') THEN NULL ELSE @Result END)
	
	SELECT @Result = (
	SELECT CASE WHEN LEN(@Result) = 10 THEN STUFF(STUFF(@Result,7,0,'.'),4,0,'.')
				ELSE CASE WHEN LEN(@Result) = 7 THEN STUFF(@Result, 4,0,'.')
					 ELSE  @Result
						  
					 END
		   END
	)
	

	-- Return the result of the function
	RETURN REPLACE(@Result,'.','')

END
