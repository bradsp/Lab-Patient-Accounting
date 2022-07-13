CREATE TABLE [dbo].[cli_dis] (
    [rowguid]    UNIQUEIDENTIFIER CONSTRAINT [DF_cli_dis_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]    BIT              CONSTRAINT [DF_cli_dis_deleted_1__23] DEFAULT ((0)) NOT NULL,
    [cli_mnem]   VARCHAR (10)     NOT NULL,
    [start_cdm]  VARCHAR (7)      NOT NULL,
    [end_cdm]    VARCHAR (7)      NOT NULL,
    [percent_ds] REAL             NULL,
    [price]      NUMERIC (18, 2)  NULL,
    [mod_date]   DATETIME         CONSTRAINT [DF_cli_dis_mod_date_2__23] DEFAULT (getdate()) NULL,
    [mod_user]   VARCHAR (50)     CONSTRAINT [DF_cli_dis_mod_user_4__23] DEFAULT (right(suser_sname(),(50))) NULL,
    [mod_prg]    VARCHAR (50)     CONSTRAINT [DF_cli_dis_mod_prg_3__23] DEFAULT (right(app_name(),(50))) NULL,
    [mod_host]   VARCHAR (50)     CONSTRAINT [DF_cli_dis_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [uri]        NUMERIC (10)     IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_cli_dis_new_1__23] PRIMARY KEY CLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_cli_dis_deleted_cli]
    ON [dbo].[cli_dis]([deleted] ASC, [cli_mnem] ASC)
    INCLUDE([start_cdm], [end_cdm], [percent_ds], [price]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_cli_dis_1]
    ON [dbo].[cli_dis]([cli_mnem] ASC, [start_cdm] ASC, [end_cdm] ASC) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 04/29/2008
-- Description:	Track changes to the cli_dis table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_UPDATE_cli_dis] 
   ON  [dbo].[cli_dis] 
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	
insert into audit_cli_dis
	( audit_rowguid, deleted, cli_mnem, start_cdm, end_cdm, 
		percent_ds,price, mod_date, mod_user, mod_prg, mod_host)
	select  
	i.rowguid, i.deleted, i.cli_mnem, i.start_cdm, i.end_cdm, 
	i.percent_ds, i.price, getdate(), suser_sname(), app_name(), host_name()
	from inserted i

END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 04/29/2008
-- Description:	Track changes in the cli_dis table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_INSERT_cli_dis] 
   ON  [dbo].[cli_dis] 
   AFTER INSERT
AS 
DECLARE @mDate datetime
set @mDate = null
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here

	select @mDate = i.mod_date
	from inserted i
	if (@mDate is null)
	begin
	select @mDate = getdate()
	end
 
	insert into audit_cli_dis
	( audit_rowguid, deleted, cli_mnem, start_cdm, end_cdm, 
		percent_ds, price, mod_date, mod_user, mod_prg, mod_host)
	select  
	i.rowguid, i.deleted, i.cli_mnem, i.start_cdm, i.end_cdm, 
	i.percent_ds, i.price, @mDate, 
	i.mod_user, i.mod_prg, i.mod_host
	from inserted i


END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 04/29/2008
-- Description:	Track deletes from the cli_dis table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_DELETE_cli_dis] 
   ON  [dbo].[cli_dis] 
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	insert into audit_cli_dis
	( audit_rowguid, deleted, cli_mnem, start_cdm, end_cdm, 
		percent_ds, price, mod_date, mod_user, mod_prg, mod_host)
	select  
	d.rowguid, d.deleted, d.cli_mnem, d.start_cdm, d.end_cdm, 
	d.percent_ds, d.price, getdate(), suser_sname(), 'D~'+app_name(), host_name()
	from deleted d

END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20110128 added for setting price vs percent discount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cli_dis', @level2type = N'COLUMN', @level2name = N'price';

