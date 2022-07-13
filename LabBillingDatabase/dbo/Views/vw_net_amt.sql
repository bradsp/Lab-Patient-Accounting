/****** Object:  View dbo.vw_net_amt    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_net_amt] AS
select chrg.account,chrg.cdm,SUM(amt.amount*chrg.qty) amt
from chrg join amt on chrg.chrg_num = amt.chrg_num
--where credited = 0 
group by account,cdm
--having SUM(chrg.qty) <> 0
