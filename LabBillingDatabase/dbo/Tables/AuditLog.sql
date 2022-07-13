CREATE TABLE [dbo].[AuditLog] (
    [ID]        INT             IDENTITY (1, 1) NOT NULL,
    [Command]   NVARCHAR (1000) NULL,
    [PostTime]  NVARCHAR (24)   NULL,
    [HostName]  NVARCHAR (100)  NULL,
    [LoginName] NVARCHAR (100)  NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

