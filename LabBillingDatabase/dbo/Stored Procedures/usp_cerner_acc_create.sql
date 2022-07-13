-- =============================================
-- Author:		David
-- Create date: 03/31/2015
-- Description:	Adds or updates data in the acc table from cerner hl7
-- =============================================
CREATE PROCEDURE usp_cerner_acc_create
	-- Add the parameters for the stored procedure here
	@msgID int = 0
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON; -- this allows the transaction code to work.

DECLARE @this_proc_name VARCHAR(50)
SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                                    +'.'+QUOTENAME(OBJECT_NAME(@@PROCID))

    -- Insert statements for procedure here
DECLARE @xml XML
SET @xml = (
SELECT CONVERT(XML,REPLACE(msgContent,'"',''))
FROM infce.messages_inbound 
WHERE infce.messages_inbound.systemMsgId = @msgID
AND infce.messages_inbound.msgType = 'ADT-A04'
)

--SELECT @msgID, @xml AS [xmlData]

IF (@xml IS NULL)
BEGIN
	RETURN; -- no xml or user sent charge message 
END


DECLARE @acc VARCHAR(15)
-------------------Trans date
DECLARE @transDate VARCHAR(8)

SET @transDate = COALESCE((SELECT  
	a.alias.value('(//PV1.44.1/text() )[1]'
		, 'varchar(8)') AS [tdate]
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
		),NULL)

--SELECT @transDate
-------------------Merged records (skip)
--DECLARE @merge VARCHAR(50)
--
--SET @merge = COALESCE((SELECT  
--	'L'+ LTRIM(dbo.AccountTrim(a.alias.value('(//MRG.3.1/text() )[1]'
--		, 'varchar(15)'))) AS [MRG]
--	FROM (select @xml AS rep_xml) r
--CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
--		),NULL)
--
----SELECT @merge
--IF (@merge IS NOT NULL)
--BEGIN
--UPDATE infce.messages_inbound
--	SET processFlag = 'P',
--	processStatusMsg = 'MERGE RECORD'
--	WHERE infce.messages_inbound.systemMsgId = @msgID
--	AND infce.messages_inbound.msgType = 'ADT-A04'
--	RETURN;
--END

---------------------------------------------------
-- get the account for message text won't work for ORM/ORU
SET @acc = COALESCE((SELECT  
	'L'+ LTRIM(dbo.AccountTrim(a.alias.value('(//PID.18.1/text() )[1]'
		, 'varchar(15)'))) AS [ACCOUNT]
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message/PID') a(alias)
		),'NONE')

--SELECT @acc

--DECLARE @msgType VARCHAR(20)
--SET @msgType = (SELECT  
--	LTRIM(a.alias.value('(//MSH.9.1/text() )[1]', 'varchar(10)')) AS [MSG TYPE]
--	FROM (select @xml AS rep_xml) r
-- 
--CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
--		)
--
--IF (@msgType IN ('DFT','MFN') )
--BEGIN
--RETURN
--END
----SELECT @msgType
------------------------FIN CODE CHECK -Need client and msg fin-----------
DECLARE @client VARCHAR(10)
SET @client = dbo.GetMappingValue('CLIENT','CERNER'
, (SELECT  
	COALESCE(
	NULLIF(a.alias.value('data(//PV1.3.4/text())[1]','varchar(50)'),''),
	NULLIF(a.alias.value('data(//PV1.3.1/text())[1]','varchar(50)'),''),
	NULLIF(a.alias.value('data(//PV1.3.7/text())[1]','varchar(50)'),''), 
	NULLIF(a.alias.value('data(//PV1.6.4/text())[1]','varchar(50)'),''),
	NULLIF(a.alias.value('data(//PV1.6.1/text())[1]','varchar(50)'),''),
	NULLIF(a.alias.value('data(//PV1.6.7/text())[1]','varchar(50)'),'')
	)AS [Client]
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message/PV1') a(alias)
		) )
		
DECLARE @fin VARCHAR(20)
SET @fin = COALESCE((
	SELECT  COALESCE(
		dbo.GetMappingValue('FIN_CODE','CERNER',
			LTRIM(a.alias.value(
			'(//PV1.20.1/text() )[1]', 'varchar(10)')) ),
		dbo.GetMappingValue('FIN_CODE','CERNER',
		   LTRIM(a.alias.value(
		   '(//IN1.35.1/text() )[1]', 'varchar(10)')) )
		)
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
		),NULL)

SET @fin = dbo.FormatFinCode(@client,@fin)
--SELECT @fin
--return
IF (EXISTS (SELECT acc.account FROM acc WHERE dbo.acc.account = @acc))
BEGIN

