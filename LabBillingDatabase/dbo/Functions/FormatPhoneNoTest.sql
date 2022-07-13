-- =============================================
-- Author:		David
-- Create date: 04/23/2014
-- Description:	Formats a Phone number
-- =============================================
CREATE FUNCTION FormatPhoneNoTest 
(
	-- Add the parameters for the function here
	@phone varchar(23)
)
RETURNS varchar(13)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(13)
	--DECLARE @ext VARCHAR(10)
	-- Add the T-SQL statements to compute the return value here
	SET @phone = 
	REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@phone,'(',''),')',''),'-',''),' ',''),':',''),'.','')
	IF (EXISTS(SELECT @phone WHERE @phone LIKE '%x%')
	 OR @phone LIKE 'EXT')
	BEGIN
	SET @phone = SUBSTRING(@phone,0,CHARINDEX('x',@phone,0))
	--SET @ext = NULLIF(REPLACE(@phone,@Result,''),'')
	END
	select @Result = (SELECT
	CASE WHEN LEN(@result) = 7 THEN STUFF(@result,3,0,'.')
		 WHEN LEN(@result) = 10 THEN 
		 STUFF(STUFF(@result,4,0,'.'),8,0,'.')
	ELSE STUFF(STUFF(@phone,4,0,'.'),8,0,'.')
	END )
		
	RETURN ISNULL(@result,'')	--8004897372xswq234
--	SET @phone = NULLIF(NULLIF(REPLACE(REPLACE(@phone,'^',''),'no',''),'U'),'UNK')
--	
--	SELECT @Result = 
--	LTRIM(RTRIM(LEFT(REPLACE(replace(replace(replace(replace(replace(@phone,'-',''),'(',''),')',''),' ',''),'EXT',''),'"',''),13)))
--	
--	IF (LEFT(@Result,1) = 1 AND LEN(@Result) = 8)
--	BEGIN
--		SET @Result = RIGHT(@Result,7)
--	END
--	
--	SELECT @Result = (SELECT CASE WHEN @Result IN ('0000000000','0000000','000','731','901') THEN NULL ELSE @Result END)
--	
--	SELECT @Result = (
--	SELECT CASE WHEN LEN(@Result) = 10 THEN STUFF(STUFF(@Result,7,0,'.'),4,0,'.')
--				ELSE CASE WHEN LEN(@Result) = 7 THEN STUFF(@Result, 4,0,'.')
--					 ELSE  @Result
--						  
--					 END
--		   END
--	)
--	
--
--	-- Return the result of the function
--	RETURN REPLACE(@Result,'.','')

END
