CREATE TABLE [audit].[audit_dbill] (
    [dbill_rowguid] UNIQUEIDENTIFIER ROWGUIDCOL NULL,
    [deleted]       BIT              NOT NULL,
    [account]       VARCHAR (15)     NOT NULL,
    [pat_name]      VARCHAR (40)     NULL,
    [fin_code]      VARCHAR (1)      NULL,
    [trans_date]    DATETIME         NULL,
    [run_date]      DATETIME         NULL,
    [printed]       BIT              NOT NULL,
    [run_user]      VARCHAR (50)     NULL,
    [batch]         NUMERIC (10)     NULL,
    [text]          VARCHAR (8000)   NULL,
    [mod_user]      VARCHAR (50)     NOT NULL,
    [mod_prg]       VARCHAR (50)     NOT NULL,
    [mod_date]      DATETIME         NOT NULL,
    [mod_host]      VARCHAR (50)     NOT NULL,
    [uid]           BIGINT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_audit_dbill] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);

