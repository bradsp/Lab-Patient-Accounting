-- =============================================
-- Author:		Bradley Powers
-- Create date: 01/23/2014
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[usp_infce_psa_demographics_special] 
	-- Add the parameters for the stored procedure here
	--@createDate DATETIME = ''

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @beginDate DATETIME;
	DECLARE @endDate DATETIME;
	
--	SET @beginDate = DATEADD(Day,0,DATEDIFF(Day,0,GETDATE()-5));
--	SET @endDate = DATEADD(Day, 0, DATEDIFF(Day, 0, GETDATE()-4));
	
	SET @beginDate = '05/10/2015';
	SET @endDate = '05/10/2015'

    -- Insert statements for procedure here
	--HL7 DATA FILE TO PSA FOR PATHOLOGY PATIENTS--
	SELECT a.account, 
		(SELECT
			--MSH SEGMENT
			--'MSH' as 'MSH/MSH.0',
			--'|' as 'MSH/MSH.1',
			'^~\&' AS 'MSH/MSH.2',
			'LAB' AS 'MSH/MSH.3',
			'LAB' AS 'MSH/MSH.4',
			'PSAPATH' AS 'MSH/MSH.5',
			'PSAPATH' AS 'MSH/MSH.6',
			CONVERT(VARCHAR(10),getdate(),112) as 'MSH/MSH.7', --date/time of message
			'ADT' as 'MSH/MSH.9/MSH.9.1',
			'A19' as 'MSH/MSH.9/MSH.9.2',
			'0001' AS 'MSH/MSH.10',
			'P' AS 'MSH/MSH.11',
			'2.3' AS 'MSH/MSH.12',
			--PID SEGMENT
			'1' AS 'PID/PID.1',
			acc.mri as 'PID/PID.2',
			--acc.account as 'PID/PID.2',
			dbo.GetNamePart(acc.pat_name,'LAST') as 'PID/PID.5/PID.5.1',
			dbo.GetNamePart(acc.pat_name,'FIRST') as 'PID/PID.5/PID.5.2',
			dbo.GetNamePart(acc.pat_name,'MIDDLE') as 'PID/PID.5/PID.5.3',
			CONVERT(VARCHAR(10), pat.dob_yyyy, 112) as 'PID/PID.7',
			pat.sex as 'PID/PID.8',
			pat.pat_addr1 as 'PID/PID.11/PID.11.1',
			pat.pat_addr2 as 'PID/PID.11/PID.11.2',
			SUBSTRING(pat.city_st_zip,0,charindex(',',pat.city_st_zip)) as 'PID/PID.11/PID.11.3',
			SUBSTRING(pat.city_st_zip,charindex(',',pat.city_st_zip)+2,2) as 'PID/PID.11/PID.11.4',
			dbo.GetNamePart(pat.city_st_zip,'MIDDLE') as 'PID/PID.11/PID.11.5',
			acc.account as 'PID/PID.18',
			pat.ssn as 'PID/PID.19',
			--PV1 segment
			'1' as 'PV1/PV1.1',
			'P' as 'PV1/PV1.2',
			phy.billing_npi AS 'PV1/PV1.7/PV1.7.1',
			phy.last_name AS 'PV1/PV1.7/PV1.7.2',
			phy.first_name AS 'PV1/PV1.7/PV1.7.3',
			phy.mid_init AS 'PV1/PV1.7/PV1.7.4/PV1.7.4.1',
			phy.upin as 'PV1/PV1.7/PV1.7.4/PV1.7.4.2',
			'UPIN' as 'PV1/PV1.7/PV1.7.12',
			phy.billing_npi AS 'PV1/PV1.8/PV1.8.1',
			phy.last_name AS 'PV1/PV1.8/PV1.8.2',
			phy.first_name AS 'PV1/PV1.8/PV1.8.3',
			phy.mid_init AS 'PV1/PV1.8/PV1.8.4',
			
			--IN1 SEGMENT (Primary)
			CASE ia.ins_a_b_c
				WHEN 'A' THEN '1'
				ELSE NULL END as 'IN1/IN1.1',
			ia.ins_code as 'IN1/IN1.2',
			--'' as 'IN1/IN1.3',
			ia.plan_nme as 'IN1/IN1.4',
			REPLACE(ia.plan_addr1,CHAR(0),'') as 'IN1/IN1.5/IN1.5.1',
			ia.plan_addr2 as 'IN1/IN1.5/IN1.5.2',
			substring(ia.p_city_st,0,charindex(',',ia.p_city_st)) as 'IN1/IN1.5/IN1.5.3',
			substring(ia.p_city_st,charindex(',',ia.p_city_st)+2,2) as 'IN1/IN1.5/IN1.5.4',
			dbo.GetNamePart(ia.p_city_st,'MIDDLE') as 'IN1/IN1.5/IN1.5.5', --use the GetNamePart function to get the zip code
			--'' as 'IN1/IN1.7', --phone
			REPLACE(ia.grp_num,CHAR(0),'') as 'IN1/IN1.8', -- GROUP NUMBER
			ia.grp_nme as 'IN1/IN1.9', --GROUP NAME
			--'' as 'IN1/IN1.12', --PLAN EFFECTIVE DATE
			--'' AS 'IN1/IN1.13', --PLAN TERMINATION DATE
			--'' AS 'IN1/IN1.15', -- PLAN TYPE
			dbo.GetNamePart(ia.holder_nme,'LAST') as 'IN1/IN1.16/IN1.16.1',
			dbo.GetNamePart(ia.holder_nme,'FIRST') as 'IN1/IN1.16/IN1.16.2',
			dbo.GetNamePart(ia.holder_nme,'MIDDLE') as 'IN1/IN1.16/IN1.16.3',
			dbo.GetNamePart(ia.holder_nme,'SUFFIX') as 'IN1/IN1.16/IN1.16.4',
			ia.relation as 'IN1/IN1.17',
			CONVERT(VARCHAR(10), ia.holder_dob, 101) AS 'IN1/IN1.18', --INSUREDS DATE OF BIRTH
			REPLACE(ia.holder_addr,CHAR(0),'') AS 'IN1/IN1.19/IN1.19.1', --INSUREDS ADDRESS
			SUBSTRING(ia.holder_addr,0,CHARINDEX(',',ia.holder_addr)) as 'IN1/IN1.19/IN1.19.3',
			SUBSTRING(ia.holder_addr,CHARINDEX(',',ia.holder_addr)+2,2) as 'IN1/IN1.19/IN1.19.4',
			dbo.GetNamePart(ia.holder_addr,'MIDDLE') as 'IN1/IN1.19/IN1.19.5',
			CASE ia.ins_a_b_c
				WHEN 'A' THEN '1'
				ELSE NULL END as 'IN1/IN1.22',--COORDINATION OF BENEFITS PRIORITY
			ia.policy_num as 'IN1/IN1.36',
			ia.holder_sex AS 'IN1/IN1.43', --INSUREDS SEX
			SUBSTRING(ia.e_city_st,0,CHARINDEX(',',ia.e_city_st)) as 'IN1/IN1.44/IN1.44.3',
			SUBSTRING(ia.e_city_st,CHARINDEX(',',ia.e_city_st)+2,2) as 'IN1/IN1.44/IN1.44.4',
			dbo.GetNamePart(ia.e_city_st,'MIDDLE') as 'IN1/IN1.44/IN1.44.5',
			ia.cert_ssn AS 'IN2/IN2.2', -- INSURED SS NO
			ia.employer AS 'IN2/IN2.3/IN2.3.2', -- INSUREDS EMPLOYER CODE AND NAME
			CASE ia.ins_a_b_c
				WHEN 'A' THEN 'I'
				ELSE NULL END AS 'IN2/IN2.5',

			--IN1 SEGMENT (Secondary)
			CASE ib.ins_a_b_c
				WHEN 'B' THEN'2' 
				ELSE NULL END as 'IN1/IN1.1',
			ib.ins_code as 'IN1/IN1.2',
			--'' as 'IN1/IN1.3',
			ib.plan_nme as 'IN1/IN1.4',
			REPLACE(ib.plan_addr1,CHAR(0),'') as 'IN1/IN1.5/IN1.5.1',
			ib.plan_addr2 as 'IN1/IN1.5/IN1.5.2',
			substring(ib.p_city_st,0,charindex(',',ib.p_city_st)) as 'IN1/IN1.5/IN1.5.3',
			substring(ib.p_city_st,charindex(',',ib.p_city_st)+2,2) as 'IN1/IN1.5/IN1.5.4',
			dbo.GetNamePart(ib.p_city_st,'MIDDLE') as 'IN1/IN1.5/IN1.5.5', --use the GetNamePart function to get the zip code
			--'' as 'IN1/IN1.7', --phone
			REPLACE(ib.grp_num,CHAR(0),'') as 'IN1/IN1.8', -- GROUP NUMBER
			ib.grp_nme as 'IN1/IN1.9', --GROUP NAME
			--'' as 'IN1/IN1.12', --PLAN EFFECTIVE DATE
			--'' AS 'IN1/IN1.13', --PLAN TERMINATION DATE
			--'' AS 'IN1/IN1.15', -- PLAN TYPE
			dbo.GetNamePart(ib.holder_nme,'LAST') as 'IN1/IN1.16/IN1.16.1',
			dbo.GetNamePart(ib.holder_nme,'FIRST') as 'IN1/IN1.16/IN1.16.2',
			dbo.GetNamePart(ib.holder_nme,'MIDDLE') as 'IN1/IN1.16/IN1.16.3',
			dbo.GetNamePart(ib.holder_nme,'SUFFIX') as 'IN1/IN1.16/IN1.16.4',
			ib.relation as 'IN1/IN1.17',
			CONVERT(VARCHAR(10), ib.holder_dob, 101) AS 'IN1/IN1.18', --INSUREDS DATE OF BIRTH
			REPLACE(ib.holder_addr,CHAR(0),'') AS 'IN1/IN1.19/IN1.19.1', --INSUREDS ADDRESS
			SUBSTRING(ib.holder_addr,0,CHARINDEX(',',ib.holder_addr)) as 'IN1/IN1.19/IN1.19.3',
			SUBSTRING(ib.holder_addr,CHARINDEX(',',ib.holder_addr)+2,2) as 'IN1/IN1.19/IN1.19.4',
			dbo.GetNamePart(ib.holder_addr,'MIDDLE') as 'IN1/IN1.19/IN1.19.5',
			CASE ib.ins_a_b_c
				WHEN 'B' THEN '2'
				ELSE NULL END as 'IN1/IN1.22',--COORDINATION OF BENEFITS PRIORITY
			ib.policy_num as 'IN1/IN1.36',
			ib.holder_sex AS 'IN1/IN1.43', --INSUREDS SEX
			SUBSTRING(ib.e_city_st,0,CHARINDEX(',',ib.e_city_st)) as 'IN1/IN1.44/IN1.44.3',
			SUBSTRING(ib.e_city_st,CHARINDEX(',',ib.e_city_st)+2,2) as 'IN1/IN1.44/IN1.44.4',
			dbo.GetNamePart(ib.e_city_st,'MIDDLE') as 'IN1/IN1.44/IN1.44.5',
			ib.cert_ssn AS 'IN2/IN2.2', -- INSURED SS NO
			ib.employer AS 'IN2/IN2.3/IN2.3.2', -- INSUREDS EMPLOYER CODE AND NAME
			CASE ib.ins_a_b_c
				WHEN 'B' THEN 'I'
				ELSE NULL END AS 'IN2/IN2.5',

			--IN1 SEGMENT (Tertiary)
			CASE ic.ins_a_b_c
				WHEN 'C' THEN '3'
				ELSE NULL END as 'IN1/IN1.1',
			ic.ins_code as 'IN1/IN1.2',
			--'' as 'IN1/IN1.3',
			ic.plan_nme as 'IN1/IN1.4',
			REPLACE(ic.plan_addr1,CHAR(0),'') as 'IN1/IN1.5/IN1.5.1',
			ic.plan_addr2 as 'IN1/IN1.5/IN1.5.2',
			substring(ic.p_city_st,0,charindex(',',ic.p_city_st)) as 'IN1/IN1.5/IN1.5.3',
			substring(ic.p_city_st,charindex(',',ic.p_city_st)+2,2) as 'IN1/IN1.5/IN1.5.4',
			dbo.GetNamePart(ic.p_city_st,'MIDDLE') as 'IN1/IN1.5/IN1.5.5', --use the GetNamePart function to get the zip code
			--'' as 'IN1/IN1.7', --phone
			REPLACE(ic.grp_num,CHAR(0),'') as 'IN1/IN1.8', -- GROUP NUMBER
			ic.grp_nme as 'IN1/IN1.9', --GROUP NAME
			--'' as 'IN1/IN1.12', --PLAN EFFECTIVE DATE
			--'' AS 'IN1/IN1.13', --PLAN TERMINATION DATE
			--'' AS 'IN1/IN1.15', -- PLAN TYPE
			dbo.GetNamePart(ic.holder_nme,'LAST') as 'IN1/IN1.16/IN1.16.1',
			dbo.GetNamePart(ic.holder_nme,'FIRST') as 'IN1/IN1.16/IN1.16.2',
			dbo.GetNamePart(ic.holder_nme,'MIDDLE') as 'IN1/IN1.16/IN1.16.3',
			dbo.GetNamePart(ic.holder_nme,'SUFFIX') as 'IN1/IN1.16/IN1.16.4',
			ic.relation as 'IN1/IN1.17',
			CONVERT(VARCHAR(10), ic.holder_dob, 101) AS 'IN1/IN1.18', --INSUREDS DATE OF BIRTH
			REPLACE(ic.holder_addr,CHAR(0),'') AS 'IN1/IN1.19/IN1.19.1', --INSUREDS ADDRESS
			SUBSTRING(ic.holder_addr,0,CHARINDEX(',',ic.holder_addr)) as 'IN1/IN1.19/IN1.19.3',
			SUBSTRING(ic.holder_addr,CHARINDEX(',',ic.holder_addr)+2,2) as 'IN1/IN1.19/IN1.19.4',
			dbo.GetNamePart(ic.holder_addr,'MIDDLE') as 'IN1/IN1.19/IN1.19.5',
			CASE ic.ins_a_b_c
				WHEN 'C' THEN '3'
				ELSE NULL END as 'IN1/IN1.22',--COORDINATION OF BENEFITS PRIORITY
			ic.policy_num as 'IN1/IN1.36',
			ic.holder_sex AS 'IN1/IN1.43', --INSUREDS SEX
			SUBSTRING(ic.e_city_st,0,CHARINDEX(',',ic.e_city_st)) as 'IN1/IN1.44/IN1.44.3',
			SUBSTRING(ic.e_city_st,CHARINDEX(',',ic.e_city_st)+2,2) as 'IN1/IN1.44/IN1.44.4',
			dbo.GetNamePart(ic.e_city_st,'MIDDLE') as 'IN1/IN1.44/IN1.44.5',
			ic.cert_ssn AS 'IN2/IN2.2', -- INSURED SS NO
			ic.employer AS 'IN2/IN2.3/IN2.3.2', -- INSUREDS EMPLOYER CODE AND NAME
			CASE ic.ins_a_b_c
				WHEN 'C' THEN 'I'
				ELSE NULL END AS 'IN2/IN2.5',

				--DG1 SEGMENTS
			CAST((SELECT
				CASE WHEN COALESCE(icd.link,'') <> '' THEN icd.link ELSE NULL END AS 'DG1.1',
				icd.icd9 as 'DG1.3/DG1.3.1',
				icd.icd9_desc as 'DG1.3/DG1.3.2',
				CASE WHEN COALESCE(icd.link,'') <> '' THEN 'I9' ELSE NULL END as 'DG1.3/DG1.3.3',
				CASE WHEN COALESCE(icd.link,'') <> '' THEN 'F' ELSE NULL END AS 'DG1.6'
			FROM dbo.GetPatIcd9(acc.account) icd
			FOR XML PATH('DG1')) AS xml
			),

			--GT1 SEGMENT
			'1' as 'GT1/GT1.1',
			dbo.GetNamePart(pat.guarantor,'LAST') as 'GT1/GT1.3/GT1.3.1',
			dbo.GetNamePart(pat.guarantor,'FIRST') as 'GT1/GT1.3/GT1.3.2',
			dbo.GetNamePart(pat.guarantor,'MIDDLE') as 'GT1/GT1.3/GT1.3.3',
			pat.guar_addr as 'GT1/GT1.5/GT1.5.1',
			SUBSTRING(pat.g_city_st,0,charindex(',',pat.g_city_st)) as 'GT1/GT1.5/GT1.5.3',
			SUBSTRING(pat.g_city_st,charindex(',',pat.g_city_st)+2,2) as 'GT1/GT1.5/GT1.5.4',
			dbo.GetNamePart(pat.g_city_st,'MIDDLE') as 'GT1/GT1.5/GT1.5.5',
			pat.guar_phone as 'GT1/GT1.6'
		FROM acc 
		LEFT OUTER JOIN pat on acc.account = pat.account
		LEFT OUTER JOIN phy on pat.phy_id = phy.tnh_num
		LEFT OUTER JOIN ins ia on acc.account = ia.account and ia.ins_a_b_c = 'A'
		LEFT OUTER JOIN ins ib on acc.account = ib.account and ib.ins_a_b_c = 'B'
		LEFT OUTER JOIN ins ic on acc.account = ic.account and ic.ins_a_b_c = 'C'
		WHERE acc.account = a.account
		FOR XML PATH('HL7Message')) AS rowxml
	FROM acc a
	where a.account IN (SELECT * FROM dbo.ufn_jpg_activity_idx(GETDATE()-1)) --RUN FOR ACTIVITY >= YESTERDAY
END
