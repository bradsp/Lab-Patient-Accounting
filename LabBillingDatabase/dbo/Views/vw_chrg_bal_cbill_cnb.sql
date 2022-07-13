/****** Object:  View dbo.vw_chrg_bal    Script Date: 9/19/2001 10:47:04 AM *****





*/
CREATE VIEW [dbo].[vw_chrg_bal_cbill_cnb]
AS
SELECT     TOP 100 PERCENT dbo.chrg.account, SUM(dbo.chrg.qty * ROUND(dbo.amt.amount, 2)) AS total, dbo.chrg.invoice, dbo.chrg.service_date
FROM         dbo.chrg INNER JOIN
                      dbo.amt ON dbo.chrg.chrg_num = dbo.amt.chrg_num INNER JOIN
                      dbo.client ON dbo.chrg.account = dbo.client.cli_mnem
WHERE     (dbo.chrg.status NOT IN ('PAID', 'CBILL', 'CHRGCR', 'N/A') OR
                      dbo.chrg.status IS NULL) AND (dbo.chrg.invoice IS NOT NULL)
GROUP BY dbo.chrg.account, dbo.chrg.invoice, dbo.chrg.service_date
ORDER BY dbo.chrg.account, dbo.chrg.service_date
