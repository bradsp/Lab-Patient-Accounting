CREATE TABLE [dbo].[data_quest_global_billing_tracking] (
    [deleted]          BIT             CONSTRAINT [DF__dict_ques__delet__37DB1BFD] DEFAULT ((0)) NULL,
    [account]          VARCHAR (15)    NOT NULL,
    [pat_name]         VARCHAR (100)   NOT NULL,
    [date_of_service]  DATETIME        NOT NULL,
    [cdm]              VARCHAR (7)     NULL,
    [credited]         BIT             CONSTRAINT [DF__dict_ques__credi__38CF4036] DEFAULT ((0)) NULL,
    [credited_date]    DATETIME        NULL,
    [amt]              NUMERIC (18, 2) NULL,
    [Qaccount_invoice] VARCHAR (15)    NULL,
    [Jaccount_invoice] VARCHAR (15)    NULL,
    [chrg_num]         NUMERIC (15)    NULL,
    [mod_date]         DATETIME        CONSTRAINT [DF__dict_ques__mod_d__39C3646F] DEFAULT (getdate()) NULL,
    [mod_prg]          VARCHAR (50)    CONSTRAINT [DF__dict_ques__mod_p__3AB788A8] DEFAULT (app_name()) NULL,
    [mod_user]         VARCHAR (50)    CONSTRAINT [DF__dict_ques__mod_u__3BABACE1] DEFAULT (suser_sname()) NULL,
    [mod_host]         VARCHAR (50)    CONSTRAINT [DF__dict_ques__mod_h__3C9FD11A] DEFAULT (host_name()) NULL,
    [uid]              BIGINT          IDENTITY (1, 1) NOT NULL
);

