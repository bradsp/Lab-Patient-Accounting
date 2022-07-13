CREATE TABLE [audit].[audit_cdm] (
    [deleted]           BIT             NULL,
    [cdm]               VARCHAR (7)     NULL,
    [descript]          VARCHAR (50)    NULL,
    [mtype]             VARCHAR (10)    NULL,
    [m_pa_amt]          MONEY           NULL,
    [ctype]             VARCHAR (10)    NULL,
    [c_pa_amt]          MONEY           NULL,
    [ztype]             VARCHAR (10)    NULL,
    [z_pa_amt]          MONEY           NULL,
    [orderable]         INT             NULL,
    [cbill_detail]      INT             NULL,
    [comments]          VARCHAR (MAX)   NULL,
    [mnem]              VARCHAR (15)    NULL,
    [cost]              NUMERIC (18, 2) NULL,
    [ref_lab_id]        VARCHAR (50)    NULL,
    [ref_lab_bill_code] VARCHAR (50)    NULL,
    [ref_lab_payment]   NUMERIC (18, 2) NULL,
    [mod_date]          DATETIME        NULL,
    [mod_user]          VARCHAR (50)    NULL,
    [mod_prg]           VARCHAR (50)    NULL,
    [mod_host]          VARCHAR (50)    NULL,
    [uid]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [mod_indicator]     VARCHAR (50)    CONSTRAINT [DF_audit_cdm_mod_indicator] DEFAULT ('ERR') NOT NULL,
    CONSTRAINT [PK_audit_cdm] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20121017 added', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_cdm', @level2type = N'COLUMN', @level2name = N'ref_lab_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20121017 added', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_cdm', @level2type = N'COLUMN', @level2name = N'ref_lab_bill_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20121017 added', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_cdm', @level2type = N'COLUMN', @level2name = N'ref_lab_payment';