-- update all the adt messages that have not been processed for 
-- this account
UPDATE infce.messages_inbound
	SET processFlag = 'P'
	, processStatusMsg = 'ACC ALREADY EXISTS'
	WHERE infce.messages_inbound.account_cerner = 
		REPLACE(@acc,'L','')
	--infce.messages_inbound.systemMsgId = @msgID
	AND infce.messages_inbound.msgType = 'ADT-A04'
	AND infce.messages_inbound.processFlag <> 'P'
	
	RETURN;
END


--IF (NOT EXISTS (SELECT return_value FROM dictionary.mapping 
--	WHERE dictionary.mapping.sending_value = @fin AND ))
--IF (NOT EXISTS (SELECT dbo.GetMappingValue('FIN_CODE','CERNER',@fin) ) )
--BEGIN
--UPDATE infce.messages_inbound
--	SET processFlag = 'ERR',
--	processStatusMsg = 'INVALID FIN CODE: ['+@fin+' ]'+'[ '+@acc+' ] '
--	WHERE infce.messages_inbound.systemMsgId = @msgID
--	AND infce.messages_inbound.msgType = 'ADT-A04'
--	--RETURN; go ahead and process the insurance
--END



BEGIN TRY
BEGIN TRANSACTION

DECLARE @stage VARCHAR(3)
SET @stage = 'ACC'

-- account
IF (NOT EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc))
BEGIN
	;WITH cteACC
	AS
	(
		SELECT  
		'L'+ LTRIM(dbo.AccountTrim(a.alias.value('(//PID.18.1/text() )[1]'
			, 'varchar(15)'))) AS [ACCOUNT]
		-- pat_name
		,REPLACE(LTRIM(RTRIM(a.alias.value('(//PID.5.1/text())[1]'
			, 'varchar(25)'))),',',' ') AS [LAST NAME]	
		,LTRIM(RTRIM(a.alias.value('(//PID.5.2/text())[1]'
			, 'varchar(25)'))) AS [FIRST NAME]
		,LTRIM(RTRIM(a.alias.value('(//PID.5.3/text())[1]'
			, 'varchar(25)'))) AS [MIDDLE NAME]

		-- fin code
		, dbo.GetMappingValue('FIN_CODE','CERNER', 
		COALESCE(
			NULLIF(a.alias.value('(//PV1.20.1/text())[1]', 'varchar(10)'),'') 
			,'EP')) 
		AS [FIN CODE]
		--trans_date (created by first charge)
		,CONVERT(DATETIME,a.alias.value('(//FT1.4.1/text())[1]'
			, 'varchar(8)')) AS [TRANS_DATE]	
		-- ssn
		,dbo.FormatSSN(a.alias.value('(//PID.19.1/text())[1]'
			, 'varchar(14)')) AS [SSN]		
		-- meditech account
		,LTRIM(dbo.AccountTrim(a.alias.value('(//PID.18.1/text() )[1]'
			, 'varchar(15)'))) AS [MEDITECH ACCOUNT]
		-- HNE
		,LTRIM(a.alias.value('(//PID.2.1/text() )[1]'
			, 'varchar(50)')) AS [HNE_NUMBER]			
		-- MRI/MRN
		,'S'+ LTRIM(a.alias.value('(//PID.3.1/text() )[1]'
			, 'varchar(15)')) AS [MRN]
--		,a.alias.value('(//PID.3.1/text())[1]'
--			, 'varchar(25)') AS [OV_ORDER_ID] for adt's only
		-- ov_pat_id
		,a.alias.value('(//PID.4.1/text())[1]'
			, 'varchar(25)') AS [OV_PAT_ID]		
		-- guarantor id
		,a.alias.value('(//GT1.2.1/text())[1]'
			, 'varchar(25)') AS [GUARANTOR_ID]		
		-- mods
--		,a.alias.value('(//EVN.5.1/text())[1]'
--			, 'varchar(25)') AS [USER NUMBER]				
		,a.alias.value('(//EVN.5.2/text())[1]'
			, 'varchar(25)') AS [USER LNAME]
		,a.alias.value('(//EVN.5.3/text())[1]'
			, 'varchar(25)') AS [USER FNAME]
		, a.alias.value('(//PID.3.1/text())[1]'
			, 'varchar(25)') AS [OV ORDER ID]	
		-- wdk 20150723 added
		,CAST(a.alias.value('(//MSH.7.1/text())[1]'
			, 'varchar(8)') AS DATETIME) AS [trans_date_time]
		 					
		FROM (select @xml AS rep_xml) r
	 
	CROSS APPLY r.rep_xml.nodes('HL7Message/PID') a(alias)

	)
	INSERT INTO dbo.acc
			(
				account ,
				pat_name ,
				cl_mnem ,
				fin_code ,
				trans_date ,
				status ,
				ssn ,
				meditech_account ,
				original_fincode ,
				mri ,
				ov_pat_id ,
				guarantorID,
				dbo.acc.mod_date,
				dbo.acc.mod_user,
				dbo.acc.mod_prg,
				dbo.acc.mod_host
				, dbo.acc.ov_order_id , acc.HNE_NUMBER
				, dbo.acc.trans_date_time
				
			)
	SELECT cteACC.ACCOUNT 
	, UPPER(LTRIM(RTRIM(cteACC.[LAST NAME]+','	+
	  UPPER(cteACC.[FIRST NAME]) +' '+
	  coalesce(UPPER(cteACC.[MIDDLE NAME]),'')))) AS [PAT NAME] 
	
	, @client AS [client]--dbo.GetMappingValue('CLIENT','CERNER',cteAcc.CLIENT) AS [client]

	,COALESCE(@fin,cteACC.[FIN CODE]) AS [FIN CODE] 
	
	, cteACC.TRANS_DATE-- CONVERT(DATETIME,cteACC.TRANS_DATE ) AS [TRANS DATE]
	
	,'NEW' AS [STATUS]
	
	,cteACC.SSN
	
	,cteACC.ACCOUNT
	
	, COALESCE(@fin,cteACC.[FIN CODE])	
		--REPLACE(cteACC.[FIN CODE],'ERR','0') 
		AS [ORIGIONAL FINCODE] 
	
	,cteACC.MRN 
	,cteACC.OV_PAT_ID 
	, cteACC.GUARANTOR_ID
	, GETDATE() AS [mod_date]
	, RIGHT(LTRIM(RTRIM(SUSER_NAME()) ),50) AS [mod_user]
	, 'usp_cerner_acc_create'  AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	,cteACC.[OV ORDER ID]--@ov_order_id, 
	,cteACC.HNE_NUMBER
	, cteACC.trans_date_time								
	FROM cteAcc
	LEFT OUTER JOIN acc ON dbo.acc.account = cteACC.ACCOUNT
	WHERE acc.account IS NULL
