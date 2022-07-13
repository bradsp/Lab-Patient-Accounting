CREATE TABLE [dbo].[data_cerner_third_party_fee_schedule] (
    [cdm]              VARCHAR (7)  NOT NULL,
    [cdm_description]  VARCHAR (50) NULL,
    [mnemonic]         VARCHAR (15) NULL,
    [link]             INT          NOT NULL,
    [cpt4]             VARCHAR (5)  NULL,
    [cpt4_description] VARCHAR (50) NULL,
    [price]            MONEY        NULL,
    [rev_code]         VARCHAR (4)  NULL,
    [type]             VARCHAR (4)  NULL,
    [modi]             VARCHAR (2)  NULL,
    [billcode]         VARCHAR (7)  NULL,
    [date_included]    DATETIME     NOT NULL
);

