CREATE TABLE [audit].[audit_lmrp] (
    [cpt4]            VARCHAR (5)  NULL,
    [beg_icd9]        VARCHAR (7)  NULL,
    [end_icd9]        VARCHAR (7)  NULL,
    [payor]           VARCHAR (30) NULL,
    [fincode]         VARCHAR (10) NULL,
    [mod_user]        VARCHAR (20) NULL,
    [mod_date]        DATETIME     NULL,
    [mod_prg]         VARCHAR (20) NULL,
    [rb_date]         DATETIME     NULL,
    [lmrp]            VARCHAR (25) NULL,
    [lmrp2]           VARCHAR (25) NULL,
    [rb_date2]        DATETIME     NULL,
    [chk_for_bad]     INT          NULL,
    [ama_year]        VARCHAR (6)  NULL,
    [uid]             DECIMAL (18) NULL,
    [expiration_date] DATETIME     NULL,
    [audit_uid]       DECIMAL (18) IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_audit_lmrp] PRIMARY KEY CLUSTERED ([audit_uid] ASC) WITH (FILLFACTOR = 90)
);

