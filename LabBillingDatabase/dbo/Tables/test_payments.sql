CREATE TABLE [dbo].[test_payments] (
    [account]        VARCHAR (15)    NOT NULL,
    [amt_paid]       NUMERIC (10, 2) NOT NULL,
    [contractual]    NUMERIC (10, 2) NOT NULL,
    [write_off]      NUMERIC (10, 2) NOT NULL,
    [write_off_code] VARCHAR (10)    NULL
);

