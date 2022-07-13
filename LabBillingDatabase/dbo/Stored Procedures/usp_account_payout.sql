-- =============================================
-- Author:		Bradley Powers
-- Create date: 6/28/2013
-- Description:	This script will replace aging_history. It is intended to be scheduled nightly
-- Steps:
-- 1) Update status of zero balance accounts to PAID_OUT.
-- 2) Add aging history record for each account with a balance.
-- =============================================
CREATE PROCEDURE [dbo].[usp_account_payout] 
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
		[account] varchar(20) not null,
		[datestamp] datetime,
		[balance] numeric(18,2),
		[fin_code] varchar(10),
		[ins_code] varchar(10),
		[ChargeTotal] numeric(18,2),
		[ChkTotal] numeric(18,2),
		[NumCharges] numeric(18,0),
		[trans_date] DATETIME,
		[mailer] VARCHAR(1)
	);
	--this procedure runs at midnight, so store the date as yesterday.
	--Account balance is stored as of the prior day.
	SET @today = dateadd(dd, datediff(dd, 0, getdate()-1), 0);
	
	--check to see if aging has already been run today. If so, abort
	select @arCount = COUNT(*)
	FROM [aging_history]
	WHERE [datestamp] = @today;

	IF @arCount > 0
	BEGIN
		--aging has already been run - delete records in aging_history for this day
		--PRINT N'Beginning delete of aging_history for ' + @today + N'.';
		DELETE FROM [aging_history]
		WHERE [datestamp] = @today;
		--PRINT N'Completed delete of aging_history for ' + @today + N'.';

		--format the email body
		SET @emailbody = N'Aging_History_Insert was run for '+CONVERT(varchar(10),@today, 10)+
			N' with aging_history records already present. Aging_history records were deleted.';
		DECLARE @subject1 varchar(100);
		set @subject1 = 'Aging History Insert Error [' + db_name() + ']';

		--send the email
		EXEC msdb.dbo.sp_send_dbmail
			@recipients=N'bradley.powers@wth.org',
			@body=@emailbody,
			@body_format = 'HTML',
			@subject = @subject1,
			@profile_name ='WTHMCLBILL';
--		RETURN;
	END

	begin try
		--get the accounts that are not PAID_OUT
		; WITH cteAccount ([account], [pat_name], [cl_mnem], [fin_code], [trans_date], [status])
		AS
		(
			SELECT [account], [pat_name], [cl_mnem], [fin_code], [trans_date], [status]
			FROM [acc]
			WHERE NOT([acc].[status] IN ('CLOSED')) -- removed filter for PAID_OUT status - 7/26/2013 bsp
		),
		--get charge balance and count of charges
		cteChrg ([account], [NumCharges], [ChargeTotal])
		AS
		(
		select [chrg].[account], COUNT([chrg].[chrg_num]) as 'NumCharges', SUM([qty]*[amount]) AS 'chrgtotal'
			from [chrg] join [amt] on [chrg].[chrg_num] = [amt].[chrg_num]
			--INNER JOIN cteAccount acc ON [acc].[account] = [chrg].[account]
			WHERE NOT (chrg.status IN ('CBILL','CAP','N/A')) AND chrg.account IN (select account from cteAccount)
				AND amt.mod_date < @today+1
			GROUP BY [chrg].[account]
		),
		--get total of payment records
		cteChk ([account], [ChkTotal])
		AS
		(
		select [chk].[account], SUM(ISNULL([amt_paid],0.00)+ISNULL([write_off],0.00)+ISNULL([contractual],0.00)) as 'chktotal'
			from chk
			--INNER JOIN cteAccount acc ON [acc].[account] = [chk].[account]
			WHERE chk.status <> 'CBILL' and chk.account IN (select account from cteAccount)
				AND chk.mod_date < @today+1
			GROUP BY [chk].[account]
		)
		--combine the results
		insert into @accAging ([account], [datestamp], [balance], [fin_code], [ins_code], [NumCharges], [ChargeTotal], [ChkTotal], [trans_date],[mailer])
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
			, [pat].[mailer]
		FROM cteAccount acc
		LEFT OUTER JOIN (select account, ins_code from [ins] where [ins_a_b_c]='A') ins on [acc].[account] = [ins].[account]
		LEFT OUTER JOIN cteChrg chrg on [acc].[account] = [chrg].[account]
		LEFT OUTER JOIN cteChk chk on [acc].[account] = [chk].[account]
		LEFT OUTER JOIN pat ON pat.account = acc.account
		--WHERE ([ins].[ins_a_b_c] IN ('A','',NULL))
		PRINT N'Completed insert of cte tables into accaging.';
		--output records to be PAID_OUT
--		SELECT * FROM @accAging
--		WHERE [balance] = 0 AND ([NumCharges] > 0 OR [trans_date] < getdate()-15);

		PRINT N'Select count of zero balance accounts';
		SELECT COUNT(*) AS 'Total Accts Paid Out' FROM @accAging
		WHERE [balance] = 0.00 
			AND ([NumCharges] > 0 OR [trans_date] < getdate()-15)
			AND [fin_code]<>'CLIENT';

		PRINT N'Start payout of zero balance accounts';
		--PAY OUT zero balances, UPDATE acc status to PAID_OUT
		UPDATE [acc] SET [acc].[status] = 'PAID_OUT',
		[acc].[mod_prg] = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL PROCEDURE'),50)
		WHERE [acc].[account] IN
		(SELECT [account]
		FROM @accAging 
		WHERE [balance] = 0.00
			AND ([NumCharges] > 0 OR [trans_date] < getdate()-15) --if there are no charge records, wait 15 days before paying out
			AND [fin_code]<>'CLIENT' --do not pay out CLIENT accounts
		)
		and status <> 'PAID_OUT' -- wdk updating 2,054,387 on 8/12/2013 so I added this.

		PRINT N'End payout of zero balance accounts';

		--output records to be inserted into aging_history
--		SELECT * FROM @accAging
--		WHERE balance <> 0;

		SELECT COUNT(account) as 'Number of Active Accts', SUM(balance) as 'Total A/R'
		FROM @accAging
		WHERE [balance] <> 0.00;

		PRINT N'Begin insert of active accts into aging_history';
		--INSERT records into aging_history
		INSERT INTO [aging_history] ([account], [datestamp], [balance], [fin_code], [ins_code],[mailer])
		SELECT [account], [datestamp], [balance], [fin_code], [ins_code],[mailer]
		FROM @accAging
		WHERE [balance] <> 0.00;

		PRINT N'Complete insert of active accts into aging_history';

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
			N'Aging Accounts update has failed for '+ CONVERT(varchar(10),@today, 10) +
			N'.<br /><br /> Error Number: ' + @ErrorNumber +
			N'<br /> Error Message: ' + @ErrorMessage +
			N'<br /> Error Line: ' + @ErrorLine +
			N'<br /><br /> Contact Bradley Powers @ 731-541-7373';

		DECLARE @subject_text varchar(100);
		Set @subject_text = 'Aging Accounts Update - ERROR [' + db_name() + ']';

		--send the email
		EXEC msdb.dbo.sp_send_dbmail
			@recipients=N'mcloperations@wth.org',
			@copy_recipients=N'bradley.powers@wth.org',
			@body=@emailbody,
			@body_format = 'HTML',
			@subject = @subject_text,
			@profile_name ='WTHMCLBILL';

		
		--raiserror ('usp_account_payout: %d: %s', 16, 1, @ErrorNumber, @ErrorMessage);

	end catch

END
