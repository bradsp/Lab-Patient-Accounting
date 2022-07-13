-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import_Accounts
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
	
	SET @file = COALESCE(NULLIF(@file,''),
	(select TOP(1) import_file FROM dbo.XmlSourceTable WHERE processed = 0 
		ORDER BY import_file))
BEGIN TRY
BEGIN TRANSACTION
	TRUNCATE TABLE temp_track
	INSERT INTO temp_track(comment,row_count,ERROR)
		VALUES('File '+ @file, NULL, @@ERROR)
	INSERT INTO temp_track(comment,row_count, ERROR)
		VALUES('Start Time '+ CAST(GETDATE() AS  varchar(17)), NULL, @@ERROR)

	DECLARE @data VARCHAR(MAX)
	SET @data = (select doc_data FROM dbo.XmlSourceTable WHERE import_file = @file)
	DECLARE @doc xml
	SET @doc = (SELECT CONVERT(XML,@data))
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/@ID)))','int') AS [ACCOUNTS])

	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('Accounts ' ,@idoc,@@ERROR)

	SET @idoc = (SELECT @doc.value('count(data(/postingbatch/encounter/charge))','int') AS [CHARGES])
	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('Charges ' ,@idoc, @@ERROR)

	EXEC sp_xml_preparedocument @idoc OUTPUT, @doc


	/* do the duplicate checking before posting any of these records. The meditech_requisition number can be used to seperate the 	merged accounts later.*/

	TRUNCATE TABLE dbo.acc_dup_check -- this is only for this file then look in the acc_merges

	-- 1. Add accounts to acc_dup_check so it's trigger can find the master account if one already exists.
	; WITH cteCheckForDups
	AS(
	SELECT TOP(100) PERCENT
	dbo.AccountTrim(a.alias.value('(charge/account/text())[1]', 'varchar(15)')) AS 'account' 
		, a.alias.value('(charge/collectdate/text())[1]', 'datetime') AS 'collectdate'
		, COALESCE(a.alias.value('(charge/financialclass/text())[1]', 'varchar(50)'),
				a.alias.value('(charge/facility/text())[1]','varchar(10)')
				) AS 'fin_code'
		, REPLACE(a.alias.value('(charge/patientname/text())[1]', 'varchar(50)'),'"','') AS 'pat_name'
		, dbo.FormatSSN(a.alias.value('(charge/patientssno/text())[1]', 'varchar(13)')) AS 'pat_ssn'
		, dbo.DateOfBirthFix(a.alias.value('(demographics/patient/dob/text())[1]'
			, 'varchar(10)')) AS 'pat_dob'
		, dbo.FormatClient( a.alias.value('(charge/facility/text())[1]','varchar(10)') --AS [facility]
		, a.alias.value('(charge/responsiblephy/text())[1]','varchar(10)') )AS [client]
	FROM (SELECT @doc AS response_xml) r
	CROSS APPLY r.response_xml.nodes('postingbatch/encounter') a(alias)
	WHERE a.alias.value('(charge/facility/text())[1]', 'varchar(10)') NOT IN ('UNK','unk')
	ORDER BY 
	dbo.AccountTrim(a.alias.value('(charge/account/text())[1]', 'varchar(15)')) 
		, a.alias.value('(charge/collectdate/text())[1]', 'datetime')
		, COALESCE(a.alias.value('(charge/financialclass/text())[1]', 'varchar(50)'),
					a.alias.value('(charge/facility/text())[1]','varchar(10)'))
		, REPLACE(a.alias.value('(charge/patientname/text())[1]', 'varchar(50)'),'"','')
		, dbo.FormatSSN(a.alias.value('(charge/patientssno/text())[1]', 'varchar(13)'))
		, dbo.DateOfBirthFix(a.alias.value('(//patient/dob/text())[1]', 'varchar(10)')) 
	)
	INSERT acc_dup_check
	( 
	account, service_date, fin_code, pat_name, pat_ssn, pat_dob,client
	, mod_date,mod_user, mod_prg, mod_host
	)
	SELECT DISTINCT account ,
			collectdate ,
			fin_code ,
			REPLACE(pat_name,'"','') ,
			dbo.FormatSSN(pat_ssn) AS [ssn] ,
			dbo.DateOfBirthFix(pat_dob) 
			,client
			, GETDATE() AS [mod_date]
			, RIGHT(SUSER_SNAME(),50) AS [mod_user]
			, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
				'XML IMPORT CHRG ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
			, RIGHT (HOST_NAME(),50)  AS [mod_host]
			FROM cteCheckForDups

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count, ERROR)
	VALUES('Acc_dup_check inserted records ',@COUNT, @@ERROR)

	/* removed per carols request
	EXEC usp_WriteAccErrors
	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,ERROR)
	VALUES('4.  Wrote [ ' + CAST(@COUNT AS VARCHAR(10))+' ] account errors to chrg_err table.', @@ERROR)
	INSERT INTO temp_track
	VALUES('4. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT ('4.  Wrote [ ' + CAST(@COUNT AS VARCHAR(10))+' ] account errors to chrg_err table.')
	*/

	-- then update
	; WITH cte
	AS
	(
	SELECT   DISTINCT TOP(100) PERCENT
	DENSE_RANK() OVER (PARTITION BY pat_name,pat_ssn,pat_dob, fin_code, service_date
					ORDER BY service_date, dbo.AccountTrim(account)
					) AS [DR],
        dbo.AccountTrim(account) AS [account],
        pat_name , service_date, client
        FROM dbo.acc_dup_check
        WHERE master_account IS NULL 
        ORDER BY dbo.AccountTrim(account), pat_name
	)
	UPDATE dbo.acc_dup_check
	SET master_account = q.account
	FROM dbo.acc_dup_check
	INNER JOIN
	(
	SELECT cte1.account ,cte.account AS [master_account]
	FROM cte
	CROSS JOIN cte AS cte1
	WHERE cte.DR = cte1.DR+1
	AND cte.pat_name = cte1.pat_name AND cte.client = cte1.client) AS [q] ON q.master_account = dbo.acc_dup_check.account

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count, ERROR)
	VALUES('Acc_dup_check updated records ' ,@COUNT, @@ERROR)

	---- acc_dup_check has duplicates from the file
	---- acc_merges is the master file
	---- 1. find out if the master account is a dup_acc

	; WITH cteDups
	AS
	(
	SELECT 
	ROW_NUMBER() OVER (PARTITION BY ac.master_account,ac.account 
		ORDER BY ac.master_account, ac.account) AS [rn],
	ac.master_account, ac.account,ac.pat_ssn, ac.service_date,ac.fin_code 
	,@file AS [import_file]
	, @modDate AS [mod_date]
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	FROM dbo.acc_dup_check ac
	LEFT OUTER JOIN dbo.acc_merges am ON am.account = ac.master_account
		AND am.dup_acc  = ac.account
	WHERE am.account IS NULL AND ac.master_account IS not NULL  
	)
	INSERT INTO dbo.acc_merges ( account , dup_acc, pat_ssn, service_date,fin_code,
		xml_file,	mod_date, mod_user, mod_prg, mod_host )
	SELECT --rn ,
			master_account ,
			account ,
			pat_ssn ,
			service_date ,
			fin_code ,
			import_file ,
			mod_date ,
			mod_user ,
			mod_prg ,
			mod_host FROM cteDups
	where rn = 1


	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('Acc_merges inserted records ' , @COUNT, @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('6. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '6.  Acc_merges inserted records ' + CONVERT(VARCHAR(4),@COUNT)


	-- 2. find out if the account is a dup_acc
	INSERT INTO dbo.acc_merges ( account , dup_acc, pat_ssn, service_date,fin_code,
		xml_file,	mod_date, mod_user, mod_prg, mod_host  )
		
	SELECT TOP(10) ac.master_account, ac.account,ac.pat_ssn, ac.service_date,ac.fin_code 
	,@file
	, @modDate AS [mod_date]
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	FROM dbo.acc_dup_check ac
	LEFT OUTER JOIN dbo.acc_merges am ON am.dup_acc = ac.master_account
		OR am.dup_acc = ac.account
	WHERE ac.master_account IS NOT NULL AND am.account IS NULL AND am.dup_acc IS NULL

	SET @COUNT	= @@ROWCOUNT
	INSERT INTO temp_track(comment,ERROR)
	VALUES('Acc_merges inserted records ' + CONVERT(VARCHAR(4),@COUNT), @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('7. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '7.  Acc_merges inserted records ' + CONVERT(VARCHAR(4),@COUNT)
	 
	 --set the xml_info into acc_merges for the duplicate accounts 
	UPDATE dbo.acc_merges
	SET xml_info = colXml
	--select *
	FROM dbo.acc_merges
	CROSS APPLY dbo.GetXmlDataForAcc (acc_merges.dup_acc,dbo.acc_merges.xml_file)
	WHERE xml_info IS NULL AND dbo.acc_merges.xml_file = @file


	SET @count = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count, ERROR)
	VALUES('Acc_merges updated records ', @count, @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('8. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '8.  Acc_merges updated records ' + CONVERT(VARCHAR(4),@count)

	-- 2. if the account is not in the account table add it.
	; with newAccs as
	(
	select distinct 
		ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc,dbo.AccountTrim(lacc.account))
			ORDER BY COALESCE(colAcc,dbo.AccountTrim(lacc.account))) AS rn
		,COALESCE(colAcc,dbo.AccountTrim(lacc.account)) AS [account]
		,collectdate
		,ovacct
		,COALESCE(financialclass,facility) AS [financialclass]
		,dbo.FormatClient(facility ,client ) AS [client]
		--, facility
		,LEFT(REPLACE(patient,'"',''),40) AS [patient]
		,dbo.FormatSSN(pat_ssn) as [pat_ssn]
		, REPLACE(pat_mri,'"','') AS [pat_mri]
		, dbo.DateOfBirthFix(pat_dob) as [pat_dob]
--		, COALESCE(CASE WHEN location = 'LIFT' AND ins_group IS NOT NULL
--						THEN	ins_group
--						ELSE NULL END,ins_group,facility) AS [location]
--		, ins_group	
		, client AS [err]
	from OPENXML (@idoc, '/postingbatch/encounter/demographics',2)
	with (
			 patient varchar(40)		'patient/name',
			 pat_ssn varchar(11)		'patient/ssno',
			 pat_mri varchar(15)		'patient/unit',
			 pat_dob varchar(10)		'patient/dob',
			 ovacct varchar(50)			'/visit/ovacc',		 
			 account varchar(15)		'../charge/account',
			 collectdate DATETIME		'../charge/collectdate',		 
			 financialclass varchar(10) '../charge/financialclass',
			 facility varchar(10)		'../charge/facility',
			 client varchar(10)			'../charge/responsiblephy'--,
--			 ins_group VARCHAR(50)		'insurance1/groupno/text()[contains (.,"111926") or contains(.,"122607")]',
--			 location VARCHAR(50)		'visit/location/text()[contains (.,"LIFT")]' 
			 
		 ) lacc
		 OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(lacc.account))
	) 
	insert  acc (
		account,pat_name,cl_mnem,[status],fin_code
		,trans_date
		,ssn,meditech_account
		,original_fincode,mri, ov_pat_id
		,mod_date, mod_user, mod_prg, mod_host)
	select  distinct 
		newAccs.account, newAccs.patient
	
	, CASE WHEN newAccs.client LIKE 'ERR%' THEN newAccs.err
		ELSE newAccs.client END AS [client]
	
	, CASE WHEN newAccs.client LIKE 'ERR%' THEN 'ERR'
		   WHEN newAccs.financialclass = 'U' THEN 'ERR'
		ELSE 'NEW' END AS [status]
		
	, [financialclass]
	, CASE WHEN [err] IN ('CG','BG','MG') 
			then min(collectdate) OVER (PARTITION BY newAccs.account,client)
			ELSE newAccs.collectdate END AS [dos]
	, newAccs.pat_ssn
	, newAccs.account -- don't put the master one here
	,[financialclass]
	,newAccs.pat_mri--, colAcc
	, newAccs.ovacct
	,@modDate
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	--, ABS(CHECKSUM(newAccs.patient,newAccs.pat_ssn,newAccs.ovacct, newAccs.pat_dob)) AS [guarantor_id]
	--,colAcc
	from newAccs
	left outer join acc on acc.account = newAccs.account
	where (acc.account is NULL) AND rn = 1
	ORDER BY newAccs.patient

	SET @count = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES(' Acc Table rows added ',@count, @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('9. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--print '9.  Acc Table rows added ' + convert(varchar(10),@count)
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
        
END CATCH

END


