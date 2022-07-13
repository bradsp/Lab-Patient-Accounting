CREATE TABLE [dbo].[ExportTableData] (
    [id]               INT             NULL,
    [dateTrans]        DATETIME        NULL,
    [pat_name]         VARCHAR (100)   NULL,
    [cli_name]         VARCHAR (40)    NULL,
    [account]          VARCHAR (15)    NULL,
    [balPrev]          NUMERIC (18, 2) NULL,
    [address1]         VARCHAR (40)    NULL,
    [csz]              VARCHAR (40)    NULL,
    [guarantor]        VARCHAR (40)    NULL,
    [mailer]           VARCHAR (1)     NULL,
    [last_dm]          DATETIME        NULL,
    [total_charges]    NUMERIC (18, 2) NULL,
    [last_billed_date] DATETIME        NULL,
    [pay_before_ldm]   NUMERIC (18, 2) NULL,
    [pay_after_ldm]    NUMERIC (18, 2) NULL,
    [balCurrent]       NUMERIC (18, 2) NULL,
    [file_date]        DATETIME        NULL
);

