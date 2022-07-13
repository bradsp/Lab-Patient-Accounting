CREATE VIEW [dbo].[vw_ssi_batch_list]
AS
SELECT DISTINCT batch, remit_date
FROM dbo.temp_ssi
WHERE (remit_date IS NOT NULL)
