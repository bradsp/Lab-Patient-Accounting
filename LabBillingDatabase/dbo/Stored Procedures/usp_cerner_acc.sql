-- =============================================
-- Author:		David
-- Create date: 03/31/2015
-- Description:	Adds or updates data in the acc table from cerner hl7
-- =============================================
CREATE PROCEDURE usp_cerner_acc
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
--AND infce.messages_inbound.msgDate between DATEADD(DAY,-10,GETDATE())
--	AND DATEADD(DAY,-1,GETDATE())
AND infce.messages_inbound.msgType = 'ADT-A04'
--AND infce.messages_inbound.processFlag IN ('E','NP','N')
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
DECLARE @merge VARCHAR(50)

SET @merge = COALESCE((SELECT  
	'L'+ LTRIM(dbo.AccountTrim(a.alias.value('(//MRG.3.1/text() )[1]'
		, 'varchar(15)'))) AS [MRG]
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
		),NULL)

--SELECT @merge
IF (@merge IS NOT NULL)
BEGIN
UPDATE infce.messages_inbound
	SET processFlag = 'P',
	processStatusMsg = 'MERGE RECORD'
	WHERE infce.messages_inbound.systemMsgId = @msgID
	AND infce.messages_inbound.msgType = 'ADT-A04'
	RETURN;
END

---------------------------------------------------
-- get the account for message text
SET @acc = COALESCE((SELECT  
	'L'+ LTRIM(dbo.AccountTrim(a.alias.value('(//PID.18.1/text() )[1]'
		, 'varchar(15)'))) AS [ACCOUNT]
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message/PID') a(alias)
		),'NONE')

--SELECT @acc

DECLARE @msgType VARCHAR(20)
SET @msgType = (SELECT  
	LTRIM(a.alias.value('(//MSH.9.1/text() )[1]', 'varchar(10)')) AS [MSG TYPE]
	FROM (select @xml AS rep_xml) r
 
CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
		)

IF (@msgType IN ('DFT','MFN') )
BEGIN
RETURN
END
--SELECT @msgType
------------------------FIN CODE CHECK ------------
DECLARE @fin VARCHAR(20)
SET @fin = COALESCE((
	SELECT  COALESCE(
		dbo.GetMappingValue('FIN_CODE','CERNER',
			NULLIF(NULLIF(LTRIM(a.alias.value(
			'(//PV1.20.1/text() )[1]', 'varchar(10)') ),'EP'),'K') ),
		dbo.GetMappingValue('FIN_CODE','CERNER',
		   LTRIM(a.alias.value(
		   '(//IN1.2.1/text() )[1]', 'varchar(50)')) )
		)
	FROM (select @xml AS rep_xml) r
CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
		),NULL)

--SELECT @fin
--return
IF (NOT EXISTS (SELECT acc.account FROM acc WHERE dbo.acc.account = @acc))
BEGIN
--	SELECT @acc, CAST(@transDate AS DATETIME) AS [transDate], 
--	dbo.GetMappingValue('FIN_CODE','CERNER',@fin) AS [fin]

UPDATE infce.messages_inbound
	SET processFlag = 
	 (CASE WHEN DATEDIFF(DAY,CAST(@transDate AS DATETIME),GETDATE()) <= 5
	THEN 'NP'
	ELSE 'ERR' END ),
	processStatusMsg = 'CHARGE DOES NOT EXIST'
	WHERE infce.messages_inbound.systemMsgId = @msgID
	AND infce.messages_inbound.msgType = 'ADT-A04'
	
	RETURN;
END


--IF (NOT EXISTS (SELECT return_value FROM dictionary.mapping 
--	WHERE dictionary.mapping.sending_value = @fin AND ))
IF (NOT EXISTS (SELECT dbo.GetMappingValue('FIN_CODE','CERNER',@fin) ) )
BEGIN
UPDATE infce.messages_inbound
	SET processFlag = 'ERR',
	processStatusMsg = 'INVALID FIN CODE: ['+@fin+' ]'+'[ '+@acc+' ] '
	WHERE infce.messages_inbound.systemMsgId = @msgID
	AND infce.messages_inbound.msgType = 'ADT-A04'
	--RETURN; go ahead and process the insurance
