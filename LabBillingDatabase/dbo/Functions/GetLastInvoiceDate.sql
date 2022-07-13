-- =============================================
-- Author:		Bradley Powers
-- Create date: 1/4/2018
-- Description:	Get the latest invoice date for a client
-- =============================================
CREATE FUNCTION [dbo].[GetLastInvoiceDate] 
(
	-- Add the parameters for the function here
	@cli_mnem VARCHAR(15)
)
RETURNS DATETIME
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result DATETIME

	-- Add the T-SQL statements to compute the return value here
	SELECT @result = MAX(thru_date)
	FROM dbo.cbill_hist
	WHERE cl_mnem = @cli_mnem
	--GROUP BY cl_mnem
	-- Return the result of the function
	RETURN @result

END
