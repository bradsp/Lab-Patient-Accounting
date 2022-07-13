-- =============================================
-- Author:		David Kelly
-- Create date: 08/14/2014
-- Description:	Returns the Total charges for an account
-- =============================================
CREATE FUNCTION [dbo].[GetAccTotalCount] 
(
	-- Add the parameters for the function here
	@account varchar(15)
	
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Count INT;

	WITH cteChrg ([account], [ChargeTotal])
	AS
	(
	select [chrg].[account]
	,SUM(qty) AS 'chrgtotal'
		from [chrg] 
		--join [amt] on [chrg].[chrg_num] = [amt].[chrg_num]
		WHERE [chrg].[status] NOT IN ('CBILL','CAP','N/A') 
		AND cdm <> 'CBILL'
			and [chrg].[account] = @account
		GROUP BY [chrg].[account]
	)
	SELECT @Count = ISNULL(cteChrg.ChargeTotal,0)
	FROM cteChrg 
	
	-- Return the result of the function
	RETURN ISNULL(@Count,0)

END