--------- PAT
	;WITH ctePat
	AS
	(
		SELECT  
		-- account
		'L'+ LTRIM(dbo.AccountTrim(a.alias.value('(//PID.18.1/text() )[1]'
			, 'varchar(15)'))) AS [ACCOUNT]
		-- ssn
		,dbo.FormatSSN(a.alias.value('(//PID.19.1/text())[1]'
			, 'varchar(14)')) AS [SSN]	
		-- address	
		,COALESCE(a.alias.query('//PID.11[./PID.11.7/. = "home"]').value(
			'data(//PID.11.1/text())[1]'
			, 'varchar(40)'),
			a.alias.value('data(//PID.11.1/text())[1]'
			, 'varchar(40)') ) AS [PAT ADDR1]	
		,COALESCE(a.alias.query('//PID.11[./PID.11.7/. = "home"]').value(
			'data(//PID.11.2/text())[1]'
			, 'varchar(40)'),
			a.alias.value('data(//PID.11.2/text())[1]'
			, 'varchar(40)') ) AS [PAT ADDR2]		
		,COALESCE(a.alias.query('//PID.11[./PID.11.7/. = "home"]').value(
			'data(//PID.11.3/text())[1]'
			, 'varchar(40)'),
			a.alias.value('data(//PID.11.3/text())[1]'
			, 'varchar(40)') ) AS [PAT CITY]	
		,COALESCE(a.alias.query('//PID.11[./PID.11.7/. = "home"]').value(
			'data(//PID.11.4/text())[1]'
			, 'varchar(40)'),
			a.alias.value('data(//PID.11.4/text())[1]'
			, 'varchar(40)') ) AS [PAT STATE]	
		,COALESCE(a.alias.query('//PID.11[./PID.11.7/. = "home"]').value(
			'data(//PID.11.5/text())[1]'
			, 'varchar(40)'),
			a.alias.value(
			'data(//PID.11.5/text())[1]'
			, 'varchar(5)') ) AS [PAT ZIP]	
		-- DATE OF BIRTH	
		,a.alias.value('(//PID.7.1/text())[1]'
			, 'varchar(8)') AS [DOB]	--, 'varchar(25)') AS [DOB]
		-- PATIENT GENDER
		,NULLIF(a.alias.value('(//PID.8.1/text())[1]'
			, 'VARCHAR(1)'),'C') AS [SEX]	
		-- relation
	, COALESCE(
		NULLIF(a.alias.value('(//IN1.17.1/text())[1]'
			, 'VARCHAR(10)'),''),
		NULLIF(a.alias.value('(//GT1.11.1/text())[1]'
			, 'VARCHAR(10)'),'')
		,'01' 
	) AS [RELATION]	
	
		-- GUAR INFO	
		,LTRIM(RTRIM(a.alias.value('(//GT1.3.1/text())[1]'
			, 'VARCHAR(15)'))) AS [GUAR LNAME]			
		,LTRIM(RTRIM(a.alias.value('(//GT1.3.2/text())[1]'
			, 'VARCHAR(15)'))) AS [GUAR FNAME]			
		,LTRIM(RTRIM(a.alias.value('(//GT1.3.3/text())[1]'
			, 'VARCHAR(10)'))) AS [GUAR MNAME]			
		
		,a.alias.value('(//GT1.5.1/text())[1]'
			, 'VARCHAR(40)') AS [GUAR ADDR1]			
		,a.alias.value('(//GT1.5.3/text())[1]'
			, 'VARCHAR(38)') AS [GUAR CITY]			
		,a.alias.value('(//GT1.5.4/text())[1]'
			, 'VARCHAR(2)') AS [GUAR STATE]			
		,a.alias.value('(//GT1.5.5/text())[1]'
			, 'VARCHAR(5)') AS [GUAR ZIP]			
					
		-- pat_name
		,LTRIM(RTRIM(a.alias.value('(//PID.5.1/text())[1]'
			, 'varchar(25)'))) AS [LAST NAME]	
		,LTRIM(RTRIM(a.alias.value('(//PID.5.2/text())[1]'
			, 'varchar(25)'))) AS [FIRST NAME]
		,LTRIM(RTRIM(a.alias.value('(//PID.5.3/text())[1]'
			, 'varchar(25)'))) AS [MIDDLE NAME]
		
		-- MARITAL?	
		,NULLIF(a.alias.value('(//PID.16.1/text())[1]'
			, 'varchar(1)'),'') AS [PAT MARIATAL]
		
		-- PHY ID
		,COALESCE(	
		 NULLIF(dbo.GetMappedPhysician('PHY_ID','CERNER'
		,NULLIF(NULLIF( a.alias.value('(//FT1.21.1/text())[1]'
		, 'varchar(25)'),'' ),'Z')  --AS [1]-- pv1.17.1 admitting/ordering phy [outreach lab dr id]
		,NULLIF(NULLIF( a.alias.value('(//FT1.21.1/text())[2]'
		, 'varchar(25)'),'' ),'Z')  --AS [2]-- pv1.17.1 admitting/ordering phy [outreach lab dr id]
		,NULLIF(NULLIF( a.alias.value('(//FT1.21.1/text())[3]'
		, 'varchar(25)'),'' ),'Z')  --AS [3]-- pv1.17.1 admitting/ordering phy [outreach lab dr id]
		,NULLIF(NULLIF( a.alias.value('(//FT1.21.1/text())[4]'
		, 'varchar(25)'),'' ),'Z')  --AS [4]-- pv1.17.1 admitting/ordering phy [outreach lab dr id]
		) ,'K')
		,
		 NULLIF(dbo.GetMappedPhysician('PHY_ID','CERNER'
		,NULLIF(NULLIF( a.alias.value('(//FT1.21.1/text())[5]'
		, 'varchar(25)'),'' ),'Z')  --AS [5]-- pv1.17.1 admitting/ordering phy [outreach lab dr id]
		,NULLIF(NULLIF( a.alias.value('(//FT1.21.1/text())[6]'
		, 'varchar(25)'),'' ),'Z')  --AS [6]-- pv1.17.1 admitting/ordering phy [outreach lab dr id]
		,NULLIF(NULLIF( a.alias.value('(//FT1.21.1/text())[7]'
		, 'varchar(25)'),'' ),'Z')  --AS [7]-- pv1.17.1 admitting/ordering phy [outreach lab dr id]
		,NULLIF(NULLIF( a.alias.value('(//FT1.21.1/text())[8]'
		, 'varchar(25)'),'' ),'Z')  --AS [8]-- pv1.17.1 admitting/ordering phy [outreach lab dr id]
		),'K') 
		,NULLIF(dbo.GetMappingValue('PHY_ID','CERNER', a.alias.query('//PV1.17[./PV1.17.9/. = "WTH DOCTOR NBR"]').value(
			'data(//PV1.17.1/text())[1]','varchar(40)')),'K')
		,NULLIF(dbo.GetMappingValue('PHY_ID','CERNER',a.alias.query('//PV1.17[./PV1.17.9/. = "Outreach Lab DR ID"]').value(
			'data(//PV1.17.1/text())[1]','varchar(40)')),'K')
		
		) AS [PHY ID]	

		,NULLIF(NULLIF( a.alias.value('(//FT1.19.3/text())[1]'
		, 'varchar(3)'),'' ),'Z')  AS [icd_Indicator]
	
		-- PHY NAME
		,LTRIM(RTRIM(a.alias.value('(//FT1.21.2/text())[1]'
			, 'varchar(25)'))) AS [PHY LNAME]
		,LTRIM(RTRIM(a.alias.value('(//FT1.21.3/text())[1]'
			, 'varchar(25)') )) AS [PHY FNAME]
		,LTRIM(RTRIM(a.alias.value('(//FT1.21.4/text())[1]'
			, 'varchar(25)') )) AS [PHY MNAME]
			
		,  LTRIM(RTRIM(
			a.alias.value('(//FT1.21.2/text())[1]'
				, 'varchar(25)')+', '
					+	a.alias.value('(//FT1.21.3/text())[1]'
						, 'varchar(25)') + ' '
					+	coalesce(a.alias.value('(//FT1.21.4/text())[1]'
						, 'varchar(25)'),''))) AS [PHY FULL NAME]

		,dbo.FormatPhoneNo(a.alias.value('(//PID.13.1/text())[1]'
			, 'varchar(25)')) AS [GUAR PHONE]				
		,dbo.FormatPhoneNo(a.alias.value('(//PID.13.1/text())[1]'
			, 'varchar(25)')) AS [PAT PHONE]				

		-- mods
