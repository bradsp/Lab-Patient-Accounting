CREATE TABLE [dbo].[chk] (
    [rowguid]              UNIQUEIDENTIFIER CONSTRAINT [DF_chk_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]              BIT              CONSTRAINT [DF_chk_deleted_2__13] DEFAULT ((0)) NOT NULL,
    [pay_no]               NUMERIC (15)     IDENTITY (1, 1) NOT NULL,
    [account]              VARCHAR (15)     NOT NULL,
    [chk_date]             DATETIME         NULL,
    [date_rec]             DATETIME         NULL,
    [chk_no]               VARCHAR (25)     NULL,
    [amt_paid]             MONEY            CONSTRAINT [DF_chk_amt_paid_1__16] DEFAULT ((0)) NULL,
    [write_off]            MONEY            CONSTRAINT [DF_chk_write_off_4__16] DEFAULT ((0)) NULL,
    [contractual]          MONEY            CONSTRAINT [DF_chk_contractual_2__16] DEFAULT ((0)) NULL,
    [status]               VARCHAR (15)     CONSTRAINT [DF_chk_status_3__16] DEFAULT ('NEW') NULL,
    [source]               VARCHAR (50)     NULL,
    [fin_code]             VARCHAR (10)     NULL,
    [w_off_date]           DATETIME         NULL,
    [invoice]              VARCHAR (15)     NULL,
    [batch]                NUMERIC (15)     NULL,
    [comment]              VARCHAR (50)     NULL,
    [bad_debt]             BIT              CONSTRAINT [DF_chk_bad_debt_1__13] DEFAULT ((0)) NOT NULL,
    [mod_date]             DATETIME         CONSTRAINT [DF_chk_mod_date_1__15] DEFAULT (getdate()) NOT NULL,
    [mod_user]             VARCHAR (50)     CONSTRAINT [DF_chk_mod_user_3__15] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_prg]              VARCHAR (50)     CONSTRAINT [DF_chk_mod_prg_2__15] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_host]             VARCHAR (50)     CONSTRAINT [DF_chk_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [mod_date_audit]       DATETIME         CONSTRAINT [DF_chk_mod_date_audit] DEFAULT (getdate()) NOT NULL,
    [cpt4Code]             VARCHAR (50)     NULL,
    [post_file]            VARCHAR (256)    NULL,
    [chrg_rowguid]         UNIQUEIDENTIFIER NULL,
    [write_off_code]       VARCHAR (4)      NULL,
    [eft_date]             DATETIME         NULL,
    [eft_number]           VARCHAR (50)     NULL,
    [post_date]            DATETIME         NULL,
    [ins_code]             VARCHAR (10)     NULL,
    [claim_adj_code]       VARCHAR (50)     NULL,
    [claim_adj_group_code] VARCHAR (50)     NULL,
    [facility_code]        VARCHAR (50)     NULL,
    [claim_no]             VARCHAR (50)     NULL,
    CONSTRAINT [PK_chk_pay_no] PRIMARY KEY CLUSTERED ([pay_no] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [batch_idx]
    ON [dbo].[chk]([batch] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [chk_no_idx]
    ON [dbo].[chk]([chk_no] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [invoice_idx]
    ON [dbo].[chk]([invoice] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_rowguid INCLUDE account]
    ON [dbo].[chk]([rowguid] ASC)
    INCLUDE([account]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_chk_mod_date_status]
    ON [dbo].[chk]([mod_date] ASC, [status] ASC)
    INCLUDE([account], [amt_paid], [write_off], [contractual]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_account]
    ON [dbo].[chk]([account] ASC) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/01/2007
-- Description:	Audit trigger for changes made tothe chk table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_chk] 
   ON  dbo.chk 
   after UPDATE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
/* update the audit_XXX fields before changing the XXX's mod_XXX fields */
	select @rowguid = c.chk_rowguid
	from	 audit_chk c inner join inserted i on c.chk_rowguid = i.rowguid
		
	if (@rowguid is NULL)
	Begin
	Insert into audit_chk
	(chk_rowguid, deleted, pay_no, account, chk_date, date_rec, chk_no, amt_paid, 
		write_off, contractual, status, source, w_off_date, invoice, batch, 
		comment, bad_debt, mod_date, mod_user, mod_prg, mod_host, mod_date_audit,
		cpt4Code, post_file, chrg_rowguid, write_off_code, eft_date, eft_number )
	select
	del.rowguid, del.deleted, del.pay_no, del.account, del.chk_date, 
	del.date_rec, del.chk_no, del.amt_paid, del.write_off, del.contractual, 
	del.status, del.source, del.w_off_date, del.invoice, del.batch, del.comment, 
    del.bad_debt, del.mod_date, del.mod_user, del.mod_prg, del.mod_host,
	del.mod_date_audit, del.cpt4Code, del.post_file, del.chrg_rowguid, 
	del.write_off_code, del.eft_date, del.eft_number 

	from deleted del
	end

	Insert into audit_chk
	(chk_rowguid, deleted, pay_no, account, chk_date, 
		date_rec, chk_no, amt_paid, write_off, contractual, 
		[status], source, w_off_date, invoice, batch, 
		comment, bad_debt, mod_date, mod_user, mod_prg, mod_host, 
		mod_date_audit,
		cpt4Code, post_file, chrg_rowguid, write_off_code, eft_date, eft_number  )
	select
	ins.rowguid, ins.deleted, ins.pay_no, ins.account, ins.chk_date, 
	ins.date_rec, ins.chk_no, ins.amt_paid, ins.write_off, ins.contractual, 
	ins.[status], ins.source, ins.w_off_date, ins.invoice, ins.batch, 
	ins.comment, ins.bad_debt, ins.mod_date, suser_sname(), app_name(), host_name(),
	getdate() -- this trigger is different from the rest don't use the ins.mod_date
	-- because the ins.mod_date is used for accounting reports like aging!!!!
	,
	ins.cpt4Code, ins.post_file, ins.chrg_rowguid, ins.write_off_code,
	ins.eft_date, ins.eft_number 
	from inserted ins
    

END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/01/2007
-- Description:	Moves deleted records to the audit table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_DELETED_chk] 
   ON  dbo.chk 
   AFTER DELETE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
/* update the audit_XXX fields before changing the XXX's mod_XXX fields */
	select @rowguid = c.chk_rowguid
	from	 audit_chk c inner join deleted d on c.chk_rowguid = d.rowguid
		
	if (@rowguid is NULL)
	Begin
	Insert into audit_chk
	(chk_rowguid, deleted, pay_no, account, chk_date, date_rec, chk_no, amt_paid, 
		write_off, contractual, [status], source, w_off_date, invoice, batch, 
		comment, bad_debt, mod_date, mod_user, mod_prg, mod_host, mod_date_audit,
		cpt4Code, post_file, chrg_rowguid, write_off_code )
	select
	del.rowguid, del.deleted, del.pay_no, del.account, del.chk_date, 
	del.date_rec, del.chk_no, del.amt_paid, del.write_off, del.contractual, 
	del.[status], del.source, del.w_off_date, del.invoice, del.batch, del.comment, 
    del.bad_debt, del.mod_date, del.mod_user, del.mod_prg, del.mod_host,
	del.mod_date_audit, del.cpt4Code, del.post_file, del.chrg_rowguid, 
	del.write_off_code

	from deleted del
	end

--	from deleted del
-- old record with mod fields from delete
	Insert into audit_chk
	(chk_rowguid, deleted, pay_no, account, chk_date, 
		date_rec, chk_no, amt_paid, write_off, contractual, 
		[status], source, w_off_date, invoice, batch, 
		comment, bad_debt, mod_date, mod_user, mod_prg, 
		mod_host, 
		mod_date_audit,
		cpt4Code, post_file, chrg_rowguid, write_off_code  )
	select
	del.rowguid, del.deleted, del.pay_no, del.account, del.chk_date, 
	del.date_rec, del.chk_no, del.amt_paid, del.write_off, del.contractual, 
	del.[status], del.source, del.w_off_date, del.invoice, del.batch, 
	del.comment,  del.bad_debt, del.mod_date, suser_sname(), 'D~'+app_name(), -- 'D~' in the audit_XXX indicats the record was REMOVED from the XXX table.
	host_name(),
	getdate(), -- different from others because the mod_date is used for ageing and other reports.
	del.cpt4Code, del.post_file, del.chrg_rowguid, 	del.write_off_code

	from deleted del
END

GO

-- =============================================
-- Author:		David
-- Create date: 02/06/2014
-- Description:	Send an email when the account is for Quest in order to do the Pathologist billing
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_QUEST_PAYMENT] 
   ON  [dbo].[chk] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	if ((select account from inserted) = 'QUESTR')
	begin
	declare @sub varchar(516)
	set @sub = 'Quest Check Posted as of ' + convert(varchar(10),getdate(),101)
	EXEC msdb.dbo.sp_send_dbmail
    @profile_name = 'wthmclbill',
    --@recipients = 'david.kelly@wth.org',
	@recipients = 'carol.sellars@wth.org',
	@copy_recipients=N'bradley.powers@wth.org; christopher.burton@wth.org;david.kelly@wth.org',
    @body = 'Time to run P:\wkelly\SQL Server Management Studio\Projects\GLOBAL BILLING\Path Payments from Bluecare.sql',
    @subject = @sub ;
	end

END


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20120728 changed from varchar(20)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'chk_no';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100621 added for better calculations of payment percentage of charges.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'fin_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is used from the vw_pay as the check''s actual date received. Use the mod_date_audit for triggers', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'mod_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'storing the cpt4 and modifier when posted from an 835 file', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'cpt4Code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'For a batch file posting from an 835 file', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'post_file';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rowguid from the charge table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'chrg_rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'02/27/2008 added to help with write off tracking. Per Ed.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'write_off_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/06/08 used as part of the electronic check posting filter from ISA09', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'eft_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/06/08 used as part of the electronic check posting filter from GS05', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'eft_number';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'20091123 added for end of month process for accounting', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'post_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100712 Added to collect ins_code of payer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'ins_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130722 Added to track the 223 contractual codes from Medicare', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'claim_adj_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404011 to add new tracking of payments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'claim_adj_group_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404011 to add new tracking of payments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chk', @level2type = N'COLUMN', @level2name = N'facility_code';

