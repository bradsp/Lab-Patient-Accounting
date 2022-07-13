CREATE TABLE [dbo].[Medicare_Accounts_Date_Time] (
    [account]    VARCHAR (15)    NOT NULL,
    [trans_date] DATETIME        NOT NULL,
    [paid]       DATETIME        NULL,
    [payment]    NUMERIC (18, 2) NULL,
    [mod_date]   DATETIME        DEFAULT (CONVERT([varchar](10),getdate(),(101))) NULL
);

