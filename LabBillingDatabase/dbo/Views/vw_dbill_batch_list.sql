/****** Object:  View dbo.vw_dbill_batch_list    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_dbill_batch_list] AS
select DISTINCT batch,
	DATEPART(month,run_date) as month,
	DATEPART(day,run_date) as day,
	DATEPART(year,run_date) as year
from dbill
