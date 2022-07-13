-- =============================================
-- Author:		David
-- Create date: 05/08/2014
-- Description:	Get the individual amount records adjusting last record for balance
-- =============================================
CREATE FUNCTION GetAmt 
(
	-- Add the parameters for the function here
	@price numeric(18,2), 
	@num int
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colLink int, 
	colAmount numeric(18,2)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	;with cte as
(
	select top(@num +1) row_number() over (order by id) as rn	
	,@num as [number]
	,round(@price/@num,2) as amt
	 from dict_date
	
)
INSERT INTO @Table_Var
        ( colLink, colAmount )
select c1.rn as [link],
	case when c1.rn = @num
	then c1.amt + @price - (c1.amt *c1.rn)
	else c1.amt end as [amt]

from cte c1
cross join cte c2
where c2.rn-1 = c1.rn 

	RETURN 
END
