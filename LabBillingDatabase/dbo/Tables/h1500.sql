CREATE TABLE [dbo].[h1500] (
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_h1500_rowguid] DEFAULT (newid()) NULL,
    [deleted]            BIT              CONSTRAINT [DF_h1500_deleted_2__20] DEFAULT ((0)) NOT NULL,
    [account]            VARCHAR (15)     NOT NULL,
    [ins_abc]            VARCHAR (1)      CONSTRAINT [DF_h1500_ins_abc_1__28] DEFAULT ('A') NOT NULL,
    [pat_name]           VARCHAR (40)     NULL,
    [fin_code]           VARCHAR (1)      NULL,
    [claimsnet_payer_id] VARCHAR (50)     NULL,
    [trans_date]         DATETIME         NULL,
    [run_date]           DATETIME         CONSTRAINT [DF_h1500_run_date_4__20] DEFAULT (getdate()) NULL,
    [printed]            BIT              CONSTRAINT [DF_h1500_printed_3__20] DEFAULT ((0)) NOT NULL,
    [run_user]           VARCHAR (20)     CONSTRAINT [DF_h1500_run_user_5__20] DEFAULT (suser_sname()) NOT NULL,
    [batch]              NUMERIC (10)     CONSTRAINT [DF_h1500_batch_1__20] DEFAULT ((-1)) NOT NULL,
    [date_sent]          DATETIME         NULL,
    [sent_user]          VARCHAR (20)     NULL,
    [ebill_status]       VARCHAR (5)      NULL,
    [ebill_batch]        NUMERIC (10)     NULL,
    [text]               VARCHAR (MAX)    NULL,
    [cold_feed]          DATETIME         NULL,
    [mod_date]           DATETIME         CONSTRAINT [DF_h1500_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]           VARCHAR (50)     CONSTRAINT [DF_h1500_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]            VARCHAR (50)     CONSTRAINT [DF_h1500_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]           VARCHAR (50)     CONSTRAINT [DF_h1500_mod_host] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_h1500] PRIMARY KEY CLUSTERED ([account] ASC, [ins_abc] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ebill_batch]
    ON [dbo].[h1500]([ebill_batch] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ebill_status_idx]
    ON [dbo].[h1500]([ebill_status] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [fin_code_idx]
    ON [dbo].[h1500]([fin_code] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [pat_name_idx]
    ON [dbo].[h1500]([pat_name] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [run_date_idx]
    ON [dbo].[h1500]([run_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_rowguid INCLUDE account, ins_abc]
    ON [dbo].[h1500]([rowguid] ASC)
    INCLUDE([account], [ins_abc]) WITH (FILLFACTOR = 90);


GO
CREATE UNIQUE NONCLUSTERED INDEX [PK_account, ins_abc]
    ON [dbo].[h1500]([account] ASC, [ins_abc] ASC) WITH (FILLFACTOR = 90);


GO
/*
	05/23/2007 Trigger written by Rick and David
	Trigger should provide audit capabilites on the h1500 table in MCL02's MCLTEST database.
	Note: h1500's field named TEXT which is a text data type cannot be updated by SQL 8.0's triggers
*/
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_h1500] ON [dbo].[h1500] 
AFTER UPDATE
AS
DECLARE @rowguid uniqueidentifier
set @rowguid = NULL
--declare @str varchar(50)
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

/* update the audit_h1500 fields before changing the h1500's mod_XXX fields */
	select @rowguid = h.h1500_rowguid
	from	 audit_h1500 h inner join inserted i on h.h1500_rowguid = i.rowguid
	
	if (@rowguid is NULL)
	Begin

	Insert into audit_h1500
	( h1500_rowguid, deleted, account, ins_abc, pat_name,
		fin_code, claimsnet_payer_id, trans_date, run_date, printed, run_user,
		batch, date_sent, sent_user, ebill_status, ebill_batch,
		[text], cold_feed, mod_date, mod_user, mod_prg, mod_host)
	select
	del.rowguid,del.deleted,del.account,del.ins_abc,del.pat_name,
		del.fin_code, del.claimsnet_payer_id, del.trans_date, del.run_date, del.printed, del.run_user,
		del.batch, del.date_sent, del.sent_user, del.ebill_status, del.ebill_batch, 
		del.[text], del.cold_feed, del.mod_date, del.mod_user, del.mod_prg, del.mod_host

	from deleted del
end


	Insert into audit_h1500
	( h1500_rowguid, deleted, account, ins_abc, pat_name,
		fin_code, claimsnet_payer_id, trans_date, run_date, printed, run_user,
		batch, date_sent, sent_user, ebill_status, ebill_batch, 
		[text], cold_feed, mod_date, mod_user, mod_prg, mod_host)
	select
	ins.rowguid, ins.deleted, ins.account, ins.ins_abc, ins.pat_name,
	ins.fin_code, ins.claimsnet_payer_id, ins.trans_date, ins.run_date, ins.printed, ins.run_user,
	ins.batch, ins.date_sent, ins.sent_user, ins.ebill_status, ins.ebill_batch, 
	ins.[text], ins.cold_feed, getdate(), suser_sname(), app_name(), host_name()

	from inserted ins


END

GO
-- =============================================
-- Author:		David and Rick
-- Create date: 05/23/2007
-- Description:	Moves deleted records to the Audit table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_DELETED_h1500] 
   ON  [dbo].[h1500] 
   AFTER DELETE
AS
DECLARE @rowguid uniqueidentifier
set @rowguid = NULL

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

/* update the audit_h1500 fields before changing the h1500's mod_XXX fields */
	select @rowguid = h.h1500_rowguid
	from	 audit_h1500 h inner join deleted d on h.h1500_rowguid = d.rowguid
	
-- deleted without being modified. So Add original record to audit table.
	if (@rowguid is NULL)
	Begin


	Insert into audit_h1500
	( h1500_rowguid, deleted, account, ins_abc, pat_name,
		fin_code, claimsnet_payer_id, trans_date, run_date, printed, run_user,
		batch, date_sent, sent_user, ebill_status, ebill_batch,
		[text], cold_feed, mod_date, mod_user, mod_prg, mod_host)
	select
	del.rowguid,del.deleted,del.account,del.ins_abc,del.pat_name,
	del.fin_code,claimsnet_payer_id, del.trans_date, del.run_date, del.printed, del.run_user,
		del.batch,del.date_sent,del.sent_user,del.ebill_status,
		del.ebill_batch, del.[text], del.cold_feed, del.mod_date, 
		del.mod_user, del.mod_prg, del.mod_host

	from deleted del
end

-- deleted and has been modified
-- old record with mod fields from delete
	Insert into audit_h1500
	( h1500_rowguid,deleted, account, ins_abc, pat_name,
		fin_code, claimsnet_payer_id, trans_date, run_date, printed, run_user,
		batch, date_sent, sent_user, ebill_status, ebill_batch,
		[text], cold_feed, mod_date, mod_user, mod_prg, mod_host)
	select
	del.rowguid, del.deleted, del.account, del.ins_abc, del.pat_name,
		del.fin_code, claimsnet_payer_id, del.trans_date, del.run_date, del.printed, del.run_user,
		del.batch, del.date_sent, del.sent_user, del.ebill_status, del.ebill_batch,
		del.[text], del.cold_feed,
		getdate(),
		suser_sname(),
		'D~'+app_name(),
		host_name()

	from deleted del


END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'07/09/2008 rgc/wdk added for control of ST/SE segments in EBILL.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'h1500', @level2type = N'COLUMN', @level2name = N'claimsnet_payer_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'R15_2 uses this to select records', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'h1500', @level2type = N'COLUMN', @level2name = N'run_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'04/15/2008 wdk/rgc Coverted from 8000 to MAX. 03/21/2008 wdk converted from text', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'h1500', @level2type = N'COLUMN', @level2name = N'text';

