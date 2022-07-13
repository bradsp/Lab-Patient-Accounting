CREATE TABLE [dbo].[tempDaily] (
    [account]  VARCHAR (15)    NOT NULL,
    [startBal] NUMERIC (18, 2) NULL,
    [charges]  NUMERIC (18, 2) NULL,
    [payments] NUMERIC (18, 2) NULL,
    [endBal]   NUMERIC (18, 2) NULL,
    [error]    AS              (((isnull([startBal],(0))+isnull([charges],(0)))-isnull([payments],(0)))-isnull([endBal],(0))),
    PRIMARY KEY CLUSTERED ([account] ASC) WITH (FILLFACTOR = 90)
);

