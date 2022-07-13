/****** Object:  View dbo.vw_lab1_cbill    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_lab1_cbill] AS
SELECT chrg.invoice,chrg.account,acc.trans_date,SUM(chrg.qty) qty,acc.pat_name,acc.cl_mnem,
	SUM(chrg.retail*chrg.qty) retail,
	SUM(chrg.net_amt*chrg.qty) amount,chrg.cdm
FROM chrg JOIN acc on acc.account = chrg.account
GROUP BY chrg.invoice,chrg.account,acc.trans_date,chrg.cdm,acc.pat_name,acc.cl_mnem
having SUM(chrg.qty) <> 0
