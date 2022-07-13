-- =============================================
-- Author:		David
-- Create date: 10/16/2014
-- Description:	
-- =============================================
CREATE FUNCTION usf_account_report_client_qty 
(
	-- Add the parameters for the function here
	@startDate DATETIME, 
	@endDate DATETIME,
	@client VARCHAR(10)
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	Client varchar(10), 
	[Client Name] varchar(50),
	[CDM] VARCHAR(7),
	[CDM DESCRIPTION] VARCHAR(50),
	[CLIENT QTY] numeric(18,0),
	[QUEST QTY] numeric(18,0),
	[JPG QTY] numeric(18,0),
	[Other QTY] numeric(18,0),
	[Total QTY] numeric(18,0)	
)
AS
BEGIN
	
	-- Fill the table variable with the rows for your result set
; WITH cteCDM (account,cdm, descript, mtype, ctype, mnem, mprice, cprice, zprice) 
AS
(
	select '',cdm.cdm, cdm.descript, cdm.mtype, cdm.ctype, cdm.mnem
	, SUM(cpt4.mprice) AS mprice
	, SUM(cpt4.cprice) AS cprice
	, SUM(cpt4.zprice) as zprice 
	from (
			select * from cdm where deleted = 0 AND dbo.cdm.orderable = 1
		 ) cdm 
		 join 
		 (
			select * from cpt4 where deleted = 0
		 ) cpt4 on cdm.cdm = cpt4.cdm 
	GROUP BY cdm.cdm, cdm.descript, cdm.mtype, cdm.ctype, cdm.mnem
)

