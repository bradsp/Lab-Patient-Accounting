/****** Object:  View dbo.vw_acct_info    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_acct_info] AS

select chk.chk_no,acc.pat_name,acc.account,chk.amt_paid,chk.source,chk.write_off,chk.contractual,chk.date_rec
from chk JOIN acc on chk.account = acc.account
