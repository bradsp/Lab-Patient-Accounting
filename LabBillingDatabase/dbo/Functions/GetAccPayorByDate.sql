-- =============================================
-- Author:		Bradley Powers
-- Create date: 07/09/2013
-- Description:	Retrieve the payor code (ins_code
-- of an account on the date submitted.
-- =============================================
CREATE FUNCTION [dbo].[GetAccPayorByDate] 
(
	-- Add the parameters for the function here
	@SelectedDate datetime,
	@Account varchar(15)
)
RETURNS varchar(15)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(15)
	
	select TOP 1 @Result = ins_code
	from audit_ins
	where account = @Account and ins_a_b_c = 'A'
	and mod_date <= @SelectedDate
	ORDER BY mod_date DESC;

	IF @Result IS NULL
	BEGIN
		select @Result = ins_code
		from ins
		where account = @Account and ins_a_b_c = 'A'
	END
	-- Return the result of the function
	RETURN @Result

END
