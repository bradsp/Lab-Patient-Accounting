CREATE TABLE [zzz].[dict_ViewerAccSql] (
    [type_check]     VARCHAR (50)   NULL,
    [fin_code]       VARCHAR (10)   NULL,
    [ins_code]       VARCHAR (50)   NULL,
    [bill_form]      VARCHAR (50)   NULL,
    [valid]          BIT            NOT NULL,
    [strSql]         VARCHAR (8000) NOT NULL,
    [effective_date] DATETIME       CONSTRAINT [DF_dict_ViewerAccSql_effective_date] DEFAULT (getdate()) NOT NULL,
    [expire_date]    DATETIME       NULL,
    [error]          VARCHAR (256)  NULL,
    [mod_date]       DATETIME       NOT NULL,
    [mod_prg]        VARCHAR (50)   NOT NULL,
    [mod_user]       VARCHAR (50)   NOT NULL,
    [mod_host]       VARCHAR (50)   NOT NULL,
    [uid]            INT            IDENTITY (1, 1) NOT NULL
);

