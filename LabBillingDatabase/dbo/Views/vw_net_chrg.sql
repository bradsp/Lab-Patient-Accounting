/****** Object:  View dbo.vw_net_chrg    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_net_chrg] AS
select chrg.account,chrg.cdm,round(chrg.inp_price,2) 
	inp_price,round(chrg.retail,2) retail,
	chrg.status,amt.cpt4,amt.type,round(amt.amount,2) 
	amount,SUM(chrg.qty) tot_qty
from chrg JOIN amt ON chrg.chrg_num = amt.chrg_num
where chrg.credited = 0 and chrg.status <> 'CBILL'
group by 
chrg.account,chrg.cdm,round(chrg.inp_price,2),round(chrg.retail,2),
	chrg.status,amt.cpt4,amt.type,round(amt.amount,2)
having (SUM(chrg.qty) <> 0) and (amt.type <> 'N/A' or 
round(amt.amount,2) <> 0.00)
