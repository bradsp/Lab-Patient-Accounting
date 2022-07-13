CREATE TABLE [dbo].[mutually_excl] (
    [cpt4_1]         VARCHAR (5)  NOT NULL,
    [cpt4_2]         VARCHAR (5)  NOT NULL,
    [version]        VARCHAR (5)  NULL,
    [mod_user]       VARCHAR (30) CONSTRAINT [DF_mutually_e_mod_user_7__13] DEFAULT (right(suser_sname(),(30))) NULL,
    [mod_date]       DATETIME     CONSTRAINT [DF_mutually_e_mod_date_4__13] DEFAULT (getdate()) NULL,
    [mod_prg]        VARCHAR (30) CONSTRAINT [DF_mutually_e_mod_prg_6__13] DEFAULT (right(app_name(),(30))) NULL,
    [mod_host]       VARCHAR (30) CONSTRAINT [DF_mutually_e_mod_host_5__13] DEFAULT (right(host_name(),(30))) NULL,
    [modi_indicator] VARCHAR (1)  CONSTRAINT [DF_mutually_excl_modi_indicator] DEFAULT ('0') NULL,
    CONSTRAINT [PK_cpt41_cpt42] PRIMARY KEY CLUSTERED ([cpt4_1] ASC, [cpt4_2] ASC) WITH (FILLFACTOR = 90)
);

