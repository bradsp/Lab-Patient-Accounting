-- =============================================
-- Author:		Bradley Powers
-- Create date: 04/24/2014
-- Description:	Returns the account balance based on the date
-- =============================================
CREATE FUNCTION [dbo].[GetAccBalByServiceDate] 
(
	-- Add the parameters for the function here
	@account varchar(15),
	@effDate DATETIME
)
RETURNS NUMERIC(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Balance NUMERIC(18,2);

	WITH cteChrg ([account], [ChargeTotal])
	AS
	(
	select [chrg].[account], SUM([qty]*[amount]) AS 'chrgtotal'
		from [chrg] join [amt] on [chrg].[chrg_num] = [amt].[chrg_num]
		WHERE [chrg].[status] NOT IN ('CBILL','CAP','N/A') and [chrg].[service_date] < @effDate
			and [chrg].[account] = @account
		GROUP BY [chrg].[account]
	),
	--get total of payment records
	cteChk ([account], [ChkTotal])
	AS
	(
	select [chk].[account], SUM(ISNULL([amt_paid],0.00)+ISNULL([write_off],0.00)+ISNULL([contractual],0.00)) as 'chktotal'
		from chk
		WHERE 
			[chk].[status] <> 'CBILL' AND [chk].[mod_date] < @effDate
			and [chk].[account] = @account
		GROUP BY [chk].[account]
	)

	SELECT @Balance = ISNULL(cteChrg.ChargeTotal,0.00) - ISNULL(cteChk.ChkTotal,0.00)
	FROM cteChrg LEFT OUTER JOIN cteChk ON cteChk.account = cteChrg.account

	-- Return the result of the function
	RETURN @Balance

END
