-- =============================================
-- Author:		David
-- Create date: 06/5/2015
-- Description:	Gets the return value from the mapping table
-- =============================================
CREATE FUNCTION GetMappingValue 
(
	-- Add the parameters for the function here
	@return_value_type varchar(50),
	@sending_system VARCHAR(50),
	@sending_value VARCHAR(50)
)
RETURNS varchar(max)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(max)
	
	-- Add the T-SQL statements to compute the return value here
	
	SELECT @Result = NULL
	IF (NOT EXISTS(SELECT TOP(1) return_value 
			 FROM dictionary.mapping WHERE 
			dictionary.mapping.sending_system = @sending_system and
			dictionary.mapping.return_value_type = @return_value_type AND
			dictionary.mapping.sending_value = @sending_value				 												
			 ))
	BEGIN
		--SET @Result = 'MAPPING ERROR: '+@return_value_type+': Value ['+@sending_value+'] is not defined'
		SET @Result = 'K' -- PER JERRY'S EMAIL 20150626
	END
	ELSE 
	BEGIN
		SET @Result = (SELECT TOP(1) return_value 
			 FROM dictionary.mapping WHERE 
			dictionary.mapping.sending_system = @sending_system and
			dictionary.mapping.return_value_type = @return_value_type AND
			dictionary.mapping.sending_value = @sending_value				 												
			 )
		
	END
		
	
	-- Return the result of the function
	RETURN UPPER(@Result)

END