END

DECLARE @client VARCHAR(10)
--SET @client = (select dbo.GetMappingValue('client','cerner',
--(SELECT @xml.value('data(//PID.3.4/text())[1]','varchar(10)')) ))
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

--SELECT @client
--SELECT @fin AS [fin], @acc AS [account], @merge AS [merge], @msgID AS [id]
--SELECT dbo.GetMappingValue('fin_code','Cerner',@fin) AS [mappedfin], @acc AS [account], @merge AS [merge], @msgID AS [id]
--, @client
--RETURN;

BEGIN TRY
BEGIN TRANSACTION

DECLARE @stage VARCHAR(3)
--SET @stage = 'ACC'

/*
20150810 update the account record's fin_code if different 
/*20150630 wdk don't process duplicate adt's*/
/*20150826 AFTER YESTERAYS MEETING UPDATE ANYWAY

IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc))
BEGIN	
UPDATE infce.messages_inbound
	SET processFlag = 'P',
	processStatusMsg = 'DUP ADT'
	WHERE infce.messages_inbound.systemMsgId = @msgID
	AND infce.messages_inbound.msgType = 'ADT-A04'
	--RETURN;
END
*/
--select 1/0 testing of processStatusMessage trapping. works as of 04/01/2015
-- account
IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc))
/* as of 20150626 10:45 do not update per Jerry/Carol
 as of 20150824 update 
*/
BEGIN
--	IF (EXISTS( SELECT account FROM acc WHERE acc.account = @acc	
--		AND acc.fin_code <> @fin))
--	BEGIN
		UPDATE acc 
		SET fin_code = @fin
		, acc.cl_mnem = @client
		, acc.status = ISNULL(NULLIF(acc.status,'PAID_OUT'),'NEW')
		FROM dbo.acc 
		WHERE acc.account = @acc
		AND acc.fin_code <> @fin
		
		-- then update the charges
		EXEC dbo.usp_prgProcessChargeADT @acc = @acc
--	END
END
*/	
SET @stage = 'GTR'

-- guarantor
IF (EXISTS (SELECT account FROM pat WHERE dbo.pat.account = @acc))
BEGIN
DECLARE @guarID			VARCHAR(50)	-- goes in Account table
DECLARE @guarLastName	VARCHAR(25) -- the rest go in the pat table
DECLARE @guarFirstName	VARCHAR(25)
DECLARE @guarMiddleName VARCHAR(25)
DECLARE	@guarMI			VARCHAR(1)
DECLARE @guarAddr1		VARCHAR(40)
DECLARE @guarAddr2		VARCHAR(40)
DECLARE @guarCity		VARCHAR(40)
DECLARE @guarState		VARCHAR(2)
DECLARE @guarZip		VARCHAR(11)
DECLARE @guarPhone		VARCHAR(15)
DECLARE @guarDob		VARCHAR(8)
DECLARE @guarRel		VARCHAR(2)

set @guarLastName = (select 
	LTRIM(RTRIM(@xml.value('data(//GT1.3.1/text()) [1]',	'varchar(25)'))) )
set @guarFirstName = (select 
	LTRIM(RTRIM(@xml.value('data(//GT1.3.2/text()) [1]',	'varchar(25)') )) )
set @guarMiddleName = (select 
	LTRIM(RTRIM(@xml.value('data(//GT1.3.3/text()) [1]',	'varchar(25)'))) )
set	@guarMI = (select 
	LTRIM(RTRIM(@xml.value('data(//GT1.3.3/text()) [1]',	'varchar(1)')   )) )
