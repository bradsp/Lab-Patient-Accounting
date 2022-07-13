CREATE TABLE [dbo].[acc] (
    [rowguid]          UNIQUEIDENTIFIER CONSTRAINT [DF_acc_rowguid] DEFAULT (newid()) ROWGUIDCOL NULL,
    [deleted]          BIT              CONSTRAINT [DF_acc_deleted_2__12] DEFAULT ((0)) NOT NULL,
    [account]          VARCHAR (15)     NOT NULL,
    [pat_name]         VARCHAR (40)     NULL,
    [cl_mnem]          VARCHAR (10)     NULL,
    [fin_code]         VARCHAR (10)     NULL,
    [trans_date]       DATETIME         NULL,
    [cbill_date]       DATETIME         NULL,
    [status]           VARCHAR (8)      CONSTRAINT [DF_acc_status_1__10] DEFAULT ('NEW') NULL,
    [ssn]              VARCHAR (11)     NULL,
    [num_comments]     INT              CONSTRAINT [DF_acc_num_comments_1__15] DEFAULT ((0)) NULL,
    [meditech_account] VARCHAR (15)     NULL,
    [original_fincode] VARCHAR (1)      NULL,
    [oereqno]          VARCHAR (15)     NULL,
    [mri]              VARCHAR (25)     NULL,
    [post_date]        DATETIME         NULL,
    [ov_order_id]      VARCHAR (50)     NULL,
    [ov_pat_id]        VARCHAR (50)     NULL,
    [mod_date]         DATETIME         CONSTRAINT [DF_acc_mod_date_3__12] DEFAULT (getdate()) NOT NULL,
    [mod_user]         VARCHAR (50)     CONSTRAINT [DF_acc_mod_user_5__12] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_prg]          VARCHAR (50)     CONSTRAINT [DF_acc_mod_prg_4__12] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_host]         VARCHAR (50)     CONSTRAINT [DF_acc_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [bill_priority]    INT              DEFAULT ((0)) NOT NULL,
    [guarantorID]      VARCHAR (50)     NULL,
    [HNE_NUMBER]       VARCHAR (50)     NULL,
    [trans_date_time]  DATETIME         NULL,
    [tdate_update]     BIT              CONSTRAINT [df_col_tdate_update] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_acc_account] PRIMARY KEY CLUSTERED ([account] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_status_fincode]
    ON [dbo].[acc]([status] ASC, [fin_code] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_status_acct_fincode_name]
    ON [dbo].[acc]([status] ASC, [account] ASC, [fin_code] ASC, [pat_name] ASC)
    INCLUDE([cl_mnem], [trans_date]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_status_transdate_fincode]
    ON [dbo].[acc]([status] ASC, [trans_date] ASC, [fin_code] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_cli_mnem]
    ON [dbo].[acc]([cl_mnem] ASC)
    INCLUDE([account], [fin_code]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_mri]
    ON [dbo].[acc]([mri] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_meditech_acct]
    ON [dbo].[acc]([meditech_account] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_pat_name]
    ON [dbo].[acc]([pat_name] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_ssn]
    ON [dbo].[acc]([ssn] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_transdate_include_acct]
    ON [dbo].[acc]([trans_date] ASC)
    INCLUDE([account]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix fin_code, trans_date, status INCLUDE deleted, account, pat_name, cl_mnem, cbill_date, plus more]
    ON [dbo].[acc]([fin_code] ASC, [trans_date] ASC, [status] ASC)
    INCLUDE([deleted], [account], [pat_name], [cl_mnem], [cbill_date], [ssn], [num_comments], [meditech_account], [original_fincode], [mod_date], [mod_user], [mod_prg], [oereqno], [mri]) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David
-- Create date: 11/26/2014
-- Description:	Send email when patients name contains an apostrophe
-- =============================================
CREATE TRIGGER dbo.TRIGGER_APOSTROPHE 
   ON  dbo.acc 
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
    
declare @tableHtml nvarchar(max);
declare @sub varchar(250)

DECLARE @this_proc_name VARCHAR(256)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +'.'+QUOTENAME(OBJECT_NAME(@@PROCID))

