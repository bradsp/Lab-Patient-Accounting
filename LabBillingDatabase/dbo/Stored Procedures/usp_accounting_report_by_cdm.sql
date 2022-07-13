-- =============================================
-- Author:		Bradley Powers
-- Create date: 7/16/2013
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_report_by_cdm] 
	-- Add the parameters for the stored procedure here
	@beginDate datetime, 
	@endDate datetime 
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
	cteChrg ([CDM], [Description], [Qty],[3rd Party Net],[Client Net], [Other Billed], [Total])
	as
	(
	SELECT chrg.cdm as 'CDM'
		, cdm.descript as 'Description'
		, qty as 'Qty'
		, CASE WHEN fin_type = 'M' THEN qty*amt.amount ELSE 0.00 END as '3rd Party Net'
		, CASE WHEN fin_type = 'C' THEN qty*amt.amount ELSE 0.00 END as 'Client Net'
		, CASE WHEN fin_type NOT IN ('M','C') THEN qty*amt.amount ELSE 0.00 END as 'Other Billed'
		, qty * amt.amount as 'Total'
	FROM chrg
	LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm
	LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt 
		on chrg.chrg_num = amt.chrg_num
	WHERE chrg.mod_date >= @beginDate and chrg.mod_date < @endDate+1
		and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP')
	)

	SELECT LEFT(cdm,3) as 'Department' 
		, RIGHT(cdm,4) as 'Item'
		, [Description]
		, SUM([Qty]) AS 'Qty'
		, CONVERT(MONEY,SUM([3rd Party Net]),1) as '3rd Party Net'
		, CONVERT(MONEY,SUM([Client Net]),1) as 'Client Net'
		, CONVERT(MONEY,SUM([Other Billed]),1) as 'Other Billed'
		, CONVERT(MONEY,SUM([Total]),1) as 'Total'
	FROM cteChrg
	GROUP BY cdm, [Description]
	ORDER BY cdm
END
