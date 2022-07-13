
-- =============================================
-- Author:		David
-- Create date: 03/31/2015
-- Description:	creates entries in acc, pat, chrg,amt tables from cerner hl7
--		additionaly information from the A04 messages will be processed after the 
--		charges are posted
-- Any charge set as 'NP' will have to be manually processed because it will 
--		have failed at least once.
-- MODS 20161208 updated to handle change in Cerners fin code handling
-- =============================================
CREATE PROCEDURE [dbo].[usp_cerner_chrg]
	-- Add the parameters for the stored procedure here
	@msgID int = 0
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON; -- this allows the transaction code to work.

    -- Insert statements for procedure here
    DECLARE @app_module VARCHAR(256)
    DECLARE @this_proc_name VARCHAR(256)
	SET @this_proc_name = QUOTENAME(OBJECT_SCHEMA_NAME(@@PROCID))
                         +'.'+QUOTENAME(OBJECT_NAME(@@PROCID))
                         
	DECLARE @xml XML
	SET @xml = (
	SELECT CONVERT(XML,REPLACE(msgContent,'"',''))
	FROM infce.messages_inbound 
	WHERE infce.messages_inbound.systemMsgId = @msgID-- 2679536 -- wdk 20180621 
	and	infce.messages_inbound.msgType = 'DFT-P03'
	AND infce.messages_inbound.processFlag IN ('N','E','NP') 
	) 



	-- If the wrong message type sent / or the message is truly blank
	IF (@xml IS NULL) 
	BEGIN
	UPDATE infce.messages_inbound
		SET processFlag = 'ERR',
		processStatusMsg = 'NO DATA IN FILE'
		WHERE infce.messages_inbound.systemMsgId = @msgID
		AND infce.messages_inbound.msgType = 'DFT-P03'

		RETURN;

	END

/* this removes the parent child issues previously identifed
if further problems are presented correct the function. 
the function also removes the PR1 segments associated with each
FT1 if they exist */
SELECT @xml = (SELECT dbo.fnRemoveChildElements(@xml))

-- Cerner Account
DECLARE @cAcc VARCHAR(15)
SET @cAcc = (SELECT  infce.messages_inbound.account_cerner 
			 FROM infce.messages_inbound
			 WHERE infce.messages_inbound.systemMsgId = @msgID
			)
	
-------- demo xml
DECLARE @xmlDemo XML
SET @xmlDemo = (
	SELECT CONVERT(XML,REPLACE(msgContent,'"',''))
	FROM infce.messages_inbound 
	WHERE infce.messages_inbound.systemMsgId =  
	(SELECT TOP(1) infce.messages_inbound.systemMsgId
		FROM infce.messages_inbound
		WHERE infce.messages_inbound.account_cerner = @cAcc 
		AND infce.messages_inbound.msgType = 'ADT-A04'
	ORDER BY infce.messages_inbound.systemMsgId desc
	))
-----------------
--SELECT @xmlDemo
DECLARE @chrgFin VARCHAR(10) -- primary unless it is an "H"
DECLARE @insFin VARCHAR(10)  -- secondary unless primary is an "H"

SET @insFin = 
	( 
		-- this was looking in IN1.2 for the fin-code. Changed to look in IN1.35 - bsp 06-10-2022
		SELECT @xmlDemo.value('(//IN1/IN1.35/IN1.35.1/text())[1]','varchar(10)')
	)
SET @chrgFin = 
	( 
		SELECT @xml.value('(//PV1/PV1.20/PV1.20.1/text())[1]','varchar(10)')
	)

DECLARE @src_app VARCHAR(50)
SET @src_app = 
	(
		SELECT @xml.value('(//MSH/MSH.3/MSH.3.1/text())[1]','varchar(50)')
	)

DECLARE @acc VARCHAR(15)
SET @acc = null

DECLARE @ov_order_id VARCHAR(50)
SET @ov_order_id = @msgID

-- Charges create the acc and pat tables if the account is valid within parameters
SET @acc = COALESCE((SELECT  
	LTRIM(dbo.AccountTrim(a.alias.value('(//PID.18.1/text() )[1]'
		, 'varchar(15)'))) AS [ACCOUNT]
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message/PID') a(alias)
		),STUFF(@cAcc,1,0,'L'),'NONE')

if (@acc like '[0-9]%')
begin
    set @acc = 'L' + @acc
	SELECT @acc = (SELECT ISNULL(NULLIF(@acc,'L0'),'NONE'))
	SELECT @acc = (SELECT ISNULL(NULLIF(@acc,'L0000000'),'NONE'))
