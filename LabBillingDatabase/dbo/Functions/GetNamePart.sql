
-- =============================================
-- Author:		Bradley Powers
-- Create date: 09/17/2013
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetNamePart] 
(
	-- Add the parameters for the function here
	-- @name - full name in LASTNAME SUF,FIRSTNAME MIDDLENAME FORMAT
	-- @namepart - part of name to return (LAST, FIRST, MIDDLE, SUFFIX)
	@name varchar(100),
	@namepart varchar(20)
)
RETURNS varchar(100)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(100);
	DECLARE @NumCommas int;
	DECLARE @LastName varchar(100);
	DECLARE @FirstName varchar(100);
	DECLARE @MiddleName VARCHAR(100);
	DECLARE @Suffix VARCHAR(100);
	DECLARE @part4 VARCHAR(50);
	DECLARE @part3 VARCHAR(50);
	DECLARE @part2 VARCHAR(50);
	DECLARE @part1 VARCHAR(50);

	-- Add the T-SQL statements to compute the return value here
	SET @name = REPLACE(@name,'.',''); --remove periods from name
	SET @name = REPLACE(@name,', ','.'); --replace comma & space with a period
	SET @name = REPLACE(@name,',','.'); --replace single comma with period
	SET @name = REPLACE(@name,' ','.'); -- replace space with period
	
	SET @part4 = PARSENAME(@name,4);
	SET @part3 = PARSENAME(@name,3);
	SET @part2 = PARSENAME(@name,2);
	SET @part1 = PARSENAME(@name,1);

	--SET @LastName = PARSENAME(@name,4);

	IF ISNULL(@part4,'') = '' and ISNULL(@part3,'') = ''
	BEGIN
		SET @LastName = @part2
		SET @FirstName = @part1
	END
	ELSE IF ISNULL(@part4,'') = ''
	BEGIN
		SET @LastName = @part3
		SET @FirstName = @part2
		SET @MiddleName = @part1
	END
	ELSE
	BEGIN
		SET @LastName = @part4
		SET @Suffix = @part3
		SET @FirstName = @part2
		SET @MiddleName = @part1
	END

	SELECT @Result = CASE @namepart
		WHEN 'LAST' THEN @LastName
		WHEN 'FIRST' THEN @FirstName
		WHEN 'MIDDLE' THEN @MiddleName
		WHEN 'SUFFIX' THEN @Suffix
	END

	-- Return the result of the function
	RETURN @Result

END

