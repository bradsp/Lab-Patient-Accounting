-- =============================================
-- Author:		David
-- Create date: 11/18/2013
-- Description:	Trim leading zeros from account number
-- =============================================
CREATE FUNCTION [dbo].[AccountTrim] 
(
	-- Add the parameters for the function here
	@acc varchar(17)
)
RETURNS varchar(15)
AS
BEGIN
	-- Declare the return variable here
	--declare @acc varchar(17)
	--set @acc = 'C003334444'
	DECLARE @Result varchar(15)
	SET @acc = REPLACE(REPLACE(@acc,'"',''),'&QUOT;','')

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = ltrim(rtrim(upper( 
		case when patindex('[A-Z]00%',upper(@acc)) = 1
			then stuff(@acc,2,2,'')
			else
			
			case when patindex('[A-Z]0%',upper(@acc)) = 1
				then stuff(@acc,2,1,'')
				else
			case when patindex('[A-Z][A-Z]0%',upper(@acc)) = 1
				then stuff(@acc,3,1,'')
			else @acc
			end
			end
			
		end))) 
	--select @Result
	-- Return the result of the function
	RETURN replace(@Result,'"','')

END
