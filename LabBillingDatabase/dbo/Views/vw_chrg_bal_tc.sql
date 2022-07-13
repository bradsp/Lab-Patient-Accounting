/****** Object:  View dbo.vw_chrg_bal_tc    Script Date: 9/19/2001 10:47:04 AM ******/
/*
	vw_chrg_bal_tc 09/15/00 Rick Crone
	A mod of vw_chrg_bal

	This view returns the NON PC charges for an account.

	This view created for the VC++ MED_STATS project.
*/

CREATE VIEW [dbo].[vw_chrg_bal_tc] AS
select account, total = sum(qty*round(amount,2))
from chrg JOIN amt ON chrg.chrg_num = amt.chrg_num
where type != 'PC' and (status not in('PAID','CBILL','CHRGCR','N/A') OR status IS NULL)
group by account
