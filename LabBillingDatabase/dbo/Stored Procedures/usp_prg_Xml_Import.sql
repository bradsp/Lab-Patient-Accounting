-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import 
	-- Add the parameters for the stored procedure here

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
		[error] [VARCHAR] (8000) NULL
		)
	END

	SET NOCOUNT ON
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	DECLARE @startTime datetime
	DECLARE @endTime datetime
	DECLARE @modDate datetime
	SET @modDate = getdate()
	DECLARE @idoc int
	DECLARE @COUNT int
	SET @startTime = getdate()
	DECLARE @file varchar(50)
	SET @file = (select TOP(1) import_file FROM dbo.XmlSourceTable WHERE processed = 0 
		ORDER BY import_file) 
BEGIN TRY
BEGIN TRANSACTION
	TRUNCATE TABLE temp_track
	INSERT INTO temp_track(comment,ERROR)
		VALUES('File '+ @file, @@ERROR)
	INSERT INTO temp_track(comment,ERROR)
		VALUES('Start Time '+ CAST(GETDATE() AS  varchar(17)), @@ERROR)

	DECLARE @data VARCHAR(MAX)
	SET @data = (select TOP(1) doc_data FROM dbo.XmlSourceTable WHERE processed = 0 
		ORDER BY import_file)
	DECLARE @doc xml
	SET @doc = (SELECT CONVERT(XML,@data))
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/@ID)))','int') AS [ACCOUNTS])

	INSERT INTO temp_track(comment,ERROR)
	VALUES('1.  Accounts ' +CONVERT(VARCHAR(10),@idoc),@@ERROR)
	INSERT INTO temp_track
	VALUES('1. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '1.  Accounts ' +CONVERT(VARCHAR(10),@idoc)

	SET @idoc = (SELECT @doc.value('count(data(/postingbatch/encounter/charge))','int') AS [CHARGES])
	INSERT INTO temp_track(comment,ERROR)
	VALUES('2.  Charges ' +CONVERT(VARCHAR(10),@idoc), @@ERROR)
	INSERT INTO temp_track
	VALUES('2. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '2.  Charges ' +CONVERT(VARCHAR(10),@idoc)

	EXEC sp_xml_preparedocument @idoc OUTPUT, @doc


	/* do the duplicate checking before posting any of these records. The meditech_requisition number can be used to seperate the 	merged accounts later.*/

	TRUNCATE TABLE dbo.acc_dup_check -- this is only for this file then look in the acc_merges

	-- 1. Add accounts to acc_dup_check so it's trigger can find the master account if one already exists.
	; WITH cteCheckForDups
	AS(
	SELECT TOP(100) PERCENT
	dbo.AccountTrim(a.alias.value('(charge/account/text())[1]', 'varchar(15)')) AS 'account' 
		, dbo.DateOfBirthFix(a.alias.value('(charge/collectdate/text())[1]', 'varchar(10)')) AS 'collectdate'
		, dbo.FormatFinCode(a.alias.value('(charge/financialclass/text())[1]', 'varchar(50)'),
				dbo.FormatClient(
					a.alias.value('(charge/facility/text())[1]','varchar(10)'),
					a.alias.value('(charge/responsiblephy/text())[1]','varchar(10)'))
				) AS 'fin_code'
		, REPLACE(a.alias.value('(charge/patientname/text())[1]', 'varchar(50)'),'"','') AS 'pat_name'
		, dbo.FormatSSN(a.alias.value('(charge/patientssno/text())[1]', 'varchar(13)')) AS 'pat_ssn'
		, dbo.DateOfBirthFix(a.alias.value('(demographics/patient/dob/text())[1]'
			, 'varchar(10)')) AS 'pat_dob'
		, dbo.FormatClient( a.alias.value('(charge/facility/text())[1]','varchar(10)') --AS [facility]
		, a.alias.value('(charge/responsiblephy/text())[1]','varchar(10)') )AS [client]
	FROM (SELECT @doc AS response_xml) r
	CROSS APPLY r.response_xml.nodes('postingbatch/encounter') a(alias)
	--WHERE a.alias.value('(charge/patientname/text())[1]', 'varchar(15)') =	'BIRD,BRANDI'
	ORDER BY 
	dbo.AccountTrim(a.alias.value('(charge/account/text())[1]', 'varchar(15)')) 
		, dbo.DateOfBirthFix( a.alias.value('(charge/collectdate/text())[1]', 'varchar(10)'))
		, dbo.FormatFinCode(a.alias.value('(charge/financialclass/text())[1]', 'varchar(50)'),
				dbo.FormatClient(
					a.alias.value('(charge/facility/text())[1]','varchar(10)'),
					a.alias.value('(charge/responsiblephy/text())[1]','varchar(10)'))
				)
		, REPLACE(a.alias.value('(charge/patientname/text())[1]', 'varchar(50)'),'"','')
		, dbo.FormatSSN(a.alias.value('(charge/patientssno/text())[1]', 'varchar(13)'))
		, dbo.DateOfBirthFix(a.alias.value('(//patient/dob/text())[1]', 'varchar(10)')) 
	)
	INSERT acc_dup_check
	( 
	account, service_date, fin_code, pat_name, pat_ssn, pat_dob,client
	, mod_date,mod_user, mod_prg, mod_host
	)
	SELECT account ,
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
	INSERT INTO temp_track(comment,ERROR)
	VALUES('3.  Acc_dup_check inserted records '+CONVERT(VARCHAR(4),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('3. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	PRINT '3.  Acc_dup_check inserted records '+CONVERT(VARCHAR(4),@COUNT)
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
        WHERE master_account IS NULL --AND pat_name = 'COLEMAN,VIRGINIA HUBBARD'
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
	INSERT INTO temp_track(comment,ERROR)
	VALUES('5.  Acc_dup_check updated records ' + CONVERT(VARCHAR(4),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('5. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '5.  Acc_dup_check updated records ' + CONVERT(VARCHAR(4),@COUNT)

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
	INSERT INTO temp_track(comment,ERROR)
	VALUES('6.  Acc_merges inserted records ' + CONVERT(VARCHAR(4),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('6. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
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
	VALUES('7.  Acc_merges inserted records ' + CONVERT(VARCHAR(4),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('7. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '7.  Acc_merges inserted records ' + CONVERT(VARCHAR(4),@COUNT)
	 
	 --set the xml_info into acc_merges for the duplicate accounts 
	UPDATE dbo.acc_merges
	SET xml_info = colXml
	--select *
	FROM dbo.acc_merges
	CROSS APPLY dbo.GetXmlDataForAcc (acc_merges.dup_acc,dbo.acc_merges.xml_file)
	WHERE xml_info IS NULL AND dbo.acc_merges.xml_file = @file


	SET @count = @@ROWCOUNT
	INSERT INTO temp_track(comment,ERROR)
	VALUES('8.  Acc_merges updated records ' + CONVERT(VARCHAR(4),@count), @@ERROR)
	INSERT INTO temp_track
	VALUES('8. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '8.  Acc_merges updated records ' + CONVERT(VARCHAR(4),@count)

	-- 2. if the account is not in the account table add it.
	; with newAccs as
	(
	select distinct 
		ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc,dbo.AccountTrim(lacc.account))
			ORDER BY COALESCE(colAcc,dbo.AccountTrim(lacc.account))) AS rn
		,COALESCE(colAcc,dbo.AccountTrim(lacc.account)) AS [account]
		,dbo.DateOfBirthFix(collectdate) AS [collectdate]
		,ovacct
		,dbo.FormatFinCode(financialclass,dbo.FormatClient(facility,client)) AS [financialclass]
		,dbo.FormatClient(COALESCE(ins_group,facility) ,	client ) AS [client]
		,LEFT(REPLACE(patient,'"',''),40) AS [patient]
		,dbo.FormatSSN(pat_ssn) as [pat_ssn]
		, REPLACE(pat_mri,'"','') AS [pat_mri]
		, dbo.DateOfBirthFix(pat_dob) as [pat_dob]
		, COALESCE(CASE WHEN location = 'LIFT' AND ins_group IS NOT NULL
						THEN	ins_group
						ELSE NULL END,ins_group,facility) AS [location]
		, ins_group	
	from OPENXML (@idoc, '/postingbatch/encounter/demographics',2)
	with (
			 patient varchar(40)		'patient/name',
			 pat_ssn varchar(11)		'patient/ssno',
			 pat_mri varchar(15)		'patient/unit',
			 pat_dob varchar(10)		'patient/dob',
			 ovacct varchar(50)			'/visit/ovacc',		 
			 account varchar(15)		'../charge/account',
			 collectdate varchar(10)	'../charge/collectdate',		 
			 financialclass varchar(10) '../charge/financialclass',
			 facility varchar(10)		'../charge/facility',
			 client varchar(10)			'../charge/responsiblephy',
			 ins_group VARCHAR(50)		'insurance1/groupno/text()[contains (.,"111926") or contains(.,"122607")]',
			 location VARCHAR(50)		'visit/location/text()[contains (.,"LIFT")]' 
			 
		 ) lacc
		 OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(lacc.account))
	) 
	insert  acc (
		account,pat_name,cl_mnem,fin_code
		,trans_date
		,ssn,meditech_account
		,original_fincode,mri, ov_pat_id
		,mod_date, mod_user, mod_prg, mod_host)
	select  distinct 
		newAccs.account, newAccs.patient, newAccs.client , [financialclass]
	, CASE WHEN client IN ('CGH','BCH','COM') 
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
	INSERT INTO temp_track(comment,ERROR)
	VALUES('9.  Acc Table rows added ' + convert(varchar(10),@count), @@ERROR)
	INSERT INTO temp_track
	VALUES('9. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--print '9.  Acc Table rows added ' + convert(varchar(10),@count)
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


BEGIN TRY
BEGIN TRANSACTION

/*
/* DIAGNOSIS CODES */
; with cteDiag
	as
	(
		select DISTINCT dbo.AccountTrim(EncounterID) as [EncounterID],
		REPLACE(COALESCE([master_account],dbo.AccountTrim(EncounterID)),'&QUOT;','') as [account]
		
		
		,dx_number, REPLACE(diagnosis,'"','') AS [diagnosis]
		,dbo.DateOfBirthFix(trans_date) AS [trans_date], code_indicator
		from 
		( 
		select DISTINCT
			EncounterID,
		COALESCE (colAcc,dbo.AccountTrim(EncounterID),colOrigAcc) AS [master_account]
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag1 ELSE NULL END AS diag1
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag2 ELSE NULL END AS diag2
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag3 ELSE NULL END AS diag3
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag4 ELSE NULL END AS diag4
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag5 ELSE NULL END AS diag5
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag6 ELSE NULL END AS diag6
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag7 ELSE NULL END AS diag7
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag8 ELSE NULL END AS diag8
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag9 ELSE NULL END AS diag9
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag10 ELSE NULL END AS diag10
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag11 ELSE NULL END AS diag11
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag12 ELSE NULL END AS diag12
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag13 ELSE NULL END AS diag13
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag14 ELSE NULL END AS diag14
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag15 ELSE NULL END AS diag15
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag16 ELSE NULL END AS diag16
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag17 ELSE NULL END AS diag17
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag18 ELSE NULL END AS diag18
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag19 ELSE NULL END AS diag19
		,CASE WHEN dbo.AccountTrim(EncounterID) = COALESCE (colAcc,colOrigAcc,dbo.AccountTrim(EncounterID))
				THEN 	diag20 ELSE NULL END AS diag20
			
			, dbo.DateOfBirthFix(trans_date) AS [trans_date]
			, code_indicator
		from OPENXML(@idoc, N'/postingbatch/encounter/demographics/diagnosis',2)
		with ( 
				EncounterID varchar(15) '../../@ID',
				diag1 varchar(10) 'dx1',
				diag2 varchar(10) 'dx2',
				diag3 varchar(10) 'dx3',
				diag4 varchar(10) 'dx4',
				diag5 varchar(10) 'dx5',		
				diag6 varchar(10) 'dx6',
				diag7 varchar(10) 'dx7',
				diag8 varchar(10) 'dx8',
				diag9 varchar(10) 'dx9',
				diag10 varchar(10) 'dx10',
				diag11 varchar(10) 'dx11',
				diag12 varchar(10) 'dx12',
				diag13 varchar(10) 'dx13',
				diag14 varchar(10) 'dx14',
				diag15 varchar(10) 'dx15',
				diag16 varchar(10) 'dx16',
				diag17 varchar(10) 'dx17',
				diag18 varchar(10) 'dx18',
				diag19 varchar(10) 'dx19',
				diag20 varchar(10) 'dx20',
				code_indicator varchar(5)  'ver',
				trans_date varchar(10) '../../charge/collectdate'
			
			 ) as FTable
			 OUTER	APPLY dbo.GetMasterAccount(dbo.AccountTrim(EncounterID)) --as [master_account]
	) xd
	unpivot
	(
		 diagnosis for dx_number 
		 in (diag1,diag2,diag3,diag4,diag5,diag6,diag7,diag8,diag9,diag10
			,diag11,diag12,diag13,diag14,diag15,diag16,diag17,diag18,diag19,diag20)
	) as upvt
)
insert patdx (account
,dx_number
,diagnosis
,version
, is_error
, code_qualifier
, import_file
, mod_date, mod_user, mod_prg, mod_host)
SELECT DISTINCT dbo.AccountTrim(cteDiag.account)
,		RIGHT(cteDiag.dx_number,1) AS [dx_number] 
		,cteDiag.diagnosis 
		
		,isnull((select version from dictionary.icd9version where cteDiag.trans_date between icd9version.effective_date and 
			coalesce(icd9version.effective_end_date,getdate())),'ERR') as version--version ,
		
		,case when not exists(select icd9_num from dbo.icd9desc where icd9_num = cteDiag.diagnosis) then 
			'True' else 'False'  end as [is_error]--is_error ,

		,coalesce(cteDiag.code_indicator,'BK') as [codeindicator] --code_qualifier
		,@file	 
		, @modDate AS [mod_date]
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
		, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 FROM cteDiag
LEFT OUTER JOIN dbo.patdx ON dbo.patdx.account = cteDiag.account
WHERE dbo.patdx.account IS null
	
SET @count = @@ROWCOUNT
INSERT INTO temp_track(comment,ERROR)
VALUES('10. Patdx Table rows added ' + convert(varchar(10),@count), @@ERROR)
INSERT INTO temp_track
VALUES('10. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
--print '10. Patdx Table rows added ' + convert(varchar(10), @count)

EXEC usp_prg_PurgeDuplicates @file
SET @count = @@ROWCOUNT
INSERT INTO temp_track(comment,ERROR)
VALUES('10a. Patdx Table duplicate rows deleted ' + convert(varchar(10),@count), @@ERROR)
INSERT INTO temp_track
VALUES('10a. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
--print '10a. Patdx Table duplicate rows deleted ' + convert(varchar(10), @count)

INSERT INTO temp_track(comment,ERROR)
VALUES('10a. Patdx Table skipped ' , 
'Msg 512, Level 16, State 1, Line 318'+CHAR(13)+
'Subquery returned more than 1 value. This is not permitted when the subquery follows =, !=, <, <= , >, >= or when the subquery is used as an expression.'+CHAR(13)+
'The statement has been terminated.'
)
*/

	/* Patient table */
	/* NOTES
	We have three distinct selection for ordering physician
	1. Affiliates -- <attendphy> and <attendphyname>
	2. Non Affiliates / Non Nursing Homes / Non CLOE -- <otherdrmnem> and <otherdrname>
	3. Nursing homes / All CLOE -- <referringphymnem> and <referringphyname> 
	*/

	; with ctePat as
	(
		select  DISTINCT 
		COALESCE(colAcc, dbo.AccountTrim(FTable.account)) AS [account]
			, ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc, dbo.AccountTrim(FTable.account)) 
					ORDER BY colAcc) as [rn]
			,REPLACE(pat_name,'"','') AS [pat_name]
			,dbo.FormatSSN(FTable.pat_ssn) as [pat_ssn] 
			, REPLACE(pat_addr1,'"','') AS [pat_addr1]
			, REPLACE(pat_addr2,'"','') AS [pat_addr2]
			, REPLACE((pat_city+', '+pat_state+' '+ pat_zip),'"','')  as [city_st_zip]
			 , replace(pat_city,'"','') AS [pat_city]
			 , REPLACE(pat_state,'"','') AS [pat_state]
			 , REPLACE(pat_zip ,'"','') AS [pat_zip]
			, dbo.DateOfBirthFix(pat_dob) as  [dob_yyyy]
			, REPLACE(pat_sex ,'"','') AS [pat_sex]
			, REPLACE(pat_marital,'"','') AS [pat_marital]
			, REPLACE(pat_race,'"','') as [pat_race]
			, REPLACE(relation,'"','') as [relation text]
			, dbo.FormatRelation(relation) AS [relation]
			, REPLACE(guarantor,'"','') AS [guarantor]
			, REPLACE(guar_addr,'"','') AS [guar_addr]
			, REPLACE((guar_city+', '+guar_state+' '+guar_zip),'"','') as [g_city_st]
			, REPLACE(guar_city,'"','') AS [guar_city]
			, REPLACE(guar_state,'"','') AS [guar_state]
			, REPLACE(guar_zip,'"','') AS [guar_zip]
			, dbo.FormatPhoneNo(guar_phone) as 	guar_phone
			,  REPLACE(icd9_1,'"','') AS [icd9_1],  REPLACE(icd9_2,'"','') AS [icd9_2]
			,  REPLACE(icd9_3,'"','') AS [icd9_3],  REPLACE(icd9_4,'"','') AS [icd9_4]
			,  REPLACE(icd9_5,'"','') AS [icd9_5],  REPLACE(icd9_6,'"','') AS [icd9_6]
			,  REPLACE(icd9_7,'"','') AS [icd9_7],  REPLACE(icd9_8,'"','') AS [icd9_8]
			,  REPLACE(icd9_9,'"','') AS [icd9_9]
			
			, dbo.FormatPhoneNo(pat_phone) as pat_phone
			, CASE WHEN REPLACE(facility,'"','') IS NULL THEN NULLIF(REPLACE(AttendPhyMnem,'"',''),'')
					ELSE COALESCE(NULLIF(REPLACE(refPhyMnem,'"',''),''),NULLIF(REPLACE(otherPhyMnem,'"',''),'')) 
					END AS [mt_mnem]
			, CASE WHEN REPLACE	(facility,'"','') IS NULL THEN NULLIF(REPLACE(AttendPhyName,'"',''),'')
					ELSE COALESCE(NULLIF(REPLACE(refPhyName,'"',''),''),NULLIF(REPLACE(otherPhyName,'"',''),'')) END AS [phy_comment]		
			, AttendPhyMnem
			, AttendPhyName
			, refPhyMnem
			, refPhyName
			, otherPhyMnem
			, otherPhyName
			, facility

	from OPENXML(@idoc, N'/postingbatch/encounter/demographics/patient',2)
	with (
			account varchar(15)		'account',
			pat_name varchar(50)	'name',
			pat_ssn varchar(13)		'ssno',
			pat_addr1 varchar(40)	'addr1',
			pat_addr2 varchar(40)	'addr2',
			pat_city varchar(40)	'city',
			pat_state varchar(4)	'state',
			pat_zip varchar(12)		'zip',
			pat_dob varchar(10)		'dob',
			pat_sex varchar(5)		'gender',
			pat_marital varchar(3)	'marital',
			pat_race varchar(7)		'race',
			relation varchar(14)	'../guarantor/relation',
			guarantor varchar(40)	'../guarantor/name',
			guar_addr varchar(40)	'../guarantor/addr1',
			guar_city varchar(50)	'../guarantor/city',
			guar_state varchar(4)	'../guarantor/state',
			guar_zip varchar(12)	'../guarantor/zip',
			guar_phone varchar(25)	'../guarantor/homephone',
			icd9_1 varchar(7)		'../diagnosis/dx1',
			icd9_2 varchar(7)		'../diagnosis/dx2',
			icd9_3 varchar(7)		'../diagnosis/dx3',
			icd9_4 varchar(7)		'../diagnosis/dx4',
			icd9_5 varchar(7)		'../diagnosis/dx5',
			icd9_6 varchar(7)		'../diagnosis/dx6',
			icd9_7 varchar(7)		'../diagnosis/dx7',
			icd9_8 varchar(7)		'../diagnosis/dx8',
			icd9_9 varchar(7)		'../diagnosis/dx9',
			pat_phone varchar(25)	'../patient/homephone',
			-- if the facility is null the client is an affilate use the AttendPhy info
			-- otherwise coalesce(referringphy,otherdrmnem)
			facility VARCHAR(10)	'../../charge/facility/text()[contains (.,"MRP")]',
			-- '../demographics/insurance1/groupno/text()[contains (.,"111926") or contains(.,"122607")]', 
			-- Affiliate phy only all others it's the client
			attendPhyMnem VARCHAR(25)	'../../charge/responsiblephy',
			attendPhyName VARCHAR(50)'../../charge/responsiblephyname',
			-- For CLOE clients this is the phy
			refPhyMnem varchar(25)	'../visit/referringphymnem',
			refPhyName varchar(128) '../visit/referringphy',
			-- for Non Affiliates and non cloe this is the phy
			otherPhyMnem varchar(25)	'../visit/otherdrmnem',
			otherPhyName varchar(128)	'../visit/otherdrname'
			
		) FTable
		OUTER APPLY dbo.GetMasterAccount(FTable.account)
	) 
	INSERT INTO pat ( 
	 account,	 pat_full_name,	ssn
	, pat_addr1,		pat_addr2, city_st_zip 
	, pat_city, pat_state, pat_zip 
	, dob_yyyy,		sex,	relation
	, pat_marital,	pat_race, pat_phone
	, icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 
	, guarantor, guar_addr, guar_city, guar_state, guar_zip , g_city_st , guar_phone
	, phy_id,		phy_comment
	, mod_date, mod_user, mod_prg, mod_host
	) 

	select  distinct UPPER(ctePat.account) AS [account]--, ctePat.master_account
	, UPPER(ctePat.pat_name) AS [pat_name]
	, ctePat.pat_ssn as [pat_ssn]
	, UPPER(ctePat.pat_addr1) AS [pat_addr1]
	, UPPER(ctePat.pat_addr2) AS [pat_addr2]
	, UPPER(ctePat.city_st_zip) AS [pat_city_st_zip]
	, UPPER(ctePat.pat_city) AS [pat_city]
	, UPPER(ctePat.pat_state) AS [pat_state]
	, ctePat.pat_zip
	, ctePat.dob_yyyy
	, UPPER(ctePat.pat_sex) as [sex], ctePat.relation
	, UPPER(ctePat.pat_marital) AS [pat_marital]
	, UPPER(ctePat.pat_race) AS [pat_race]
	, COALESCE(ctePat.pat_phone, ctePat.guar_phone) AS [pat_phone]
	, UPPER(ctePat.icd9_1) AS [dx1],UPPER(ctePat.icd9_2) AS [dx2],UPPER(ctePat.icd9_3) AS [dx3]
	, UPPER(ctePat.icd9_4) AS [dx4],UPPER(ctePat.icd9_5)AS [dx5],UPPER(ctePat.icd9_6) AS [dx6]
	, UPPER(ctePat.icd9_7) AS [dx7],UPPER(ctePat.icd9_8) AS [dx8],UPPER(ctePat.icd9_9)AS [dx9]
	, UPPER(ctePat.guarantor) AS [guarantor], UPPER(ctePat.guar_addr) AS [guar_addr]
	, UPPER(ctePat.guar_city) AS [guar_city], UPPER(ctePat.guar_state) AS [guar_state], ctePat.guar_zip
	, UPPER(ctePat.g_city_st) AS [g_city_st]
	, COALESCE(ctePat.guar_phone,ctePat.pat_phone) [guar_phone]
	, [npi].colNPI AS [phyid]
	, COALESCE([npi].colName, UPPER(ctePat.phy_comment)) AS [phy_comment]
	, @modDate AS [mod_date]
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID)
		, 'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	from ctePat
	left outer join pat on pat.account = ctePat.account
	outer APPLY dbo.GetPhyNPI(mt_mnem) AS [npi]		
	where pat.account is null AND ctePat.rn = 1
	
	SET @count = @@ROWCOUNT
	INSERT INTO temp_track(comment,ERROR)
	VALUES('11. Pat Table rows added ' + convert(varchar(10),@count), @@ERROR)
	INSERT INTO temp_track
	VALUES('11. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--print '11. Pat Table rows added ' + convert(varchar(10),@count)
	--end of patient table 
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



BEGIN TRY
BEGIN TRANSACTION

	/* insurance */
	;with cteInsA
	as (
	select DISTINCT
		 COALESCE(colAcc,dbo.AccountTrim(xmlIns.account)) AS [account]
		 , ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc, dbo.AccountTrim(xmlIns.account)) 
					ORDER BY colAcc) as [rn]
		, 'A' as [ins_priority]
		, CASE WHEN LTRIM(RTRIM(REPLACE(ins_plan_name,'"',''))) = 'BILL TO CLIENT'
				THEN NULL
				ELSE CASE WHEN LTRIM(RTRIM(REPLACE(subscriber,'"',''))) = ','
						  THEN 'ERROR: NO SUBSCRIBER'
						  ELSE LTRIM(RTRIM(REPLACE(subscriber,'"','')))
						  END
				END AS [subscriber]
		,case when dbo.FormatRelation(ins_relation) = '01' 
				then dbo.DateOfBirthFix(subscriber_dob) 
				end as [subscriber_dob]
		,case when dbo.FormatRelation(ins_relation) = '01'
				then subscriber_sex
				end as [subscriber_sex]
		,case when dbo.FormatRelation(ins_relation) = '01'
				then subscriber_addr
				end as [subscriber_addr]
		,case when dbo.FormatRelation(ins_relation) = '01'
				then subscriber_city+', '+subscriber_st+' '+subscriber_zip 
				end as [subscriber_city_st_zip]		
		,REPLACE(ins_plan_name,'"','') AS [ins_plan_name]
		,REPLACE(insc.addr1,'"','') AS [addr1], REPLACE(insc.addr2,'"','') AS [addr2]
		,REPLACE(insc.citystzip,'"','') AS [citystzip]
		,REPLACE(policy,'"','') AS [policy]
		,REPLACE(ins_group_no,'"','') AS [ins_group_name]		
		,REPLACE(ins_group_no,'"','') AS [ins_group_no]
		,dbo.FormatFinCode(insc.fin_code, dbo.FormatClient(resphy,client)) AS [fin_code]
		,CASE WHEN coalesce(ins_code,'') = '' THEN 'NULL' ELSE ins_code END AS [ins_code]
		, dbo.FormatRelation(ins_relation) AS [ins_relation]
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance1',2 )
	with (

			account		varchar(15)				'account',
			ins_code varchar(10)				'mnem',
			ins_plan_name varchar(45)			'name',
			policy		varchar(50)				'policy',
			ins_group_name	varchar(50)			'group',
			ins_group_no varchar(50)			'groupno',
			subscriber	varchar(50)				'subscriber',
			subscriber_dob varchar(12)			'dob',
			subscriber_sex	varchar(1)			'../gender',--'../../demographics/patient/gender',
			subscriber_addr varchar(40)			'../addr1',
			subscriber_city varchar(40)			'../guarantor/city',--../demographics/guarantor/city',
			subscriber_st varchar(40)			'../guarantor/state',
			subscriber_zip varchar(40)			'../guarantor/zip',
			ins_relation varchar(2)				'../gurantor/relation'	,
			client varchar(10)					'../../charge/facility',
			resphy VARCHAR(10)					'../../charge/responsiblephy'			
				 ) xmlIns
				 
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			OUTER APPLY dbo.GetMasterAccount(xmlIns.account)
	)
	INSERT INTO ins
	(account,		ins_a_b_c,			holder_nme,			holder_dob,			holder_sex
	,holder_addr,	holder_city_st_zip,	plan_nme,			plan_addr1,			plan_addr2
	,p_city_st,		policy_num,			grp_nme,			grp_num			
	,fin_code,		ins_code,			relation
	,mod_date,		mod_user,			mod_prg,			mod_host)

	select distinct
	UPPER(cteInsA.account)
		,UPPER(cteInsA.ins_priority)
			,UPPER(cteInsA.subscriber)
				,cteInsA.subscriber_dob
					,UPPER(cteInsA.subscriber_sex)
	,UPPER(cteInsA.subscriber_addr)
		,UPPER(cteInsA.subscriber_city_st_zip)
			,UPPER(cteInsA.ins_plan_name)
				,UPPER(cteInsA.addr1)
					,UPPER(cteInsA.addr2)
	,UPPER(cteInsA.citystzip)
		,UPPER(cteInsA.policy)
			,UPPER(cteInsA.ins_group_name)
				,cteInsA.ins_group_no
	,UPPER(cteInsA.fin_code)
		--,UPPER(cteInsA.ins_code)
		, CASE WHEN cteInsA.ins_group_no = '11926' AND cteInsA.fin_code = 'B' THEN 'LMBC'
				WHEN cteInsA.ins_group_no = '122607' AND cteInsA.fin_code = 'B' THEN 'LCBC'
				ELSE cteInsA.ins_code END AS [ins_code]
			,cteInsA.ins_relation
	, @modDate as [mod_date]	
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 from cteInsA
	left outer join ins on ins.account = cteInsA.account and ins.ins_a_b_c = cteInsA.ins_priority
	where  ins.account is null and ins.ins_a_b_c is NULL AND cteInsA.rn = 1


	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,ERROR)
	VALUES('12. InsA Table rows added ' + convert(varchar(10),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('12. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--print '12. InsA Table rows added ' + convert(varchar(10),@COUNT)

	;with cteInsB
	as (
	select distinct 
		COALESCE(colAcc,dbo.AccountTrim(xmlIns.account)) AS [account]
		, ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc, dbo.AccountTrim(xmlIns.account)) 
				ORDER BY colAcc) as [rn]
		, 'B' as [ins_priority]
		,LTRIM(RTRIM(REPLACE(subscriber,'"',''))) AS [subscriber]
		, dbo.DateOfBirthFix(subscriber_dob) AS [subscriber_dob]
		,REPLACE(subscriber_sex,'"','') AS [subscriber_sex]
		/* don't have these values for the B,C or D insurances
		,subscriber_addr
		,subscriber_city_st_zip		
		,ins_relation */
		,REPLACE(ins_plan_name,'"','') AS [ins_plan_name]
		,replace(insc.addr1,'"','') AS [addr1]
		,REPLACE(insc.addr2,'"','') AS [addr2]
		,REPLACE(insc.citystzip,'"','') AS [citystzip]
		,REPLACE(policy,'"','') AS [policy]
		,REPLACE(ins_group_name,'"','') AS [ins_group_name]
		,REPLACE(ins_group_no,'"','') AS [ins_group_no]
		,REPLACE(insc.fin_code,'"','') AS [fin_code]
		,CASE WHEN COALESCE(REPLACE(ins_code,'"','') ,'') = '' THEN 'NULL' ELSE ins_code END AS [ins_code]
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance2',2 )
	with (
			account			varchar(17)	'account'
			,ins_code		varchar(12)	'mnem',
			ins_plan_name	varchar(45)	'name',
			policy			varchar(50)	'policy',
			ins_group_name	varchar(50)	'group',
			ins_group_no	varchar(50)	'groupno',
			subscriber		varchar(50)	'subscriber',
			subscriber_dob  VARCHAR(12)	'holderdob',
			subscriber_sex	varchar(3)	'holdersex'
		 ) xmlIns
		left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
		OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
	)
	INSERT INTO ins
	(account,ins_a_b_c,holder_nme,holder_dob,holder_sex,plan_nme,plan_addr1,plan_addr2
	,p_city_st,policy_num,grp_nme,grp_num,fin_code,ins_code
	,mod_date,		mod_user,			mod_prg,			mod_host)

	select distinct
	UPPER(cteInsB.account)
		,UPPER(cteInsB.ins_priority)
			,UPPER(cteInsB.subscriber)
				,cteInsB.subscriber_dob
					,UPPER(cteInsB.subscriber_sex)
			,UPPER(cteInsB.ins_plan_name)
				,UPPER(cteInsB.addr1)
					,UPPER(cteInsB.addr2)
	,UPPER(cteInsB.citystzip)
		,UPPER(cteInsB.policy)
			,UPPER(cteInsB.ins_group_name)
				,UPPER(cteInsB.ins_group_no)
	,UPPER(cteInsB.fin_code)
		--,UPPER(cteInsB.ins_code)
		, CASE WHEN cteInsB.ins_group_no = '11926' AND cteInsB.fin_code = 'B' THEN 'LMBC'
				WHEN cteInsB.ins_group_no = '122607' AND cteInsB.fin_code = 'B' THEN 'LCBC'
				ELSE cteInsB.ins_code END AS [ins_code]
	, @modDate as [mod_date]	
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 from cteInsB
	left outer join ins on ins.account = cteInsB.account and ins.ins_a_b_c = cteInsB.ins_priority
	where  ins.account is null and ins.ins_a_b_c is NULL AND cteInsB.rn = 1

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,ERROR)
	VALUES('13. InsB Table rows added ' + convert(varchar(10),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('13. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--print '13. InsB Table rows added ' + convert(varchar(10),@COUNT)
	;with cteInsC
	as (
	select distinct 
		COALESCE(colAcc,dbo.AccountTrim(xmlIns.account)) AS [account]
		, ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc, dbo.AccountTrim(xmlIns.account)) 
				ORDER BY colAcc) as [rn]
		, 'C' as [ins_priority]
		,REPLACE(subscriber,'"','') AS [subscriber]
		, dbo.DateOfBirthFix(subscriber_dob) AS [subscriber_dob]
		,REPLACE(subscriber_sex,'"','') AS [subscriber_sex]
		,REPLACE(ins_plan_name,'"','') AS [ins_plan_name]
		,REPLACE(insc.addr1,'"','') AS [addr1]
		,REPLACE(insc.addr2,'"','') AS [addr2]
		,REPLACE(insc.citystzip,'"','') AS [citystzip]
		,REPLACE(policy,'"','') AS [policy]
		,REPLACE(ins_group_name,'"','') AS [ins_group_name]
		,REPLACE(ins_group_no,'"','') AS [ins_group_no]
		,REPLACE(insc.fin_code,'"','') AS [fin_code]
		,CASE WHEN COALESCE(REPLACE(ins_code,'"',''),'') = '' THEN 'NULL' ELSE ins_code END AS [ins_code]
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance3',2 )
	with (
			account			varchar(17)	'account',
			ins_code		varchar(12)	'mnem',
			ins_plan_name	varchar(45)	'name',
			policy			varchar(50)	'policy',
			ins_group_name	varchar(50)	'group',
			ins_group_no	varchar(50)	'groupno',
			subscriber		varchar(50)	'subscriber',
			subscriber_dob VARCHAR(12)	'subscriberdob',
			subscriber_sex	VARCHAR(3)	'subscribersex'		
				 ) xmlIns
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
	)
	INSERT INTO ins
	(account,		ins_a_b_c,			holder_nme,
	 holder_dob, holder_sex
	,	plan_nme,			plan_addr1,			plan_addr2
	,p_city_st,		policy_num,			grp_nme,			grp_num			
	,fin_code,		ins_code
	,mod_date,		mod_user,			mod_prg,			mod_host)

	select distinct
	UPPER(cteInsC.account)
		,UPPER(cteInsC.ins_priority)
			,UPPER(cteInsC.subscriber)
				,cteInsC.subscriber_dob
				,UPPER(cteInsC.subscriber_sex)
			,UPPER(cteInsC.ins_plan_name)
				,UPPER(cteInsC.addr1)
					,cteInsC.addr2
	,cteInsC.citystzip
		,cteInsC.policy
			,cteInsC.ins_group_name
				,cteInsC.ins_group_no
	,cteInsC.fin_code
		--,cteInsC.ins_code
		, CASE WHEN cteInsC.ins_group_no = '11926' AND cteInsC.fin_code = 'B' THEN 'LMBC'
				WHEN cteInsC.ins_group_no = '122607' AND cteInsC.fin_code = 'B' THEN 'LCBC'
				ELSE cteInsC.ins_code END AS [ins_code]
	, @modDate as [mod_date]	
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 from cteInsC
	left outer join ins on ins.account = cteInsC.account and ins.ins_a_b_c = cteInsC.ins_priority
	where  ins.account is null and ins.ins_a_b_c is NULL AND cteInsC.rn = 1

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,ERROR)
	VALUES('14. InsC Table rows added ' + convert(varchar(10),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('14. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--print '14. InsC Table rows added ' + convert(varchar(10),@COUNT)

	;with cteInsD
	as (
	select 
		COALESCE(colAcc,dbo.AccountTrim(xmlIns.account)) AS [account]
		, ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc, dbo.AccountTrim(xmlIns.account)) 
				ORDER BY colAcc) as [rn]
		, 'D' as [ins_priority]
		,REPLACE(subscriber,'"','') AS [subscriber]
		,dbo.DateOfBirthFix(subscriber_dob) AS [subscriber_dob]
		,REPLACE(subscriber_sex,'"','') AS [subscriber_sex]
		,REPLACE(ins_plan_name,'"','') AS [ins_plan_name]
		,REPLACE(insc.addr1,'"','') AS [addr1]
		,REPLACE(insc.addr2,'"','') AS [addr2]
		,REPLACE(insc.citystzip,'"','') AS [citystzip]
		,REPLACE(policy,'"','') AS [policy]
		,REPLACE(ins_group_name,'"','') AS [ins_group_name]
		,REPLACE(ins_group_no,'"','') AS [ins_group_no]
		,REPLACE(insc.fin_code,'"','') AS [fin_code]
		,CASE WHEN COALESCE(REPLACE(ins_code,'"','') ,'') = '' THEN 'NULL' ELSE ins_code END AS [ins_code]
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance4',2 )
	with (
			account			varchar(17)	'account',
			ins_code		varchar(12)	'mnem',
			ins_plan_name	varchar(45)	'name',
			policy			varchar(50)	'policy',
			ins_group_name	varchar(50)	'group',
			ins_group_no	varchar(50)	'groupno',
			subscriber		varchar(50)	'subscriber',
			subscriber_dob  VARCHAR(12)	'subscriberdob',
			subscriber_sex	VARCHAR(3)	'subscribersex'
			 ) xmlIns
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			outer APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
		
	)
	INSERT INTO ins
	(account,		ins_a_b_c,			holder_nme,	holder_dob, holder_sex,		
	plan_nme,			plan_addr1,			plan_addr2
	,p_city_st,		policy_num,			grp_nme,			grp_num			
	,fin_code,		ins_code
	,mod_date,		mod_user,			mod_prg,			mod_host)

	select distinct
	cteInsD.account
		,cteInsD.ins_priority
			,cteInsD.subscriber
				,cteInsD.subscriber_dob
				,cteInsD.subscriber_sex
			,cteInsD.ins_plan_name
				,cteInsD.addr1
					,cteInsD.addr2
	,cteInsD.citystzip
		,cteInsD.policy
			,cteInsD.ins_group_name
				,cteInsD.ins_group_no
	,cteInsD.fin_code
		--, cteInsD.ins_code		
		, CASE WHEN cteInsD.ins_group_no = '11926' AND cteInsD.fin_code = 'B' THEN 'LMBC'
				WHEN cteInsD.ins_group_no = '122607' AND cteInsD.fin_code = 'B' THEN 'LCBC'
				ELSE cteInsD.ins_code END AS [ins_code]
		, @modDate as [mod_date]	
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
			'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
		, RIGHT (HOST_NAME(),50)  AS [mod_host]

	 from cteInsD
	left outer join ins on ins.account = cteInsD.account and ins.ins_a_b_c = cteInsD.ins_priority
	where  ins.account is null and ins.ins_a_b_c is NULL AND cteInsD.rn = 1

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,ERROR)
	VALUES('15. InsD Table rows added ' + convert(varchar(10),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('15. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--print '15. InsD Table rows added ' + convert(varchar(10),@COUNT)
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



BEGIN TRY
BEGIN TRANSACTION

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
	INSERT INTO temp_track(comment,ERROR)
	VALUES('16. ACC_LOCATION Table rows added ' + CONVERT(VARCHAR(10),@COUNT), @@ERROR)
	INSERT INTO temp_track
	VALUES('16. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	--PRINT '16. ACC_LOCATION Table rows added ' + CONVERT(VARCHAR(10),@COUNT)
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

BEGIN TRY
BEGIN TRANSACTION

DECLARE @batch VARCHAR(16)
SET @batch = REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(16),GETDATE(),126),'-',''),'T',''),':','')
--SELECT @batch
/* chrg time*/
; with cteChrg
as
(
select DISTINCT 
	ROW_NUMBER() OVER (order BY COALESCE(colAcc,dbo.AccountTrim(xmlChrg.account))
		,coalesce(dc.bill_cdm,billcode)) AS [rn],
COALESCE(colAcc,dbo.AccountTrim(xmlChrg.account)) as [account]
, dbo.FormatClient(facility, client ) AS [client]
, REPLACE(patientname,'"','') AS [Patient]
, dbo.FormatSSN(patientssno) as [pat_ssn] 
, REPLACE(unitno,'"','') AS  [unitno]
, REPLACE([status],'"','') AS [status]
, REPLACE(location,'"','') AS [location]
,	dbo.AttendPhyTrim(responsiblephy) AS [responsiblephy] 
, dbo.FormatFinCode(financialclass, dbo.FormatClient(facility,client)) AS [financialclass]
, REPLACE(quantity,'"','') AS [quantity]
, REPLACE(ordersite,'"','') AS [ordersite]
, coalesce(dc.bill_cdm,REPLACE(billcode,'"','')) as [billcode]--, dcc.bill_code as [converted_cdm]
, REPLACE([test],'"','') AS [test] 
, REPLACE([action],'"','') AS [action]
, dbo.DateOfBirthFix(collectdate) AS [collectdate]
, case when coalesce(REPLACE(performingsite,'"','') ,'') = '' and [test] = 'VP'
		then REPLACE(ordersite,'"','') 
		else case when replace(performingsite,'"','')  = ''
			then replace(ordersite,'"','')
			else replace(performingsite,'"','')
		end
		end as [performingsite]
, replace(facility,'"','') AS [facility]
, REPLACE(istemp,'"','') AS [istemp]
, REPLACE([number],'"','') AS [number]
, REPLACE([number],'"','') AS [referencereq]
,	(select cLNAME from ufn_Split_Name(patientname)) as [pat_lname],
	(select cFNAME from ufn_Split_Name(patientname)) as [pat_fname],
	(select cMNAME from ufn_Split_Name(patientname)) as [pat_mname],
	(select cSUFFIX from ufn_Split_Name(patientname)) as [pat_suffix]

, dbo.DateOfBirthFix(dob) as [dob]
, datediff(year,dbo.DateOfBirthFix(dob),dbo.DateOfBirthFix(collectdate)) as [age]
, REPLACE(xmlChrg.ins_code,'"','') AS [ins_code]
, REPLACE(xmlChrg.ins_name,'"','') AS [ins_name]
, REPLACE(xmlChrg.ins_policy_cover_kids,'"','') AS [ins_policy_cover_kids]
, REPLACE(xmlChrg.ins_policy_bca,'"','') AS [ins_policy_bca]
, REPLACE(xmlChrg.group_no_lmbc,'"','') AS [group_no_lmbc]
, REPLACE(xmlChrg.group_no_lcbc,'"','') AS [group_no_lcbc]
, REPLACE(ins_policy,'"','') AS [ins_policy]
, REPLACE(ins_group,'"','') AS [ins_group]


from OPENXML (@idoc, '/postingbatch/encounter/charge',2)
with (
		 account varchar(17)		'account',
		 patientname varchar(50)	'patientname',
		 patientssno varchar(50)	'patientssno',
		 unitno varchar(50)			'unitno',
		 [status] varchar(50)		'status',
		 location varchar(50)		'location',
		 responsiblephy varchar(50) 'responsiblephy',
		 financialclass varchar(50) 'financialclass',
		 quantity varchar(50)		'quantity',
		 ordersite varchar(100)		'ordersite',
		 billcode varchar(7)		'billcode',
		 [test] varchar(50)			'test',
		 [action] varchar(50)		'action',
		 [collectdate] varchar(12) 	'collectdate',
		 performingsite varchar(50)	'performingsite',
		 facility varchar(10)		'facility',
		 client   varchar(10)		'responsiblephy',
		 istemp varchar(50)			'istemp',
		 [number] varchar(50)		'number',
		 referencereq varchar(50)	'referencereq',
		 dob varchar(12)			'../demographics/patient/dob',
		 ins_code varchar(10)		'../demographics/insurance1/mnem',
		 ins_name varchar(45)		'../demographics/insurance1/name',
		 ins_group VARCHAR(50)		
		 '../demographics/insurance1/groupno/text()[contains (.,"111926") or contains(.,"122607")]', 
		 ins_policy VARCHAR(50)		'../demographics/insurance1/policy', -- /text()[contains (.,"409060108A") or contains(.,"323428113A")]', 
		 -- only need below for cover kids so if null and 'fin_code = 'b'' bill like bluecare
		 ins_policy_cover_kids varchar(50)		'../demographics/insurance1/policy/text()[contains (.,"ZXK")]', 
 		 ins_policy_bca varchar(50)	'../demographics/insurance1/policy/text()[contains (.,"ZXD")]', 

		 -- only need below 2 for lift location  and fin_code 'b' for capitated contract
		 group_no_lmbc varchar(50)	'../demographics/insurance1/groupno/text()[contains (.,"111926")]', 
		 group_no_lcbc varchar(50)	'../demographics/insurance1/groupno/text()[contains (.,"122607")]', 

		 attend_phy varchar(50)		'../demographics/visit/attendphy'
		 
		
	 ) xmlChrg
	 OUTER APPLY dbo.GetMasterAccount(xmlChrg.account)
left outer join cdm on cdm.cdm = xmlChrg.[billcode]
left outer join dict_cdm_conversion dc on dc.order_code = cdm.cdm
			and  (dc.ins_code = xmlChrg.ins_code OR dc.ins_code IS NULL)
			-- may need to add the insc.code also for L and H codes
			-- wont work if the ins_code is null		
) 
INSERT INTO chrg (
account	, fname	, mname, lname	,name_suffix,
pat_name		,pat_ssn		,unitno			--,[status]
,location	,responsiblephy	,fin_code		,qty			,order_site
,cdm		,mt_mnem		,[action]		,service_date	,performing_site
,facility	,istemp			,mt_req_no		,referencereq	,pat_dob	
,chrg_err	, bill_method	,mod_date		, mod_user		, mod_prg		, mod_host
,fin_type , comment ,post_file, net_amt--, age_on_date_of_service
)
--OUTPUT INSERTED.chrg_num--, colCpt4
--INTO amt
select --DISTINCT-- don't use distinct as some tests can be ordered multiple times on the same day for the patient
cteChrg.account, cteChrg.pat_lname,	cteChrg.pat_fname,	cteChrg.pat_mname,	cteChrg.pat_suffix
,	cteChrg.Patient,	cteChrg.pat_ssn,	cteChrg.unitno
,cteChrg.location, cteChrg.responsiblephy
, COALESCE(colFinCode,cteChrg.financialclass),cteChrg.quantity,	cteChrg.ordersite
,cteChrg.billcode AS [CDM],cteChrg.test,	cteChrg.action,	cteChrg.collectdate
,coalesce(cteChrg.performingsite,cteChrg.responsiblephy) as [performingsite]
, cteChrg.facility,cteChrg.istemp,cteChrg.number,cteChrg.referencereq,cteChrg.dob, NULL
,colBillMethod
, @modDate
, RIGHT(SUSER_SNAME(),50) AS [mod_user]
, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
	'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
, RIGHT (HOST_NAME(),50)  AS [mod_host]
,coalesce(fin.type,'C') AS [fin_type]
,'Batch: ' + @batch AS [comment]
,	@file AS [post_file]
,colCdmPrice --, cteChrg.client

from cteChrg
OUTER APPLY dbo.GetChrgBillMethod(cteChrg.client -- client
	, cteChrg.billcode -- cdm
	, (CASE WHEN (cteChrg.financialclass = 'B' AND ins_policy_cover_kids LIKE 'ZDK%') THEN 'D'
		WHEN (cteChrg.financialclass = 'B' AND ins_policy_bca like 'ZXD%') THEN 'BCA' --ELSE cteChrg.financialclass 
	   WHEN (cteChrg.financialclass = 'B' AND group_no_lmbc = '111926' AND cteChrg.location = 'LIFT' ) THEN 'LMBC'
	   WHEN (ctechrg.financialclass = 'B' AND group_no_lcbc = '122607' AND cteChrg.location = 'LIFT') THEN 'LCBC'
	   ELSE cteChrg.financialclass END) -- fin_code
	   , dbo.GetInsCode(ins_code,ins_name,cteChrg.financialclass) -- bill_to
	   , cteChrg.age --age
	   )
left outer join chrg on chrg.post_file = @file
left outer join fin on fin.fin_code = colFinCode --cteChrg.financialclass
left outer join client on client.cli_mnem = cteChrg.responsiblephy
where chrg.post_file is NULL 


SET @COUNT = @@ROWCOUNT
INSERT INTO temp_track(comment,ERROR)
VALUES('17. Chrg Table rows added ' + convert(varchar(10),@COUNT), @@ERROR)
INSERT INTO temp_track
VALUES('17. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
--print '17. Chrg Table rows added ' + convert(varchar(10),@COUNT)


INSERT INTO dbo.amt
		(
			chrg_num ,cpt4 ,type , amount , modi ,revcode ,
			diagnosis_code_ptr ,order_code ,mt_req_no, --bill_type 
			--,account ,fin_code ,description ,link ,payor ,age_on_service_date ,
			bill_method ,
			mod_date ,
			mod_user ,
			mod_prg 			
		)
SELECT 	
		dbo.chrg.chrg_num ,am.colCpt ,am.colType ,am.colCptPrice ,am.colModi ,am.colRevCode
		
		--,gpd.colDiagOrder AS [diag_code_ptr]
		, '1:' AS [diag_code_ptr]
		,cdm,chrg.mt_req_no --,dbo.chrg.bill_method
		--,dbo.chrg.account 	,dbo.chrg.fin_code ,am.colCptDescription ,am.colLink,dbo.chrg.bill_method,		age_on_date_of_service 
		,dbo.chrg.bill_method
		, @modDate
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		 	'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]		
		 FROM dbo.chrg
		-- CROSS APPLY dbo.GetPatDiagnosis(chrg.account) AS gpd
		 CROSS APPLY dbo.SplitCdmPriceByCpt(chrg.cdm, chrg.net_amt, dbo.chrg.fin_type) AS am
		 LEFT outer JOIN dbo.amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num		 
WHERE post_file = @file AND dbo.amt.chrg_num IS NULL

--ALTER TABLE chrg drop COLUMN age_on_date_of_service
--ALTER TABLE chrg ADD age_on_date_of_service AS NULLIF((datediff(year,coalesce([pat_dob],getdate()-(1)),coalesce([service_date],getdate()-(1)))),0)
--(datediff(year,coalesce([pat_dob],getdate()-(1)),coalesce([service_date],getdate()-(1))))
--SELECT pat_dob,NULLIF((datediff(year,coalesce([pat_dob],getdate()-(1)),coalesce([service_date],getdate()-(1)))),0)
--FROM chrg WHERE account = 'c7139356'

SET @COUNT = @@ROWCOUNT
INSERT INTO temp_track(comment,ERROR)
VALUES('18. Amt Table rows added ' + CONVERT(VARCHAR(10),@COUNT), @@ERROR)
INSERT INTO temp_track
VALUES('18. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
--PRINT '18. Amt Table rows added ' + CONVERT(VARCHAR(10),@COUNT)


INSERT INTO temp_track(comment,ERROR)
VALUES('For Batch '+ @batch, 'FINSIHED')

EXEC sp_xml_removedocument @idoc

--UPDATE dbo.XmlSourceTable SET processed = 1 WHERE import_file = @file
INSERT INTO temp_track(comment,ERROR)
VALUES ('File processed '+ CAST(GETDATE() AS  varchar(17)), @@ERROR)

INSERT INTO temp_track(comment,ERROR)
VALUES ('End Time '+ CAST(GETDATE() AS  varchar(17)), @@ERROR)

declare @tableHtml nvarchar(max);
declare @sub varchar(250)
select @tableHtml = (
N'<H1> XML POST CHARGE </H1>'+
N'<table border = "1" bordercolor ="blue">'+
N'<tr bgcolor ="blue"><th>ACTION</th><th>ERROR</th></tr>' +

CAST (( select td = comment,'',
		td = error,''
from temp_track
for XML PATH('tr'), TYPE) as NVARCHAR(MAX))+
N'</Table>')


if (len(@tableHtml) > 0)
begin
set @sub = 'XML Post Charge as of  ' + convert(varchar(17),getdate(),109)
EXEC msdb.dbo.sp_send_dbmail
@profile_name = 'Outlook',
@recipients = 'david.kelly@wth.org;bradley.powers@wth.org',
--@recipients = 'carol.sellars@wth.org;jan.smith@wth.org;cheryl.lane@wth.org',
--@blind_copy_recipients=N'bradley.powers@wth.org; david.kelly@wth.org',
@body = @tableHtml,
@subject = @sub,
@body_format = 'HTML';

PRINT 'EMAIL SENT'
end
UPDATE dbo.XmlSourceTable
SET processed = 1
WHERE import_file = @file
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
