-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import_Location
	-- Add the parameters for the stored procedure here
	@file VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON; -- this allows the transaction code to work.

    -- Insert statements for procedure here
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[temp_track]') AND type in (N'U'))
	BEGIN
	CREATE TABLE [dbo].[temp_track](
		[comment] [varchar](8000) NOT NULL,
		[row_count] INT NULL,
		[error] [VARCHAR] (8000) NULL
		)
	END

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	DECLARE @startTime datetime
	DECLARE @endTime datetime
	DECLARE @modDate datetime
	SET @modDate = getdate()
	DECLARE @idoc int
	DECLARE @COUNT int
	SET @startTime = getdate()
	
	SET @file = COALESCE(NULLIF(@file,''),(select TOP(1) import_file FROM dbo.XmlSourceTable WHERE processed = 0 
		ORDER BY import_file) )

BEGIN TRY
BEGIN TRANSACTION
	DECLARE @data VARCHAR(MAX)
	SET @data = (select TOP(1) doc_data FROM dbo.XmlSourceTable WHERE import_file = @file
		ORDER BY import_file)
	DECLARE @doc xml
	SET @doc = (SELECT CONVERT(XML,@data))
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/@ID)))','int') AS [ACCOUNTS])


	EXEC sp_xml_preparedocument @idoc OUTPUT, @doc


	; WITH cteLoc
	AS
	(
		SELECT  DISTINCT 
		ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc,dbo.AccountTrim(xmlAccLocation.account))
		ORDER BY COALESCE(colAcc,dbo.AccountTrim(xmlAccLocation.account))) AS [rn],
		COALESCE(colAcc,dbo.AccountTrim(xmlAccLocation.account)) as [account],
		--dbo.AccountTrim(xmlAccLocation.account) AS [account],
		[status] AS [pt_type],
		CASE WHEN facility = 'BG' THEN 'BGH'
			 WHEN facility = 'MG' THEN 'COM'
			 WHEN facility = 'CG' THEN 'CGH'
			 ELSE xmlAccLocation.location END AS [location],
		facility
					
	from OPENXML (@idoc, '/postingbatch/encounter/charge',2)
	with (
			 account varchar(17)		'account',
			 [status] varchar(50)		'status',
			 location varchar(50)		'location',
			 facility varchar(10)		'facility'
			 
			
		 ) xmlAccLocation
	OUTER APPLY dbo.GetMasterAccount(xmlAccLocation.account)
	)
	INSERT INTO dbo.acc_location
			(
				account ,
				location ,
				pt_type ,
				mod_date ,
				mod_user ,
				mod_prg ,
				mod_host 
		
			)
	SELECT DISTINCT 
	cteLoc.account ,
			cteLoc.location, cteLoc.[pt_type] 
			, @modDate AS [mod_date]
			, RIGHT(SUSER_SNAME(),50) AS [mod_user]
			, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
				'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
			, RIGHT (HOST_NAME(),50)  AS [mod_host]
	FROM cteLoc
	LEFT OUTER JOIN dbo.acc_location ON dbo.acc_location.account = cteLoc.account
	WHERE dbo.acc_location.account IS NULL AND rn = 1

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('ACC_LOCATION Table rows added ' ,@COUNT, @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('16. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '16. ACC_LOCATION Table rows added ' + CONVERT(VARCHAR(10),@COUNT)
	EXEC sp_xml_removedocument @idoc
COMMIT TRANSACTION
END TRY		
	
BEGIN CATCH
    
	IF (XACT_STATE()) = -1
    BEGIN
        PRINT
            N'The transaction is in an uncommittable state.' +
            'Rolling back transaction.'
        ROLLBACK TRANSACTION;
    END;

    -- Test whether the transaction is committable.
    IF (XACT_STATE()) = 1
    BEGIN
        PRINT
            N'The transaction is committable.' +
            'Committing transaction.'
        COMMIT TRANSACTION;   
    END;
    INSERT INTO dbo.temp_track
    		( comment, error )
    VALUES	( 'ERROR: TRANSACTION ROLLED BACK', -- comment - varchar(8000)
    			ERROR_MESSAGE()  -- error - varchar(8000)
    			)
    
	INSERT INTO dbo.error_prg
            ( error_type,
            [app_name] ,
              app_module ,
              error ,
              mod_date ,
              mod_prg ,
              mod_user ,
              mod_host
            )
SELECT	'SQL',
            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 )AS [mod_prg],
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_MESSAGE() AS ErrorMessage,
            GETDATE(),
            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 ) AS ErrorProcedure,
            RIGHT(SUSER_SNAME(),50),
            RIGHT (HOST_NAME(),50);
        
END CATCH;


END	









