CREATE TABLE [audit].[audit_h1500] (
    [h1500_rowguid]      UNIQUEIDENTIFIER NOT NULL,
    [deleted]            BIT              NOT NULL,
    [account]            VARCHAR (15)     NOT NULL,
    [ins_abc]            VARCHAR (1)      NOT NULL,
    [pat_name]           VARCHAR (40)     NULL,
    [fin_code]           VARCHAR (1)      NULL,
    [claimsnet_payer_id] VARCHAR (50)     NULL,
    [trans_date]         DATETIME         NULL,
    [run_date]           DATETIME         NULL,
    [printed]            BIT              NOT NULL,
    [run_user]           VARCHAR (20)     NOT NULL,
    [batch]              NUMERIC (10)     NOT NULL,
    [date_sent]          DATETIME         NULL,
    [sent_user]          VARCHAR (20)     NULL,
    [ebill_status]       VARCHAR (5)      NULL,
    [ebill_batch]        NUMERIC (10)     NULL,
    [text]               VARCHAR (MAX)    NULL,
    [cold_feed]          DATETIME         NULL,
    [mod_date]           DATETIME         NOT NULL,
    [mod_user]           VARCHAR (50)     NOT NULL,
    [mod_prg]            VARCHAR (50)     NOT NULL,
    [mod_host]           VARCHAR (50)     NOT NULL
);


GO
CREATE CLUSTERED INDEX [IX_audit_h1500]
    ON [audit].[audit_h1500]([account] ASC, [ins_abc] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_account]
    ON [audit].[audit_h1500]([account] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'07/09/2008 rgc/wdk added for control of ST/SE segments in EBILL.', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_h1500', @level2type = N'COLUMN', @level2name = N'claimsnet_payer_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'04/15/2008 wdk/rgc Coverted from 8000 to MAX. 03/21/2008 wdk converted from text', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_h1500', @level2type = N'COLUMN', @level2name = N'text';

