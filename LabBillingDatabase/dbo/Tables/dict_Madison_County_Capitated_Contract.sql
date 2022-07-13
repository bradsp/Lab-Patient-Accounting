CREATE TABLE [dbo].[dict_Madison_County_Capitated_Contract] (
    [deleted]        BIT            NOT NULL,
    [cdm]            VARCHAR (7)    NOT NULL,
    [description]    VARCHAR (8000) NULL,
    [effective_date] DATETIME       NOT NULL,
    [end_date]       DATETIME       NULL,
    [mod_date]       DATETIME       NOT NULL,
    [mod_prg]        VARCHAR (50)   NOT NULL,
    [mod_user]       VARCHAR (50)   NOT NULL,
    [mod_host]       VARCHAR (50)   NOT NULL
);

