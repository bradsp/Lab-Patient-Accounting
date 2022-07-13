-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import_Accounts_Verify
	-- Add the parameters for the stored procedure here
	@file VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON; -- this allows the transaction code to work.

    -- Insert statements for procedure here
--	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[temp_track]') AND type in (N'U'))
--	BEGIN
--	CREATE TABLE [dbo].[temp_track](
--		[comment] [varchar](8000) NOT NULL,
--		[error] [VARCHAR] (8000) NULL
--		)
--	END

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
	DECLARE @doc xml	
	SET @file =NULLIF(@file,'')

	IF (@file IS NOT NULL)
	BEGIN
		DECLARE @data VARCHAR(MAX)
		SET @data = (select TOP(1) doc_data FROM dbo.XmlSourceTable WHERE import_file = @file
			ORDER BY import_file)
		SET @doc = (SELECT CONVERT(XML,@data))
	END
	ELSE
	BEGIN
	set @doc  = (select convert(XML,replace(replace(BulkColumn,'&','&amp;'),'"','')) as BulkColumn
	--from OPENROWSET (BULK '\\MCL03\ftproot\billing-xml\processed\20140702.xml', SINGLE_BLOB) as x)
	from OPENROWSET (BULK '\\wthmclbill\MedicareRemit\POST_CHARGE\20140702.XML', SINGLE_BLOB) as x)

	END
	
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/@ID)))','int') AS [ACCOUNTS])
	PRINT '1.  Accounts ' +CONVERT(VARCHAR(10),@idoc)

	SET @idoc = (SELECT @doc.value('count(data(/postingbatch/encounter/charge))','int') AS [CHARGES])
	PRINT '2.  Charges ' +CONVERT(VARCHAR(10),@idoc)
 
	EXEC sp_xml_preparedocument @idoc OUTPUT, @doc


	/* do the duplicate checking before posting any of these records. The meditech_requisition number can be used to seperate the 	merged accounts later.*/

	
