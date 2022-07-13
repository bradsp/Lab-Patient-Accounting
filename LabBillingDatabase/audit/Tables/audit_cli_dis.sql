CREATE TABLE [audit].[audit_cli_dis] (
    [audit_rowguid] UNIQUEIDENTIFIER NULL,
    [deleted]       BIT              NULL,
    [cli_mnem]      VARCHAR (10)     NULL,
    [start_cdm]     VARCHAR (7)      NULL,
    [end_cdm]       VARCHAR (7)      NULL,
    [percent_ds]    REAL             NULL,
    [price]         NUMERIC (18, 2)  NULL,
    [mod_date]      DATETIME         NULL,
    [mod_user]      VARCHAR (50)     NULL,
    [mod_prg]       VARCHAR (50)     NULL,
    [mod_host]      VARCHAR (50)     NULL,
    [uri]           NUMERIC (10)     IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_audit_cli_dis] PRIMARY KEY CLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20110128 added for setting price vs percent discount', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_cli_dis', @level2type = N'COLUMN', @level2name = N'price';