, cteChrg ([ACCOUNT],[Department], [Item], [Description], [Qty], [FinType]
,[Client],[Client Name],[INS QTY],[CLIENT QTY],[QUEST QTY],[JPG QTY],[Other QTY]) 
as 
( 
	SELECT chrg.account,LEFT(chrg.cdm,3) as 'Department' , RIGHT(chrg.cdm,4) as 'Item'
	, cdm.descript as 'Description', qty as 'Qty', fin_type, acc.cl_mnem AS 'Client'
	, client.cli_nme as 'Client Name'
	, CASE WHEN fin_type = 'M' THEN qty ELSE 0 
		END as 'INS QTY' 
	, CASE WHEN fin_type = 'C' AND chrg.account LIKE '[C,D]%' THEN qty ELSE 0 
		END as 'CLIENT QTY'
	, CASE WHEN fin_type = 'C' AND chrg.account LIKE 'Q' THEN qty ELSE 0 
		END as 'QUEST QTY'
	, CASE WHEN fin_type = 'C' AND chrg.account LIKE 'J' THEN qty ELSE 0 
		END as 'JPG QTY'
	, CASE WHEN fin_type NOT IN ('M','C') THEN qty ELSE 0 
		END as 'Other QTY'	
	FROM chrg 
	inner JOIN acc on chrg.account = acc.account
	INNER JOIN acc accJPG ON chrg.account = REPLACE(REPLACE(acc.account,'C','J'),'D','J')
	LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm 
	LEFT OUTER JOIN client on acc.cl_mnem = client.cli_mnem 
	--LEFT OUTER JOIN 		(			select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt on chrg.chrg_num = amt.chrg_num 
	--LEFT OUTER JOIN dbo.acc_location ON dbo.acc_location.account = dbo.chrg.account 
	WHERE chrg.mod_date BETWEEN @startDate and @endDate 
		and chrg.cdm <> 'CBILL' 
		AND chrg.status NOT IN('N/A','CBILL','CAP') 
		and acc.cl_mnem in (ISNULL(@client,acc.[cl_mnem]) ,'jpg','questr')
		--AND client.cli_mnem NOT IN ('QUESTR', 'JPG')
)
INSERT INTO @Table_Var
		(
			Client ,[Client Name] , 
			cdm, [@Table_Var].[CDM DESCRIPTION],
			[CLIENT QTY] ,	[QUEST QTY] , [JPG QTY] ,	[Other QTY],
			[Total QTY] 
		)
SELECT cteChrg.Client ,cteChrg.[Client Name]	
		,CONVERT(VARCHAR(3),[Department])+CONVERT(VARCHAR(4), [Item]) AS [CDM], [Description]
		, SUM(cteChrg.[Client QTY]) AS [Client]	
		, SUM(cteChrg.[Quest QTY]) AS [Quest Qty]
		, SUM(cteChrg.[JPG QTY]) AS [JPG Qty]		
		, SUM(cteChrg.[INS QTY]) AS [INS QTY]		
		, SUM(cteChrg.[INS QTY]) AS [TOTAL QUANTITY] FROM cteChrg
		GROUP BY cteChrg.Client ,cteChrg.[Client Name]	
		,CONVERT(VARCHAR(3),[Department])+CONVERT(VARCHAR(4), [Item]) , [Description]
		WITH ROLLUP

 
--,cteQuest ([ACCOUNT],[Department], [Item], [Description], [Qty], [FinType],[Client],[Client Name],[3rd Party Net],[CLIENT Net],[QUEST Net],[JPG Net],[Other Billed], [Total Charges]) as ( SELECT REPLACE(chrg.account,'Q','C') AS [account],LEFT(chrg.cdm,3) as 'Department' , RIGHT(chrg.cdm,4) as 'Item', cdm.descript as 'Description', qty as 'Qty', fin_type, acc.cl_mnem AS 'Client', client.cli_nme as 'Client Name', CASE WHEN fin_type = 'M' THEN qty*amount ELSE 0.00 END as '3rd Party Net',  CASE WHEN fin_type = 'C' AND chrg.account LIKE '[C,D]%' THEN qty*amount ELSE 0.00 END as 'CLIENT Net', CASE WHEN fin_type = 'C' AND chrg.account LIKE 'Q' THEN qty*amount ELSE 0.00 END as 'QUEST Net', CASE WHEN fin_type = 'C' AND chrg.account LIKE 'J' THEN qty*amount ELSE 0.00 END as 'JPG Net', CASE WHEN fin_type NOT IN ('M','C') THEN qty*amount ELSE 0.00 END as 'Other Billed', qty * amt.amount as 'Total' FROM chrg LEFT OUTER JOIN acc on REPLACE(chrg.account,'Q','C') = acc.account LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm LEFT OUTER JOIN client on acc.cl_mnem = client.cli_mnem LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt on chrg.chrg_num = amt.chrg_num LEFT OUTER JOIN dbo.acc_location ON dbo.acc_location.account = REPLACE(chrg.account,'Q','C') WHERE chrg.mod_date BETWEEN @startDate and @endDate and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP') and cl_mnem = ISNULL(@cl_mnem,[cl_mnem]) AND chrg.account LIKE 'Q%') ,cteJPG ([ACCOUNT],[Department], [Item], [Description], [Qty], [FinType],[Client],[Client Name],[3rd Party Net],[CLIENT Net],[QUEST Net],[JPG Net],[Other Billed], [Total Charges]) as ( SELECT REPLACE(chrg.account,'J','C') AS [account],LEFT(chrg.cdm,3) as 'Department' , RIGHT(chrg.cdm,4) as 'Item', cdm.descript as 'Description', qty as 'Qty', fin_type, acc.cl_mnem AS 'Client', client.cli_nme as 'Client Name', CASE WHEN fin_type = 'M' THEN qty*amount ELSE 0.00 END as '3rd Party Net',  CASE WHEN fin_type = 'C' AND chrg.account LIKE '[C,D]%' THEN qty*amount ELSE 0.00 END as 'CLIENT Net', CASE WHEN fin_type = 'C' AND chrg.account LIKE 'Q' THEN qty*amount ELSE 0.00 END as 'QUEST Net', CASE WHEN fin_type = 'C' AND chrg.account LIKE 'J' THEN qty*amount ELSE 0.00 END as 'JPG Net', CASE WHEN fin_type NOT IN ('M','C')  THEN qty*amount ELSE 0.00 END as 'Other Billed', qty * amt.amount as 'Total' FROM chrg LEFT OUTER JOIN acc on REPLACE(chrg.account,'J','C') = acc.account LEFT OUTER JOIN cteCDM cdm on chrg.cdm = cdm.cdm LEFT OUTER JOIN client on acc.cl_mnem = client.cli_mnem LEFT OUTER JOIN (select chrg_num, SUM(amount) as amount from amt GROUP BY chrg_num) amt on chrg.chrg_num = amt.chrg_num LEFT OUTER JOIN dbo.acc_location ON dbo.acc_location.account = REPLACE(chrg.account,'Q','C') WHERE chrg.mod_date BETWEEN @startDate and @endDate and chrg.cdm <> 'CBILL' AND chrg.status NOT IN('N/A','CBILL','CAP') and cl_mnem = ISNULL(@cl_mnem,[cl_mnem])AND chrg.account LIKE 'J%'), cteT AS ( select cteChrg.ACCOUNT AS [ACCOUNT] ,SUM(cteChrg.Qty) AS [QTY] ,cteChrg.Client AS [Client] ,cteChrg.[Client Name] ,SUM(cteChrg.[3rd Party Net]) AS [3rd Party Net] ,SUM(cteChrg.[CLIENT Net]) AS [CLIENT Net],SUM(cteChrg.[QUEST Net]) AS [QUEST Net],SUM(cteChrg.[JPG Net]) AS [JPG Net],SUM(cteChrg.[Other Billed]) AS [Other Billed],SUM(cteChrg.[Total Charges]) AS [Total Charges] ,0 AS [INCLUDES QUEST],0 AS [INCLUDES JPG] from cteChrg GROUP BY cteChrg.Client, cteChrg.[Client Name],cteChrg.FinType, cteChrg.Account UNION ALL select cteQuest.ACCOUNT AS [ACCOUNT] ,SUM(cteQuest.Qty) AS [QTY] ,cteQuest.client AS [Client] ,cteQuest.[Client Name] ,SUM(cteQuest.[3rd Party Net]) AS [3rd Party Net] ,SUM(cteQuest.[CLIENT Net]) AS [CLIENT Net],SUM(cteQuest.[Total Charges]) AS [Quest Net],SUM(cteQuest.[JPG Net]) AS [JPG Net],SUM(cteQuest.[Other Billed]) AS [Other Billed],SUM(cteQuest.[Total Charges]) AS [Total Charges] ,1 AS [INCLUDES QUEST],0 AS [INCLUDES JPG] from cteQuest GROUP BY cteQuest.Client, cteQuest.[Client Name],cteQuest.FinType, cteQuest.Account UNION ALL select cteJPG.ACCOUNT AS [ACCOUNT] ,SUM(cteJPG.Qty) AS [QTY] ,cteJPG.client AS [Client] ,cteJPG.[Client Name] ,SUM(cteJPG.[3rd Party Net]) AS [3rd Party Net] , SUM(cteJPG.[CLIENT Net]) AS [CLIENT Net],SUM(cteJPG.[QUEST Net]) AS [QUEST Net],SUM(cteJPG.[Total Charges]) AS [JPG Net],SUM(cteJPG.[Other Billed]) AS [Other Billed],SUM(cteJPG.[Total Charges]) AS [Total Charges] ,0 AS [INCLUDES QUEST],1 AS [INCLUDES JPG] from cteJPG GROUP BY cteJPG.Client, cteJPG.[Client Name],cteJPG.FinType, cteJPG.Account) , cteGrandTotals AS (SELECT 	cteT.Client ,cteT.[Client Name] ,STR(SUM(cteT.QTY),10,0) AS [QTY] ,STR(SUM(cteT.[3rd Party Net]),10,2) AS [3rd Party Net] ,STR(SUM(cteT.[CLIENT Net]),10,2) AS [CLIENT Net],STR(SUM(cteT.[Quest Net]),10,2) AS [Quest Net] ,STR(SUM(cteT.[JPG Net]),10,2) AS [JPG Net] ,STR(SUM(cteT.[Other Billed]),10,2) AS [OtherBilled] ,STR(SUM(cteT.[Total Charges]),10,2) AS [Total Charges] , STR(CASE WHEN [INCLUDES QUEST] = 0 THEN SUM(ISNULL(gpbdr.colAmtPaid,0.00)) ELSE 0.00 END,10,2) AS [3rd Party Total Paid],CASE WHEN [Includes Quest] = 0 THEN STR(ISNULL((SELECT colAmtPaid from dbo.GetPaymentsByClient(cteT.Client,@startDate,@endDate)),0.00),10,2) ELSE STR(ISNULL(SUM(cteT.[Quest Net]),0.00),10,2) END	AS [Client Total Paid] FROM cteT OUTER APPLY dbo.GetPaymentsByAccount(cteT.ACCOUNT) gpbdr GROUP BY cteT.Client ,cteT.[Client Name] , [includes quest],[INCLUDES JPG]
--) 



--	INSERT INTO @Table_Var
--			(
--				Client ,[Client Name] , 
--				[CLIENT QTY] ,	[QUEST QTY] , [JPG QTY] ,	[Other QTY],
--				[Total QTY] 
--			)
--	
--	
--	SELECT 	Client,	[Client Name],SUM(CONVERT(NUMERIC(18,0),QTY)) AS [QTY]
--	,SUM(CONVERT(NUMERIC(18,2),[3rd Party Net])) AS [3rd Party Net]
--	,sum(CONVERT(NUMERIC(18,2),[CLIENT Net])) AS [CLIENT Net]
--	,sum(CONVERT(NUMERIC(18,2),[Quest Net])) AS [Quest Net]
--	
--	,sum(CONVERT(NUMERIC(18,2),[JPG Net])) AS [JPG Net]
--	,SUM(CONVERT(NUMERIC(18,2),[OtherBilled])) AS [OtherBilled]
--	
--	,SUM(CONVERT(NUMERIC(18,2),[Total Charges])) AS [Total Charges]
--	,SUM(CONVERT(NUMERIC(18,2),[3rd Party Total Paid])) AS [3rd Party Total Paid]
--	,SUM(CONVERT(NUMERIC(18,2),[Client Net])+CONVERT(NUMERIC(18,2),[Quest Net])+CONVERT(NUMERIC(18,2),[JPG Net])) AS [Client Total Paid] 
--	FROM cteGrandTotals 
--	WHERE Client = @cl_mnem 
--	GROUP BY Client, [Client Name]
	RETURN 
END
