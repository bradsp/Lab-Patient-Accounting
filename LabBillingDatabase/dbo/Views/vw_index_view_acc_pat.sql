CREATE VIEW dbo.vw_index_view_acc_pat WITH SCHEMABINDING 
AS
SELECT tst.data_patBill.account ,
		tst.data_patBill.pat_name ,
		tst.data_patBill.trans_date ,
		tst.data_patBill.fin_code ,
		tst.data_patBill.cl_mnem ,
		tst.data_patBill.status ,
		tst.data_patBill.dbill_date ,
		tst.data_patBill.ub_date ,
		tst.data_patBill.h1500_date ,
		tst.data_patBill.batch_date ,
		tst.data_patBill.ebill_batch_date ,
		tst.data_patBill.mailer ,
		tst.data_patBill.h1500 ,
		tst.data_patBill.ub92 ,
		tst.data_patBill.phy_id ,
		tst.data_patBill.ebill_batch_1500 ,
		tst.data_patBill.last_dm ,
		tst.data_patBill.bd_list_date ,
		tst.data_patBill.claimsnet_1500_batch_date 
FROM tst.data_patBill

--SELECT     dbo.acc.account
--, acc.pat_name AS [pat_name]
--, acc.trans_date AS [trans_date]
--, acc.fin_code, acc.cl_mnem, acc.status, pat.dbill_date, pat.ub_date, 
--                      pat.h1500_date, pat.batch_date, pat.ebill_batch_date, pat.mailer, fin.h1500, fin.ub92, pat.phy_id, 
--                      pat.ebill_batch_1500, pat.last_dm, pat.bd_list_date, pat.claimsnet_1500_batch_date
--INTO tst.data_patBill
--FROM         dbo.acc LEFT OUTER JOIN
--                      dbo.pat ON dbo.acc.account = dbo.pat.account INNER JOIN
--                      dbo.fin ON dbo.acc.fin_code = dbo.fin.fin_code
--WHERE      (acc.status NOT IN ('PAID_OUT', 'CLOSED'))
--AND (acc.trans_date < CONVERT(varchar(10), GETDATE(), 101))
--AND pat.mailer <> 'N'

GO
CREATE UNIQUE CLUSTERED INDEX [i_vw_index_view_acc_pat]
    ON [dbo].[vw_index_view_acc_pat]([account] ASC) WITH (STATISTICS_NORECOMPUTE = ON);

