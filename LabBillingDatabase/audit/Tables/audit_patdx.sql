CREATE TABLE [audit].[audit_patdx] (
    [account]    VARCHAR (15) NOT NULL,
    [dx_number]  INT          NOT NULL,
    [diagnosis]  VARCHAR (50) NOT NULL,
    [entry_date] DATETIME     NOT NULL,
    [mod_date]   DATETIME     CONSTRAINT [DF_audit_patpd_mod_date] DEFAULT (getdate()) NOT NULL,
    [uid]        BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_audit_patdx] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);

