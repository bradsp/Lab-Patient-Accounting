-- =============================================
-- Author:		David
-- Create date: 09/04/2014
-- Description:	Create a table for Charges with payments
-- =============================================
CREATE PROCEDURE usp_prg_Charges_With_Payments2 
	-- Add the parameters for the stored procedure here
	@client varchar(10) = 0, 
	@startDate datetime = 0,
	@endDate DATETIME = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT @Client, @startDate, @endDate
	DECLARE @columns NVARCHAR(max), @sql NVARCHAR(MAX)


IF EXISTS (SELECT * FROM sys.objects 
WHERE OBJECT_ID = OBJECT_ID(N'#tempChk') AND TYPE IN (N'U'))
BEGIN	
DROP TABLE #tempChk
END
CREATE TABLE #tempChk
(
	account VARCHAR(15),
	source VARCHAR(50),
	paid NUMERIC(18,2),
	contractual NUMERIC(18,2),
	write_off NUMERIC(18,2),
	total AS (paid+contractual+write_off)
	)


;WITH cteChrg
AS
(
SELECT ROW_NUMBER() OVER (PARTITION BY chrg.account  ORDER BY chrg.account) AS [rn],
chrg.account, SUM(qty*amount) AS [charges]
FROM dbo.chrg
INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account AND acc.account <> @client

WHERE acc.cl_mnem = @client
AND amt.mod_date BETWEEN @startDate AND @endDate
GROUP BY chrg.account 
) 
, cteChk
AS
(
	SELECT ROW_NUMBER() OVER (PARTITION BY cteChrg.account  ORDER BY cteChrg.account) AS [rn],
	cteChrg.account, COALESCE(ins_code,source,ins_code,'WO'+dbo.chk.write_off_code,dbo.chk.comment) AS [source], SUM(amt_paid) AS [paid], 
		SUM(contractual) AS [contractual],
		SUM(write_off) AS [write off]
		FROM cteChrg
		LEFT OUTER JOIN chk ON cteChrg.account = dbo.chk.account
		where  dbo.chk.account <> @client
		GROUP BY cteChrg.account, COALESCE(ins_code,dbo.chk.source,ins_code,'WO'+dbo.chk.write_off_code,dbo.chk.comment)
)
INSERT INTO #tempChk (account,
source,
paid,
contractual,
write_off)
SELECT cteChk.account ,
		cteChk.source ,
		cteChk.paid ,
		cteChk.contractual ,
		cteChk.[write off] 
FROM cteChk

SET @columns = N'';
SELECT @columns = @columns + N', p.'+QUOTENAME(source)
FROM  (SELECT distinct p.source FROM #tempChk AS p ) AS X
ORDER BY source

--SELECT @columns

SET @sql = N'
SELECT account, charges, ' + STUFF(@columns, 1,2,'') + '
from 
(
	select c.account, isnull(p.source,'''+'NO CHECK'+''') as [source], p.total, c.charges
	from 
	(
	SELECT chrg.account, SUM(qty*amount) AS [charges]
	FROM dbo.chrg
	INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num
	inner join acc on dbo.acc.account = chrg.account and acc.cl_mnem = '''+@client+'''
	where amt.mod_date between ''' + CAST(@startDate AS VARCHAR)
	+''' and '''+ CAST(@endDate AS VARCHAR)+''' 
	group by chrg.account
	) as c 
	left outer join #tempChk as p on p.account = c.account
) as K

pivot 
( 
	sum(total) for source IN ('
	+ STUFF(REPLACE(@columns, ', p.[', ',['),1,1,'')
	+')
) as p ;'
--SELECT @sql
EXEC sp_executesql @sql
END
