-- =============================================
-- Author:		Bradley Powers
-- Create date: 7/23/2013
-- Description:	Charge Report by CPT4
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_report_by_cpt4] 
	-- Add the parameters for the stored procedure here
	@beginDate datetime, 
	@endDate datetime 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/*
	Accounting Report by CPT4
	*/

--	DECLARE @beginDate DATETIME;
--	DECLARE @endDate DATETIME;
--
--	SET @beginDate = '06/01/2013';
--	SET @endDate = '06/30/2013';

	with cteCDM
	as
	(
		SELECT c1.cpt4, COALESCE(cpt4_ama.short_desc,c2.descript) AS 'Description'
		FROM (SELECT DISTINCT cpt4 FROM cpt4) c1
			LEFT OUTER JOIN dbo.cpt4_ama ON cpt4_ama.cpt4 = c1.cpt4
			LEFT OUTER JOIN (SELECT cpt4, MIN(descript) AS descript FROM cpt4 GROUP BY cpt4) c2 ON c2.cpt4 = c1.cpt4
		WHERE c1.cpt4 IS NOT NULL
		GROUP BY c1.cpt4, COALESCE(cpt4_ama.short_desc,descript)
		UNION ALL
		SELECT cdm AS cpt4, cdm.descript AS 'Description' FROM cdm

	)
	, cteChrg
	AS
	(
		SELECT 
			CASE
				WHEN amt.cpt4 IS NULL THEN chrg.cdm
				WHEN amt.cpt4 = '' THEN chrg.cdm
				WHEN amt.cpt4 = 'NA' THEN chrg.cdm
				ELSE amt.cpt4 END				                                        
				as 'CPT4'
			, chrg.cdm AS 'CDM'
			--, cdm.Description as 'Description'
			, qty as 'Qty'
			, CASE WHEN fin_type = 'M' THEN qty*amount ELSE 0.00 END as '3rd Party'
			, CASE WHEN fin_type = 'C' THEN qty*amount ELSE 0.00 END as 'Client'
			, CASE WHEN fin_type NOT IN ('M','C') THEN qty*amount ELSE 0.00 END as 'Other'
			, qty * amount as 'Total'
		FROM chrg
		JOIN amt on chrg.chrg_num = amt.chrg_num
		WHERE amt.mod_date >= @beginDate and amt.mod_date < @endDate+1
			and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP')
	)

	SELECT 
		cteChrg.CPT4 as 'CPT4'
		, cdm.Description
		, SUM([Qty]) AS 'Qty'
		, CONVERT(MONEY,SUM([3rd Party]),1) as '3rd Party'
		, CONVERT(MONEY,SUM([Client]),1) as 'Client'
		, CONVERT(MONEY,SUM([Other]),1) as 'Other'
		, CONVERT(MONEY,SUM([Total]),1) as 'Total'
	FROM cteChrg
		LEFT OUTER JOIN cteCDM cdm on cteChrg.CPT4 = cdm.cpt4
	GROUP BY cteChrg.CPT4, cdm.Description
	ORDER BY cteChrg.CPT4

END
