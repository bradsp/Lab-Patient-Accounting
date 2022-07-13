/****** Object:  View dbo.vw_test    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_test] AS

select chrg.chrg_num,chrg.account,acc.cl_mnem,chrg.cdm,chrg.status,chrg.qty,chrg.service_date,chrg.retail,
	SUM(amt.amount) amount
from acc JOIN chrg on acc.account = chrg.account
JOIN amt on chrg.chrg_num = amt.chrg_num
where amt.type <> 'N/A' and chrg.invoice between '4262' and '4412'
	and acc.fin_code <> 'W' and chrg.cdm <> 'CBILL'
group by chrg.account,chrg.cdm,chrg.status,chrg.qty,chrg.service_date,chrg.retail,
	chrg.chrg_num,acc.cl_mnem
