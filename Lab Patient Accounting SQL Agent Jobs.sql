USE [msdb]
GO

/****** Object:  Job [PROD Table Purge LabBillingProd]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'617df539-3d53-4503-bf67-51c737a8ec57', @delete_unused_schedule=1
GO

/****** Object:  Job [PROD PSA Daily Report Email]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'f5d6137b-502b-4304-b160-23ffee7497c5', @delete_unused_schedule=1
GO

/****** Object:  Job [PROD Diagnosis ORM]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'c198ae81-031f-42f0-9085-bebabcb20200', @delete_unused_schedule=1
GO

/****** Object:  Job [PROD Daily Hourly Updates]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'75e37ea6-46e1-49c5-881d-378150271c11', @delete_unused_schedule=1
GO

/****** Object:  Job [PROD Daily Billing Updates]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'334cc447-8ec6-4666-98de-ac8ff77f1dfc', @delete_unused_schedule=1
GO

/****** Object:  Job [PROD Daily AM Run]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'84256018-4a10-4a68-8102-f368aefdc892', @delete_unused_schedule=1
GO

/****** Object:  Job [PROD BadDebt Account Writeoff]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'3bedd91e-370c-4394-860c-cc7baccb4da5', @delete_unused_schedule=1
GO

/****** Object:  Job [nLog Table Purge]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'f34f9fa1-4fea-43c8-8886-a6307dc94570', @delete_unused_schedule=1
GO

/****** Object:  Job [Accounts Aging Payout]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC msdb.dbo.sp_delete_job @job_id=N'e404652f-7068-45e6-9668-274755f55c25', @delete_unused_schedule=1
GO

/****** Object:  Job [Accounts Aging Payout]    Script Date: 5/14/2023 12:29:46 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:46 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'Accounts Aging Payout', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'This job runs the nightly aging accounts stored procedure. This procedure pays out zero balance accounts, and records the account balance at the end of the prior day in aging_history.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'Bradley Powers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [LabBillingProd - Execute usp_account_payout]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'LabBillingProd - Execute usp_account_payout', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC dbo.usp_account_payout', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\MCLLIVE - aging account payout.txt', 
		@flags=2
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [LabBillingTest - Aging Account Payout]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'LabBillingTest - Aging Account Payout', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC dbo.usp_account_payout', 
		@database_name=N'LabBillingTest', 
		@output_file_name=N'H:\sqlText\LabBillingTest-aging-account-payout.txt', 
		@flags=2
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily at midnight', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20130701, 
		@active_end_date=99991231, 
		@active_start_time=200, 
		@active_end_time=235959, 
		@schedule_uid=N'90925af7-7567-415d-b41e-6bc54e16312b'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [nLog Table Purge]    Script Date: 5/14/2023 12:29:46 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:46 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'nLog Table Purge', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Purge Logs]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Purge Logs', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'delete from Logs where CreatedOn < getdate()-7', 
		@database_name=N'NLog', 
		@output_file_name=N'H:\SqlText\NLogPurge.txt', 
		@flags=2
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'NLog Purge Nightly', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20230208, 
		@active_end_date=99991231, 
		@active_start_time=231300, 
		@active_end_time=235959, 
		@schedule_uid=N'6cf1ccbf-e5fa-423e-83a8-7be96a2e7058'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [PROD BadDebt Account Writeoff]    Script Date: 5/14/2023 12:29:46 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:46 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'PROD BadDebt Account Writeoff', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Enters a chk record on the BADDEBT account at the end of every month to zero out the account.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'Bradley Powers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Enter chk record]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Enter chk record', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @beginDate DATETIME;
DECLARE @endDate DATETIME;
DECLARE @woffAmt NUMERIC(18,2);
DECLARE @emailbody VARCHAR(3000);

--compute dates based on current month
DECLARE @mydate DATETIME;
SET @mydate = DATEADD(DAY, 0, DATEDIFF(DAY, 0, GETDATE()));
SET @beginDate = DATEADD(MONTH,DATEDIFF(MONTH,0,@mydate),0);
SET @endDate = DATEADD(MONTH, DATEDIFF(MONTH,-1,@mydate),-1);

--write the BADDEBT offset record
select @woffAmt = SUM(amt_paid+write_off+contractual)
from chk where account = ''BADDEBT'' AND mod_date >= @beginDate and mod_date < @endDate+1

IF @woffAmt <> 0.00
BEGIN
	insert INTO chk (account, write_off, chk_date ,date_rec, Source, chk_no,
		comment, mod_date)
	values (''BADDEBT'', @woffAmt*-1, @endDate,@endDate,''ADJUST'', ''BADDEBT'',
		''MONTHLY BADDEBT WOFF OFFSET'', @endDate)

	--format the email body
	SET @emailbody = N''Write off entry for BADDEBT account for ''+CONVERT(varchar(10),@beginDate, 10)+
		N''  through ''+CONVERT(varchar(10),@endDate, 10) + 
		N'' in the amount of ''+CONVERT(VARCHAR(20),@woffAmt)+
		N'' has been written.'';

	--send the email
	EXEC msdb.dbo.sp_send_dbmail
		@recipients=N''bradley.powers@wth.org;carol.plumlee@wth.org'',
		@body=@emailbody,
		@body_format = ''HTML'',
		@subject =''Monthly BADDEBT WOFF OFFSET [LabBillingProd]'',
		@profile_name =''WTHMCLBILL'';

END
ELSE
BEGIN
	--format the email body
	SET @emailbody = N''Write off entry for BADDEBT account for ''+CONVERT(varchar(10),@beginDate, 10)+
		N''  through ''+CONVERT(varchar(10),@endDate, 10) + 
		N'' in the amount of ''+CONVERT(varchar(20),@woffAmt)+
		N'' has NOT been written due to being zero amount.'';

	--send the email
	EXEC msdb.dbo.sp_send_dbmail
		@recipients=N''bradley.powers@wth.org;carol.plumlee@wth.org'',
		@body=@emailbody,
		@body_format = ''HTML'',
		@subject =''Monthly BADDEBT WOFF OFFSET [LabBillingProd]'',
		@profile_name =''WTHMCLBILL'';
END', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'h:\sqlText\bad_debt_writeoff_job.txt', 
		@flags=2
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Monthly', 
		@enabled=1, 
		@freq_type=32, 
		@freq_interval=8, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=16, 
		@freq_recurrence_factor=1, 
		@active_start_date=20131121, 
		@active_end_date=99991231, 
		@active_start_time=220000, 
		@active_end_time=235959, 
		@schedule_uid=N'19f64253-628f-4d37-9f66-aa6fd53067ad'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [PROD Daily AM Run]    Script Date: 5/14/2023 12:29:46 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:46 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'PROD Daily AM Run', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Performs several special reports to track the Cerner posting of data to our system. The first two steps are no longer used and have been bypassed. (too scared to remove them at that time)', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'Bradley Powers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CERNER CHARGE CHECK]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CERNER CHARGE CHECK', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/*
set nocount on
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))


set @tableHtml = 
N''<H1> CHARGES </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ROW</th><th>SYSTEM MSG ID</th><th>ACCOUNT</th>''+
N''<th>MSG DATE</th><th>MSG STATUS</th><th>MSG TEXT</th></tr>'' +

CAST (( select td = ROW_NUMBER() 
OVER (ORDER BY infce.messages_inbound.systemMsgId),'''',
	td = infce.messages_inbound.systemMsgId,'''', 
	td = infce.messages_inbound.account_cerner,'''', 
	td = CONVERT(VARCHAR,infce.messages_inbound.msgDate,101) ,'''', 
	td = infce.messages_inbound.processFlag,'''', 
	td = ISNULL(NULLIF(infce.messages_inbound.processStatusMsg,''''),''Not Processed'') ,'''' 
			   
FROM         infce.messages_inbound
WHERE   infce.messages_inbound.processFlag IN (''N'',''E'',''NP'')
AND infce.messages_inbound.msgType = ''DFT-P03''
ORDER BY infce.messages_inbound.systemMsgId
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Charges Not Processed as of  '' + convert(varchar(10),getdate(),101)
END
ELSE
BEGIN
set @sub = ''All Charges Processed as of  '' + convert(varchar(10),getdate(),101)
END
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org;jerry.barker@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', if necessary
@copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

*/', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CERNER DEMOGRAPHICS CHECK]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CERNER DEMOGRAPHICS CHECK', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
/*
set nocount on
declare @startDate datetime
declare @endDate datetime

SET @startDate = CONVERT(DATETIME,convert(varchar(10),GETDATE(),101))

--SELECT @startDate

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))


; WITH cteQ
AS
(
SELECT TOP(100) percent
--CAST(infce.messages_inbound.systemMsgId AS VARCHAR)+'','' AS [yep],
systemMsgId,infce.messages_inbound.processFlag
, infce.messages_inbound.account_cerner
, CONVERT(VARCHAR,infce.messages_inbound.msgDate,112) AS [mDate]
, infce.messages_inbound.processStatusMsg
 FROM infce.messages_inbound 
WHERE infce.messages_inbound.msgType = ''ADT-A04''
AND infce.messages_inbound.processFlag IN (''NP'',''E'',''N'')

ORDER BY infce.messages_inbound.systemMsgId

)

select @tableHtml = 
N''<H1> DEMOGRAPHICS </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>MSG DATE</th><th>PROCESS FLAG</th>''+
N''<th>DAILY ACCOUNTS</th><th>PROCESS COMMENT</tr>'' +
CAST (( select td = cteQ.mDate,'''',
			   td = cteQ.processFlag,'''', 
			   td = COUNT (cteQ.account_cerner),'''', 
			   td = cteQ.processStatusMsg,'''' 
		 
FROM cteQ
GROUP BY cteQ.mDate, cteQ.processFlag 
		,cteQ.processStatusMsg
		
ORDER BY cteQ.mDate, cteQ.processFlag 
		,cteQ.processStatusMsg
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Demographics as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', --if necessary
@copy_recipients=N''bradley.powers@wth.org;jerry.barker@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)


*/', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=12
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CERNER DAILY CDM NOT IN LEGACY BILLING]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CERNER DAILY CDM NOT IN LEGACY BILLING', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
BEGIN TRY
set nocount on
SET QUOTED_IDENTIFIER  OFF 
 

declare @startDate DATETIME
DECLARE @endDate DATETIME
set @startDate = CONVERT(DATETIME,convert(varchar(10),GETDATE()-1,101))
SET @endDate = GETDATE()

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

IF OBJECT_ID(''tempdb..#tempInfceQueryInvalidCDMInvalidCDM'',''U'') IS NOT NULL
BEGIN
	DROP TABLE #tempInfceQueryInvalidCDM
END

CREATE TABLE #tempInfceQueryInvalidCDM (
	account VARCHAR(150),
	msgid NUMERIC(18,0),
	xmlContent VARCHAR(max))
INSERT INTO #tempInfceQueryInvalidCDM
		( account, msgid, xmlContent )
