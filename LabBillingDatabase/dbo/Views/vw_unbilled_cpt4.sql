/****** Object:  View dbo.vw_unbilled_cpt4    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_unbilled_cpt4] AS
select chrg.cdm,chrg.status,chrg.chrg_num,
	amt.cpt4,
	 pat.dbill_date,pat.ub_date,pat.h1500_date,pat.batch_date
from chrg join pat on chrg.account = pat.account
join amt on chrg.chrg_num = amt.chrg_num
where chrg.status <> 'CBILL' and
(pat.dbill_date IS NULL and pat.ub_date IS NULL and
	 pat.h1500_date IS NULL and pat.batch_date IS NULL)