set @guarAddr1 = (select @xml.value('data(//GT1.5.1/text()) [1]',		'varchar(40)') )
set @guarAddr2 = (select @xml.value('data(//GT1.5.2/text()) [1]',		'varchar(40)') )
set @guarCity = (select @xml.value('data(//GT1.5.3/text()) [1]',		'varchar(40)')  )
set @guarState = (select @xml.value('data(//GT1.5.4/text()) [1]',		'varchar(2)')  )
set @guarZip = (select @xml.value('data(//GT1.5.5/text()) [1]',			'varchar(5)')  )
set @guarPhone = (select @xml.value('data(//GT1.6.1/text()) [1]',		'varchar(15)')  )
set @guarDob = (select @xml.value('data(//GT1.8.1/text()) [1]',		'varchar(8)')	 )
set @guarRel = (select COALESCE(REPLACE(REPLACE( @xml.value('data(//GT1.11.1/text())[1]','varchar(2)'),'SE','01'),'Default Guarantor', '01'),'01') )
SET @guarID = (select @xml.value('data(//GT1.2.1/text() ) [1]','varchar(50)') )
--SELECT  @guarLastName,@guarFirstName,@guarMiddleName,@guarMI
--,@guarAddr1,@guarAddr2,@guarCity,@guarState,@guarZip
--,@guarPhone,@guarDob, @guarRel 

UPDATE dbo.pat
SET pat.guarantor = UPPER(@guarLastName)
			+', '+upper(@guarFirstName)+' '
			+	ISNULL(upper(@guarMiddleName),'')
	,guar_addr	= UPPER(@guarAddr1)
	,guar_city = UPPER(@guarCity)
	,guar_state = UPPER(@guarState)
	,guar_zip	= @guarZip
	,g_city_st = UPPER(@guarCity) +', '
			+	UPPER(@guarState) +' '+@guarZip 

	,pat.guar_phone = dbo.FormatPhoneNo(@guarPhone)
FROM dbo.pat
WHERE pat.account = @acc

IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc
	AND dbo.acc.guarantorID <> @guarID AND @guarID IS NOT NULL))
BEGIN
UPDATE dbo.acc
SET guarantorID = @guarID
FROM acc 
WHERE account = @acc
end

END

SET @stage = 'ACC'
IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc
	AND dbo.acc.cl_mnem = 'K'))
BEGIN
	UPDATE dbo.acc
	SET cl_mnem = @client
	, dbo.acc.status = ISNULL(NULLIF(acc.status,'PAID_OUT'),'NEW')
	FROM acc WHERE dbo.acc.account = @acc
--	PRINT 'before recharge'
	EXEC dbo.usp_prg_ReCharge_Acc_Transaction @acc = @acc -- varchar(15)
--	PRINT 'after recharge'
	
	
--select @stage
END	


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
IF (NOT EXISTS (SELECT account FROM ins 
WHERE dbo.ins.account = @acc)
-- wdk 20160226 added the below line to eliminate the insurance for 'Y' accounts
AND NOT EXISTS (SELECT acc.account FROM acc WHERE account = @acc AND acc.fin_code = 'Y' )
)
/* as of 20150626 10:45 do not update per Jerry/Carol*/
BEGIN
	EXEC sp_xml_preparedocument @iDoc OUTPUT, @xml

	INSERT INTO dbo.ins
			(
				account ,		ins_a_b_c ,		holder_nme ,	
				holder_dob ,	holder_sex ,
				holder_addr ,	holder_city_st_zip ,
				plan_nme ,		policy_num ,
				grp_nme ,		grp_num ,
				fin_code ,		ins_code ,
				relation ,		mod_date ,
				mod_user ,		mod_prg ,
				mod_host ,		holder_lname ,
				holder_fname ,	holder_mname
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
		COALESCE(iIns.ins_ov_code,iIns.ins_plan_name)),'ERR','0') AS [ins_fin_code]
	, dbo.GetMappingValue('INS_CODE','CERNER', iIns.ins_ov_code) AS [ins_ov_code]
	, COALESCE(iIns.ins_rel_to_pat,'01')
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
, ins_fin_code = COALESCE(@fin,'NONE')
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
