CREATE TABLE [dbo].[ReChrg] (
    [host_name] VARCHAR (80) CONSTRAINT [DF_ReChrg_host_name_2__13] DEFAULT (host_name()) NULL,
    [cdm]       VARCHAR (10) NULL,
    [qty]       NUMERIC (4)  NULL,
    [amount]    MONEY        NULL,
    [urn]       NUMERIC (15) IDENTITY (1, 1) NOT NULL,
    [account]   VARCHAR (15) NULL,
    CONSTRAINT [PK___1__13] PRIMARY KEY NONCLUSTERED ([urn] ASC) WITH (FILLFACTOR = 90)
);

