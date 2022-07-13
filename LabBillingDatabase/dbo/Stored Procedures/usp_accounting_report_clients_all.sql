-- =============================================
-- Author:		David Ripoff of Bradley Powers
-- Create date: 10/08/2014
-- Description:	Compiles 3rd Party, Client, Quest and JPG charges for an individual client
-- =============================================
CREATE PROCEDURE [dbo].[usp_accounting_report_clients_all] 
	-- Add the parameters for the stored procedure here
--DECLARE
	@startDate datetime, 
	@endDate DATETIME--,
	--@cl_mnem varchar(15) 
	--SET @cl_mnem = 'goo'
AS
BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--SET @startDate ='07/01/2013 00:00:00'--106102
--SET @endDate =  '06/30/2014 23:59:59'
--select DATEADD(MONTH,12,@endDate)
;with cteCDM (account,cdm, descript, mtype, ctype, mnem, mprice, cprice, zprice)
as
(
	select '',cdm.cdm, cdm.descript, cdm.mtype, cdm.ctype, cdm.mnem,
		SUM(cpt4.mprice) AS mprice, SUM(cpt4.cprice) AS cprice, SUM(cpt4.zprice) as zprice
	from (select * from cdm where deleted = 0) cdm
	join (select * from cpt4 where deleted = 0) cpt4 on cdm.cdm = cpt4.cdm
	GROUP BY cdm.cdm, cdm.descript, cdm.mtype, cdm.ctype, cdm.mnem
)
, cteChrg ([ACCOUNT],[Department], [Item], [Description], [Qty], [FinType],
		[Client],[Client Name],
		[3rd Party Net],
		[CLIENT Net],
		[QUEST Net],
		[JPG Net],
		[Other Billed], [Total Charges])
as
(
	SELECT chrg.account,
		  LEFT(chrg.cdm,3) as 'Department' 
		, RIGHT(chrg.cdm,4) as 'Item'
		, cdm.descript as 'Description'
		, qty as 'Qty'
		, fin_type
		, acc.cl_mnem AS 'Client'
		, client.cli_nme as 'Client Name'
		, CASE WHEN fin_type = 'M' THEN qty*amount ELSE 0.00 END as '3rd Party Net'
		,  CASE WHEN fin_type = 'C' AND chrg.account LIKE '[C,D]%' 
			THEN qty*amount 
			ELSE 0.00 
			END as 'CLIENT Net'
		, CASE WHEN fin_type = 'C' AND chrg.account LIKE 'Q' 
			THEN qty*amount 
			ELSE 0.00 
			END as 'QUEST Net'
		, CASE WHEN fin_type = 'C' AND chrg.account LIKE 'J' 
			THEN qty*amount 
			ELSE 0.00 
			END as 'JPG Net'
		, CASE WHEN fin_type NOT IN ('M','C') 
			THEN qty*amount 
			ELSE 0.00 
			END as 'Other Billed'
		, qty * amt.amount as 'Total'
	FROM chrg
	LEFT OUTER JOIN acc on chrg.account = acc.account
	LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm
	LEFT OUTER JOIN client on acc.cl_mnem = client.cli_mnem
	LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt 
		on chrg.chrg_num = amt.chrg_num
	LEFT OUTER JOIN dbo.acc_location ON dbo.acc_location.account = dbo.chrg.account
	WHERE chrg.mod_date BETWEEN @startDate and @endDate
		and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP')
		--and cl_mnem = ISNULL(@cl_mnem,[cl_mnem])
	AND client.cli_mnem NOT IN ('QUESTR', 'JPG')

) --SELECT * FROM cteChrg

,cteChrgQuest ([ACCOUNT],[Department], [Item], [Description], [Qty], [FinType],
		[Client],[Client Name],
		[3rd Party Net],
		[CLIENT Net],
		[QUEST Net],
		[JPG Net],
		[Other Billed], [Total Charges])
as
(
	SELECT REPLACE(chrg.account,'Q','C') AS [account],
		  LEFT(chrg.cdm,3) as 'Department' 
		, RIGHT(chrg.cdm,4) as 'Item'
		, cdm.descript as 'Description'
		, qty as 'Qty'
		, fin_type
		, acc.cl_mnem AS 'Client'
		, client.cli_nme as 'Client Name'
		, CASE WHEN fin_type = 'M' THEN qty*amount ELSE 0.00 END as '3rd Party Net'
		,  CASE WHEN fin_type = 'C' AND chrg.account LIKE '[C,D]%' 
			THEN qty*amount 
			ELSE 0.00 
			END as 'CLIENT Net'
		, CASE WHEN fin_type = 'C' AND chrg.account LIKE 'Q' 
				THEN qty*amount 
				ELSE 0.00 
				END as 'QUEST Net'
		, CASE WHEN fin_type = 'C' AND chrg.account LIKE 'J' 
				THEN qty*amount 
				ELSE 0.00 
				END as 'JPG Net'
		, CASE WHEN fin_type NOT IN ('M','C') THEN qty*amount ELSE 0.00 END as 'Other Billed'
		, qty * amt.amount as 'Total'
	FROM chrg
	LEFT OUTER JOIN acc on REPLACE(chrg.account,'Q','C') = acc.account
	LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm
	LEFT OUTER JOIN client on acc.cl_mnem = client.cli_mnem
	LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt 
		on chrg.chrg_num = amt.chrg_num
	LEFT OUTER JOIN dbo.acc_location ON dbo.acc_location.account = REPLACE(chrg.account,'Q','C')
	WHERE chrg.mod_date BETWEEN @startDate and @endDate
		and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP')
		--and cl_mnem = ISNULL(@cl_mnem,[cl_mnem])
	AND chrg.account LIKE 'Q%'

) --SELECT * FROM cteChrgQuest ORDER BY account

