CREATE TABLE [dbo].[acc_status_updates] (
    [account]    VARCHAR (15) CONSTRAINT [DF_acc_status_updates_account] DEFAULT ((0)) NOT NULL,
    [acc_status] NCHAR (10)   NOT NULL,
    [trans_date] DATETIME     NOT NULL,
    [chrg_dos]   DATETIME     NOT NULL,
    [emailed]    BIT          CONSTRAINT [DF_acc_status_updates_emailed] DEFAULT ((0)) NOT NULL,
    [mod_date]   DATETIME     CONSTRAINT [DF_acc_status_updates_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]    VARCHAR (50) CONSTRAINT [DF_acc_status_updates_mod_prg] DEFAULT (app_name()) NULL,
    [mod_user]   VARCHAR (50) CONSTRAINT [DF_acc_status_updates_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_host]   VARCHAR (50) CONSTRAINT [DF_acc_status_updates_mod_host] DEFAULT (host_name()) NULL
);


GO
CREATE CLUSTERED INDEX [CDX_account]
    ON [dbo].[acc_status_updates]([account] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_emailed]
    ON [dbo].[acc_status_updates]([emailed] ASC)
    INCLUDE([account], [acc_status], [trans_date], [mod_date]) WITH (FILLFACTOR = 90);

