-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import_Insurance_Verify
	-- Add the parameters for the stored procedure here
	@file VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON; -- this allows the transaction code to work.

    -- Insert statements for procedure here


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
	--DECLARE @file varchar(50)
	SET @file = COALESCE(NULLIF(@file,''), (select TOP(1) import_file FROM dbo.XmlSourceTable WHERE processed = 0 
		ORDER BY import_file) )


--BEGIN TRY
--BEGIN TRANSACTION

	DECLARE @data VARCHAR(MAX)
	SET @data = (select TOP(1) doc_data FROM dbo.XmlSourceTable WHERE import_file = @file)
	DECLARE @doc xml
	SET @doc = (SELECT CONVERT(XML,@data))
	SET @idoc = 
	(SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/demographics/insurance1)))','int') AS [ACCOUNTS])
	PRINT @idoc
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/demographics/insurance2)))','int') AS [ACCOUNTS])
	PRINT	 @idoc
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/demographics/insurance3)))','int') AS [ACCOUNTS])
	PRINT	 @idoc

	EXEC sp_xml_preparedocument @idoc OUTPUT, @doc


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
		,REPLACE(ins_group_name,'"','') AS [ins_group_name]		
		,REPLACE(ins_group_no,'"','') AS [ins_group_no]
		,dbo.FormatFinCode(financialclass, facility) AS [fin_code]
		,COALESCE(NULLIF(ins_code,''),ins_code) AS [ins_code]
		, dbo.FormatRelation(ins_relation) AS [ins_relation]
		, location
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance1',2 )
	with (

			account		varchar(15)			'account',
			ins_code varchar(10)			'mnem',
			ins_plan_name varchar(45)		'name',
			policy		varchar(50)			'policy',
			ins_group_name	varchar(50)		'group',
			ins_group_no varchar(50)		'groupno',
			subscriber	varchar(50)			'subscriber',
			ins_relation varchar(10)		'relation'	,
			subscriber_dob varchar(12)		'holderdob',
			subscriber_sex	varchar(1)		'../gender',--'../../demographics/patient/gender',
			subscriber_addr varchar(40)		'../addr1',
			subscriber_city varchar(40)		'../guarantor/city',--../demographics/guarantor/city',
			subscriber_st varchar(40)		'../guarantor/state',
			subscriber_zip varchar(40)		'../guarantor/zip',
			facility varchar(10)			'../../charge/facility',
			client VARCHAR(10)				'../../charge/responsiblephy',
			location VARCHAR(10)			'../../charge/location',
			financialclass varchar(10)		'../../charge/financialclass'
				 ) xmlIns
				 
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
			
	)

	select distinct
	cteInsA.account
--	, cteInsA.location
		,UPPER(cteInsA.ins_priority) AS [ins_priority]
			,UPPER(cteInsA.subscriber) AS [subscriber]
				,cteInsA.subscriber_dob AS [subscriber_dob]
					,UPPER(cteInsA.subscriber_sex) AS [subscriber_sex]
	,UPPER(cteInsA.subscriber_addr) AS [subscriber_addr]
		,UPPER(cteInsA.subscriber_city_st_zip) AS [subscriber_city_st_zip]
			,UPPER(cteInsA.ins_plan_name) AS [ins_plan_name]
				,UPPER(cteInsA.addr1) AS [addr1]
					,UPPER(cteInsA.addr2) AS [addr2]
	,UPPER(cteInsA.citystzip) AS [citystzip]
		,UPPER(cteInsA.policy) AS [policy]
			,UPPER(cteInsA.ins_group_name) AS [group_name]
				,cteInsA.ins_group_no
	,UPPER(cteInsA.fin_code)
	, CASE WHEN cteInsA.ins_group_no = '111926' AND cteInsA.fin_code = 'B' AND location = 'LIFT' THEN 'LMBC'
		   WHEN cteInsA.ins_group_no = '122607' AND cteInsA.fin_code = 'B' AND location = 'LIFT' THEN 'LCBC'
		   ELSE cteInsA.ins_code END AS [ins_code]
	,cteInsA.ins_relation
	, @modDate as [mod_date]	
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 from cteInsA
	 CROSS APPLY dbo.GetInsurance(cteInsA.account)


	SET @COUNT = @@ROWCOUNT
	print 'InsA Table rows added ' + convert(varchar(10),@COUNT)

