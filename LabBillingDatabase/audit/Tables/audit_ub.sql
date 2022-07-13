CREATE TABLE [audit].[audit_ub] (
    [ub_rowguid]         UNIQUEIDENTIFIER NOT NULL,
    [deleted]            BIT              NOT NULL,
    [account]            VARCHAR (15)     NOT NULL,
    [ins_abc]            VARCHAR (1)      NOT NULL,
    [run_date]           DATETIME         NULL,
    [printed]            BIT              NOT NULL,
    [run_user]           VARCHAR (30)     NULL,
    [fin_code]           VARCHAR (1)      NULL,
    [trans_date]         DATETIME         NULL,
    [pat_name]           VARCHAR (40)     NULL,
    [claimsnet_payer_id] VARCHAR (50)     NULL,
    [ebill_status]       VARCHAR (5)      NULL,
    [batch]              NUMERIC (10)     NULL,
    [text]               VARCHAR (8000)   NULL,
    [edited_ub]          VARCHAR (8000)   NULL,
    [cold_feed]          DATETIME         NULL,
    [mod_date]           DATETIME         NOT NULL,
    [mod_user]           VARCHAR (50)     NOT NULL,
    [mod_prg]            VARCHAR (50)     NOT NULL,
    [mod_host]           VARCHAR (50)     NOT NULL
);


GO
CREATE CLUSTERED INDEX [IX_audit_ub]
    ON [audit].[audit_ub]([account] ASC, [ins_abc] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_account]
    ON [audit].[audit_ub]([account] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'11/13/2008 wdk added for control of ST/SE segments in EBILL.', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_ub', @level2type = N'COLUMN', @level2name = N'claimsnet_payer_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'11/13/2008 wdk added for billing ub''s via claimsnet', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_ub', @level2type = N'COLUMN', @level2name = N'ebill_status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'changed from text on 03/13/2008 wdk', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_ub', @level2type = N'COLUMN', @level2name = N'text';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'added 03/13/2008 to track editing of ub''s before printing.', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_ub', @level2type = N'COLUMN', @level2name = N'edited_ub';

