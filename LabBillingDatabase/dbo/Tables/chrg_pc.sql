CREATE TABLE [dbo].[chrg_pc] (
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_chrg_pc_rowguid] DEFAULT (newid()) NOT NULL,
    [cli_mnem]     VARCHAR (10)     NOT NULL,
    [account]      VARCHAR (15)     NOT NULL,
    [cdm]          VARCHAR (7)      NOT NULL,
    [qty]          INT              NOT NULL,
    [service_date] DATETIME         NOT NULL,
    [src_file]     VARCHAR (50)     NOT NULL,
    [mod_date]     DATETIME         CONSTRAINT [DF_chrg_pc_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]      VARCHAR (50)     CONSTRAINT [DF_chrg_pc_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_user]     VARCHAR (50)     CONSTRAINT [DF_chrg_pc_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_host]     VARCHAR (50)     CONSTRAINT [DF_chrg_pc_mod_host] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_chrg_pc] PRIMARY KEY CLUSTERED ([rowguid] ASC) WITH (FILLFACTOR = 90)
);

