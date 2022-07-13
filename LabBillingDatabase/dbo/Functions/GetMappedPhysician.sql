-- =============================================
-- Author:		David
-- Create date: 06/5/2015
-- Description:	Gets the return value from the mapping table
-- =============================================
CREATE FUNCTION GetMappedPhysician
(
	-- Add the parameters for the function here
	@return_value_type varchar(50),
	@sending_system VARCHAR(50),
	@ordering_phy VARCHAR(50), --	PV1.17.1[1] admitting/ordering
	@admitting_phy VARCHAR(50), --	PV1.17.1[2] admitting/ordering second instance
	@primary_care_phy VARCHAR(50), -- PD1.4.1 
	@attending_phy VARCHAR(50) -- PV1.7.1
	
)
RETURNS varchar(15)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(15)
	
	-- Add the T-SQL statements to compute the return value here
	
	SELECT @Result = 
		(SELECT dbo.GetMappingValue(@return_value_type,@sending_system
			,@ordering_phy))

	IF (@Result = 'K')
	BEGIN
		SELECT @Result = 
		(SELECT dbo.GetMappingValue(@return_value_type,@sending_system
			,@admitting_phy))
	END
	--RETURN @Result
	
	IF (@Result = 'K')
	BEGIN
		SELECT @Result = 
		(SELECT dbo.GetMappingValue(@return_value_type,@sending_system
			,@primary_care_phy))
	END
	
	IF (@Result = 'K')
	BEGIN
		SELECT @Result = 
		(SELECT dbo.GetMappingValue(@return_value_type,@sending_system
			,@attending_phy))
		
	END
--	ELSE
--	BEGIN
--	END
	
	
--	IF (NOT EXISTS(SELECT TOP(1) return_value 
--			 FROM dictionary.mapping WHERE 
--			dictionary.mapping.sending_system = @sending_system and
--			dictionary.mapping.return_value_type = @return_value_type AND
--			dictionary.mapping.sending_value = @Result				 												
--			 ))
--	BEGIN
--		--SET @Result = 'MAPPING ERROR: '+@return_value_type+': Value ['+@sending_value+'] is not defined'
--		SET @Result = 'K' -- PER JERRY'S EMAIL 20150626
--	END
--	ELSE 
--	BEGIN
--		SET @Result = (SELECT TOP(1) return_value 
--			 FROM dictionary.mapping WHERE 
--			dictionary.mapping.sending_system = @sending_system and
--			dictionary.mapping.return_value_type = @return_value_type AND
--			dictionary.mapping.sending_value = @Result				 												
--			 )
--		
--	END
		
	
	-- Return the result of the function
	RETURN UPPER(@Result)

END
