CREATE TABLE [audit].[audit_client] (
    [deleted]           BIT           NULL,
    [audit_cli_mnem]    VARCHAR (10)  NULL,
    [cli_nme]           VARCHAR (40)  NULL,
    [addr_1]            VARCHAR (40)  NULL,
    [addr_2]            VARCHAR (40)  NULL,
    [city]              VARCHAR (30)  NULL,
    [st]                VARCHAR (2)   NULL,
    [zip]               VARCHAR (10)  NULL,
    [phone]             VARCHAR (40)  NULL,
    [fax]               VARCHAR (15)  NULL,
    [contact]           VARCHAR (MAX) NULL,
    [comment]           VARCHAR (MAX) NULL,
    [prn_cpt4]          BIT           NULL,
    [per_disc]          REAL          NULL,
    [date_ord]          BIT           NULL,
    [print_cc]          BIT           NULL,
    [bill_at_disc]      BIT           NULL,
    [do_not_bill]       BIT           NULL,
    [type]              INT           NULL,
    [last_invoice]      VARCHAR (15)  NULL,
    [last_invoice_date] DATETIME      NULL,
    [last_discount]     VARCHAR (15)  NULL,
    [mod_date]          DATETIME      NULL,
    [mod_user]          VARCHAR (50)  NULL,
    [mod_prg]           VARCHAR (50)  NULL,
    [mod_host]          VARCHAR (50)  NULL,
    [mro_name]          VARCHAR (40)  NULL,
    [mro_addr1]         VARCHAR (40)  NULL,
    [mro_addr2]         VARCHAR (40)  NULL,
    [mro_city]          VARCHAR (30)  NULL,
    [mro_st]            VARCHAR (2)   NULL,
    [mro_zip]           VARCHAR (10)  NULL,
    [prn_loc]           VARCHAR (1)   NULL,
    [route]             VARCHAR (10)  NULL,
    [county]            VARCHAR (30)  NULL,
    [email]             VARCHAR (40)  NULL,
    [late_notice]       VARCHAR (1)   NULL,
    [late_notice_date]  DATETIME      NULL,
    [statsFacility]     VARCHAR (15)  NULL,
    [fee_schedule]      TINYINT       NULL,
    [commission]        BIT           NULL,
    [client_class]      VARCHAR (5)   NULL,
    [client_maint_rep]  VARCHAR (30)  NULL,
    [client_sales_rep]  VARCHAR (30)  NULL,
    [mod_indicator]     VARCHAR (10)  NULL,
    [bill_pc_charges]   VARCHAR (4)   NULL
);


GO
CREATE CLUSTERED INDEX [IX_audit_client]
    ON [audit].[audit_client]([audit_cli_mnem] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140505 to determine if the client is billed pc charges ''YES'' or ''NO'',''ONLY'' ', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_client', @level2type = N'COLUMN', @level2name = N'bill_pc_charges';

