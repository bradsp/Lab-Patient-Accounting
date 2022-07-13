CREATE TABLE [audit].[audit_cpt4_3] (
    [audit_rowguid] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [deleted]       BIT              NOT NULL,
    [cdm]           VARCHAR (7)      NOT NULL,
    [link]          INT              NOT NULL,
    [cpt4]          VARCHAR (5)      NULL,
    [descript]      VARCHAR (50)     NULL,
    [mprice]        MONEY            NULL,
    [cprice]        MONEY            NULL,
    [zprice]        MONEY            NULL,
    [rev_code]      VARCHAR (4)      NULL,
    [type]          VARCHAR (4)      NULL,
    [modi]          VARCHAR (2)      NULL,
    [billcode]      VARCHAR (7)      NULL,
    [mod_date]      DATETIME         NULL,
    [mod_user]      VARCHAR (50)     NULL,
    [mod_prg]       VARCHAR (50)     NULL,
    [mod_host]      VARCHAR (50)     NULL,
    [uid]           BIGINT           IDENTITY (1, 1) NOT NULL,
    [cost]          DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK_audit_cpt4_3] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);

