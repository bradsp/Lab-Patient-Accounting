CREATE TABLE [dbo].[ub] (
    [rowguid]            UNIQUEIDENTIFIER CONSTRAINT [DF_ub_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]            BIT              CONSTRAINT [DF_ub_deleted_2__20] DEFAULT ((0)) NOT NULL,
    [account]            VARCHAR (15)     NOT NULL,
    [ins_abc]            VARCHAR (1)      CONSTRAINT [DF_ub_ins_abc_1__28] DEFAULT ('A') NOT NULL,
    [run_date]           DATETIME         CONSTRAINT [DF_ub_run_date_4__20] DEFAULT (getdate()) NULL,
    [printed]            BIT              CONSTRAINT [DF_ub_printed_3__20] DEFAULT ((0)) NOT NULL,
    [run_user]           VARCHAR (30)     CONSTRAINT [DF_ub_run_user_5__20] DEFAULT (suser_sname()) NULL,
    [fin_code]           VARCHAR (1)      NULL,
    [trans_date]         DATETIME         NULL,
    [pat_name]           VARCHAR (40)     NULL,
    [claimsnet_payer_id] VARCHAR (50)     NULL,
    [ebill_status]       VARCHAR (5)      NULL,
    [batch]              NUMERIC (10)     CONSTRAINT [DF_ub_batch_1__20] DEFAULT ((-1)) NULL,
    [text]               VARCHAR (8000)   NULL,
    [edited_ub]          VARCHAR (8000)   NULL,
    [cold_feed]          DATETIME         NULL,
    [mod_date]           DATETIME         CONSTRAINT [DF_ub_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]           VARCHAR (50)     CONSTRAINT [DF_ub_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]            VARCHAR (50)     CONSTRAINT [DF_ub_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]           VARCHAR (50)     CONSTRAINT [DF_ub_mod_host] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_ub] PRIMARY KEY NONCLUSTERED ([rowguid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_account, ins_abc]
    ON [dbo].[ub]([account] ASC, [ins_abc] ASC) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/01/2007
-- Description:	Provides auditing to the UB table
-- =============================================
create TRIGGER [dbo].[TRIGGER_AUDIT_DELETE_ub] 
   ON  [dbo].[ub] 
   AFTER DELETE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- if the rowguid is not in the audit table the record has not been 
	-- modiifed so copy the original to the audit table before inserting the 
	-- deleted record with updates to the audit fields.
	select @rowguid = u.ub_rowguid
	from	 audit_ub u inner join inserted i on u.ub_rowguid = i.rowguid

	if (@rowguid is NULL)
	begin

		-- Insert statements for trigger here
		insert into audit_ub
		(ub_rowguid, deleted, account, ins_abc, run_date, 
			printed, run_user, fin_code, trans_date, [text],
			edited_ub, pat_name, 
			claimsnet_payer_id, ebill_status, batch, cold_feed, mod_date, 
			mod_user, mod_prg, mod_host)
		select
		  del.rowguid, del.deleted, del.account, del.ins_abc, del.run_date,
			del.printed, del.run_user, del.fin_code, del.trans_date, del.[text],
			del.edited_ub, del.pat_name,	
			del.claimsnet_payer_id, del.ebill_status,del.batch, del.cold_feed, del.mod_date,
			del.mod_user, del.mod_prg, del.mod_host
		from deleted del
	end



    -- Insert statements for trigger here
	insert into audit_ub
	(ub_rowguid, deleted, account, ins_abc, run_date, 
		printed, run_user, fin_code, trans_date, [text],
		edited_ub, pat_name, 
		claimsnet_payer_id, ebill_status, batch, cold_feed, mod_date, 
		mod_user, mod_prg, mod_host)
	select
	  del.rowguid, del.deleted, del.account, del.ins_abc, del.run_date,
		del.printed, del.run_user, del.fin_code, del.trans_date, del.[text],
		 del.edited_ub, del.pat_name, 
		del.claimsnet_payer_id, del.ebill_status,del.batch, del.cold_feed,	getdate(),
		suser_sname(), 'D~'+app_name(),  host_name()
		-- 'D~' in the audit_ub indicats the record was REMOVED from the ub table.
	from deleted del

END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/01/2007
-- Description:	Track changes in the ub table
-- Note the UB table contains a text field 
--		you cannot use text, ntext, 
--		or image columns in the 'inserted' and 'deleted' tables.

-- =============================================
create TRIGGER [dbo].[TRIGGER_AUDIT_ub] 
   ON  [dbo].[ub] 
   AFTER UPDATE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select @rowguid = u.ub_rowguid
	from	 audit_ub u inner join inserted i on u.ub_rowguid = i.rowguid

	if (@rowguid is NULL)
	begin

		-- Insert statements for trigger here
		insert into audit_ub
		(ub_rowguid, deleted, account, ins_abc, run_date, 
			printed, run_user, fin_code, trans_date, [text],
			edited_ub, pat_name, 
			claimsnet_payer_id, ebill_status, 
			batch, cold_feed, mod_date, 
			mod_user, mod_prg, mod_host)
		select
		  del.rowguid, del.deleted, del.account, del.ins_abc, del.run_date,
			del.printed, del.run_user, del.fin_code, del.trans_date, del.[text],
			del.edited_ub, del.pat_name,	
			del.claimsnet_payer_id, del.ebill_status,
			del.batch, del.cold_feed, del.mod_date,
			del.mod_user, del.mod_prg, del.mod_host
		from deleted del
	end

	insert into audit_ub
	(ub_rowguid, deleted, account, ins_abc, run_date,
		printed, run_user, fin_code, trans_date, [text],
		edited_ub, pat_name, 
		claimsnet_payer_id, ebill_status, batch, cold_feed, mod_date, 
		mod_user, mod_prg, mod_host)
	select
	  ins.rowguid, ins.deleted, ins.account, ins.ins_abc, ins.run_date,
		ins.printed, ins.run_user, ins.fin_code, ins.trans_date, ins.[text],
		ins.edited_ub, ins.pat_name, 
		ins.claimsnet_payer_id, ins.ebill_status, ins.batch, ins.cold_feed, getdate(),
		 suser_sname(), app_name(), host_name()
	from deleted ins
END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'11/13/2008 wdk added for control of ST/SE segments in EBILL.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ub', @level2type = N'COLUMN', @level2name = N'claimsnet_payer_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'11/13/2008 wdk added for billing ub''s via claimsnet', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ub', @level2type = N'COLUMN', @level2name = N'ebill_status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'converted from text 03/13/2008 wdk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ub', @level2type = N'COLUMN', @level2name = N'text';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'added on 03/13/2008 for tracking edited ubs for printing.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ub', @level2type = N'COLUMN', @level2name = N'edited_ub';

