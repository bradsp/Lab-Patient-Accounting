CREATE TABLE [dbo].[data_tier_pricing] (
    [CLIENT]          VARCHAR (10)    NULL,
    [NAME]            VARCHAR (40)    NULL,
    [CDM]             VARCHAR (7)     NULL,
    [CDM DESCRIPTION] VARCHAR (50)    NULL,
    [CLIENT PRICE]    NUMERIC (18, 2) NULL,
    [INSURANCE PRICE] NUMERIC (18, 2) NULL
);


GO
CREATE NONCLUSTERED INDEX [ndx_data_tier_pricing_primary]
    ON [dbo].[data_tier_pricing]([CLIENT] ASC, [CDM] ASC) WITH (FILLFACTOR = 90);

