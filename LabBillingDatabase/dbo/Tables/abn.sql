CREATE TABLE [dbo].[abn] (
    [account]  VARCHAR (15) NOT NULL,
    [cdm]      VARCHAR (7)  NOT NULL,
    [mod_date] DATETIME     CONSTRAINT [DF_abn_mod_date_2__20] DEFAULT (getdate()) NULL,
    [mod_user] VARCHAR (50) CONSTRAINT [DF_abn_mod_user_4__20] DEFAULT (suser_sname()) NULL,
    [mod_prg]  VARCHAR (50) CONSTRAINT [DF_abn_mod_prg_3__20] DEFAULT (app_name()) NULL,
    [lmrp]     INT          CONSTRAINT [DF_abn_lmrp_1__20] DEFAULT ((0)) NULL,
    CONSTRAINT [PK___1__20] PRIMARY KEY CLUSTERED ([account] ASC, [cdm] ASC) WITH (FILLFACTOR = 90)
);