/*INSURANCE B*/

;with cteInsC
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
		,REPLACE(ins_group_name,'"','') AS [ins_group_name]		
		,REPLACE(ins_group_no,'"','') AS [ins_group_no]
		,dbo.FormatFinCode(financialclass, facility) AS [fin_code]
		,COALESCE(NULLIF(ins_code,''),ins_code) AS [ins_code]
		, dbo.FormatRelation(ins_relation) AS [ins_relation]
		, location
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance2',2 )
	with (

			account		varchar(15)			'account',
			ins_code varchar(10)			'mnem',
			ins_plan_name varchar(45)		'name',
			policy		varchar(50)			'policy',
			ins_group_name	varchar(50)		'group',
			ins_group_no varchar(50)		'groupno',
			subscriber	varchar(50)			'subscriber',
			ins_relation varchar(10)		'relation'	,
			subscriber_dob varchar(12)		'holderdob',
			subscriber_sex	varchar(1)		'../gender',--'../../demographics/patient/gender',
			subscriber_addr varchar(40)		'../addr1',
			subscriber_city varchar(40)		'../guarantor/city',--../demographics/guarantor/city',
			subscriber_st varchar(40)		'../guarantor/state',
			subscriber_zip varchar(40)		'../guarantor/zip',
			facility varchar(10)			'../../charge/facility',
			client VARCHAR(10)				'../../charge/responsiblephy',
			location VARCHAR(10)			'../../charge/location',
			financialclass varchar(10)		'../../charge/financialclass'
				 ) xmlIns
				 
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
			
	)

	select distinct
	cteInsC.account
--	, cteInsC.location
		,UPPER(cteInsC.ins_priority) AS [ins_priority]
			,UPPER(cteInsC.subscriber) AS [subscriber]
				,cteInsC.subscriber_dob AS [subscriber_dob]
					,UPPER(cteInsC.subscriber_sex) AS [subscriber_sex]
	,UPPER(cteInsC.subscriber_addr) AS [subscriber_addr]
		,UPPER(cteInsC.subscriber_city_st_zip) AS [subscriber_city_st_zip]
			,UPPER(cteInsC.ins_plan_name) AS [ins_plan_name]
				,UPPER(cteInsC.addr1) AS [addr1]
					,UPPER(cteInsC.addr2) AS [addr2]
	,UPPER(cteInsC.citystzip) AS [citystzip]
		,UPPER(cteInsC.policy) AS [policy]
			,UPPER(cteInsC.ins_group_name) AS [group_name]
				,cteInsC.ins_group_no
	,UPPER(cteInsC.fin_code)
	, CASE WHEN cteInsC.ins_group_no = '111926' AND cteInsC.fin_code = 'B' AND location = 'LIFT' THEN 'LMBC'
		   WHEN cteInsC.ins_group_no = '122607' AND cteInsC.fin_code = 'B' AND location = 'LIFT' THEN 'LCBC'
		   ELSE cteInsC.ins_code END AS [ins_code]
	,cteInsC.ins_relation
	, @modDate as [mod_date]	
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 from cteInsC
	 CROSS APPLY dbo.GetInsurance(cteInsC.account)


	SET @COUNT = @@ROWCOUNT
	print 'InsB Table rows added ' + convert(varchar(10),@COUNT)

/*INSURANCE C*/

