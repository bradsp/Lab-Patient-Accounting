-- =============================================
-- Author:		David
-- Create date: 06/10/2011
-- Description:	Returns Charges by cost centers
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_charges_by_client] 
	-- Add the parameters for the stored procedure here
    @startDate DATETIME = 0 ,
    @endDate DATETIME = 0
AS
    BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
        SET NOCOUNT ON;

-- P:\wkelly\SQL Server Management Studio\Projects\AD HOC Queries\
--	Payor Mix by Insurance with Aetna and HP.sql


--SELECT * FROM ##cteAccIns

; WITH cteChrg as
(
select cteAccIns.CLIENT,--chrg.account, 
ISNULL(cteAccIns.ins_code,'UNKNOWN') AS [ins_code],
sum(amount*qty) as [charges]
from chrg
inner join amt on amt.chrg_num = chrg.chrg_num
LEFT outer JOIN ##cteAccIns as cteAccIns ON cteAccIns.account = dbo.chrg.account
where chrg.mod_date between @startDate AND @endDate
AND cdm <> 'CBILL' AND NOT (chrg.status IN ('CBILL','N/A','CAP'))
GROUP BY chrg.account,--cteAccIns.account, 
cteAccIns.CLIENT,cteAccIns.ins_code --WITH ROLLUP
HAVING SUM(dbo.amt.amount*qty) <> 0

) 
-- this lists all the charges by the "IN" clause below and is accurate as of 12/22/2014
SELECT x.Client ,
		SUM(x.clientAmt) AS [clientAmt] ,
		SUM(x.thirdpartyAmt) AS [thirdpartyAmt] FROM (
SELECT 
	cteChrg.CLIENT,

		CASE WHEN cteChrg.ins_code = 'CLIENT'
			THEN SUM(ISNULL(cteChrg.charges,0)) 
			ELSE 0 END AS [ClientAmt],
		CASE WHEN cteChrg.ins_code <> 'CLIENT'
			THEN SUM(ISNULL(cteChrg.charges,0)) 
			ELSE 0 END AS [thirdpartyAmt]
			
			
FROM cteChrg 
GROUP BY cteChrg.Client	 , cteChrg.ins_code--WITH ROLLUP
		) AS x
GROUP BY x.CLIENT
	
	
    END
