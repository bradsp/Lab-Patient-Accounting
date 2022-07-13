CREATE TABLE [audit].[audit_insc] (
    [audit_rowguid]      UNIQUEIDENTIFIER NOT NULL,
    [deleted]            BIT              NOT NULL,
    [code]               VARCHAR (10)     NOT NULL,
    [name]               VARCHAR (45)     NULL,
    [addr1]              VARCHAR (30)     NULL,
    [addr2]              VARCHAR (30)     NULL,
    [citystzip]          VARCHAR (30)     NULL,
    [provider_no]        VARCHAR (20)     NULL,
    [payer_no]           VARCHAR (50)     NULL,
    [claimsnet_payer_id] VARCHAR (10)     NULL,
    [bill_form]          VARCHAR (5)      NULL,
    [num_labels]         INT              NULL,
    [fin_code]           VARCHAR (10)     NULL,
    [fin_class]          VARCHAR (10)     NULL,
    [comment]            VARCHAR (250)    NULL,
    [mod_date]           DATETIME         NULL,
    [mod_user]           VARCHAR (50)     NULL,
    [mod_prg]            VARCHAR (50)     NULL,
    [mod_host]           VARCHAR (50)     NULL,
    [uid]                BIGINT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_audit_insc] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'06/11/2008 wdk added for auditing purposes', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_insc', @level2type = N'COLUMN', @level2name = N'audit_rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'06/11/2008 rgc/wdk added. This is the routing number for the insurance company.', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_insc', @level2type = N'COLUMN', @level2name = N'payer_no';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'from http://www.claimsnet.com/register/payerlist/payersearch.asp', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_insc', @level2type = N'COLUMN', @level2name = N'claimsnet_payer_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/15/2008 wdk/rgc added', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_insc', @level2type = N'COLUMN', @level2name = N'fin_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/15/2008 added', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_insc', @level2type = N'COLUMN', @level2name = N'comment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/15/2008 added', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_insc', @level2type = N'COLUMN', @level2name = N'mod_host';

