CREATE TABLE [dbo].[month_end_del] (
    [account]   VARCHAR (15) NOT NULL,
    [datestamp] DATETIME     NOT NULL,
    [balance]   MONEY        NULL,
    [fin_code]  VARCHAR (10) NULL,
    [ins_code]  VARCHAR (10) NULL,
    CONSTRAINT [PK_month_end] PRIMARY KEY CLUSTERED ([account] ASC, [datestamp] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100712 Added to collect fin_code on last day of month', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'month_end_del', @level2type = N'COLUMN', @level2name = N'fin_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100712 Added to collect ins_code on last day of month', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'month_end_del', @level2type = N'COLUMN', @level2name = N'ins_code';

