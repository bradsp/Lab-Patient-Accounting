/****** Object:  View dbo.vw_chk_bal_cbill    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_chk_bal_cbill] AS
select account, total = sum(round(amt_paid,2) + round(write_off,2) + round(contractual,2))
from chk
where (status <> 'CBILL' or status IS NULL) and invoice IS NOT NULL
group by account
