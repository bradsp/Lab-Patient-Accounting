CREATE TABLE [audit].[audit_aging_history] (
    [account]       VARCHAR (15) NOT NULL,
    [datestamp]     DATETIME     NOT NULL,
    [balance]       MONEY        NULL,
    [uid]           BIGINT       IDENTITY (1, 1) NOT NULL,
    [mod_indicator] VARCHAR (10) CONSTRAINT [DF_audit_aging_history_mod_indicator] DEFAULT ('N') NOT NULL,
    [mod_date]      DATETIME     CONSTRAINT [DF_aah_mod_date_1__1] DEFAULT (getdate()) NOT NULL,
    [mod_user]      VARCHAR (50) CONSTRAINT [DF_aah_mod_user_1__2] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_prg]       VARCHAR (50) CONSTRAINT [DF_aah_mod_prg_1__3] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_host]      VARCHAR (50) CONSTRAINT [DF_aah_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [ins_code]      VARCHAR (10) NULL,
    [fin_code]      VARCHAR (10) NULL,
    CONSTRAINT [PK_audit_aging_history] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'12/03/2008 wdk default to ''N'' nothing provided. Ideal values from trigger will be I(nserted original record), MUD(ModUpdate from Deleted table), MUI(ModUpdate from the Inserted table, D(eleted) .
Tracking. 
1.  I should be the first record for anthing put into the table
2.  MUD should be the second record for anthing that is updated
3.  MUI should be the third record for anthing that is updated
2&3 should repeat as often as there are changes
4. D should be the last entry when a record is deleted.
', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_aging_history', @level2type = N'COLUMN', @level2name = N'mod_indicator';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100712 Added to collect ins_code on last day of month', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_aging_history', @level2type = N'COLUMN', @level2name = N'ins_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100712 Added to collect fin_code on last day of month', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_aging_history', @level2type = N'COLUMN', @level2name = N'fin_code';

