
-- =============================================
-- Author:		Bradley Powers
-- Create date: 5/15/2017
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetMailProfile] 
()
RETURNS	varchar(8000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(8000)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = [value] from dbo.[system] where [key_name] = 'db_mail_profile'

	-- Return the result of the function
	RETURN @Result

END

