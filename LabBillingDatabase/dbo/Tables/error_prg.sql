CREATE TABLE [dbo].[error_prg] (
    [PROCESSED]  BIT            CONSTRAINT [DF__error_prg__PROCE__5C835C1E] DEFAULT ((0)) NULL,
    [error_type] VARCHAR (50)   NULL,
    [app_name]   VARCHAR (50)   NOT NULL,
    [app_module] VARCHAR (1024) NULL,
    [error]      VARCHAR (MAX)  NOT NULL,
    [mod_date]   DATETIME       CONSTRAINT [DF_error_prg_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]    VARCHAR (50)   CONSTRAINT [DF_error_prg_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]   VARCHAR (50)   CONSTRAINT [DF_error_prg_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]   VARCHAR (50)   CONSTRAINT [DF_error_prg_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [account]    VARCHAR (15)   NULL,
    [uid]        INT            IDENTITY (1, 1) NOT NULL
);

