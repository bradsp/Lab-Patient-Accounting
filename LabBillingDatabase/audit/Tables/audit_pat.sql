CREATE TABLE [audit].[audit_pat] (
    [pat_rowguid]               UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [deleted]                   BIT              NOT NULL,
    [account]                   VARCHAR (15)     NOT NULL,
    [ssn]                       VARCHAR (11)     NULL,
    [pat_addr1]                 VARCHAR (40)     NULL,
    [pat_addr2]                 VARCHAR (40)     NULL,
    [city_st_zip]               VARCHAR (40)     NULL,
    [dob_yyyy]                  DATETIME         NULL,
    [sex]                       VARCHAR (1)      NULL,
    [relation]                  VARCHAR (2)      NULL,
    [guarantor]                 VARCHAR (40)     NULL,
    [guar_addr]                 VARCHAR (40)     NULL,
    [g_city_st]                 VARCHAR (50)     NULL,
    [pat_marital]               VARCHAR (1)      NULL,
    [icd9_1]                    VARCHAR (7)      NULL,
    [icd9_2]                    VARCHAR (7)      NULL,
    [icd9_3]                    VARCHAR (7)      NULL,
    [icd9_4]                    VARCHAR (7)      NULL,
    [icd9_5]                    VARCHAR (7)      NULL,
    [icd9_6]                    VARCHAR (7)      NULL,
    [icd9_7]                    VARCHAR (7)      NULL,
    [icd9_8]                    VARCHAR (7)      NULL,
    [icd9_9]                    VARCHAR (7)      NULL,
    [pc_code]                   INT              NULL,
    [mailer]                    VARCHAR (1)      NULL,
    [first_dm]                  DATETIME         NULL,
    [last_dm]                   DATETIME         NULL,
    [min_amt]                   MONEY            NULL,
    [phy_id]                    VARCHAR (15)     NULL,
    [dbill_date]                DATETIME         NULL,
    [ub_date]                   DATETIME         NULL,
    [h1500_date]                DATETIME         NULL,
    [colltr_date]               DATETIME         NULL,
    [baddebt_date]              DATETIME         NULL,
    [batch_date]                DATETIME         NULL,
    [guar_phone]                VARCHAR (13)     NULL,
    [bd_list_date]              DATETIME         NULL,
    [ebill_batch_date]          DATETIME         NULL,
    [ebill_batch_1500]          DATETIME         NULL,
    [e_ub_demand]               BIT              CONSTRAINT [DF_audit_pat_e_ub_demand_1] DEFAULT ((0)) NOT NULL,
    [e_ub_demand_date]          DATETIME         NULL,
    [claimsnet_1500_batch_date] DATETIME         NULL,
    [claimsnet_ub_batch_date]   DATETIME         NULL,
    [mod_date]                  DATETIME         NOT NULL,
    [mod_user]                  VARCHAR (50)     NULL,
    [mod_prg]                   VARCHAR (50)     NULL,
    [mod_host]                  VARCHAR (50)     NULL,
    [uid]                       BIGINT           IDENTITY (1, 1) NOT NULL,
    [hne_epi_number]            VARCHAR (50)     NULL,
    [ssi_batch]                 VARCHAR (50)     NULL,
    [location]                  VARCHAR (50)     NULL,
    [pat_email]                 VARCHAR (256)    NULL,
    CONSTRAINT [PK_audit_pat] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_pat_rowguid]
    ON [audit].[audit_pat]([pat_rowguid] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON);


GO
CREATE NONCLUSTERED INDEX [ix_account]
    ON [audit].[audit_pat]([account] ASC)
    INCLUDE([ub_date], [h1500_date]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_audit_pat_mod_date]
    ON [audit].[audit_pat]([mod_date] ASC)
    INCLUDE([ub_date], [h1500_date], [mailer], [first_dm], [last_dm], [mod_user]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_audit_pat]
    ON [audit].[audit_pat]([pat_rowguid] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_audit_pat_user_date]
    ON [audit].[audit_pat]([mod_user] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is the date we placed in the h1500 table for billing thru Claimsnet 07/04/2008 wdk/rgc', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_pat', @level2type = N'COLUMN', @level2name = N'claimsnet_1500_batch_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100915 Added for new demo tfxr file', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_pat', @level2type = N'COLUMN', @level2name = N'hne_epi_number';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20120510 Added for tracking ssi_batch runs', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_pat', @level2type = N'COLUMN', @level2name = N'ssi_batch';

