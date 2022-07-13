CREATE VIEW [dbo].[vw_APC_cnb]
AS
SELECT     TOP 100 PERCENT dbo.chrg.chrg_num, dbo.chrg.service_date, dbo.chrg.qty, dbo.chrg.net_amt, dbo.chrg.fin_type, dbo.acc.cl_mnem, dbo.acc.fin_code, 
                      dbo.acc.trans_date, dbo.chrg.status, dbo.chrg.qty * dbo.chrg.net_amt AS the_val
FROM         dbo.acc INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.acc.account
GROUP BY dbo.chrg.chrg_num, dbo.chrg.service_date, dbo.chrg.qty, dbo.chrg.net_amt, dbo.chrg.fin_type, dbo.acc.cl_mnem, dbo.acc.fin_code, 
                      dbo.acc.trans_date, dbo.chrg.status, dbo.chrg.qty * dbo.chrg.net_amt
HAVING      (dbo.acc.fin_code = 'APC') AND (dbo.acc.cl_mnem = 'pk') AND (dbo.acc.trans_date BETWEEN CONVERT(DATETIME, '2002-01-01 00:00:00', 102) AND 
                      CONVERT(DATETIME, '2002-01-31 00:00:00', 102)) AND (dbo.chrg.status <> 'paid_out' AND dbo.chrg.status <> 'cbill') AND (dbo.chrg.qty < 0)
ORDER BY dbo.acc.cl_mnem
