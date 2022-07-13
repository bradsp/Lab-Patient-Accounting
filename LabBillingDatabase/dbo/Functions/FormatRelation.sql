-- =============================================
-- Author:		David
-- Create date: 04/22/2014
-- Description:	Formats the imput into acceptable relations
-- =============================================
CREATE FUNCTION FormatRelation 
(
	-- Add the parameters for the function here
	@InputRelation varchar(15)
)
RETURNS varchar(2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(2)
	SELECT @InputRelation = REPLACE(@InputRelation,'"','')
	SELECT @InputRelation = SUBSTRING(@InputRelation,1,2)
	
	-- Add the T-SQL statements to compute the return value here
--	SELECT @Result = '09'
	SELECT @Result =  
		(select CASE WHEN @InputRelation IN ('01','02','03') then @InputRelation
					ELSE CASE WHEN @InputRelation IN ('','SE','SELF','OTHER','UNKNOWN') THEN '01'
						 ELSE CASE WHEN @InputRelation IN ('SP','SPOUSE') THEN '02'
							  ELSE CASE WHEN @InputRelation IN ('CH','CHILD') THEN '03'
							  ELSE '01'
									END
							  END
						END	
				    END )
	
	
	
	
	-- Return the result of the function
	RETURN @Result

END
