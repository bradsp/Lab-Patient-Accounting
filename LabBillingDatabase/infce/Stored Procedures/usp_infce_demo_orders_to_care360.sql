-- =============================================
-- Author:		Bradley Powers
-- Create date: 9/11/2013
-- Description:	Create the XML HL7 format of messages on requisitions
--	entered in the GOMCL database.
-- =============================================
CREATE PROCEDURE [infce].[usp_infce_demo_orders_to_care360]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
--	DECLARE @xmlData varchar(MAX);
	DECLARE @infce_name VARCHAR(20);

	IF DB_NAME() = 'MCLLIVE'
	BEGIN
		SET @infce_name = 'CARE360LIVE'
	END
	ELSE
	BEGIN
		SET @infce_name = 'CARE360TEST'
	END

	--FORMAT THE XML STRING
	--MSH SEGMENT
	select a.patid, a.account,a.uid,
		(SELECT	(select
				'^~\&' as 'MSH/MSH.2',
				'LAB' as 'MSH/MSH.3', --SENDING APPLICATION
				'MEDICAL CENTER LABORATORY' as 'MSH/MSH.4', --SENDING FACILITY
				'CARE360' AS 'MSH/MSH.5', --RECEIVING APPLICATION
				'QUEST DIAGNOSTICS' AS 'MSH/MSH.6', --REVEIVING FACILITY
				dbo.fnFormatDate(GETDATE(),'YYYYMMDDhhii') as 'MSH/MSH.7',
				'ADT' AS 'MSH/MSH.9/MSH.9.1',
				'A19' AS 'MSH/MSH.9/MSH.9.2',
				@infce_name+'.'+CONVERT(VARCHAR(20),mi.uid) AS 'MSH/MSH.10',
				'P' as 'MSH/MSH.11',
				'2.3' as 'MSH/MSH.12',
				@infce_name+'.'+CONVERT(VARCHAR(20),mi.uid) as 'MSH/MSH.13'
			FROM data_quest_360 mi WHERE mi.uid = a.uid
			FOR XML PATH(''),TYPE),
			--PID SEGMENT
			(select
				--QRD segment
				'' as 'QRD/QRD.1', -- QUERY DATE/TIME
				'R' AS 'QRD/QRD.2',
				'I' AS 'QRD/QRD.3',
				'1' AS 'QRD/QRD.4',
				'1' AS 'QRD/QRD.7/QRD.7.1',
				'RD' AS 'QRD/QRD.7/QRD.7.2',
				CASE 
					WHEN acc.mri IS NULL THEN acc.account
					WHEN acc.mri = '' THEN acc.account
					ELSE acc.mri 
				END AS 'QRD/QRD.8', --UNIQUE PATIENT ID
				'DEM' AS 'QRD/QRD.9', --WHAT SUBJECT FILTER
				'' AS 'QRD/QRD.10', --SITE SPECIFIC
				'T' AS 'QRD/QRD.12', --QUERY RESULTS LEVEL
				--PID SEGMENT
				'1' AS 'PID/PID.1',
				CASE 
					WHEN acc.mri IS NULL THEN acc.account
					WHEN acc.mri = '' THEN acc.account
					ELSE acc.mri END as 'PID/PID.2/PID.2.1',
				acc.account as 'PID/PID.2/PID.2.2',
				dbo.GetNamePart(acc.pat_name,'LAST') as 'PID/PID.5/PID.5.1',
				dbo.GetNamePart(acc.pat_name,'FIRST') as 'PID/PID.5/PID.5.2',
				dbo.GetNamePart(acc.pat_name,'MIDDLE') as 'PID/PID.5/PID.5.3',
				CONVERT(VARCHAR(8), pat.dob_yyyy, 112) as 'PID/PID.7',
				pat.sex as 'PID/PID.8',
				pat.pat_addr1 as 'PID/PID.11/PID.11.1',
				pat.pat_addr2 as 'PID/PID.11/PID.11.2',
				SUBSTRING(pat.city_st_zip,0,charindex(',',pat.city_st_zip)) as 'PID/PID.11/PID.11.3',
				SUBSTRING(pat.city_st_zip,charindex(',',pat.city_st_zip)+2,2) as 'PID/PID.11/PID.11.4',
				dbo.GetNamePart(pat.city_st_zip,'MIDDLE') as 'PID/PID.11/PID.11.5',
				pat.guar_phone AS 'PID/PID.13',
				pat.ssn as 'PID/PID.19'
			FROM data_quest_360 mi 
			LEFT OUTER JOIN dbo.acc ON mi.account = acc.account
			LEFT OUTER JOIN dbo.pat ON mi.account = pat.account
			WHERE mi.uid = a.uid
			FOR XML PATH(''),TYPE),
			--PV1 SEGMENT
			(SELECT
				'1' as 'PV1/PV1.1',
				'P' as 'PV1/PV1.2',
				phy.billing_npi AS 'PV1/PV1.7/PV1.7.1',
				ISNULL(phy.last_name,'') AS 'PV1/PV1.7/PV1.7.2',
				ISNULL(phy.first_name,'') AS 'PV1/PV1.7/PV1.7.3',
				ISNULL(phy.mid_init,'') AS 'PV1/PV1.7/PV1.7.4/PV1.7.4.1',
				ISNULL(phy.upin,'') as 'PV1/PV1.7/PV1.7.4/PV1.7.4.2',
				'UPIN' as 'PV1/PV1.7/PV1.7.12',
				phy.billing_npi AS 'PV1/PV1.8/PV1.8.1',
				ISNULL(phy.last_name,'') AS 'PV1/PV1.8/PV1.8.2',
				ISNULL(phy.first_name,'') AS 'PV1/PV1.8/PV1.8.3',
				ISNULL(phy.mid_init,'') AS 'PV1/PV1.8/PV1.8.4'
			FROM data_quest_360 mi 
			LEFT OUTER JOIN acc ON mi.account = acc.account
			LEFT OUTER JOIN pat ON mi.account = pat.account
			LEFT OUTER JOIN phy ON pat.phy_id = phy.tnh_num AND phy.deleted = 0
			WHERE mi.uid = a.uid
			FOR XML PATH(''),TYPE),
			--DG1 SEGMENT
			(SELECT
				icd.link AS 'DG1/DG1.1',
				icd.icd9 as 'DG1/DG1.3/DG1.3.1',
				icd.icd9_desc as 'DG1/DG1.3/DG1.3.2',
				'I9' as 'DG1/DG1.3/DG1.3.3',
				'F' AS 'DG1/DG1.6',
				phy.billing_npi AS 'DG1/DG1.16/DG1.16.1',
				ISNULL(phy.last_name,'') AS 'DG1/DG1.16/DG1.16.2',
				ISNULL(phy.first_name,'') AS 'DG1/DG1.16/DG1.16.3',
				ISNULL(phy.mid_init,'') AS 'DG1/DG1.16/DG1.16.4'
			FROM data_quest_360 mi 
			LEFT OUTER JOIN dbo.GetPatIcd9(a.account) icd on mi.account = icd.account
			LEFT OUTER JOIN pat ON mi.account = pat.account
			--LEFT OUTER JOIN #icdLines icd on mi.account = icd.account
			LEFT OUTER JOIN phy on pat.phy_id = phy.tnh_num AND phy.deleted = 0
			WHERE mi.uid = a.uid
			ORDER BY icd.link
			FOR XML PATH(''),TYPE),
			--GT1 SEGMENT
			(SELECT
				'1' as 'GT1/GT1.1',
				dbo.GetNamePart(pat.guarantor,'LAST') as 'GT1/GT1.3/GT1.3.1',
				dbo.GetNamePart(pat.guarantor,'FIRST') as 'GT1/GT1.3/GT1.3.2',
				dbo.GetNamePart(pat.guarantor,'MIDDLE') as 'GT1/GT1.3/GT1.3.3',
				pat.guar_addr as 'GT1/GT1.5/GT1.5.1',
				SUBSTRING(pat.g_city_st,0,charindex(',',pat.g_city_st)) as 'GT1/GT1.5/GT1.5.3',
				SUBSTRING(pat.g_city_st,charindex(',',pat.g_city_st)+2,2) as 'GT1/GT1.5/GT1.5.4',
				dbo.GetNamePart(pat.g_city_st,'MIDDLE') as 'GT1/GT1.5/GT1.5.5',
				pat.guar_phone as 'GT1/GT1.6'
			FROM data_quest_360 mi 
			LEFT OUTER JOIN acc ON mi.account = acc.account
			LEFT OUTER JOIN pat ON mi.account = pat.account
			WHERE mi.uid = a.uid
			FOR XML PATH(''),TYPE),
			--INSURANCE (IN1) SEGMENT
			(SELECT
				CASE ins_a_b_c
					WHEN 'A' then '1'
					WHEN 'B' then '2'
					WHEN 'C' then '3'
				END as 'IN1/IN1.1',
				ins_code as 'IN1/IN1.2',
				'' as 'IN1/IN1.3',
				plan_nme as 'IN1/IN1.4',
				ISNULL(REPLACE(plan_addr1,CHAR(0),''),'') as 'IN1/IN1.5/IN1.5.1',
				plan_addr2 as 'IN1/IN1.5/IN1.5.2',
				substring(p_city_st,0,charindex(',',p_city_st)) as 'IN1/IN1.5/IN1.5.3',
				substring(p_city_st,charindex(',',p_city_st)+2,2) as 'IN1/IN1.5/IN1.5.4',
				dbo.GetNamePart(p_city_st,'MIDDLE') as 'IN1/IN1.5/IN1.5.5', --use the GetNamePart function to get the zip code
				'' as 'IN1/IN1.7', --phone
				ISNULL(REPLACE(grp_num,CHAR(0),''),'') as 'IN1/IN1.8', -- GROUP NUMBER
				grp_nme as 'IN1/IN1.9', --GROUP NAME
				'' as 'IN1/IN1.12', --PLAN EFFECTIVE DATE
				'' AS 'IN1/IN1.13', --PLAN TERMINATION DATE
				'' AS 'IN1/IN1.15', -- PLAN TYPE
				dbo.GetNamePart(holder_nme,'LAST') as 'IN1/IN1.16/IN1.16.1',
				dbo.GetNamePart(holder_nme,'FIRST') as 'IN1/IN1.16/IN1.16.2',
				dbo.GetNamePart(holder_nme,'MIDDLE') as 'IN1/IN1.16/IN1.16.3',
				dbo.GetNamePart(holder_nme,'SUFFIX') as 'IN1/IN1.16/IN1.16.4',
				relation as 'IN1/IN1.17',
				CONVERT(VARCHAR(10), holder_dob, 101) AS 'IN1/IN1.18', --INSUREDS DATE OF BIRTH
				ISNULL(REPLACE(holder_addr,CHAR(0),''),'') AS 'IN1/IN1.19/IN1.19.1', --INSUREDS ADDRESS
				SUBSTRING(holder_addr,0,CHARINDEX(',',holder_addr)) as 'IN1/IN1.19/IN1.19.3',
				SUBSTRING(holder_addr,CHARINDEX(',',holder_addr)+2,2) as 'IN1/IN1.19/IN1.19.4',
				dbo.GetNamePart(holder_addr,'MIDDLE') as 'IN1/IN1.19/IN1.19.5',
				CASE ins_a_b_c
					WHEN 'A' then '1'
					WHEN 'B' then '2'
					WHEN 'C' then '3'
				END as 'IN1/IN1.22',--COORDINATION OF BENEFITS PRIORITY
				policy_num as 'IN1/IN1.36',
				holder_sex AS 'IN1/IN1.43', --INSUREDS SEX
				SUBSTRING(e_city_st,0,CHARINDEX(',',e_city_st)) as 'IN1/IN1.44/IN1.44.3',
				SUBSTRING(e_city_st,CHARINDEX(',',e_city_st)+2,2) as 'IN1/IN1.44/IN1.44.4',
				dbo.GetNamePart(e_city_st,'MIDDLE') as 'IN1/IN1.44/IN1.44.5',
				cert_ssn AS 'IN2/IN2.2', -- INSURED SS NO
				employer AS 'IN2/IN2.3/IN2.3.2', -- INSUREDS EMPLOYER CODE AND NAME
				'I' AS 'IN2/IN2.5'
			FROM data_quest_360 mi 
			LEFT OUTER JOIN acc ON mi.account = acc.account
			LEFT OUTER JOIN ins ON mi.account = ins.account
			WHERE mi.uid = a.uid AND ins.deleted = 0
			FOR XML PATH(''),TYPE),
			--order and charge information
			--ORC--
			(select
				ROW_NUMBER() OVER(ORDER BY qc.chrg_num) as 'FT1/FT1.1',
				CONVERT(VARCHAR(8),qc.dos,112) AS 'FT1/FT1.4', --TRANSACTION DATE
				'CG' AS 'FT1/FT1.6', --TRANSACTION TYPE
				qc.[Quest Code] AS 'FT1/FT1.7/FT1.7.1', -- TRANSACTION CODE
				qc.[Quest Description] AS 'FT1/FT1.7/FT1.7.2', -- TRANSACTION DESCRIPTION
				'QUEST' AS 'FT1/FT1.7/FT1.7.3', -- NAME OF CODING SYSTEM
				qc.cdm AS 'FT1/FT1.7/FT1.7.4', --ALT CODE
				'' AS 'FT1/FT1.7/FT1.7.5', --ALT DESCRIPTION
				'MCL' AS 'FT1/FT1.7/FT1.7.6', --ALT CODING SYSTEM
				qc.qty as 'FT1/FT1.10', --TRANSACTION QUANTITY
				'' AS 'FT1/FT1.20', --PERFORMED BY CODE
				qc.chrg_num AS 'FT1/FT1.23' --FILLER ORDER NUMBER
			FROM data_quest_360 mi 
			LEFT OUTER JOIN acc  ON mi.account = acc.account
			--LEFT OUTER JOIN #questCharges qc ON mi.account = qc.account
			LEFT OUTER JOIN dbo.GetAccountQuestChargeCodes(a.account,SUBSTRING(patid,CHARINDEX('[CH',patid),9)) qc on mi.account = qc.account
			WHERE mi.uid = a.uid
			FOR XML PATH(''),TYPE)
		FOR XML PATH(''), TYPE,
		ROOT('HL7Message')) as [xmlData]
	FROM data_quest_360 a
	where bill_type = 'Q' and entered = 0 and deleted = 0
END
