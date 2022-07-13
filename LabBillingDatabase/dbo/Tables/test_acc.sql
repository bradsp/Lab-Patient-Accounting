CREATE TABLE [dbo].[test_acc] (
    [account] VARCHAR (15) NOT NULL,
    [code]    VARCHAR (5)  NULL,
    CONSTRAINT [PK_test_acc] PRIMARY KEY CLUSTERED ([account] ASC) WITH (FILLFACTOR = 90)
);

