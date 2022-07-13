-- =============================================
-- Author:		Bradley Powers
-- Create date: 7/9/2013
-- Description:	This function scans audit_acc for 
-- date and account submitted and returns the fincode
-- on that date.
-- =============================================
CREATE FUNCTION [dbo].[GetAccFincodeByDate] 
(
	-- Add the parameters for the function here
	@SelectedDate datetime,
	@Account varchar(15)
)
RETURNS varchar(10)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(10);

	select TOP 1 @Result = fin_code
	from audit_acc
	where account = @Account
	and mod_date <= @SelectedDate
	ORDER BY mod_date DESC;

	IF @Result IS NULL
	BEGIN
		select @Result = fin_code
		from acc
		where account = @Account
	END

	-- Return the result of the function
	RETURN @Result;

END
