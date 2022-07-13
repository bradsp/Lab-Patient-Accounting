-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Processes the XML import
-- =============================================
CREATE PROCEDURE usp_prg_Xml_Import_Charges_Verify
	-- Add the parameters for the stored procedure here
	@file VARCHAR(50)
	--,@acc VARCHAR(15)

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
--		[row_count] INT NULL,
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
	SET @file = NULLIF(@file,'')
	
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
	-- copy file from above to below for testing
	from OPENROWSET (BULK '\\wthmclbill\MedicareRemit\POST_CHARGE\20140702.XML', SINGLE_BLOB) as x)

	END
	
	
	SET @idoc = (SELECT @doc.value('count(data(distinct-values(/postingbatch/encounter/@ID)))','int') AS [ACCOUNTS])
	PRINT @idoc

	EXEC sp_xml_preparedocument @idoc OUTPUT, @doc


-- works SELECT CONVERT(DATETIME,'07/03/2014')

DECLARE @batch VARCHAR(16)
--SET @batch = REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(16),GETDATE(),126),'-',''),'T',''),':','')
SET @batch = REPLACE(@file,'MT.XML','')
SELECT @batch
/* chrg time*/
; with cteChrg
as
(
select DISTINCT 
	ROW_NUMBER() OVER (order BY COALESCE(colAcc,dbo.AccountTrim(xmlChrg.account))
	--ROW_NUMBER() OVER (order BY dbo.AccountTrim(xmlChrg.account)
		,coalesce(dc.bill_cdm,billcode)) AS [rn],
COALESCE(colAcc,dbo.AccountTrim(xmlChrg.account)) as [account]
--dbo.AccountTrim(xmlChrg.account) as [account]
, dbo.FormatClient(facility, client ) AS [client]
, REPLACE(patientname,'"','') AS [Patient]
, dbo.FormatSSN(patientssno) as [pat_ssn] 
, REPLACE(unitno,'"','') AS  [unitno]
, REPLACE([status],'"','') AS [status]
, REPLACE(location,'"','') AS [location]
,	dbo.AttendPhyTrim(responsiblephy) AS [responsiblephy] 
, dbo.FormatFinCode(financialclass, facility) AS [financialclass]
, CAST(REPLACE(quantity,'"','') AS int) AS [quantity]
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
	(select cSUFFIX from ufn_Split_Name(patientname)) as [pat_suffix]

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
		 location varchar(50)		'location',
		 responsiblephy varchar(50) 'responsiblephy',
		 financialclass varchar(50) 'financialclass',
		 quantity	VARCHAR(3)				'quantity',
		 ordersite varchar(100)		'ordersite',
		 --		[. = "5939051" or . = "5959041" or . ="5929006" or . ="5929022" or . ="5849600" or . ="5939033" or . ="5949041"  ]', from usp_prg_Xml_UnProcessed_Charges
		 billcode varchar(7)		'billcode 		[.!= "5849510" and .!= "5939047" 
		 and . !="5849510" and . !="5939003" and . !="5939051" and . !="5959041" 
		 and . !="5929006" and . !="5929022" and . !="5849600" and . !="5939033" 
		 and . !="5949041"]',
		 [test] varchar(50)			'test',
		 [action] varchar(50)		'action',
		 [collectdate] DATETIME 	'collectdate',
		 performingsite varchar(50)	'performingsite',
		 facility varchar(10)		'facility',
		 client   varchar(10)		'responsiblephy [. != "zLPT"]',
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

select --DISTINCT-- don't use distinct as some tests can be ordered multiple times on the same day for the patient
CASE WHEN SUM(cteChrg.quantity) OVER (PARTITION BY cteChrg.account,
cteChrg.collectdate,cteChrg.billcode,cteChrg.number,cteChrg.test) = 0
	THEN 1
	ELSE 0 END AS [credited],
cteChrg.account, cteChrg.pat_fname,	cteChrg.pat_mname, cteChrg.pat_lname, cteChrg.pat_suffix
,	cteChrg.Patient,	cteChrg.pat_ssn,	cteChrg.unitno
,cteChrg.location, cteChrg.responsiblephy
,COALESCE(acc.fin_code,cteChrg.responsiblephy)--cteChrg.financialclass
,cteChrg.quantity,	cteChrg.ordersite
,cteChrg.billcode AS [CDM],cteChrg.test,	cteChrg.action,	cteChrg.collectdate
,coalesce(cteChrg.performingsite,cteChrg.responsiblephy) as [performingsite]
, cteChrg.facility,cteChrg.istemp,cteChrg.number,cteChrg.referencereq,cteChrg.dob
, NULL
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

--left outer join chrg on chrg.post_file = @file
left outer join fin on fin.fin_code = cteChrg.financialclass--colFinCode 
left outer join client on client.cli_mnem = COALESCE(acc.fin_code,cteChrg.responsiblephy)
WHERE cteChrg.billcode IS NOT NULL

ORDER BY cteChrg.account, cteChrg.billcode



SET @COUNT = @@ROWCOUNT

print '17. Chrg Table rows that would be added ' + convert(varchar(10),@COUNT)




--ALTER TABLE chrg drop COLUMN age_on_date_of_service
--ALTER TABLE chrg ADD age_on_date_of_service AS NULLIF((datediff(year,coalesce([pat_dob],getdate()-(1)),coalesce([service_date],getdate()-(1)))),0)
--(datediff(year,coalesce([pat_dob],getdate()-(1)),coalesce([service_date],getdate()-(1))))
--SELECT pat_dob,NULLIF((datediff(year,coalesce([pat_dob],getdate()-(1)),coalesce([service_date],getdate()-(1)))),0)
--FROM chrg WHERE account = 'C7140358'




EXEC sp_xml_removedocument @idoc


	
	



END	









