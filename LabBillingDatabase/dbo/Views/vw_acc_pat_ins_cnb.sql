CREATE VIEW [dbo].[vw_acc_pat_ins_cnb]
AS
SELECT     TOP 100 PERCENT dbo.acc.account, dbo.acc.fin_code, dbo.acc.trans_date, dbo.acc.status, dbo.ins.ins_a_b_c, dbo.ins.policy_num, dbo.pat.ssn, 
                      dbo.pat.dob_yyyy, dbo.pat.sex, dbo.acc.pat_name
FROM         dbo.acc INNER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account INNER JOIN
                      dbo.ins ON dbo.acc.account = dbo.ins.account
WHERE     (dbo.acc.fin_code = 'T')
ORDER BY dbo.acc.account
