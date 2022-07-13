CREATE VIEW [dbo].[vw_uhc_balance]
AS
SELECT     dbo.vw_chk_bal.account, LEFT(dbo.acc.pat_name, 25) AS Pat_Name, dbo.vw_chrg_net.service_date, SUM(dbo.vw_chrg_net.net_amt) AS Charges, 
                      dbo.vw_chk_bal.total AS Payed, SUM(dbo.vw_chrg_net.net_amt) - dbo.vw_chk_bal.total AS Balance
FROM         dbo.vw_chk_bal INNER JOIN
                      dbo.vw_chrg_net ON dbo.vw_chk_bal.account = dbo.vw_chrg_net.account INNER JOIN
                      dbo.ins ON dbo.vw_chk_bal.account = dbo.ins.account INNER JOIN
                      dbo.acc ON dbo.vw_chk_bal.account = dbo.acc.account
WHERE     (dbo.ins.plan_nme IN ('uhc', 'uh0', 'united healthcare'))
GROUP BY dbo.vw_chk_bal.account, dbo.vw_chk_bal.total, LEFT(dbo.acc.pat_name, 25), dbo.vw_chrg_net.service_date
HAVING      (dbo.vw_chrg_net.service_date BETWEEN CONVERT(DATETIME, '2005-12-31 23:59:00', 102) AND CONVERT(DATETIME, '2006-07-01 00:00:00', 102)) 
                      AND (SUM(dbo.vw_chrg_net.net_amt) - dbo.vw_chk_bal.total <> 0)
