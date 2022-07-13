CREATE VIEW [dbo].[vw_debug_acc_ins]
AS
SELECT     dbo.acc.account, dbo.acc.pat_name, dbo.acc.fin_code AS acc_fin_code, dbo.ins.ins_code
FROM         dbo.acc INNER JOIN
                      dbo.ins ON dbo.acc.account = dbo.ins.account
WHERE     (dbo.acc.fin_code = 'Y') AND (dbo.ins.ins_code IN ('UAHC', 'UNISON'))
