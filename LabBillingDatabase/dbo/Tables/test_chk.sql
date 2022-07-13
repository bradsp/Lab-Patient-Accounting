CREATE TABLE [dbo].[test_chk] (
    [rowguid]  UNIQUEIDENTIFIER CONSTRAINT [DF_chk_rowguid_test] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]  BIT              CONSTRAINT [DF_chk_deleted_2__13_test] DEFAULT ((0)) NOT NULL,
    [pay_no]   NUMERIC (15)     IDENTITY (1, 1) NOT NULL,
    [account]  VARCHAR (15)     NOT NULL,
    [chk_date] DATETIME         NULL,
    [date_rec] DATETIME         NULL,
    [chk_no]   VARCHAR (10)     NULL,
    [amt_paid] MONEY            CONSTRAINT [DF_chk_amt_paid_1__16_test] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_chk_1__10_test] PRIMARY KEY NONCLUSTERED ([pay_no] ASC) WITH (FILLFACTOR = 90)
);

