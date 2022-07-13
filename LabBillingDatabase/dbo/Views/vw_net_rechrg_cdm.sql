/****** Object:  View dbo.vw_net_rechrg_cdm    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_net_rechrg_cdm] AS
select chrg.account,chrg.cdm,SUM(chrg.qty) qty
from chrg
where credited = 0
group by account,cdm
having SUM(chrg.qty) <> 0
