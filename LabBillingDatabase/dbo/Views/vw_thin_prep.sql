CREATE VIEW [dbo].[vw_thin_prep]
AS
SELECT     TOP 100 PERCENT dbo.chrg.account, dbo.chrg.service_date, dbo.chrg.cdm, dbo.chrg.qty, dbo.acc.fin_code, dbo.acc.trans_date, dbo.amt.cpt4, 
                      dbo.amt.amount, dbo.fin.res_party
FROM         dbo.chrg INNER JOIN
                      dbo.acc ON dbo.chrg.account = dbo.acc.account INNER JOIN
                      dbo.amt ON dbo.chrg.chrg_num = dbo.amt.chrg_num INNER JOIN
                      dbo.fin ON dbo.acc.fin_code = dbo.fin.fin_code
ORDER BY dbo.acc.fin_code, dbo.chrg.cdm
