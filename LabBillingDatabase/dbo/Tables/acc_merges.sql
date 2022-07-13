CREATE TABLE [dbo].[acc_merges] (
    [account]      VARCHAR (15) NOT NULL,
    [dup_acc]      VARCHAR (15) NOT NULL,
    [pat_ssn]      VARCHAR (11) NULL,
    [pat_mri]      VARCHAR (50) NULL,
    [service_date] DATETIME     NULL,
    [fin_code]     VARCHAR (10) NULL,
    [xml_info]     XML          NULL,
    [xml_file]     VARCHAR (50) NULL,
    [mod_date]     DATETIME     CONSTRAINT [DF_acc_merges_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]      VARCHAR (50) CONSTRAINT [DF_acc_merges_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_user]     VARCHAR (50) CONSTRAINT [DF_acc_merges_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_host]     VARCHAR (50) CONSTRAINT [DF_acc_merges_mod_host] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_acc_merges] PRIMARY KEY CLUSTERED ([account] ASC, [dup_acc] ASC) WITH (FILLFACTOR = 90)
);


GO
-- =============================================
-- Author:		David
-- Create date: 12/17/2013
-- Description:	Remove the leading zeros from accounts
-- =============================================
create  TRIGGER dbo.TRIGGER_TRIM_ACCOUNT 
   ON  dbo.acc_merges 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	update acc_merges
	set account = dbo.AccountTrim(i.account)
	from acc_merges a1
	inner join inserted i on i.account = a1.account	
	


END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404021 to track duplicates accounts', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc_merges', @level2type = N'COLUMN', @level2name = N'pat_ssn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404021 to track duplicates accounts', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc_merges', @level2type = N'COLUMN', @level2name = N'pat_mri';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404021 to track duplicates accounts', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc_merges', @level2type = N'COLUMN', @level2name = N'service_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404021 to track duplicates accounts', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc_merges', @level2type = N'COLUMN', @level2name = N'fin_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404021 to track duplicates accounts', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'acc_merges', @level2type = N'COLUMN', @level2name = N'xml_file';

