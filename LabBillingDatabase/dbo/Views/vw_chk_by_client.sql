CREATE VIEW [dbo].[vw_chk_by_client]
AS
SELECT dbo.acc.account, dbo.acc.cl_mnem, dbo.acc.pat_name, 
    dbo.acc.fin_code, dbo.acc.trans_date, dbo.acc.original_fincode, 
    dbo.chk.chk_date, dbo.chk.date_rec, dbo.chk.chk_no, 
    dbo.chk.amt_paid, dbo.chk.write_off, dbo.chk.contractual, 
    dbo.chk.status, dbo.chk.source, dbo.chk.w_off_date, 
    dbo.chk.invoice, dbo.chk.batch, dbo.chk.bad_debt, 
    dbo.chk.mod_date
FROM dbo.acc INNER JOIN
    dbo.chk ON dbo.acc.account = dbo.chk.account
