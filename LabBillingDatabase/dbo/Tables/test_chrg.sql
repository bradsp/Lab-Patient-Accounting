CREATE TABLE [dbo].[test_chrg] (
    [rowguid]      UNIQUEIDENTIFIER CONSTRAINT [DF_chrg_rowguid_test] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [chrg_num]     NUMERIC (15)     IDENTITY (1, 1) NOT NULL,
    [account]      VARCHAR (15)     NULL,
    [service_date] DATETIME         CONSTRAINT [DF_chrg_service_date_1__11_test] DEFAULT (getdate()) NULL,
    [cdm]          VARCHAR (7)      NULL,
    [qty]          NUMERIC (4)      NULL,
    [retail]       MONEY            CONSTRAINT [DF_chrg_retail_1__11_test] DEFAULT ((0.00)) NULL,
    [net_amt]      MONEY            CONSTRAINT [DF_chrg_net_amt_1__22_test] DEFAULT ((0.00)) NULL,
    CONSTRAINT [PK_chrg_1__10_test] PRIMARY KEY NONCLUSTERED ([chrg_num] ASC) WITH (FILLFACTOR = 90)
);

