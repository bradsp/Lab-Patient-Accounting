CREATE TABLE [dbo].[ACC_LMRP] (
    [account]  VARCHAR (15)   NOT NULL,
    [dos]      DATETIME       NOT NULL,
    [fin_code] VARCHAR (10)   NOT NULL,
    [cl_mnem]  VARCHAR (10)   NOT NULL,
    [erorr]    VARCHAR (1024) NOT NULL,
    [mod_date] DATETIME       CONSTRAINT [DF_ACC_LMRP_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user] VARCHAR (50)   CONSTRAINT [DF_ACC_LMRP_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_prg]  VARCHAR (50)   CONSTRAINT [DF_ACC_LMRP_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_host] VARCHAR (50)   CONSTRAINT [DF_ACC_LMRP_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [uri]      INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ACC_LMRP] PRIMARY KEY CLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_acc_lmrp_account]
    ON [dbo].[ACC_LMRP]([account] ASC) WITH (FILLFACTOR = 90);

