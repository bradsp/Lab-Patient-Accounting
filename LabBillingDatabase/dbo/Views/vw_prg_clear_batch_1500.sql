CREATE VIEW [dbo].[vw_prg_clear_batch_1500]
AS
SELECT     TOP 100 PERCENT dbo.h1500.batch, dbo.h1500.account, dbo.acc.pat_name
FROM         dbo.h1500 INNER JOIN
                      dbo.acc ON dbo.h1500.account = dbo.acc.account
WHERE     (dbo.h1500.batch IS NOT NULL)
ORDER BY dbo.h1500.batch DESC, dbo.acc.pat_name
