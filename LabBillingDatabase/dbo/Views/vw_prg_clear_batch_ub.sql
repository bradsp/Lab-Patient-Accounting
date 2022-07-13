CREATE VIEW [dbo].[vw_prg_clear_batch_ub]
AS
SELECT     TOP 100 PERCENT dbo.ub.batch, dbo.ub.account, dbo.acc.pat_name
FROM         dbo.ub INNER JOIN
                      dbo.acc ON dbo.ub.account = dbo.acc.account
WHERE     (dbo.ub.batch IS NOT NULL)
ORDER BY dbo.ub.batch DESC, dbo.acc.pat_name
