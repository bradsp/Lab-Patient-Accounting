CREATE TABLE [dbo].[phy] (
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_phy_rowguid_1] DEFAULT (newid()) NOT NULL,
    [deleted]     BIT              CONSTRAINT [DF_phy_deleted_1__12] DEFAULT ((0)) NOT NULL,
    [upin]        VARCHAR (6)      NULL,
    [ub92_upin]   VARCHAR (6)      CONSTRAINT [DF_phy_ub92_upin] DEFAULT ('OTH000') NULL,
    [tnh_num]     VARCHAR (15)     NULL,
    [billing_npi] VARCHAR (15)     NULL,
    [pc_code]     VARCHAR (2)      NULL,
    [cl_mnem]     VARCHAR (15)     NULL,
    [last_name]   VARCHAR (30)     NULL,
    [first_name]  VARCHAR (30)     NULL,
    [mid_init]    VARCHAR (30)     NULL,
    [group1]      VARCHAR (35)     NULL,
    [addr_1]      VARCHAR (40)     NULL,
    [addr_2]      VARCHAR (40)     NULL,
    [city]        VARCHAR (30)     NULL,
    [state]       VARCHAR (2)      NULL,
    [zip]         VARCHAR (10)     NULL,
    [phone]       VARCHAR (40)     NULL,
    [reserved]    VARCHAR (1)      NULL,
    [num_labels]  INT              CONSTRAINT [DF_phy_num_labels] DEFAULT ((0)) NULL,
    [mod_date]    DATETIME         CONSTRAINT [DF_phy_mod_date_2__12] DEFAULT (getdate()) NULL,
    [mod_user]    VARCHAR (40)     CONSTRAINT [DF_phy_mod_user_4__12] DEFAULT (right(suser_sname(),(40))) NULL,
    [mod_prg]     VARCHAR (40)     CONSTRAINT [DF_phy_mod_prg_3__12] DEFAULT (right(app_name(),(40))) NULL,
    [uri]         NUMERIC (15)     IDENTITY (100, 1) NOT NULL,
    [mt_mnem]     VARCHAR (15)     NULL,
    [credentials] VARCHAR (50)     NULL,
    [ov_code]     VARCHAR (50)     NULL,
    [docnbr]      VARCHAR (5)      NULL,
    CONSTRAINT [PK_phy_5__12] PRIMARY KEY CLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_phy_deleted_tnh_num]
    ON [dbo].[phy]([tnh_num] ASC, [deleted] ASC)
    INCLUDE([billing_npi], [last_name], [first_name], [mid_init], [mt_mnem]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_phy_upin]
    ON [dbo].[phy]([upin] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_phy_billing_npi]
    ON [dbo].[phy]([billing_npi] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [name_idx]
    ON [dbo].[phy]([last_name] ASC, [first_name] ASC, [mid_init] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_phy_mt_mnem]
    ON [dbo].[phy]([mt_mnem] ASC) WITH (FILLFACTOR = 90);


GO
/****** Object:  Trigger dbo.tu_phy    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_phy] ON [dbo].[phy] 
FOR UPDATE 
AS
UPDATE phy
SET phy.mod_user = RIGHT(SUSER_SNAME(),40), phy.mod_date = getdate(), phy.mod_prg = left(app_name(),40)
FROM inserted,phy
WHERE inserted.uri = phy.uri

GO
-- =============================================
-- Author:		<Author,,David Kely>
-- Create date: <Create Date,,20170531>
-- Description:	<Description,,post the changes to an audit table>
-- =============================================
--CREATE TRIGGER <Schema_Name, sysname, Schema_Name>.<Trigger_Name, sysname, Trigger_Name> 
--   ON  <Schema_Name, sysname, Schema_Name>.<Table_Name, sysname, Table_Name> 
--   AFTER <Data_Modification_Statements, , INSERT,DELETE,UPDATE>
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_PHY]
	ON [dbo].[phy]
	after update
AS 
declare @rowguid uniqueidentifier
set @rowguid = null
declare @mod_date datetime
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
    -- Insert statements for trigger here
	SET NOCOUNT ON;
	select @rowguid = ap.rowguid
	from audit_phy ap inner join inserted i on ap.rowguid = i.rowguid

	if (@rowguid is null)
		insert into audit_phy
		(rowguid,deleted,upin,ub92_upin,tnh_num,billing_npi,pc_code,cl_mnem,last_name,first_name,mid_init,
		group1,addr_1,addr_2,city,state,zip,phone,reserved,num_labels,mod_date,mod_user,mod_prg,
		mt_mnem,credentials,ov_code,docnbr)
		select
		d.rowguid,d.deleted,d.upin,d.ub92_upin,d.tnh_num,d.billing_npi,d.pc_code,d.cl_mnem,d.last_name,d.first_name,d.mid_init,
		d.group1,d.addr_1,d.addr_2,d.city,d.state,d.zip,d.phone,d.reserved,d.num_labels,d.mod_date,d.mod_user,d.mod_prg,
		d.mt_mnem,d.credentials,d.ov_code,d.docnbr
		from deleted d

	-- Once the original record is handled use the inserted record to
	-- track the mod_XXXX fields and store in the audit_XXX table
	insert into audit_phy
		(rowguid,deleted,upin,ub92_upin,tnh_num,billing_npi,pc_code,cl_mnem,last_name,first_name,mid_init,
		group1,addr_1,addr_2,city,state,zip,phone,reserved,num_labels,mod_date,mod_user,mod_prg,
		mt_mnem,credentials,ov_code,docnbr)
		select
		i.rowguid,i.deleted,i.upin,i.ub92_upin,i.tnh_num,i.billing_npi,i.pc_code,i.cl_mnem,i.last_name,i.first_name,i.mid_init,
		i.group1,i.addr_1,i.addr_2,i.city,i.state,i.zip,i.phone,i.reserved,i.num_labels,
		getdate(), right(suser_sname(),40), right(app_name(),40),
		i.mt_mnem,i.credentials,i.ov_code,i.docnbr
		from inserted i
END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Field used by the UB92 call to Billing_form::UB_LoadData in MCL60.lib', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'phy', @level2type = N'COLUMN', @level2name = N'ub92_upin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'now used for NPI number 12/13/2006', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'phy', @level2type = N'COLUMN', @level2name = N'tnh_num';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'20121119 added for Medicare and Bluecare changes for billing physician not PA or FNP numbers.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'phy', @level2type = N'COLUMN', @level2name = N'billing_npi';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140409 to add credentials for billing purposes on new forms', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'phy', @level2type = N'COLUMN', @level2name = N'credentials';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20150401 added for translations from Other Vendors phy_id schemas', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'phy', @level2type = N'COLUMN', @level2name = N'ov_code';

