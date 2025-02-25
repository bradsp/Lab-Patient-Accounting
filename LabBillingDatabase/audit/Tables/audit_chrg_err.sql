﻿CREATE TABLE [audit].[audit_chrg_err] (
    [account]         VARCHAR (15)  NOT NULL,
    [pat_name]        VARCHAR (40)  NULL,
    [cl_mnem]         VARCHAR (10)  NULL,
    [fin_code]        VARCHAR (1)   NULL,
    [cdm]             VARCHAR (7)   NULL,
    [cpt4]            VARCHAR (5)   NULL,
    [amount]          MONEY         NULL,
    [trans_date]      DATETIME      NULL,
    [service_date]    DATETIME      NULL,
    [qty]             INT           NULL,
    [type]            VARCHAR (6)   NULL,
    [error]           VARCHAR (100) NULL,
    [uri]             NUMERIC (15)  NOT NULL,
    [deleted]         BIT           CONSTRAINT [DF_audit_chrg_err_deleted_2__11] DEFAULT ((0)) NOT NULL,
    [mt_reqno]        VARCHAR (8)   NULL,
    [location]        VARCHAR (15)  NULL,
    [performing_site] VARCHAR (50)  NULL,
    [mod_date]        DATETIME      NULL,
    [mod_prg]         VARCHAR (50)  NULL,
    [mod_user]        VARCHAR (50)  NULL,
    [mod_host]        VARCHAR (50)  NULL
);


GO
CREATE CLUSTERED INDEX [IX_audit_chrg_err]
    ON [audit].[audit_chrg_err]([uri] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);

