-- =============================================
-- Author:		David
-- Create date: 06/10/2011
-- Description:	Returns Charges by cost centers
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_charges_by_insurance] 
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


--; WITH cteAccIns
--AS
--(
--	SELECT acc.account AS [ACCOUNT], acc.fin_code, acc.trans_date
--	, CASE WHEN acc.fin_code IN ('CLIENT','W','X','Y','Z') 
--			OR ins.ins_code IN ('CLIENT','W','X','Y','Z') then 'CLIENT'
--		   WHEN COALESCE(NULLIF(ins.ins_code,''),acc.fin_code) = 'E' 
--			OR ins.ins_code = 'SP' THEN 'SP'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'B' 
--			OR ins.ins_code = 'BC' THEN 'BC'		   
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'A' 
--		    OR ins.ins_code = 'MC' THEN 'MC'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'D' 
--			OR ins.ins_code  = 'TNBC' THEN 'TNBC'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'M' 
--			OR ins.ins_code = 'AM' THEN 'AM'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'S'
--		    OR ins.ins_code = 'MISC' THEN 'MISC'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'H' 
--			OR ins.ins_code = 'COMM.H' THEN 'COMM.H'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'L' 
--			OR ins.ins_code = 'COMM.L' THEN 'COMM.L'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'AETNA' THEN 'AETNA'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'UHC' THEN 'UHC'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'AM' THEN 'AM'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'CIGNA' THEN 'CIGNA'
--		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'TNBC' THEN 'TNBC'
--		   else 'OTHER' END AS ins_code
--	FROM dbo.acc
--	LEFT OUTER JOIN ins ON dbo.ins.account = dbo.acc.account AND ins.ins_a_b_c = 'A'
--		--AND dbo.ins.fin_code = dbo.acc.fin_code
--	WHERE acc.trans_date >= DATEADD(MONTH,-12,@startDate) 
--) 

; WITH cteChrg as
(
select 
chrg.account, ISNULL(cteAccIns.ins_code,'UNKNOWN') AS [ins_code],
sum(amount*qty) as [charges]
from chrg
inner join amt on amt.chrg_num = chrg.chrg_num
LEFT outer JOIN ##cteAccIns as cteAccIns ON cteAccIns.account = dbo.chrg.account
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