,cteChrgJPG ([ACCOUNT],[Department], [Item], [Description], [Qty], [FinType],
		[Client],[Client Name],
		[3rd Party Net],
		[CLIENT Net],
		[QUEST Net],
		[JPG Net],
		[Other Billed], [Total Charges])
as
(
	SELECT REPLACE(chrg.account,'J','C') AS [account],
		  LEFT(chrg.cdm,3) as 'Department' 
		, RIGHT(chrg.cdm,4) as 'Item'
		, cdm.descript as 'Description'
		, qty as 'Qty'
		, fin_type
		, acc.cl_mnem AS 'Client'
		, client.cli_nme as 'Client Name'
		, CASE WHEN fin_type = 'M' THEN qty*amount ELSE 0.00 END as '3rd Party Net'
		,  CASE WHEN fin_type = 'C' AND chrg.account LIKE '[C,D]%' 
			THEN qty*amount 
			ELSE 0.00 
			END as 'CLIENT Net'
		, CASE WHEN fin_type = 'C' AND chrg.account LIKE 'Q' 
			THEN qty*amount 
			ELSE 0.00 
			END as 'QUEST Net'
		, CASE WHEN fin_type = 'C' AND chrg.account LIKE 'J' 
			THEN qty*amount 
			ELSE 0.00 
			END as 'JPG Net'
		, CASE WHEN fin_type NOT IN ('M','C')  
			THEN qty*amount 
			ELSE 0.00 
			END as 'Other Billed'
		, qty * amt.amount as 'Total'
	FROM chrg
	LEFT OUTER JOIN acc on REPLACE(chrg.account,'J','C') = acc.account
	LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm
	LEFT OUTER JOIN client on acc.cl_mnem = client.cli_mnem
	LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt 
		on chrg.chrg_num = amt.chrg_num
	LEFT OUTER JOIN dbo.acc_location ON dbo.acc_location.account = REPLACE(chrg.account,'Q','C')
	--WHERE chrg.mod_date >= @startDate and chrg.mod_date < @endDate+1
	WHERE chrg.mod_date BETWEEN @startDate and @endDate
		and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP')
		--and cl_mnem = ISNULL(@cl_mnem,[cl_mnem])
	
	AND chrg.account LIKE 'J%'
) --SELECT * FROM cteChrgJPG ORDER BY account

