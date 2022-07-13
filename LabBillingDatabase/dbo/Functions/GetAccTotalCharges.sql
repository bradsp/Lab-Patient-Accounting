-- =============================================
-- Author:		David Kelly
-- Create date: 08/14/2014
-- Description:	Returns the Total charges for an account
-- =============================================
CREATE FUNCTION [dbo].[GetAccTotalCharges] 
(
	-- Add the parameters for the function here
	@account varchar(15)
	
)
RETURNS NUMERIC(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Balance NUMERIC(18,2);

	WITH cteChrg ([account], [ChargeTotal])
	AS
	(
	select [chrg].[account]
	--, SUM([qty]*[amount])
	,SUM(calc_amt) AS 'chrgtotal'
		from [chrg] 
		--join [amt] on [chrg].[chrg_num] = [amt].[chrg_num]
		WHERE [chrg].[status] NOT IN ('CBILL','CAP','N/A') 
		AND cdm <> 'CBILL'
			and [chrg].[account] = @account
		GROUP BY [chrg].[account]
	)
	SELECT @Balance = ISNULL(cteChrg.ChargeTotal,0.00)
	FROM cteChrg 
	
	-- Return the result of the function
	RETURN ISNULL(@Balance,0.00)

END
