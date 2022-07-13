CREATE TABLE [dbo].[tempChk] (
    [account]     VARCHAR (15)    NULL,
    [source]      VARCHAR (50)    NULL,
    [paid]        NUMERIC (18, 2) NULL,
    [contractual] NUMERIC (18, 2) NULL,
    [write_off]   NUMERIC (18, 2) NULL,
    [total]       AS              (([paid]+[contractual])+[write_off])
);

