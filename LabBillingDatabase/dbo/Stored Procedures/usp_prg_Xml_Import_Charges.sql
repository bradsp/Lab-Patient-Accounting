-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_Xml_Import_Charges]
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
	
	SET @file = COALESCE(NULLIF(@file,''),(select TOP(1) import_file FROM dbo.XmlSourceTable WHERE processed = 0 
		ORDER BY import_file) )

BEGIN TRY
BEGIN TRANSACTION

	DECLARE @data VARCHAR(MAX)
	SET @data = (select TOP(1) doc_data FROM dbo.XmlSourceTable WHERE import_file = @file
		ORDER BY import_file)
	DECLARE @doc xml
	SET @doc = (SELECT CONVERT(XML,@data))
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/@ID)))','int') AS [ACCOUNTS])


	EXEC sp_xml_preparedocument @idoc OUTPUT, @doc




DECLARE @batch VARCHAR(16)
--SET @batch = REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(16),GETDATE(),126),'-',''),'T',''),':','')
SET @batch = REPLACE(@file, 'MT.XML','')
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
, COALESCE(financialclass, facility) AS [financialclass]
, CAST(REPLACE(quantity,'"','') AS INT) AS [quantity]
, REPLACE(ordersite,'"','') AS [ordersite]
, coalesce(dc.bill_cdm,REPLACE(billcode,'"','')) as [billcode]--, dcc.bill_code as [converted_cdm]
, REPLACE([test],'"','') AS [test] 
, REPLACE([action],'"','') AS [action]
, collectdate
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
	(select NULLIF(cSUFFIX,'') from ufn_Split_Name(patientname)) as [pat_suffix]

, dbo.DateOfBirthFix(dob) as [dob]
, datediff(year,dbo.DateOfBirthFix(dob),collectdate) as [age]
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
		 
		 facility varchar(10)		'facility',
		 location varchar(50)		'location',
		 client   varchar(10)		'responsiblephy',
		 responsiblephy varchar(50) 'responsiblephy',
		 financialclass varchar(50) 'financialclass',
		 
		 quantity varchar(50)		'quantity',
		 ordersite varchar(100)		'ordersite',
		 billcode varchar(7)		'billcode[.!= "5849510" and .!= "5939047" and .!= "5939003" and . != "5939051" and . != "5959041" and .!="5929006" and .!="5929022" and .!="5849600" and .!="5939033" and .!="5949041"  ]',
		 [test] varchar(50)			'test',
		 [action] varchar(50)		'action',
		 [collectdate] DATETIME 	'collectdate',
		 performingsite varchar(50)	'performingsite',
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
			
) 
INSERT INTO chrg (
credited, account	, fname	, mname, lname	,name_suffix,
pat_name		,pat_ssn		,unitno			--,[status]
,location	,responsiblephy	,fin_code		,qty			,order_site
,cdm		,mt_mnem		,[action]		,service_date	,performing_site
,facility	,istemp			,mt_req_no		,referencereq	,pat_dob	
,mod_date		, mod_user		, mod_prg		, mod_host
,fin_type , comment ,post_file, net_amt--, age_on_date_of_service
)

select --DISTINCT-- don't use distinct as some tests can be ordered multiple times on the same day for the patient
CASE WHEN SUM(cteChrg.quantity) OVER (PARTITION BY cteChrg.account, cteChrg.collectdate,
cteChrg.billcode, cteChrg.number, cteChrg.test) = 0 
	THEN 1 ELSE 0 END AS [credited]
,cteChrg.account,cteChrg.pat_fname,cteChrg.pat_mname,cteChrg.pat_lname,cteChrg.pat_suffix
,	cteChrg.Patient,	cteChrg.pat_ssn,	cteChrg.unitno
,cteChrg.location, cteChrg.responsiblephy
,COALESCE(NULLIF(acc.fin_code,'U'),cteChrg.financialclass) AS [fin_code]
,cteChrg.quantity,	cteChrg.ordersite
,cteChrg.billcode AS [CDM],cteChrg.test,	cteChrg.action,	cteChrg.collectdate
,coalesce(cteChrg.performingsite,cteChrg.responsiblephy) as [performingsite]
, cteChrg.facility,cteChrg.istemp,cteChrg.number,cteChrg.referencereq,cteChrg.dob
, @modDate
, RIGHT(SUSER_SNAME(),50) AS [mod_user]
, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
	'XML CHRG IMPORT ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
, RIGHT (HOST_NAME(),50)  AS [mod_host]
,coalesce(fin.type,'C') AS [fin_type]
,'Batch: ' + @batch AS [comment]
,	@file AS [post_file]
,CASE WHEN fin.TYPE = 'C' THEN colClientPrice
	ELSE colMprice END AS [net_amt]

from cteChrg
INNER JOIN acc ON acc.account = cteChrg.account
OUTER APPLY dbo.GetFeeSchedulePrice(cteChrg.client, cteChrg.billcode )
left outer join chrg on chrg.post_file = @file
left outer join fin on fin.fin_code = COALESCE(NULLIF(acc.fin_code,'U'),cteChrg.financialclass)
left outer join client on client.cli_mnem = cteChrg.responsiblephy
where chrg.post_file is NULL  AND cteChrg.billcode IS NOT null


SET @COUNT = @@ROWCOUNT
INSERT INTO temp_track(comment,row_count,ERROR)
VALUES('Chrg Table rows added ' ,@COUNT, @@ERROR)
--INSERT INTO temp_track(comment,ERROR)
--VALUES('17. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
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
INSERT INTO temp_track(comment,row_count,ERROR)
VALUES('Amt Table rows added ' ,@COUNT, @@ERROR)
--INSERT INTO temp_track(comment,ERROR)
--VALUES('18. ET Time in seconds '+ CAST(DATEDIFF(SECOND,@startTime,GETDATE()) AS varchar(10)),@@ERROR)
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
N'<tr bgcolor ="blue"><th>ACTION</th><th>ROW COUNT</th><th>ERROR</th>'+
N'<th>START TIME</th></tr>' +

CAST (( select td = comment,'',
		td = COALESCE(row_count,0),'',
		td = COALESCE(error,'N/A'),'',
		td = CONVERT(VARCHAR(10),start_time,108),' '
from temp_track
for XML PATH('tr'), TYPE) as NVARCHAR(MAX))+
N'</Table>')

if (len(@tableHtml) > 0)
BEGIN
	set @sub = 'XML Post Charge as of  ' + convert(varchar(17),getdate(),109)
	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = 'WTHMCLBILL',
	@recipients = 'david.kelly@wth.org;bradley.powers@wth.org',
	--@recipients = 'carol.sellars@wth.org;jan.smith@wth.org;cheryl.lane@wth.org',
	--@blind_copy_recipients=N'bradley.powers@wth.org; david.kelly@wth.org',
	@body = @tableHtml,
	@subject = @sub,
	@body_format = 'HTML';

	PRINT 'EMAIL SENT'
END

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









