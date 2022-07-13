CREATE TABLE [dbo].[cbill_hist] (
    [cl_mnem]          VARCHAR (10)    NOT NULL,
    [thru_date]        DATETIME        NOT NULL,
    [invoice]          VARCHAR (10)    NOT NULL,
    [bal_forward]      MONEY           CONSTRAINT [DF_cbill_hist_bal_forward1__14] DEFAULT ((0)) NULL,
    [total_chrg]       MONEY           CONSTRAINT [DF_cbill_hist_total_chrg_7__14] DEFAULT ((0)) NULL,
    [discount]         MONEY           CONSTRAINT [DF_cbill_hist_discount_3__14] DEFAULT ((0)) NULL,
    [balance_due]      MONEY           CONSTRAINT [DF_cbill_hist_balance_due2__14] DEFAULT ((0)) NULL,
    [payments]         MONEY           NULL,
    [true_balance_due] MONEY           NULL,
    [cbill_html]       VARCHAR (MAX)   NULL,
    [cbill_filestream] VARBINARY (MAX) NULL,
    [mod_user]         VARCHAR (50)    CONSTRAINT [DF_cbill_hist_mod_user_6__14] DEFAULT (suser_sname()) NULL,
    [mod_date]         DATETIME        CONSTRAINT [DF_cbill_hist_mod_date_4__14] DEFAULT (getdate()) NULL,
    [mod_prg]          VARCHAR (50)    CONSTRAINT [DF_cbill_hist_mod_prg_5__14] DEFAULT (app_name()) NULL,
    [mod_host]         VARCHAR (50)    CONSTRAINT [DF_cbill_hist_mod_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_cbill_hist_1__19] PRIMARY KEY CLUSTERED ([cl_mnem] ASC, [invoice] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [invoice_idx]
    ON [dbo].[cbill_hist]([invoice] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_thru_date]
    ON [dbo].[cbill_hist]([thru_date] ASC)
    INCLUDE([cl_mnem], [invoice], [bal_forward], [total_chrg], [discount], [balance_due], [payments], [true_balance_due], [mod_user], [mod_date], [mod_prg], [mod_host]) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20120813 added to hold the cbill text for printing this invoice', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cbill_hist', @level2type = N'COLUMN', @level2name = N'cbill_html';