set @tableHtml = 
N'<H1> NAME with APOSTROPHE </H1>'+
N'<H2>'+ @this_proc_name+'</H2>'+
N'<table border = "1" bordercolor = "blue">'+
N'<tr bgcolor = "blue"><th>ACCOUNT</th><th>PATIENT</th><th>DOS</th></tr>' +
CAST (( select td = i.account,'',
			   td = REPLACE(i.pat_name,'''',''),'', 
			   td = CONVERT(VARCHAR(10),i.trans_date,101),''
			   
FROM    INSERTED i
WHERE   i.pat_name LIKE '%''%' AND i.cl_mnem = 'QUESTR'

for XML PATH('tr'), TYPE) as NVARCHAR(MAX))+
N'</Table>';

if (len(@tableHtml) > 0)
	BEGIN
	set @sub = 'APOSTROPHE ' + convert(varchar(10),getdate(),101)
	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = 'Outlook',
	@recipients = N'david.kelly@wth.org', 
	@body = @tableHtml,
	@subject = @sub,
	@body_format = 'HTML';
	END
END

GO
-- =============================================
-- Author:		David and Rick
-- Create date: 05/24/2007
-- Description:	Moves deleted records to the Audit table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_DELETED_acc] 
   ON  [dbo].[acc] 
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from

	SET NOCOUNT ON;
-- old record before delete
--	Insert into audit_acc
--	( acc_rowguid,
--		deleted,
--		account,
--		pat_name,
--		cl_mnem,
--		fin_code,
--		trans_date,
--		cbill_date,
--		status,
--		ssn,
--		num_comments,
--		meditech_account,
--		original_fincode,
--		mod_date,
--		mod_user,
--		mod_prg,
--		oereqno,
--		mri,
--		mod_host, post_date, ov_order_id, ov_pat_id)
--	select
--		del.rowguid,del.deleted,del.account,del.pat_name,
--		del.cl_mnem,
--		del.fin_code,
--		del.trans_date,
--		del.cbill_date,
--		del.status,
--		del.ssn,
--		del.num_comments,
--		del.meditech_account,
--		del.original_fincode,
--		del.mod_date,
--		del.mod_user,
--		del.mod_prg,
--		del.oereqno,
--		del.mri,
--		del.mod_host, del.post_date, del.ov_order_id, del.ov_pat_id
--
--
--
--	from deleted del

-- old record with mod fields from delete
	Insert into audit_acc
	( acc_rowguid,
		deleted,
		account,
		pat_name,
		cl_mnem,
		fin_code,
		trans_date,
		cbill_date,
		status,
		ssn,
		num_comments,
		meditech_account,
		original_fincode,
		mod_date,
		mod_user,
		mod_prg,
		oereqno,
		mri,
		mod_host, post_date, ov_order_id, ov_pat_id)
	select
		del.rowguid,
		del.deleted,
		del.account,
		del.pat_name,
		del.cl_mnem,
		del.fin_code,
		del.trans_date,
		del.cbill_date,
		del.status,
		del.ssn,
		del.num_comments,
		del.meditech_account,
		del.original_fincode,
		getdate(),
		suser_sname(),
		'D~'+app_name(),
		del.oereqno,
		del.mri,
		host_name(), del.post_date, del.ov_order_id, del.ov_pat_id

	from deleted del


END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 09/04/2007
-- Description:	Moves the deleted (updated) and the inserted (updating) record to the audit table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_acc] 
   ON  [dbo].[acc] 
   AFTER UPDATE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
DECLARE @mod_date datetime
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	-- First check the audit_XXX table for the rowguid of the record being
	-- inserted. If it is not there insert the original recored from the 
	-- XXX table into the audit_XXX table.
	select @rowguid = a.acc_rowguid
	from	 audit_acc a inner join inserted i on a.acc_rowguid = i.rowguid
		
	if (@rowguid is NULL)
	begin
		insert into audit_acc
		(acc_rowguid, deleted,  account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, 
		mod_date, mod_user, mod_prg, oereqno, mri, mod_host)
		select
		d.rowguid, d.deleted, d.account, d.pat_name, d.cl_mnem, 
			d.fin_code, d.trans_date, d.cbill_date, d.status, d.ssn, 
			d.num_comments, d.meditech_account, d.original_fincode, 
			d.mod_date, d.mod_user, d.mod_prg, d.oereqno, d.mri, 
			d.mod_host
		from deleted d
	end

	-- Once the original record is handled use the inserted record to
	-- track the mod_XXXX fields and store in the audit_XXX table
	insert into audit_acc
		(acc_rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, 
		mod_date, mod_user, mod_prg, oereqno, mri, mod_host)
	select
		i.rowguid, i.deleted, i.account, i.pat_name, i.cl_mnem, 
			i.fin_code, i.trans_date, i.cbill_date, i.status, i.ssn, 
			i.num_comments, i.meditech_account, i.original_fincode, 
		    getdate(), right(suser_sname(),50), right(app_name(),50), i.oereqno, i.mri, --rgc/wdk 20110803 changed
			--getdate(), suser_sname(), i.mod_prg, i.oereqno, i.mri,
			right(host_name(),50)
	from inserted i
	
END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'used to flag a record deleted - typically not used in this table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'deleted';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'account number for patient - per visit. This is used as the linking key throughout the db structure.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'account';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'patient''s name in [lname,fname] mi format', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'pat_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'submitting client mnemonic - lookup from client table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'cl_mnem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'financial class - lookup from fin table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'fin_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date of service - collection date of submitted specimen', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'trans_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'not used', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'cbill_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'status of account - can be NEW, CLOSED, PAID_OUT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'patient''s social security number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'ssn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'number of comments on an account - no longer used', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'num_comments';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'account number as entered in Meditech', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'meditech_account';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fin code of account when it was originally entered', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'original_fincode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'requisition number from OE system - not used', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'oereqno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'medical record (unit) number from Meditech system', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'mri';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20091116 added for aging use ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'post_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100215 added for tracking EHS billing back to the original order. Should be NULL for MCLOE accounts.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'ov_order_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100215 added for tracking EHS billing back to the original order. Should be NULL for MCLOE accounts.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'ov_pat_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'date record was last modified', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'mod_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'user who last modified this record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'mod_user';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'program used to add/modify the record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'mod_prg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140225 change how secondary and tertiary billing is done.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'bill_priority';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140407 to track the guarantors by account', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc', @level2type = N'COLUMN', @level2name = N'guarantorID';

