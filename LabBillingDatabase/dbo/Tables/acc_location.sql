CREATE TABLE [dbo].[acc_location] (
    [account]    VARCHAR (15) NOT NULL,
    [location]   VARCHAR (15) NULL,
    [pt_type]    VARCHAR (5)  CONSTRAINT [DF_acc_location_pt_type] DEFAULT ('REF') NULL,
    [surveydate] DATETIME     NULL,
    [mod_date]   DATETIME     CONSTRAINT [DF_acc_location_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]   VARCHAR (50) CONSTRAINT [DF_acc_location_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_prg]    VARCHAR (50) CONSTRAINT [DF_acc_location_mod_prg] DEFAULT (app_name()) NULL,
    [mod_host]   VARCHAR (50) CONSTRAINT [DF_acc_location_mod_host] DEFAULT (host_name()) NULL,
    [ov_acct]    VARCHAR (30) NULL,
    [ov_mri]     VARCHAR (30) NULL,
    CONSTRAINT [PK_acc_location] PRIMARY KEY CLUSTERED ([account] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_acc_location]
    ON [dbo].[acc_location]([location] ASC, [surveydate] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_acc_location_ov_acct]
    ON [dbo].[acc_location]([ov_acct] ASC, [ov_mri] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_location, surveydate INCLUDE account, pt_type, mod_date, mod_user, mod_prg, mod_host, ov_acct, ov_mri]
    ON [dbo].[acc_location]([location] ASC, [surveydate] ASC)
    INCLUDE([account], [pt_type], [mod_date], [mod_user], [mod_prg], [mod_host], [ov_acct], [ov_mri]) WITH (FILLFACTOR = 90);


GO
CREATE TRIGGER [dbo].[tu_acc_location] ON [dbo].[acc_location]
FOR UPDATE 
AS
UPDATE acc_location
SET acc_location.mod_user = suser_sname(), acc_location.mod_date = getdate(), acc_location.mod_prg = app_name()
FROM inserted,acc_location
WHERE inserted.account = acc_location.account