end

IF (@acc = 'NONE')
BEGIN	
	UPDATE infce.messages_inbound
	SET processFlag = 'E',
	processStatusMsg = 'CHRG: MSG [ ' + ISNULL(@msgID,-1)+' ] HAS NO ACCOUNT'
	WHERE infce.messages_inbound.systemMsgId = @msgID
	AND infce.messages_inbound.msgType = 'DFT-P03'

	RETURN;
END

IF (LEN(@acc) < 7 OR LEN(@acc)>15 )
BEGIN	
UPDATE infce.messages_inbound
	SET processFlag = 'E',
	processStatusMsg = 'CHRG: ACCOUNT [ '+@acc+' ] IS AN INVALID ACCOUNT NUMBER'
	WHERE infce.messages_inbound.systemMsgId = @msgID
	AND infce.messages_inbound.msgType = 'DFT-P03'

	RETURN;
END

-- set the status to temporary so if we crash we or run over
-- we cannot pick this one up again.
UPDATE infce.messages_inbound
SET processFlag = 'TEMP'
, processStatusMsg = @acc 

WHERE infce.messages_inbound.systemMsgId = @msgID
AND infce.messages_inbound.msgType = 'DFT-P03'

DECLARE @stage VARCHAR(4)
SET @stage = 'ACC'

-- effective 9/25/2015 carols request to always make these clients a y fincode
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

-- effective 05212018
IF (@client = 'QD_DNPost' or @client = 'QUESTREV')
BEGIN
         UPDATE infce.messages_inbound
			SET processFlag = 'DNP',
			processStatusMsg = @stage+' - [ '+@acc+' ] '+'QD-Do Not Post'
			WHERE infce.messages_inbound.systemMsgId = @msgID
			AND infce.messages_inbound.msgType = 'DFT-P03'
			RETURN	
END

IF (@client = 'K')
BEGIN
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
		FROM (select @xmlDemo AS rep_xml) r
	CROSS APPLY r.rep_xml.nodes('HL7Message/PV1') a(alias)
		) )
END

/*wdk 20161209 fincode work*/
	DECLARE @fin VARCHAR(10)
	SET @fin = (SELECT  COALESCE(
		NULLIF(dbo.GetMappingValue('FIN_CODE','CERNER',NULLIF(@chrgFin,'H')),'K'),
		NULLIF(dbo.GetMappingValue('FIN_CODE','CERNER',@insFin),'K'),'Y'))

	SET @app_module = 'CAP CONTRACT'                     
	DECLARE @CapContract VARCHAR(50)
	SET @CapContract = 
	(
		SELECT @xmlDemo.value('(//IN1/IN1.8/IN1.8.1/text()) [1]
			[(.="122607" and ../../../IN1.2/IN1.2.1 = "68352980") 
		  or (.="111926" and ../../../IN1.2/IN1.2.1 = "68353147") ] '
			,'varchar(50)')
		
	)

-- client bill only
;WITH cteY
AS
(
SELECT dbo.client.cli_mnem --,		dbo.client.cli_nme ,		dbo.client.type  
FROM dbo.client WHERE dbo.client.cli_mnem IN(
--'EHS' -- Employee Health -- removed per Carol Plumlee 12/30/2021 bsp
'EMIV'-- –Employee Intervention
,'WTTC'-- – West Tennessee Transitional Care
,'JAH'-- – Hospice
,'LLBS'-- – Lifeline Blood Services
,'LLWEP' --– Living Wellness
,'MAC'-- – Mac Screenings
,'CANCW'-- – Cancer Care Wellness account
,'OHBC'-- - Oak Hill Behavioral center
,'CLRS' -- Clinical Research Solutions
,'CPR' -- Compass Point Research wdk 20161024
)
AND dbo.client.deleted = 0

UNION 

SELECT dbo.client.cli_mnem 
FROM dbo.client WHERE type = 6
AND dbo.client.deleted = 0

UNION 

SELECT dbo.client.cli_mnem
FROM dbo.client 
WHERE dbo.client.cli_mnem LIKE '%2' AND type <> 6
	AND dbo.client.deleted = 0

UNION

SELECT dbo.client.cli_mnem 
FROM dbo.client 
WHERE dbo.client.cli_mnem LIKE 'MP%' AND type <> 6
	AND dbo.client.deleted = 0
)
Select @fin =  
COALESCE((SELECT CASE WHEN cteY.cli_mnem IS NOT NULL THEN 'Y' end
 FROM cteY WHERE cteY.cli_mnem = @client),@fin)