--			,a.alias.value('(//EVN/EVN.5/EVN.5.1/text())[1]'
--				, 'varchar(25)') AS [USER NUMBER]				
			,a.alias.value('(//EVN/EVN.5/EVN.5.2/text())[1]'
				, 'varchar(25)') AS [USER LNAME]
			,a.alias.value('(//EVN/EVN.5/EVN.5.3/text())[1]'
				, 'varchar(25)') AS [USER FNAME]			
							
		FROM (select @xml AS rep_xml) r
	 
	CROSS APPLY r.rep_xml.nodes('HL7Message/PID') a(alias)

	)
	INSERT INTO dbo.pat
			(
				account ,				ssn ,
				pat_addr1 ,				pat_addr2 ,
				city_st_zip ,
				pat_city ,
				pat_state ,
				pat_zip ,
				dob_yyyy ,
				sex ,
				relation ,
				guarantor ,
				guar_addr ,
				g_city_st ,			guar_city ,
				guar_state ,
				guar_zip ,
				pat_marital ,
				pat_phone ,
				phy_id ,
				phy_comment ,
				guar_phone ,
				mod_date ,
				mod_user ,
				mod_prg ,			mod_host ,
				pat_full_name 
				, dbo.pat.icd_indicator
			)
	SELECT ctePAT.ACCOUNT 			,ctePAT.SSN		
			, UPPER(ctePat.[PAT ADDR1])	
			, UPPER(ctePat.[PAT ADDR2])
			,CASE WHEN RIGHT(LTRIM(RTRIM(
				COALESCE(
				ctePAT.[PAT CITY],'')+','+	
				UPPER(COALESCE(ctePAT.[PAT STATE],'')) + ' '+	
				coalesce(UPPER(ctePAT.[PAT ZIP]),''))),50) = ','
				THEN NULL			
				ELSE RIGHT(LTRIM(RTRIM(
					COALESCE(UPPER(ctePAT.[PAT CITY]),'')+', '+	
					UPPER(COALESCE(ctePAT.[PAT STATE],'')) +
					' '			+	-- wdk 20150825 add back per carol--wdk 20150812 removed per Carol
					coalesce(UPPER(ctePAT.[PAT ZIP]),''))),50)		
					END  AS [PAT CSZ]
			
			,UPPER(ctePAT.[PAT CITY])
			,UPPER(ctePAT.[PAT STATE])		,ctePAT.[PAT ZIP]
			, ctePat.DOB
			, UPPER(ctepat.SEX)
			
			,dbo.GetMappingValue('GUAR_REL', 'CERNER', ctepat.relation) AS [RELATION]
				
			,  LTRIM(RTRIM(ctePAT.[GUAR LNAME]+','+
					ctePAT.[GUAR FNAME] + ' '+
					coalesce(ctePAT.[GUAR MNAME],''))) AS [GUAR NAME]
			, ctePat.[GUAR ADDR1]
		,CASE WHEN RIGHT(LTRIM(RTRIM(COALESCE(ctePAT.[GUAR CITY],'')+', '			+	UPPER(COALESCE(ctePAT.[GUAR STATE],'')) + ' '			+	coalesce(UPPER(ctePAT.[GUAR ZIP]),''))),50) = ','
				THEN NULL
				ELSE RIGHT(LTRIM(RTRIM(COALESCE(ctePAT.[GUAR CITY],'')+', '			+	UPPER(COALESCE(ctePAT.[GUAR STATE],'')) + ' '			+	coalesce(UPPER(ctePAT.[GUAR ZIP]),''))),50) END  AS [GUAR CSZ]
			,UPPER(ctePAT.[GUAR CITY])
			,UPPER(ctePAT.[GUAR STATE])
			,ctePAT.[GUAR ZIP]
			, UPPER(ctePat.[PAT MARIATAL])
			, ctePat.[PAT PHONE]
			, NULLIF(ctePat.[phy id],'K') AS [PHYSICIAN]
