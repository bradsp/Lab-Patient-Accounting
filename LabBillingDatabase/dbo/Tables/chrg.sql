CREATE TABLE [dbo].[chrg] (
    [rowguid]                UNIQUEIDENTIFIER CONSTRAINT [DF_chrg_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [credited]               BIT              CONSTRAINT [DF_chrg_deleted_2__12] DEFAULT ((0)) NOT NULL,
    [chrg_num]               NUMERIC (15)     IDENTITY (1, 1) NOT NULL,
    [account]                VARCHAR (15)     NULL,
    [status]                 VARCHAR (15)     CONSTRAINT [DF_chrg_status_1__10] DEFAULT ('NEW') NULL,
    [service_date]           DATETIME         CONSTRAINT [DF_chrg_service_date_1__11] DEFAULT (getdate()) NULL,
    [hist_date]              DATETIME         CONSTRAINT [DF_chrg_hist_date_1__11] DEFAULT (getdate()) NULL,
    [cdm]                    VARCHAR (7)      NULL,
    [qty]                    NUMERIC (4)      NULL,
    [retail]                 MONEY            CONSTRAINT [DF_chrg_retail_1__11] DEFAULT ((0.00)) NULL,
    [inp_price]              MONEY            CONSTRAINT [DF_chrg_inp_price_1__11] DEFAULT ((0.00)) NULL,
    [comment]                VARCHAR (50)     NULL,
    [invoice]                VARCHAR (15)     NULL,
    [mod_date]               DATETIME         CONSTRAINT [DF_chrg_mod_date_3__12] DEFAULT (getdate()) NULL,
    [mod_user]               VARCHAR (50)     CONSTRAINT [DF_chrg_mod_user_5__12] DEFAULT (suser_sname()) NULL,
    [mod_prg]                VARCHAR (50)     CONSTRAINT [DF_chrg_mod_prg_4__12] DEFAULT (app_name()) NULL,
    [net_amt]                MONEY            CONSTRAINT [DF_chrg_net_amt_1__22] DEFAULT ((0.00)) NULL,
    [fin_type]               VARCHAR (1)      NULL,
    [mod_host]               VARCHAR (50)     CONSTRAINT [DF_chrg_mod_host_1__16] DEFAULT (host_name()) NULL,
    [mt_req_no]              VARCHAR (50)     NULL,
    [post_date]              DATETIME         NULL,
    [fin_code]               VARCHAR (50)     NULL,
    [performing_site]        VARCHAR (50)     NULL,
    [bill_method]            VARCHAR (50)     NULL,
    [post_file]              VARCHAR (50)     NULL,
    [lname]                  VARCHAR (100)    NULL,
    [fname]                  VARCHAR (100)    NULL,
    [mname]                  VARCHAR (100)    NULL,
    [name_suffix]            VARCHAR (100)    NULL,
    [name_prefix]            VARCHAR (100)    NULL,
    [pat_name]               VARCHAR (100)    NULL,
    [order_site]             VARCHAR (100)    NULL,
    [pat_ssn]                VARCHAR (100)    NULL,
    [unitno]                 VARCHAR (50)     NULL,
    [location]               VARCHAR (50)     NULL,
    [responsiblephy]         VARCHAR (50)     NULL,
    [mt_mnem]                VARCHAR (50)     NULL,
    [action]                 VARCHAR (50)     NULL,
    [facility]               VARCHAR (50)     NULL,
    [referencereq]           VARCHAR (50)     NULL,
    [pat_dob]                DATETIME         NULL,
    [chrg_err]               VARCHAR (8000)   NULL,
    [istemp]                 VARCHAR (50)     NULL,
    [age_on_date_of_service] AS               (nullif(datediff(year,coalesce([pat_dob],getdate()-(1)),coalesce([service_date],getdate()-(1))),(0))),
    [calc_amt]               AS               ([dbo].[GetAmountTotal]([chrg_num])*[qty]),
    CONSTRAINT [PK_chrg_1__10] PRIMARY KEY NONCLUSTERED ([chrg_num] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE CLUSTERED INDEX [account_cdx]
    ON [dbo].[chrg]([account] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [invoice_idx]
    ON [dbo].[chrg]([invoice] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_chrg_mod_date]
    ON [dbo].[chrg]([mod_date] ASC, [bill_method] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_status, cdm INCLUDE chrg_num, account, qty, retail, inp_price, mod_date, net_amt, fin_type]
    ON [dbo].[chrg]([cdm] ASC, [status] ASC)
    INCLUDE([chrg_num], [account], [qty], [retail], [inp_price], [mod_date], [net_amt], [fin_type]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_service_date, cdm INCLUDE chrg_num, account, qty]
    ON [dbo].[chrg]([service_date] ASC, [cdm] ASC)
    INCLUDE([chrg_num], [account], [qty], [fin_code]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_credited,cdm,mtreqno]
    ON [dbo].[chrg]([credited] ASC, [cdm] ASC, [mt_req_no] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_rowguid]
    ON [dbo].[chrg]([rowguid] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_postfile]
    ON [dbo].[chrg]([post_file] ASC)
    INCLUDE([chrg_num], [account], [qty], [retail], [inp_price], [mod_date], [net_amt], [fin_type]) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David
-- Create date: 01/02/2014
-- Description:	Update drug screens
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_DRUG_SCREENS] 
   ON  [dbo].[chrg] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	declare @count int
	select @count = (select count(mod_prg) from inserted where mod_prg like 'ACC%')
	if (@count <= 0)
	BEGIN
		return;
	END
	


	;with cteCdm
		as
		(
		select distinct cdm from cpt4
		where cpt4 = '80101'
		union 
		select cdm from cpt4_2
		where cpt4 = '80101'
		union 
		select cdm from cpt4_3
		where cpt4 = '80101'
		union 
		select cdm from cpt4_4
		where cpt4 = '80101'
		union 
		select cdm from cpt4_5
		where cpt4 = '80101'
		)	
		select @count = (select count(cteCdm.cdm) from cteCdm
						inner join inserted i on i.cdm = cteCdm.cdm)
	if (@count = 0)	
	return;

	declare @fin_code  varchar(10)
	set @fin_code = (select fin_code from inserted)
	declare @chrgNum numeric(18,0)
	set @chrgNum = (select chrg_num from inserted)
	declare @insCode varchar(10)
	set @insCode = (select ins_code from ins where account = (select account from inserted) and ins_a_b_c = 'A')
	
	if (@fin_code in ('A','M') or (@fin_code = 'L' and @insCode in ('WIN','HUM')) or (@fin_code = 'H' and @insCode in ('HESP')))	
	begin
		
		exec usp_prg_Drug_Screens @OrigChrgNum = @chrgNum
	end
	

END

GO
DISABLE TRIGGER [dbo].[TRIGGER_DRUG_SCREENS]
    ON [dbo].[chrg];


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 03/21/2008
-- Description:	Audit trigger for charges
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_chrg] 
   ON  [dbo].[chrg] 
   AFTER UPDATE
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
	Insert into audit_chrg
	( chrg_rowguid, credited, chrg_num, account, [status], 
		service_date, hist_date, cdm, qty, retail, 
		inp_price, comment, invoice, mod_date, mod_user,
		 mod_prg, net_amt, fin_type, mod_host, post_date, fin_code,performing_site)
	select
		d.rowguid, d.credited, d.chrg_num, d.account, d.[status], 
		d.service_date, d.hist_date, d.cdm, d.qty, d.retail, 
		d.inp_price, d.comment, d.invoice, d.mod_date, d.mod_user, 
		d.mod_prg, d.net_amt, d.fin_type, d.mod_host, d.post_date, d.fin_code,d.performing_site

	from deleted d
	end
	
	Insert into audit_chrg
	( chrg_rowguid, credited, chrg_num, account, [status], 
		service_date, hist_date, cdm, qty, retail, 
		inp_price, comment, invoice, mod_date, mod_user,
		 mod_prg, net_amt, fin_type, mod_host, post_date, fin_code,performing_site)
	select
		i.rowguid, i.credited, i.chrg_num, i.account, i.[status], 
		i.service_date, i.hist_date, i.cdm, i.qty, i.retail, 
		i.inp_price, i.comment, i.invoice, getdate(), suser_sname(), 
		left(app_name(),50), i.net_amt, i.fin_type, host_name(), i.post_date, i.fin_code,i.performing_site

	from inserted i

END


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 03/21/2008
-- Description:	Delete trigger for Charges
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_DELETE_chrg] 
   ON  [dbo].[chrg] 
   AFTER DELETE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
/* now get the rowguid from the chk table to see if a delete has happended if the rowguid
		is null then perform the delete audit tracking.
*/
	select @rowguid = c.chrg_rowguid
	from	 audit_chrg c inner join deleted d on c.chrg_rowguid = d.rowguid
	-- Insert statements for trigger here
	/* update the audit_XXX fields before changing the XXX's mod_XXX fields */
	if (@rowguid is NULL)
	Begin
	/*Now see if the rowguid is in the audit table if not no previous edits have been made
		so put the original record in the audit table intact*/	
	Insert into audit_chrg
	( chrg_rowguid, credited, chrg_num, account, [status], 
		service_date, hist_date, cdm, qty, retail, 
		inp_price, comment, invoice, mod_date, mod_user,
		 mod_prg, net_amt, fin_type, mod_host, post_date, fin_code,performing_site)
	select
		d.rowguid, d.credited, d.chrg_num, d.account, d.[status], 
		d.service_date, d.hist_date, d.cdm, d.qty, d.retail, 
		d.inp_price, d.comment, d.invoice, d.mod_date, d.mod_user, 
		d.mod_prg, d.net_amt, d.fin_type, d.mod_host, d.post_date, d.fin_code, d.performing_site

	from deleted d
	
	end


	Insert into audit_chrg
	( chrg_rowguid, credited, chrg_num, account, [status], 
		service_date, hist_date, cdm, qty, retail, 
		inp_price, comment, invoice, mod_date, mod_user,
		 mod_prg, net_amt, fin_type, mod_host, post_date, fin_code, performing_site)
	select
		d.rowguid, d.credited, d.chrg_num, d.account, d.[status], 
		d.service_date, d.hist_date, d.cdm, d.qty, d.retail, 
		d.inp_price, d.comment, d.invoice, getdate(), RIGHT(suser_sname(),50), 
		RIGHT('D~'+app_name(),50), d.net_amt, d.fin_type, RIGHT(host_name(),50)
		, d.post_date, d.fin_code, d.performing_site

	from deleted d

declare @msg varchar(8000)	
set @msg = 
	(select
		 'Account: '+d.account + char(13)+
		'Charge Number: '+ convert(varchar(18),d.chrg_num) + char(13)+
		'Application: '+app_name() +char(13)+
		'User: '+ suser_sname() + char(13) +
		'ServiceDate: '+convert(varchar(10),d.service_date,101)+ char(13)+ char(13)+
		'Audit Rowguid: '+convert(varchar(256),d.rowguid)
	from deleted d)
declare @sub  varchar(256)
set @sub  = (select 'Deleted Charge from: '+ convert(varchar(128),DB_NAME()))
exec msdb.dbo.sp_send_dbmail 
	@profile_name = 'Outlook',
	@recipients = 'david.kelly@wth.org;bradley.powers@wth.org',
	@body = @msg ,
	@subject = @sub


END

GO
DISABLE TRIGGER [dbo].[TRIGGER_AUDIT_DELETE_chrg]
    ON [dbo].[chrg];


GO
-- =============================================
-- Author:		David
-- Create date: 07/03/2014
-- Description:	Identify accounts where status is not "NEW'
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_ACC_STATUS] 
   ON  [dbo].[chrg] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for trigger here
    IF (NOT EXISTS (SELECT credited FROM INSERTED WHERE credited = 0))
    BEGIN
		RETURN
	END
   
    IF (EXISTS(SELECT acc.status FROM acc 
		INNER JOIN INSERTED i ON i.account = acc.account WHERE acc.status NOT IN ('ERR', 'NEW')))
	BEGIN
	INSERT INTO dbo.acc_status_updates
			(
				account ,
				acc_status ,
				trans_date ,
				chrg_dos ,
				mod_date,
				dbo.acc_status_updates.mod_user,
				dbo.acc_status_updates.mod_prg,
				dbo.acc_status_updates.mod_host
			)
	SELECT RIGHT(i.account,15)
		, RIGHT(acc.status,10)
		, CONVERT(VARCHAR(10),acc.trans_date,101)
		, CONVERT(VARCHAR(10),i.service_date,101)
		, GETDATE()
		,RIGHT(SUSER_SNAME(),50)
		,RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
			'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) ,
		RIGHT (HOST_NAME(),50)
	FROM acc
	INNER JOIN INSERTED i ON i.account = acc.account WHERE acc.status NOT IN ('ERR','NEW')
	
	END
		

END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'03/21/2008 wdk added for auditing', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'03/21/2008 wdk changed from 20 charaters', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'mod_user';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'03/21/2008 wdk changed from 20 chars', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'mod_prg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'03/21/2008 wdk changed from 20 characters', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'mod_host';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20090930 added for tracking charges that should be credited from PostChrg', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'mt_req_no';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'20091123 added for end of month accounting', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'post_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20101026 added to make Posting Date work for each charge.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'fin_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20131008 to split location where charges can be billed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'bill_method';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140627 to prevent duplicate posting of files', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'post_file';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'lname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'fname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'mname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'name_suffix';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'name_prefix';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'pat_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130705 Added to flatten charges into the amt table. order_site .', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'order_site';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'pat_ssn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'unitno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'responsiblephy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'mt_mnem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'action';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'facility';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'referencereq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'pat_dob';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'chrg_err';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130624 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg', @level2type = N'COLUMN', @level2name = N'istemp';

