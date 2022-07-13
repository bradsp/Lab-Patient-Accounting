-- =============================================
-- Author:		David / Bradley
-- Create date: 03/29/2016
-- Description:	trims leading zeros from mri number, keeping the leading prefix
-- =============================================
CREATE FUNCTION [dbo].[sfn_trim_zeros_mri] 
(
	-- Add the parameters for the function here
	@param VARCHAR(50)
)
RETURNS VARCHAR(35)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(35)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = LEFT(@param,patindex('%[^A-Z]%',@param+' ')-1 )+dbo.sfn_trim_zeros(right(@param,len(@param)-patindex('%[^A-Z]%',@param+' ')+1))
	
	-- Return the result of the function
	RETURN @Result

END
