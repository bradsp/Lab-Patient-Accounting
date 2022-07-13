-- =============================================
-- Author:		Bradley Powers
-- Create date: 7/17/2013
-- Description:	Compiles accounting report data for reporting services
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_report_data] 
	-- Add the parameters for the stored procedure here
	@beginDate datetime, 
	@endDate datetime,
	@cl_mnem varchar(15) = NULL,
	@type INT = -1
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	with cteCDM (cdm, descript, mtype, ctype, mnem, mprice, cprice, zprice)
	as
	(
		select cdm.cdm, cdm.descript, cdm.mtype, cdm.ctype, cdm.mnem,
			SUM(cpt4.mprice) AS mprice, SUM(cpt4.cprice) AS cprice, SUM(cpt4.zprice) as zprice
		from (select * from cdm where deleted = 0) cdm
		join (select * from cpt4 where deleted = 0) cpt4 on cdm.cdm = cpt4.cdm
		GROUP BY cdm.cdm, cdm.descript, cdm.mtype, cdm.ctype, cdm.mnem
	),
	cteChrg ([Department], [Item], [Description], [Qty], [FinType],
		[Client],[Client Name],
		[3rd Party Net],
		[Client Net],
		[Other Billed], [Total])
	as
	(
	SELECT
		  LEFT(chrg.cdm,3) as 'Department' 
		, RIGHT(chrg.cdm,4) as 'Item'
		, cdm.descript as 'Description'
		, qty as 'Qty'
		, fin_type
		, acc.cl_mnem AS 'Client'
		, client.cli_nme as 'Client Name'
		, CASE WHEN fin_type = 'M' THEN calc_amt ELSE 0.00 END as '3rd Party Net'
		, CASE WHEN fin_type = 'C' THEN calc_amt ELSE 0.00 END as 'Client Net'
		, CASE WHEN fin_type NOT IN ('M','C') THEN calc_amt ELSE 0.00 END as 'Other Billed'
		, calc_amt as 'Total'
	FROM chrg
	LEFT OUTER JOIN acc on chrg.account = acc.account
	LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm
	LEFT OUTER JOIN client on acc.cl_mnem = client.cli_mnem
--	LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt 
--		on chrg.chrg_num = amt.chrg_num
	WHERE chrg.mod_date >= @beginDate and chrg.mod_date < @endDate+1
		and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP')
		and (
			(acc.cl_mnem = @cl_mnem OR @cl_mnem IS NULL)
			AND
			([client].[type] = @type OR @type = -1)
			)
	)

	select * from cteChrg
END
