CREATE TABLE [dbo].[patdx] (
    [deleted]        BIT          CONSTRAINT [DF_patdx_deleted] DEFAULT ((0)) NOT NULL,
    [account]        VARCHAR (15) NOT NULL,
    [dx_number]      INT          NOT NULL,
    [diagnosis]      VARCHAR (7)  NOT NULL,
    [version]        VARCHAR (50) NOT NULL,
    [code_qualifier] VARCHAR (5)  NULL,
    [is_error]       BIT          CONSTRAINT [DF_Table_2_is_valid] DEFAULT ((0)) NOT NULL,
    [import_file]    VARCHAR (50) NULL,
    [mod_date]       DATETIME     CONSTRAINT [DF_patdx_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]        VARCHAR (50) CONSTRAINT [DF_patdx_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]       VARCHAR (50) CONSTRAINT [DF_patdx_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]       VARCHAR (50) CONSTRAINT [DF_patdx_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [posted_date]    DATETIME     NULL,
    [uid]            BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_patdx] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_patdx_account]
    ON [dbo].[patdx]([account] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ABK for Icd10; BK for Icd9', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'patdx', @level2type = N'COLUMN', @level2name = N'code_qualifier';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'set to true if the disgnosis is not contained in the icd9desc table for this version', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'patdx', @level2type = N'COLUMN', @level2name = N'is_error';

