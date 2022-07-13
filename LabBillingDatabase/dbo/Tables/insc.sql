CREATE TABLE [dbo].[insc] (
    [rowguid]                  UNIQUEIDENTIFIER CONSTRAINT [DF_insc_rowguid] DEFAULT (newid()) NOT NULL,
    [deleted]                  BIT              CONSTRAINT [DF_insc_deleted_2__12] DEFAULT ((0)) NOT NULL,
    [code]                     VARCHAR (10)     NOT NULL,
    [name]                     VARCHAR (45)     NULL,
    [addr1]                    VARCHAR (30)     NULL,
    [addr2]                    VARCHAR (30)     NULL,
    [citystzip]                VARCHAR (30)     NULL,
    [provider_no_qualifier]    VARCHAR (3)      NULL,
    [provider_no]              VARCHAR (20)     NULL,
    [payer_no]                 VARCHAR (50)     NULL,
    [claimsnet_payer_id]       VARCHAR (10)     CONSTRAINT [DF_insc_claimsnet_payer_id] DEFAULT ('EXCLUDED') NULL,
    [bill_form]                VARCHAR (5)      NULL,
    [num_labels]               INT              CONSTRAINT [DF_insc_num_labels_1__11] DEFAULT ((0)) NULL,
    [fin_code]                 VARCHAR (10)     NULL,
    [comment]                  VARCHAR (250)    NULL,
    [mod_date]                 DATETIME         CONSTRAINT [DF_insc_mod_date_3__12] DEFAULT (getdate()) NULL,
    [mod_user]                 VARCHAR (50)     CONSTRAINT [DF_insc_mod_user_5__12] DEFAULT (suser_sname()) NULL,
    [mod_prg]                  VARCHAR (50)     CONSTRAINT [DF_insc_mod_prg_4__12] DEFAULT (app_name()) NULL,
    [mod_host]                 VARCHAR (50)     CONSTRAINT [DF_insc_mod_host] DEFAULT (host_name()) NULL,
    [is_mc_hmo]                BIT              NULL,
    [allow_outpatient_billing] BIT              CONSTRAINT [DF__insc__allow_outp__21C1BDAC] DEFAULT ((1)) NOT NULL,
    [payor_code]               VARCHAR (8000)   NULL,
    [fin_class]                AS               ([fin_code]),
    [bill_as_jmcgh]            BIT              NULL,
    CONSTRAINT [PK_insc_1__12] PRIMARY KEY CLUSTERED ([code] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [name_idx]
    ON [dbo].[insc]([name] ASC) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/11/2008
-- Description:	Delete Trigger for INSC table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_DELETE_INSC] 
   ON  dbo.insc 
   AFTER DELETE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select @rowguid = a.audit_rowguid
	from	 audit_insc a inner join deleted d on a.audit_rowguid = d.rowguid
		
	if (@rowguid is NULL) /* if this is null no record exists in the audit table 
								so copy the original record to the audit table from the deleted record*/
	BEGIN
	insert into audit_insc
	(audit_rowguid, deleted, code, [name], addr1, 
	 addr2, citystzip, provider_no, payer_no, 
	 claimsnet_payer_id, bill_form, num_labels,
	 fin_code, fin_class, comment, mod_date, mod_user, 
     mod_prg, mod_host)
	select
	 d.rowguid, d.deleted, d.code, d.[name], d.addr1, 
	 d.addr2, d.citystzip, d.provider_no, d.payer_no, 
	d.claimsnet_payer_id, d.bill_form, d.num_labels,
	 d.fin_code, d.fin_class, d.comment, d.mod_date, d.mod_user,
     d.mod_prg, d.mod_host
	from deleted d

	END

	insert into audit_insc
	(audit_rowguid, deleted, code, [name], addr1, 
	 addr2, citystzip, provider_no, payer_no, 
	 claimsnet_payer_id, bill_form, num_labels,
	 fin_code, fin_class, comment, mod_date, mod_user,
     mod_prg, mod_host)
	select
	 d.rowguid, d.deleted, d.code, d.[name], d.addr1, 
	 d.addr2, d.citystzip, d.provider_no, d.payer_no, 
	 d.claimsnet_payer_id, d.bill_form, d.num_labels,
	 d.fin_code, d.fin_class, d.comment, getdate(), suser_sname(),
     'D~ '+app_name(), host_name()
	from deleted d

END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/11/2008
-- Description:	Insert Trigger for INSC table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_INSERT_INSC] 
   ON  dbo.insc 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	insert into audit_insc
	(audit_rowguid, deleted, code, [name], addr1, 
	 addr2, citystzip, provider_no, payer_no,
	 claimsnet_payer_id, bill_form, num_labels,
	 fin_code, fin_class, comment, mod_date, mod_user,
     mod_prg, mod_host)
	select
	 i.rowguid, i.deleted, i.code, i.[name], i.addr1, 
	 i.addr2, i.citystzip, i.provider_no, i.payer_no,
	 i.claimsnet_payer_id, i.bill_form, i.num_labels,
	 i.fin_code, i.fin_class, i.comment, i.mod_date, i.mod_user,
     i.mod_prg, i.mod_host
	from inserted i

