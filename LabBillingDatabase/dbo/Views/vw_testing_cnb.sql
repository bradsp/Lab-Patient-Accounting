CREATE VIEW [dbo].[vw_testing_cnb]
AS
SELECT     TOP 100 PERCENT dbo.acc.account, dbo.acc.fin_code, dbo.acc.trans_date, SUM(dbo.chk.amt_paid) AS paid, SUM(dbo.chk.write_off) AS woff, 
                      SUM(dbo.chk.contractual) AS cont, dbo.fin.res_party
FROM         dbo.acc INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account INNER JOIN
                      dbo.amt ON dbo.chrg.chrg_num = dbo.amt.chrg_num INNER JOIN
                      dbo.chk ON dbo.acc.account = dbo.chk.account INNER JOIN
                      dbo.fin ON dbo.acc.fin_code = dbo.fin.fin_code
WHERE     (dbo.acc.status = 'PAID_OUT') AND (dbo.acc.trans_date BETWEEN CONVERT(DATETIME, '2006-07-01 00:00:00', 102) AND CONVERT(DATETIME, 
                      '2006-07-31 00:00:00', 102))
GROUP BY dbo.acc.trans_date, dbo.acc.status, dbo.acc.account, dbo.acc.fin_code, dbo.fin.res_party
ORDER BY dbo.acc.fin_code
