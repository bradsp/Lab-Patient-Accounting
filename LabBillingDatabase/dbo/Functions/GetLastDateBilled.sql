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
		MCLLIVE.dbo.pat.account ,
		MCLLIVE.dbo.pat.last_dm AS [day0],
		MCLLIVE.dbo.pat.dbill_date AS [day1] ,
		MCLLIVE.dbo.pat.ub_date AS [day2] ,
		MCLLIVE.dbo.pat.h1500_date  AS [day3],
		MCLLIVE.dbo.pat.colltr_date  AS [day4],
		MCLLIVE.dbo.pat.baddebt_date  AS [day5],
		MCLLIVE.dbo.pat.batch_date  AS [day6],
		MCLLIVE.dbo.pat.bd_list_date  AS [day7],
		MCLLIVE.dbo.pat.ebill_batch_date  AS [day8],
		MCLLIVE.dbo.pat.ebill_batch_1500  AS [day9],
		MCLLIVE.dbo.pat.e_ub_demand_date  AS [day10],
		MCLLIVE.dbo.pat.claimsnet_1500_batch_date AS [day11] ,
		MCLLIVE.dbo.pat.claimsnet_ub_batch_date AS [day12] 		
FROM MCLLIVE.dbo.pat 
--INNER JOIN (SELECT dbo.chrg.account FROM chrg INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num
--WHERE amt.mod_date BETWEEN @startDate AND @endDate) AS [caSelect] 
--	ON caSelect.account = dbo.pat.account
WHERE dbo.pat.account = @acc AND COALESCE(MCLLIVE.dbo.pat.last_dm ,
		MCLLIVE.dbo.pat.dbill_date,MCLLIVE.dbo.pat.ub_date ,
		MCLLIVE.dbo.pat.h1500_date,	MCLLIVE.dbo.pat.colltr_date,
		MCLLIVE.dbo.pat.baddebt_date,	MCLLIVE.dbo.pat.batch_date,
		MCLLIVE.dbo.pat.bd_list_date,	MCLLIVE.dbo.pat.ebill_batch_date,
		MCLLIVE.dbo.pat.ebill_batch_1500,MCLLIVE.dbo.pat.e_ub_demand_date,
		MCLLIVE.dbo.pat.claimsnet_1500_batch_date,	MCLLIVE.dbo.pat.claimsnet_ub_batch_date ) IS NOT NULL
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
