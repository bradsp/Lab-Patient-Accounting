CREATE TABLE [dbo].[test_charges] (
    [charge_number] INT          IDENTITY (1, 1) NOT NULL,
    [account]       VARCHAR (15) NOT NULL,
    [cdm]           VARCHAR (10) NOT NULL,
    [qty]           INT          NOT NULL,
    CONSTRAINT [PK_test_charges] PRIMARY KEY CLUSTERED ([charge_number] ASC) WITH (FILLFACTOR = 90)
);