--	TRUNCATE TABLE dbo.acc_dup_check
--	-- 1. Add accounts to acc_dup_check so it's trigger can find the master account if one already exists.
-- fin code and client notes
/*
	if 
		charge/facility = 'MRP' then charge/financialclass is the fin_code
	else
		the fin code = 'Y' for the affiliate hospitals
	------------	
	if 
		charge/facility = 'MRP' then client is charge/responsiblephy trimming the leading 'z'
	else
		the client is convert 'BG,CG,MG' to the corresponding client.		
		
*/
	; WITH cteCheckForDups
	AS(
	SELECT TOP(100) PERCENT
	dbo.AccountTrim(a.alias.value('(charge/account/text())[1]', 'varchar(15)')) AS 'account' 
		, a.alias.value('(charge/collectdate/text())[1]', 'datetime') AS 'collectdate'
		, dbo.FormatFinCode(a.alias.value('(charge/financialclass/text())[1]', 'varchar(50)'),
					a.alias.value('(charge/facility/text())[1]','varchar(10)')				
				) AS 'fin_code'
		, REPLACE(a.alias.value('(charge/patientname/text())[1]', 'varchar(50)'),'"','') AS 'pat_name'
		, dbo.FormatSSN(a.alias.value('(charge/patientssno/text())[1]', 'varchar(13)')) AS 'pat_ssn'
		, dbo.DateOfBirthFix(a.alias.value('(demographics/patient/dob/text())[1]'
			, 'varchar(10)')) AS 'pat_dob'
		, dbo.FormatClient( a.alias.value('(charge/facility/text())[1]','varchar(10)'),
			 a.alias.value('(charge/responsiblephy/text())[1]','varchar(10)') )AS [client]
	FROM (SELECT @doc AS response_xml) r
	CROSS APPLY r.response_xml.nodes('postingbatch/encounter') a(alias)
	WHERE a.alias.value('(charge/facility/text())[1]', 'varchar(15)') NOT IN ('unk','UNK') -- =	'BIRD,BRANDI'
	--OR a.alias.value('(charge/location/text())[1]','varchar(15)') NOT IN ('LPT')
--	ORDER BY 
--	dbo.AccountTrim(a.alias.value('(charge/account/text())[1]', 'varchar(15)')) 
--		, a.alias.value('(charge/collectdate/text())[1]', 'datetime')
--		, dbo.FormatFinCode(a.alias.value('(charge/financialclass/text())[1]', 'varchar(50)'),
--					a.alias.value('(charge/facility/text())[1]','varchar(10)')
--				)
--		, REPLACE(a.alias.value('(charge/patientname/text())[1]', 'varchar(50)'),'"','')
--		, dbo.FormatSSN(a.alias.value('(charge/patientssno/text())[1]', 'varchar(13)'))
--		, dbo.DateOfBirthFix(a.alias.value('(//patient/dob/text())[1]', 'varchar(10)')) 
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
			WHERE client <> 'LPT'
--ORDER BY client,fin_code,account
ORDER BY REPLACE(pat_name,'"','')  desc

	
--	--PRINT '3.  Acc_dup_check inserted records '+CONVERT(VARCHAR(4),@COUNT)
--	/* removed per carols request
--	EXEC usp_WriteAccErrors
--	SET @COUNT = @@ROWCOUNT
--	
--	--PRINT ('4.  Wrote [ ' + CAST(@COUNT AS VARCHAR(10))+' ] account errors to chrg_err table.')
--	*/
--	-- then update
--	; WITH cte
--	AS
--	(
--	SELECT   DISTINCT TOP(100) PERCENT
--	DENSE_RANK() OVER (PARTITION BY pat_name,pat_ssn,pat_dob, fin_code, service_date
--					ORDER BY service_date, dbo.AccountTrim(account)
--					) AS [DR],
--        dbo.AccountTrim(account) AS [account],
--        pat_name , service_date, client
--        FROM dbo.acc_dup_check
--        WHERE master_account IS NULL --AND pat_name = 'COLEMAN,VIRGINIA HUBBARD'
--        ORDER BY dbo.AccountTrim(account), pat_name
--	)
----	UPDATE dbo.acc_dup_check
----	SET master_account = q.account
--	SELECT * 
--	FROM dbo.acc_dup_check
--	INNER JOIN
--	(
--	SELECT cte1.account ,cte.account AS [master_account]
--	FROM cte
--	CROSS JOIN cte AS cte1
--	WHERE cte.DR = cte1.DR+1
--	AND cte.pat_name = cte1.pat_name AND cte.client = cte1.client) AS [q] ON q.master_account = dbo.acc_dup_check.account
--
--	
--	--PRINT '5.  Acc_dup_check updated records ' + CONVERT(VARCHAR(4),@COUNT)
--
--	---- acc_dup_check has duplicates from the file
--	---- acc_merges is the master file
--	---- 1. find out if the master account is a dup_acc
--
--	; WITH cteDups
--	AS
--	(
--	SELECT 
--	ROW_NUMBER() OVER (PARTITION BY ac.master_account,ac.account 
--		ORDER BY ac.master_account, ac.account) AS [rn],
--	ac.master_account, ac.account,ac.pat_ssn, ac.service_date,ac.fin_code 
--	,@file AS [import_file]
--	, @modDate AS [mod_date]
--	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
--	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
--		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
--	, RIGHT (HOST_NAME(),50)  AS [mod_host]
--	FROM dbo.acc_dup_check ac
--	LEFT OUTER JOIN dbo.acc_merges am ON am.account = ac.master_account
--		AND am.dup_acc  = ac.account
--	WHERE am.account IS NULL AND ac.master_account IS not NULL  
--	)
----	INSERT INTO dbo.acc_merges ( account , dup_acc, pat_ssn, service_date,fin_code,
----		xml_file,	mod_date, mod_user, mod_prg, mod_host )
--	SELECT --rn ,
--			master_account ,
--			account ,
--			pat_ssn ,
--			service_date ,
--			fin_code ,
--			import_file ,
--			mod_date ,
--			mod_user ,
--			mod_prg ,
--			mod_host FROM cteDups
--	where rn = 1
--
--
--	SET @COUNT = @@ROWCOUNT
--	
--	--PRINT '6.  Acc_merges inserted records ' + CONVERT(VARCHAR(4),@COUNT)
--
--
--	-- 2. find out if the account is a dup_acc
----	INSERT INTO dbo.acc_merges ( account , dup_acc, pat_ssn, service_date,fin_code,
----		xml_file,	mod_date, mod_user, mod_prg, mod_host  )
--		
--	SELECT TOP(10) ac.master_account, ac.account,ac.pat_ssn, ac.service_date,ac.fin_code 
--	,@file
--	, @modDate AS [mod_date]
--	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
--	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
--		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
--	, RIGHT (HOST_NAME(),50)  AS [mod_host]
--	FROM dbo.acc_dup_check ac
--	LEFT OUTER JOIN dbo.acc_merges am ON am.dup_acc = ac.master_account
--		OR am.dup_acc = ac.account
--	WHERE ac.master_account IS NOT NULL AND am.account IS NULL AND am.dup_acc IS NULL
--
--	SET @COUNT	= @@ROWCOUNT
--	PRINT '7.  Acc_merges inserted records ' + CONVERT(VARCHAR(4),@COUNT)
--	 
--	 --set the xml_info into acc_merges for the duplicate accounts 
----	UPDATE dbo.acc_merges
----	SET xml_info = colXml
----	--select *
----	FROM dbo.acc_merges
----	CROSS APPLY dbo.GetXmlDataForAcc (acc_merges.dup_acc,dbo.acc_merges.xml_file)
----	WHERE xml_info IS NULL AND dbo.acc_merges.xml_file = @file
----
--
--	
--	PRINT '8.  Acc_merges updated records ' + CONVERT(VARCHAR(4),@count)

	-- 2. if the account is not in the account table add it.
	; with newAccs as
	(
	select distinct 
		ROW_NUMBER() OVER (PARTITION BY COALESCE(colAcc,dbo.AccountTrim(lacc.account))
			ORDER BY COALESCE(colAcc,dbo.AccountTrim(lacc.account))) AS rn
		,COALESCE(colAcc,dbo.AccountTrim(lacc.account)) AS [account]
		,collectdate
		,ovacct
		,dbo.FormatFinCode(financialclass, facility) as [financialclass]
		,dbo.FormatClient(facility ,	lacc.client ) AS [client]
		,LEFT(REPLACE(patient,'"',''),40) AS [patient]
		,dbo.FormatSSN(lacc.pat_ssn) as [pat_ssn]
		, REPLACE(pat_mri,'"','') AS [pat_mri]
		, dbo.DateOfBirthFix(lacc.pat_dob) as [pat_dob]
--		, COALESCE(CASE WHEN location = 'LIFT' AND ins_group IS NOT NULL
--						THEN	ins_group
--						ELSE NULL END,ins_group,facility) AS [location]
--		, ins_group	
		, lacc.client AS [err]
		,location
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
			 client varchar(10)			'../charge/responsiblephy',
			-- ins_group VARCHAR(50)		'insurance1/groupno/text()[contains (.,"111926") or contains(.,"122607")]',
			 location VARCHAR(50)		'../charge/location [. != "LPT"]' 
			 
		 ) lacc
		 OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(lacc.account))
		
		 
	) 
	
	select  distinct 
		newAccs.account, newAccs.patient
	, CASE WHEN newAccs.client LIKE 'ERR%' THEN newAccs.err
		ELSE newAccs.client END AS [client]
	
	, CASE WHEN newAccs.client LIKE 'ERR%' THEN 'ERR'
		   WHEN newAccs.financialclass = 'U' THEN 'ERR'
		ELSE 'NEW' END AS [status]
	
	, [financialclass]
	, CASE WHEN newAccs.financialclass = 'U' THEN 'U Financial class is invalid'
		ELSE [err] END  AS [err]
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
	where --(newAccs.account = 'C4054366') AND 	
	rn = 1
	ORDER BY newAccs.patient

	
	print '9.  Acc Table rows that would have been added ' + convert(varchar(10),@count)
	EXEC sp_xml_removedocument @idoc

	
	


END


