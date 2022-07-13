-- =============================================
-- Author:		Bradley Powers
-- Create date: 7/11/2013
-- Description:	Generate accounts receivable list for a particular date. This will report what the
--	actual A/R was for a date and list the accounts and balances that made up the A/R on that
--	date. Used to regenerate aging_history.
-- =============================================
CREATE PROCEDURE [dbo].[usp_UpdateARHistoryByDate] 
	-- Add the parameters for the stored procedure here
	@arDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @today datetime;
	DECLARE @dayend datetime;
	DECLARE @arCount int;
	DECLARE @accAging TABLE
	(
		[account] varchar(20),
		[datestamp] datetime,
		[balance] numeric(18,2),
		[fin_code] varchar(10),
		[ins_code] varchar(10),
		[ChargeTotal] numeric(18,2),
		[ChkTotal] numeric(18,2),
		[NumCharges] numeric(18,0),
		[trans_date] datetime
	);
	
	SET @today = dateadd(dd, datediff(dd, 0, @arDate), 0);
	SET @dayend = dateadd(day, datediff(day, 0, @today), 0) + '23:59:59.999'

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

	--get all accounts
	; WITH cteAccount ([account], [pat_name], [cl_mnem], [fin_code], [trans_date], [status])
	AS
	(
		SELECT [account], [pat_name], [cl_mnem], [fin_code], [trans_date], [status]
		FROM [acc]
	),
	--get charge balance and count of charges
	cteChrg ([account], [NumCharges], [ChargeTotal])
	AS
	(
	select [chrg].[account], COUNT([chrg].[chrg_num]) as 'NumCharges', SUM([qty]*[amount]) AS 'chrgtotal'
		from [chrg] join [amt] on [chrg].[chrg_num] = [amt].[chrg_num]
		INNER JOIN cteAccount acc ON [acc].[account] = [chrg].[account]
		WHERE NOT ([chrg].[status] IN ('CBILL','CAP','N/A')) and [amt].[mod_date] < @today+1
		GROUP BY [chrg].[account]
	),
	--get total of payment records
	cteChk ([account], [ChkTotal])
	AS
	(
	select [chk].[account], SUM([amt_paid]+[write_off]+[contractual]) as 'chktotal'
		from chk
		INNER JOIN cteAccount acc ON [acc].[account] = [chk].[account]
		WHERE 
			[chk].[status] <> 'CBILL' AND [chk].[mod_date] < @today+1
		GROUP BY [chk].[account]
	)

	--combine the results
	insert into @accAging ([account], [datestamp], [balance], [fin_code], [ins_code], [NumCharges], [ChargeTotal], [ChkTotal], [trans_date])
	select [acc].[account]
		, @today as datestamp
		, ISNULL([chrg].[ChargeTotal],0.00)-ISNULL([chk].[ChkTotal],0.00) AS 'Balance'
		, dbo.GetAccFincodeByDate(@dayend,[acc].[account]) as fin_code
		, dbo.GetAccPayorByDate(@dayend,[acc].[account]) as ins_code
		, ISNULL([chrg].[NumCharges],0) as 'NumCharges'
		, ISNULL([chrg].[ChargeTotal],0.00) as 'ChargeTotal'
		, ISNULL([chk].[ChkTotal],0.00) as 'ChkTotal'
		, [acc].[trans_date]
	FROM cteAccount acc
	LEFT OUTER JOIN cteChrg chrg on [acc].[account] = [chrg].[account]
	LEFT OUTER JOIN cteChk chk on [acc].[account] = [chk].[account]

	--INSERT records into aging_history
	INSERT INTO [aging_history] ([account], [datestamp], [balance], [fin_code], [ins_code])
	SELECT [account], [datestamp], [balance], [fin_code], [ins_code]
	FROM @accAging
	WHERE [balance] <> 0.00;
	
END