---- end of client bill only

--- never a client bill
IF (EXISTS (SELECT cli_mnem FROM client 
			WHERE dbo.client.cli_mnem = @client
			AND type = 3 AND dbo.client.cli_mnem 
			NOT IN ('EHS','EMIV','JAH','MAC','HEAW') ) 
			AND @fin = 'Y')
BEGIN

 SELECT @fin = COALESCE((NULLIF(dbo.GetMappingValue('FIN_CODE','CERNER',@insFin),'K')),'E')
-- dbo.GetMappingValue('FIN_CODE','CERNER',
--(select COALESCE(	
--	@xmlDemo.value('(//IN1.2.1/text()) [1]','varchar(10)'),'E'
--)))
END	
--- end of never a client bill

--SELECT @fincodeMapped AS [mapped] , @finCodeOrig AS [orig]


-- end of this part use the variable in the acc insert below
-- we have valid account 
IF (NOT EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc))
BEGIN
	;WITH cteACC
	AS
	(
		SELECT  
		@acc as [ACCOUNT]
		--'L'+ LTRIM(dbo.AccountTrim(a.alias.value('(//PID.18.1/text() )[1]'
		--	, 'varchar(15)'))) AS [ACCOUNT]
		-- pat_name
		,LTRIM(RTRIM(a.alias.value('(//PID.5.1/text())[1]'
			, 'varchar(25)'))) AS [LAST NAME]	
		,LTRIM(RTRIM(a.alias.value('(//PID.5.2/text())[1]'
			, 'varchar(25)'))) AS [FIRST NAME]
		,LTRIM(RTRIM(a.alias.value('(//PID.5.3/text())[1]'
			, 'varchar(25)'))) AS [MIDDLE NAME]

		-- fin code
--		, dbo.GetMappingValue('FIN_CODE','CERNER', 
--		COALESCE(
--			NULLIF(a.alias.value('(//PV1.20.1/text())[1]', 'varchar(10)'),'') 
--			,'EP')) 
--		AS [FIN CODE]
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
		,'S'+ dbo.sfn_trim_zeros(LTRIM(a.alias.value('(//PID.3.1/text() )[1]'
			, 'varchar(15)') ) ) AS [MRN]
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
		, @ov_order_id AS [OV ORDER ID]	
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
	, UPPER(LTRIM(RTRIM(cteACC.[LAST NAME]+', '	+ -- wdk 20170221 added space between last and first name
	  UPPER(cteACC.[FIRST NAME]) +' '+
	  coalesce(UPPER(cteACC.[MIDDLE NAME]),'')))) AS [PAT NAME] 
	
	, @client AS [client]--dbo.GetMappingValue('CLIENT','CERNER',cteAcc.CLIENT) AS [client]

	--,COALESCE(@fincodeMapped,cteACC.[FIN CODE]) AS [FIN CODE] 
	-- wdk 20170412
	,CASE WHEN  NULLIF(@client,'LIFT') is null and NULLIF(@fin,'Y') is null
			THEN 'E'
			ELSE @fin
			end as [FIN CODE]
	
	, cteACC.TRANS_DATE-- CONVERT(DATETIME,cteACC.TRANS_DATE ) AS [TRANS DATE]
	
	,'NEW' AS [STATUS]
	
	,cteACC.SSN
	
	,cteACC.ACCOUNT
	
--	, COALESCE(@fincodeMapped,cteACC.[FIN CODE])	
--		--REPLACE(cteACC.[FIN CODE],'ERR','0') 
--		AS [ORIGIONAL FINCODE] 
	, @fin AS [ORIGIONAL FINCODE]
	,cteACC.MRN 
	,cteACC.OV_PAT_ID 
	, cteACC.GUARANTOR_ID
	, GETDATE() AS [mod_date]
	, RIGHT(LTRIM(RTRIM(SUSER_NAME()) ),50) AS [mod_user]
	, RIGHT(concat('usp_cerner_chrg - ',@src_app),50)  AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host]
	,@ov_order_id, cteACC.HNE_NUMBER
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
		@acc as [ACCOUNT]
		--'L'+ LTRIM(dbo.AccountTrim(a.alias.value('(//PID.18.1/text() )[1]'
		--	, 'varchar(15)'))) AS [ACCOUNT]
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
				account ,
				ssn ,
				pat_addr1 ,
				pat_addr2 ,
				city_st_zip ,
				pat_city ,
				pat_state ,
				pat_zip ,
				dob_yyyy ,
				sex ,
				relation ,
				guarantor ,
				guar_addr ,
				g_city_st ,
				guar_city ,
				guar_state ,
				guar_zip ,
				pat_marital ,
				pat_phone ,
				phy_id ,
				phy_comment ,
				guar_phone ,
				mod_date ,
				mod_user ,
				mod_prg ,
				mod_host ,
				pat_full_name 
				, dbo.pat.icd_indicator
			)
	SELECT ctePAT.ACCOUNT
			,ctePAT.SSN		
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
					' '		+	-- wdk 20150825 add back per carol--wdk 20150812 removed per Carol
					coalesce(UPPER(ctePAT.[PAT ZIP]),''))),50)		
					END  AS [PAT CSZ]			
			,UPPER(ctePAT.[PAT CITY])
			,UPPER(ctePAT.[PAT STATE])
			,ctePAT.[PAT ZIP]
			, ctePat.DOB
			, UPPER(ctepat.SEX)
			
			,dbo.GetMappingValue('GUAR_REL', 'CERNER', ctepat.relation) AS [RELATION]
				
			,  LTRIM(RTRIM(ctePAT.[GUAR LNAME]+','+
					ctePAT.[GUAR FNAME] + ' '+
					coalesce(ctePAT.[GUAR MNAME],''))) AS [GUAR NAME]
			, ctePat.[GUAR ADDR1]
		,CASE WHEN RIGHT(LTRIM(RTRIM(COALESCE(ctePAT.[GUAR CITY],'')+', ' +	UPPER(COALESCE(ctePAT.[GUAR STATE],'')) + ' ' +	coalesce(UPPER(ctePAT.[GUAR ZIP]),''))),50) = ','
				THEN NULL
				ELSE RIGHT(LTRIM(RTRIM(COALESCE(ctePAT.[GUAR CITY],'')+', '	+	UPPER(COALESCE(ctePAT.[GUAR STATE],'')) + ' ' +	coalesce(UPPER(ctePAT.[GUAR ZIP]),''))),50) END  AS [GUAR CSZ]
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
		
		,RIGHT(concat('usp_cerner_chrg - ',@src_app),50) AS [mod_prg]
		,RIGHT (HOST_NAME(),50)  AS [mod_host]
		,UPPER(LTRIM(RTRIM(ctePAT.[LAST NAME]+','
				+	ctePAT.[FIRST NAME] + ' '+	coalesce(ctePAT.[MIDDLE NAME],'')))) AS [PAT COMMENT]
		, ctePat.icd_indicator					
	FROM ctePat
