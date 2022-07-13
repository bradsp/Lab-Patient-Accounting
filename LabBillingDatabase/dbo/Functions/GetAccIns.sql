-- =============================================
-- Author:		David
-- Create date: 01/07/2015
-- Description:	Gets the insurance code for an account
-- =============================================
CREATE FUNCTION GetAccIns 
(
	-- Add the parameters for the function here
	@acc varchar(15) 
	 
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAcc varchar(15), 
	colIns varchar(10)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	
	WITH cteAccIns
	AS
	(
	SELECT acc.account AS [ACCOUNT], acc.fin_code, acc.trans_date, acc.status
	, CASE WHEN acc.fin_code IN ('CLIENT','W','X','Y','Z') 
			OR ins.ins_code IN ('CLIENT','W','X','Y','Z') then 'CLIENT'
		   WHEN COALESCE(NULLIF(ins.ins_code,''),acc.fin_code) = 'E' 
			OR ins.ins_code = 'SP' THEN 'SP'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'B' 
			OR ins.ins_code = 'BC' THEN 'BC'		   
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'A' 
		    OR ins.ins_code = 'MC' THEN 'MC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'D' 
			OR ins.ins_code  = 'TNBC' THEN 'TNBC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'M' 
			OR ins.ins_code = 'AM' THEN 'AM'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'S'
		    OR ins.ins_code = 'MISC' THEN 'MISC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'H' 
			OR ins.ins_code = 'COMM.H' THEN 'COMM.H'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'L' 
			OR ins.ins_code = 'COMM.L' THEN 'COMM.L'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'AETNA' THEN 'AETNA'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'UHC' THEN 'UHC'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'AM' THEN 'AM'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'CIGNA' THEN 'CIGNA'
		   WHEN ISNULL(ins.ins_code,acc.fin_code) = 'TNBC' THEN 'TNBC'
		   else 'OTHER' END AS ins_code
		   , CASE WHEN bd.account_no IS NOT NULL AND bd.date_sent IS NOT NULL
			THEN 'true' 
			ELSE NULL END AS [bad debt]
			, CASE WHEN bd.account_no IS NOT NULL AND bd.date_sent IS NULL
			THEN 'true' 
			ELSE NULL END AS [pre bad debt]
			
		FROM dbo.acc
		LEFT OUTER JOIN ins ON dbo.ins.account = dbo.acc.account AND ins.ins_a_b_c = 'A'
		LEFT OUTER JOIN dbo.bad_debt bd ON bd.account_no = acc.account 
			--AND dbo.bd.date_sent IS NOT null
		WHERE acc.account = @acc
	)
	INSERT INTO @Table_Var (colAcc, colIns)
	SELECT @acc, ins_code from cteAccIns
	
	RETURN 
END
