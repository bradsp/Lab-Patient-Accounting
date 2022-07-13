CREATE TABLE [dbo].[chrg_pa_save] (
    [chrg_num]     NUMERIC (15)     NOT NULL,
    [pa_amount]    MONEY            NULL,
    [batch]        NUMERIC (10)     NULL,
    [mod_date]     DATETIME         NULL,
    [mod_user]     VARCHAR (20)     NULL,
    [mod_prg]      VARCHAR (20)     NULL,
    [mod_host]     VARCHAR (20)     NULL,
    [mt_req_no]    VARCHAR (8)      NULL,
    [perform_site] VARCHAR (10)     NULL,
    [rowguid]      UNIQUEIDENTIFIER NULL
);

