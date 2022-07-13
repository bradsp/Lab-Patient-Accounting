-- =============================================
-- Author:		Bradley Powers
-- Create date: 11/22/2019
-- Description:	Returns the system value for given key_name
-- =============================================
CREATE FUNCTION [dbo].[GetSystemValue] 
(
	-- Add the parameters for the function here
	@key_name varchar(25)
)
RETURNS varchar(8000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(8000)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = [value] from dbo.system where key_name = @key_name

	-- Return the result of the function
	RETURN @Result

END
