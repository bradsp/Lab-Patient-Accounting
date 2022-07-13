CREATE TABLE [dbo].[chrg_rev_trk] (
    [chrg_num] NUMERIC (15) NOT NULL,
    [mod_date] DATETIME     CONSTRAINT [DF_chrg_rev_t_mod_date_2__16] DEFAULT (getdate()) NULL,
    [mod_user] VARCHAR (20) CONSTRAINT [DF_chrg_rev_t_mod_user_5__16] DEFAULT (suser_sname()) NULL,
    [mod_prg]  VARCHAR (20) CONSTRAINT [DF_chrg_rev_t_mod_prg_4__16] DEFAULT (app_name()) NULL,
    [mod_host] VARCHAR (20) CONSTRAINT [DF_chrg_rev_t_mod_host_3__16] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_chrg_rev_trk_1__16] PRIMARY KEY CLUSTERED ([chrg_num] ASC) WITH (FILLFACTOR = 90)
);