;with cteInsC
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
		,REPLACE(ins_group_name,'"','') AS [ins_group_name]		
		,REPLACE(ins_group_no,'"','') AS [ins_group_no]
		,dbo.FormatFinCode(financialclass, facility) AS [fin_code]
		,COALESCE(NULLIF(ins_code,''),ins_code) AS [ins_code]
		, dbo.FormatRelation(ins_relation) AS [ins_relation]
		, location
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance3',2 )
	with (

			account		varchar(15)			'account',
			ins_code varchar(10)			'mnem',
			ins_plan_name varchar(45)		'name',
			policy		varchar(50)			'policy',
			ins_group_name	varchar(50)		'group',
			ins_group_no varchar(50)		'groupno',
			subscriber	varchar(50)			'subscriber',
			ins_relation varchar(10)		'relation'	,
			subscriber_dob varchar(12)		'holderdob',
			subscriber_sex	varchar(1)		'../gender',--'../../demographics/patient/gender',
			subscriber_addr varchar(40)		'../addr1',
			subscriber_city varchar(40)		'../guarantor/city',--../demographics/guarantor/city',
			subscriber_st varchar(40)		'../guarantor/state',
			subscriber_zip varchar(40)		'../guarantor/zip',
			facility varchar(10)			'../../charge/facility',
			client VARCHAR(10)				'../../charge/responsiblephy',
			location VARCHAR(10)			'../../charge/location',
			financialclass varchar(10)		'../../charge/financialclass'
				 ) xmlIns
				 
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
			
	)

	select distinct
	cteInsC.account
--	, cteInsC.location
		,UPPER(cteInsC.ins_priority) AS [ins_priority]
			,UPPER(cteInsC.subscriber) AS [subscriber]
				,cteInsC.subscriber_dob AS [subscriber_dob]
					,UPPER(cteInsC.subscriber_sex) AS [subscriber_sex]
	,UPPER(cteInsC.subscriber_addr) AS [subscriber_addr]
		,UPPER(cteInsC.subscriber_city_st_zip) AS [subscriber_city_st_zip]
			,UPPER(cteInsC.ins_plan_name) AS [ins_plan_name]
				,UPPER(cteInsC.addr1) AS [addr1]
					,UPPER(cteInsC.addr2) AS [addr2]
	,UPPER(cteInsC.citystzip) AS [citystzip]
		,UPPER(cteInsC.policy) AS [policy]
			,UPPER(cteInsC.ins_group_name) AS [group_name]
				,cteInsC.ins_group_no
	,UPPER(cteInsC.fin_code)
	, CASE WHEN cteInsC.ins_group_no = '111926' AND cteInsC.fin_code = 'B' AND location = 'LIFT' THEN 'LMBC'
		   WHEN cteInsC.ins_group_no = '122607' AND cteInsC.fin_code = 'B' AND location = 'LIFT' THEN 'LCBC'
		   ELSE cteInsC.ins_code END AS [ins_code]
	,cteInsC.ins_relation
	, @modDate as [mod_date]	
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 from cteInsC
	 CROSS APPLY dbo.GetInsurance(cteInsC.account)


	SET @COUNT = @@ROWCOUNT
	print 'InsC Table rows added ' + convert(varchar(10),@COUNT)
--COMMIT TRANSACTION
--END TRY		
	
--BEGIN CATCH
--    
--	IF (XACT_STATE()) = -1
--    BEGIN
--        PRINT
--            N'The transaction is in an uncommittable state.' +
--            'Rolling back transaction.'
--        ROLLBACK TRANSACTION;
--    END;
--
--    -- Test whether the transaction is committable.
--    IF (XACT_STATE()) = 1
--    BEGIN
--        PRINT
--            N'The transaction is committable.' +
--            'Committing transaction.'
--        COMMIT TRANSACTION;   
--    END;
--    INSERT INTO dbo.temp_track
--    		( comment, error )
--    VALUES	( 'ERROR: TRANSACTION ROLLED BACK', -- comment - varchar(8000)
--    			ERROR_MESSAGE()  -- error - varchar(8000)
--    			)
--    
--	INSERT INTO dbo.error_prg
--            ( error_type,
--            [app_name] ,
--              app_module ,
--              error ,
--              mod_date ,
--              mod_prg ,
--              mod_user ,
--              mod_host
--            )
--SELECT	'SQL',
--            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 )AS [mod_prg],
--            ERROR_PROCEDURE() AS ErrorProcedure,
--            ERROR_MESSAGE() AS ErrorMessage,
--            GETDATE(),
--            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 ) AS ErrorProcedure,
--            RIGHT(SUSER_SNAME(),50),
--            RIGHT (HOST_NAME(),50);
--        
--END CATCH;



END	









