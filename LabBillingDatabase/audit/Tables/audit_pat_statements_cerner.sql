CREATE TABLE [audit].[audit_pat_statements_cerner] (
    [statement_text] VARCHAR (MAX) NOT NULL,
    [mod_date]       DATETIME      CONSTRAINT [DF_audit_pat_statements_cerner_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]       VARCHAR (50)  CONSTRAINT [DF_audit_pat_statements_cerner_mod_user] DEFAULT ('right(suser_sname,50)') NOT NULL,
    [mod_prg]        VARCHAR (50)  CONSTRAINT [DF_audit_pat_statements_cerner_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_host]       VARCHAR (50)  CONSTRAINT [DF_audit_pat_statements_cerner_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL
);

