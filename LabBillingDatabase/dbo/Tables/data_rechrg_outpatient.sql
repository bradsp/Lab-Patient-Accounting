CREATE TABLE [dbo].[data_rechrg_outpatient] (
    [account]        VARCHAR (15) NULL,
    [chrg_num]       NUMERIC (15) NOT NULL,
    [cdm]            VARCHAR (7)  NULL,
    [cpt4]           VARCHAR (5)  NULL,
    [net_amt]        MONEY        NULL,
    [qty]            NUMERIC (4)  NULL,
    [amount]         MONEY        NULL,
    [new price]      MONEY        NULL,
    [diff]           MONEY        NULL,
    [processed]      BIT          NOT NULL,
    [processed_date] DATETIME     NULL
);

