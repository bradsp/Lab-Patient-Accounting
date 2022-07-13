/****** Object:  View dbo.vw_chk_bal    Script Date: 9/19/2001 10:47:04 AM *****




*/
CREATE VIEW [dbo].[vw_chk_bal_cbill_cnb]
AS
SELECT     TOP 100 PERCENT account, SUM(ROUND(amt_paid, 2) + ROUND(write_off, 2) + ROUND(contractual, 2)) AS total, invoice, date_rec
FROM         dbo.chk
WHERE     (status <> 'CBILL' OR
                      status IS NULL) AND (invoice IS NOT NULL)
GROUP BY account, invoice, date_rec
ORDER BY account
