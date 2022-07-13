CREATE TABLE [dbo].[rds] (
    [uri]       NUMERIC (15) IDENTITY (100, 1) NOT NULL,
    [deleted]   BIT          CONSTRAINT [DF_rds_deleted_7__11] DEFAULT ((0)) NOT NULL,
    [name]      VARCHAR (40) NOT NULL,
    [cli_mnem]  VARCHAR (10) NOT NULL,
    [mod_date]  DATETIME     CONSTRAINT [DF_rds_mod_date_1__11] DEFAULT (getdate()) NULL,
    [mod_user]  VARCHAR (50) CONSTRAINT [DF_rds_mod_user_2__11] DEFAULT (suser_sname()) NULL,
    [mod_prg]   VARCHAR (50) CONSTRAINT [DF_rds_mod_prg_3__11] DEFAULT (app_name()) NULL,
    [mod_host]  VARCHAR (50) CONSTRAINT [DF_rds_mod_host_4__11] DEFAULT (host_name()) NULL,
    [shift]     VARCHAR (10) NULL,
    [test_date] DATETIME     NULL,
    CONSTRAINT [PK_rds_1__24] PRIMARY KEY NONCLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE CLUSTERED INDEX [name_cdx]
    ON [dbo].[rds]([name] ASC) WITH (FILLFACTOR = 90);


GO
CREATE TRIGGER [dbo].[tu_rds] ON dbo.rds 
FOR UPDATE
AS
UPDATE rds
SET rds.mod_user = suser_sname(), rds.mod_date = getdate(), rds.mod_prg = app_name()
FROM inserted,rds
WHERE inserted.uri = rds.uri
