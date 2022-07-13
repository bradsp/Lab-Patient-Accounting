/****** Object:  View dbo.vw_chk_bal    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_chk_bal_cnb]
AS
SELECT     TOP 100 PERCENT account, SUM(ROUND(amt_paid, 2) + ROUND(write_off, 2) + ROUND(contractual, 2)) AS total, date_rec, invoice
FROM         dbo.chk
WHERE     (status = 'CBILL') OR
                      (status = 'new')
GROUP BY account, date_rec, invoice
HAVING      (NOT (invoice IS NULL))
ORDER BY account, date_rec, invoice
