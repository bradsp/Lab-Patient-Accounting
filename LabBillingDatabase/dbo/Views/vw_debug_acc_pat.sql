CREATE VIEW [dbo].[vw_debug_acc_pat]
AS
SELECT     dbo.acc.account, dbo.acc.pat_name, dbo.acc.fin_code, dbo.acc.trans_date, dbo.acc.cl_mnem, dbo.pat.phy_id
FROM         dbo.acc INNER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account