END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/11/2008
-- Description:	Update Trigger for INSC table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_UPDATE_INSC] 
   ON  dbo.insc 
   AFTER UPDATE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	select @rowguid = a.audit_rowguid
	from	 audit_insc a inner join inserted i on a.audit_rowguid = i.rowguid
		
	if (@rowguid is NULL) /* if this is null no record exists in the audit table 
								so copy the original record to the audit table from the deleted record*/
	BEGIN
	insert into audit_insc
	(audit_rowguid, deleted, code, [name], addr1, 
	 addr2, citystzip, provider_no, payer_no, 
	 claimsnet_payer_id, bill_form, num_labels,
	 fin_code, fin_class, comment, mod_date, mod_user,
     mod_prg, mod_host)
	select
	 d.rowguid, d.deleted, d.code, d.[name], d.addr1, 
	 d.addr2, d.citystzip, d.provider_no, d.payer_no, 
	 d.claimsnet_payer_id, d.bill_form, d.num_labels,
	 d.fin_code, d.fin_class, d.comment, d.mod_date, d.mod_user,
     d.mod_prg, d.mod_host
	
		from deleted d

	END

	insert into audit_insc
	(audit_rowguid, deleted, code, [name], addr1, 
	 addr2, citystzip, provider_no, payer_no, 
	 claimsnet_payer_id, bill_form, num_labels,
	 fin_code, fin_class, comment, mod_date, mod_user,
     mod_prg, mod_host)
	select
	 i.rowguid, i.deleted, i.code, i.[name], i.addr1, 
	 i.addr2, i.citystzip, i.provider_no, i.payer_no, 
	 i.claimsnet_payer_id, i.bill_form, i.num_labels,
	 i.fin_code, i.fin_class, i.comment, getdate(), suser_sname(),
     app_name(), host_name()
	from inserted i

END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20111012 added for SSI billing 2310B, 2330G, 2420C loops.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'provider_no_qualifier';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'06/11/2008 rgc/wdk added. This is the routing number for the insurance company.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'payer_no';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'from http://www.claimsnet.com/register/payerlist/payersearch.asp 11/14/2008 wdk added the default of ''EXCLUDED''', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'claimsnet_payer_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/15/2008 wdk/rgc added', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'fin_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/15/2008 added', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'comment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/15/2008 added', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'mod_host';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140123 added for ease in tracking path charges', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'is_mc_hmo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140319 use to eliminate insurance company from outpatient billing', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'allow_outpatient_billing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404011 to add new tracking of payments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'payor_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20150323 removed from mclrset\RINSC due to billing error in record set', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'insc', @level2type = N'COLUMN', @level2name = N'fin_class';

