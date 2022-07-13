CREATE TABLE [dbo].[chrg_pa] (
    [chrg_num]     NUMERIC (15)     NOT NULL,
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF__chrg_pa__rowguid__642E9F87] DEFAULT (newid()) NOT NULL,
    [pa_amount]    MONEY            NULL,
    [batch]        NUMERIC (10)     NULL,
    [mod_date]     DATETIME         CONSTRAINT [DF_chrg_pa_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]     VARCHAR (50)     CONSTRAINT [DF_chrg_pa_mod_user_5__13] DEFAULT (suser_sname()) NULL,
    [mod_prg]      VARCHAR (50)     CONSTRAINT [DF_chrg_pa_mod_prg_4__13] DEFAULT (app_name()) NULL,
    [mod_host]     VARCHAR (50)     CONSTRAINT [DF_chrg_pa_mod_host_3__13] DEFAULT (host_name()) NULL,
    [mt_req_no]    VARCHAR (8)      NULL,
    [perform_site] VARCHAR (10)     NULL,
    CONSTRAINT [PK_chrg_pa_1__13] PRIMARY KEY CLUSTERED ([chrg_num] ASC, [rowguid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_chrg_pa_batch]
    ON [dbo].[chrg_pa]([batch] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [mt_req_no_idx]
    ON [dbo].[chrg_pa]([mt_req_no] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20110204 changed from varchar(20) to datetime rgc/wdk 20110504 added default value', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'chrg_pa', @level2type = N'COLUMN', @level2name = N'mod_date';

