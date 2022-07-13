-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import_Patients_Verify
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
	DECLARE @doc xml
	DECLARE @idoc int
	DECLARE @COUNT int
	SET @startTime = getdate()
	SET @file =NULLIF(@file,'')
	
	IF (@file IS NOT NULL)
	BEGIN
		
--	SET @file = (COALESCE(NULLIF(@file,'') ,(select TOP(1) import_file FROM dbo.XmlSourceTable 
--			WHERE processed = 0 
--		ORDER BY import_file) ))

	DECLARE @data VARCHAR(MAX)
	SET @data = (select TOP(1) doc_data FROM dbo.XmlSourceTable WHERE import_file = @file
		ORDER BY import_file)
	END
	
	SET @doc = (SELECT CONVERT(XML,@data))
	
	
	EXEC sp_xml_preparedocument @idoc OUTPUT, @doc


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
					ORDER BY COALESCE(colAcc, dbo.AccountTrim(FTable.account))) as [rn]
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
			, dbo.FormatPhoneNo(NULLIF(guar_phone,'^^^noemail@wth.org')) as 	guar_phone
			,  REPLACE(icd9_1,'"','') AS [icd9_1],  REPLACE(icd9_2,'"','') AS [icd9_2]
			,  REPLACE(icd9_3,'"','') AS [icd9_3],  REPLACE(icd9_4,'"','') AS [icd9_4]
			,  REPLACE(icd9_5,'"','') AS [icd9_5],  REPLACE(icd9_6,'"','') AS [icd9_6]
			,  REPLACE(icd9_7,'"','') AS [icd9_7],  REPLACE(icd9_8,'"','') AS [icd9_8]
			,  REPLACE(icd9_9,'"','') AS [icd9_9]
			
			, dbo.FormatPhoneNo(NULLIF(pat_phone,'^^^noemail@wth.org')) as pat_phone
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
			, CASE WHEN mailer = 'E' THEN 'Y'
				ELSE 'N' END AS mailer

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
			mailer VARCHAR(2)		'../visit/financialclass',
			relation varchar(14)	'../guarantor/relation',
			guarantor varchar(40)	'../guarantor/name',
			guar_addr varchar(40)	'../guarantor/addr1',
			guar_city varchar(50)	'../guarantor/city',
			guar_state varchar(4)	'../guarantor/state',
			guar_zip varchar(12)	'../guarantor/zip',
			--guar_phone varchar(25)	'../guarantor/homephone[. != "^^^noemail@wth.org"]',
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
			--pat_phone varchar(25)	'../patient/homephone[. != "^^^noemail@wth.org"]',
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
--	INSERT INTO pat ( 
--	 account,	 pat_full_name,	ssn
--	, pat_addr1,		pat_addr2, city_st_zip 
--	, pat_city, pat_state, pat_zip 
--	, dob_yyyy,		sex,	relation
--	, pat_marital,	pat_race, pat_phone
--	, icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 
--	, guarantor, guar_addr, guar_city, guar_state, guar_zip , g_city_st , guar_phone
--	, phy_id,		phy_comment
--	, mod_date, mod_user, mod_prg, mod_host
--	) 
SELECT * FROM ctePat
--	select  distinct UPPER(ctePat.account) AS [account]--, ctePat.master_account
----	, ctePat.mailer
--	, UPPER(ctePat.pat_name) AS [pat_name]
--	, ctePat.pat_ssn as [pat_ssn]
--	, UPPER(ctePat.pat_addr1) AS [pat_addr1]
--	, UPPER(ctePat.pat_addr2) AS [pat_addr2]
--	, UPPER(ctePat.city_st_zip) AS [pat_city_st_zip]
--	, UPPER(ctePat.pat_city) AS [pat_city]
--	, UPPER(ctePat.pat_state) AS [pat_state]
--	, ctePat.pat_zip
--	, ctePat.dob_yyyy
--	, UPPER(ctePat.pat_sex) as [sex]
--	, ctePat.relation
--	, UPPER(ctePat.pat_marital) AS [pat_marital]
--	, UPPER(ctePat.pat_race) AS [pat_race]
--	, COALESCE(ctePat.pat_phone, ctePat.guar_phone) AS [pat_phone]
--	, UPPER(ctePat.icd9_1) AS [dx1],UPPER(ctePat.icd9_2) AS [dx2],UPPER(ctePat.icd9_3) AS [dx3]
--	, UPPER(ctePat.icd9_4) AS [dx4],UPPER(ctePat.icd9_5)AS [dx5],UPPER(ctePat.icd9_6) AS [dx6]
--	, UPPER(ctePat.icd9_7) AS [dx7],UPPER(ctePat.icd9_8) AS [dx8],UPPER(ctePat.icd9_9)AS [dx9]
--	, UPPER(ctePat.guarantor) AS [guarantor], UPPER(ctePat.guar_addr) AS [guar_addr]
--	, UPPER(ctePat.guar_city) AS [guar_city], UPPER(ctePat.guar_state) AS [guar_state], ctePat.guar_zip
--	, UPPER(ctePat.g_city_st) AS [g_city_st]
--	, COALESCE(ctePat.guar_phone,ctePat.pat_phone) [guar_phone]
--	--, [npi].colNPI AS [phyid]
--	--, COALESCE([npi].colName, UPPER(ctePat.phy_comment)) AS [phy_comment]
--	, @modDate AS [mod_date]
--	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
--	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID)
--		, 'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
--	, RIGHT (HOST_NAME(),50)  AS [mod_host]
--	from ctePat
--	left outer join pat on pat.account = ctePat.account
--	outer APPLY dbo.GetPhyNPI(mt_mnem) AS [npi]		
--	where pat.account is null AND ctePat.rn = 1
--	--WHERE ctePat.account IN ('c7158428','c7158429')
	
	
	SET @count = @@ROWCOUNT
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('11. Pat Table rows added ' + convert(varchar(10),@count), @@ERROR)
--	INSERT INTO temp_track(comment,ERROR)
--	VALUES('11. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
	print '11. Pat Table rows added ' + convert(varchar(10),@count)
	--end of patient table 
	EXEC sp_xml_removedocument @idoc
	
	/*
COMMIT TRANSACTION
END TRY		
	
BEGIN CATCH
    
	IF (XACT_STATE()) = -1
    BEGIN
        PRINT
            N'PATIENT IMPORT ERROR: The transaction is in an uncommittable state.' +
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
    VALUES	( 'PATIENT IMPORT ERROR: TRANSACTION ROLLED BACK', -- comment - varchar(8000)
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
*/

END	


