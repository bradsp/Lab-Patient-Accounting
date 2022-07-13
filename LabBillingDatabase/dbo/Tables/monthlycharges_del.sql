CREATE TABLE [dbo].[monthlycharges_del] (
    [rowid]         INT          IDENTITY (1, 1) NOT NULL,
    [account]       VARCHAR (15) NULL,
    [amt]           MONEY        DEFAULT ((0)) NULL,
    [nYear]         INT          NULL,
    [nMonth]        INT          NULL,
    [fincodetotal]  MONEY        DEFAULT ((0)) NULL,
    [finCode]       VARCHAR (10) NULL,
    [finType]       VARCHAR (10) NULL,
    [audit_moddate] DATETIME     NULL,
    [chrg_moddate]  DATETIME     NULL,
    [service_date]  DATETIME     NULL,
    [huhdate]       DATETIME     NULL,
    CONSTRAINT [PK_MC_ROWID] PRIMARY KEY NONCLUSTERED ([rowid] ASC) WITH (FILLFACTOR = 90)
);

