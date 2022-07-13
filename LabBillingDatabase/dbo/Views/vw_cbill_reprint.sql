/****** Object:  View dbo.vw_cbill_reprint    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_cbill_reprint] AS

select acc.account,chrg.invoice,acc.fin_code,acc.cl_mnem,acc.pat_name
from acc JOIN chrg on acc.account = chrg.account
