CREATE TABLE [audit].[audit_bad_debt] (
    [baddebt_rowguid]   UNIQUEIDENTIFIER NULL,
    [deleted]           BIT              NULL,
    [debtor_last_name]  VARCHAR (20)     NULL,
    [debtor_first_name] VARCHAR (15)     NULL,
    [st_addr_1]         VARCHAR (25)     NULL,
    [st_addr_2]         VARCHAR (25)     NULL,
    [city]              VARCHAR (18)     NULL,
    [state_zip]         VARCHAR (15)     NULL,
    [spouse]            VARCHAR (15)     NULL,
    [phone]             VARCHAR (12)     NULL,
    [soc_security]      VARCHAR (10)     NULL,
    [license_number]    VARCHAR (20)     NULL,
    [employment]        VARCHAR (35)     NULL,
    [remarks]           VARCHAR (35)     NULL,
    [account_no]        VARCHAR (25)     NOT NULL,
    [patient_name]      VARCHAR (20)     NULL,
    [remarks2]          VARCHAR (35)     NULL,
    [misc]              VARCHAR (29)     NULL,
    [service_date]      DATETIME         NULL,
    [payment_date]      DATETIME         NULL,
    [balance]           MONEY            NULL,
    [date_entered]      DATETIME         NOT NULL,
    [date_sent]         DATETIME         NULL,
    [mod_date]          DATETIME         NULL,
    [mod_user]          VARCHAR (50)     NULL,
    [mod_host]          VARCHAR (50)     NULL,
    [mod_prg]           VARCHAR (50)     NULL,
    [uid]               BIGINT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_audit_bad_debt] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'primary key in audit because we can regenerate the numbers if necessary', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_bad_debt', @level2type = N'COLUMN', @level2name = N'uid';

