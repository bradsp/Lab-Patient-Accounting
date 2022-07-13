-- =============================================
-- Author:		David
-- Create date: 03/20/2015
-- Description:	Returns Charges by insurance and client
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_charges_by_insurance_by_client] 
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


; WITH cteChrg as
(
select 
chrg.account, ISNULL(cteAccIns.ins_code,'UNKNOWN') AS [ins_code],
sum(amount*qty) as [charges]
from chrg
inner join amt on amt.chrg_num = chrg.chrg_num
INNER JOIN ##cteAccIns as cteAccIns ON cteAccIns.account = dbo.chrg.account
where amt.mod_date between @startDate AND @endDate
AND cdm <> 'CBILL' AND NOT (chrg.status IN ('CBILL','N/A','CAP'))
GROUP BY chrg.account,cteAccIns.account, cteAccIns.ins_code --WITH ROLLUP
HAVING SUM(dbo.amt.amount*qty) <> 0

) 
-- this lists all the charges by the "IN" clause below and is accurate as of 12/22/2014
SELECT x.INSURANCE ,
		SUM(x.Client) AS [CLIENT] ,
		SUM(x.thirdparty) AS [thirdparty] FROM (
SELECT CASE WHEN cteChrg.ins_code 
	IN ('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
		THEN ins_code
		ELSE 'OTHER' END AS [INSURANCE],

		CASE WHEN cteChrg.ins_code = 'CLIENT'
			THEN SUM(ISNULL(cteChrg.charges,0)) 
			ELSE 0 END AS [Client],
		CASE WHEN cteChrg.ins_code <> 'CLIENT'
			THEN SUM(ISNULL(cteChrg.charges,0)) 
			ELSE 0 END AS [thirdparty]
			
			
FROM cteChrg 
GROUP BY CASE WHEN cteChrg.ins_code 
	IN ('AETNA','AM','BC','CIGNA','CLIENT','MC','SP','TNBC','UHC','UNKNOWN')
		THEN ins_code
		ELSE 'OTHER' END , cteChrg.ins_code--WITH ROLLUP
		) AS x
GROUP BY x.INSURANCE
	
    END