--			, CASE WHEN ctePat.[phy id] = 'K' THEN NULL
--				ELSE ctePat.[phy id] END 
			, ctePat.[PHY FULL NAME]
			, ctePat.[guar phone]
			, GETDATE()
			,RIGHT(LTRIM(RTRIM(SUSER_NAME()) ),50) AS [mod_user]
		
		,'usp_cerner_acc_create' AS [mod_prg],
			  RIGHT (HOST_NAME(),50)  AS [mod_host]
			  
		,  UPPER(LTRIM(RTRIM(ctePAT.[LAST NAME]+','
				+	ctePAT.[FIRST NAME] + ' '+	coalesce(ctePAT.[MIDDLE NAME],'')))) AS [PAT COMMENT]
		, ctePat.icd_indicator					
	FROM ctePat


END
	
--SET @stage = 'GTR'
--
---- guarantor
--IF (EXISTS (SELECT account FROM pat WHERE dbo.pat.account = @acc))
--BEGIN
--DECLARE @guarID			VARCHAR(50)	-- goes in Account table
--DECLARE @guarLastName	VARCHAR(25) -- the rest go in the pat table
--DECLARE @guarFirstName	VARCHAR(25)
--DECLARE @guarMiddleName VARCHAR(25)
--DECLARE	@guarMI			VARCHAR(1)
--DECLARE @guarAddr1		VARCHAR(40)
--DECLARE @guarAddr2		VARCHAR(40)
--DECLARE @guarCity		VARCHAR(40)
--DECLARE @guarState		VARCHAR(2)
--DECLARE @guarZip		VARCHAR(11)
--DECLARE @guarPhone		VARCHAR(15)
--DECLARE @guarDob		VARCHAR(8)
--DECLARE @guarRel		VARCHAR(2)
--
--set @guarLastName = (select 
--	LTRIM(RTRIM(@xml.value('data(//GT1.3.1/text()) [1]',	'varchar(25)'))) )
--set @guarFirstName = (select 
--	LTRIM(RTRIM(@xml.value('data(//GT1.3.2/text()) [1]',	'varchar(25)') )) )
--set @guarMiddleName = (select 
--	LTRIM(RTRIM(@xml.value('data(//GT1.3.3/text()) [1]',	'varchar(25)'))) )
--set	@guarMI = (select 
--	LTRIM(RTRIM(@xml.value('data(//GT1.3.3/text()) [1]',	'varchar(1)')   )) )
--set @guarAddr1 = (select @xml.value('data(//GT1.5.1/text()) [1]',		'varchar(40)') )
--set @guarAddr2 = (select @xml.value('data(//GT1.5.2/text()) [1]',		'varchar(40)') )
--set @guarCity = (select @xml.value('data(//GT1.5.3/text()) [1]',		'varchar(40)')  )
--set @guarState = (select @xml.value('data(//GT1.5.4/text()) [1]',		'varchar(2)')  )
--set @guarZip = (select @xml.value('data(//GT1.5.5/text()) [1]',			'varchar(5)')  )
--set @guarPhone = (select @xml.value('data(//GT1.6.1/text()) [1]',		'varchar(15)')  )
--set @guarDob = (select @xml.value('data(//GT1.8.1/text()) [1]',		'varchar(8)')	 )
--set @guarRel = (select COALESCE(REPLACE( @xml.value('data(//GT1.11.1/text())[1]','varchar(2)'),'SE','01'),'09') )
--SET @guarID = (select @xml.value('data(//GT1.2.1/text() ) [1]','varchar(50)') )
----SELECT  @guarLastName,@guarFirstName,@guarMiddleName,@guarMI
----,@guarAddr1,@guarAddr2,@guarCity,@guarState,@guarZip
----,@guarPhone,@guarDob, @guarRel 
--
--UPDATE dbo.pat
--SET pat.guarantor = UPPER(@guarLastName)
--			+', '+upper(@guarFirstName)+' '
--			+	ISNULL(upper(@guarMiddleName),'')
--	,guar_addr	= UPPER(@guarAddr1)
--	,guar_city = UPPER(@guarCity)
--	,guar_state = UPPER(@guarState)
--	,guar_zip	= @guarZip
--	,g_city_st = UPPER(@guarCity) +', '
--			+	UPPER(@guarState) +' '+@guarZip 
--
--	,pat.guar_phone = dbo.FormatPhoneNo(@guarPhone)
--FROM dbo.pat
--WHERE pat.account = @acc
--
--IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc
--	AND dbo.acc.guarantorID <> @guarID AND @guarID IS NOT NULL))
--BEGIN
--UPDATE dbo.acc
--SET guarantorID = @guarID
--FROM acc 
--WHERE account = @acc
--end
--
--END
--
--SET @stage = 'ACC'
--IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc
--	AND dbo.acc.cl_mnem = 'K'))
--BEGIN
--	UPDATE dbo.acc
--	SET cl_mnem = @client
--	, dbo.acc.status = ISNULL(NULLIF(acc.status,'PAID_OUT'),'NEW')
--	FROM acc WHERE dbo.acc.account = @acc
----	PRINT 'before recharge'
--	EXEC dbo.usp_prg_ReCharge_Acc_Transaction @acc = @acc -- varchar(15)
----	PRINT 'after recharge'
--	
--	
----select @stage
--END	


