-- =============================================
-- Author:		David
-- Create date: 12/17/2013
-- Description:	Format the SSN
-- =============================================
CREATE FUNCTION [dbo].[FormatSSN] 
(
	-- Add the parameters for the function here
	@ssn varchar(13)
)
RETURNS varchar(13)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(13)
	SET @ssn = REPLACE(REPLACE(@ssn,'-',''),'"','')

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = ltrim(rtrim(
		case when len(@ssn) = 9
			then stuff(stuff(@ssn,6,0,'-'),4,0,'-')
			else NULL
			end 
		))
	-- Return the result of the function
	RETURN NULLIF(REPLACE(@Result,'-',''),'')

END
