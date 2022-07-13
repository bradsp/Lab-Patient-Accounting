CREATE VIEW [dbo].[vw_dmail_cnb]
AS
SELECT     dbo.vw_acc_pat.account, dbo.vw_acc_pat.pat_name, dbo.vw_acc_pat.cl_mnem, dbo.vw_acc_pat.mailer, dbo.vw_acc_pat.last_dm, 
                      dbo.vw_chrg_bal.total AS chrgtot, dbo.vw_chk_bal.total AS chktot
FROM         dbo.vw_acc_pat INNER JOIN
                      dbo.vw_chrg_bal ON dbo.vw_acc_pat.account = dbo.vw_chrg_bal.account INNER JOIN
                      dbo.vw_chk_bal ON dbo.vw_acc_pat.account = dbo.vw_chk_bal.account
WHERE     (dbo.vw_acc_pat.mailer = 'P')
