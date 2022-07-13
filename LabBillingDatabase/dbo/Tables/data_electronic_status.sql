CREATE TABLE [dbo].[data_electronic_status] (
    [account]         VARCHAR (15)   NOT NULL,
    [status_type]     VARCHAR (50)   NOT NULL,
    [bill_type]       VARCHAR (4)    NULL,
    [provider]        VARCHAR (50)   NULL,
    [subid]           VARCHAR (50)   NULL,
    [tracer_no]       VARCHAR (50)   NULL,
    [amt_on_report]   MONEY          NULL,
    [status_on_claim] VARCHAR (50)   NULL,
    [status_text]     VARCHAR (8000) NULL,
    [status_date]     DATETIME       NULL,
    [status_batch]    VARCHAR (50)   NULL,
    [mod_date]        DATETIME       CONSTRAINT [DF_data_electronic_status_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]         VARCHAR (50)   CONSTRAINT [DF_data_electronic_status_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]        VARCHAR (50)   CONSTRAINT [DF_data_electronic_status_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]        VARCHAR (50)   CONSTRAINT [DF_data_electronic_status_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [uid]             BIGINT         IDENTITY (1, 1) NOT NULL
);


GO
CREATE CLUSTERED INDEX [IX_data_electronic_status]
    ON [dbo].[data_electronic_status]([account] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EDI from hospital to ClearPlus, PAY from ClearPlus to Payor, INS from payor back to ClearPlus/us', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_electronic_status', @level2type = N'COLUMN', @level2name = N'status_type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'should be "HOSP" or "PHY"', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_electronic_status', @level2type = N'COLUMN', @level2name = N'bill_type';

