-- =============================================
-- Author:		David
-- Create date: 07/24/2013
-- Description:	Quest CBill by invoice
-- =============================================
CREATE PROCEDURE [dbo].[usp_quest_cbill_by_invoice] 
	-- Add the parameters for the stored procedure here
@invoice varchar(15) = 0

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;
;WITH cteInvoice
AS
(
SELECT  
        chrg.invoice,
        account ,
        thru_date,
        cdm ,
        SUM(qty*net_amt) AS [Amount] 
        
FROM dbo.chrg
inner JOIN dbo.cbill_hist ON dbo.cbill_hist.invoice = chrg.invoice
WHERE cl_mnem = 'QUESTR' 
AND cdm <> 'CBILL' AND chrg.invoice = @invoice
GROUP BY chrg.invoice, account, cdm, thru_date --WITH ROLLUP
) --SELECT * FROM cteInvoice
 , cteQB
AS
(
	SELECT  DISTINCT 
			cteInvoice.invoice,
			cteInvoice.account,
			qb.deleted ,
	        cteInvoice.cdm ,
	        qb.Patient ,
	        qb.req_no ,
	        qb.collection_date ,
	        qb.DOB ,
	        qb.quest_code ,
	        qb.quest_desc ,
				cteInvoice.Amount,
	        qb.invoice AS [NOTES] ,
	        qb.mod_date,
	        qb.comment
FROM cteInvoice 
INNER JOIN dbo.data_quest_billing qb
	ON qb.account = cteInvoice.account AND qb.cdm = cteInvoice.cdm and qb.deleted = 0
) --SELECT * FROM cteQB
, cteDiff
AS
(
	SELECT  invoice, account, cdm, Amount
	FROM cteInvoice
	EXCEPT 
	SELECT  invoice, account, cdm, Amount
	FROM cteQB
)
, cteChanges
AS
(
	SELECT  cteInvoice.invoice ,
	        cteInvoice.account ,
	        cteInvoice.thru_date ,
	        cteInvoice.cdm ,
	        cteInvoice.Amount ,
				 qb.deleted ,
				        qb.status ,
				        qb.req_no ,
				        qb.uid ,
				        qb.Patient ,
				        qb.collection_date ,
				        qb.DOB ,
				        qb.quest_code ,
				        qb.quest_desc ,
				        qb.quest_cpt4 ,
				        qb.comment 
				        
FROM cteDiff
INNER JOIN cteInvoice
	ON cteInvoice.account = cteDiff.account AND cteInvoice.invoice = cteDiff.invoice
		AND cteInvoice.cdm = cteDiff.cdm
INNER JOIN dbo.data_quest_billing qb
	ON qb.account = cteDiff.account AND qb.cdm = cteDiff.cdm and qb.deleted = 1
)
SELECT  invoice ,
        account ,
        cdm ,
        Patient ,
        req_no ,
        collection_date ,
        DOB ,
        quest_code ,
        quest_desc ,
        Amount ,
        ISNULL(comment,status) AS [NOTES]
FROM cteChanges
WHERE CONVERT(NUMERIC(18,2),amount) <> 0.00
UNION ALL
SELECT   invoice ,
        account ,
        cdm ,
        Patient ,
        req_no ,
        collection_date ,
        DOB ,
        quest_code ,
        quest_desc ,
        Amount ,
        comment AS [NOTES]
FROM cteQB
WHERE CONVERT(NUMERIC(18,2),amount) <> 0.00	
/* previous method
select chrg.account,chrg.cdm,data.patient,data.req_no, data.collection_date, data.dob
,data.quest_code,data.quest_desc
,round(sum(qty*amount),2) as [AMT], data.invoice as [NOTES]
from chrg 
inner join amt on amt.chrg_num = chrg.chrg_num
left outer join 
	data_quest_billing data
	on 
		(data.account = chrg.account and 
		data.cdm = chrg.cdm and
		data.deleted = 0 and 
		data.cdm <> '5322074' ) or
		(data.account = chrg.account and 
		data.cdm = chrg.cdm and
		data.deleted = 0 and 
		data.cdm = '5322074' and (
		quest_code = case when amt.cpt4 = '82951'
			then '18927' end
		or
		quest_code = case when amt.cpt4 = '82952'
			then  '95361' end))
where chrg.invoice = @invoice 
and chrg.cdm <> 'cbill' 
AND (NOT (dbo.chrg.status IN ('CBILL', 'CAP', 'N/A')))
group by chrg.account, chrg.cdm, data.patient,data.req_no,data.collection_date, data.dob, amt.cpt4
, data.quest_code, data.quest_desc, data.invoice
having round(sum(qty*amount),2) <> 0
*/
END