--- INSURANCE
SET @stage = 'INS'
DECLARE @iDoc INT
DECLARE	@x XML
SELECT @x = (SELECT (CONVERT(XML,@xml)))
--SELECT @x

--SET @iDoc = 
--	(SELECT @x.value('count(data(distinct-values(//IN1.1.1/text())))','varchar(1)'))
--SELECT 'IN1 Segments', @iDoc

--SELECT 'start'
--SELECT @iDoc
IF (NOT EXISTS (SELECT account FROM ins WHERE dbo.ins.account = @acc))
/* as of 20150626 10:45 do not update per Jerry/Carol*/
BEGIN
	EXEC sp_xml_preparedocument @iDoc OUTPUT, @xml

	INSERT INTO dbo.ins
			(
				account ,		ins_a_b_c ,		holder_nme ,	
				holder_dob ,	holder_sex ,
				holder_addr ,			holder_city_st_zip ,
				plan_nme ,
				policy_num ,
				grp_nme ,
				grp_num ,
				fin_code ,
				ins_code ,
				relation ,
				mod_date ,			mod_user ,			mod_prg ,			mod_host ,
				holder_lname ,			holder_fname ,			holder_mname
			)

	SELECT 'L'+ LTRIM(dbo.AccountTrim(iIns.account)), CHAR(ins_pri+64) AS [INS PRI]
	, LTRIM(RTRIM(
			LTRIM(RTRIM(iIns.holder_lname))+','
					+	LTRIM(RTRIM(iIns.holder_fname)) + ' '
					+	coalesce(LTRIM(RTRIM(iIns.holder_mname)),''))) AS [HOLDER FULL NAME]
	, iIns.holder_dob, iIns.holder_sex 
	, iIns.holder_addr --, iIns.holder_city, iIns.holder_state, iIns.holder_zip
	,CASE WHEN RIGHT(LTRIM(RTRIM(COALESCE(iIns.holder_city,'')+','
				+	UPPER(COALESCE(iIns.holder_state,'')) + ' '
							+	coalesce(UPPER(iIns.holder_zip),''))),50) = ','
				THEN NULL
				ELSE RIGHT(LTRIM(RTRIM(COALESCE(iIns.holder_city,'')+','
				+	UPPER(COALESCE(iIns.holder_state,'')) + ' '
							+	coalesce(UPPER(iIns.holder_zip),''))),50) END  AS [HOLDER CSZ]

	, UPPER(iIns.ins_plan_name)
	, COALESCE(iIns.ins_policy_numWC,iIns.ins_policy_num,iIns.ins_policy_num2) AS [ins_policy_num]
	, iIns.ins_group_name, iIns.ins_group_num
	, REPLACE(dbo.GetMappingValue('FIN_CODE','CERNER', 
		COALESCE(iIns.ins_fin_code,iIns.ins_ov_code)),'ERR','0') AS [ins_fin_code]
	, dbo.GetMappingValue('INS_CODE','CERNER', iIns.ins_ov_code) AS [ins_ov_code]
	, iIns.ins_rel_to_pat
	-- mods
	--, iIns.cuser_number, iIns.cuser_lname, iIns.cuser_fname
	, GETDATE()
	,RIGHT(LTRIM(RTRIM(COALESCE(iIns.cuser_number,'UNK')+'|'
						+	UPPER(COALESCE(iIns.cuser_lname,'UNK')) + ','
						+	coalesce(UPPER(iIns.cuser_fname),'UNK'))),50) AS [MOD_USER]
	,@this_proc_name
