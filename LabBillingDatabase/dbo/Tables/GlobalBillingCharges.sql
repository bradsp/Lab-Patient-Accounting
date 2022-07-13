CREATE TABLE [dbo].[GlobalBillingCharges] (
    [colClient]      VARCHAR (10)    NULL,
    [colAcc]         VARCHAR (15)    NULL,
    [colChrgNum]     NUMERIC (38)    NULL,
    [colCDM]         VARCHAR (7)     NULL,
    [colCPT]         VARCHAR (5)     NULL,
    [colQty]         INT             NULL,
    [colChrgAmt]     NUMERIC (18, 2) NULL,
    [colDOS]         DATETIME        NULL,
    [colDateEntered] DATETIME        NULL,
    [colFinCode]     VARCHAR (10)    NULL,
    [colClType]      INT             NULL,
    [colError]       VARCHAR (50)    NULL
);

