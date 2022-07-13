-- =============================================
-- Author:		Bradley Powers
-- Create date: 6/28/2013
-- Description:	This script will replace aging_history. It is intended to be scheduled nightly
-- Steps:
-- 1) Update status of zero balance accounts to PAID_OUT.
-- 2) Add aging history record for each account with a balance.
-- =============================================
CREATE PROCEDURE [dbo].[usp_account_payout_david_x] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	-- select accounts with status <> PAID_OUT and check balance.
	-- If balance is zero and charges exists - mark PAID_OUT
	-- If balance is zero and charges do not exist and account is > 15 days old, mark PAID_OUT
	DECLARE @today datetime;
	DECLARE @arCount int;
	DECLARE @emailbody varchar(3000);
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
	--this procedure runs at midnight, so store the date as yesterday.
	--Account balance is stored as of the prior day.
	SET @today = dateadd(dd, datediff(dd, 0, getdate()-1), 0);
	--check to see if aging has already been run today. If so, abort
	select @arCount = COUNT(*)
	FROM [aging_history_david]
	WHERE [datestamp] = @today;

	IF @arCount > 0
	BEGIN
		--aging has already been run - delete records in aging_history for this day
		DELETE FROM [aging_history_david]
		WHERE [datestamp] = @today;

		--format the email body
		SET @emailbody = N'DAVID Aging_History_Insert was run for '+CONVERT(varchar(10),@today, 10)+
			N' with aging_history records already present. Aging_history records were deleted.';

		--send the email
		EXEC msdb.dbo.sp_send_dbmail
			@recipients=N'bradley.powers@wth.org; david.kelly@wth.org',
			@body=@emailbody,
			@body_format = 'HTML',
			@subject ='DAVID Aging History Insert Error [MCLLIVE]',
			@profile_name = 'WTHMCLBILL';
--		RETURN;
	END

	begin try
		--get the accounts that are not PAID_OUT
		; WITH cteAccount ([account], [pat_name], [cl_mnem], [fin_code], [trans_date], [status])
		AS
		(
			SELECT [account], [pat_name], [cl_mnem], [fin_code], [trans_date], [status]
			FROM [acc]
			WHERE NOT([acc].[status] IN ('PAID_OUT','CLOSED'))
		),
		--get charge balance and count of charges
		cteChrg ([account], [NumCharges], [ChargeTotal])
		AS
		(
		select [chrg].[account], COUNT([chrg].[chrg_num]) as 'NumCharges', SUM([qty]*[amount]) AS 'chrgtotal'
			from [chrg] join [amt] on [chrg].[chrg_num] = [amt].[chrg_num]
			INNER JOIN cteAccount acc ON [acc].[account] = [chrg].[account]
			WHERE NOT (chrg.status IN ('CBILL','CAP','N/A'))
			GROUP BY [chrg].[account]
		),
		--get total of payment records
		cteChk ([account], [ChkTotal])
		AS
		(
		select [chk].[account], SUM([amt_paid]+[write_off]+[contractual]) as 'chktotal'
			from chk
			INNER JOIN cteAccount acc ON [acc].[account] = [chk].[account]
			GROUP BY [chk].[account]
		)

		--combine the results
		insert into @accAging ([account], [datestamp], [balance], [fin_code], [ins_code], [NumCharges], [ChargeTotal], [ChkTotal], [trans_date])
		--get a listing of accounts to be PAID_OUT
		select [acc].[account]
			, @today
			, ISNULL([chrg].[ChargeTotal],0.00)-ISNULL([chk].[ChkTotal],0.00) AS 'Balance'
			, [acc].[fin_code]
			, [ins].[ins_code]
			, ISNULL([chrg].[NumCharges],0) as 'NumCharges'
			, ISNULL([chrg].[ChargeTotal],0.00) as 'ChargeTotal'
			, ISNULL([chk].[ChkTotal],0.00) as 'ChkTotal'
			, [acc].[trans_date]
		FROM cteAccount acc
		LEFT OUTER JOIN [ins] on [acc].[account] = [ins].[account]
		LEFT OUTER JOIN cteChrg chrg on [acc].[account] = [chrg].[account]
		LEFT OUTER JOIN cteChk chk on [acc].[account] = [chk].[account]
		WHERE ([ins].[ins_a_b_c] = 'A')

		--output records to be PAID_OUT
--		SELECT * FROM @accAging
--		WHERE [balance] = 0 AND ([NumCharges] > 0 OR [trans_date] < getdate()-15);

		SELECT COUNT(*) AS 'Total Accts Paid Out' FROM @accAging
		WHERE [balance] = 0.00 AND ([NumCharges] > 0 OR [trans_date] < getdate()-15);

		--PAY OUT zero balances, UPDATE acc status to PAID_OUT
/* don't do this in test on live
		UPDATE [acc] SET [acc].[status] = 'PAID_OUT'
		WHERE [acc].[account] IN
		(SELECT [account]
		FROM @accAging 
		WHERE [balance] = 0.00
			AND ([NumCharges] > 0 OR [trans_date] < getdate()-15) --if there are no charge records, wait 15 days before paying out
		)
*/

		--output records to be inserted into aging_history
--		SELECT * FROM @accAging
--		WHERE balance <> 0;

		SELECT COUNT(account) as 'Number of Active Accts', SUM(balance) as 'Total A/R'
		FROM @accAging
		WHERE [balance] <> 0.00;

		--INSERT records into aging_history
		INSERT INTO [aging_history_david] ([account], [datestamp], [balance], [fin_code], [ins_code])
		SELECT [account], [datestamp], [balance], [fin_code], [ins_code]
		FROM @accAging
		WHERE [balance] <> 0.00;

		--send an email to operators letting them know it is OK to post charges
		--format the email body
		SET @emailbody = 
			N'Aging Accounts update has completed successfully for '+ CONVERT(varchar(10),@today, 10) +
			N'.<br /> You may now post charges.';

		--send the email
		EXEC msdb.dbo.sp_send_dbmail
			@recipients=N'david.kelly@wth.org',
			@copy_recipients=N'bradley.powers@wth.org',
			@body=@emailbody,
			@body_format = 'HTML',
			@subject ='DAVID Aging Accounts Update - COMPLETE [MCLLIVE]',
			@profile_name ='WTHMCLBILL';

	end try
	begin catch
		DECLARE @ErrorNumber varchar(20);
		DECLARE @ErrorMessage varchar(1000);
		DECLARE @ErrorLine varchar(20);

		SELECT 
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorLine = ERROR_LINE();
			

		--format the email body
		SET @emailbody = 
			N'DAVID Aging Accounts update has failed for '+ CONVERT(varchar(10),@today, 10) +
			N'.<br /><br /> Error Number: ' + @ErrorNumber +
			N'<br /> Error Message: ' + @ErrorMessage +
			N'<br /> Error Line: ' + @ErrorLine +
			N'<br /><br /> DO NOT RUN POST CHARGES! Contact David Kelly @ 731-660-1925';

		--send the email
		EXEC msdb.dbo.sp_send_dbmail
			@recipients=N'david.kelly@wth.org',
			@copy_recipients=N'bradley.powers@wth.org',
			@body=@emailbody,
			@body_format = 'HTML',
			@subject ='DAVID Aging Accounts Update - ERROR [MCLLIVE]',
			@profile_name ='WTHMCLBILL';

		
		--raiserror ('usp_account_payout: %d: %s', 16, 1, @ErrorNumber, @ErrorMessage);

	end catch

END
