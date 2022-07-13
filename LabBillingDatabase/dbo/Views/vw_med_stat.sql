/****** Object:  View dbo.vw_med_stat    Script Date: 9/19/2001 10:47:04 AM ******/
/*
	vw_med_stat 09/15/00 Rick Crone
	
	This view was developed for the VC++ MED_STAT program
	which creates reports on our collections from Medicare.


*/
CREATE VIEW [dbo].[vw_med_stat] AS
select acc.fin_code, chk.date_rec, chk.account, chk.amt_paid, chk.write_off, chk.contractual
from chk JOIN acc on chk.account = acc.account
where fin_code = 'A'
