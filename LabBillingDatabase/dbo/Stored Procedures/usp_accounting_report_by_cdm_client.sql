-- =============================================
-- Author:		Bradley Powers
-- Create date: 8/4/2014
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_report_by_cdm_client] 
	-- Add the parameters for the stored procedure here
	@beginDate datetime, 
	@endDate DATETIME,
	@cl_mnem VARCHAR(15) = NULL,
	@cl_type INT = NULL 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	/*
	Accounting Report by CDM
	*/

--	DECLARE @beginDate DATETIME;
--	DECLARE @endDate DATETIME;
--
--	SET @beginDate = '08/01/2013';
--	SET @endDate = '08/31/2013';

	with cteCDM (cdm, descript, mtype, ctype, mnem, mprice, cprice, zprice)
	as
	(
		select cdm.cdm, cdm.descript, cdm.mtype, cdm.ctype, cdm.mnem,
			SUM(cpt4.mprice) AS mprice, SUM(cpt4.cprice) AS cprice, SUM(cpt4.zprice) as zprice
		from (select * from cdm where deleted = 0) cdm
		join (select * from cpt4 where deleted = 0) cpt4 on cdm.cdm = cpt4.cdm
		GROUP BY cdm.cdm, cdm.descript, cdm.mtype, cdm.ctype, cdm.mnem
	),
	cteChrg ([cl_mnem], [CDM], [Description], [Qty],[3rd Party Net],[Client Net], [Other Billed], [Total])
	as
	(
	SELECT
		acc.cl_mnem 
		, chrg.cdm as 'CDM'
		, cdm.descript as 'Description'
		, qty as 'Qty'
		, CASE WHEN fin_type = 'M' THEN calc_amt ELSE 0.00 END as '3rd Party Net'
		, CASE WHEN fin_type = 'C' THEN calc_amt ELSE 0.00 END as 'Client Net'
		, CASE WHEN fin_type NOT IN ('M','C') THEN calc_amt ELSE 0.00 END as 'Other Billed'
		, calc_amt as 'Total'
	FROM chrg
	JOIN acc ON acc.account = chrg.account
	LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm
	LEFT OUTER JOIN client ON acc.cl_mnem = client.cli_mnem
--	LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt 
--		on chrg.chrg_num = amt.chrg_num
	WHERE chrg.mod_date >= @beginDate and chrg.mod_date < @endDate+1
		and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP')
		AND 
			(
				(@cl_mnem IS NOT NULL AND cl_mnem = @cl_mnem)
				OR
				(@cl_type IS NOT NULL AND client.type = @cl_type)
			)
	)

	SELECT
		cl_mnem AS 'Client Mnem'
		, client.cli_nme AS 'Client Name'
		, LEFT(cdm,3) as 'Department' 
		, RIGHT(cdm,4) as 'Item'
		, [Description]
		, SUM([Qty]) AS 'Qty'
		, CONVERT(MONEY,SUM([3rd Party Net]),1) as '3rd Party Net'
		, CONVERT(MONEY,SUM([Client Net]),1) as 'Client Net'
		, CONVERT(MONEY,SUM([Other Billed]),1) as 'Other Billed'
		, CONVERT(MONEY,SUM([Total]),1) as 'Total'
	FROM cteChrg LEFT OUTER JOIN dbo.client ON cteChrg.cl_mnem = client.cli_mnem
	GROUP BY cteChrg.cl_mnem, client.cli_nme, cdm, [Description]
	ORDER BY client.cli_nme, cdm
END
