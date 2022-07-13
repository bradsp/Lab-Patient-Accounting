CREATE VIEW [dbo].[vw_temp_fix_affil_chrg]
AS
SELECT chrg.chrg_num, chrg.account, chrg.status, 
    chrg.service_date, chrg.cdm, chrg.qty, chrg.retail, 
    chrg.inp_price, chrg.invoice, chrg.mod_date, chrg.mod_user, 
    chrg.net_amt, chrg.fin_type
FROM dbo.chrg
WHERE (dbo.chrg.mod_date BETWEEN '04/08/04 00:00' AND 
    '04/12/2004 23:59') AND (dbo.chrg.service_date BETWEEN 
    '04/08/04 00:00' AND '04/12/2004 23:59') AND 
    (dbo.chrg.mod_prg = 'chrg_err Application') AND 
    (dbo.chrg.invoice IS NULL)
