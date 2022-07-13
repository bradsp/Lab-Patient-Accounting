CREATE VIEW [dbo].[vw_lmrp]
AS
SELECT     cpt4, beg_icd9, end_icd9, payor, fincode, mod_user, mod_prg, mod_date, rb_date, lmrp, lmrp2, rb_date2, chk_for_bad, ama_year, uid, 
                      ISNULL(expiration_date, GETDATE()) AS expire_date
FROM         dbo.lmrp
