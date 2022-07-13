-- =============================================
-- Author:		David
-- Create date: 09/24/2015
-- Description:	Get data from xml
-- =============================================
CREATE FUNCTION fn_Get_XMLCerner_data 
(
	-- Add the parameters for the function here
	@acc varchar(15) 
	--,@p2 char
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAcc varchar(15), 
	colClient varchar(10)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	INSERT INTO @Table_Var
			( colAcc, colClient )
	VALUES	( @acc, -- colAcc - varchar(15)
				'me'  -- colClient - varchar(10)
				)
	RETURN 
END
