-- =============================================
-- Author:		David
-- Create date: 07/10/2014
-- Description:	File the charges that did not get Processed
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import_UnProcessed_Charges_Verify 
	-- Add the parameters for the stored procedure here
	@file varchar(50) = '' 
	   
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SET @file = COALESCE(NULLIF(@file,''),(select TOP(1) import_file FROM dbo.XmlSourceTable 
		WHERE processed = 0	ORDER BY import_file))
BEGIN TRY
BEGIN TRANSACTION
	--SELECT @file 
	DECLARE @xml XML
SET @xml = (
SELECT CONVERT(XML,doc_data)
FROM dbo.XmlSourceTable 
WHERE import_FILE = @file)

; WITH cteCdm 
AS
( 
	SELECT  dbo.AccountTrim(a.alias.value('(charge/account/text())[1]'
	, 'varchar(15)')) AS [account]
	, a.alias.value('(charge/collectdate/text())[1]','datetime') AS [transDate]
	,a.alias.value('(charge/billcode/text())[1]','varchar(7)') AS [cdm]
		,a.alias.value('(charge/billcode/text())[1]
		[  .="5939047" or .="5939003" or .="5939051" or .="5959041" or .="5929006"  
		or .="5929022" or .="5849600" or .="5939033" or .="5949041" or .="5849510" ]',
		'varchar(7)') AS [cdms]
	,a.alias.value('(charge/quantity/text())[1]','int') AS [qty]
	,a.alias.value('(charge/number/text())[1]','varchar(12)') AS [mtReqNo]
	,a.alias.value('(charge/performingsite/text())[1]','varchar(50)') AS [performingSite]
	,a.alias.value('(charge/patientname/text())[1]','varchar(50)') AS [patName]
	,a.alias.value('(charge/ordersite/text())[1]','varchar(100)') AS [orderSite]
	,a.alias.value('(charge/location/text())[1]','varchar(50)') AS [location]
	,a.alias.value('(charge/responsiblephy/text())[1]','varchar(50)') AS [responsiblePhy]
	,a.alias.value('(charge/test/text())[1]','varchar(50)') AS [mtMnem]
	,a.alias.value('(charge/action/text())[1]','varchar(50)') AS [action]
	,a.alias.value('(charge/facility/text())[1]','varchar(10)') AS [facility]
	,a.alias.value('(demographics/patient/dob/text())[1]','varchar(12)') AS [patDob]
	,a.alias.value('(charge/facility/text())[1] [. = "unk" or .= "UNK"]','varchar(10)') AS [unk]
	
	FROM (select @xml AS rep_xml) r
 
CROSS APPLY r.rep_xml.nodes('postingbatch/encounter') a(alias)
)
--INSERT INTO dbo.chrg_unprocessed
--		(
--			account ,service_date ,cdm ,qty ,mt_req_no ,performing_site ,			
--			pat_name ,order_site ,location ,responsiblephy ,mt_mnem ,
--			action ,facility ,pat_dob ,
--			chrg_err,post_file ,
--			lname ,fname ,mname ,name_suffix ,name_prefix ,
--			mod_date ,
--			mod_user ,
--			mod_prg ,
--			mod_host 
--			
--		)

SELECT account ,transDate 
		,CASE WHEN [unk] IS NOT NULL 
			THEN cdm 
			ELSE cdms 
			END AS [cdm],qty ,mtReqNo ,performingSite ,
		patName ,orderSite ,location ,responsiblePhy ,mtMnem ,
		action ,facility ,patDob 
		,CASE WHEN [unk] IS NOT NULL THEN 'UNKNOWN FACILITY'
			  ELSE 'CDM EXISTS AT NO CHARGE'
			  END AS [ERR]
			  , @file
			  
		, (SELECT NULLIF(cLNAME,'') FROM dbo.ufn_Split_Name(patName))
		, (SELECT NULLIF(cFNAME,'') FROM dbo.ufn_Split_Name(patName))
		, (SELECT NULLIF(cMNAME,'') FROM dbo.ufn_Split_Name(patName))
		,  (SELECT NULLIF(cSUFFIX,'') FROM dbo.ufn_Split_Name(patName))
		,   (SELECT NULLIF(cPREFIX,'') FROM dbo.ufn_Split_Name(patName))
		, GETDATE() AS [mod_date]
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		 	'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
		, RIGHT (HOST_NAME(),50)  AS [mod_host]
		
FROM cteCdm
WHERE [cdms] IS NOT NULL
-- OR 
--[unk] IS NOT NULL
ORDER BY [cdms]

--DECLARE @count INT
--SET @count = @@ROWCOUNT
--INSERT INTO dbo.temp_track	( comment, error, mod_date )
--VALUES	( 'UnProcessed Charges',@count, @@ERROR	)	
--	
	
	
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
        
END CATCH


END