--	,COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
--		  				'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50),'NO APP IDENTIFIED') AS [MOD_PRG]	
	, RIGHT(HOST_NAME(),50) AS [MOD_HOST]		  			
	, UPPER(iIns.holder_lname), UPPER(iIns.holder_fname)
	, UPPER(iIns.holder_mname)
	FROM OPENXML(@iDoc, 'HL7Message/IN1',2)
	WITH
	(
		account			[varchar] (50)	'//PID.18.1',
		ins_pri			INT				'IN1.1/IN1.1.1',
		holder_lname	[varchar] (40)	'IN1.16/IN1.16.1',
		holder_fname	[varchar] (40)	'IN1.16/IN1.16.2',
		holder_mname	[varchar] (40)	'IN1.16/IN1.16.3',
		holder_dob		[varchar] (8)	'IN1.18/IN1.18.1',
		holder_sex		[VARCHAR] (1)	'IN1.43/IN1.43.1',
		holder_addr		[varchar] (40)	'IN1.19/IN1.19.1',
		holder_city		[varchar] (28)	'IN1.19/IN1.19.3',
		holder_state	[varchar] (2)	'IN1.19/IN1.19.4',
		holder_zip		[varchar] (10)	'IN1.19/IN1.19.5',
		ins_plan_name	[varchar] (45)	'IN1.2/IN1.2.2',
		ins_policy_num	[varchar] (50)	'IN1.36/IN1.36.1',
		ins_policy_num2 [varchar] (50)  '//IN2.61.1',
		ins_policy_numWC [VARCHAR] (50)	'IN1.49/IN1.49.1',
		ins_group_name	[varchar] (50)	'IN1.9/IN1.9.1',
		ins_group_num	[varchar] (15)	'IN1.8/IN1.8.1',
		ins_fin_code	[varchar] (50)	'IN1.35/IN1.35.1',
		ins_ov_code		[varchar] (20)	'IN1.2/IN1.2.1',
		ins_rel_to_pat	[varchar] (2)	'IN1.17/IN1.17.1',
		cuser_number	[varchar] (50)	'//EVN.5.1',
		cuser_lname		[varchar] (25)	'//EVN.5.2',
		cuser_fname		[varchar] (25)	'//EVN.5.3'	
		
	) AS iIns
	LEFT OUTER JOIN ins ON ins.account = 'L'+ LTRIM(dbo.AccountTrim(iIns.account))
		AND ins.deleted = 0
	WHERE ins.account IS NULL

	EXEC sp_xml_removedocument @iDoc
