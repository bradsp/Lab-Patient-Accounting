CREATE PROCEDURE [dbo].[usp_prg_pat_bill_update_flags]
    @thrudate datetime  
AS
	-- This procedure replaces the functionality in the PatBill program. 
	-- Uses vw_acc_pat - where mailer <> 'N' and trans_date < @thrudate
	-- order by pat_name, account
	-- errors need to be written to a log table
	-- if fin code is X, Y, W, Z, or CLIENT - do not do anything
	-- if account balance < 2.50 - do not process - write to error log
	-- update flags based on current flag
	-- if mailer == Y - update to 1
	--    mailer == 1 - update to 2
	--    mailer == 2 - update to 3
	--    mailer == P - no change
	-- if mailer is 3, 4, 5, 6 - Place on Collection List, update bd_list_date to today
	-- if mailer is 1, 2 - update last_dm to today, increment mailer
	-- if mailer is Y - update first_dm to today, update last_dm to today, set mailer to 1
	-- if mailer is P - update last_dm to today
	-- create table to record actions taken to accounts

	DECLARE @today DATETIME;	
	SET @today = DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0);

--select the accounts that are eligible to receive a statement, or will be sent to bad_debt
select vw.account, vw.mailer, vw.bd_list_date, vw.last_dm, vw.fin_code, dbo.GetAccBalByDate(vw.account,GETDATE()) as [Balance]
into #MailerTemp
from vw_acc_pat vw 
where vw.mailer <> 'N' and vw.trans_date <= @thrudate
	and vw.fin_code not in ('X','Y','W','Z','CLIENT')
	and dbo.GetAccBalByDate(vw.account,@today) > cast(dbo.GetSystemValue('small_balance_amt') as float)
	and vw.bd_list_date is null

BEGIN TRANSACTION [Tran1]
	BEGIN TRY
		-- add accounts to bad debt list if mailer is 3 or more and has not already been sent to bad debt
		update pat set bd_list_date = @today, mailer = cast(cast(mailer as int)+1 as varchar),
			mod_date = GETDATE(), mod_host = right(host_name(),50), mod_prg = 'usp_prg_pat_bill_update_flags',
			mod_user = right(suser_sname(),50)
		where pat.account in (select account from #MailerTemp where mailer in ('3','4','5','6') and bd_list_date is null)
		--add entries to system-log for updates
		insert into system_log (log_date, log_text, account)
		select GETDATE(), 'Account added to Bad Debt List and mailer flag incremented', account 
		from #MailerTemp where mailer in ('3','4','5','6') and bd_list_date is null
	
		--add accounts going to collection to bad_debt table
		insert into bad_debt 
			(account_no, debtor_first_name, debtor_last_name, st_addr_1, st_addr_2, city, state_zip, spouse, 
			phone, soc_security, license_number, employment, remarks, patient_name, remarks2,
			misc, service_date, payment_date, balance, date_entered, date_sent)
		select temp.account as account_no,
			left(dbo.GetNamePart(acc.pat_name,'FIRST'),15) as debtor_first_name,
			left(dbo.GetNamePart(acc.pat_name,'LAST'),20) as debtor_last_name,
			left(pat.pat_addr1,25) as st_addr_1,
			left(pat.pat_addr2,25) as st_addr_2,
			left(dbo.SplitCITY_ST_ZIP(pat.city_st_zip,'C'),18) as city,
			left(concat(dbo.SplitCITY_ST_ZIP(pat.city_st_zip,'S'),' ',dbo.SplitCITY_ST_ZIP(pat.city_st_zip,'Z')),15) as state_zip,
			CASE ins.relation when '02' then left(ins.holder_nme,15) else null end as spouse,
			pat.pat_phone as phone,
			pat.ssn as soc_security,
			null as license_number,
			left(ins.employer,35) as employment,
			null as remarks,
			left(acc.pat_name,20) as patient_name,
			null as remarks2,
			concat(convert(varchar,pat.dob_yyyy,101),' dob') as misc,
			acc.trans_date as service_date,
			null as payment_date,
			dbo.GetAccBalByDate(temp.account,getdate()) as balance,
			@today as date_entered,
			null as date_sent
		from #MailerTemp temp 
		left outer join acc on acc.account = temp.account
		left outer join pat on pat.account = temp.account
		left outer join ins on ins.account = temp.account and ins.ins_a_b_c = 'A'
		left outer join (select account, max(chk_date) as LastPmtDate from chk group by account) chk on chk.account = temp.account
		where temp.mailer in ('3','4','5','6') and temp.bd_list_date is null

		--update flag on accounts receiving a mailer - update last_dm date
		update pat set last_dm = @today, mailer = cast(cast(mailer as int)+1 as varchar),
			mod_date = @today, mod_host = right(host_name(),50), mod_prg = 'usp_prg_pat_bill_update_flags',
			mod_user = right(suser_sname(),50)
		where pat.account in (select account from #MailerTemp where mailer in ('1','2'))
		--add entries to system-log for updates
		insert into system_log (log_date, log_text, account)
		select GETDATE(), 'Mailer flag incremented and last_dm date updated', account 
		from #MailerTemp where mailer in ('1','2')

		--update flag on accounts receiving a mailer for the first time - update first_dm and last_dm date
		update pat set first_dm = @today, last_dm = @today, mailer = '1',
			mod_date = GETDATE(), mod_host = right(host_name(),50), mod_prg = 'usp_prg_pat_bill_update_flags',
			mod_user = right(suser_sname(),50)
		where pat.account in (select account from #MailerTemp where mailer = 'Y')
		--add entries to system_log for updates
		insert into system_log (log_date, log_text, account)
		select GETDATE(), 'Mailer flag set to 1 and first_dm, last_dm updated', account 
		from #MailerTemp where mailer = 'Y'

		--update last_dm date for accounts on payment plan
		update pat set last_dm = @today,
			mod_date = GETDATE(), mod_host = right(host_name(),50), mod_prg = 'usp_prg_pat_bill_update_flags',
			mod_user = right(suser_sname(),50)
		where pat.account IN (select account from #MailerTemp where mailer = 'P')
		--add entries to system_log for updates
		insert into system_log (log_date, log_text, account)
		select GETDATE(), 'Last_DM updated for payment plan', account 
		from #MailerTemp where mailer = 'P'
	END TRY

	BEGIN CATCH
	    SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;

		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
	END CATCH

	IF(@@TRANCOUNT > 0)
		COMMIT TRANSACTION [Tran1]

drop table #MailerTemp

RETURN 0 