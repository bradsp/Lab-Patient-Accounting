-- =============================================
-- Author:		David
-- Create date: 08/01/2014
-- Description:	Gets charges from an old invoice number
-- =============================================
CREATE FUNCTION GetInvoiceCharges 
(
	-- Add the parameters for the function here
	@invoice varchar(50)
	 
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
--	DATE	ACCOUNT	PATIENT NAME	CHARGE CODE	QTY	CHARGE DESCRIPTION	AMOUNT

	[DATE] varchar(10), 
	[ACCOUNT] varchar(15),
	[PATIENT NAME] varchar(100),
	[CHARGE CODE] VARCHAR(7),
	[QTY] INT,
	[CHARGE DESCRIPTION] VARCHAR(100),
	[AMOUNT] NUMERIC(18,2),
	[INVOICE] VARCHAR(20)
)
AS
BEGIN
	
	-- Fill the table variable with the rows for your result set
	; WITH cteChrg
AS
(
SELECT 	TOP(100)percent 
		CONVERT(VARCHAR(10),chrg.service_date,101) AS [DATE],
		dbo.chrg.account AS [ACCOUNT] ,
		dbo.acc.pat_name AS [PATIENT NAME],
		dbo.chrg.cdm AS [CHARGE CODE],
		SUM(dbo.chrg.qty) AS [QTY],
		dbo.cdm.descript AS [CHARGE DESCRIPTION],
		SUM(dbo.chrg.qty*dbo.amt.amount) AS [AMOUNT]
FROM chrg 
INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account
LEFT OUTER JOIN cdm ON dbo.cdm.cdm = dbo.chrg.cdm AND cdm.deleted = 0
WHERE invoice = @invoice AND chrg.cdm <> 'CBILL' AND credited = 0
GROUP BY chrg.service_date,chrg.account, acc.pat_name, chrg.cdm, cdm.descript
HAVING SUM(qty) <> 0
ORDER BY chrg.account, chrg.cdm
) 
INSERT INTO @Table_Var
		(
		DATE ,ACCOUNT ,			[PATIENT NAME] ,[CHARGE CODE] ,
			QTY,
			
			
			[CHARGE DESCRIPTION] ,
			AMOUNT , INVOICE
			
		)

SELECT DATE ,		ACCOUNT ,		[PATIENT NAME] ,
		[CHARGE CODE] ,
		QTY ,
		[CHARGE DESCRIPTION] ,
		AMOUNT ,@invoice
		FROM cteChrg	
		
	RETURN 
END
