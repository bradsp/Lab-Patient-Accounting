CREATE TABLE [dbo].[data_quest_360] (
    [deleted]           BIT              CONSTRAINT [DF_data_quest_360_deleted] DEFAULT ((0)) NOT NULL,
    [Patient]           VARCHAR (255)    NULL,
    [patid]             VARCHAR (256)    NOT NULL,
    [html_doc]          VARCHAR (MAX)    NULL,
    [account]           VARCHAR (15)     NOT NULL,
    [date_of_service]   DATETIME         NULL,
    [pre360_error]      BIT              CONSTRAINT [DF_data_quest_360_has_pre360_error] DEFAULT ((0)) NOT NULL,
    [bill_code_error]   BIT              CONSTRAINT [DF_data_quest_360_bill_code_error] DEFAULT ((0)) NOT NULL,
    [entered]           BIT              CONSTRAINT [DF_data_quest_360_entered] DEFAULT ((0)) NOT NULL,
    [charges_entered]   BIT              CONSTRAINT [DF_data_quest_360_charges_entered] DEFAULT ((0)) NOT NULL,
    [bill_type]         VARCHAR (50)     NULL,
    [mod_date]          DATETIME         CONSTRAINT [DF_data_quest_360_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]          VARCHAR (50)     CONSTRAINT [DF_data_quest_360_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]           VARCHAR (50)     CONSTRAINT [DF_data_quest_360_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]          VARCHAR (50)     CONSTRAINT [DF_data_quest_360_mod_host] DEFAULT (host_name()) NOT NULL,
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_data_quest_360_rowguid] DEFAULT (newid()) NOT NULL,
    [entry_date]        DATETIME         NULL,
    [uid]               INT              IDENTITY (1, 1) NOT NULL,
    [emailed]           BIT              CONSTRAINT [df_col_emailed] DEFAULT ((0)) NULL,
    [transmission_date] DATETIME         NULL,
    CONSTRAINT [PK_data_quest_360] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_data_quest_360_account]
    ON [dbo].[data_quest_360]([account] ASC, [bill_type] ASC, [patid] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_data_quest_360_error]
    ON [dbo].[data_quest_360]([pre360_error] ASC, [bill_code_error] ASC, [entered] ASC, [bill_type] ASC)
    INCLUDE([date_of_service]) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David
-- Create date: 09/09/2014
-- Description:	put the patient name in the table for sending cancel emails to quest
-- =============================================
CREATE TRIGGER dbo.TRIGGER_ADD_PAT_NAME 
   ON  dbo.data_quest_360 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE dbo.data_quest_360
	SET Patient = dbo.acc.pat_name
	FROM dbo.data_quest_360
	INNER JOIN acc ON dbo.acc.account = dbo.data_quest_360.account
	INNER JOIN INSERTED	i ON i.account = dbo.acc.account
	WHERE dbo.data_quest_360.account = i.account
    -- Insert statements for trigger here

END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20121206 added ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_quest_360', @level2type = N'COLUMN', @level2name = N'date_of_service';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Demographics Entered via INTERFACE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_quest_360', @level2type = N'COLUMN', @level2name = N'entered';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Charges Entered via Viewer360', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_quest_360', @level2type = N'COLUMN', @level2name = N'charges_entered';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'should be one of two types "QUEST" -- they perform the test and we send a fax sheet with demographics or  "MCL" -- tests performed here and billed only to Quest for Bluecare.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_quest_360', @level2type = N'COLUMN', @level2name = N'bill_type';

