CREATE TABLE [dbo].[dict_quest_global_billing] (
    [deleted]        BIT             CONSTRAINT [DF__dict_ques__delet__322242A7] DEFAULT ((0)) NULL,
    [cdm]            VARCHAR (7)     NULL,
    [cpt]            VARCHAR (5)     NULL,
    [amt]            NUMERIC (18, 2) NULL,
    [effective_date] DATETIME        NULL,
    [end_date]       DATETIME        NULL,
    [mod_date]       DATETIME        CONSTRAINT [DF__dict_ques__mod_d__331666E0] DEFAULT (getdate()) NULL,
    [mod_prg]        VARCHAR (50)    CONSTRAINT [DF__dict_ques__mod_p__340A8B19] DEFAULT (app_name()) NULL,
    [mod_user]       VARCHAR (50)    CONSTRAINT [DF__dict_ques__mod_u__34FEAF52] DEFAULT (suser_sname()) NULL,
    [mod_host]       VARCHAR (50)    CONSTRAINT [DF__dict_ques__mod_h__35F2D38B] DEFAULT (host_name()) NULL
);