SELECT ''L''+mi.account_cerner AS [account]
	, mi.systemMsgId
	--, CAST (REPLACE(mi.msgContent,''"'','''') AS XML) AS [xmlContent]
	, mi.msgContent
	FROM infce.messages_inbound mi
	WHERE mi.msgType = ''DFT-P03''
	AND mi.msgDate BETWEEN @startDate AND @endDate
--SELECT * FROM #tempInfceQueryInvalidCDMInvalidCDM
; WITH cteTemp
AS
(
	SELECT #tempInfceQueryInvalidCDM.account,
			#tempInfceQueryInvalidCDM.msgid AS [systemMsgId] ,
			CAST (REPLACE(#tempInfceQueryInvalidCDM.xmlContent ,''"'','''') AS XML) AS [xmlContent]
			FROM #tempInfceQueryInvalidCDM
)
, cteData
AS
(
SELECT r.systemMsgId,
 COALESCE(NULLIF(dbo.GetMappingValue(''CLIENT'',''CERNER'',
	COALESCE(  
	 NULLIF(LTRIM(a.alias.value(''(//PV1.3.4/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.3.1/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.3.7/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.4/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.1/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.7/text() )[1]''	, ''varchar(50)'')),'''')
	)),''K'')
	, a.alias.value(''(//PV1.3.4/text() )[1]''	, ''varchar(50)''))
	AS [CLIENT]
 ,
 ''L''+a.alias.value(''(//PID.18.1/text())					
			[1]'', ''varchar(15)'')				AS [ACCOUNT]

		,a.alias.query(''//HL7Message'') AS [xmlContent]
		
FROM (SELECT cteTemp.systemMsgId
	, xmlContent AS rep_xml FROM cteTemp) r
CROSS APPLY r.rep_xml.nodes(''HL7Message'') a(alias)
)

,cteCdm 
AS
(
SELECT cteData.systemMsgId ,
		cteData.CLIENT ,
		cteData.ACCOUNT ,
		cteData.xmlContent 
FROM cteData

)
, xCDM as
(


 SELECT 
 ''L''+a.alias.value(''(//PID.18.1/text())					
			[1]'', ''varchar(15)'')				AS [ACCOUNT],
 ft1.cdm.value(''(./FT1.1/FT1.1.1/text() ) [1]'', ''int'') AS [SET ID],
 ft1.cdm.value(''(./FT1.7/FT1.7.1/text() ) [1]'', ''varchar(7)'') AS [CDM],
 ft1.cdm.value(''(./FT1.10/FT1.10.1/text() )[1]'', ''int'' ) AS [CDM QTY],
 ft1.cdm.value(''(./FT1.7/FT1.7.2/text() ) [1]'', ''varchar(50)'') AS [CERNER DESCRIPTION]
FROM (SELECT xmlContent AS rep_xml FROM cteCdm) r
CROSS APPLY r.rep_xml.nodes	(''HL7Message'') a(alias)
CROSS APPLY	r.rep_xml.nodes(''//FT1'') AS ft1(cdm)

)
SELECT @tableHtml = 
N''<H1> Daily Invalid Charges by CDM </H1>''+
N''<H2> JOB - Daily AM Run</H2>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ITEM NO</th><th>ACCOUNT</th><th>CDM</th><th>DESCRIPTION</th><th>QTY</>''+
N''<th>CERNER DESCRIPTION</th><th>TRACKED</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY xCDM.ACCOUNT,xCDM.CDM),'''',
			   td = xCDM.ACCOUNT,'''', 
			   td = xCDM.CDM,'''', 
			   td = ISNULL(dbo.cdm.descript,xcdm.cdm+'' Not in Billing''),'''', 
			   td = SUM(xCDM.[CDM QTY]),'''',
			   td = 
			   CASE WHEN cdm.descript IS NULL
			   THEN xCDM.[CERNER DESCRIPTION] 
			   ELSE '''' END,'''',
			   --td = ISNULL(NULLIF(dbo.chrg_unprocessed.cdm,''''),''NO''),''''
			   td = dbo.chrg_unprocessed.cdm,''''

FROM xCDM		  
LEFT OUTER JOIN cdm ON dbo.cdm.cdm = xCDM.CDM
LEFT OUTER JOIN dbo.chrg_unprocessed ON dbo.chrg_unprocessed.cdm = xCDM.cdm
	AND dbo.chrg_unprocessed.account = ''CERNER''
	AND dbo.chrg_unprocessed.status = ''TABLE''
WHERE cdm.descript IS null
GROUP BY xCDM.ACCOUNT, 
xCDM.CDM, cdm.descript, xCDM.[CERNER DESCRIPTION]--WITH ROLLUP
, dbo.chrg_unprocessed.cdm
ORDER BY xCDM.cdm


for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Cerner CDM''''s not in Legacy Billing for '' + convert(varchar(10),DATEADD(DAY,-1,getdate()),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

END

DROP TABLE #tempInfceQueryInvalidCDM

SET QUOTED_IDENTIFIER ON
set nocount OFF
END TRY
BEGIN CATCH
	PRINT ERROR_MESSAGE()
END CATCH




 
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=12
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ACCOUNTS WITH INVALID CLIENTS]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ACCOUNTS WITH INVALID CLIENTS', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
set nocount on
declare @startDate datetime
declare @endDate datetime


set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

set @tableHtml = 
N''<H1> ACCOUNTS WITH "K" CLIENTS </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ROW</th><th>ACCOUNT</th><th>PATIENT</th><th>CLIENT</th><th>FIN CODE</th><th>TRANS DATE</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY dbo.acc.account),'''',
			   td = dbo.acc.account,'''', 
			   td = dbo.acc.pat_name,'''',
			   td = dbo.acc.cl_mnem,'''' ,
			   td = dbo.acc.fin_code,'''',
			   td = CONVERT(VARCHAR(10),dbo.acc.trans_date,101),''''
			   
FROM acc WHERE dbo.acc.cl_mnem = ''K''
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Accounts with "K" Clients '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

--PRINT ''$LOG FILE MSG$'' -- like CARE 360 EMAIL SENT
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CERNER DAILY CDM COUNT]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CERNER DAILY CDM COUNT', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'BEGIN TRY
set nocount on
SET QUOTED_IDENTIFIER  OFF 
 

declare @startDate DATETIME
DECLARE @endDate DATETIME
set @startDate = CONVERT(DATETIME,convert(varchar(10),GETDATE(),101))
SET @endDate = GETDATE()

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

IF OBJECT_ID(''tempdb..#tempInfceQuery'',''U'') IS NOT NULL
BEGIN
	DROP TABLE #tempInfceQuery
END

CREATE TABLE #tempInfceQuery (
	account VARCHAR(15),
	msgid NUMERIC(18,0),
	xmlContent varchar(max))
INSERT INTO #tempInfceQuery
		( account, msgid, xmlContent )
SELECT ''L''+mi.account_cerner AS [account]
	, mi.systemMsgId
	--, CAST (REPLACE(mi.msgContent,''"'','''') AS XML) AS [xmlContent]
	, mi.msgContent
	FROM infce.messages_inbound mi
	WHERE mi.msgType = ''DFT-P03''
	AND mi.msgDate BETWEEN @startDate AND @endDate
--SELECT * FROM #tempInfceQuery
; WITH cteTemp
AS
(
	SELECT #tempInfceQuery.account,
			#tempInfceQuery.msgid AS [systemMsgId] ,
			cast(#tempInfceQuery.xmlContent as XML) as [xmlContent]
 FROM #tempInfceQuery
)
, cteData
AS
(
SELECT r.systemMsgId,
 COALESCE(NULLIF(dbo.GetMappingValue(''CLIENT'',''CERNER'',
	COALESCE(  
	 NULLIF(LTRIM(a.alias.value(''(//PV1.3.4/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.3.1/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.3.7/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.4/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.1/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.7/text() )[1]''	, ''varchar(50)'')),'''')
	)),''K'')
	, a.alias.value(''(//PV1.3.4/text() )[1]''	, ''varchar(50)''))
	AS [CLIENT]
 ,
 ''L''+a.alias.value(''(//PID.18.1/text())					
			[1]'', ''varchar(15)'')				AS [ACCOUNT]

		,a.alias.query(''//HL7Message'') AS [xmlContent]
		
FROM (SELECT cteTemp.systemMsgId
	, xmlContent AS rep_xml FROM cteTemp) r
CROSS APPLY r.rep_xml.nodes(''HL7Message'') a(alias)
)

,cteCdm 
AS
(
SELECT cteData.systemMsgId ,
		cteData.CLIENT ,
		cteData.ACCOUNT ,
		cteData.xmlContent 
FROM cteData

)
, xCDM as
(


 SELECT 
 ''L''+a.alias.value(''(//PID.18.1/text())					
			[1]'', ''varchar(15)'')				AS [ACCOUNT],
 ft1.cdm.value(''(./FT1.1/FT1.1.1/text() ) [1]'', ''int'') AS [SET ID],
 ft1.cdm.value(''(./FT1.7/FT1.7.1/text() ) [1]'', ''varchar(7)'') AS [CDM],
 ft1.cdm.value(''(./FT1.10/FT1.10.1/text() )[1]'', ''int'' ) AS [CDM QTY],
 ft1.cdm.value(''(./FT1.7/FT1.7.2/text() ) [1]'', ''varchar(50)'') AS [CERNER DESCRIPTION]
FROM (SELECT xmlContent AS rep_xml FROM cteCdm) r
CROSS APPLY r.rep_xml.nodes	(''HL7Message'') a(alias)
CROSS APPLY	r.rep_xml.nodes(''//FT1'') AS ft1(cdm)

)
SELECT @tableHtml = 
N''<H1> Daily Charges by CDM </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ITEM NO</th><th>CDM</th><th>DESCRIPTION</th><th>QTY</>''+
N''<th>CERNER DESCRIPTION</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY xCDM.CDM),'''',
			   td = xCDM.CDM,'''', 
			   td = ISNULL(dbo.cdm.descript,xcdm.cdm+'' Not in Billing''),'''', 
			   td = SUM(xCDM.[CDM QTY]),'''',
			   td = 
			   COALESCE(cdm.descript,cup.chrg_err,xCDM.[CERNER DESCRIPTION])
--			   CASE
--			  	WHEN cdm.descript IS NULL THEN xCDM.[CERNER DESCRIPTION] 
--			   ELSE '''' END
				,''''


FROM xCDM		  
LEFT OUTER JOIN cdm ON dbo.cdm.cdm = xCDM.CDM
LEFT OUTER JOIN dbo.chrg_unprocessed  cup ON cup.cdm = xCDM.cdm
	AND cup.account = ''CERNER'' 
	AND cup.status = ''TABLE''
GROUP BY --xCDM.ACCOUNT, 
xCDM.CDM, cdm.descript, xCDM.[CERNER DESCRIPTION]--WITH ROLLUP
, cup.chrg_err
ORDER BY xCDM.cdm


for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Daily CDM count for '' + convert(varchar(10),DATEADD(DAY,-1,getdate()),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@copy_recipients=N''christopher.burton@wth.org;bradley.powers@wth.org; jerry.barker@wth.org'', 
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

END

DROP TABLE #tempInfceQuery

SET QUOTED_IDENTIFIER ON
set nocount OFF
END TRY
BEGIN CATCH
	PRINT ERROR_MESSAGE()
END CATCH




 
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [UPDATE ICD INDICATORS]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'UPDATE ICD INDICATORS', 
		@step_id=6, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'SET QUOTED_IDENTIFIER  ON 

UPDATE TOP(100) PERCENT dbo.pat 
SET icd_indicator = CASE WHEN acc.trans_date >= ''01/01/2016 00:00'' 
	 THEN COALESCE(REPLACE(i10.icd9_num,i10.icd9_num,''I10''),''I10'') 
	 ELSE COALESCE(REPLACE(i9.icd9_num,i9.icd9_num,''I9''),''I9'') END

FROM dbo.pat
INNER JOIN acc ON dbo.acc.account = dbo.pat.account
LEFT OUTER JOIN dbo.icd9desc i10 ON i10.AMA_year = ''2016''
	AND i10.icd9_num = REPLACE(pat.icd9_1,''.'','''')
	AND dbo.acc.trans_date >= ''10/01/2015 00:00''
LEFT OUTER JOIN dbo.icd9desc i9 ON i9.AMA_year = ''2015''
	AND acc.trans_date < ''10/01/2015 00:00''
	AND i9.icd9_num = pat.icd9_1 
WHERE acc.status = ''NEW'' AND pat.icd_indicator IS null', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=12
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [MISC REF LAB TEST]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'MISC REF LAB TEST', 
		@step_id=7, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'--C:\Documents and Settings\wkelly\My Documents\SQL Server Management Studio\Projects\5527500 worklist.sql
set nocount on
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))

set @tableHtml = 
N''<H1> MISC REF LAB TEST </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ITEM</th><th>FACILITY</th><th>ACCOUNT</th>'' +
N''<th>PATIENT</th><th>DOS</th><th>CDM</th>'' +
N''<th>QTY</th><th>COMMENT</th><th>ACCESSION NUMBER</th><th>FIN CODE</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY acc.cl_mnem, dbo.acc.trans_date, dbo.acc.account),'''',
			   td = ISNULL(dbo.chrg.facility,''UNK''),'''', 
			   td = dbo.chrg.account,'''', 
			   td = ISNULL(dbo.chrg.pat_name,''UNK''),'''', 
			   td = CONVERT(VARCHAR(10),dbo.chrg.service_date,101),'''', 
			   td = dbo.chrg.cdm ,'''', 
			   td = dbo.chrg.qty,'''', 
			   td = dbo.chrg.comment,'''', 
			   td = ISNULL(dbo.chrg.mt_req_no,''UNK''),'''', 
			   td = ISNULL(dbo.chrg.fin_code,''UNK''),'''' 
			   
--SELECT dbo.chrg.facility ,dbo.chrg.account ,dbo.chrg.pat_name ,
--CONVERT(VARCHAR(10),dbo.chrg.service_date,101) AS [DOS] ,
--	dbo.chrg.cdm ,dbo.chrg.qty ,dbo.chrg.comment ,
--	dbo.chrg.mt_req_no AS [accession_no] ,
--dbo.chrg.fin_code --,		
		FROM chrg 
		INNER JOIN acc ON acc.account = chrg.account
		WHERE cdm = ''5527500''
	AND dbo.chrg.credited = 0
	AND acc.status = ''NEW''
	AND chrg.service_date >= ''05/31/2015 00:00''
	ORDER BY acc.cl_mnem, dbo.acc.trans_date, dbo.acc.account
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Misc Ref Lab Tests as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', --if necessary
@copy_recipients=N''bradley.powers@wth.org; christopher.burton@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''MISC REF LAB TEST EMAIL SENT'' 
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=12
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ERROR PROGRAM REPORT]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ERROR PROGRAM REPORT', 
		@step_id=8, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
set nocount on
declare @startDate datetime
declare @endDate datetime


set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))


set @tableHtml = 
N''<H1> ERROR PROGRAM '' + db_name() + ''</H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ITEM NR</th><th>ACCOUNT</th><th>APPLICATION</th> ''+
N''<th>ERROR</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY dbo.error_prg.uid) ,'''',
			   td = dbo.error_prg.account,'''', 
			   td = dbo.error_prg.app_name ,'''',
			   td = dbo.error_prg.error ,''''  
			   
--		dbo.error_prg.error_type ,		
--		dbo.error_prg.app_module ,
--		dbo.error_prg.error ,
--		dbo.error_prg.mod_date ,
--		dbo.error_prg.mod_prg ,
--		dbo.error_prg.mod_user ,
--		dbo.error_prg.mod_host 
		FROM dbo.error_prg
WHERE NULLIF(dbo.error_prg.PROCESSED,0) IS NULL
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''ERROR PRG REPORT for  '' + convert(varchar(10),getdate(),101)
END
else
begin
set @sub = ''NO ERROR PRG REPORT for '' + convert(varchar(10),getdate(),101) 
end

EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', --if necessary
--@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@recipients = N''bradley.powers@wth.org'', -- for testing
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

select ''Count is '' +cast(@@ROWCOUNT as varchar)', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=12
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [HOSPITAL OVERLAP]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'HOSPITAL OVERLAP', 
		@step_id=9, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'SET QUOTED_IDENTIFIER OFF
DECLARE @startDate DATETIME
DECLARE @endDate DATETIME
print ''Hospital Overlap''
-- defaults to midnight today
SET @startDate = CONVERT(DATETIME,convert(varchar(10),DATEADD(DAY,-5,GETDATE()),101))
--CONVERT(DATETIME,convert(varchar(10),GETDATE(),101))

SET @endDate = DATEADD(DAY,-1,GETDATE())

IF OBJECT_ID(''tempdb..#tempInfceQueryHospOverlap'',''U'') IS NOT NULL
BEGIN
	DROP TABLE #tempInfceQueryHospOverlap
END
CREATE TABLE #tempInfceQueryHospOverlap (
	account VARCHAR(15),
	msgid NUMERIC(18,0),
	msgtype VARCHAR(3),
	xmlContent VARCHAR(MAX))
	
/*
ADT-A01 Admit
ADT-A02 Transfer
ADT-A03 Discharge
ADT-A04 Outpatient Admission 
ADT-A05 Preadmission
ADT-A06 Rollover from O/P to I/P
ADT-A07 Rollover from I/P to O/P
ADT-A08 Update
ADT-A11 Cancel Admit
ADT-A13 Cancel Discharge
*/
INSERT INTO #tempInfceQueryHospOverlap
		( account, msgid,msgType, xmlContent )
SELECT account, systemMsg, msgtype,msgContent
FROM (
SELECT  mi.account_cerner AS [account]
	, MAX(mi.systemMsgId) AS systemMsg
	, right(mi.msgType,3) AS [msgType]
	, mi.msgContent
	FROM infce.messages_inbound_adt mi
	WHERE mi.msgDate BETWEEN @startDate AND @endDate
	--AND mi.account_cerner = ''8522320''
	AND RIGHT(mi.msgType,3) = ''A03''
	GROUP BY mi.account_cerner,
	RIGHT(mi.msgType,3), mi.msgContent
	) x

		
--SELECT * FROM #tempInfceQueryHospOverlap

; WITH cteTemp
AS
(
SELECT #tempInfceQueryHospOverlap.account,
	#tempInfceQueryHospOverlap.msgid AS [systemMsgId] ,
	#tempInfceQueryHospOverlap.msgtype,
	CAST(
		REPLACE(#tempInfceQueryHospOverlap.xmlContent,''"'','''')
		AS XML )
		AS [xmlContent] FROM #tempInfceQueryHospOverlap
)
, cteData
AS
(
SELECT r.systemMsgId, r.msgtype, r.account,
 COALESCE(NULLIF(dbo.GetMappingValue(''CLIENT'',''CERNER'',
	COALESCE(  
	 NULLIF(LTRIM(a.alias.value(''(//PV1.3.4/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.3.1/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.3.7/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.4/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.1/text() )[1]''	, ''varchar(50)'')),'''')
	,NULLIF(LTRIM(a.alias.value(''(//PV1.6.7/text() )[1]''	, ''varchar(50)'')),'''')
	
	)),''K''), a.alias.value(''(//PV1.3.4/text() )[1]''	, ''varchar(50)''))
	AS [CLIENT]
	
	-- ENCOUNTER INFO
	 , COALESCE(NULLIF(dbo.GetMappingValue(''ENCTR_TYPE'',''CERNER''
		, a.alias.value(''(//PV1.2.1/text())					
			[1]'', ''varchar(15)'') ) ,''K'') ,
			a.alias.value(''(//PV1.2.1/text()) [1]'', ''varchar(15)'') )
			AS [ENCTR TYPE]

	, a.alias.value(''(//PV1.2.1/text()) [1]'', ''varchar(15)'')  AS [TYPE]


	--ADT
	, CONVERT(DATETIME,a.alias.value(''(//PV1.44.1/text())[1]''
			, ''varchar(8)''))	
		 AS [ADMIT_DATE]

	,STUFF(STUFF(STUFF(STUFF(a.alias.value(''(//PV1.45.1/text())[1]''
			, ''varchar(12)'')+'':00.000'',11,0,'':''),9,0,''T''),7,0,''-''),5,0,''-'')
	 AS [DISCHARGE_DATE]	

	-- pat_name
		,LTRIM(RTRIM(a.alias.value(''(//PID.5.1/text())[1]''
			, ''varchar(25)''))) AS [LAST NAME]	
		,LTRIM(RTRIM(a.alias.value(''(//PID.5.2/text())[1]''
			, ''varchar(25)''))) AS [FIRST NAME]
		,ISNULL(LTRIM(RTRIM(a.alias.value(''(//PID.5.3/text())[1]''
			, ''varchar(25)''))),'''') AS [MIDDLE NAME]

,ISNULL(LTRIM(a.alias.value(''(//PID.2.1/text() )[1]''
			, ''varchar(50)'')),'''') AS [HNE_NUMBER]			
		-- MRI/MRN
		,isnull(dbo.sfn_trim_zeros( LTRIM(a.alias.value(''(//PID.3.1/text() )[1]''
			, ''varchar(15)'')) ),'''') AS [MRN]
		-- ov_pat_id
,ISNULL(dbo.sfn_trim_zeros( a.alias.value(''(//PID.4.1/text())[1]''
			, ''varchar(25)'') ),'''') AS [OV_PAT_ID]

-- ov order id			
,ISNULL(dbo.sfn_trim_zeros( a.alias.value(''(//PV1.19.1/text())[1]''
			, ''varchar(25)'') ),'''') AS [OV_ORDER_ID]
-- SSN			
,ISNULL(a.alias.value(''(//PID.19.1/text())[1]''
			, ''varchar(25)''),'''') AS [SSN]	
	-- fin code
, dbo.GetMappingValue(''FIN_CODE'',''CERNER'', 
		COALESCE(
			NULLIF(a.alias.value(''(//PV1.20.1/text())[1]'', ''varchar(10)''),'''') 
			,''EP'')) 
		AS [FIN CODE]
--,a.alias.query(''//MSH'') AS [MSH]
--,a.alias.query(''//EVN'') AS [EVN]
--,a.alias.query(''//PID'') AS [PID]
--,a.alias.query(''//PD1'') AS [PD1]
--,a.alias.query(''//PV1'') AS [PV1]
--,a.alias.query(''//GT1'') AS [GT1]
--,a.alias.query(''//IN1'') AS [IN1] -- can have multiples
--,a.alias.query(''//IN2'') AS [IN2] -- can have multiples and is relevent to the IN1 above it
--,a.alias.query(''//FT1'') AS [FT1] -- can have multiples
--,a.alias.query(''//PR1'') AS [PR1] -- can have multiples and is relevent to the FT1 above it
--,a.alias.query(''//FT1/.'') AS [fp]
--,a.alias.query(''//DG1'') AS [DG1]
---- the two items below are from the ORM only and will not match in 
---- our billing system nor will they include the 
---- automatic VP which is added in Cerner where appropriate.
--,a.alias.query(''//ORC'') AS [ORC] -- as this is the common order segment it should be the same for all orders but is included before each OBR
--,a.alias.query(''//OBR'') AS [OBR] -- can have multiples
--,a.alias.query(''//HL7Message'') AS [xmlContent]
		
FROM (SELECT cteTemp.systemMsgId, cteTemp.msgtype, cteTemp.account
	, xmlContent AS rep_xml FROM cteTemp) r
CROSS APPLY r.rep_xml.nodes(''HL7Message'') a(alias)
)
insert INTO dbo.tblPropAccCrossover
		(
			propPK ,
			propFincode ,
			propEnctrType ,
			propType ,
			propAcc ,
			propTDate ,
			propPatName ,
			propHospAcc ,
			propHneNumber ,
			propAdmitDate ,
			propDischargeDate
		)

SELECT --DISTINCT
d.systemMsgId,
acc.fin_code
,d.[ENCTR TYPE]
,d.[TYPE]
,acc.account, acc.trans_date, acc.pat_name
,d.hosp_acc,d.HNE_NUMBER,d.ADMIT_DATE,d.[DISCHARGE DATE]

--, d.xmlContent

FROM acc 
INNER JOIN (SELECT cteData.systemMsgId,cteData.ACCOUNT AS [hosp_acc],cteData.HNE_NUMBER, cteData.ADMIT_DATE
, COALESCE(cteData.DISCHARGE_DATE,GETDATE()) AS [DISCHARGE DATE]
,cteData.[ENCTR TYPE]
,cteData.[TYPE]

FROM cteData
WHERE cteData.ADMIT_DATE >= ''05/31/2015 00:00''
) AS d
ON d.HNE_NUMBER = acc.HNE_NUMBER
	AND acc.trans_date BETWEEN d.ADMIT_DATE 
	AND d.[DISCHARGE DATE]
LEFT OUTER JOIN dbo.tblPropAccCrossover tc 
	ON tc.propAcc = acc.account
	AND tc.propPK = d.systemMsgId
WHERE tc.propAcc IS NULL AND tc.propPK IS NULL
AND acc.fin_code = ''A''

select ''Count is '' +cast(@@ROWCOUNT as varchar)

DROP TABLE #tempInfceQueryHospOverlap

SET QUOTED_IDENTIFIER ON', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [UPDATE ACC FIN CODE FROM INS]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'UPDATE ACC FIN CODE FROM INS', 
		@step_id=10, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'SET QUOTED_IDENTIFIER  OFF 
UPDATE dbo.acc 
SET acc.fin_code = ''L''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''L'' AND ins.ins_code = ''AETNA''

UPDATE dbo.acc 
SET acc.fin_code = ''L''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''L'' AND ins.ins_code = ''COMM.L''

UPDATE dbo.acc 
SET acc.fin_code = ''H''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''L'' --NOT IN (''Y'')
AND ins.fin_code = ''H'' AND ins.ins_code = ''CIGNA''

UPDATE dbo.acc 
SET acc.fin_code = ''H''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''L'' --NOT IN (''Y'')
AND ins.fin_code = ''H'' AND ins.ins_code = ''HESP''


UPDATE dbo.acc 
SET acc.fin_code = ''H''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''L'' --NOT IN (''Y'')
AND ins.fin_code = ''H'' AND ins.ins_code = ''COMM.H''

UPDATE dbo.acc 
SET acc.fin_code = ''A''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''A'' AND ins.ins_code = ''MC''

UPDATE dbo.acc 
SET acc.fin_code = ''C''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''C'' AND ins.ins_code = ''CHAMPUS''

UPDATE dbo.acc 
SET acc.fin_code = ''M''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''Q'' --NOT IN (''Y'')
AND ins.fin_code = ''M'' AND ins.ins_code = ''AM''


UPDATE dbo.acc 
SET acc.fin_code = ''L''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''L'' AND ins.ins_code = ''UMR''


UPDATE dbo.acc 
SET acc.fin_code = ''E''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''E'' AND ins.ins_code = ''SP''

UPDATE dbo.acc 
SET acc.fin_code = ''L''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''L'' AND ins.ins_code = ''HUM''


UPDATE dbo.acc 
SET acc.fin_code = ''B''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''D'' --NOT IN (''Y'')
AND ins.fin_code = ''B'' AND ins.ins_code = ''BC''


UPDATE dbo.acc 
SET acc.fin_code = ''Q''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''Q'' AND ins.ins_code = ''AGMA''


UPDATE dbo.acc 
SET acc.fin_code = ''L''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''E'' --NOT IN (''Y'')
AND ins.fin_code = ''L'' AND ins.ins_code = ''COMM.L''

UPDATE dbo.acc 
SET acc.fin_code = ''L''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''L'' AND ins.ins_code = ''WELL''


UPDATE dbo.acc 
SET acc.fin_code = ''L''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''M'' --NOT IN (''Y'')
AND ins.fin_code = ''L'' AND ins.ins_code = ''UHC''


UPDATE dbo.acc 
SET acc.fin_code = ''M''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''M'' AND ins.ins_code = ''AM''

UPDATE dbo.acc 
SET acc.fin_code = ''D''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''Q'' --NOT IN (''Y'')
AND ins.fin_code = ''D'' AND ins.ins_code = ''TNBC''

UPDATE dbo.acc 
SET acc.fin_code = ''L''
FROM acc
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND dbo.ins.ins_a_b_c = ''A''
WHERE acc.fin_code  <> ins.fin_code 
AND acc.STATUS = ''NEW''
AND acc.fin_code = ''H'' --NOT IN (''Y'')
AND ins.fin_code = ''L'' AND ins.ins_code = ''SECP''

select ''Count is '' +cast(@@ROWCOUNT as varchar)', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [TEST ACCOUNTS IN MCLLIVE]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'TEST ACCOUNTS IN MCLLIVE', 
		@step_id=11, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=1, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime


set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))
; WITH cteZZ
AS
(
SELECT acc.account, acc.pat_name, CONVERT(VARCHAR(10),acc.trans_date,101) AS [trans_date]
,dbo.GetAccBalByDate(acc.account,convert(varchar(10),GETDATE(),101)) AS [Current Balance]
FROM acc
WHERE dbo.acc.trans_date BETWEEN ''06/01/2015 00:00'' AND GETDATE()
AND acc.pat_name LIKE ''ZZ%''
/*the three accounts below were billed to the client before being reversed*/
AND acc.account NOT IN (''L5145043'',''L5180453'',''L5179219'')
)
SELECT @tableHtml = 
N''<H1> TEST ACCOUNTS IN LIVE </H1>''+
N''<H2> These accounts should be reversed on LDOM </H2>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ACCOUNT</th><th>PATIENT</th><th>DOS</th>'' +
N''<th>CURRENT BALANCE</th><th>UOS</th></tr>'' +
CAST (( select td = cteZZ.account,'''',
			   td = cteZZ.pat_name,'''', 
			   td = cteZZ.trans_date,'''', 
			   td = cteZZ.[Current Balance],'''', 
			   td = SUM(qty),'''' 
			   
--SELECT cteZZ.*,  AS [UOS] 
FROM cteZZ
INNER JOIN chrg ON dbo.chrg.account = cteZZ.account
WHERE [cteZZ].[Current Balance] <> 0.00
GROUP BY cteZZ.account, cteZZ.pat_name, ctezz.trans_date,
cteZZ.[Current Balance]
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Test Accounts in LIVE '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
--@recipients = N''carol.sellars@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', --if necessary
--@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@recipients = N''bradley.powers@wth.org'', -- for testing
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''$LOG FILE MSG$'' -- like CARE 360 EMAIL SENT
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)


/* on the last day of the month this should run */ 
IF ( DATEPART(DAY,GETDATE()) =
	DATEPART(DAY,DATEADD(mm,DATEDIFF(m,0,GETDATE())+1, -.000003)) )
BEGIN

IF OBJECT_ID(''tempdb..#tempZZ20160628'',''U'') IS NOT NULL
BEGIN
	DROP TABLE #tempZZ20160628
END	

; WITH cteZZRev
AS
(
SELECT acc.account, acc.pat_name, CONVERT(VARCHAR(10),acc.trans_date,101) AS [trans_date]
,dbo.GetAccBalByDate(acc.account,convert(varchar(10),GETDATE(),101)) AS [Current Balance]
FROM acc
WHERE dbo.acc.trans_date BETWEEN ''06/01/2015 00:00'' AND GETDATE()
AND acc.pat_name LIKE ''ZZ%''
/*the three accounts below were billed to the client before being reversed*/
AND acc.account NOT IN (''L5145043'',''L5180453'',''L5179219'')
AND acc.status NOT IN (''PAID_OUT'',''CLOSED'')
)
SELECT * 
INTO #tempZZ20160628
FROM cteZZRev

SELECT * FROM #tempZZ20160628
DECLARE @cteAcc VARCHAR(15)
WHILE (EXISTS (SELECT TOP(1) account FROM #tempZZ20160628))
BEGIN
	SELECT @cteAcc = (SELECT TOP(1) account FROM #tempZZ20160628)
	
	EXEC dbo.usp_prg_ReverseChrgeOnly_Acc_Transaction @acc = @cteAcc, -- varchar(15)
		@comment = ''Test Account in LIVE'' -- varchar(50)
	
	DELETE FROM #tempZZ20160628
		WHERE account = @cteAcc
		
end

END

', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Nightly_Job.log', 
		@flags=12
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 3
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'DAILY AM', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20140819, 
		@active_end_date=99991231, 
		@active_start_time=50000, 
		@active_end_time=235959, 
		@schedule_uid=N'b4cad08c-d15c-4337-91ae-3c4550846dd1'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [PROD Daily Billing Updates]    Script Date: 5/14/2023 12:29:46 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:46 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'PROD Daily Billing Updates', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Provides daily emails to check the state of the billing system for items that have been identified as needing work. Plus a couple of emails that are just good to know.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'Bradley Powers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SET SSI BILL THRU DATE]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SET SSI BILL THRU DATE', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
print ''Job Start '' + convert(varchar(10),getdate(),101)
set nocount on
UPDATE    system
SET              value = convert(varchar(10),getdate()-10,101)

WHERE     (key_name = ''ssi_bill_thru_date'')

print ''SSI Date set to '' + convert(varchar(10) ,getdate()-10,101)

', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=14
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [DAILY BILLING CHANGES]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'DAILY BILLING CHANGES', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'CREATE TABLE #temp
(
[USER] varchar(50),
[DATE] datetime,
[0500] int,
[0600] int,
[0700] int,
[0800] int,
[0900] int,
[1000] int,
[1100] int,
[1200] int,
[1300] int,
[1400] int,
[1500] int,
[1600] int,
[1700] int,
[1800] int,
[1900] int,
[2000] int
	-- column_name data_type,...
)

--SELECT * FROM #temp

declare @startDate DATETIME
DECLARE @endDate DATETIME
set @startDate = DATEADD(DAY,-1,(CONVERT(DATETIME,convert(varchar(10),GETDATE(),101))))
SET @endDate = DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),@startDate+1,101)))
--SET @startDate = ''12/14/2014 00:00'' -- for mondays only
--SET @endDate = DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),@startDate+1,101)))

; WITH cteAcc AS 
(
SELECT  TOP(100) percent
		REPLACE(REPLACE(REPLACE(dbo.audit_acc.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''') AS [user],
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_acc.mod_date,108),'':'','''')+''00'' AS [HOUR],
		ISNULL(COUNT(dbo.audit_acc.account),0) AS [Updates],
		CONVERT(VARCHAR(10),dbo.audit_acc.mod_date,101) AS [DATE]		
FROM dbo.audit_acc

WHERE REPLACE(REPLACE(REPLACE(dbo.audit_acc.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''')
	IN (''agalloway'',''btaylor'',''btaylor1'',''cfranklin'',''chayes'',''clane'',''csellars'',''cwalton''
,''jgardner'',''jmcclear'',''jpatt'',''jpatterson'',''jtart'',''kajones'',''kjones''
,''msmarsh'',''rmiller'',''rreed'',''sabelew'',''sbelew'',''vtaylor'',''delder'',''scollins'',''scollins2'')
AND dbo.audit_acc.mod_date BETWEEN @startDate AND @endDate
GROUP BY REPLACE(REPLACE(REPLACE(dbo.audit_acc.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'',''''),
CONVERT(VARCHAR(10),dbo.audit_acc.mod_date,101),
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_acc.mod_date,108),'':'','''')+''00''

) 
INSERT INTO #temp
		(
			[USER] ,
			DATE ,
			[0500] ,			[0600] ,			[0700] ,			[0800] ,
			[0900] ,			[1000] ,			[1100] ,			[1200] ,
			[1300] ,			[1400] ,			[1500] ,			[1600] ,
			[1700] ,			[1800] ,			[1900] ,			[2000]
		)

SELECT [user],DATE, ISNULL([0500],0) AS [0500]
,ISNULL([0600],0) AS [0600],ISNULL([0700],0) AS [0700],ISNULL([0800],0) AS [0800]
,ISNULL([0900],0) AS [0900],ISNULL([1000],0) AS [1000],ISNULL([1100],0) AS [1100]
,ISNULL([1200],0) AS [1200],ISNULL([1300],0) AS [1300],ISNULL([1400],0) AS [1400]
,ISNULL([1500],0) AS [1500],ISNULL([1600],0) AS [1600],ISNULL([1700],0) AS [1700]
,ISNULL([1800],0) AS [1800],ISNULL([1900],0) AS [1900],ISNULL([2000],0) AS [2000]
from (
SELECT TOP(100) PERCENT	
cteAcc.[user] ,
		cteAcc.HOUR ,
		ISNULL(NULLIF(cteAcc.[Updates],''''),0)		AS [Updates],
		cteAcc.DATE 
FROM cteAcc
--WHERE cteAcc.[user] IN (''delder'')
) AS x
PIVOT 
( 
	sum([Updates])
	FOR HOUR IN ([0500],[0600],[0700],[0800],[0900],[1000],[1100],[1200],
				 [1300],[1400],[1500],[1600],[1700],[1800],[1900],[2000])
	)
	AS pvt
	ORDER BY [user], [DATE]

--SELECT * FROM #temp
---------------------------------------------------

; WITH ctePat AS 
(
SELECT  TOP(100) percent
		REPLACE(REPLACE(REPLACE(dbo.audit_pat.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''') AS [user],
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_pat.mod_date,108),'':'','''')+''00'' AS [HOUR],
		ISNULL(COUNT(dbo.audit_pat.account),0) AS [Updates],
		CONVERT(VARCHAR(10),dbo.audit_pat.mod_date,101) AS [DATE]		
FROM dbo.audit_pat
WHERE REPLACE(REPLACE(REPLACE(dbo.audit_pat.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''')
	IN (''agalloway'',''btaylor'',''btaylor1'',''cfranklin'',''chayes'',''clane'',''csellars'',''cwalton''
,''jgardner'',''jmcclear'',''jpatt'',''jpatterson'',''jtart'',''kajones'',''kjones''
,''msmarsh'',''rmiller'',''rreed'',''sabelew'',''sbelew'',''vtaylor'',''delder'',''scollins'',''scollins2'')
AND dbo.audit_pat.mod_date BETWEEN @startDate AND @endDate
GROUP BY REPLACE(REPLACE(REPLACE(dbo.audit_pat.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'',''''),

CONVERT(VARCHAR(10),dbo.audit_pat.mod_date,101),
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_pat.mod_date,108),'':'','''')+''00''

) 
INSERT INTO #temp
		(
			[USER] ,
			DATE ,
			[0500] ,			[0600] ,			[0700] ,			[0800] ,
			[0900] ,			[1000] ,			[1100] ,			[1200] ,
			[1300] ,			[1400] ,			[1500] ,			[1600] ,
			[1700] ,			[1800] ,			[1900] ,			[2000]
		)

SELECT [user],DATE, ISNULL([0500],0) AS [0500]
,ISNULL([0600],0) AS [0600],ISNULL([0700],0) AS [0700],ISNULL([0800],0) AS [0800]
,ISNULL([0900],0) AS [0900],ISNULL([1000],0) AS [1000],ISNULL([1100],0) AS [1100]
,ISNULL([1200],0) AS [1200],ISNULL([1300],0) AS [1300],ISNULL([1400],0) AS [1400]
,ISNULL([1500],0) AS [1500],ISNULL([1600],0) AS [1600],ISNULL([1700],0) AS [1700]
,ISNULL([1800],0) AS [1800],ISNULL([1900],0) AS [1900],ISNULL([2000],0) AS [2000]
from (
SELECT TOP(100) PERCENT	
ctePat.[user] ,
		ctePat.HOUR ,		
		ISNULL(NULLIF(ctePat.[Updates],''''),0) AS [Updates],
		ctePat.DATE 
FROM ctePat
--WHERE ctePat.[user] IN (''delder'')
) AS x
PIVOT 
( 
	sum([Updates])
	FOR HOUR IN ([0500],[0600],[0700],[0800],[0900],[1000],[1100],[1200],
				 [1300],[1400],[1500],[1600],[1700],[1800],[1900],[2000])
	)
	AS pvt
	ORDER BY [user], [DATE]
	
	
--SELECT * FROM #temp


---------------------------
; WITH cteIns AS 
(
SELECT  TOP(100) percent
		REPLACE(REPLACE(REPLACE(dbo.audit_ins.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''') AS [user],
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_ins.mod_date,108),'':'','''')+''00'' AS [HOUR],
		ISNULL(COUNT(dbo.audit_ins.account),0) AS [Updates],
		CONVERT(VARCHAR(10),dbo.audit_ins.mod_date,101) AS [DATE]		
FROM dbo.audit_ins
WHERE REPLACE(REPLACE(REPLACE(dbo.audit_ins.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''')
	IN (''agalloway'',''btaylor'',''btaylor1'',''cfranklin'',''chayes'',''clane'',''csellars'',''cwalton''
,''jgardner'',''jmcclear'',''jpatt'',''jpatterson'',''jtart'',''kajones'',''kjones''
,''msmarsh'',''rmiller'',''rreed'',''sabelew'',''sbelew'',''vtaylor'',''delder'',''scollins'',''scollins2'')
AND dbo.audit_ins.mod_date BETWEEN @startDate AND @endDate
GROUP BY REPLACE(REPLACE(REPLACE(dbo.audit_ins.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'',''''),

CONVERT(VARCHAR(10),dbo.audit_ins.mod_date,101),
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_ins.mod_date,108),'':'','''')+''00''

) --SELECT * FROM cteIns WHERE cteIns.[user] IN (''sabelew'',''sbelew'')
INSERT INTO #temp
		(
			[USER] ,
			DATE ,
			[0500] ,			[0600] ,			[0700] ,			[0800] ,
			[0900] ,			[1000] ,			[1100] ,			[1200] ,
			[1300] ,			[1400] ,			[1500] ,			[1600] ,
			[1700] ,			[1800] ,			[1900] ,			[2000]
		)

SELECT [user],DATE, ISNULL([0500],0) AS [0500]
,ISNULL([0600],0) AS [0600],ISNULL([0700],0) AS [0700],ISNULL([0800],0) AS [0800]
,ISNULL([0900],0) AS [0900],ISNULL([1000],0) AS [1000],ISNULL([1100],0) AS [1100]
,ISNULL([1200],0) AS [1200],ISNULL([1300],0) AS [1300],ISNULL([1400],0) AS [1400]
,ISNULL([1500],0) AS [1500],ISNULL([1600],0) AS [1600],ISNULL([1700],0) AS [1700]
,ISNULL([1800],0) AS [1800],ISNULL([1900],0) AS [1900],ISNULL([2000],0) AS [2000]
from (
SELECT TOP(100) PERCENT	
cteIns.[user] ,
		cteIns.HOUR ,		
		ISNULL(NULLIF(cteIns.[Updates],''''),0) AS [Updates],
		cteIns.DATE 
FROM cteIns
--WHERE cteIns.[user] IN (''delder'')
) AS x
PIVOT 
( 
	sum([Updates])
	FOR HOUR IN ([0500],[0600],[0700],[0800],[0900],[1000],[1100],[1200],
				 [1300],[1400],[1500],[1600],[1700],[1800],[1900],[2000])
	)
	AS pvt
	ORDER BY [user], [DATE]
	
	
--SELECT * FROM #temp



-----------------------------------------------------------
; WITH cteChrg AS 
(
SELECT  TOP(100) percent
		REPLACE(REPLACE(REPLACE(dbo.audit_chrg.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''') AS [user],
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_chrg.mod_date,108),'':'','''')+''00'' AS [HOUR],
		ISNULL(COUNT(dbo.audit_chrg.account),0) AS [Updates],
		CONVERT(VARCHAR(10),dbo.audit_chrg.mod_date,101) AS [DATE]		
FROM dbo.audit_chrg
WHERE REPLACE(REPLACE(REPLACE(dbo.audit_chrg.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''')
	IN (''agalloway'',''btaylor'',''btaylor1'',''cfranklin'',''chayes'',''clane'',''csellars'',''cwalton''
,''jgardner'',''jmcclear'',''jpatt'',''jpatterson'',''jtart'',''kajones'',''kjones''
,''msmarsh'',''rmiller'',''rreed'',''sabelew'',''sbelew'',''vtaylor'',''delder'',''scollins'',''scollins2'')
AND dbo.audit_chrg.mod_date BETWEEN @startDate AND @endDate
GROUP BY REPLACE(REPLACE(REPLACE(dbo.audit_chrg.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'',''''),

CONVERT(VARCHAR(10),dbo.audit_chrg.mod_date,101),
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_chrg.mod_date,108),'':'','''')+''00''

) 
INSERT INTO #temp
		(
			[USER] ,
			DATE ,
			[0500] ,			[0600] ,			[0700] ,			[0800] ,
			[0900] ,			[1000] ,			[1100] ,			[1200] ,
			[1300] ,			[1400] ,			[1500] ,			[1600] ,
			[1700] ,			[1800] ,			[1900] ,			[2000]
		)

SELECT [user],DATE, ISNULL([0500],0) AS [0500]
,ISNULL([0600],0) AS [0600],ISNULL([0700],0) AS [0700],ISNULL([0800],0) AS [0800]
,ISNULL([0900],0) AS [0900],ISNULL([1000],0) AS [1000],ISNULL([1100],0) AS [1100]
,ISNULL([1200],0) AS [1200],ISNULL([1300],0) AS [1300],ISNULL([1400],0) AS [1400]
,ISNULL([1500],0) AS [1500],ISNULL([1600],0) AS [1600],ISNULL([1700],0) AS [1700]
,ISNULL([1800],0) AS [1800],ISNULL([1900],0) AS [1900],ISNULL([2000],0) AS [2000]
from (
SELECT TOP(100) PERCENT	
cteChrg.[user] ,
		cteChrg.HOUR ,		
		ISNULL(NULLIF(cteChrg.[Updates],''''),0) AS [Updates],
		cteChrg.DATE 
FROM cteChrg
--WHERE cteChrg.[user] IN (''delder'')
) AS x
PIVOT 
( 
	sum([Updates])
	FOR HOUR IN ([0500],[0600],[0700],[0800],[0900],[1000],[1100],[1200],
				 [1300],[1400],[1500],[1600],[1700],[1800],[1900],[2000])
	)
	AS pvt
	ORDER BY [user], [DATE]
	
	
--SELECT * FROM #temp


---------------------------------------------------------------------------
; WITH cteChk AS 
(
SELECT  TOP(100) percent
		REPLACE(REPLACE(REPLACE(dbo.audit_chk.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''') AS [user],
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_chk.mod_date_audit,108),'':'','''')+''00'' AS [HOUR],
		ISNULL(COUNT(dbo.audit_chk.account),0) AS [Updates],
		CONVERT(VARCHAR(10),dbo.audit_chk.mod_date_audit,101) AS [DATE]		
FROM dbo.audit_chk
WHERE REPLACE(REPLACE(REPLACE(dbo.audit_chk.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'','''')
	IN (''agalloway'',''btaylor'',''btaylor1'',''cfranklin'',''chayes'',''clane'',''csellars'',''cwalton''
,''jgardner'',''jmcclear'',''jpatt'',''jpatterson'',''jtart'',''kajones'',''kjones''
,''msmarsh'',''rmiller'',''rreed'',''sabelew'',''sbelew'',''vtaylor'',''delder'',''scollins'',''scollins2'')
AND dbo.audit_chk.mod_date BETWEEN @startDate AND @endDate
GROUP BY REPLACE(REPLACE(REPLACE(dbo.audit_chk.mod_user,''MCL\'',''''),''WTHMC\'',''''),''MCLDOMAIN\'',''''),

CONVERT(VARCHAR(10),dbo.audit_chk.mod_date_audit,101),
		REPLACE(CONVERT(VARCHAR(2),dbo.audit_chk.mod_date_audit,108),'':'','''')+''00''

) 

INSERT INTO #temp
		(
			[USER] ,
			DATE ,
			[0500] ,			[0600] ,			[0700] ,			[0800] ,
			[0900] ,			[1000] ,			[1100] ,			[1200] ,
			[1300] ,			[1400] ,			[1500] ,			[1600] ,
			[1700] ,			[1800] ,			[1900] ,			[2000]
		)

SELECT [user],DATE, ISNULL([0500],0) AS [0500]
,ISNULL([0600],0) AS [0600],ISNULL([0700],0) AS [0700],ISNULL([0800],0) AS [0800]
,ISNULL([0900],0) AS [0900],ISNULL([1000],0) AS [1000],ISNULL([1100],0) AS [1100]
,ISNULL([1200],0) AS [1200],ISNULL([1300],0) AS [1300],ISNULL([1400],0) AS [1400]
,ISNULL([1500],0) AS [1500],ISNULL([1600],0) AS [1600],ISNULL([1700],0) AS [1700]
,ISNULL([1800],0) AS [1800],ISNULL([1900],0) AS [1900],ISNULL([2000],0) AS [2000]
from (
SELECT TOP(100) PERCENT	
cteChk.[user] ,
		cteChk.HOUR ,		
		ISNULL(NULLIF(cteChk.[Updates],''''),0) AS [Updates],
		cteChk.DATE 
FROM cteChk
--WHERE cteChk.[user] IN (''delder'')
) AS x
PIVOT 
( 
	sum([Updates])
	FOR HOUR IN ([0500],[0600],[0700],[0800],[0900],[1000],[1100],[1200],
				 [1300],[1400],[1500],[1600],[1700],[1800],[1900],[2000])
	)
	AS pvt
	ORDER BY [user], [DATE]
	
/*
--- query to accumulate data
SELECT [USER],	CONVERT(VARCHAR(10),[DATE],101) AS [DATE], SUM([0500]) AS [0500],
SUM([0600]) AS [0600],SUM([0700]) AS [0700],SUM([0800]) AS [0800],SUM([0900]) AS [0900],
SUM([1000]) AS [1000],SUM([1100]) AS [1100],SUM([1200]) AS [1200],SUM([1300]) AS [1300],
SUM([1400]) AS [1400],SUM([1500]) AS [1500],SUM([1600]) AS [1600],SUM([1700]) AS [1700],
SUM([1800]) AS [1800],SUM([1900]) AS [1900],SUM([2000]) AS [2000]
FROM #temp
GROUP BY [USER],[DATE]
ORDER BY [USER],[DATE]
*/
set nocount on
--SELECT @startDate, @endDate

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))
set @tableHtml = 
N''<H1> Daily Billing Changes </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>USER</th><th>DATE</th><th>0500</th>''+
N''<th>0600</th><th>0700</th><th>0800</th><th>0900</th><th>1000</th>''+
N''<th>1100</th><th>1200</th><th>1300</th><th>1400</th><th>1500</th>''+
N''<th>1600</th><th>1700</th><th>1800</th><th>1900</th><th>2000</th>''+
N''</tr>'' +
CAST (( select td = [USER],'''', 
td = CONVERT(VARCHAR(10),[DATE],101),'''', td = SUM([0500]),'''', td = SUM([0600]),'''',
td=SUM([0700]),'''',td=SUM([0800]),'''',td=SUM([0900]),'''',td=SUM([1000]),'''',
td=SUM([1100]),'''',td=SUM([1200]),'''',td=SUM([1300]),'''',td=SUM([1400]),'''',
td=SUM([1500]),'''',td=SUM([1600]),'''',td=SUM([1700]),'''',td=SUM([1800]),'''',
td=SUM([1900]),'''',td=SUM([2000]),''''
FROM #temp
GROUP BY [USER],[DATE]
ORDER BY [USER],[DATE]
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Daily Billing Table Changes '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT @this_proc_name + ''COMPLETED'' -- like CARE 360 EMAIL SENT
END

--PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

DROP TABLE #temp
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=14
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CONVERT PAT ICD TO PATDX]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CONVERT PAT ICD TO PATDX', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'return;
; with cte
as
(
	select account, dx_number,diagnosis
	from
	(
		select  pat.account, icd9_1,icd9_2,icd9_3,icd9_4,icd9_5,icd9_6,icd9_7,icd9_8,icd9_9
		from dbo.pat
		left outer join dbo.patdx on patdx.account = pat.account
		where patdx.account is null
		--where account = ''C7101759''
		) query
		unpivot
		(
			diagnosis for dx_number in (icd9_1,icd9_2,icd9_3,icd9_4,icd9_5,icd9_6,icd9_7,icd9_8,icd9_9)
		)
		as upvt
)
INSERT INTO dbo.patdx (account, dx_number, diagnosis, version, is_error, mod_prg, code_qualifier)

output INSERTED.account, INSERTED.dx_number, INSERTED.diagnosis, INSERTED.mod_date, getdate()
into dbo.audit_patdx

select cte.account
		, right(cte.dx_number,1) as [dx_number]
		,isnull(cte.diagnosis,''ERR'') as diagnosis
		,isnull((select version from dictionary.icd9version where acc.trans_date between icd9version.effective_date and 
			coalesce(icd9version.effective_end_date,getdate())),''ERR'') as version
	,case when not exists(select icd9_num from icd9desc where icd9_num = cte.diagnosis) then 
		 ''True''
		else
		 ''False''
end as [is_error]
, ''TSQL Query ''+convert(varchar(23),getdate(),126)
, ''BK'' -- for icd9 ''ABK'' for icd10

from cte	
inner join dbo.acc on acc.account = cte.account

PRINT ''PAT DX Row count '' + CONVERT(VARCHAR(10), @@ROWCOUNT)', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PAT DX DUPLICATE FIX]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PAT DX DUPLICATE FIX', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
; with cteDx
as
(
select account, StartDx=MIN(dx_number), EndDx=MAX(dx_number)--, diagnosis
from
	(
		select account, dx_number, rn= dx_number-row_number() over (partition by account order by dx_number)
		
		from patdx
	) dx
group by account, rn
)
--select a.*, patdx.account, patdx.dx_number 
update patdx
set dx_number = a.StartDx
from (
select distinct  a.account,a.StartDx-1 as [StartDx], a.StartDx as [dx_number]

from cteDx a
cross join cteDx b 
where (a.account = b.account and a.StartDx > b.StartDx 
)
) a
inner join patdx on patdx.account = a.account and patdx.dx_number = a.dx_number

PRINT ''PAT DX FIXED Row count '' + CONVERT(VARCHAR(10), @@ROWCOUNT)', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [MEDICARE BNPs]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'MEDICARE BNPs', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

; with cteNeedHold
as
(
	select top(100) percent acc.cl_mnem, chrg.account, acc.pat_name,service_date, cdm
	,sum(qty) --over (partition by chrg.account, cdm) 
		as [qty]
	 from chrg
	inner join acc on acc.account = chrg.account
	where cdm  in (''5325048'',''5325094'',''5322126'',''RL00181'')
	and credited = 0
	and chrg.account in 
	(
	 SELECT ACCOUNT FROM ACC WHERE FIN_CODE = ''A'' and status in (''UBOP'',''UB'',''1500'', ''NEW'')
	) 
	group by acc.cl_mnem, chrg.account, acc.pat_name, service_date, cdm
	having sum(qty) <> 0
	order by cl_mnem, service_date,cdm
	
) 
select @tableHtml = (
N''<H1> MEDICARE BNP''''s </H1>''+
N''<H2> Charges have been fixed </H2>''+
N''<table border = "1" bordercolor ="blue">''+
N''<tr bgcolor ="blue"><th>ROW</th><th>CLIENT</th><th>ACCOUNT</th><th>PATIENT</th><th>CDM</th><th>SERVICE DATE</th></tr>'' +

CAST (( select td =  ROW_NUMBER() 
		OVER (PARTITION BY cteNeedHold.account, cteNeedHold.cdm 
				ORDER BY cteNeedHold.account, cteNeedHold.cdm) ,'''', 
			  td = cteNeedHold.cl_mnem,'''',
			   td = cteNeedHold.account,'''', 
			   td = cteNeedHold.pat_name,'''',
			   td = cteNeedHold.cdm,'''',
			   td = convert(varchar(10),cteNeedHold.service_date,101),''''
from cteNeedHold
order by cteNeedHold.cl_mnem,service_date,cdm,pat_name
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'');

if (len(@tableHtml) > 0)
begin
set @sub = ''BNP''''s as of  '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = ''carol.plumlee@wth.org;cheryl.lane@wth.org'',
@blind_copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''EMAIL SENT''
end

PRINT ''BNP Row count '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

CREATE TABLE #tempChrgNum (chrg_num NUMERIC(18,0) NOT NULL)

; WITH cteChrg
AS
(
select top(100) PERCENT  acc.cl_mnem, chrg.account, acc.pat_name,service_date, cdm
	,sum(qty) --over (partition by chrg.account, cdm) 
		as [qty]
	 from chrg
	inner join acc on acc.account = chrg.account
	where cdm  in (''5325048'',''5325094'',''5322126'',''RL00181'')
	and credited = 0
	and chrg.account in 
	(
	 SELECT ACCOUNT FROM ACC WHERE FIN_CODE = ''A'' and status in (''UBOP'',''UB'',''1500'', ''NEW'')
	) 
	group by acc.cl_mnem, chrg.account, acc.pat_name, service_date, cdm
	having sum(qty) <> 0
	order by cl_mnem, service_date,cdm
)
INSERT INTO #tempChrgNum
		( chrg_num )
SELECT chrg_num FROM dbo.chrg
INNER JOIN cteChrg ON cteChrg.account = dbo.chrg.account AND cteChrg.cdm = dbo.chrg.cdm
AND cteChrg.service_date = dbo.chrg.service_date
WHERE credited = 0

--SELECT * 
UPDATE    chrg
		SET net_amt = 0,
		credited = 1
		,comment = ''BNP no charge per Ed Hughes and Patricia Puckett''
		FROM  chrg 
		where chrg_num IN (SELECT chrg_num FROM #tempChrgNum)

--SELECT *		
update amt
set amount = 0.0000
	from amt
	where chrg_num IN (SELECT chrg_num FROM #tempChrgNum)
	

		insert into notes (account,comment,mod_prg)
		select account,''BNP no charge per Ed Hughes and Patricia Puckett''
		, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		 	''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50) 
		from chrg WHERE chrg_num IN (SELECT chrg_num FROM #tempChrgNum)
		
		
		SELECT * FROM chrg WHERE chrg_num IN (SELECT chrg_num FROM #tempChrgNum)
		
		DROP TABLE #tempChrgNum', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=12
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CARE 360 TO BE ENTERED]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CARE 360 TO BE ENTERED', 
		@step_id=6, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime


set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)


set @tableHtml = 
N''<H1> CARE360 TO BE ENTERED</H1>''+
N''<H2> Count down to automation</H2>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>MONTH</th><th>DAY</th><th>QUANTITY</th></tr>'' +
CAST (( select td = isnull(convert(varchar(15),datepart(month, date_of_service)),''TOTAL ''),'''',
			   td = convert(varchar(10), date_of_service,101),'''', 
			   td = COUNT(date_of_service),'''' 
			   
FROM         data_quest_360
WHERE     (bill_type = ''Q'') AND (entered = 1) AND charges_entered = 0 and (deleted = 0)
GROUP BY datepart(month, date_of_service)
	, date_of_service
with rollup
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Care360 Needing Entry as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
@blind_copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''CARE 360 EMAIL SENT''
END

PRINT ''CARE360 TBE Row count '' + CONVERT(VARCHAR(10), @@ROWCOUNT)
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SET PAT RELATIONS]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SET PAT RELATIONS', 
		@step_id=7, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
SET QUOTED_IDENTIFIER  OFF 
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

update pat
set relation = ''01''
where not relation in (''01'',''02'',''03'',''09'')
and mod_date between @startDate and @endDate

PRINT ''PAT Relation Row count '' + CONVERT(VARCHAR(10), @@ROWCOUNT)', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [INVALID INS HOLDER NAMES]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'INVALID INS HOLDER NAMES', 
		@step_id=8, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)
/* Insurance records with no comma in name*/
;with cte as
(
	select top(100) percent cl_mnem, ins.account,holder_nme , acc.fin_code,trans_date
	from ins
	left outer join acc on acc.account = ins.account
	where (not holder_nme like ''%,%'') and trans_date >= dateadd(month,-8,getdate())
	and not acc.fin_code in (''E'',''SP'',''Y'',''CLIENT'') and acc.status = ''new''
	order by acc.fin_code,trans_date,ins.fin_code

)
select @count = (select count(cte.account) from cte)
if (@count > 0)
begin
;with cte as
(
	select top(100) percent cl_mnem, ins.account,trans_date ,holder_nme , acc.fin_code as [Acc FinCode]
	from ins
	left outer join acc on acc.account = ins.account
	where (not holder_nme like ''%,%'') and trans_date >= dateadd(month,-8,getdate())
	and not acc.fin_code in (''E'',''SP'',''Y'',''CLIENT'') and acc.status = ''new''
	order by acc.fin_code,trans_date,ins.fin_code
	
)
select @tableHtml = (
N''<H1> Insurance records with no comma in holders name</H1>''+
N''<table border = "1" bordercolor ="blue">''+
N''<tr bgcolor ="blue"><th>CLIENT</th><th>ACCOUNT</th><th>TRANS DATE</th><th>HOLDER NAME</th><th>ACC FIN CODE</th></tr>'' +

CAST (( select td = cte.cl_mnem,'''',
			   td = cte.account,'''', 
			   td = convert(varchar(10),cte.trans_date,101),'''',
			   td = holder_nme,'''', 
			   td = cte.[Acc FinCode],''''
			 
from cte
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'')

if (len(@tableHtml) > 0)
BEGIN

set @sub = ''Insurance records with no comma in holders name '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@copy_recipients = ''carol.plumlee@wth.org'',
@recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''EMAIL SENT''
end

PRINT ''INVALID INS HOLDER NAME Row Count '' + convert(varchar(10), @@ROWCOUNT)
end', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ORPHANED ACCOUNTS]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ORPHANED ACCOUNTS', 
		@step_id=9, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

/* orphaned accounts*/

;with cte as
(
	select distinct --''Accounts with '' as [Accounts with no charges]	,
	acc.cl_mnem ,acc.account, convert(varchar(10),trans_date,101) as [trans_date], 
	acc.fin_code
	,datediff(day, acc.trans_date, getdate()) as [No of Days],
	acc.mod_prg as [Created by]
	from acc
	left outer join chrg on chrg.account = acc.account
	where chrg.account is null 
	and (not acc.status in (''closed'',''paid_out''))
	and not acc.fin_code in (''CLIENT'',''Y'')
)
select @tableHtml = (
N''<H1> ACCOUNTS WITH NO CHARGES ERRORS</H1>''+
N''<table border = "1" bordercolor ="blue">''+
N''<tr bgcolor ="blue"><th>CLIENT</th><th>ACCOUNT</th><th>TRANS DATE</th><th>FIN CODE</th><th>APPLICATION</th></tr>'' +

CAST (( select td = cte.cl_mnem,'''',
			   td = cte.account,'''', 
			   td = convert(varchar(10),cte.trans_date,101),'''', 
			   td = cte.fin_code,'''',
			   td = cte.[Created By], ''''		
from cte
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'')
if (len(@tableHtml) > 0)
begin

set @sub = ''Accounts with no charges errors '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
--@copy_recipients = ''carol.plumlee@wth.org'',
@recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
PRINT ''EMAIL SENT - Orphaned Accounts''
end

PRINT ''Orphaned Accounts Row count '' + CONVERT(VARCHAR(10), @@ROWCOUNT)
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ORPHANED AMT RECORDS]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ORPHANED AMT RECORDS', 
		@step_id=10, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

/* orphaned amt*/
;with cte as
(
	select ''orphan'' as [amt record with no chrg record],
	amt.chrg_num as [amt chrg num],
	convert(varchar(10),amt.mod_date,101) as [amt mod_date]
	from amt
	left outer join chrg on chrg.chrg_num = amt.chrg_num
	where chrg.chrg_num is null

)
select @count = (select count(cte.[amt chrg num]) from cte)
if (@count > 0)
begin
;with cte as
(
	select ''orphan'' as [amt record with no chrg record],
	amt.chrg_num as [amt chrg num],
	convert(varchar(10),amt.mod_date,101) as [amt mod_date]
	, amt.mod_prg
	from amt
	left outer join chrg on chrg.chrg_num = amt.chrg_num
	where chrg.chrg_num is null

)
select @tableHtml = (
N''<H1> AMT RECORDS WITH NO CHARGES RECORDS</H1>''+
N''<table border = "1" bordercolor ="blue">''+
N''<tr bgcolor ="blue"><th>AMT CHRG NUMBER</th><th>AMT MOD DATE</th><th>AMT MOD PRG</th></tr>'' +

CAST (( select td = cte.[amt chrg num],'''',
			   td = convert(varchar(10),cte.[amt mod_date],101),'''', 
			   td = cte.mod_prg,''''
from cte
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'')

set @sub = ''Amt records with no charge records '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
--@copy_recipients = ''carol.plumlee@wth.org'',
@recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
end
--*/ end of orphaned accounts', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [TRANS DATE MISMATCHES]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'TRANS DATE MISMATCHES', 
		@step_id=11, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'

set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

/* trans_date / service_date mismatches */
;with cte as
(
	select distinct ''TRANS/Service date mismatches since 06/01/2015'' as [QType]
,cl_mnem,acc.account, cdm, convert(varchar(10),trans_date,101) as [trans_date], 
	convert(varchar(10),service_date,101) as [service_date], acc.fin_code
	, convert(varchar(10),chrg.mod_date,101) as [date entered]
	from acc
	inner join chrg on chrg.account = acc.account
	where trans_date <> service_date 
	--and trans_date > getdate()-30
	--AND dbo.acc.trans_date > ''06/01/2015 00:00''
	and cdm <> ''CBILL'' and not acc.fin_code in (''CLIENT'',''w'',''x'',''y'',''z'') 
	and chrg.credited = 0
	and (not acc.status in (''closed'',''paid_out''))
	
)
select @count = (select count(cte.account) from cte)
if (@count > 0)
begin
;with cte as
(
	select distinct ''TRANS/Service date mismatches'' as [QType]
	,cl_mnem,acc.account--, cdm
	, convert(varchar(10),trans_date,101) as [trans_date]
	, convert(varchar(10),service_date,101) as [service_date]
	, acc.fin_code
	, convert(varchar(10),chrg.mod_date,101) as [date entered]
	from acc
	inner join chrg on chrg.account = acc.account
	where trans_date <> service_date 
	--and trans_date > getdate()-30
	--AND dbo.acc.trans_date > ''06/01/2015 00:00''
	and cdm <> ''CBILL'' and not acc.fin_code in (''CLIENT'',''w'',''x'',''y'',''z'') 
	and chrg.credited = 0
	and (acc.status NOT IN (''closed'',''paid_out''))
)
select @tableHtml = (
N''<H2> ACCOUNTS WITH TRANS/SERVICE DATE ERRORS</H1>''+
N''<table border = "1" bordercolor ="blue">''+
--N''<tr bgcolor ="blue"><th>ROW NO</th><th>CLIENT</th><th>ACCOUNT</th><th>TRANS DATE</th><th>CDM</th><th>SERVICE DATE</th></tr>'' +
N''<tr bgcolor ="blue"><th>ROW NO</th><th>CLIENT</th><th>FIN CODE</th><th>ACCOUNT</th><th>TRANS DATE</th><th>SERVICE DATE</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY CAST(cte.trans_date AS DATETIME),cte.account),'''',
			   td = cte.cl_mnem,'''',
			   td = cte.fin_code,'''',
			   td = cte.account,'''', 
			   td = convert(varchar(10),cte.trans_date,101),'''', 
			   --td = cte.cdm,'''',
			   td = convert(varchar(10),cte.service_date,101), ''''		
from cte
ORDER BY CAST(cte.trans_date AS DATETIME), cte.account
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'')

set @sub = ''Accounts with trans/service date errors '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = ''carol.plumlee@wth.org'', 
@blind_copy_recipients=N''bradley.powers@wth.org;christopher.burton@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
end
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [QUEST MONTHLY CANCELLATIONS]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'QUEST MONTHLY CANCELLATIONS', 
		@step_id=12, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/*set nocount on
--declare @startDate datetime
--declare @endDate datetime

--set @startDate = dateadd(month,-6, getdate())
--set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

-- Monthly report of care 360 status
if (datepart(day,getdate()) = 1)
begin
	set @tableHtml = 
	N''<H1> MONTHLY QUEST CANCELLATION REPORT</H1>''+
	N''<table border = "1" bordercolor ="blue">''+
	N''<tr bgcolor ="blue"><th>MONTH</th><th>YEAR</th><th>STATUS</th><th>COUNT</th></tr>'' +

	CAST (( select td = isnull(convert(varchar(2),datepart(month,collection_date)),''TOTAL ''),'''',
				   td = convert(varchar(4),datepart(year,collection_date)),'''', 
				   td = [status],'''',
				   td = count([status]), ''''
	from data_quest_billing
	where collection_date between dateadd(ms,+3,dateadd(mm,datediff(m,0,getdate())-1,0)) 
						and dateadd(ms,-3,dateadd(mm,datediff(m,0,getdate())+0,0))
	group by datepart(year,collection_date),datepart(month,collection_date), [status]
	having count([status]) > 0
	for XML PATH(''tr''),TYPE) as NVARCHAR(MAX))+
	N''</Table>''			

	set @sub = ''Monthly Quest Cancellation Report '' + convert(varchar(10),getdate(),101)
	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = ''WTHMCLBILL'',
	@recipients = ''carol.plumlee@wth.org'',
	@copy_recipients=N''bradley.powers@wth.org'',
	@body = @tableHtml,
	@subject = @sub,
	@body_format = ''HTML'';
end
*/', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ACCOUNTS NEEDING QUEST CODES]    Script Date: 5/14/2023 12:29:46 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ACCOUNTS NEEDING QUEST CODES', 
		@step_id=13, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

-- Accounts with cdm''s needing Quest Codes
;with cteAcc as 
( 
	select acc.account, 
	datediff(year,dob_yyyy,trans_date) as [Age] from acc 
	inner join pat on pat.account = acc.account 
	where 
		fin_code = ''D'' 
		and trans_date between @startDate and @endDate and status = ''QUEST''
) 
, cteChrg as ( 
	select chrg.account, chrg.chrg_num, qty, cdm , cpt4, [Age] 
	, convert(datetime,convert(varchar(10),service_date,101)) as [DOS] 
	from chrg inner join cteAcc on cteAcc.account = chrg.account 
	inner join amt on amt.chrg_num = chrg.chrg_num 
	where credited = 0 and qty > 0 and (invoice is null or invoice = '''') 
	and not chrg.cdm in (select cdm from cdm where (cdm between ''5520000'' and ''5527417'' or cdm between ''5527420'' and ''552ZZZZ''))
) 
select @tableHtml = (
N''<H1> ACCOUNTS WITH CDM NEEDING QUEST CODES</H1>''+
N''<table border = "1" bordercolor ="blue">''+
N''<tr bgcolor ="blue"><th>ACCOUNT</th><th>CDM</th><th>CPT4</th><th>TRANSACTION DATE</th></tr>'' +

CAST (( select td = cteChrg.account,'''',
			   td = cteChrg.cdm,'''', 
			   td = cteChrg.cpt4,'''',
			   td = convert(varchar(10),cteChrg.dos,101), ''''
			

from cteChrg 
left outer join dict_quest_exclusions_final_draft dd on dd.cpt = cteChrg.cpt4 and outpatient_surgery = 0
left outer join dict_quest_reference_lab_tests dt on dt.cdm = cteChrg.cdm and dt.has_multiples = 0 and dt.deleted = 0
left outer join dict_quest_reference_lab_tests dt2 on  dt2.cdm = cteChrg.cdm  and dt2.link = cteChrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0
where case when dd.cpt is null then ''GAP''  else case when (age > 11 and age_appropriate = 1)  then ''GAP'' else ''EXCLUSION''  end end = ''GAP'' 
and coalesce(dt.quest_code,dt2.quest_code) is null
order by cteChrg.cdm

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'')

set @count = (len(@tableHtml))
if (@count > 0)

set @sub = ''Accounts needing Quest Codes '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = ''carol.plumlee@wth.org'',
@blind_copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PHYSICIANS NOT IN BILLING]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PHYSICIANS NOT IN BILLING', 
		@step_id=14, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

-- physicians not in our system
set @tableHtml = 
N''<H1> PHYSICIANS NOT IN BILLING</H1>''+
N''<table border = "1">''+
N''<tr><th>ITEM</th><th>FIN CODE</th><th>ACCOUNT</th><th>PHY ID</th>''+
N''<th>MOD PRG</th><th>MOD DATE</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER ( ORDER BY acc.fin_code, pat.account),'''',
			   td = acc.fin_code,'''',
			   td = pat.account,'''',
			   td = phy_id,'''', 
			  -- td = phy_comment,'''',
			   td = pat.mod_prg,'''', 
			   td = convert(varchar(10),pat.mod_date,101),''''
from pat
inner join acc on acc.account = pat.account
where (coalesce(phy_id,'''')<> ''''and (not phy_id in (select tnh_num from phy where deleted = 0)))
and acc.status = ''new'' and acc.fin_code not in (''w'',''X'',''Y'',''Z'',''CLIENT'')
and mailer = ''N''
ORDER BY acc.fin_code, dbo.acc.account
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';


set @count = 
(select count(pat.account)
from pat
inner join acc on acc.account = pat.account
where NULLIF(phy_id,'''') IS NOT NULL
and (phy_id NOT IN (select tnh_num from phy where deleted = 0))
and acc.status = ''new'' and acc.fin_code not in (''w'',''X'',''Y'',''Z'',''CLIENT''))
if (@count > 0)
begin

set @sub = ''Physicians not in Billing as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = ''carol.plumlee@wth.org'',
@copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
end
-- physicians not in our system
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [INSURANCE - PATIENT RELATION MISMATCHES WITH MONTHLY REPORT]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'INSURANCE - PATIENT RELATION MISMATCHES WITH MONTHLY REPORT', 
		@step_id=15, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

-- insurance/patient relation mismatches
set @tableHtml = 
N''<H1> INSURANCE / PATIENT RELATION MISMATCHES</H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr border = "1" bgcolor ="blue"><th>ITEM</th><th>ACCOUNT</th><th>DOS</th>'' +
N''<th >PATIENT</th><th>PAT GENDER</th><th>INS HOLDER GENDER</th>'' +
N''<th>INS CODE</th><th>INS PLAN NAME</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY pat.account),'''',
				td = pat.account,'''',
				td = CONVERT(VARCHAR,acc.trans_date,101),'''',
			   td = left(acc.pat_name,20),'''', 
			   td = pat.sex,'''', 
			   td = ins.holder_sex,'''',
			   td = isnull(ins.ins_code,''''),'''',
			   td = ins.plan_nme,'''' 
from pat
inner join ins on ins.account = pat.account
inner join acc on acc.account = pat.account
where pat.sex <> ins.holder_sex
and acc.trans_date between @startDate and @endDate and acc.status = ''NEW''
and ins.relation = ''01'' and pat.relation = ''01''
and coalesce(ins.holder_sex,'''') <> ''''
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</table>'';
set @count = 
(select count(pat.account)
from pat
inner join ins on ins.account = pat.account
inner join acc on acc.account = pat.account
where pat.sex <> ins.holder_sex
and acc.trans_date between @startDate and @endDate and acc.status = ''NEW''
and ins.relation = ''01'' and pat.relation = ''01''
and coalesce(ins.holder_sex,'''') <> '''')
if (@count > 0)
begin
set @sub = ''Insurance / patient Relation Mismatches as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
@blind_copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
end
-- insurance/patient relation mismatches

-- add to monthly report table each day
INSERT INTO data_monthly_ins_report
                      (account, pat_name, pat_sex, ins_holder_sex, ins_code, ins_plan_name, reported_date)
select pat.account,acc.pat_name, pat.sex,ins.holder_sex,ins.ins_code,ins.plan_nme,getdate() 
from pat
inner join ins on ins.account = pat.account
inner join acc on acc.account = pat.account
where pat.sex <> ins.holder_sex
and acc.trans_date between @startDate and @endDate and acc.status = ''NEW''
and ins.relation = ''01'' and pat.relation = ''01''
and coalesce(ins.holder_sex,'''') <> ''''

if (datepart(day,getdate())=1)

begin
--select account, pat_name, pat_sex, ins_holder_sex, ins_code, ins_plan_name, reported_date
--from data_monthly_ins_report
       
set @tableHtml = 
N''<H1> MONTHLY INSURANCE / PATIENT RELATION MISMATCHES</H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr border = "1" bgcolor ="blue"><th >ACCOUNT</th><th >PATIENT</th><th>PAT GENDER</th><th>INS HOLDER GENDER</th>'' +
N''<th>INS CODE</th><th>INS PLAN NAME</th><th>REPORTED DATE</th></tr>'' +
CAST (( select td = account,'''',
			   td = pat_name,'''', 
			   td = pat_sex,'''', 
			   td = ins_holder_sex,'''',
			   td = ins_code,'''','''',
			   td = ins_plan_name,'''' ,
			   td = convert(varchar(10),reported_date,101)
from data_monthly_ins_report

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</table>'';
set @sub = ''Monthly Insurance / Patient Relation Mismatches '' + convert(varchar(10),getdate(),101)
	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = ''WTHMCLBILL'',
	--@recipients = ''david.kelly@wth.org'',
	@recipients = ''carol.sellars@wth.org'',
	--@copy_recipients=N''bradley.powers@wth.org; david.kelly@wth.org'',
	@body = @tableHtml,
	@subject = @sub,
	@body_format = ''HTML'';

truncate table data_monthly_ins_report
end
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [BLUECARE NEEDING BUNDLING]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'BLUECARE NEEDING BUNDLING', 
		@step_id=16, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

/* bluecare needing bundling*/

set @tableHtml = 
N''<H1> BLUECARE NEEDING BUNDLING</H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr border = "1" bgcolor ="blue" ><th>ACCOUNT</th><th>PATIENT NAME</th><th>CDM</th><th>CDM COUNT</th><th>QTY SUM</th></tr>'' +
CAST (( select td = chrg.account,'''',
			   td = acc.pat_name,'''', 
			   td = cdm,'''', 
			   td = count(cdm),'''',
			   td = sum(qty)
from chrg
inner join acc on acc.account = chrg.account
where chrg.account in (select account from acc where fin_code = ''d'' 
and status = ''new'')
and (not cdm between ''5520000'' and ''552ZZZZ'')
and credited = 0
and coalesce(invoice,'''') = ''''
group by chrg.account, acc.pat_name, cdm
having sum(qty) > 1 and count(cdm) > 1
order by chrg.account, cdm
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

set @count = (

select top(1) count(chrg.account)
from chrg
inner join acc on acc.account = chrg.account
where chrg.account in (select account from acc where fin_code = ''d'' --and cl_mnem = ''QUESTR''
and status = ''new'')
and (not cdm between ''5520000'' and ''552ZZZZ'')
and credited = 0
and coalesce(invoice,'''') = ''''
group by chrg.account, acc.pat_name, cdm
having sum(qty) > 1 and count(cdm) > 1

)

if (@count > 0)
begin
set @sub = ''Bluecare needing bundling as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
@copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
end
-- end of bluecare needing bundling
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [HP INSURANCE CHECK]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'HP INSURANCE CHECK', 
		@step_id=17, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

set @tableHtml = 
N''<H1> HP INSURANCE FOR 2014</H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr border = "1" bgcolor ="blue"><th >ACCOUNT</th><th >PATIENT</th><th>TRANS_DATE</th><th>CLIENT</th>'' +
N''<th>BILLING</th><th>GROUP NUMBER</td></tr>'' +
CAST (( select td = ins.account,'''',
			   td = acc.pat_name,'''', 
			   td = convert(varchar(10),trans_date,101),'''', 
			   td = acc.cl_mnem,'''',
			   td = case when ins.ins_a_b_c = ''A''
							then ''PRIMARY''
							else ''SECOND or THIRD Insurance''
							end	,'''',
				td = ins.grp_num,''''
			    
from ins
inner join acc on acc.account = ins.account
where acc.trans_date > ''01/01/2014 00:00'' and ins.ins_code = ''HP'' and ins.grp_num = ''700100''
and ins.deleted = 0
order by pat_name
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</table>'';
/*
set @count = 
(select count(ins.account)
from ins
inner join acc on acc.account = ins.account
where acc.trans_date > ''01/01/2014 00:00'' and ins.ins_code = ''HP'' and ins.grp_num = ''700100'')
*/
if (len(@tableHtml)  > 0)
begin
set @sub = ''HP Insurance as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
@blind_copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
end


', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [QUEST CHARGES NOT PROCESSED]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'QUEST CHARGES NOT PROCESSED', 
		@step_id=18, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

set @tableHtml = 
N''<H1> QUEST CHARGES NOT PROCESSED </H1>''+
N''''+
N''<HR>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr border = "1" bgcolor ="blue"><th >ACCOUNT</th><th>CDM</th><th >CLIENT</th><th>TRANS DATE</th><th>FIN CODE</th></tr>'' +
CAST (( select td = acc.account,'''',
	   td =cdm ,'''', 
	   td = cl_mnem,'''', 
	   td = convert(varchar(10),trans_date,101),'''',
	   td = acc.fin_code,''''
FROM    acc
inner join chrg on chrg.account = acc.account
where acc.status = ''QUEST'' and not (cl_mnem in (''questr'',''questref''))
and trans_date <= getdate() - 1
and chrg.credited = 0
order by trans_date desc

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</table>'' 

set @count = (len(@tableHtml))

if (@count > 0)
begin
set @sub = ''Quest Charges not processed as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
@blind_copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
select ''QUEST CHARGES ''+ CONVERT(VARCHAR(10),@@rowcount)
end
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ACCOUNTS WITH CREDIT OR LARGE BALANCES]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ACCOUNTS WITH CREDIT OR LARGE BALANCES', 
		@step_id=19, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'--PRINT ''ACCOUNTS WITH CREDIT OR LARGE BALANCES skipped per Carols request''
--return;

--/*
set nocount on
declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

; with cte 
as
(
select top(100) percent ah.account, balance, convert(varchar(10),datestamp,101) as [aging date], ah.fin_code
, case when 
coalesce(dbo.pat.ub_date,dbo.pat.h1500_date,dbo.pat.colltr_date,dbo.pat.baddebt_date,
	dbo.pat.batch_date,dbo.pat.bd_list_date,dbo.pat.ebill_batch_date,dbo.pat.ebill_batch_1500,
	dbo.pat.e_ub_demand_date,dbo.pat.claimsnet_1500_batch_date,dbo.pat.claimsnet_ub_batch_date) IS NULL  
	then 
		case when chrg.invoice is null 
			then ''NOT BILLED''
			else ''CLIENT BILLED''
			end
	else ''INSURANCE BILLED''
	end as [Billing Status]
from aging_history ah
left outer join pat on pat.account = ah.account
inner join chrg on chrg.account = ah.account
where datestamp > getdate()-2
and (balance < 2.51 or balance > 1000.00)

 --and pat.mailer = ''N''  

order by balance
)
, cteChk
as
(
	select account, max(convert(varchar(10),chk.mod_date,101)) as [Last Chk Date]
	from chk
	group by account
)
select @tableHtml = (
N''<H1> ACCOUNTS WITH CREDIT OR LARGE BALANCES </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr border = "1" bgcolor ="blue"><th >ACCOUNT</th><th >AGING BALANCE</th><th>AGING DATE</th><th>LAST CHK DATE</th>'' +
N''<th>FIN CODE</th><th>BILLING STATUS</th></tr>'' +
CAST (( select distinct td = cte.account,'''',
			   td = convert(numeric(18,2),cte.balance),'''', 
			   td = cte.[aging date],'''', 
			   td = isnull(cteChk.[Last Chk Date],''NO CHKs''),'''',
			   td = cte.[fin_code],'''',
			   td = cte.[Billing Status],''''
			   
--select cte.account, cte.balance,cte.[aging date],cteChk.[Last Chk Date]
from cte
left outer join cteChk on cteChk.account = cte.account 
order by convert(numeric(18,2),balance)

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</table>'')

set @count = (len(@tableHtml))

if (@count > 0)
begin
set @sub = ''ACCOUNTS WITH CREDIT OR LARGE BALANCES '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.sellars@wth.org'',
@copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

select ''BALANCE QUERY ''+ CONVERT(VARCHAR(10),@@rowcount)
end
--*/', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CDM CHANGES]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CDM CHANGES', 
		@step_id=20, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
set nocount on

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)


set @tableHtml = 
N''<H1> CDM CHANGES </H1>''+
N''''+
N''<H2>INDICATOR LEGEND</H2>''+
N''<H3>	IO  - Inserted Original</H3>''+
N''<H3>	MUD - Modified Update Deleted - indicates changes to current cdm</H3>''+
N'''' +
N''<HR>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr border = "1" bgcolor ="blue"><th>DELETED</th><th>CDM</th><th>DATE</th><th>INDICATOR</th>''+
N''<th>MOD USER</th></tr>'' +
CAST (( select td = ac.deleted,'''',
			   td = ac.cdm,'''', 
			  -- td = orderable,'''', 
			   td = convert(varchar(10),ac.mod_date,101),'''',
			   td = mod_indicator,'''',
			   td = ac.mod_user,''''
			   
				
FROM    dbo.audit_cdm ac
where mod_indicator in (''io'',''mud'') and 
ac.mod_date >= CONVERT(DATETIME,convert(varchar(10),GETDATE()-1,101))
order by ac.cdm, ac.mod_indicator desc

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</table>'' 

set @count = (len(@tableHtml))

if (@count > 0)
begin
set @sub = ''CDM Changes as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.sellars@wth.org'',
@copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';
end
--select ''CDM UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [G046X HOLDS]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'G046X HOLDS', 
		@step_id=21, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'return;
/*
declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

set @tableHtml = 
N''<H1> MEDICARE CPT G0461 AND G0462 HOLD</H1>''+
N''<H2> These accounts will be placed on "MC HOLD" status</H2>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr border = "1" bgcolor ="blue"><th >ACCOUNT</th><th >FIN CODE</th><th>TRANS_DATE</th><th>CLIENT</th>'' +
N''<th>CDM</th><th>CPT4</td></tr>'' +
CAST (( select td = chrg.account,'''',
			   td = acc.fin_code,'''', 
			   td = convert(varchar(10),trans_date,101),'''', 
			   td = acc.cl_mnem,'''',
			   td = chrg.cdm,'''',
				td = amt.cpt4,''''
from chrg
inner join amt on amt.chrg_num = chrg.chrg_num
inner join acc on acc.account = chrg.account
where cpt4 in (''G0461'',''G0462'') and acc.fin_code = ''A''
and credited = 0 and acc.status = ''NEW''
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</table>'';
set @count = (select len(@tableHtml))

if (@count > 0)
begin
set @sub = ''Medicare Hold as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.sellars@wth.org'',
@blind_copy_recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

update acc
set status = ''MC HOLD''
from acc cross join
(select distinct chrg.account
from chrg
inner join amt on amt.chrg_num = chrg.chrg_num
inner join acc on acc.account = chrg.account
where cpt4 in (''G0461'',''G0462'') and acc.fin_code = ''A''
and credited = 0 and acc.status = ''NEW'') a 
where  a.account = acc.account

select ''MC HOLD UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)
end


*/', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ELECTRONIC STATUS DUPLICATE PURGE]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ELECTRONIC STATUS DUPLICATE PURGE', 
		@step_id=22, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'return;
/*
;with cte
as
(
SELECT     
	row_number() over (partition by account, status_type, bill_type, provider, subid, tracer_no, amt_on_report, status_on_claim 
					order by account,status_type, bill_type, provider, subid, tracer_no, amt_on_report, status_on_claim) as [rn]
,uid,account, status_type, bill_type, provider, subid, tracer_no, amt_on_report, status_on_claim
FROM         data_electronic_status
)
DELETE FROM data_electronic_status
--select * 
FROM         data_electronic_status inner JOIN
                          (SELECT uid    
                            FROM  cte  where rn > 1      ) AS tbl on tbl.uid = data_electronic_status.uid

select ''data_electronic_status rows deleted '' + convert(varchar(10),@@RowCount)
*/', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=4
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [UPDATE TABLES]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'UPDATE TABLES', 
		@step_id=23, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'SET QUOTED_IDENTIFIER ON
set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

--- post cerner updates

UPDATE top(100) percent  dbo.acc
SET fin_code = ''L''
FROM acc 
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND ins.ins_a_b_c = ''A''
INNER JOIN dbo.insc ON dbo.insc.fin_code = dbo.ins.fin_code
WHERE dbo.acc.fin_code = ''H''
AND dbo.ins.ins_code = ''UHC''


UPDATE dbo.acc
SET fin_code = ''D''
FROM acc 
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND ins.ins_a_b_c = ''A''
INNER JOIN dbo.insc ON dbo.insc.fin_code = dbo.ins.fin_code
WHERE dbo.acc.fin_code = ''H''
AND dbo.ins.ins_code = ''TNBC''


UPDATE dbo.acc
SET fin_code = ''Q''
FROM acc 
INNER JOIN ins ON dbo.ins.account = dbo.acc.account
	AND ins.ins_a_b_c = ''A''
INNER JOIN dbo.insc ON dbo.insc.fin_code = dbo.ins.fin_code
WHERE dbo.acc.fin_code = ''H''
AND dbo.ins.ins_code = ''AG''


UPDATE dbo.pat
SET relation = ''01''
WHERE dbo.pat.relation IN (''self'', ''SE'', NULL, '''')
AND dbo.pat.mod_date > ''05/31/2015 00:00''

UPDATE dbo.pat
SET relation = ''02''
WHERE dbo.pat.relation IN (''SP'')
AND dbo.pat.mod_date > ''05/31/2015 00:00''

UPDATE dbo.pat
SET relation = ''03''
WHERE dbo.pat.relation IN (''SO'',''DA'',''FA'')
AND dbo.pat.mod_date > ''05/31/2015 00:00''

UPDATE dbo.ins
SET relation = ''01''
WHERE dbo.ins.relation IN (''self'',''se'', NULL, '''')
AND dbo.ins.mod_date > ''05/31/2015 00:00''

UPDATE dbo.ins
SET relation = ''02''
WHERE dbo.ins.relation IN (''SP'')
AND dbo.ins.mod_date > ''05/31/2015 00:00''

-- pre cerner updates
--------------------------------------------------------
UPDATE dbo.pat
SET icd9_1 = REPLACE(icd9_1,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5 AND icd9_1 LIKE ''%.''

UPDATE dbo.pat
SET icd9_2 = REPLACE(icd9_2,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5 AND icd9_2 LIKE ''%.''

UPDATE dbo.pat
SET icd9_3 = REPLACE(icd9_3,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5  AND icd9_3 LIKE ''%.''

UPDATE dbo.pat
SET icd9_4 = REPLACE(icd9_4,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5 AND icd9_4 LIKE ''%.''

UPDATE dbo.pat
SET icd9_5 = REPLACE(icd9_5,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5 AND icd9_5 LIKE ''%.''

UPDATE dbo.pat
SET icd9_6 = REPLACE(icd9_6,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5 AND icd9_6 LIKE ''%.''

UPDATE dbo.pat
SET icd9_7 = REPLACE(icd9_7,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5 AND icd9_7 LIKE ''%.''

UPDATE dbo.pat
SET icd9_8 = REPLACE(icd9_8,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5  AND icd9_8 LIKE ''%.''

UPDATE dbo.pat
SET icd9_9 = REPLACE(icd9_9,''.'','''')
, mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
FROM pat 
WHERE mod_date > GETDATE()-5 AND icd9_9 LIKE ''%.''

---------------------------------------------------------
update pat
set city_st_zip = replace(city_st_zip,''-'','''')
where right(city_st_zip,1) = ''-''
select ''PAT city_st_zip  UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE pat
SET pat_zip = replace(pat_zip,''-'','''')
--SELECT pat_zip , replace(pat_zip,''-'','''') FROM pat
where right(pat_zip,1) = ''-''
select ''PAT city_st_zip  UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update pat
set guar_zip = replace(guar_zip,''-'','''')
--SELECT * FROM pat
where right(guar_zip,1) = ''-''
select ''PAT GUAR_zip  UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update pat
set g_city_st = replace(g_city_st,''-'','''')
--SELECT * FROM pat
where right(g_city_st,1) = ''-''
select '' GUAR_csz  UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE TOP(100) PERCENT dbo.pat
SET city_st_zip = REPLACE(g_city_st,'','','', '')
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
WHERE (city_st_zip NOT LIKE ''%,%'') AND (city_st_zip IS NOT NULL) and (city_st_zip LIKE ''%,%'')
select ''PAT city_st_zip UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE TOP(100) percent dbo.pat
SET g_city_st = REPLACE(g_city_st,'','','', '')
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
WHERE (g_city_st NOT LIKE ''%,%'') AND (g_city_st IS NOT NULL) and (g_city_st LIKE ''%,%'')
select ''PAT g_city_st UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update ins
set grp_num = replace(grp_num,''-'','''')
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
from ins
where ins_code = ''AETNA'' and mod_date > ''01/01/2014'' and grp_num like ''%-%''
select ''AETNA UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount) + '' -- ''+ 
ISNULL(RIGHT(OBJECT_NAME(@@PROCID),50),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112))

update amt
set modi = replace(modi,'' '','''')
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
where modi like ''% %''
and mod_date between @startDate and @endDate
select ''AMT MODI UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update amt
set modi2 = replace(modi2,'' '','''')
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
where modi2 like ''% %''
and mod_date between @startDate and @endDate
select ''AMT MODI2 UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update amt
set modi = null
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
where modi = ''''
and mod_date between @startDate and @endDate
select ''AMT MODI UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update amt
set modi2 = null
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
where modi2 = ''''
and mod_date between @startDate and @endDate
select ''AMT MODI2 UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

-- update the charge fin_codes 
update chrg
set invoice = null
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
where invoice = ''''
select ''CHRG INVOICE UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount) 

UPDATE    chrg
SET       fin_code = acc.fin_code
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
FROM         chrg INNER JOIN
                      acc ON acc.account = chrg.account
WHERE     (chrg.service_date > @startDate) AND (chrg.fin_code IS NULL OR
                      chrg.fin_code = '''') AND (chrg.cdm <> ''cbill'')
select ''CHRG FIN CODES UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

-- update insurance info
UPDATE    ins
SET             policy_num = replace(policy_num, ''-'','''')
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE      (ins.policy_num LIKE ''%-%'') AND (acc.status = ''new'')
select ''INS POLICY DASH REMOVAL UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE    ins
SET             policy_num = replace(policy_num, '' '','''')
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE     (ins.ins_code in (''bc'', ''cigna'')) AND (ins.policy_num LIKE ''% %'') AND (acc.status = ''new'')
select ''INS POLICY SPACE REMOVAL UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)


UPDATE    ins
SET   
plan_nme = ''CHAMPUS TRICARE'',          
plan_addr1 = ''P.O.BOX 7031'',
p_city_st = ''CAMDEN, SC 29020''
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE     (ins.ins_code = ''CHAMPUS'') 
AND plan_nme <> ''CHAMPUS TRICARE'' AND (acc.status = ''new'')
select ''INS CHAMPUS UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE    ins
SET             
plan_addr1 = ''UHC OF THE RIVER VALLEY'',
plan_addr2 = ''3800 AVE OF CITIES, SUITE 200'',
p_city_st = ''MOLINE, IL 61265'',
plan_nme = ''UHC COMMUNITY PLAN''
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE     (ins.ins_code = ''AM'') 
AND plan_nme <> ''UHC COMMUNITY PLAN'' 
AND (acc.status = ''new'')
select ''INS UHC UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE    ins
SET             
plan_addr1 = ''1 CAMERON HILL CIRCLE STE 0002'',
p_city_st = ''CHATTANOOGA, TN 374020002'',
plan_nme = ''BLUECARE/TNCARE SEL''
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE     (ins.ins_code = ''TNBC'') AND plan_nme = ''BLUECARE/TNCARE SELECT'' AND (acc.status = ''new'')
select ''INS BLUECARE UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE INS
SET HOLDER_ADDR =	null,
holder_city_st_zip = null
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
WHERE HOLDER_ADDR = ''PO BOX 3490''
select ''INS HOLDER UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)
--update the blank ones diagnosis code pointers

update amt
set diagnosis_code_ptr = ''1:''
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),''SQL Query ''+ CONVERT(VARCHAR(10),GETDATE(),112)),50)
where chrg_num in 
	(select chrg_num from chrg 
	 inner join pat on pat.account = chrg.account
	 inner join acc on acc.account = chrg.account
	 where /*acc.status = ''NEW'' and */  icd9_1 is not null 
		and coalesce(icd9_2,icd9_3,icd9_4,icd9_5,icd9_6,icd9_7,icd9_8,icd9_9) is null)
and diagnosis_code_ptr is NULL
select ''DIAGNOSIS CODE POINTER UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

--;with cte as
--(
--select row_number() over (partition by account order by account, mod_date) as [rn]
--,  account, mod_date, mod_user, mod_prg, mod_host, comment, rowguid
--FROM         notes
--where comment = ''Americhoice requires panels be bundled.'' and mod_date > getdate()-5
--)
--select ''unbundled Americhoice panel'' as [field],cte.* from cte
--inner join acc on acc.account = cte.account
--where (not acc.status in (''paid_out'')) and 
--rn > 1
--order by cte.account, rn

', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ACCOUNT WITH NO PAT RECORDS]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ACCOUNT WITH NO PAT RECORDS', 
		@step_id=24, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

/* account with no pat record*/

;with cte as
(
	select top(100) percent acc.account,
	acc.pat_name as [Patient]
	,convert(varchar(10),acc.trans_date,101) as [trans_date]
	,convert(varchar(10),acc.mod_date,101) as [mod_date]
	, acc.mod_prg
	from acc
	left outer join pat on dbo.pat.account = dbo.acc.account
	where pat.account is NULL
	AND acc.fin_code NOT IN (''client'',''w'',''x'',''y'',''z'')
	AND acc.status = ''NEW''
	order by acc.trans_date
)  --SELECT * FROM cte
select @tableHtml = (
N''<H1> ACC RECORDS WITH NO PAT RECORDS </H1>''+
N''<table border = "1" bordercolor ="blue">''+
N''<tr bgcolor ="blue"><th>ROW NUMBER</th><th>ACCOUNT</th><th>PATIENT</th><th>TRANS_DATE</th><th>MOD DATE</th><th>MOD PRG</th></tr>'' +

CAST (( select td = row_number() over (order by cte.account),'''',
				td = cte.account,'''',
			   td = cte.Patient,'''',
			   td = CONVERT(VARCHAR,cte.trans_date,101),'''',
			   td = CONVERT(VARCHAR,cte.mod_date,101),'''', 
			   td = cte.mod_prg,''''
from cte
ORDER BY cte.trans_date, cte.mod_date
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'')

if (len(@tableHtml) > 0)
begin

set @sub = ''Account records with no PAT records '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = ''carol.sellars@wth.org'',
@copy_recipients=N''bradley.powers@wth.org;christopher.burton@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''EMAIL SENT - Account with no PAT records''
end

PRINT ''Account WITH NO PAT RECORDS Row count '' + CONVERT(VARCHAR(10), @@ROWCOUNT)', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CHARGES WITH NO AMT RECORDS]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CHARGES WITH NO AMT RECORDS', 
		@step_id=25, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @count int 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

/* Charges with no Amt records*/

;with cte as
(
	select top(100) percent chrg.account,
	chrg.chrg_num as [chrg num],cdm,
	convert(varchar(10),chrg.mod_date,101) as [mod_date]
	, chrg.mod_prg
	from chrg
	left outer join amt on amt.chrg_num = chrg.chrg_num
	where amt.chrg_num is null
	order by chrg.mod_date
)
select @tableHtml = (
N''<H1> CHARGES RECORDS WITH NO AMT RECORDS </H1>''+
N''<table border = "1" bordercolor ="blue">''+
N''<tr bgcolor ="blue"><th>ROW NUMBER</th><th>ACCOUNT</th><th>CHRG NUMBER</th><th>CDM</th><th>MOD DATE</th><th>MOD PRG</th></tr>'' +

CAST (( select td = row_number() over (order by cte.cdm,cte.mod_date),'''',
				td = cte.account,'''',
			   td = cte.[chrg num],'''',
			   td = cte.cdm,'''',
			   td = cte.mod_date,'''', 
			   td = cte.mod_prg,''''
from cte
ORDER BY cte.cdm, cte.mod_date
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'')

if (len(@tableHtml) > 0)
begin

set @sub = ''Charge records with no AMT records '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
--@copy_recipients = ''carol.sellars@wth.org'',
@recipients=N''bradley.powers@wth.org'',
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''EMAIL SENT - Charges with no amt records''
end

PRINT ''CHARGES WITH NO AMT RECORDS Row count '' + CONVERT(VARCHAR(10), @@ROWCOUNT)', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ACCOUNT ERRORS]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ACCOUNT ERRORS', 
		@step_id=26, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

set @tableHtml = 
N''<H1> ACCOUNTS WITH ERRORS </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ACCOUNT</th><th>CLIENT</th><th>FIN CODE</th>'' +
N''<th>TRANS DATE</th><th>STATUS</th><th>ERROR</th></tr>'' +
CAST (( select td = account,'''',
			   td = cl_mnem,'''', 
			   td = fin_code,'''',
			   td = CONVERT(VARCHAR(10),trans_date,101),'''', 
			   td = status,'''',
			   td = CASE WHEN fin_code IN (''U'') THEN ''Financial class [U] is not valid''
			 WHEN (SELECT cli_mnem FROM client WHERE deleted = 0 AND cli_mnem = acc.cl_mnem) IS NULL	
				THEN ''CLIENT [''+cl_mnem+''] is not valid'' 
				ELSE NULL END,NULLIF('''','''')
			   
			   
--SELECT 	account ,
--		cl_mnem ,
--		fin_code ,
--		trans_date ,
--		status,
--		CASE WHEN fin_code IN (''U'') THEN ''Financial class [U] is not valid''
--			 WHEN (SELECT cli_mnem FROM client WHERE deleted = 0 AND cli_mnem = acc.cl_mnem) IS NULL	
--				THEN ''CLIENT [''+cl_mnem+''] is not valid'' 
--				ELSE NULL END AS [ERROR]
		 
FROM acc WHERE status like  ''%ERR%''

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = '' Accounts with errors as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''Error message sent for accounts'' -- like CARE 360 EMAIL SENT
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

/*

INSERT INTO dbo.notes
		(
			account ,
			mod_date ,
			mod_user ,
			mod_prg ,
			mod_host ,
			comment 
			
		)
SELECT 	account 
, GETDATE() AS [mod_date]
				, RIGHT(SUSER_SNAME(),50) AS [mod_user]
				, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
	''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
				, RIGHT (HOST_NAME(),50)  AS [mod_host]
	,''Financial class [U] is not valid. fin code changed from [U] to [H]''   AS [comment]
FROM acc WHERE status like  ''%ERR%'' AND fin_code = ''U''

PRINT ''Comments inserted ''+CONVERT(VARCHAR(2),@@ROWCOUNT)
			
		
		
		
UPDATE acc 
SET fin_code = ''H''
, status = ''NEW''
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
	''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50) 
WHERE fin_code = ''U''
*/

', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CREDITED CHARGES]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CREDITED CHARGES', 
		@step_id=27, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

set @tableHtml = 
N''<H1> CREDITED CHARGES </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ACCOUNT</th><th>CHRG NUM</th><th>CDM</th><th>QTY</th></tr>'' +
CAST (( select td = chrg.account,'''',
			   td = chrg.chrg_num,'''', 
			   td = chrg.cdm,'''', 
			   td = chrg.qty,'''' 
FROM dbo.chrg
INNER JOIN 
(
	SELECT     account, cdm, SUM(qty) AS QTY
	FROM          dbo.chrg AS chrg_1
    WHERE      (status NOT IN (''N/A'', ''CBILL'')) AND (credited = 0) AND (cdm <> ''CBILL'') AND (NULLIF (invoice, '''') IS NULL)
    GROUP BY cdm, account
) AS [cte] ON cte.account = dbo.chrg.account AND cte.cdm = dbo.chrg.cdm
WHERE chrg.mod_date >= GETDATE()-30
AND (status NOT IN (''N/A'', ''CBILL'')) AND (credited = 0) AND (chrg.cdm <> ''CBILL'') AND (NULLIF (invoice, '''') IS NULL)
and cte.Qty = 0			   

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''CREDITED CHARGES '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.sellars@wth.org'',
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT @this_proc_name + '' EMAIL SENT''


PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

UPDATE dbo.chrg
SET credited = 1
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
  	''SQL QUERY '' +CONVERT(VARCHAR(10),GETDATE(),112)),50)
  				
FROM dbo.chrg
INNER JOIN 
(
SELECT     account, cdm, SUM(qty) AS QTY
                            FROM          dbo.chrg AS chrg_1
                            WHERE      (status NOT IN (''N/A'', ''CBILL'')) AND (credited = 0) AND (cdm <> ''CBILL'') AND (NULLIF (invoice, '''') IS NULL)
                            GROUP BY cdm, account
) AS [cte] ON cte.account = dbo.chrg.account AND cte.cdm = dbo.chrg.cdm
WHERE chrg.mod_date >= GETDATE()-30
AND (status NOT IN (''N/A'', ''CBILL'')) AND (credited = 0) AND (chrg.cdm <> ''CBILL'') AND (NULLIF (invoice, '''') IS NULL)
and cte.Qty = 0
PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

END


--------------



SET @tableHtml = NULL;
SET @sub = NULL;

; WITH cte
AS
(
SELECT TOP(100) PERCENT ROW_NUMBER() OVER 
	(PARTITION BY chrg.account,chrg.cdm, chrg.qty
		ORDER BY chrg.account,chrg_num) AS [rn],
chrg.account, chrg.chrg_num, chrg.cdm ,chrg.qty, cte.QTY AS [TQTY], cte.ct				
FROM dbo.chrg
INNER JOIN 
(
SELECT     account, cdm, SUM(qty) AS QTY, COUNT(cdm) AS [ct]
                            FROM          dbo.chrg AS chrg_1
                            WHERE      (status NOT IN (''N/A'', ''CBILL'')) AND (credited = 0) AND (cdm <> ''CBILL'') AND (NULLIF (invoice, '''') IS NULL)
                            GROUP BY cdm, account
) AS [cte] ON cte.account = dbo.chrg.account AND cte.cdm = dbo.chrg.cdm
WHERE chrg.mod_date >= GETDATE()-30
AND (status NOT IN (''N/A'', ''CBILL'')) AND (credited = 0) AND (chrg.cdm <> ''CBILL'') AND (NULLIF (invoice, '''') IS NULL)
and cte.Qty = 1 AND cte.ct > 2
ORDER BY chrg.account, chrg.cdm, chrg.chrg_num
)
select @tableHtml = 
N''<H1> CREDITED CHARGES WITH MULTIPLES </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ACCOUNT</th><th>CHRG_NUM</th><th>CDM</th><th>QTY</th></tr>'' +
CAST (( select td = cte.account,'''',
			   td = cte.chrg_num,'''', 
			   td = cte.cdm,'''',
			   td = cte.Qty,'''' 
FROM chrg
INNER JOIN cte ON cte.account = dbo.chrg.account AND cte.cdm = dbo.chrg.cdm 
AND cte.chrg_num = dbo.chrg.chrg_num
WHERE  [rn] = 1
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''CREDITED CHARGES WITH MULTIPLES '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
--@recipients = N''carol.sellars@wth.org'',
@recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT @this_proc_name + '' EMAIL SENT''


PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

	; WITH cte
	AS
	(
	SELECT TOP(100) PERCENT ROW_NUMBER() OVER 
		(PARTITION BY chrg.account,chrg.cdm, chrg.qty
			ORDER BY chrg.account,chrg_num) AS [rn],
	chrg.account, chrg.chrg_num, chrg.cdm ,chrg.qty, cte.QTY AS [TQTY], cte.ct
					
	FROM dbo.chrg
	INNER JOIN 
	(
	SELECT     account, cdm, SUM(qty) AS QTY, COUNT(cdm) AS [ct]
								FROM          dbo.chrg AS chrg_1
								WHERE      (status NOT IN (''N/A'', ''CBILL'')) AND (credited = 0) AND (cdm <> ''CBILL'') AND (NULLIF (invoice, '''') IS NULL)
								GROUP BY cdm, account
	) AS [cte] ON cte.account = dbo.chrg.account AND cte.cdm = dbo.chrg.cdm
	WHERE chrg.mod_date >= GETDATE()-30
	AND (status NOT IN (''N/A'', ''CBILL'')) AND (credited = 0) AND (chrg.cdm <> ''CBILL'') AND (NULLIF (invoice, '''') IS NULL)
	and cte.Qty = 1 AND cte.ct > 1
	ORDER BY chrg.account, chrg.cdm, chrg.chrg_num
	)
	UPDATE dbo.chrg
	SET credited = 1
	,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
  		''SQL QUERY '' +CONVERT(VARCHAR(10),GETDATE(),112)),50)		
	FROM dbo.chrg
	INNER JOIN cte ON cte.account = dbo.chrg.account AND cte.cdm = dbo.chrg.cdm 
	AND cte.chrg_num = dbo.chrg.chrg_num
	WHERE  [rn] = 1 and cte.ct > 2
	
	PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)
END', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [TEST PATIENTS IN LIVE]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'TEST PATIENTS IN LIVE', 
		@step_id=28, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'return;
set nocount on
declare @startDate datetime
declare @endDate datetime


set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))



set @tableHtml = 
N''<H1> TEST PATIENTS IN LIVE </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>CLIENT</th><th>ACCOUNT</th><th>DOS</th></tr>'' +
CAST (( select td = acc.cl_mnem,'''',
			   td = chrg.account,'''', 
			   --td = chrg.cdm,''''
			   td = CONVERT(VARCHAR(10),acc.trans_date,101),'''' 
FROM dbo.chrg
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account
WHERE cl_mnem = ''LPT''
AND acc.trans_date >= GETDATE()-180
GROUP BY chrg.cdm,acc.status,acc.cl_mnem, acc.fin_code ,CONVERT(VARCHAR(10),acc.trans_date,101),chrg.account
HAVING sum(qty) <> 0
ORDER BY acc.cl_mnem, CONVERT(VARCHAR(10),acc.trans_date,101) 
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Test Patient Charges in LIVE '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.sellars@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', if necessary
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''EMAIL SENT'' -- like CARE 360 EMAIL SENT
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)



', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PRE-ECLAMPTIC PANEL]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PRE-ECLAMPTIC PANEL', 
		@step_id=29, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'return;

set nocount on
declare @startDate datetime
declare @endDate datetime


set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

set @tableHtml = 
N''<H1> PRE-ECLAMPTIC PANELs </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ITEM</th><th>ACCOUNT</th><th>CDM</th>''+
N''<th>FIN TYPE</th><th>NET AMT</th><th>AMT AMOUNT</th><th>DOS</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY chrg.account),'''',
			   td = chrg.account,'''', 
			   td = chrg.cdm,'''', 
			   td = fin_type,'''',
			   td = dbo.chrg.net_amt,'''', 
			   td =  x.aChrg,''''	,
			   td = CONVERT(VARCHAR(10),chrg.service_date,101) ,''''
FROM dbo.chrg
INNER JOIN (
   select account, cdm, SUM(qty*amount) AS [aChrg]
   from chrg
   inner join amt on amt.chrg_num = chrg.chrg_num
   WHERE amt.mod_date > ''08/25/2014 00:00'' AND cdm = ''5382526''
   GROUP BY chrg.account, chrg.cdm) x ON x.account = chrg.account AND x.cdm = chrg.cdm
WHERE chrg.cdm = ''5382526''
AND dbo.chrg.mod_date > ''08/25/2014 00:00''
AND dbo.chrg.credited = 0
AND chrg.net_amt <> x.aChrg

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''PRE-ECLAMPTIC PANELs '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.sellars@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', if necessary
--@copy_recipients=N''bradley.powers@wth.org; david.kelly@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''PRE-ECLAMPTIC PANELS'' -- like CARE 360 EMAIL SENT
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)
					', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PGX WEEKLY TOTALS]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PGX WEEKLY TOTALS', 
		@step_id=30, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'return;
/*
DECLARE @day INT
SELECT @day =  DATEPART(WEEKDAY,GETDATE())
IF (@day = 7)
BEGIN


DECLARE @startDate DATETIME
DECLARE @endDate DATETIME

SET @startDate = GETDATE()-7
SET @endDate = GETDATE()-1

set nocount on

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))



set @tableHtml = 
N''<H1> PGX WEEKLY CHARGES BY FINANCIAL CLASS </H1>''+
N''<H2> FROM ''+convert(VARCHAR(10),@startDate, 101)+'' THRU ''+convert(varchar(10),@endDate,101)+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>CLIENT</th><th>DOS</th><th>FIN CODE</th><th>CHARGES</th></tr>'' +
CAST (( select td = cl_mnem,'''',
			   td = CONVERT(VARCHAR(10),acc.trans_date,101),'''', 
			   td = acc.fin_code,'''',
			   td = STR(SUM(qty*amount),9,2),''''
			   
--select cl_mnem,CONVERT(VARCHAR(10),acc.trans_date,101) AS [DOS]
--, acc.fin_code, SUM(qty*amount) AS [charges]
from chrg
inner join amt on amt.chrg_num = chrg.chrg_num
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account
WHERE amt.mod_date BETWEEN @startDate AND @endDate AND dbo.acc.cl_mnem = ''PGX''
GROUP BY cl_mnem , acc.fin_code, CONVERT(VARCHAR(10),acc.trans_date,101) --WITH ROLLUP

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''PGX Weekly Totals as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', if necessary
--@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''PGX WEEKLY REPORT'' -- like CARE 360 EMAIL SENT
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

END
*/', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [DUPLICATE INSURANCES]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'DUPLICATE INSURANCES', 
		@step_id=31, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
declare @startDate datetime
declare @endDate datetime


set @startDate =  getdate()
set @endDate = getdate()+1

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

set @tableHtml = 
N''<H1> DUPLICATE INSURANCES </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ROW</th><th>ACCOUNT</th><th>PRIMARY</th><th>SECONDARY</th><th>POLICY NUMBER</th><th>GROUP NUMBER</th>''+
N''<th>FIN CODE</th><th>INS CODE</th></tr>'' +

CAST (( select td = ROW_NUMBER() OVER (ORDER BY i1.account),'''',
			td = i1.account,'''',
			   td = i1.ins_a_b_c,'''', 
			   td = i2.ins_a_b_c,'''' ,
			   td = i1.policy_num ,'''',
			   td = coalesce(NULLIF(i1.grp_num,''''),''NA'') ,'''',
			   td =	i1.fin_code ,'''',
			   td = i1.ins_code ,''''
		 FROM dbo.ins i1
INNER JOIN acc ON dbo.acc.account = i1.account AND acc.status = ''NEW''		 
INNER JOIN dbo.ins i2 ON i2.account = i1.account AND i2.policy_num = i1.policy_num
	AND i2.ins_a_b_c = ''B'' and i2.deleted = 0
WHERE i1.ins_a_b_c = ''A''
AND COALESCE(i1.mod_date,i2.mod_date) >= ''07/01/2014 00:00''
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Duplicate Insurances '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', if necessary
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''Duplicate Insurance email sent'' -- like CARE 360 EMAIL SENT
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)


', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CDM's WITH NO AMOUNT]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CDM''s WITH NO AMOUNT', 
		@step_id=32, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
SET QUOTED_IDENTIFIER  OFF 
declare @startDate datetime
declare @endDate datetime

set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

set @tableHtml = 
N''<H1> CDM''''s WITH NO AMOUNT </H1>''+
N''<H2> NON AFFILIATED CLIENTs CHARGES </H2>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>LINE</th><th>ACCOUNT</th><th>CDM</th>'' +
N''<th>DOS</th><th>FIN CODE</th><th>FEE SCHEDULE</th></tr>'' +
CAST (( select	td = ROW_NUMBER() OVER (ORDER BY chrg.account,cdm),'''',
				td = chrg.account,'''',
			   td = chrg.cdm,'''', 
			   td = CONVERT(VARCHAR(10),acc.trans_date,101),'''' ,
			   td = acc.fin_code,'''',
			   td = dbo.client.fee_schedule,''''
			   
--SELECT ROW_NUMBER() OVER (ORDER BY chrg.account,cdm) AS [rn], chrg.account, service_date, net_amt, cdm,
--, acc.fin_code, client.fee_schedule
FROM chrg 
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account
INNER JOIN dbo.client ON dbo.client.cli_mnem = dbo.acc.cl_mnem
WHERE dbo.chrg.net_amt IS NULL
AND dbo.chrg.account LIKE ''[C,D][1-9]%''
AND dbo.acc.status = ''NEW''
AND dbo.chrg.credited = 0
ORDER BY chrg.account,cdm, dbo.chrg.service_date

for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''CDMs With No Amount as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', if necessary
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''CDMs with no amounts email sent'' 


update dbo.acc
set status = ''CDM HOLD''
, mod_prg = @this_proc_name
, mod_date = getdate()
, mod_user = suser_sname()
FROM chrg 
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account
INNER JOIN dbo.client ON dbo.client.cli_mnem = dbo.acc.cl_mnem
WHERE dbo.chrg.net_amt IS NULL
AND dbo.chrg.account LIKE ''[C,D][1-9]%''
AND acc.status = ''NEW''
AND chrg.credited = 0

PRINT @this_proc_name +'' Accounts updated '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

END





	', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=12
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CARE360 BY HL7]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CARE360 BY HL7', 
		@step_id=33, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/*set nocount on

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))
DECLARE @transmissionDate DATETIME
SET @transmissionDate = GETDATE()
DECLARE @file VARCHAR(50)
SET @file = ''Care360_'' + CONVERT(VARCHAR(10), @transmissionDate,102)+''.html''

set @tableHtml = 
N''<!DOCTYPE html>''+
N''<H1> CARE 360 BILLING DATA </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>REQUISITION NUMBER</th></tr>'' +
CAST (( select td = uid,''''--,
			   --td = html_doc,''''			   
FROM dbo.data_quest_360
WHERE bill_type = ''q'' 
AND emailed =0
AND deleted = 0
AND entered = 1
AND charges_entered = 1
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Care360 as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''ATL T-SUPPORT@QuestDiagnostics.com'',
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@query = ''select uid
, replace(html_doc,
''''<IMG SRC= "file://C:/Program Files/Medical Center Laboratory/MCL Billing/mcllogo.bmp">'''','''''''') FROM dbo.data_quest_360 
WHERE bill_type = ''''q'''' AND emailed = 0 AND deleted = 0 AND entered = 1 AND charges_entered = 1'',
@attach_query_result_as_file = 1,
@query_attachment_filename = @file,
@query_no_truncate = 1,
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML''; -- ''TEXT'';



UPDATE dbo.data_quest_360
set emailed	 = 1
, transmission_date = GETDATE()--@transmissionDate
FROM dbo.data_quest_360 
WHERE bill_type = ''q'' 
--AND mod_host =  ''test'' -- origional 10
AND emailed = 0 
AND deleted = 0 AND entered = 1 AND charges_entered = 1


END
*/', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PAID OUT ACCOUNTS WITH CHARGES]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PAID OUT ACCOUNTS WITH CHARGES', 
		@step_id=34, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'/*
SELECT  ROW_NUMBER() OVER (ORDER BY acc.cl_mnem,acc.fin_code,acc.trans_date, acc.account) AS [Item]
,acc.cl_mnem
,acc.account--, acc.status
, acc.fin_code
, CONVERT(VARCHAR(10),acc.trans_date,101) AS trans_date
--, dbo.aging_history.datestamp
, dbo.aging_history.balance 
FROM acc 
INNER JOIN dbo.aging_history ON dbo.aging_history.account = dbo.acc.account
WHERE status = ''PAID_OUT''
AND dbo.aging_history.datestamp >= GETDATE()-2
ORDER BY acc.cl_mnem,acc.fin_code,acc.trans_date, acc.account

*/


set nocount on
SET QUOTED_IDENTIFIER  OFF
 
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))


set @tableHtml = 
N''<H1> PAID_OUT With New Charges </H1>''+
N''<H3> Accounts have been updated</H3>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ITEM</th><th>CLIENT</th><th>ACCOUNT</th><th>FIN CODE</th>''+
N''<th>TRANS DATE</th><th>BALANCE</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY acc.cl_mnem,acc.fin_code,acc.trans_date, acc.account),'''',
			td = acc.cl_mnem,'''',
			   td = acc.account,'''', 
			   td = acc.fin_code,'''',
			    td = CONVERT(VARCHAR(10),acc.trans_date,101),'''',
			    td = STR(dbo.aging_history.balance,10,2),'''' 
FROM acc 
INNER JOIN dbo.aging_history ON dbo.aging_history.account = dbo.acc.account
WHERE status = ''PAID_OUT''
AND dbo.aging_history.datestamp >= GETDATE()-2
ORDER BY  acc.cl_mnem,acc.fin_code,acc.trans_date, acc.account
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''PAID_OUT With New Charges '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT  @this_proc_name +'' Emailed '' + CONVERT(VARCHAR(10), @@ROWCOUNT)-- like CARE 360 EMAIL SENT
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)


UPDATE  acc 
SET status = ''NEW''
,mod_date = GETDATE() 
,mod_user = RIGHT(SUSER_SNAME(),50) 
,mod_prg = COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		''SQL PROC '' +CONVERT(VARCHAR(10),GETDATE(),112)),50),''NO APP IDENTIFIED'')
, mod_host = RIGHT (HOST_NAME(),50)
FROM acc 
INNER JOIN dbo.aging_history ON dbo.aging_history.account = dbo.acc.account
WHERE status = ''PAID_OUT''
AND dbo.aging_history.datestamp >= GETDATE()-2

SET QUOTED_IDENTIFIER  OFF 
 
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=14
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [VENIPUNCTURES ONLY]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'VENIPUNCTURES ONLY', 
		@step_id=35, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'

set nocount on
declare @startDate datetime
declare @endDate datetime


set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))


; WITH cteVeni
AS
(
SELECT chrg.account, cdm, dbo.chrg.service_date , acc.fin_code
FROM chrg 
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account
LEFT OUTER JOIN chk ON dbo.chk.account = dbo.acc.account
WHERE cdm =  ''5527418'' AND dbo.chrg.credited = 0
AND acc.status IN (''NEW'') 
--AND acc.fin_code NOT IN (''D'',''B'')
AND chk.account IS NULL -- no payments
GROUP BY chrg.account, cdm, dbo.chrg.service_date, dbo.acc.fin_code
HAVING SUM(qty) > 0
AND chrg.service_date between ''05/31/2015 00:00'' and  DATEADD(DAY,-7,GETDATE())
)
, cteNon
AS
(
SELECT chrg.account, COUNT(cdm) AS [cdm] FROM chrg 
INNER JOIN dbo.acc ON dbo.acc.account = dbo.chrg.account
WHERE cdm NOT IN ( ''5527418'')
--AND credited =   0
GROUP BY dbo.chrg.account
HAVING COUNT(cdm) > 0	
)
/* for debug purposes
--SELECT *, dbo.GetAccBalByDate(cteVeni.account,GETDATE()) AS [bal] FROM cteVeni
--LEFT OUTER JOIN cteNon ON cteNon.account = cteVeni.account
--WHERE cteNon.account IS NULL
--ORDER BY cteVeni.service_date
--RETURN;
*/

select @tableHtml = (
N''<H1> Venipunctures Only  </H1>''+
--N''<H2> Does Not include "D" financial classes  </H2>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ROW</th><th>ACCOUNT</th><th>CDM</th><th>SERVICE DATE</th><th>FIN CODE</th><th>PROCEDURE</th></tr>'' +
CAST (( select td = ROW_NUMBER() OVER (ORDER BY cteVeni.account),'''',
			   td = cteVeni.account,'''',
			   td = cteVeni.cdm,'''', 
			   td = CONVERT(VARCHAR,cteVeni.service_date,101),'''', 
			   td = cteVeni.fin_code,'''',
			   td = @this_proc_name, '''' 
			   
--SELECT cteVeni.* 
FROM cteVeni
LEFT OUTER JOIN cteNon ON cteNon.account = cteVeni.account
WHERE cteNon.account IS NULL
ORDER BY cteVeni.service_date
--GROUP BY $GB$ IF NECESSARY with rollup
--ORDER BY $OB$ IF NECESSARY
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'');

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Venipuncture Only Accounts as of '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org'',
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT ''$LOG FILE MSG$'' -- like CARE 360 EMAIL SENT


PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

--------------------- update veni''s only
; WITH cteVeni
AS
(
SELECT chrg.account, cdm, dbo.chrg.service_date , acc.fin_code
, chrg.chrg_num
FROM chrg 
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account
LEFT OUTER JOIN chk ON dbo.chk.account = dbo.acc.account
WHERE cdm IN (''5527418'') AND dbo.chrg.credited = 0
AND acc.status IN (''NEW'') 
--AND acc.fin_code NOT IN (''D'',''B'')
AND chk.account IS NULL -- no payments
GROUP BY chrg.account, cdm, dbo.chrg.service_date, dbo.acc.fin_code
,chrg.chrg_num
HAVING SUM(qty) > 0
AND chrg.service_date < DATEADD(DAY,-7,GETDATE())
)
, cteNon
AS
(
SELECT chrg.account, COUNT(cdm) AS [cdm] FROM chrg 
INNER JOIN dbo.acc ON dbo.acc.account = dbo.chrg.account
WHERE cdm NOT IN ( ''5527418'') 
GROUP BY dbo.chrg.account
HAVING COUNT(cdm) > 0	
)
SELECT cteVeni.account ,
		cteVeni.cdm ,
		cteVeni.service_date ,
		cteVeni.fin_code ,
		cteVeni.chrg_num 
INTO #tempVeniE
FROM cteVeni
LEFT OUTER JOIN cteNon ON cteNon.account = cteVeni.account
WHERE cteNon.account IS NULL
ORDER BY cteVeni.service_date

DECLARE @chrgNum NUMERIC
SELECT @chrgNum = (
		SELECT TOP(1) #tempVeniE.chrg_num FROM #tempVeniE
		ORDER BY #tempVeniE.chrg_num)

WHILE (@chrgNum IS NOT NULL)

begin

DELETE FROM #tempVeniE where #tempVeniE.chrg_num = @chrgNum
EXEC dbo.usp_prg_ReverseChargeOnly @chrgNum = @chrgNum, -- numeric
	@comment = ''Reversal of VP only'' -- varchar(50)

SELECT account FROM chrg WHERE dbo.chrg.chrg_num=  @chrgNum

SELECT @chrgNum = (
		SELECT TOP(1) #tempVeniE.chrg_num FROM #tempVeniE
		ORDER BY #tempVeniE.chrg_num)
	

END


DROP TABLE #tempVeniE

END
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [NAME ALIAS]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'NAME ALIAS', 
		@step_id=36, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
SET QUOTED_IDENTIFIER  OFF 
 

declare @startDate datetime
declare @endDate datetime


set @startDate = CONVERT(DATETIME,convert(varchar(10),GETDATE()-1,101)) -- start of yesterday

set @endDate = DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),GETDATE(),101))) -- end of yesterday

SELECT @startDate , @endDate

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))


; WITH cte
AS
(
SELECT TOP(100) percent  account_cerner
,DBO.GetMappingValue(''CLIENT'',''CERNER'',
		COALESCE(
		NULLIF(CAST(msgContent AS XML).value(''(//PV1.3.4/text())[1]''
			, ''varchar(50)'') ,'''')
		,NULLIF(CAST(msgContent AS XML).value(''(//PV1.3.1/text())[1]''
			, ''varchar(50)'') ,'''')
		,NULLIF(CAST(msgContent AS XML).value(''(//PV1.3.7/text())[1]''
			, ''varchar(50)'') ,'''')
			
			 ) ) 	AS [CLIENT]
--, infce.messages_inbound.processFlag
--, CASE WHEN msgType = ''DFT-P03'' THEN  CAST(msgContent AS XML)--.query(''//PID.3.4'')
--	.value(''data(//PV1.20.1/text())[1]'', ''varchar(50)'') end AS [Charge Fin Code]
--, CASE WHEN msgType = ''ADT-A04'' THEN  CAST(msgContent AS XML)--.query(''//PID.3.4'')
--	.value(''data(//PV1.20.1/text())[1]'', ''varchar(50)'') END AS [Demographics Fin Code]
, CAST(msgContent AS XML)--.query(''//PID.3.4'')
	.value(''data(//PID.5.1/text())[1]'', ''varchar(50)'') AS [NAME_LAST]
, CAST(msgContent AS XML)--.query(''//PID.3.4'')
	.value(''data(//PID.5.2/text())[1]'', ''varchar(50)'') AS [NAME_FIRST]
, ISNULL(CAST(msgContent AS XML)--.query(''//PID.3.4'')
	.value(''data(//PID.5.3/text())[1]'', ''varchar(50)''),'''') AS [NAME_MIDDLE]
	

, CAST(msgContent AS XML)
	.value(''data(//PID.9.1/text())[1]'', ''varchar(50)'') AS [ALIAS_LAST]
, CAST(msgContent AS XML)
	.value(''data(//PID.9.2/text())[1]'', ''varchar(50)'') AS [ALIAS_FIRST]
, CAST(msgContent AS XML)
	.value(''data(//PID.9.3/text())[1]'', ''varchar(50)'') AS [ALIAS_MIDDLE]



, CONVERT(VARCHAR,infce.messages_inbound.msgDate,112) AS [msgDate]
--, CAST(msgContent AS XML)--.query(''//PID.3.4'')
--	.value(''data(//PV1.3.4/text())[1]'', ''varchar(50)'') AS [xmlContentPV1.3.4]
--, CAST(msgContent AS XML)--.query(''//PID.3.1'') 
--	.value(''data(//PV1.3.1/text())[1]'', ''varchar(50)'') AS [xmlContentPV1.3.1]
--, CAST(msgContent AS XML)--.query(''//PID.3.5'') 
--	.value(''data(//PV1.3.5/text())[1]'', ''varchar(50)'')AS [xmlContentPV1.3.5]

--, CAST(msgContent AS XML)--.query(''//PID.3.4'')
--	.value(''data(//PID.3.4/text())[1]'', ''varchar(50)'') AS [xmlContentPID.3.4]
--, CAST(msgContent AS XML)--.query(''//PID.3.1'') 
--	.value(''data(//PID.3.1/text())[1]'', ''varchar(50)'') AS [xmlContentPID.3.1]
--, CAST(msgContent AS XML)--.query(''//PID.3.5'') 
--	.value(''data(//PID.3.5/text())[1]'', ''varchar(50)'')AS [xmlContentPID.3.5]
--, CAST(msgContent AS XML)--.query(''//PID.3.5'') 
--	.value(''data(//IN1.2.1/text())[1]'', ''varchar(50)'')AS [xmlContentInsCode]
--, CAST(msgContent AS XML)--.query(''//PID.3.5'') 
--	.value(''data(//IN1.2.2/text())[1]'', ''varchar(50)'')AS [xmlContentInsName]
--,CAST(msgContent AS XML) AS [xmlContent]
FROM infce.messages_inbound
WHERE 
infce.messages_inbound.account_cerner LIKE ''[5][^3]%''
AND infce.messages_inbound.msgDate BETWEEN @startDate AND @endDate
ORDER BY infce.messages_inbound.msgDate
)
, cteName
AS
(
SELECT cte.account_cerner ,
cte.CLIENT
--,cte.[NAME_LAST],cte.NAME_FIRST, cte.NAME_MIDDLE
,cte.[NAME_LAST]+'', ''+cte.NAME_FIRST+'' ''+cte.NAME_MIDDLE
	AS [NAME]

,COALESCE(cte.[ALIAS_LAST],	cte.[NAME_LAST])+'', ''+
	COALESCE(cte.ALIAS_FIRST,cte.NAME_FIRST)+'' ''+
	COALESCE(cte.ALIAS_MIDDLE,cte.NAME_MIDDLE) AS [NAME_ALIAS]

--,cte.[ALIAS_LAST],cte.ALIAS_FIRST, cte.ALIAS_MIDDLE
		,cte.msgDate 
		 
FROM cte

)
select @tableHtml = 
N''<H1> NAME ALIAS </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>LINE</th><th>CLIENT</th><th>ACCOUNT</th>''+
N''<th>NAME</th><th>ALIAS NAME</th><th>MSG DATE</th></tr>'' +
CAST (( select td = ROW_NUMBER()OVER (ORDER BY cteName.account_cerner),'''',
			   td = cteName.CLIENT,'''',
			   td = cteName.account_cerner,'''', 			    
			   td = cteName.NAME,'''', 
			   td = cteName.NAME_ALIAS,'''', 
			   td = cteName.msgDate,'''' 
FROM cteName WHERE cteName.NAME_ALIAS <> cteName.NAME
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';	 

if (len(@tableHtml) > 0)
BEGIN	
	set @sub = ''Alias Name Work List as of '' + convert(varchar(10),getdate(),101)

END
else	
BEGIN
	set @sub = ''Alias Name Work List is negative as of '' + convert(varchar(10),getdate(),101)
END

EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org;'',
--@copy_recipients=N''bradley.powers@wth.org;rita.matheny@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';




SET QUOTED_IDENTIFIER  ON

set nocount off

 
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [FAILED INTERFACE CHARGES]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'FAILED INTERFACE CHARGES', 
		@step_id=37, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
SET QUOTED_IDENTIFIER  ON 

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

set @tableHtml = 
N''<H1> FAILED INTERFACE CHARGES </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ACCOUNT</th><th>SERVICE DATE</th><th>MSG DATE</th><th>MSG ID</th><th>FLAG</th><th>STATUS</th> ''+
''</tr>'' +
CAST (( select td = account_cerner,'''',
			   td = CONVERT(VARCHAR(10), DOS, 101), '''',
			   td = CONVERT(VARCHAR(10), msgDate,101),'''', 
			   td = systemMsgId,'''', 
			   td = processFlag,'''',
			   td = processStatusMsg,'''' 
		   
FROM infce.messages_inbound
WHERE   processFlag not in (''DNP'',''P'') and msgType like ''DFT%''
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
   set @sub = ''FAILED INTERFACE CHARGES '' + convert(varchar(10),getdate(),101)
   EXEC msdb.dbo.sp_send_dbmail
   @profile_name = ''WTHMCLBILL'',
   @recipients = N''Bradley.Powers@wth.org'',
   --@copy_recipients=N''carol.plumlee@wth.org'', -- if necessary
   @body = @tableHtml,
   @subject = @sub,
   @body_format = ''HTML'';

   PRINT @this_proc_name + '' EMAIL SENT''
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

SET QUOTED_IDENTIFIER  OFF', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CHARGES ADDED TO PROCESSED ACCOUNTs]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CHARGES ADDED TO PROCESSED ACCOUNTs', 
		@step_id=38, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'set nocount on
SET QUOTED_IDENTIFIER  ON 
 

declare @startDate datetime
declare @endDate datetime


set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))



set @tableHtml = 
N''<H1> PROCESSED ACCOUNTS THAT HAVE HAD CHARGES ADDED </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ACCOUNT</th><th>STATUS</th><th>TRANS_DATE</th><th>DATE CHRG ADDED</th> ''+
''<th>FIN CODE</th></tr>'' +
CAST (( select td = asu.account,'''',
			   td = asu.acc_status,'''', 
			   td = CONVERT(VARCHAR(10),asu.trans_date,101),'''', 
			   td = CONVERT(VARCHAR(10),asu.mod_date,101),'''',
			   td = acc.fin_code,'''' 
			   
FROM         dbo.acc_status_updates asu
INNER JOIN acc ON dbo.acc.account = asu.account
WHERE   emailed = 0 AND acc.fin_code NOT IN (''w'',''x'',''y'',''z'',''client'')-- carol doesn''t want to see client charges on this report
--GROUP BY $GB$ IF NECESSARY with rollup
ORDER BY asu.trans_date, acc_status, asu.account
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''CHARGES ADDED TO PROCESSED ACCOUNTS '' + convert(varchar(10),getdate(),101)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''carol.plumlee@wth.org;Grant.Holland@wth.org'',
@copy_recipients=N''christopher.burton@wth.org'', -- if necessary
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML'';

PRINT @this_proc_name + '' EMAIL SENT''
END

PRINT @this_proc_name +'' '' + CONVERT(VARCHAR(10), @@ROWCOUNT)

UPDATE dbo.acc_status_updates
SET emailed = 1
WHERE emailed = 0

SET QUOTED_IDENTIFIER  OFF 
 
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [EXECUTE GLOBAL BILLING]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'EXECUTE GLOBAL BILLING', 
		@step_id=39, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
go	

declare @tableHtml nvarchar(max);
declare @sub varchar(250)

--DECLARE @startDate DATETIME
DECLARE @endDate DATETIME

--SET @startDate = ''06/01/2020 00:00''
--SET @startDate = dbo.GetSystemValue(''global_billing_start_date'')
SET @endDate = GETDATE()
 
DECLARE @DIM INT 
SET @DIM = DATEPART(DAY,DATEADD(mm,DATEDIFF(m,0,GETDATE())+1, -.000003))-- Last day This Month

DECLARE @TDAY INT
SET @TDAY = DATEPART(DAY,GETDATE())
SELECT @endDate = (select 
CASE WHEN @DIM - @TDAY > 4
	THEN DATEADD(dd,-4,DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),@endDate+1,101))))
	ELSE DATEADD(dd,@TDAY-@DIM,DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),@endDate+1,101))))
	END AS [endDate])


select @tableHtml = (
N''<center><H1> GLOBAL BILLING CHARGES </H1>''+
N''<H2> Automation of Global Billing complete </H2></center>''+
N''<hr width="75%" color="#ff0000" size="4" />''+
--N''<H3 color = "#ff0000"> CAROL these accounts will have been moved to JPG as of today ''+
--N''be converted let me know. Bradley </H3>''+
N''<table border = "1" bordercolor ="blue">''+
N''<tr bgcolor ="blue"><th>CLIENT</th><th>ACCOUNT</th><th>CHARGE NUM</th><th>CDM</th>''+
''<th>CPT</th><th>QTY</th><th>CHRG AMT</th><th>TRANS DATE</th>''+
''<th>ENTERED DATE</th><th>FIN CODE</th><th>CLIENT TYPE</th>''+
''</tr>'' +

CAST (( select td = colClient,'''',
			   td = colAcc,'''', 
			   td = colChrgNum,'''',
			   td = colCDM,'''',
			   td = colCPT,'''',
			   td = colQty,'''',
			   td = colChrgAmt,'''',
			   td = convert(varchar(10),colDOS,101),'''', 
			   td = convert(varchar(10),colDateEntered,101),'''', 
			   td = colFinCode,'''',
			   td = colClType, ''''		
--SELECT  colClient ,
--        colAcc ,
--        colChrgNum ,
--        colCDM ,
--        colCPT ,
--        colQty ,
--        colChrgAmt ,
--        colDOS ,
--        colDateEntered ,
--        colFinCode ,
--        colClType 
FROM dbo.GetGlobalBillingCharges(NULL)  
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'')

IF (LEN(@tableHTml) > 0)
BEGIN
	set @sub = ''Accounts to be processed by Global Billing '' + convert(varchar(10),getdate(),101)
	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = ''WTHMCLBILL'',
	@recipients = ''carol.plumlee@wth.org; joann.patterson@wth.org'',
	@copy_recipients=N''bradley.powers@wth.org; christopher.burton@wth.org'',
	@body = @tableHtml,
	@subject = @sub,
	@body_format = ''HTML'';
END
GO

exec usp_prg_UpdateGlobalBilling NULL
go
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'BILLING DAILY', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20131224, 
		@active_end_date=99991231, 
		@active_start_time=60000, 
		@active_end_time=235959, 
		@schedule_uid=N'cb929297-b900-4329-a5f9-2d8bc6930f89'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [PROD Daily Hourly Updates]    Script Date: 5/14/2023 12:29:47 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:47 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'PROD Daily Hourly Updates', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Hourly update of table to maintain billing and provides an hourly eamil to Quest for Care360 to keep the records on the email below 90 per eamail.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'Bradley Powers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [UPDATE TABLES]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'UPDATE TABLES', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

set nocount on
declare @startDate datetime
declare @endDate datetime

--SELECT @startDate, @endDate
set @startDate = dateadd(month,-6, getdate())
set @endDate = dateadd(month, 6, getdate())

-- post cerner updates
/* ins relation updates*/
UPDATE TOP(100) PERCENT dbo.ins
SET relation = ''01''
FROM dbo.ins
INNER JOIN acc ON dbo.acc.account = dbo.ins.account AND acc.status = ''NEW''
WHERE ins.relation IN (''SE'',''SELF'')

/* acc pat_name updates */
UPDATE TOP(100) PERCENT dbo.acc
SET pat_name =  rtrim(replace(pat_name,'', '','',''))
,mod_prg = ''SQL Query ''+CONVERT(VARCHAR,GETDATE(),112)
FROM acc WHERE status = ''NEW''
AND (pat_name LIKE ''%, %'' or right(pat_name,1) = char(32))
AND fin_code NOT IN (''CLIENT'',''Y'')


/* pat guar name and zip updates*/
UPDATE top(100)percent pat set		
        pat_full_name = rtrim(replace(p.pat_full_name,'', '','',''))
        ,city_st_zip = COALESCE(dbo.GetNamePart(p.city_st_zip,''LAST'')+'', ''+
					   dbo.GetNamePart(p.city_st_zip,''FIRST'')+'' ''+
					   LEFT(dbo.GetNamePart(p.city_st_zip,''MIDDLE''),5),p.city_st_zip)
        ,guarantor = RTRIM(REPLACE(p.guarantor,'', '','',''))
        ,g_city_st = COALESCE(dbo.GetNamePart(p.g_city_st,''LAST'')+'', ''+
					dbo.GetNamePart(p.g_city_st,''FIRST'')+'' ''+
			        LEFT(dbo.GetNamePart(p.g_city_st,''MIDDLE''),5),p.g_city_st)
        ,pat_zip =  LEFT(p.pat_zip,5)
        ,guar_zip = LEFT(p.guar_zip,5)
        ,mod_prg  = ''SQL QUERY ''+CONVERT(VARCHAR,GETDATE(),112)
OUTPUT DELETED.rowguid , 
          DELETED.deleted , 
          DELETED.account , 
          DELETED.ssn , 
          DELETED.pat_addr1 , 
          DELETED.pat_addr2 , 
          DELETED.city_st_zip , 
          DELETED.dob_yyyy , 
          DELETED.sex , 
          DELETED.relation , 
          DELETED.guarantor , 
          DELETED.guar_addr , 
          DELETED.g_city_st , 
          DELETED.pat_marital , 
          DELETED.icd9_1 , 
          DELETED.icd9_2 , 
          DELETED.icd9_3 , 
          DELETED.icd9_4 , 
          DELETED.icd9_5 , 
          DELETED.icd9_6 , 
          DELETED.icd9_7 , 
          DELETED.icd9_8 , 
          DELETED.icd9_9 , 
          DELETED.icd_indicator , 
          DELETED.pc_code , 
          DELETED.mailer , 
          DELETED.first_dm , 
          DELETED.last_dm , 
          DELETED.min_amt , 
          DELETED.phy_id , 
          DELETED.dbill_date , 
          DELETED.ub_date , 
          DELETED.h1500_date , 
          DELETED.ssi_batch , 
          DELETED.colltr_date , 
          DELETED.baddebt_date , 
          DELETED.batch_date , 
          DELETED.guar_phone , 
          DELETED.bd_list_date , 
          DELETED.ebill_batch_date , 
          DELETED.ebill_batch_1500 , 
          DELETED.e_ub_demand , 
          DELETED.e_ub_demand_date , 
          DELETED.claimsnet_1500_batch_date , 
          DELETED.claimsnet_ub_batch_date , 
          DELETED.mod_date , 
          DELETED.mod_user , 
          DELETED.mod_prg , 
          DELETED.mod_host , 
          DELETED.hne_epi_number , 
          DELETED.pat_full_name , 
          DELETED.pat_city , 
          DELETED.pat_state , 
          DELETED.pat_zip , 
          DELETED.guar_city , 
          DELETED.guar_state , 
          DELETED.guar_zip , 
          DELETED.pat_race , 
          DELETED.pat_phone , 
          DELETED.phy_comment , 
          DELETED.location , 
          DELETED.pat_email , 
          DELETED.dx_update_prg , 
          0	
INTO pat_zip_update          
FROM pat p
INNER JOIN acc a ON a.account = p.account AND a.status = ''NEW''
LEFT OUTER JOIN dbo.pat_zip_update ON dbo.pat_zip_update.rowguid = p.rowguid
WHERE a.trans_date >= @startDate
AND dbo.pat_zip_update.rowguid IS NULL
AND (RIGHT(p.guarantor,1) = CHAR(32)
	OR LEN(p.pat_zip) > 5
	OR LEN(p.guar_zip) > 5)


UPDATE pat set	
		pat_full_name = rtrim(replace(p.pat_full_name,'', '','',''))
        ,city_st_zip =  REPLACE(p.city_st_zip,SUBSTRING(p.city_st_zip,CHARINDEX(''-'',p.city_st_zip),LEN(p.city_st_zip)),'''')
		,guarantor = RTRIM(REPLACE(p.guarantor,'', '','',''))
        ,g_city_st =REPLACE(p.g_city_st,SUBSTRING(p.g_city_st,CHARINDEX(''-'',p.g_city_st),LEN(p.g_city_st)),'''')
        ,pat_zip =  LEFT(p.pat_zip,5)
        ,guar_zip = LEFT(p.guar_zip,5)
        ,mod_prg  = ''SQL QUERY ''+CONVERT(VARCHAR,GETDATE(),112)
OUTPUT DELETED.*,1
INTO pat_zip_update       
  FROM dbo.pat p
INNER JOIN acc ON dbo.acc.account = p.account and acc.status = ''NEW'' 
LEFT OUTER JOIN dbo.pat_zip_update ON dbo.pat_zip_update.rowguid = p.rowguid
WHERE
acc.trans_date >= @startDate and
p.city_st_zip LIKE ''%-%'' 
AND dbo.pat_zip_update.rowguid IS NULL


/* insurance updates for holders name*/
UPDATE  dbo.ins
SET dbo.ins.holder_nme = COALESCE(rtrim(REPLACE(dbo.ins.holder_nme,'', '','',''))
	,rtrim(REPLACE(acc.pat_name,'', '','','') ) ),
	dbo.ins.holder_lname = COALESCE(rtrim(dbo.ins.holder_lname),dbo.GetNamePart(acc.pat_name,''LAST'') ),
	dbo.ins.holder_fname = COALESCE(rtrim(dbo.ins.holder_fname),dbo.GetNamePart(acc.pat_name,''FIRST'') ),
	dbo.ins.holder_mname = COALESCE(rtrim(dbo.ins.holder_mname),dbo.GetNamePart(acc.pat_name,''MIDDLE'')),
	dbo.ins.holder_sex = COALESCE(ins.holder_sex,pat.sex)	,
	dbo.ins.holder_dob = COALESCE(ins.holder_dob, pat.dob_yyyy),
	dbo.ins.holder_addr = COALESCE(ins.holder_addr,pat.pat_addr1),
	dbo.ins.holder_city_st_zip = COALESCE(left(ins.holder_addr,5),LEFT(pat.pat_addr1,5))
from dbo.ins
INNER JOIN acc ON dbo.acc.account = dbo.ins.account
INNER JOIN pat ON pat.account = ins.account
WHERE acc.status = ''NEW''
AND NULLIF(ins.holder_nme,'''') IS NULL


UPDATE  dbo.ins
SET holder_nme = RTRIM(REPLACE(i.holder_nme,'', '','',''))
, holder_fname = RTRIM(LTRIM(i.holder_fname))
, holder_mname = RTRIM(LTRIM(i.holder_mname))
, holder_lname = RTRIM(LTRIM(i.holder_lname))
FROM ins i
INNER JOIN acc ON dbo.acc.account = i.account AND acc.status = ''NEW''
WHERE i.mod_date > CONVERT(DATETIME,CONVERT(varchar(10),@startDate,101))
AND	(RIGHT(i.holder_nme,1) = CHAR(32)					
	 OR i.holder_nme LIKE ''%, %'')

/*diagnosis code ptr update in amt*/
; WITH cteChrgAmt
AS
(
SELECT chrg.account, chrg.chrg_num 
FROM dbo.chrg
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account AND acc.fin_code NOT IN (''Y'',''CLIENT'')
	AND acc.status = ''NEW''
INNER JOIN pat ON dbo.pat.account = dbo.acc.account AND pat.icd9_1 IS NOT NULL AND pat.icd9_2 IS null
INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num AND diagnosis_code_ptr IS NULL
AND chrg.mod_date > ''11/01/2016 00:00''

)
UPDATE  dbo.amt
SET diagnosis_code_ptr = ''1:'',
	--mod_date = GETDATE(), ; wdk 20170103 don''t update the date it causes accounting errors.
    mod_user = RIGHT(SUSER_SNAME(),50) ,
    mod_prg = ''SQL DG_CODE_PTR '' +CONVERT(VARCHAR(10),GETDATE(),112)
FROM amt 
INNER JOIN cteChrgAmt ON cteChrgAmt.chrg_num = dbo.amt.chrg_num
WHERE diagnosis_code_ptr IS null
--


-- icd9/10 effective 12/07/2016
update pat 
 SET icd_indicator = ''I10''
 WHERE COALESCE(NULLIF(icd_indicator,''''),icd_indicator) IS NULL AND 
 mod_date > ''12/01/2016''

/*
UPDATE  dbo.ins
SET dbo.ins.holder_nme = COALESCE(dbo.ins.holder_nme,rtrim(acc.pat_name) ),
	dbo.ins.holder_lname = COALESCE(dbo.ins.holder_lname,dbo.GetNamePart(acc.pat_name,''LAST'') ),
	dbo.ins.holder_fname = COALESCE(dbo.ins.holder_fname,dbo.GetNamePart(acc.pat_name,''FIRST'') ),
	dbo.ins.holder_mname = COALESCE(dbo.ins.holder_mname,dbo.GetNamePart(acc.pat_name,''MIDDLE'')),
	dbo.ins.holder_sex = COALESCE(ins.holder_sex,pat.sex)	,
	dbo.ins.holder_dob = COALESCE(ins.holder_dob, pat.dob_yyyy),
	dbo.ins.holder_addr = COALESCE(ins.holder_addr,pat.pat_addr1),
	dbo.ins.holder_city_st_zip = COALESCE(ins.holder_addr,pat.pat_addr1)
from dbo.ins
INNER JOIN acc ON dbo.acc.account = dbo.ins.account
INNER JOIN pat ON pat.account = ins.account
WHERE acc.status = ''NEW''
AND NULLIF(ins.holder_nme,'''') IS NULL
*/

UPDATE TOP(100) PERCENT dbo.ins
SET relation = ''01''
FROM dbo.ins
INNER JOIN acc ON dbo.acc.account = dbo.ins.account
WHERE acc.status = ''NEW''
AND NULLIF(ins.relation,'''') IS NULL





/* deledted effective 12/07/2016
UPDATE pat
SET icd_indicator = ''I9'' 
FROM pat
INNER JOIN acc ON dbo.acc.account = dbo.pat.account
WHERE NULLIF(dbo.pat.icd_indicator,'''') IS NULL
AND dbo.acc.trans_date BETWEEN ''05/31/2015 00:00'' 
	AND ''09/30/2015 23:59:59''
	AND acc.status = ''new''
	

UPDATE pat
SET icd_indicator = ''I10'' 
FROM pat
INNER JOIN acc ON dbo.acc.account = dbo.pat.account
WHERE NULLIF(dbo.pat.icd_indicator,'''') IS NULL
AND dbo.acc.trans_date >= ''10/01/2015 00:00''
	AND acc.status = ''new''
-- icd9/10
*/

UPDATE TOP(100) PERCENT pat 
SET pat_full_name = acc.pat_name
FROM pat 
INNER JOIN acc ON dbo.acc.account = dbo.pat.account
--INNER JOIN dbo.patbill_acc ON dbo.patbill_acc.account_id = pat.account
WHERE NULLIF(dbo.pat.pat_full_name ,'''') IS NULL
AND acc.trans_date >= @startDate

----- per cerner updates
update ins
set grp_num = replace(grp_num,''-'','''')
from ins
where ins_code = ''AETNA'' and mod_date > ''01/01/2014'' and grp_num like ''%-%''
select ''AETNA UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update amt
set modi = replace(modi,'' '','''')
where modi like ''% %''
and mod_date between @startDate and @endDate
select ''AMT MODI UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update amt
set modi2 = replace(modi2,'' '','''')
where modi2 like ''% %''
and mod_date between @startDate and @endDate
select ''AMT MODI2 UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update amt
set modi = null
where modi = ''''
and mod_date between @startDate and @endDate
select ''AMT MODI UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

update amt
set modi2 = null
where modi2 = ''''
and mod_date between @startDate and @endDate
select ''AMT MODI2 UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

-- update the charge fin_codes 
update chrg
set invoice = null
where invoice = ''''
select ''CHRG INVOICE UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE    chrg
SET       fin_code = acc.fin_code
FROM         chrg INNER JOIN
                      acc ON acc.account = chrg.account
WHERE     (chrg.service_date > @startDate) AND (chrg.fin_code IS NULL OR
                      chrg.fin_code = '''') AND (chrg.cdm <> ''cbill'')
select ''CHRG FIN CODES UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

-- update insurance info
UPDATE    ins
SET             policy_num = replace(policy_num, ''-'','''')
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE      (ins.policy_num LIKE ''%-%'') AND (acc.status = ''new'')
select ''INS POLICY DASH REMOVAL UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE    ins
SET             policy_num = replace(policy_num, '' '','''')
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE     (ins.ins_code in (''bc'', ''cigna'')) AND (ins.policy_num LIKE ''% %'') AND (acc.status = ''new'')
select ''INS POLICY SPACE REMOVAL UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)


UPDATE    ins
SET   
plan_nme = ''CHAMPUS TRICARE'',          
plan_addr1 = ''P.O.BOX 7031'',
p_city_st = ''CAMDEN, SC 29020''
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE     (ins.ins_code = ''CHAMPUS'') 
AND plan_nme <> ''CHAMPUS TRICARE'' AND (acc.status = ''new'')
select ''INS CHAMPUS UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE    ins
SET             
plan_addr1 = ''UHC OF THE RIVER VALLEY'',
plan_addr2 = ''3800 AVE OF CITIES, SUITE 200'',
p_city_st = ''MOLINE, IL 61265'',
plan_nme = ''UHC COMMUNITY PLAN''
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE     (ins.ins_code = ''AM'') 
AND plan_nme <> ''UHC COMMUNITY PLAN'' 
AND (acc.status = ''new'')
select ''INS UHC UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE    ins
SET             
plan_addr1 = ''1 CAMERON HILL CIRCLE STE 0002'',
p_city_st = ''CHATTANOOGA, TN 374020002'',
plan_nme = ''BLUECARE/TNCARE SEL''
FROM         ins INNER JOIN
                      acc ON acc.account = ins.account
WHERE     (ins.ins_code = ''TNBC'') AND plan_nme = ''BLUECARE/TNCARE SELECT'' AND (acc.status = ''new'')
select ''INS BLUECARE UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

UPDATE INS
SET HOLDER_ADDR =	null,
holder_city_st_zip = null
WHERE HOLDER_ADDR = ''PO BOX 3490''
select ''INS HOLDER UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

--update the blank ones diagnosis code pointers
update amt
set diagnosis_code_ptr = ''1:''
where chrg_num in 
	(select chrg_num from chrg 
	 inner join pat on pat.account = chrg.account
	 inner join acc on acc.account = chrg.account
	 where icd9_1 is not null 
		)
and diagnosis_code_ptr is NULL
select ''DIAGNOSIS CODE POINTER UPDATES ''+ CONVERT(VARCHAR(10),@@rowcount)

', 
		@database_name=N'master', 
		@output_file_name=N'H:\sqlText\Daily_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CARE 360 EMAIL]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CARE 360 EMAIL', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
DECLARE @Part INT
SELECT @Part = CONVERT(INT, DATEPART(HOUR,GETDATE())-12)*10

if (@Part < 50 or @Part > 100)
begin
	SELECT ''NOT TIME FOR CARE360 EMAIL''
	return
END

select @Part = 90

set nocount on

declare @tableHtml nvarchar(max);
declare @sub varchar(250)
DECLARE @query VARCHAR(8000)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))
DECLARE @transmissionDate DATETIME
SET @transmissionDate = GETDATE()
DECLARE @file VARCHAR(50)
SET @file = ''Care360_'' + CONVERT(VARCHAR(10), @transmissionDate,102)+''.html''

set @tableHtml = 
N''<!DOCTYPE html>''+
N''<H1> CARE 360 BILLING DATA </H1>''+
N''<table border = "1" bordercolor = "blue">''+
N''<tr bgcolor = "blue"><th>ITEM</th><th>REQUISITION NUMBER</th></tr>'' +
CAST (( 
SELECT TOP(@Part) td = ROW_NUMBER() OVER (ORDER BY uid),'''',
td = uid,''''
FROM dbo.data_quest_360
WHERE bill_type = ''q'' 
AND emailed = 0
AND deleted = 0
AND entered = 1
AND charges_entered = 1
order by UID
for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
N''</Table>'';

if (len(@tableHtml) > 0)
BEGIN
set @sub = ''Care360 as of '' + convert(varchar(10),getdate(),101)
SET @query = ''select top(''+CONVERT(VARCHAR(5),@part)+'')  
stuff( replace(html_doc,
''''<IMG SRC= "file://C:/Program Files/Medical Center Laboratory/MCL Billing/mcllogo.bmp">'''','''''''') 
,7,0,''''<div style="page-break-before: always">Requistion number ''''+cast(uid as varchar)+''''</div>'''' )
FROM dbo.data_quest_360 
WHERE bill_type = ''''q'''' AND emailed = 0 AND deleted = 0 AND entered = 1 AND charges_entered = 1 
order by uid''

EXEC msdb.dbo.sp_send_dbmail
@profile_name = ''WTHMCLBILL'',
@recipients = N''atltsupport@questdiagnostics.com;Kathy.E.Edwards@questdiagnostics.com'',
--@blind_copy_recipients=N''bradley.powers@wth.org'', 
@copy_recipients=N''bradley.powers@wth.org'', -- if necessary
--@recipients = N''bradley@bradleypowers.com'', -- for testing 
@query = @query,
@attach_query_result_as_file = 1,
@query_attachment_filename = @file,
@query_no_truncate = 1,
@body = @tableHtml,
@subject = @sub,
@body_format = ''HTML''; -- ''TEXT'';


--
; with cte
as
(
 select TOP(@Part) 
dbo.data_quest_360.uid
FROM dbo.data_quest_360 
WHERE bill_type = ''q'' 
AND emailed = 0 
AND deleted = 0 AND entered = 1 AND charges_entered = 1
ORDER BY uid
)
UPDATE dbo.data_quest_360
set emailed	 = 1
, transmission_date = GETDATE()--@transmissionDate
FROM dbo.data_quest_360 
INNER JOIN CTE on cte.uid = dbo.data_quest_360.uid

print ''REQ_NO UPDATES ''+ convert(varchar(5),@@ROWCOUNT)

END', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\Daily_Job_Care360.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'DAILY HOURLY UPDATES', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=8, 
		@freq_subday_interval=1, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20221010, 
		@active_end_date=99991231, 
		@active_start_time=10000, 
		@active_end_time=220000, 
		@schedule_uid=N'fc37ea38-4335-40cc-b730-d50dc2aed6f4'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [PROD Diagnosis ORM]    Script Date: 5/14/2023 12:29:47 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:47 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'PROD Diagnosis ORM', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'Bradley Powers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ORDER INFO UPDATE]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ORDER INFO UPDATE', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET NOCOUNT ON

DECLARE @this_proc_name VARCHAR(256)
	SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                         +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

DECLARE @startDate DATETIME
DECLARE @endDate DATETIME

SET @startDate = CONVERT(DATETIME,convert(varchar(10)
,DATEADD(DAY,-1,GETDATE() ) ,101))
SET @endDate = GETDATE()

--SELECT @startDate, @endDate

DECLARE @count INT
SET @count = 0

DECLARE @msg NUMERIC(18,0)
SET @msg = (
SELECT TOP(1) mi.systemMsgId
	
	FROM infce.messages_inbound mi
	WHERE mi.order_pat_id IS NULL 
	   OR mi.order_visit_id IS NULL
	   OR mi.DOS IS NULL
	ORDER BY mi.msgDate
)
--SELECT @msg 

WHILE (@msg IS NOT NULL AND @count < 101)
BEGIN
	IF OBJECT_ID(''tempdb..#tempInfceQueryC'',''U'') IS NOT NULL
	BEGIN
		DROP TABLE #tempInfceQueryC
	END
	CREATE TABLE #tempInfceQueryC (
		account VARCHAR(15),
		msgid NUMERIC(18,0),
		msgtype VARCHAR(3),
		processFlag VARCHAR(5),
		dx_processed BIT,
		xmlContent VARCHAR(MAX))
	INSERT INTO #tempInfceQueryC
			( account, msgid,msgType
			,processFlag,dx_processed
			, xmlContent )
	SELECT TOP(101) mi.account_cerner AS [account]
		, mi.systemMsgId
		, LEFT(mi.msgType,3) AS [msgType]
		, mi.processFlag
		, mi.dx_processed
		, REPLACE(mi.msgContent,''"'','''') AS [msgContent]
		FROM infce.messages_inbound mi
		WHERE (mi.order_pat_id IS NULL 
	   OR mi.order_visit_id IS NULL
	   OR mi.DOS IS NULL)
	   AND mi.msgDate >= @startDate

--SELECT * FROM #tempInfceQueryC

	UPDATE infce.messages_inbound
	SET DOS = ISNULL(CAST(CAST(xmlContent AS XML)
		.value(''(//PV1.44.1/text()) [1] '',''varchar(8)'') AS DATETIME),'''')
	, infce.messages_inbound.order_visit_id = 
		ISNULL(CAST(xmlContent AS XML)
		.value(''(//PID.4.1/text())	[1]'', ''varchar(50)'') ,'''') 
	, infce.messages_inbound.order_pat_id
		= ISNULL(CAST(xmlContent AS XML)
			.value(''(//PV1.19.1/text())	[1]'', ''varchar(50)'') ,'''')
	, mod_date = GETDATE()
	FROM infce.messages_inbound mi
	INNER JOIN #tempInfceQueryC	c ON c.msgid = mi.systemMsgId

SET @msg = (
SELECT TOP(1) mi.systemMsgId
	
	FROM infce.messages_inbound mi
	WHERE (mi.order_pat_id IS NULL 
	   OR mi.order_visit_id IS NULL
	   OR mi.DOS IS NULL)
	   AND mi.msgDate >= @startDate
	ORDER BY mi.msgDate
)
SET @count = @count+1

END




', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'h:\sqlText\Diagnosis_ORM_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [ORM UPDATE ACCOUNT]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'ORM UPDATE ACCOUNT', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET NOCOUNT ON

DECLARE @this_proc_name VARCHAR(256)
	SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                         +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))

DECLARE @startDate DATETIME
DECLARE @endDate DATETIME

SET @startDate = 
	CONVERT(DATETIME,convert(varchar(10)
	,DATEADD(DAY,-1,GETDATE() ) ,101))
SET @endDate = GETDATE()
SELECT @startDate, @endDate

WHILE (EXISTS (SELECT TOP(1) ''L''+mi.account_cerner AS [account]
	FROM infce.messages_inbound mi
	WHERE mi.DOS BETWEEN @startDate AND @endDate
	AND LEFT(mi.msgType,3) =  ''ORM'' 
	AND mi.account_cerner IS NULL) )
	
BEGIN
	
IF OBJECT_ID(''tempdb..#tempInfceQuery201512020900'',''U'') IS NOT NULL
BEGIN
	DROP TABLE #tempInfceQuery201512020900
END
CREATE TABLE #tempInfceQuery201512020900 (
	account VARCHAR(15),
	msgid NUMERIC(18,0),
	msgtype VARCHAR(3),
	order_pat_id VARCHAR(50),
	order_visit_id VARCHAR(50),
	xmlContent VARCHAR(MAX))

INSERT INTO #tempInfceQuery201512020900
		( account, msgid,msgType, order_pat_id, order_visit_id, xmlContent )
SELECT TOP(1) ''L''+mi.account_cerner AS [account]
	, mi.systemMsgId
	, LEFT(mi.msgType,3) AS [msgType]
	, mi.order_pat_id, mi.order_visit_id
	, mi.msgContent
	FROM infce.messages_inbound mi
	WHERE mi.DOS BETWEEN @startDate AND @endDate
	AND LEFT(mi.msgType,3) =  ''ORM'' 
	AND mi.account_cerner IS NULL

INSERT INTO #tempInfceQuery201512020900
		( account, msgid,msgType, order_pat_id, order_visit_id, xmlContent )
SELECT TOP(1)''L''+mi.account_cerner AS [account]
	, mi.systemMsgId
	, LEFT(mi.msgType,3) AS [msgType]
	, mi.order_pat_id, mi.order_visit_id
	, mi.msgContent
	FROM infce.messages_inbound mi
	INNER JOIN #tempInfceQuery201512020900
		ON #tempInfceQuery201512020900.order_pat_id = mi.order_pat_id
		AND #tempInfceQuery201512020900.order_visit_id = mi.order_visit_id
	INNER JOIN pat ON dbo.pat.account = 
		''L''+mi.account_cerner	
	WHERE mi.DOS BETWEEN @startDate AND @endDate
	AND LEFT(mi.msgType,3) =  ''ADT'' 
	AND NULLIF(pat.icd9_1,'''') IS null

--SELECT * FROM #tempInfceQuery201512020900
	
IF (EXISTS (SELECT account FROM #tempInfceQuery201512020900
	WHERE #tempInfceQuery201512020900.msgtype = ''ADT'') )
BEGIN
	DECLARE @acc VARCHAR(15)
	SET @acc =(SELECT account FROM #tempInfceQuery201512020900
	WHERE #tempInfceQuery201512020900.msgtype = ''ADT'')
	DECLARE @msg NUMERIC(18,0)
	SELECT @msg =(SELECT msgid FROM #tempInfceQuery201512020900
	WHERE #tempInfceQuery201512020900.msgtype = ''ORM'')	
	
	UPDATE infce.messages_inbound
	SET account_cerner = @acc
	from infce.messages_inbound i
	WHERE i.systemMsgId = @msg 
END 
else
BEGIN
	DECLARE @msgNo NUMERIC(18,0)
	SELECT @msgNo =(SELECT msgid FROM #tempInfceQuery201512020900
	WHERE #tempInfceQuery201512020900.msgtype = ''ORM'')	
	
	UPDATE infce.messages_inbound
	SET account_cerner = COALESCE((
	select TOP(1) ''L''+infce.messages_inbound.account_cerner
	FROM infce.messages_inbound
	INNER JOIN #tempInfceQuery201512020900
		ON	 infce.messages_inbound.order_pat_id
			= #tempInfceQuery201512020900.order_pat_id 
	AND infce.messages_inbound.order_visit_id = 
	#tempInfceQuery201512020900.order_visit_id
	WHERE infce.messages_inbound.msgType = ''ADT-A04'')
		,''PAT'')
	from infce.messages_inbound 
	WHERE systemMsgId = @msgNo 
END	
END
', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\SqlText\Diagnosis_ORM_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [UPDATE PAT]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'UPDATE PAT', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'return;

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET NOCOUNT ON



DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +''.''+QUOTENAME(OBJECT_NAME(@@PROCID))


IF OBJECT_ID(''tempdb..#tempQueryORMUpdate'',''U'') IS NOT NULL
BEGIN
	DROP TABLE #tempQueryORMUpdate
END
CREATE TABLE #tempQueryORMUpdate (
	account VARCHAR(15),
	msgid NUMERIC(18,0),
	msgType VARCHAR(3),
	msgDate DATETIME,
	xmlContent VARCHAR(MAX))
INSERT INTO #tempQueryORMUpdate
		( account, msgid, msgType, msgDate, xmlContent )
SELECT mi.account_cerner AS [account]
	, mi.systemMsgId
	, LEFT(mi.msgType,3)
	, CONVERT(DATETIME,convert(varchar(10),mi.DOS,101))
	, mi.msgContent
	FROM infce.messages_inbound mi
	
		INNER JOIN pat ON 
			replace(pat.account,''L'','''') = 
			replace(mi.account_cerner,''L'','''')
		INNER JOIN acc ON dbo.acc.account = dbo.pat.account
		WHERE NULLIF(pat.icd9_1,'''') IS NULL
		AND acc.status = ''NEW''
		AND mi.msgType LIKE ''ORM%''
		AND acc.fin_code NOT IN (''W'',''X'',''Y'',''Z'',''client'') 
	
	
--SELECT ''temp'' AS [table],* FROM #tempQueryORMUpdate

DECLARE @msgId INT

SET @msgId = (
SELECT TOP(1) 
		#tempQueryORMUpdate.msgid 
		FROM #tempQueryORMUpdate
		WHERE #tempQueryORMUpdate.msgType = ''ORM''

)
--SELECT @msgId

IF (@msgId IS NULL)
BEGIN
	PRINT ''No work at this time ''
	RETURN;
END

DECLARE @acc VARCHAR(15)
DECLARE @nt INT
SET @nt = 0

DECLARE @xml XML


WHILE (@nt < 101 AND @msgId IS NOT NULL)
BEGIN

SET @xml = (
SELECT  	CONVERT(XML, REPLACE(M.xmlContent,''"'','''') ) AS xmlMsg
	FROM #tempQueryORMUpdate M
	WHERE M.msgType = ''ORM''	
	AND M.msgId = @msgID
	)

--SELECT @xml
--return;

SET @acc = ( 
SELECT TOP(1) M.account FROM #tempQueryORMUpdate M
	WHERE M.msgType = ''ORM''	
	AND M.msgId = @msgID)

; WITH cteDX
	AS
	(	

	SELECT  DISTINCT @acc AS [account], 	
		code.doc.value(''(./DG1.1/DG1.1.1/text())[1]'', ''nvarchar(10)'') AS [RN],
		code.doc.value(''(./DG1.3/DG1.3.1/text())[1]'', ''nvarchar(10)'') AS [ICD],
		code.doc.value(''(./DG1.2/DG1.2.1/text())[1]'', ''nvarchar(10)'') AS [IND]
		from 
		@xml.nodes(''//DG1'') AS code(doc) 
	)	
	SELECT 	cteDX.rn,
	cteDX.account, 
	CASE WHEN ISNULL(cteDX.IND,''I10'') = ''I10''
		THEN REPLACE(cteDX.ICD,''.'','''')
		ELSE cteDX.ICD END AS [colDiagnosis]
	, ISNULL(cteDX.IND,''I9'') AS [cteDX.colIndicator]
	INTO #temp
	FROM cteDx		

	UPDATE dbo.pat
	SET icd9_1 = (SELECT colDiagnosis FROM #temp WHERE rn = 1)
	,icd9_2 = ISNULL((SELECT colDiagnosis FROM #temp WHERE rn = 2),NULL)
	,icd9_3 = ISNULL((SELECT colDiagnosis FROM #temp WHERE rn = 3),NULL)
	,icd9_4 = ISNULL((SELECT colDiagnosis FROM #temp WHERE rn = 4),NULL)
	,icd9_5 = ISNULL((SELECT colDiagnosis FROM #temp WHERE rn = 5),NULL)
	,icd9_6 = ISNULL((SELECT colDiagnosis FROM #temp WHERE rn = 6),NULL)
	,icd9_7 = ISNULL((SELECT colDiagnosis FROM #temp WHERE rn = 7),NULL)
	,icd9_8 = ISNULL((SELECT colDiagnosis FROM #temp WHERE rn = 8),NULL)
	,icd9_9 = ISNULL((SELECT colDiagnosis FROM #temp WHERE rn = 9),NULL)
	,mod_prg = COALESCE(@this_proc_name,''Job DIAGNOSIS ORM Step 3-Update Pat ''+CONVERT(VARCHAR,GETDATE(),112))
	FROM pat
	INNER JOIN #temp ON #temp.account = pat.account

	--SELECT * FROM #temp
	--PRINT @acc

	UPDATE infce.messages_inbound
	SET dx_processed = 1
	, processFlag = ''P''
	, processStatusMsg = @acc
	WHERE infce.messages_inbound.systemMsgId = @msgId
--END
DELETE 
--SELECT * 
FROM #tempQueryORMUpdate
WHERE #tempQueryORMUpdate.msgid = @msgId

SET @msgId = 
(
SELECT TOP(1) 
		#tempQueryORMUpdate.msgid 
		FROM #tempQueryORMUpdate
		WHERE #tempQueryORMUpdate.msgType = ''ORM''
)

DROP TABLE #temp

IF (@msgId IS NULL)
BEGIN
	PRINT ''No work at this time''
	break;
END


SET @nt = @nt+1
END', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'h:\sqlText\Diagnosis_ORM_Job.log', 
		@flags=6
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'ORM UPDATE', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=8, 
		@freq_subday_interval=1, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20151112, 
		@active_end_date=99991231, 
		@active_start_time=44500, 
		@active_end_time=214500, 
		@schedule_uid=N'd237bbf6-121c-46cf-a541-ee26fde35058'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [PROD PSA Daily Report Email]    Script Date: 5/14/2023 12:29:47 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:47 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'PROD PSA Daily Report Email', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Queries the the data tables for pathology activity. If there is no activity, it generated an email to PSA as a notification to not expect a data file. The 2nd step sends a notification when a new cdm is created.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'Bradley Powers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [usp_psa_demo_activity_check]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'usp_psa_demo_activity_check', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC usp_psa_demo_activity_check', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\psa_demo_activity_log', 
		@flags=2
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [CDM Additions and Changes]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'CDM Additions and Changes', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'declare @tableHtml nvarchar(max);
DECLARE @yesterday DATETIME;
DECLARE @today DATETIME;

SELECT @today = dateadd(day,datediff(day,0,GETDATE()),0),
	@yesterday = DATEADD(day,datediff(day,1,GETDATE()),0)

IF EXISTS(
	SELECT * FROM dbo.GetAddedCDMs(@yesterday, @today)
)
BEGIN
	select @tableHtml = (
	N''<span style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; color: #000000;">''+
	N''<H1> MEDICAL CENTER LABORATORY </H1>''+
	N''<H2> ADDED/MODIFIED CDMS</H2>''+
	N''<P>The following cdms have either been created or modified. Please review your mapping and make any adjustments necessary.''+
	N''If you have any questions, please contact Carol Plumlee or Bradley Powers.</P>''+
	N''<table border = "1" bordercolor ="blue">''+
	N''<tr bgcolor ="blue"><th>CDM</th><th>DESCRIPTION</th><th>CPT CODES</th>'' +
	CAST (( select td = cdm,'''',
				   td = descript,'''',
				   td = cpt,''''
	from dbo.GetAddedCDMs(@yesterday,@today)
	for XML PATH(''tr''), TYPE) as NVARCHAR(MAX))+
	N''</Table></span>'')
	--PRINT @tableHtml;

	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = ''WTHMCLBILL'',
	@recipients = ''audra.steen@mckesson.com;kimberly.owens@mckesson.com;bradley.powers@wth.org'', 
	@body = @tableHtml,
	@subject = ''MCL Added/Modified CDM'',
	@body_format = ''HTML'';
END', 
		@database_name=N'LabBillingProd', 
		@output_file_name=N'H:\sqlText\EmailCDMChanges.txt', 
		@flags=2
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily at 0700', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20150720, 
		@active_end_date=99991231, 
		@active_start_time=70000, 
		@active_end_time=235959, 
		@schedule_uid=N'080e8b18-0ded-4a5e-9665-86abb51348d3'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

/****** Object:  Job [PROD Table Purge LabBillingProd]    Script Date: 5/14/2023 12:29:47 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 5/14/2023 12:29:47 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'PROD Table Purge LabBillingProd', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Purge rows from tables based on system criteria', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', 
		@notify_email_operator_name=N'Bradley Powers', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Purge infce.messages_inbound]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Purge infce.messages_inbound', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @fm_infce_messages_inbound INT
SELECT @fm_infce_messages_inbound = s.value FROM [system] s WHERE s.key_name = ''fm_infce_messages_inbound''
DELETE infce.messages_inbound
WHERE msgDate < GETDATE()-@fm_infce_messages_inbound', 
		@database_name=N'LabBillingProd', 
		@database_user_name=N'dbo', 
		@output_file_name=N'H:\sqlText\MCLLIVE Purge Tables.txt', 
		@flags=2
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Purge infce.messages_inbound_adt]    Script Date: 5/14/2023 12:29:47 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Purge infce.messages_inbound_adt', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @fm_infce_messages_adt INT
SELECT @fm_infce_messages_adt = s.value FROM [system] s WHERE s.key_name = ''fm_infce_messages_adt''
DELETE INFCE.messages_inbound_adt
WHERE msgDate < GETDATE()-@fm_infce_messages_adt
', 
		@database_name=N'LabBillingProd', 
		@database_user_name=N'dbo', 
		@output_file_name=N'H:\sqlText\MCLLIVE Purge Tables.txt', 
		@flags=2
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Daily at 1800', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20170404, 
		@active_end_date=99991231, 
		@active_start_time=180000, 
		@active_end_time=235959, 
		@schedule_uid=N'99bcc70d-2685-4153-9b22-529134f4f0b7'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO

