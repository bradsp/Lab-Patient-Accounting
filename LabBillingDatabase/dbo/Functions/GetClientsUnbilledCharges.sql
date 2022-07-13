-- =============================================
-- Author:		David
-- Create date: 07/31/2014
-- Description:	Returns the unprocessed charges for Client bills
-- =============================================
CREATE FUNCTION GetClientsUnbilledCharges 
(
	-- Add the parameters for the function here
	@client varchar(10), 
	@thruDate datetime
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colDate DATETIME, 
	colAccount varchar(15),
	colPatientName VARCHAR(100),
	colCdm	VARCHAR(7),
	colQty	INT,
	colChargeDescription VARCHAR(50),
	colAmount NUMERIC(18,2)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
; WITH cteChrg
AS
(
SELECT 	TOP(100)percent dbo.chrg.account AS [ACCOUNT] ,
		dbo.chrg.cdm AS [CHARGE CODE],
		SUM(dbo.chrg.qty*dbo.amt.amount) AS [AMOUNT]
FROM chrg 
INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num
WHERE NULLIF(invoice,'') IS NULL
AND cdm <> 'CBILL'
AND amt.mod_date <= @thruDate
GROUP BY chrg.account, chrg.cdm
HAVING SUM(qty) <> 0
ORDER BY chrg.account, chrg.cdm
)
,cteTotals
AS
(

SELECT TOP(100) PERCENT CONVERT(VARCHAR(10),chrg.service_date,101) AS [DATE],
		dbo.chrg.account AS [ACCOUNT] ,
		dbo.chrg.pat_name AS [PATIENT NAME],
		dbo.chrg.cdm AS [CHARGE CODE],
		SUM(dbo.chrg.qty) AS [QTY] ,
		dbo.cdm.descript AS [CHARGE DESCRIPTION],
		SUM(dbo.chrg.qty*cteChrg.AMOUNT) AS [AMOUNT]
		
FROM chrg
INNER JOIN cteChrg ON cteChrg.ACCOUNT = dbo.chrg.account AND cteChrg.[CHARGE CODE] = chrg.cdm 
INNER JOIN acc ON dbo.acc.account = cteChrg.ACCOUNT AND acc.cl_mnem = @client	
INNER JOIN cdm ON dbo.cdm.cdm = dbo.chrg.cdm AND cdm.deleted = 0
WHERE NULLIF(invoice,'') IS NULL
GROUP BY chrg.service_date,
		dbo.chrg.account,
		dbo.chrg.pat_name,
		dbo.chrg.cdm,
		dbo.cdm.descript
		
ORDER BY CAST(service_date AS DATETIME),chrg.pat_name
)
INSERT INTO @Table_Var
			(
				colDate , colAccount ,colPatientName , colCdm ,colQty,
				colChargeDescription ,colAmount 
			)
SELECT * FROM cteTotals


RETURN 
END
