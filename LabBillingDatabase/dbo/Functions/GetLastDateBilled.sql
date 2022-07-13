-- =============================================
-- Author:		David
-- Create date: 08/19/2014
-- Description:	Gets last date billing was modified
-- =============================================
CREATE FUNCTION GetLastDateBilled 
(
	-- Add the parameters for the function here
	@acc varchar(15)
)
RETURNS datetime
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result datetime

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = NULL
	
	; WITH cte AS 
(
SELECT account, [billed]
FROM(
SELECT TOP 1000 
		dbo.pat.account ,
		dbo.pat.last_dm AS [day0],
		dbo.pat.dbill_date AS [day1] ,
		dbo.pat.ub_date AS [day2] ,
		dbo.pat.h1500_date  AS [day3],
		dbo.pat.colltr_date  AS [day4],
		dbo.pat.baddebt_date  AS [day5],
		dbo.pat.batch_date  AS [day6],
		dbo.pat.bd_list_date  AS [day7],
		dbo.pat.ebill_batch_date  AS [day8],
		dbo.pat.ebill_batch_1500  AS [day9],
		dbo.pat.e_ub_demand_date  AS [day10],
		dbo.pat.claimsnet_1500_batch_date AS [day11] ,
		dbo.pat.claimsnet_ub_batch_date AS [day12] 		
FROM dbo.pat 
--INNER JOIN (SELECT dbo.chrg.account FROM chrg INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num
--WHERE amt.mod_date BETWEEN @startDate AND @endDate) AS [caSelect] 
--	ON caSelect.account = dbo.pat.account
WHERE dbo.pat.account = @acc AND COALESCE(dbo.pat.last_dm ,
		dbo.pat.dbill_date,dbo.pat.ub_date ,
		dbo.pat.h1500_date,	dbo.pat.colltr_date,
		dbo.pat.baddebt_date,	dbo.pat.batch_date,
		dbo.pat.bd_list_date,	dbo.pat.ebill_batch_date,
		dbo.pat.ebill_batch_1500,dbo.pat.e_ub_demand_date,
		dbo.pat.claimsnet_1500_batch_date,	dbo.pat.claimsnet_ub_batch_date ) IS NOT NULL
) AS bill
UNPIVOT
(
	[billed] FOR [dates] IN ([day0],[day1],[day2],[day3],[day4],[day5],[day6],[day7],
	[day8],[day9],[day10],[day11],[day12])
	) AS up
) 
SELECT @Result = (select MAX(billed) FROM cte	
GROUP BY cte.account)

	-- Return the result of the function
	RETURN @Result

END
