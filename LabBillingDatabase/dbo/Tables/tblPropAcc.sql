CREATE TABLE [dbo].[tblPropAcc] (
    [propPK]        INT          NULL,
    [propClient]    VARCHAR (10) NULL,
    [propAccount]   VARCHAR (15) NULL,
    [propFinCode]   VARCHAR (10) NULL,
    [propInsCode]   VARCHAR (10) NULL,
    [propInsPolicy] VARCHAR (50) NULL,
    [mod_date]      DATETIME     CONSTRAINT [DF_tblPropAcc_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]      VARCHAR (50) CONSTRAINT [DF_tblPropAcc_mod_user] DEFAULT (left(suser_sname(),(50))) NOT NULL,
    [mod_prg]       VARCHAR (50) CONSTRAINT [DF_tblPropAcc_mod_prg] DEFAULT (left(app_name(),(50))) NOT NULL,
    [mod_host]      VARCHAR (50) CONSTRAINT [DF_tblPropAcc_mod_host] DEFAULT (left(host_name(),(50))) NOT NULL
);