END

BEGIN TRY
BEGIN TRANSACTION

SET @stage = 'CHRG'

DECLARE @dos DATETIME
SET @dos = COALESCE((SELECT  
	a.alias.value('(//FT1.4.1/text() )[1]','varchar(8)') AS [DOS]
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
		),NULL)

-- charges
IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc))
BEGIN	
; WITH cteMsg
AS 
(
	SELECT  M.systemMsgId,
	--CONVERT(XML, M.msgContent) AS xmlMsg 
	@xml AS xmlMsg
	FROM infce.messages_inbound	M
	WHERE M.msgType = 'DFT-P03'	
	AND M.systemMsgId IN (@msgID)

) --SELECT * FROM cteMsg
, cteCDM as
(SELECT 
*	FROM	(
	SELECT
	r.systemMsgId,
	@acc as [ACCOUNT]
	--'L'+a.alias.value('(//PID.18.1/text())					
	--		[1]', 'varchar(15)')				AS [ACCOUNT]
					
	, a.alias.value('(//PID.5.1/text() ) 
			[1] ','varchar(25)')				AS [PAT LAST NAME]
	, a.alias.value('(//PID.5.2/text() ) 
			[1] ','varchar(25)')				AS [PAT FIRST NAME]
	, a.alias.value('(//PID.5.3/text() ) 
			[1] ','varchar(25)')				AS [PAT MIDDLE NAME]

	, dbo.DateOfBirthFix( CAST(a.alias.value('(//PID.7/PID.7.1/text() ) 
			[1] ','varchar(8)') AS DATETIME) )	AS [PAT DOB]			
	, dbo.FormatSSN( a.alias.value('(//PID.19/PID.19.1/text() )
			[1] ','varchar(9)') ) AS [PAT SSN]
				
	, a.alias.value('(//PID.8/PID.8.1/text() )
			[1] ', 'varchar(1)') AS [PAT GENDER]
	
	, tests.cref.value('(./FT1.1/FT1.1.1/text() )
			[1]', 'int') AS [SET ID]
	, tests.cref.value('(./FT1.2/FT1.2.1/text() )
			[1]', 'varchar(25)') AS [TRANSACTION ID] -- [DOS]
	, tests.cref.value('(./FT1.3/FT1.3.1/text() )
			[1]', 'varchar(20)') AS [TRANSACTION BATCH ID]
	
	, tests.cref.value('(./FT1.4/FT1.4.1/text() )
			[1]', 'varchar(8)') AS [TRANSACTION DATE]

	, tests.cref.value('(./FT1.6/FT1.6.1/text() )
			[1]', 'varchar(1)') AS [TRANSACTION TYPE]
	-- everything to be excluded goes between [] below with 
	-- {and .!="xxxxxxx"} no longer necessary
, tests.cref.value('(./FT1.7/FT1.7.1/text())[1] 
			', 'varchar(7)')		AS [CDM]
	
	-- cdm groups can be placed like this using
	-- {or .="xxxxxxx"}
--	, tests.cref.value('(./FT1.7/FT1.7.1/text())[1]
--	[.="5959041" or .="MCL0003" or .="5929022" or .="5939033" or
--	 .="5768182" or .="5849600" or .="5939003"] 
--			', 'varchar(7)')		AS [PC CDM]	
--	, tests.cref.value('(./FT1.7/FT1.7.2/text())
--			[1]', 'varchar(50)')		AS [CDM DESCRIPTION]
		
	, CASE WHEN tests.cref.value('(./FT1.6/FT1.6.1/text() )
			[1]', 'varchar(1)') = 'D'
			THEN tests.cref.value('(FT1.10/FT1.10.1/text()) [1]', 'int')
			ELSE tests.cref.value('(FT1.10/FT1.10.1/text()) [1]', 'int')*-1
			END
	AS [QTY]
	
--	, cref.value('(./FT1.14/FT1.14.1/text())
--			[1]', 'varchar(50)')		AS [INSURANCE CODE]
	, cref.value('(./FT1.16/FT1.16.7/text())
			[1]', 'varchar(50)')		AS [PATIENT LOCATION]
	
	, cref.value('(FT1.18/FT1.18.1/text())
			[1]', 'varchar(10)')	AS [PAT_TYPE]	

	, cref.value('(FT1.23/FT1.23.1/text())  [1]', 'varchar(50)')
			AS [OV ORDER NO]
		
	FROM (SELECT cteMsg.systemMsgId ,xmlMsg AS rep_xml FROM cteMsg) r
	CROSS APPLY r.rep_xml.nodes	('HL7Message') a(alias)
	CROSS APPLY alias.nodes('//FT1') AS tests(cref) 
	
	) xd
	
)
INSERT INTO chrg (credited, account	, fname	, mname, lname	
, pat_name
,pat_ssn
,unitno			--,location	
,fin_code
,qty, cdm		
,service_date	
,facility
,mt_req_no
,referencereq	
, comment 
, post_file
, net_amt, inp_price, retail
, fin_type
,pat_dob,mod_date
, mod_user
, mod_prg
, mod_host
)
SELECT 0 AS credited, cteCDM.ACCOUNT, cteCDM.[PAT FIRST NAME],	cteCDM.[PAT MIDDLE NAME], cteCDM.[PAT LAST NAME] 
,LTRIM(RTRIM(cteCDM.[PAT LAST NAME]+','+cteCDM.[PAT FIRST NAME]+' '+ ISNULL(cteCDM.[PAT MIDDLE NAME],'')))
,cteCDM.[PAT SSN] ,cteCDM.[TRANSACTION BATCH ID] 
,acc.fin_code,		cteCDM.QTY,	cteCDM.CDM , cteCDM.[TRANSACTION DATE] 
,dbo.acc.cl_mnem AS [facility],	dbo.OVNumberTrim(cteCDM.[OV ORDER NO]) AS [accession] ,	cteCDM.[TRANSACTION ID] 
,'MSG ID: '+CAST(systemMsgId AS VARCHAR) AS [comment]
,	cteCDM.systemMsgId AS [post_file]		
,CASE WHEN acc.fin_code IN ('Y','CLIENT')
			then gfs.colClientPrice 
			ELSE gfs.colMPrice 
	END AS [net_amt]
, COALESCE(gfs.colZPrice,
		(SELECT zprice FROM cpt4 WHERE cdm = gfs.colCdm)
	) AS [colZPrice] -- updated 20151228
, gfs.colCPrice -- wdk 20151117 added
,CASE WHEN acc.fin_code IN ('Y','CLIENT')
			then 'C'
			ELSE 'M'
	END AS [fin_type]
,cteCDM.[pat dob]
,GETDATE() AS [mod_date]
,RIGHT(SUSER_SNAME(),50) AS [mod_user]
,RIGHT(concat('usp_cerner_chrg - ',@src_app),50) AS [mod_prg]
,RIGHT (HOST_NAME(),50)  AS [mod_host]

FROM cteCDM
INNER JOIN cdm ON cdm.cdm = cteCDM.cdm AND cdm.deleted = 0 -- eliminates the need to remove cdm's from cerner not listed in billing
INNER JOIN acc ON dbo.acc.account = cteCDM.ACCOUNT --AND acc.tdate_update = 0
OUTER APPLY dbo.GetFeeSchedulePrice(
	/*wdk 20160531*/
	COALESCE(NULLIF(acc.cl_mnem,'K'),'MCL')
	,cteCDM.cdm) gfs
WHERE cteCDM.CDM IS NOT NULL

SET @stage = 'AMT'

INSERT INTO dbo.amt
		(
			chrg_num ,cpt4 ,[type] , amount , modi ,revcode ,
			diagnosis_code_ptr ,order_code ,mt_req_no, 
			mod_date ,
			mod_user ,
			mod_prg 			
		)
SELECT 	
		dbo.chrg.chrg_num ,
		NULLIF(am.colCpt,'ERR') AS [colCpt] 
		,am.colType 
		,ISNULL(am.colCptPrice,0) AS [colCptPrice] 
		,am.colModi ,am.colRevCode		
		--,gpd.colDiagOrder AS [diag_code_ptr]
		, '1:' AS [diag_code_ptr]
		,cdm, chrg.mt_req_no 
		, GETDATE()
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT(concat('usp_cerner_chrg - ',@src_app),50) AS [mod_prg]		
		 FROM dbo.chrg
		 CROSS APPLY dbo.SplitCdmPriceByCpt(chrg.cdm, chrg.net_amt, dbo.chrg.fin_type) AS am
		 LEFT outer JOIN dbo.amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num		 
		WHERE post_file = CAST(@msgID AS VARCHAR) AND dbo.amt.chrg_num IS NULL
		-- added for charges that are not in the filling system to keep this from crashing
		--AND am.colCpt <> 'ERR' wdk 20151002


END
ELSE
BEGIN 
	SET @stage = 'CHRG NO ACC'
	RAISERROR (N'CHARGE HAS NO ACCOUNT %s', -- Message text.
           10, -- Severity,
           1, -- State,
           @acc); 

	
END -- AMT record




SET @stage = 'PAID'
IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc
		AND acc.status = 'PAID_OUT'))
BEGIN	
UPDATE dbo.acc
SET status = 'NEW'
, mod_prg = 'usp_cerner_chrg ' + CONVERT(VARCHAR,GETDATE(),112)
WHERE dbo.acc.account = @acc
END

-- Last statment in the file 
UPDATE infce.messages_inbound
SET processFlag = 'P'
, processStatusMsg = @acc +' - '+@fin
WHERE infce.messages_inbound.systemMsgId = @msgID
AND infce.messages_inbound.msgType = 'DFT-P03'



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
              account
            )
SELECT	'SQL',
            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 )AS [mod_prg],
            ERROR_PROCEDURE() AS ErrorProcedure,
            @acc+' : '+CONVERT(VARCHAR,@msgId)+' : '+ERROR_MESSAGE() AS ErrorMessage,
            GETDATE(),
            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 ) AS ErrorProcedure,
            RIGHT(SUSER_SNAME(),50),
            RIGHT (HOST_NAME(),50),
            @acc;
        
          UPDATE infce.messages_inbound
			SET processFlag = 'ERR',
			processStatusMsg = @stage+' - [ '+@acc+' ] '+RIGHT(ERROR_MESSAGE(),244)
			WHERE infce.messages_inbound.systemMsgId = @msgID
			AND infce.messages_inbound.msgType = 'DFT-P03'

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





