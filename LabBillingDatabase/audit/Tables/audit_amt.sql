CREATE TABLE [audit].[audit_amt] (
    [audit_chrg_num]     NUMERIC (15) NOT NULL,
    [cpt4]               VARCHAR (5)  NULL,
    [type]               VARCHAR (6)  NULL,
    [amount]             MONEY        NULL,
    [mod_date]           DATETIME     CONSTRAINT [DF_audit_amt_mod_date_3__12] DEFAULT (getdate()) NULL,
    [mod_user]           VARCHAR (50) CONSTRAINT [DF_audit_amt_mod_user_1__10] DEFAULT (suser_sname()) NULL,
    [mod_prg]            VARCHAR (50) CONSTRAINT [DF_audit_amt_mod_prg_4__12] DEFAULT (app_name()) NULL,
    [mod_host]           VARCHAR (50) CONSTRAINT [DF_audit_amt_mod_host] DEFAULT (host_name()) NULL,
    [mod_indicator]      VARCHAR (50) NULL,
    [deleted]            BIT          CONSTRAINT [DF_audit_amt_deleted_2__12] DEFAULT ((0)) NOT NULL,
    [amt_uri]            NUMERIC (15) NULL,
    [modi]               VARCHAR (5)  NULL,
    [revcode]            VARCHAR (5)  NULL,
    [modi2]              VARCHAR (5)  NULL,
    [uri]                NUMERIC (15) IDENTITY (1, 1) NOT NULL,
    [diagnosis_code_ptr] VARCHAR (50) NULL,
    [pointer_set]        BIT          NULL,
    [account]            VARCHAR (15) NULL,
    [bill_method]        VARCHAR (50) NULL,
    CONSTRAINT [PK_audit_amt] PRIMARY KEY CLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [audit_chrg_num]
    ON [audit].[audit_amt]([audit_chrg_num] ASC, [mod_date] ASC, [mod_indicator] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'12/08/2008 wdk added for audit tracking purposes maintained through the trigger', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_amt', @level2type = N'COLUMN', @level2name = N'mod_host';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'12/08/2008 wdk
IO for Insert (initial record)
DUD for Deleted (last record as it appeared with the mod data for who deleted it)
DUI for deleted (last record as it appeared in the table only if the record has never been in the audit table before) 
MUD for Modified Update Deleted as it appeared before it was updated
MUI for Modified Update Inserted this is the last change made
', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_amt', @level2type = N'COLUMN', @level2name = N'mod_indicator';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'12/08/2008 wdk this is the uri from the amount table it is not the identity column on this table, it was the identity on the amt table', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_amt', @level2type = N'COLUMN', @level2name = N'amt_uri';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'12/08/2008 wdk added as the uri for this table only. The amt''s uri is stored in the amt_uri field of this table.', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_amt', @level2type = N'COLUMN', @level2name = N'uri';