END





-- Last statment in the file 
UPDATE infce.messages_inbound
SET processFlag = 'P'
, processStatusMsg = @acc
WHERE infce.messages_inbound.systemMsgId = @msgID
AND infce.messages_inbound.msgType = 'ADT-A04'

COMMIT TRANSACTION
END TRY	
BEGIN CATCH	

IF (XACT_STATE()) = -1
    BEGIN
    
        PRINT
            N'The transaction is in an uncommittable state.' +
            'Rolling back transaction.'
        ROLLBACK TRANSACTION;
        
        
        INSERT INTO dbo.error_prg
            ( error_type,
            [app_name] ,
              app_module ,
              error ,
              mod_date ,
              mod_prg ,
              mod_user ,
              mod_host,
              dbo.error_prg.account
            )
		SELECT	'SQL',
            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 )AS [mod_prg],
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_MESSAGE() AS ErrorMessage,
            GETDATE(),
            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 ) AS ErrorProcedure,
            RIGHT(SUSER_SNAME(),50),
            RIGHT (HOST_NAME(),50),
            @acc;
            
          UPDATE infce.messages_inbound
			SET processFlag = 'E',
			processStatusMsg = @stage+' - '+'[ '+@acc+ ' ]'+RIGHT(ERROR_MESSAGE(),200)
			WHERE infce.messages_inbound.systemMsgId = @msgID
			AND infce.messages_inbound.msgType = 'ADT-A04'
    END;

    -- Test whether the transaction is committable.
    IF (XACT_STATE()) = 1
    BEGIN

        PRINT
            N'The transaction is committable.' +
            'Committing transaction.'
        COMMIT TRANSACTION;   
    END;
    
       
END CATCH
	
END
