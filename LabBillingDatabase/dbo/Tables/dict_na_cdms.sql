CREATE TABLE [dbo].[dict_na_cdms] (
    [deleted]        BIT          CONSTRAINT [DF_dict_na_cdms_deleted] DEFAULT ((0)) NOT NULL,
    [cdm]            VARCHAR (7)  NOT NULL,
    [effective_date] DATETIME     NOT NULL,
    [end_date]       DATETIME     NULL,
    [mod_date]       DATETIME     CONSTRAINT [DF_dict_na_cdms_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]        VARCHAR (50) CONSTRAINT [DF_dict_na_cdms_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]       VARCHAR (50) CONSTRAINT [DF_dict_na_cdms_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]       VARCHAR (50) CONSTRAINT [DF_dict_na_cdms_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL
);

