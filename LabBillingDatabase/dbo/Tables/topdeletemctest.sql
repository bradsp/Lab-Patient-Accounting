CREATE TABLE [dbo].[topdeletemctest] (
    [account]    VARCHAR (15)    NOT NULL,
    [trans_date] DATETIME        NOT NULL,
    [paid]       DATETIME        NULL,
    [payment]    NUMERIC (18, 2) NULL,
    [mod_date]   DATETIME        NULL
);

