CREATE TABLE [dbo].[Temp_GlobalBilling] (
    [colClient]      VARCHAR (10)     NULL,
    [colAcc]         VARCHAR (15)     NULL,
    [colOrigChrgNum] NUMERIC (38)     NULL,
    [colCDM]         VARCHAR (7)      NULL,
    [colCPT]         VARCHAR (5)      NULL,
    [colQty]         INT              NULL,
    [colChrgAmt]     NUMERIC (18, 2)  NULL,
    [colDOS]         DATETIME         NULL,
    [colDateEntered] DATETIME         NULL,
    [colPrice]       NUMERIC (18, 2)  NULL,
    [colError]       VARCHAR (50)     NULL,
    [rowguid]        UNIQUEIDENTIFIER CONSTRAINT [DF_Temp_GlobalBilling_rowguid] DEFAULT (newid()) NOT NULL,
    [colNewChrgNum]  NUMERIC (38)     NULL,
    [colSite]        VARCHAR (50)     NULL
);

