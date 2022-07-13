CREATE TABLE [dbo].[ssi_remittance] (
    [file_date]           DATETIME        NOT NULL,
    [prov_no]             VARCHAR (15)    NULL,
    [last_name]           VARCHAR (20)    NULL,
    [first_name]          VARCHAR (10)    NULL,
    [mid_init]            VARCHAR (1)     NULL,
    [icn]                 VARCHAR (15)    NOT NULL,
    [clm]                 VARCHAR (6)     NULL,
    [claim_status]        VARCHAR (11)    NULL,
    [pcn]                 VARCHAR (17)    NULL,
    [hic]                 VARCHAR (30)    NULL,
    [tob]                 VARCHAR (3)     NULL,
    [svc_date_from]       DATETIME        NULL,
    [svc_date_thru]       DATETIME        NULL,
    [reported_charges]    MONEY           NULL,
    [non_cov_charges]     MONEY           NULL,
    [denied_charges]      MONEY           NULL,
    [net_payable_charges] MONEY           NULL,
    [deductible]          MONEY           NULL,
    [co_insurance]        MONEY           NULL,
    [reim_rate]           DECIMAL (18, 2) NULL,
    [contractual]         MONEY           NULL,
    [net_reimbursement]   MONEY           NULL,
    [fiscal_period_end]   DATETIME        NULL,
    [carrier]             VARCHAR (20)    NULL,
    [mod_date]            DATETIME        CONSTRAINT [DF_ssi_remittance_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]            VARCHAR (50)    CONSTRAINT [DF_ssi_remittance_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_prg]             VARCHAR (50)    CONSTRAINT [DF_ssi_remittance_mod_prg] DEFAULT (app_name()) NULL,
    [mod_host]            VARCHAR (50)    CONSTRAINT [DF_ssi_remittance_mod_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_ssi_remittance] PRIMARY KEY CLUSTERED ([file_date] ASC, [icn] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_pcn]
    ON [dbo].[ssi_remittance]([pcn] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 10/2/2006 to track changes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ssi_remittance', @level2type = N'COLUMN', @level2name = N'mod_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 10/2/2006 to track changes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ssi_remittance', @level2type = N'COLUMN', @level2name = N'mod_user';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 10/2/2006 to track changes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ssi_remittance', @level2type = N'COLUMN', @level2name = N'mod_prg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 10/2/2006 to track changes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ssi_remittance', @level2type = N'COLUMN', @level2name = N'mod_host';

