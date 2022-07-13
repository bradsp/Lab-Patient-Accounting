CREATE TABLE [dbo].[pat_statements_cerner] (
    [statement_type]    VARCHAR (50)  NULL,
    [statement_type_id] VARCHAR (50)  NULL,
    [account]           VARCHAR (15)  NULL,
    [statement_text]    VARCHAR (MAX) NOT NULL,
    [batch_id]          VARCHAR (50)  NOT NULL
);


GO
-- =============================================
-- Author:		David
-- Create date: 05/05/2015
-- Description:	Copy the data to an audit table when deleted each month
-- =============================================
CREATE TRIGGER dbo.UPDATE_STATEMENTS 
   ON  dbo.pat_statements_cerner 
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
    INSERT INTO dbo.audit_pat_statements_cerner
    		(
    			statement_text ,
    			mod_date ,
    			mod_user ,
    			mod_prg ,
    			mod_host
    		)
--    VALUES	(
--    			'' , -- statement_text - varchar(max)
--    			'2015-05-05 15:15:10' , -- mod_date - datetime
--    			'' , -- mod_user - varchar(50)
--    			'' , -- mod_prg - varchar(50)
--    			''  -- mod_host - varchar(50)
--    		)
    SELECT d.statement_text 
		,GETDATE() 
		,RIGHT(SUSER_SNAME(),50)
		,COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
				'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50),'NO APP IDENTIFIED')
		,RIGHT (HOST_NAME(),50) 
    FROM deleted d
END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'should be BMSG, SMSG,AMSG etc', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat_statements_cerner', @level2type = N'COLUMN', @level2name = N'statement_type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'use the statement number or guarantor id depending on the statement type.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat_statements_cerner', @level2type = N'COLUMN', @level2name = N'statement_type_id';

