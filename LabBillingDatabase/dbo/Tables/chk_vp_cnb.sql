CREATE TABLE [dbo].[chk_vp_cnb] (
    [acct_cnt] INT           NULL,
    [acct_dup] INT           NULL,
    [acct_no]  VARCHAR (MAX) NULL,
    [run_date] DATETIME      NULL,
    [run_for]  DATETIME      NULL,
    [mod_date] DATETIME      NULL,
    [mod_user] VARCHAR (50)  NULL
);


GO
CREATE CLUSTERED INDEX [IX_chk_vp_cnb]
    ON [dbo].[chk_vp_cnb]([run_date] ASC) WITH (FILLFACTOR = 90);

