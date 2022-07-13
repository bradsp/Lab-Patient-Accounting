/****** Object:  View dbo.vw_lmrp_rep    Script Date: 9/19/2001 10:47:04 AM ******/
/* vw_lmrp_rep 05/23/00 Rick Crone
	This view is a mod of vw_acc_pat that includes the client
	type from the client table. This will allow us to create
	LMRP reports for nursing homes and to not have these
	patients on the report for the doctor.
*/
CREATE VIEW [dbo].[vw_lmrp_rep] AS
SELECT acc.account,acc.pat_name,acc.trans_date,acc.fin_code,acc.cl_mnem,acc.status,
	pat.dbill_date,pat.ub_date,pat.h1500_date,pat.batch_date,pat.ebill_batch_date,
	pat.mailer,fin.h1500,fin.ub92,pat.phy_id,client.type
FROM acc LEFT OUTER JOIN pat ON acc.account = pat.account
	JOIN fin ON acc.fin_code = fin.fin_code
	JOIN client ON acc.cl_mnem = client.cli_mnem
WHERE acc.status <> 'PAID_OUT'
