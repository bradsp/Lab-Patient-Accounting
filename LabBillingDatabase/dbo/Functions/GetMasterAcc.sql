-- =============================================
-- Author:		David
-- Create date: 06/02/2014
-- Description:	Gets the Master Account from acc_merges
-- =============================================
CREATE FUNCTION GetMasterAcc 
(
	-- Add the parameters for the function here
	@acc varchar(15) 
	 
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAcc varchar(15)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	
	; WITH MainAcc
	AS
	(
	
	SELECT ROW_NUMBER() OVER (ORDER BY am.account,am.dup_acc) AS [RN],
	account, dup_acc, 0 as LEVEL
		from acc_merges as AM
	where dup_acc = @acc
	
	union all

	select ROW_NUMBER() OVER (ORDER BY MA.account,MA.dup_acc) AS [RN],
		AM1.account, AM1.dup_acc, Level+1
	from acc_merges as AM1
	inner join mainAcc as MA on MA.account = AM1.dup_acc
	
	)	
	INSERT INTO @Table_Var( colAcc )
	SELECT 	top(1) account 
	FROM MainAcc
	ORDER by LEVEL desc
	
			
	
	RETURN 
END
