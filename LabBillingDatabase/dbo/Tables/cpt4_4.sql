CREATE TABLE [dbo].[cpt4_4] (
    [rowguid]   UNIQUEIDENTIFIER CONSTRAINT [DF_cpt4_4_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]   BIT              CONSTRAINT [DF_cpt4_4_deleted_3__23] DEFAULT ((0)) NOT NULL,
    [cdm]       VARCHAR (7)      NOT NULL,
    [link]      INT              NOT NULL,
    [cpt4]      VARCHAR (5)      NULL,
    [descript]  VARCHAR (50)     NULL,
    [mprice]    MONEY            CONSTRAINT [DF_cpt4_4_mprice_8__23] DEFAULT ((0)) NULL,
    [cprice]    MONEY            CONSTRAINT [DF_cpt4_4_cprice_2__23] DEFAULT ((0)) NULL,
    [zprice]    MONEY            CONSTRAINT [DF_cpt4_4_zprice_9__23] DEFAULT ((0)) NULL,
    [rev_code]  VARCHAR (4)      NULL,
    [type]      VARCHAR (4)      NULL,
    [modi]      VARCHAR (2)      NULL,
    [billcode]  VARCHAR (7)      NULL,
    [mod_date]  DATETIME         CONSTRAINT [DF_cpt4_4_mod_date_4__23] DEFAULT (getdate()) NULL,
    [mod_user]  VARCHAR (50)     CONSTRAINT [DF_cpt4_4_mod_user_7__23] DEFAULT (right(suser_sname(),(50))) NULL,
    [mod_prg]   VARCHAR (50)     CONSTRAINT [DF_cpt4_4_mod_prg_6__23] DEFAULT (right(app_name(),(50))) NULL,
    [mod_host]  VARCHAR (50)     CONSTRAINT [DF_cpt4_4_mod_host_5__23] DEFAULT (right(host_name(),(50))) NULL,
    [cost]      DECIMAL (18, 2)  NULL,
    [code_flag] VARCHAR (50)     NULL,
    CONSTRAINT [PK_cpt4_4_1__23] PRIMARY KEY CLUSTERED ([cdm] ASC, [link] ASC) WITH (FILLFACTOR = 90)
);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 03/19/2012
-- Description:	Delete trigger for the cpt4_4 table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_DELETE_cpt4_4] 
   ON  [dbo].[cpt4_4] 
   AFTER DELETE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
    -- Insert statements for trigger here
	select @rowguid = c.audit_rowguid
	from	 audit_cpt4_4 c inner join deleted d on c.audit_rowguid = d.rowguid
		
	if (@rowguid is NULL) /* if this is null no record exists in the audit table 
								so copy the original record to the audit table from the deleted record*/
	BEGIN
	
		insert into audit_cpt4_4
		(audit_rowguid, deleted, cdm, link, cpt4, 
			descript, mprice, cprice, zprice, rev_code,
			[type], modi, billcode, 
			mod_date, mod_user, mod_prg, mod_host, cost)
		select
		d.rowguid, d.deleted, d.cdm, d.link, d.cpt4,
			d.descript, d.mprice, d.cprice, d.zprice, d.rev_code,
			d.[type], d.modi, d.billcode, 
			d.mod_date, d.mod_user, d.mod_prg, d.mod_host, d.cost
		from deleted d

	END
-- Record as it exists in the table
--	BEGIN
--	Insert into audit_cpt4_4
--	(audit_rowguid, deleted, cdm, link, cpt4_4, 
--	 descript, mprice, cprice, zprice, rev_code, 
--	 [type], modi, billcode, 
--	 mod_date, mod_user, mod_prg, mod_host)
--	select 
--	del.rowguid, del.deleted, del.cdm, del.link, del.cpt4_4, 
--	 del.descript, del.mprice, del.cprice, del.zprice, del.rev_code, 
--	 del.[type], del.modi, del.billcode, 
--	 del.mod_date, del.mod_user, del.mod_prg, del.mod_host
--	from deleted del
--	END
-- Record modified with the mod_XXX data changed
	BEGIN
	Insert into audit_cpt4_4
	(audit_rowguid, deleted, cdm, link, cpt4, 
	 descript, mprice, cprice, zprice, rev_code, 
	 [type], modi, billcode, 
	 mod_date, mod_user, mod_prg, mod_host, cost)
	select 
	del.rowguid, del.deleted, del.cdm, del.link, del.cpt4, 
	 del.descript, del.mprice, del.cprice, del.zprice, del.rev_code, 
	 del.[type], del.modi, del.billcode, 
	 getdate(), suser_sname(), 'D~'+app_name(), host_name(), del.cost
	from deleted del
	END

END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 03/19/2012
-- Description:	Insert trigger
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_INSERT_cpt4_4] 
   ON  [dbo].[cpt4_4] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	insert into audit_cpt4_4
	(audit_rowguid, deleted, cdm, link, cpt4, 
		descript, mprice, cprice, zprice, rev_code,
		[type], modi, billcode, 
		mod_date, mod_user, mod_prg, mod_host,cost)
	select
	i.rowguid, i.deleted, i.cdm, i.link, i.cpt4,
		i.descript, i.mprice, i.cprice, i.zprice, i.rev_code,
		i.[type], i.modi, i.billcode, 
		i.mod_date, i.mod_user, i.mod_prg, i.mod_host,i.cost
	from inserted i


END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 03/19/2012
-- Description:	Update Trigger
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_UPDATE_cpt4_4] 
   ON  [dbo].[cpt4_4] 
   FOR UPDATE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select @rowguid = c.audit_rowguid
	from	 audit_cpt4_4 c inner join deleted d on c.audit_rowguid = d.rowguid
		
	if (@rowguid is NULL) /* if this is null no record exists in the audit table 
								so copy the original record to the audit table from the deleted record*/
	BEGIN
	
		insert into audit_cpt4_4
		(audit_rowguid, deleted, cdm, link, cpt4, 
			descript, mprice, cprice, zprice, rev_code,
			[type], modi, billcode, 
			mod_date, mod_user, mod_prg, mod_host,cost)
		select
		d.rowguid, d.deleted, d.cdm, d.link, d.cpt4,
			d.descript, d.mprice, d.cprice, d.zprice, d.rev_code,
			d.[type], d.modi, d.billcode, 
			d.mod_date, d.mod_user, d.mod_prg, d.mod_host, d.cost
		from deleted d

	END

	insert into audit_cpt4_4
	(audit_rowguid, deleted, cdm, link, cpt4, 
		descript, mprice, cprice, zprice, rev_code,
		[type], modi, billcode, 
		mod_date, mod_user, mod_prg, mod_host, cost)
	select
	i.rowguid, i.deleted, i.cdm, i.link, i.cpt4,
		i.descript, i.mprice, i.cprice, i.zprice, i.rev_code,
		i.[type], i.modi, i.billcode, 
		getdate(), right(suser_sname(),50), right('U~'+app_name(),50), right(host_name(),50), i.cost
	from inserted i


END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20120319 Added for capturing the Test Cost', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cpt4_4', @level2type = N'COLUMN', @level2name = N'cost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140131 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cpt4_4', @level2type = N'COLUMN', @level2name = N'code_flag';

