CREATE TABLE [audit].[audit_chk] (
    [chk_rowguid]    UNIQUEIDENTIFIER NOT NULL,
    [deleted]        BIT              NOT NULL,
    [pay_no]         NUMERIC (15)     NOT NULL,
    [account]        VARCHAR (15)     NULL,
    [chk_date]       DATETIME         NULL,
    [date_rec]       DATETIME         NULL,
    [chk_no]         VARCHAR (25)     NULL,
    [amt_paid]       MONEY            NULL,
    [write_off]      MONEY            NULL,
    [contractual]    MONEY            NULL,
    [status]         VARCHAR (15)     NULL,
    [source]         VARCHAR (50)     NULL,
    [w_off_date]     DATETIME         NULL,
    [invoice]        VARCHAR (15)     NULL,
    [batch]          NUMERIC (15)     NULL,
    [comment]        VARCHAR (50)     NULL,
    [bad_debt]       BIT              NOT NULL,
    [mod_date]       DATETIME         NOT NULL,
    [mod_user]       VARCHAR (50)     NOT NULL,
    [mod_prg]        VARCHAR (50)     NOT NULL,
    [mod_host]       VARCHAR (50)     NOT NULL,
    [mod_date_audit] DATETIME         CONSTRAINT [DF_audit_chk_mod_host_audit] DEFAULT (getdate()) NOT NULL,
    [uid]            BIGINT           IDENTITY (1, 1) NOT NULL,
    [cpt4Code]       VARCHAR (50)     NULL,
    [post_file]      VARCHAR (256)    NULL,
    [chrg_rowguid]   UNIQUEIDENTIFIER NULL,
    [write_off_code] VARCHAR (4)      NULL,
    [eft_date]       DATETIME         NULL,
    [eft_number]     VARCHAR (50)     NULL,
    [ins_code]       VARCHAR (10)     NULL,
    [fin_code]       VARCHAR (10)     NULL,
    CONSTRAINT [PK_audit_chk_1] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_audit_chk_rowguid]
    ON [audit].[audit_chk]([chk_rowguid] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_audit_chk_account]
    ON [audit].[audit_chk]([account] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'storing the cpt4 and modifier when posted from an 835 file', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_chk', @level2type = N'COLUMN', @level2name = N'cpt4Code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'For a batch file posting from an 835 file', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_chk', @level2type = N'COLUMN', @level2name = N'post_file';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rowguid from the charge table', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_chk', @level2type = N'COLUMN', @level2name = N'chrg_rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'02/27/2008 added to help with write off tracking. Per Ed.', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_chk', @level2type = N'COLUMN', @level2name = N'write_off_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/06/08 used as part of the electronic check posting filter from ISA09', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_chk', @level2type = N'COLUMN', @level2name = N'eft_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/06/08 used as part of the electronic check posting filter from GS05', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_chk', @level2type = N'COLUMN', @level2name = N'eft_number';