, cteTotals
AS
(
	select cteChrg.ACCOUNT AS [ACCOUNT] ,
			SUM(cteChrg.Qty) AS [QTY] ,
			cteChrg.Client AS [Client] ,
			cteChrg.[Client Name] ,
			SUM(cteChrg.[3rd Party Net]) AS [3rd Party Net] ,
			SUM(cteChrg.[CLIENT Net]) AS [CLIENT Net],
			SUM(cteChrg.[QUEST Net]) AS [QUEST Net],
			SUM(cteChrg.[JPG Net]) AS [JPG Net],
			SUM(cteChrg.[Other Billed]) AS [Other Billed],
			SUM(cteChrg.[Total Charges]) AS [Total Charges] 
			,0 AS [INCLUDES QUEST]
			,0 AS [INCLUDES JPG]
			from cteChrg
		GROUP BY cteChrg.Client, cteChrg.[Client Name],cteChrg.FinType
			, cteChrg.Account
		
		UNION ALL
		select cteChrgQuest.ACCOUNT AS [ACCOUNT] ,
			SUM(cteChrgQuest.Qty) AS [QTY] ,
			cteChrgQuest.client AS [Client] ,
			cteChrgQuest.[Client Name] ,
			SUM(cteChrgQuest.[3rd Party Net]) AS [3rd Party Net] ,
			SUM(cteChrgQuest.[CLIENT Net]) AS [CLIENT Net],
			SUM(cteChrgQuest.[Total Charges]) AS [Quest Net],
			SUM(cteChrgQuest.[JPG Net]) AS [JPG Net],
			SUM(cteChrgQuest.[Other Billed]) AS [Other Billed],
			SUM(cteChrgQuest.[Total Charges]) AS [Total Charges] 
			,1 AS [INCLUDES QUEST]
			,0 AS [INCLUDES JPG]
		from cteChrgQuest
		GROUP BY cteChrgQuest.Client, cteChrgQuest.[Client Name],cteChrgQuest.FinType
			, cteChrgQuest.Account
		
		UNION ALL
		select cteChrgJPG.ACCOUNT AS [ACCOUNT] ,
			SUM(cteChrgJPG.Qty) AS [QTY] ,
			cteChrgJPG.client AS [Client] ,
			cteChrgJPG.[Client Name] ,
			SUM(cteChrgJPG.[3rd Party Net]) AS [3rd Party Net] , -- because quest is a client payment this is not valid use below
			SUM(cteChrgJPG.[CLIENT Net]) AS [CLIENT Net],
			SUM(cteChrgJPG.[QUEST Net]) AS [QUEST Net],
			SUM(cteChrgJPG.[Total Charges]) AS [JPG Net],
			SUM(cteChrgJPG.[Other Billed]) AS [Other Billed],
			SUM(cteChrgJPG.[Total Charges]) AS [Total Charges] 
			,0 AS [INCLUDES QUEST]
			,1 AS [INCLUDES JPG]
			
			from cteChrgJPG
			
		GROUP BY cteChrgJPG.Client, cteChrgJPG.[Client Name],cteChrgJPG.FinType
		, cteChrgJPG.Account
			
) --SELECT * FROM cteTotals

, cteGrandTotals
AS
(
SELECT --cteTotals.ACCOUNT ,
		cteTotals.Client ,
		cteTotals.[Client Name] ,
		STR(SUM(cteTotals.QTY),10,0) AS [QTY] ,
		STR(SUM(cteTotals.[3rd Party Net]),10,2) AS [3rd Party Net] ,
		STR(SUM(cteTotals.[CLIENT Net]),10,2) AS [CLIENT Net],
		STR(SUM(cteTotals.[Quest Net]),10,2) AS [Quest Net] ,
		STR(SUM(cteTotals.[JPG Net]),10,2) AS [JPG Net] ,
		STR(SUM(cteTotals.[Other Billed]),10,2) AS [OtherBilled] ,
		STR(SUM(cteTotals.[Total Charges]),10,2) AS [Total Charges] , 
		STR(CASE WHEN [INCLUDES QUEST] = 0-- OR [INCLUDES JPG] = 0
			THEN SUM(ISNULL(gpbdr.colAmtPaid,0.00)) 
			ELSE 0.00
			END,10,2) AS [3rd Party Total Paid],
		CASE WHEN [Includes Quest] = 0 --OR [INCLUDES JPG] = 0
			THEN STR(ISNULL((SELECT colAmtPaid from dbo.GetPaymentsByClient(cteTotals.Client,@startDate,@endDate)),0.00),10,2)
			ELSE STR(ISNULL(SUM(cteTotals.[Quest Net]),0.00),10,2) 
			END	
		 AS [Client Total Paid]
		--, [Includes Quest]
FROM cteTotals
OUTER APPLY dbo.GetPaymentsByAccount(cteTotals.ACCOUNT) gpbdr

 
GROUP BY cteTotals.Client ,
		cteTotals.[Client Name] 
	--, cteTotals.ACCOUNT
	, [includes quest]
	,[INCLUDES JPG]
--ORDER BY cteTotals.ACCOUNT

)--SELECT * FROM cteToTals
SELECT 	Client,	[Client Name]
	,SUM(CONVERT(NUMERIC(18,0),QTY)) AS [QTY]
	,SUM(CONVERT(NUMERIC(18,2),[3rd Party Net])) AS [3rd Party Net]
	,sum(CONVERT(NUMERIC(18,2),[CLIENT Net])) AS [CLIENT Net]
	,sum(CONVERT(NUMERIC(18,2),[Quest Net])) AS [Quest Net]
	,sum(CONVERT(NUMERIC(18,2),[JPG Net])) AS [JPG Net]
	,SUM(CONVERT(NUMERIC(18,2),[OtherBilled])) AS [OtherBilled]
	,SUM(CONVERT(NUMERIC(18,2),[Total Charges])) AS [Total Charges]
	,SUM(CONVERT(NUMERIC(18,2),[3rd Party Total Paid])) AS [3rd Party Total Paid]
	,SUM(CONVERT(NUMERIC(18,2),[Client Net])
		+CONVERT(NUMERIC(18,2),[Quest Net])
		+CONVERT(NUMERIC(18,2),[JPG Net])
		) AS [Client Total Paid]
FROM cteGrandTotals
--WHERE Client = @cl_mnem


GROUP BY Client,	[Client Name]

END
