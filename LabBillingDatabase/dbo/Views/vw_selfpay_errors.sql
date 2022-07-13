/****** Object:  View dbo.vw_selfpay_errors    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_selfpay_errors] AS
SELECT acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, acc.trans_date, acc.ssn
FROM acc LEFT OUTER JOIN pat ON acc.account = pat.account
WHERE acc.fin_code = 'E' AND acc.status <> 'PAID_OUT' AND pat.account Is Null
