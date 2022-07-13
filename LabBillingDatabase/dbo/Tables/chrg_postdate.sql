CREATE TABLE [dbo].[chrg_postdate] (
    [account]         VARCHAR (15)    NULL,
    [fin_code]        VARCHAR (10)    NULL,
    [ins_code]        VARCHAR (10)    NULL,
    [accType]         INT             NULL,
    [cli_mnem]        VARCHAR (10)    NULL,
    [date_of_service] DATETIME        NULL,
    [total_charges]   NUMERIC (18, 2) NULL,
    [chrg_post_date]  DATETIME        NULL,
    [amt_paid]        NUMERIC (18, 2) NULL,
    [contractual]     NUMERIC (18, 2) NULL,
    [date_of_chk]     DATETIME        NULL,
    [write_off]       NUMERIC (18, 2) NULL,
    [write_off_code]  VARCHAR (4)     NULL,
    [chk_post_date]   DATETIME        NULL
);

