-- =============================================
-- Author:		Bradley Powers
-- Create date: 02/14/2014
-- Description:	Rebuilds A/R History for a date
-- =============================================
CREATE PROCEDURE usp_prg_ar_rebuild 
	-- Add the parameters for the stored procedure here
	@today DATETIME 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--DECLARE @today datetime;
	DECLARE @dayend datetime;
	DECLARE @arCount int;

	SET @today = DATEADD(DAY,0,DATEDIFF(DAY,0,@today));  --set the date to reproduce A/R
	SET @dayend = DATEADD(DAY, DATEDIFF(DAY, 0, @today), 0) + '23:59:59'

	--check to see if aging has already been run today. If so, delete those records
	select @arCount = COUNT(*)
	FROM [aging_history]
	WHERE [datestamp] = @today;

	IF @arCount > 0
	BEGIN
		--aging has already been run - delete records in aging_history for this day
		DELETE FROM [aging_history]
		WHERE [datestamp] = @today;

	END

	--get accounts
	; WITH cteAccount ([account], [pat_name], [cl_mnem], [fin_code], [trans_date], [status])
	AS
	(
		SELECT [account], [pat_name], [cl_mnem], [fin_code], [trans_date], [status]
		FROM [acc] 
		where status <> 'CLOSED'
	),
	--get charge balance and count of charges
	cteChrg ([account], [ChargeTotal])
	AS
	(
	select [chrg].[account], SUM([qty]*[amount]) AS 'chrgtotal'
		from [chrg] join [amt] on [chrg].[chrg_num] = [amt].[chrg_num]
		WHERE [chrg].[status] NOT IN ('CBILL','CAP','N/A') and [amt].[mod_date] < @today+1
			and [chrg].[account] IN (select account from cteAccount)
		GROUP BY [chrg].[account]
	),
	--get total of payment records
	cteChk ([account], [ChkTotal])
	AS
	(
	select [chk].[account], SUM(ISNULL([amt_paid],0.00)+ISNULL([write_off],0.00)+ISNULL([contractual],0.00)) as 'chktotal'
		from chk
		WHERE 
			[chk].[status] <> 'CBILL' AND [chk].[mod_date] < @today+1
			and [chk].[account] IN (select account from cteAccount)
		GROUP BY [chk].[account]
	)

	--combine the results
	insert into [aging_history] ([account], [datestamp], [balance], [fin_code], [ins_code])
	select [acc].[account]
		, @today as datestamp
		, ISNULL([chrg].[ChargeTotal],0.00)-ISNULL([chk].[ChkTotal],0.00) AS 'Balance'
		, dbo.GetAccFincodeByDate(@dayend,[acc].[account]) as fin_code
		, dbo.GetAccPayorByDate(@dayend,[acc].[account]) as ins_code
	FROM cteAccount acc
	LEFT OUTER JOIN cteChrg chrg on [acc].[account] = [chrg].[account]
	LEFT OUTER JOIN cteChk chk on [acc].[account] = [chk].[account]
	WHERE (ISNULL([chrg].[ChargeTotal],0.00)-ISNULL([chk].[ChkTotal],0.00))<> 0.00;
END
