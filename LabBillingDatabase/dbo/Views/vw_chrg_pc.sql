CREATE VIEW [dbo].[vw_chrg_pc]
AS
SELECT     TOP 100 PERCENT dbo.chrg.credited, dbo.chrg.account, dbo.chrg.status, dbo.chrg.service_date, dbo.chrg.cdm, dbo.chrg.qty, dbo.amt.type
FROM         dbo.chrg INNER JOIN
                      dbo.amt ON dbo.chrg.chrg_num = dbo.amt.chrg_num
WHERE     (dbo.chrg.service_date > CONVERT(DATETIME, '2005-12-31 00:00:00', 102)) AND (dbo.amt.type = 'TC')
ORDER BY dbo.chrg.account
