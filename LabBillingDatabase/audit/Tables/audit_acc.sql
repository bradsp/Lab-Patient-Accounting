CREATE TABLE [audit].[audit_acc] (
    [acc_rowguid]      UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [deleted]          BIT              NOT NULL,
    [account]          VARCHAR (15)     NOT NULL,
    [pat_name]         VARCHAR (40)     NULL,
    [cl_mnem]          VARCHAR (10)     NULL,
    [fin_code]         VARCHAR (10)     NULL,
    [trans_date]       DATETIME         NULL,
    [cbill_date]       DATETIME         NULL,
    [status]           VARCHAR (8)      NULL,
    [ssn]              VARCHAR (11)     NULL,
    [num_comments]     INT              NULL,
    [meditech_account] VARCHAR (15)     NULL,
    [original_fincode] VARCHAR (1)      NULL,
    [mod_date]         DATETIME         NOT NULL,
    [mod_user]         VARCHAR (50)     NOT NULL,
    [mod_prg]          VARCHAR (50)     NOT NULL,
    [oereqno]          VARCHAR (15)     NULL,
    [mri]              VARCHAR (25)     NULL,
    [mod_host]         VARCHAR (50)     NOT NULL,
    [uid]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [post_date]        DATETIME         NULL,
    [ov_order_id]      VARCHAR (50)     NULL,
    [ov_pat_id]        VARCHAR (50)     NULL,
    [guarantorID]      VARCHAR (50)     NULL,
    [HNE_NUMBER]       VARCHAR (50)     NULL,
    [tdate_update]     BIT              NULL,
    CONSTRAINT [PK_audit_acc] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_account_mod_date]
    ON [audit].[audit_acc]([account] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [missing_index_100_99_audit_acc]
    ON [audit].[audit_acc]([acc_rowguid] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_user_mod_date]
    ON [audit].[audit_acc]([mod_user] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20091116 added for aging use ', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_acc', @level2type = N'COLUMN', @level2name = N'post_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100215 added for tracking EHS billing back to the original order. Should be NULL for MCLOE accounts.', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_acc', @level2type = N'COLUMN', @level2name = N'ov_order_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100215 added for tracking EHS billing back to the original order. Should be NULL for MCLOE accounts.', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_acc', @level2type = N'COLUMN', @level2name = N'ov_pat_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140407 to track the guarantors by account', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_acc', @level2type = N'COLUMN', @level2name = N'guarantorID';

