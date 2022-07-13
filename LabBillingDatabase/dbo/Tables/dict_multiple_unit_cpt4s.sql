CREATE TABLE [dbo].[dict_multiple_unit_cpt4s] (
    [rowguid]  UNIQUEIDENTIFIER CONSTRAINT [DF_dict_multiple_unit_cpt4s_rowguid] DEFAULT (newid()) NOT NULL,
    [cpt4]     VARCHAR (5)      NOT NULL,
    [mod_date] VARCHAR (50)     CONSTRAINT [DF_dict_multiple_unit_cpt4s_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user] VARCHAR (50)     CONSTRAINT [DF_dict_multiple_unit_cpt4s_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]  VARCHAR (50)     CONSTRAINT [DF_dict_multiple_unit_cpt4s_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host] VARCHAR (50)     CONSTRAINT [DF_dict_multiple_unit_cpt4s_mod_host] DEFAULT (host_name()) NOT NULL
);

