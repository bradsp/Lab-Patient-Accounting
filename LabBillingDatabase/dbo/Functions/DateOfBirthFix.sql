-- =============================================
-- Author:		David
-- Create date: 12/23/2013
-- Description:	Format Dates of birth
-- =============================================
CREATE FUNCTION [dbo].[DateOfBirthFix] 
(
	-- Add the parameters for the function here
	@dob varchar(12)
)
RETURNS varchar(10)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @year varchar(4)
	DECLARE @mo	VARCHAR(2)
	DECLARE @day VARCHAR(2)

	SET @dob = LTRIM(RTRIM(@dob))

	IF (LEN(@dob) = 10 AND CHARINDEX('/',@dob) = 0)
	BEGIN
		SELECT @dob = SUBSTRING(@dob,0,9)
		SELECT @year = SUBSTRING(@dob,0,5)
		SELECT @mo = SUBSTRING(@dob,5,2)
		SELECT @day = SUBSTRING(@dob,7,2)
		SELECT @dob = @mo+@day+@year
		
	END
	--SELECT @dob
	
	set @dob = LTRIM(RTRIM(replace(@dob,'"','')))
	DECLARE @Result varchar(10)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = case when len(@dob) = 8 and charindex('/',@dob) = 0
					then  stuff(stuff(@dob,3,0,'/'),6,0,'/')
					else 
							case when convert(datetime,@dob) >= getdate()-1
							then convert(varchar(10),dateadd(year,-100,(convert(datetime,@dob))),101)
							else convert(varchar(10),convert(datetime,@dob),101)
							end
					end

	-- Return the result of the function
	IF (@Result = '01/01/1900')
	BEGIN 
		SET @Result = NULL
	END
	RETURN @Result

END
