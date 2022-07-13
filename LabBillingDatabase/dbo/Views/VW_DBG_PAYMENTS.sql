CREATE VIEW [dbo].[VW_DBG_PAYMENTS]
AS
SELECT     TOP 100 PERCENT dbo.chrg.account, dbo.acc.cl_mnem, dbo.acc.trans_date, dbo.acc.fin_code, dbo.chk.amt_paid, dbo.chk.write_off, 
                      dbo.chk.contractual
FROM         dbo.chrg INNER JOIN
                      dbo.acc ON dbo.chrg.account = dbo.acc.account INNER JOIN
                      dbo.chk ON dbo.acc.account = dbo.chk.account
WHERE     (dbo.acc.trans_date > CONVERT(DATETIME, '2006-12-31 00:00:00', 102)) AND (dbo.acc.cl_mnem IN ('TPG', 'MCH', 'HC')) AND (dbo.acc.fin_code = 'A')
ORDER BY dbo.acc.trans_date DESC
