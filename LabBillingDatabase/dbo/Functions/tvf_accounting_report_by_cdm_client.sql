-- =============================================
-- Author:		David
-- Create date: 01/30/2015
-- Description:	returns a table for usp_accounting_report_by_cdm_client
-- to be used for fee schedule work for Cerner
-- =============================================
CREATE FUNCTION tvf_accounting_report_by_cdm_client 
(
	-- Add the parameters for the function here
	@startDate datetime, 
	@endDate DATETIME,
	@cl_mnem VARCHAR(10)
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colClientMnem varchar(10), 
	colClientName varchar(40),
	colCDM	varchar(7),
	colCDMDepartment varchar(3),
	colCDMItem varchar(4),
	colCDMDescription varchar(50),
	colQty int,
	col3rdPartyNet numeric(18,2),
	colClientNet numeric(18,2),
	colOtherBilled numeric(18,2),
	colTotal numeric (18,2)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
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
		, COALESCE(NULLIF(cdm.descript,''),'UNK') as 'Description'
		, qty as 'Qty'
		, CASE WHEN fin_type = 'M' THEN qty*amt.amount ELSE 0.00 END as '3rd Party Net'
		, CASE WHEN fin_type = 'C' THEN qty*amt.amount ELSE 0.00 END as 'Client Net'
		, CASE WHEN fin_type NOT IN ('M','C') THEN qty*amt.amount ELSE 0.00 END as 'Other Billed'
		, qty * amt.amount as 'Total'
	FROM chrg
	JOIN acc ON acc.account = chrg.account
	LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm
	LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt 
		on chrg.chrg_num = amt.chrg_num
	WHERE chrg.mod_date >= @startDate and chrg.mod_date < @endDate+1
		and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP')
		AND cl_mnem = ISNULL(@cl_mnem,[cl_mnem])
	)
	INSERT INTO @Table_Var
			(
				colClientMnem ,
				colClientName ,
				colCDM ,
				colCDMDepartment ,
				colCDMItem ,
				colCDMDescription ,
				colQty ,
				col3rdPartyNet,
				colClientNet ,
				colOtherBilled ,
				colTotal 
			)

	SELECT
		cl_mnem AS [Client Mnem]
		, client.cli_nme AS [Client Name]
		, cdm AS [CDM]
		, LEFT(cdm,3) as [Department]
		, RIGHT(cdm,4) as [Item]
		, [Description]
		, SUM([Qty]) AS [Qty]
		, CONVERT(MONEY,SUM([3rd Party Net]),1) as [3rd Party Net]
		, CONVERT(MONEY,SUM([Client Net]),1) as [Client Net]
		, CONVERT(MONEY,SUM([Other Billed]),1) as [Other Billed]
		, CONVERT(MONEY,SUM([Total]),1) as [Total]
	FROM cteChrg LEFT OUTER JOIN dbo.client ON cteChrg.cl_mnem = client.cli_mnem
	GROUP BY cteChrg.cl_mnem, client.cli_nme, cdm, [Description]
	ORDER BY client.cli_nme, cdm
	
	
	
	
	RETURN 
END
