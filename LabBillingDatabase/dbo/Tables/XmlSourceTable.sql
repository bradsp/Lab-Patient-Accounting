CREATE TABLE [dbo].[XmlSourceTable] (
    [doc_data]    VARCHAR (MAX) NOT NULL,
    [import_file] VARCHAR (50)  NOT NULL,
    [processed]   BIT           CONSTRAINT [DF_XmlSourceTable_processed] DEFAULT ((0)) NOT NULL,
    [accounts]    SMALLINT      NULL,
    [charges]     SMALLINT      NOT NULL,
    [mod_date]    DATETIME      CONSTRAINT [DF_XmlSourceTable_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]     VARCHAR (50)  CONSTRAINT [DF_XmlSourceTable_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]    VARCHAR (50)  CONSTRAINT [DF_XmlSourceTable_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]    VARCHAR (50)  CONSTRAINT [DF_XmlSourceTable_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [uid]         INT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK__XmlSourceTable__74BA0D0B] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_XmlSourceTable_processed]
    ON [dbo].[XmlSourceTable]([processed] ASC) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David
-- Create date: 03/22/2014
-- Description:	Convert the varchar to xml formatted data removing the '&' and second set of 'quote' on the encounter id
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_CONVERT_XML] 
   ON  [dbo].[XmlSourceTable] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
   BEGIN TRY
	DECLARE @data VARCHAR(MAX)
	SET @data = (SELECT i.doc_data FROM INSERTED i)
	SET @data = REPLACE(REPLACE(REPLACE(@data,'&quot;',''''),'"',''),'&','&amp;')
				
	DECLARE @xml XML
	SET @xml = CONVERT(XML,@data)
	DECLARE @nAccCount INT
	DECLARE @nCharges INT
	
	SET @nAccCount = (SELECT @xml.value('count(data(distinct-values(/postingbatch/encounter/@ID)))','int') AS [count data distinct encounter ids])
	SET @nCharges =  (SELECT @xml.value('count(data(/postingbatch/encounter/charge/account))','int'))
	
	UPDATE dbo.XmlSourceTable
	SET 
		doc_data = CONVERT(VARCHAR(max), @xml)
		, accounts = @nAccCount
		, charges = @nCharges
		, mod_prg = 'TSQL'
		, mod_user = 'TRIGGER_CONVERT_XML'
		FROM dbo.XmlSourceTable
		INNER JOIN INSERTED i ON i.import_file = dbo.XmlSourceTable.import_file
	END TRY
	BEGIN CATCH 
	
	declare @sub varchar(256)
	set @sub = 'PostingBatch as of ' + convert(varchar(10),getdate(),101)
	DECLARE @body VARCHAR(max)
	SET @body = (SELECT convert(varchar(1),@@ROWCOUNT) + CONVERT(VARCHAR(10),@@ERROR) + ERROR_MESSAGE())
	exec msdb.dbo.sp_send_dbmail 
	    @profile_name = 'Outlook',
	    @recipients = 'david.kelly@wth.org',
	    --@recipients = 'carol.sellars@wth.org;jan.smith@wth.org',
	    --@copy_recipients=N'bradley.powers@wth.org; david.kelly@wth.org',
	    --@blind_copy_recipients = 'david.kelly@wth.org', 
	    @subject = @sub, 
	    @body =  @body,
	    @body_format = 'TEXT', --HTML',
	    @append_query_error = 1
--	    --@importance = 'importance',
--	    --@sensitivity = 'sensitivity' ,
--	    --@file_attachments =  'attachment [ ; ...n ]' ,
--	    --@query =  'query' ,
--	    --[ , [ @execute_query_database = ] 'execute_query_database' ]
--	    --[ , [ @attach_query_result_as_file = ] attach_query_result_as_file ]
--	    --[ , [ @query_attachment_filename = ] query_attachment_filename ]
--	    --[ , [ @query_result_header = ] query_result_header ]
--	    --[ , [ @query_result_width = ] query_result_width ]
--	    --[ , [ @query_result_separator = ] 'query_result_separator' ]
--	    --[ , [ @exclude_query_output = ] exclude_query_output ]
--	    --[ , [ @query_no_truncate = ] query_no_truncate ]
--	    --[ , [ @mailitem_id = ] mailitem_id ] [ OUTPUT ]
--	 
--	
--		
	END CATCH

END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Trigger will add the number of records in the file after the insert of the xml.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'XmlSourceTable', @level2type = N'COLUMN', @level2name = N'charges';

