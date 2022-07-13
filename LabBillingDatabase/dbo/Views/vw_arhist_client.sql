CREATE VIEW [dbo].[vw_arhist_client]
AS
SELECT acc.cl_mnem, aging_history.datestamp, 
    SUM(aging_history.balance) AS [AR Balance]
FROM dbo.aging_history INNER JOIN
    dbo.acc ON 
    dbo.aging_history.account = dbo.acc.account
GROUP BY dbo.acc.cl_mnem, dbo.aging_history.datestamp
