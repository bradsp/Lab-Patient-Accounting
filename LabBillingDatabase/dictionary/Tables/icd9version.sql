CREATE TABLE [dictionary].[icd9version] (
    [version]            VARCHAR (10)  NOT NULL,
    [effective_date]     DATETIME      NULL,
    [effective_end_date] DATETIME      NULL,
    [mod_date]           DATETIME      NULL,
    [mod_user]           VARCHAR (100) NULL,
    [mod_prg]            VARCHAR (100) NULL,
    [mod_host]           VARCHAR (100) NULL
);

