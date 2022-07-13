-- =============================================
-- Author:		David
-- Create date: 06/10/2014
-- Description:	Gets the ins_code from the insurance mnem or name or visit info if avaliable
-- =============================================
CREATE FUNCTION GetInsCode 
(
	-- Add the parameters for the function here
	@inscode varchar(10),
	@name VARCHAR(45),
	@fincode VARCHAR(10)
)
RETURNS varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(50)
	SET @inscode = REPLACE(@inscode,'"','')
	SET @name = REPLACE(@name,'"','')
	SET @fincode = REPLACE(@fincode,'"','')
	-- Add the T-SQL statements to compute the return value here
--1. validate the code here	
	SELECT @Result = NULL
	IF (NOT EXISTS(SELECT code , [name] , fin_code
			 FROM dbo.insc WHERE deleted = 0 
			 AND code = @inscode
			 ))
	BEGIN
		SET @Result = 'XML ERROR: insc code ['+@inscode+'] is not defined'
	END
	ELSE 
	BEGIN
		SET @Result = UPPER(@inscode)
		RETURN @Result
	END
		
--2. if code is not valid
	IF (NOT EXISTS(SELECT code , [name] , fin_code
			 FROM dbo.insc WHERE deleted = 0 
			 AND ([name] = @name ) 
			 ))
	BEGIN
		SET @Result = 'XML ERROR: insc name ['+@name+'] is not defined'
	END
	ELSE
	BEGIN
		SET @Result = UPPER((select code FROM dbo.insc WHERE deleted = 0 
			AND [name] = @name))
			RETURN @Result
			
	END

	IF (NOT EXISTS(SELECT code , [name] , fin_code
			 FROM dbo.insc WHERE (deleted = 0 AND fin_code = @fincode) 
			 ))
	BEGIN
		SET @Result = (SELECT COALESCE(@inscode,@name,@fincode))
	END
	ELSE
	BEGIN
		IF (@fincode = 'H' OR @fincode = 'L')
		BEGIN	
			SET @Result = (SELECT COALESCE(@inscode,@name,@fincode))
		END
		ELSE
		BEGIN
		IF (EXISTS
			(
				SELECT COUNT(code) 
				FROM insc 
				WHERE deleted = 0 AND fin_code = @fincode 
				GROUP BY fin_code
				HAVING COUNT(code) = 1
				))
			BEGIN	
				SET @Result = (SELECT code FROM dbo.insc WHERE deleted = 0 AND fin_code = @fincode)
			END
		ELSE
			BEGIN
				SET @Result = @inscode+' , '+ @fincode +' , '+ @name-- (select code FROM dbo.insc WHERE deleted = 0 AND fin_code = @fincode)
				--'what '+ @fincode-- 
			END
		END
	END
	
	-- Return the result of the function
	RETURN UPPER(@Result)

END
