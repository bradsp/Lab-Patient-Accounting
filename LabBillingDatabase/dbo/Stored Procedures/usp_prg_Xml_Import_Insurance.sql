-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import_Insurance
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
		ORDER BY import_file) )

--	INSERT INTO temp_track(comment,row_count,ERROR)
--	VALUES('Insurance Start ' ,NULL, @file)

BEGIN TRY
BEGIN TRANSACTION

	DECLARE @data VARCHAR(MAX)
	SET @data = (select doc_data FROM dbo.XmlSourceTable WHERE import_file = @file)
	DECLARE @doc XML
	PRINT 'BEFORE insA select count'
	SET @doc = (SELECT CONVERT(XML,@data))
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/demographics/insurance1)))','int') AS [ACCOUNTS])
	
	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('Primary insurance Records  ' ,@idoc, @@ERROR)
	
	PRINT 'BEFORE insB select count'
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/demographics/insurance2)))','int') AS [ACCOUNTS])

	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('Secondary insurance Records  ' ,@idoc, @@ERROR)

	PRINT 'BEFORE insC select count'
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/demographics/insurance3)))','int') AS [ACCOUNTS])

	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('Tertiary insurance Records  ' ,@idoc, @@ERROR)
	
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
		,COALESCE(financialclass,facility) AS [fin_code]
		,COALESCE(NULLIF(ins_code,''),ins_code) AS [ins_code]
		, dbo.FormatRelation(ins_relation) AS [ins_relation]
		, location
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance1',2 )
	with (

			account		varchar(15)				'account',
			ins_code varchar(10)				'mnem',
			ins_plan_name varchar(45)			'name',
			policy		varchar(50)				'policy',
			ins_group_name	varchar(50)			'group',
			ins_group_no varchar(50)			'groupno',
			subscriber	varchar(50)				'subscriber',
			ins_relation varchar(10)			'relation'	,
			subscriber_dob varchar(12)			'holderdob',
			subscriber_sex	varchar(1)			'../gender',
			subscriber_addr varchar(40)			'../addr1',
			subscriber_city varchar(40)			'../guarantor/city',
			subscriber_st varchar(40)			'../guarantor/state',
			subscriber_zip varchar(40)			'../guarantor/zip',
			facility varchar(10)				'../../charge/facility',
			client VARCHAR(10)					'../../charge/responsiblephy',
			location varchar(10)				'../../charge/location',
			financialclass varchar(10)			'../../charge/financialclass'
				 ) xmlIns
				 
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
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
	, CASE WHEN cteInsA.ins_group_no = '111926' AND cteInsA.fin_code = 'B' AND cteInsA.location = 'LIFT' THEN 'LMBC'
		   WHEN cteInsA.ins_group_no = '122607' AND cteInsA.fin_code = 'B' AND cteInsA.location = 'LIFT'  THEN 'LCBC'
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
	INSERT INTO temp_track(comment,row_count, ERROR)
	VALUES('InsA Table rows added ' ,@COUNT, @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('12. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	print 'InsA Table rows added ' + convert(varchar(10),@COUNT)

COMMIT TRANSACTION

BEGIN TRANSACTION

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
		,dbo.FormatRelation(ins_relation) AS [ins_relation]
		,location
		from OPENXML (@idoc, '/postingbatch/encounter/demographics/insurance2',2 )
	with (
			account			varchar(17)	'account'
			,ins_code		varchar(12)	'mnem',
			ins_plan_name	varchar(45)	'name',
			policy			varchar(50)	'policy',
			ins_group_name	varchar(50)	'group',
			ins_group_no	varchar(50)	'groupno',
			ins_relation	VARCHAR(10) 'relation',
			subscriber		varchar(50)	'subscriber',
			subscriber_dob  VARCHAR(12)	'holderdob',
			subscriber_sex	varchar(3)	'holdersex',
			location varchar(10)		'../../charge/location'
		 ) xmlIns
		left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
		OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
	)
	INSERT INTO ins
	(account,ins_a_b_c,holder_nme,holder_dob,holder_sex,plan_nme,plan_addr1,plan_addr2
	,p_city_st,policy_num,grp_nme,grp_num,fin_code,ins_code,relation
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
		, CASE WHEN cteInsB.ins_group_no = '11926' AND cteInsB.fin_code = 'B' AND cteInsB.location = 'LIFT' THEN 'LMBC'
				WHEN cteInsB.ins_group_no = '122607' AND cteInsB.fin_code = 'B' AND cteInsB.location = 'LIFT'  THEN 'LCBC'
				ELSE cteInsB.ins_code END AS [ins_code]
	, cteInsB.ins_relation
	, @modDate as [mod_date]	
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 from cteInsB
	left outer join ins on ins.account = cteInsB.account and ins.ins_a_b_c = cteInsB.ins_priority
	where  ins.account is null and ins.ins_a_b_c is NULL AND cteInsB.rn = 1

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('InsB Table rows added ', @COUNT, @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('13. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	print 'InsB Table rows added ' + convert(varchar(10),@COUNT)
	
COMMIT TRANSACTION

BEGIN TRANSACTION
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
		,dbo.FormatRelation(ins_relation) AS [ins_relation]
		,location
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
			subscriber_sex	VARCHAR(3)	'subscribersex'	,
			ins_relation varchar(10)		'relation',
			location varchar(10)		'../../charge/location'	
				 ) xmlIns
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			OUTER APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
	)
	INSERT INTO ins
	(account,		ins_a_b_c,			holder_nme,
	 holder_dob, holder_sex
	,	plan_nme,			plan_addr1,			plan_addr2
	,p_city_st,		policy_num,			grp_nme,			grp_num			
	,fin_code,		ins_code, relation
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
		, CASE WHEN cteInsC.ins_group_no = '11926' AND cteInsC.fin_code = 'B' AND cteInsC.location = 'LIFT'  THEN 'LMBC'
				WHEN cteInsC.ins_group_no = '122607' AND cteInsC.fin_code = 'B' AND cteInsC.location = 'LIFT'  THEN 'LCBC'
				ELSE cteInsC.ins_code END AS [ins_code]
	, cteInsC.ins_relation
	, @modDate as [mod_date]	
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	 from cteInsC
	left outer join ins on ins.account = cteInsC.account and ins.ins_a_b_c = cteInsC.ins_priority
	where  ins.account is null and ins.ins_a_b_c is NULL AND cteInsC.rn = 1

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('InsC Table rows added ' ,@COUNT, @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('14. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	print 'InsC Table rows added ' + convert(varchar(10),@COUNT)
COMMIT TRANSACTION

BEGIN TRANSACTION

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
		,dbo.FormatRelation(ins_relation) AS [ins_relation]
		,location
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
			subscriber_sex	VARCHAR(3)	'subscribersex',
			ins_relation varchar(10)		'relation',
			location varchar(10)				'../../charge/location'	
			 ) xmlIns
			left outer join insc on insc.code = xmlIns.ins_code and insc.deleted = 0
			outer APPLY dbo.GetMasterAccount(dbo.AccountTrim(xmlIns.account))
		
	)
	INSERT INTO ins
	(account,		ins_a_b_c,			holder_nme,	holder_dob, holder_sex,		
	plan_nme,			plan_addr1,			plan_addr2
	,p_city_st,		policy_num,			grp_nme,			grp_num			
	,fin_code,		ins_code, relation
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
		, CASE WHEN cteInsD.ins_group_no = '11926' AND cteInsD.fin_code = 'B' AND cteInsD.location = 'LIFT' THEN 'LMBC'
				WHEN cteInsD.ins_group_no = '122607' AND cteInsD.fin_code = 'B'  AND cteInsD.location = 'LIFT' THEN 'LCBC'
				ELSE cteInsD.ins_code END AS [ins_code]
		,cteInsD.ins_relation
		, @modDate as [mod_date]	
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
			'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
		, RIGHT (HOST_NAME(),50)  AS [mod_host]

	 from cteInsD
	left outer join ins on ins.account = cteInsD.account and ins.ins_a_b_c = cteInsD.ins_priority
	where  ins.account is null and ins.ins_a_b_c is NULL AND cteInsD.rn = 1

	SET @COUNT = @@ROWCOUNT
	INSERT INTO temp_track(comment,row_count,ERROR)
	VALUES('InsD Table rows added ' ,@COUNT, @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('15. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	print 'InsD Table rows added ' + convert(varchar(10),@COUNT)
COMMIT TRANSACTION
END TRY		
	
BEGIN CATCH
    
	IF (XACT_STATE()) = -1
    BEGIN
		INSERT INTO dbo.temp_track
				( comment, row_count, error )
		VALUES	( N'The transaction is in an uncommittable state.' +
		            'Rolling back transaction.', -- comment - varchar(8000)
					NULL, -- row_count - int
					ERROR_MESSAGE() -- error - varchar(8000)
					)
--        PRINT
--            N'The transaction is in an uncommittable state.' +
--            'Rolling back transaction.'
        ROLLBACK TRANSACTION;
    END;

    -- Test whether the transaction is committable.
    IF (XACT_STATE()) = 1
    BEGIN
		INSERT INTO dbo.temp_track
				( comment, row_count, error )
		VALUES	( N'The transaction is committable.' +
			          'Committing transaction.', -- comment - varchar(8000)
					NULL, -- row_count - int
					'' -- error - varchar(8000)
					)
--        PRINT
--            N'The transaction is committable.' +
--            'Committing transaction.'
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









