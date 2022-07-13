CREATE TABLE [dbo].[data_billing_history] (
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_dbh_rowguid] DEFAULT (newid()) NULL,
    [deleted]      BIT              CONSTRAINT [DF_dbh_deleted_2__20] DEFAULT ((0)) NOT NULL,
    [account]      VARCHAR (15)     NOT NULL,
    [ins_abc]      VARCHAR (1)      NOT NULL,
    [pat_name]     VARCHAR (40)     NULL,
    [fin_code]     VARCHAR (1)      NULL,
    [ins_code]     VARCHAR (10)     NULL,
    [trans_date]   DATETIME         NULL,
    [run_date]     DATETIME         CONSTRAINT [DF_dbh_run_date_4__20] DEFAULT (getdate()) NOT NULL,
    [printed]      BIT              CONSTRAINT [DF_dbh_printed_3__20] DEFAULT ((0)) NOT NULL,
    [run_user]     VARCHAR (20)     CONSTRAINT [DF_dbh_run_user_5__20] DEFAULT (suser_sname()) NOT NULL,
    [batch]        NUMERIC (10)     CONSTRAINT [DF_dbh_batch_1__20] DEFAULT ((-1)) NOT NULL,
    [ebill_status] VARCHAR (10)     NULL,
    [ebill_batch]  NUMERIC (10)     NULL,
    [text]         VARCHAR (MAX)    NULL,
    [ins_complete] DATETIME         NULL,
    [mod_date]     DATETIME         CONSTRAINT [DF_dbh_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]     VARCHAR (50)     CONSTRAINT [DF_dbh_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]      VARCHAR (50)     CONSTRAINT [DF_dbh_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]     VARCHAR (50)     CONSTRAINT [DF_dbh_mod_host] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_data_billing_history] PRIMARY KEY CLUSTERED ([account] ASC, [run_date] ASC) WITH (FILLFACTOR = 90)
);

