CREATE TABLE [dbo].[TransactionDetail] (
    [TransactionDetailID] INT          IDENTITY (1, 1) NOT NULL,
    [Date]                DATETIME     NULL,
    [AccountID]           VARCHAR (15) NULL,
    [Charges]             MONEY        NULL,
    [Payments]            MONEY        NULL,
    [AccountRunningTotal] MONEY        NULL,
    [AccountRunningCount] INT          NULL,
    [NCID]                INT          NULL,
    CONSTRAINT [PK_TransactionDetail_TransactionDetailID] PRIMARY KEY NONCLUSTERED ([TransactionDetailID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE CLUSTERED INDEX [IXC_Transaction_AccountID_Date_TransactionDetailID]
    ON [dbo].[TransactionDetail]([AccountID] ASC, [Date] ASC, [TransactionDetailID] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_Transaction_NCID]
    ON [dbo].[TransactionDetail]([NCID] DESC) WITH (FILLFACTOR = 90);

