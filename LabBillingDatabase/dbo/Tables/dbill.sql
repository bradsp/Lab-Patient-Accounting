CREATE TABLE [dbo].[dbill] (
    [rowguid]    UNIQUEIDENTIFIER CONSTRAINT [DF_dbill_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]    BIT              CONSTRAINT [DF_dbill_deleted_2__12] DEFAULT ((0)) NOT NULL,
    [account]    VARCHAR (15)     NOT NULL,
    [pat_name]   VARCHAR (40)     NULL,
    [fin_code]   VARCHAR (1)      NULL,
    [trans_date] DATETIME         NULL,
    [run_date]   DATETIME         CONSTRAINT [DF_dbill_run_date_1__11] DEFAULT (getdate()) NULL,
    [printed]    BIT              CONSTRAINT [DF_dbill_printed_3__12] DEFAULT ((0)) NOT NULL,
    [run_user]   VARCHAR (20)     CONSTRAINT [DF_dbill_run_user_2__11] DEFAULT (suser_sname()) NULL,
    [batch]      NUMERIC (10)     CONSTRAINT [DF_dbill_batch_1__17] DEFAULT ((-1)) NULL,
    [text]       VARCHAR (8000)   NULL,
    [mod_user]   VARCHAR (50)     CONSTRAINT [DF_dbill_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]    VARCHAR (50)     CONSTRAINT [DF_dbill_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_date]   VARCHAR (50)     CONSTRAINT [DF_dbill_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_host]   VARCHAR (50)     CONSTRAINT [DF_dbill_mod_host] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_dbill] PRIMARY KEY CLUSTERED ([account] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [batch_idx]
    ON [dbo].[dbill]([batch] ASC, [run_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_dbill_fin_code_name]
    ON [dbo].[dbill]([fin_code] ASC, [pat_name] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_dbill_pat_name]
    ON [dbo].[dbill]([pat_name] ASC) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 02/20/2008
-- Description:	Delete trigger for the DBill table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_DELETE_DBILL] 
   ON  dbo.dbill 
   AFTER DELETE
AS 

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here

	Insert into audit_dbill
	(dbill_rowguid, deleted, account, pat_name, fin_code, 
		trans_date, run_date, printed, run_user, batch, 
		[text], mod_user, mod_prg, mod_date, mod_host)
	select
	del.rowguid, del.deleted, del.account, del.pat_name, del.fin_code, 
		del.trans_date, del.run_date, del.printed, del.run_user, del.batch, 
		del.[text], 
		suser_sname(),
		'D~'+app_name(), -- 'D~' in the audit_XXX indicats the record was REMOVED from the XXX table.
		GetDate(),
		host_name()

	from deleted del
	   
		
END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 02/20/2008
-- Description:	Update trigger for the dbill table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_DBILL] 
   ON  dbo.dbill 
   AFTER UPDATE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = NULL
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	/* update the audit_dbill fields */
	select @rowguid = d.dbill_rowguid
	from	 audit_dbill d inner join inserted i on d.dbill_rowguid = i.rowguid
	
	if (@rowguid is NULL)
	Begin
	Insert into audit_dbill
	(dbill_rowguid, deleted, account, pat_name, fin_code, 
		trans_date, run_date, printed, run_user, batch, 
		[text], mod_user, mod_prg, mod_date, mod_host)
	select
	del.rowguid, del.deleted, del.account, del.pat_name, del.fin_code, 
		del.trans_date, del.run_date, del.printed, del.run_user, del.batch, 
		del.[text], del.mod_user, del.mod_prg, del.mod_date, del.mod_host

	from deleted del
end
	Insert into audit_dbill
	(dbill_rowguid, deleted, account, pat_name, fin_code, 
			trans_date, run_date, printed, run_user, batch, 
			[text], mod_user, mod_prg, mod_date, mod_host)
	select
	ins.rowguid, ins.deleted, ins.account, ins.pat_name, ins.fin_code, 
			ins.trans_date, ins.run_date, ins.printed, ins.run_user, ins.batch, 
			ins.[text], suser_sname(), app_name(), GetDate(), host_name()
		from inserted ins

END
