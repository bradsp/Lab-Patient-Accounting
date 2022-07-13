-- =============================================
-- Author:		David
-- Create date: 03/03/2016
-- Description:	Get any payment on an account
-- =============================================
CREATE FUNCTION ufn_GetAccountPayment 
(
	-- Add the parameters for the function here
	@account varchar(15)
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAccount varchar(15),
	colLink INT,
	colSource VARCHAR(50),
	colAmtPaid numeric(18,2),
	colContractual numeric(18,2),
	colWriteOff numeric(18,2),
	colBadDebt numeric(18,2)
	
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	INSERT INTO @Table_Var
			(
				colAccount ,
				colLink ,
				colSource ,
				colAmtPaid ,
				colContractual,
				colWriteOff ,
				colBadDebt 
			)
	SELECT 
	chk.account, 
	ROW_NUMBER() OVER (PARTITION BY chk.account
		ORDER BY chk.account) AS [Link],
	chk.source,
	SUM(amt_paid) AS [AmtPaid],
	SUM(contractual) AS [Contractual],
	CASE WHEN dbo.chk.bad_debt = 0
	THEN 	SUM(write_off)
	ELSE 0.00
	END AS [Write Off],
	CASE WHEN dbo.chk.bad_debt = 1
	THEN 	SUM(write_off)
	ELSE 0.00
	END AS [Bad Debt]
	FROM dbo.chk
	WHERE dbo.chk.account = @account
	GROUP BY chk.account, chk.source, chk.bad_debt
	ORDER BY chk.source
	
	IF(NOT EXISTS(SELECT colAccount 
		FROM @Table_Var WHERE [@Table_Var].colAccount = @account)
		AND (EXISTS (SELECT account 
		FROM chrg WHERE dbo.chrg.account 
		= @account AND chrg.cdm = 'CBILL')))
	BEGIN
		INSERT INTO @Table_Var
				(
				colAccount ,
				colLink ,
				colSource ,
				colAmtPaid ,
				colContractual,
				colWriteOff ,
				colBadDebt 
				)
		SELECT chrg.account,1,chrg.cdm,
		 SUM(chrg.calc_amt)*-1,0,0,0 
		 FROM chrg 
		 WHERE dbo.chrg.account = @account 
		 AND cdm = 'cbill'
		 GROUP BY chrg.account, chrg.cdm
		
	END
		
		
	RETURN 
END
