CREATE TABLE [dbo].[Totals_TPG] (
    [Status(F)]      NVARCHAR (255) NULL,
    [Processed Date] DATETIME       NULL,
    [Message]        NVARCHAR (255) NULL,
    [Account#]       NVARCHAR (255) NULL,
    [Status]         NVARCHAR (255) NULL,
    [Trans_Date]     DATETIME       NULL,
    [Mod_Date]       DATETIME       NULL,
    [pay_no]         FLOAT (53)     NULL,
    [date_rec]       DATETIME       NULL,
    [amt_paid]       FLOAT (53)     NULL,
    [source]         NVARCHAR (255) NULL,
    [uid]            BIGINT         IDENTITY (1, 1) NOT NULL
);

