-- =============================================
-- Author:		David
-- Create date: 05/25/2015
-- Description:	converts the physican to NPI
-- =============================================
CREATE FUNCTION XmlFormatPhysicianCerner
(
	-- Add the parameters for the function here
	@Input varchar(50)
)
RETURNS varchar(10)
AS
BEGIN
	-- Declare the return variable here
--	DECLARE @Input VARCHAR(50)
--	SET @Input = 'zJE1 -JENKINS, JOHN'
	-- Add the T-SQL statements to compute the return value here

	IF (ASCII(LEFT(@Input,1)) = 122 )
	BEGIN
		SELECT @Input = LTRIM(RTRIM(UPPER(SUBSTRING(@Input,2,LEN(@Input)) )))
		--SELECT @Input
	END
	DECLARE @Mnem VARCHAR(15)
	DECLARE @Name VARCHAR(50)
	
	DECLARE @Result varchar(100)
	DECLARE @nDex INT
	DECLARE @Len INT
	SET @Len = LEN(@Input)
	SET @nDex = CHARINDEX('-',@Input)
	SET @Result = @Input
	
	IF (@nDex > 0)
	BEGIN	
		SELECT @Mnem = LTRIM(RTRIM(SUBSTRING(@Input,1,@nDex-1)))
		SET @Len = @Len - @nDex 
		
		SELECT @Name = LTRIM(RTRIM(SUBSTRING(@Input,@nDex+1,@Len)))
		
		IF (EXISTS(SELECT tnh_num FROM phy WHERE dbo.phy.mt_mnem = @Mnem))
		BEGIN 
			SELECT @Result = (SELECT tnh_num FROM phy WHERE dbo.phy.mt_mnem = @Mnem)
		END
		
		IF (@Result = @Input)
		BEGIN
			SELECT @Result = (SELECT tnh_num FROM phy WHERE dbo.phy.ov_code = @Mnem)
		END
		
		
		IF (@Result = @Input)
		BEGIN
		SELECT @Result = (SELECT tnh_num FROM phy 
				WHERE last_name + ', '+first_name +' '+ ISNULL(mid_init,'')	
				= @Name)													
		
		END
	
		
	END
	
	-- Return the result of the function
	IF (@Result = @Input) -- couldn't find it so return null
	BEGIN
		SET @Result = NULL
	END
	RETURN @Result

END
