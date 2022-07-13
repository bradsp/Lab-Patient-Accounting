CREATE FUNCTION [dbo].[ConvertName]
(@Str varchar(8000))
RETURNS varchar(8000) AS
BEGIN
	/*Converts a name string including a comma into a camel cased string in 
	normal form First Middle Last 
	if no comma just uppper cases the string*/
	DECLARE @commaPoint INT
	DECLARE @len INT
	SET @commaPoint = CHARINDEX(',',@Str,0)
	SET @len = LEN(@Str)

	IF (PATINDEX('%,%',@Str) = 0)
	BEGIN
		RETURN dbo.CamelCase(@Str)
	END
	
	DECLARE @Result varchar(8000)
	SET @Result = LTRIM(RTRIM(SUBSTRING(@Str,0,@commaPoint))) -- gets the last name
	SET @Str = LTRIM(RTRIM(REPLACE(SUBSTRING(@Str,@commaPoint+1,(@len -@commaPoint+1)),'%','')))
	
  RETURN  dbo.CamelCase((@Str+' '+@Result))
END  
