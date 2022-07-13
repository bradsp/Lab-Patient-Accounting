/****** Object:  View dbo.vw_chrg_bal_cbill    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_chrg_bal_cbill] AS
select account, total = sum(qty*round(amount,2))
from chrg JOIN amt ON chrg.chrg_num = amt.chrg_num
where (status not in('PAID','CBILL','CHRGCR','N/A') OR status IS NULL) and invoice IS NOT NULL
group by account
