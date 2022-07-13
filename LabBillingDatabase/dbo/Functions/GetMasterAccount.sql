-- =============================================
-- Author:		David
-- Create date: 06/03/2014
-- Description:	Get the master account using a recursive CTE
-- =============================================
CREATE FUNCTION [dbo].[GetMasterAccount] 
(
	-- Add the parameters for the function here
	@acc varchar(25)
)
RETURNS @Table_Var TABLE
( colAcc varchar(15)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	SET @acc = dbo.AccountTrim(@acc)
	INSERT INTO @table_Var (colAcc)
	SELECT COALESCE(AM.account, @acc) FROM dbo.acc_merges AM
		WHERE AM.dup_acc = @acc	
	
	
	RETURN 
END
