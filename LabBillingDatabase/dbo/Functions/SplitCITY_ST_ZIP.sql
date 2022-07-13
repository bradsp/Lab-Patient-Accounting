-- =============================================
-- Author:		David
-- Create date: 07/08/2015
-- Description:	Splits city_st_zip into slected parts
-- C - City
-- S - State
-- Z - Zip
-- =============================================
CREATE FUNCTION SplitCITY_ST_ZIP 
(
	-- Add the parameters for the function here
	@Value varchar(125),
	@Part VARCHAR(1)
)
RETURNS varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(50)
	DECLARE @City VARCHAR(50)
	SET @City = NULL
	DECLARE @State varchar(2)
	SET @State = NULL
	DECLARE @Zip VARCHAR(10)
	SET @Zip = NULL
	DECLARE @nSplit int

	SET @Value = REPLACE(@Value,', ',',') -- standardize the pattern
	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = NULL
	SET @nSplit = (SELECT CHARINDEX(',',@Value,0))
	IF ( @nSplit > -1)
	BEGIN
		SET @City = LTRIM(RTRIM(SUBSTRING(@Value,0,@nSplit) ) )
		SET @Value = REPLACE(LTRIM(RTRIM(REPLACE(@Value,@City,'') ) ),',','')
		SET @nSplit =  (SELECT CHARINDEX(' ',@Value,0))
		SET @State = LTRIM(RTRIM(SUBSTRING(@Value,0,@nSplit) ) )
		SET @Value = LTRIM(RTRIM(REPLACE(@Value,@State,'') ) )
		SET @Zip = @Value
		
	END

	-- Return the result of the function
	IF (@Part = 'C')
	BEGIN
		SET @Result = NULLIF(LTRIM(RTRIM(@City)),'' )
	END
	IF (@Part = 'S')
	BEGIN
		SET @Result = NULLIF(LTRIM(RTRIM(@State)),'' )
	END
	IF (@Part = 'Z')
	BEGIN
		SET @Result = NULLIF(LTRIM(RTRIM(@Zip)),'')
	END

	RETURN @Result

END
