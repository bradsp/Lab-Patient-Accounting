-- =============================================
-- Author:		David
-- Create date: 01/26/2016
-- Description:	Checks an account for LMRP errors
-- =============================================
CREATE FUNCTION usf_AccCheckForLMRP 
(
	-- Add the parameters for the function here
	@acc varchar(15)
)
RETURNS VARCHAR(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result VARCHAR(50)
	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = 'LMRP ERROR'
	
	IF (NOT EXISTS(SELECT account FROM acc 
		WHERE account = @acc AND fin_code IN ('A')))
	BEGIN
		RETURN NULL
	END 
	
	IF (NOT EXISTS(SELECT dictionary.lmrp.ama_year
		FROM dictionary.lmrp
		WHERE dictionary.lmrp.ama_year IN 
		(SELECT dbo.GetAMAYear(acc.trans_date) FROM dbo.acc
		WHERE account = @acc AND fin_code IN ('A'))))
	BEGIN
		RETURN @Result
	END 
	
--DECLARE @acc VARCHAR(15)
--SET @acc = 'L5071099'

; WITH cte 
AS
(

SELECT DISTINCT acc.account,
dbo.vw_lmrp.cpt4 ,
		dbo.vw_lmrp.beg_icd9 ,
		dbo.vw_lmrp.end_icd9 ,
		dbo.vw_lmrp.lmrp ,
		dbo.vw_lmrp.lmrp2 ,
		dbo.vw_lmrp.rb_date2 ,
		dbo.vw_lmrp.chk_for_bad ,
		dbo.vw_lmrp.ama_year ,
		dbo.vw_lmrp.expire_date 
FROM dbo.vw_lmrp
INNER JOIN acc 
		ON dbo.GetAMAYear(acc.trans_date)=	dbo.vw_lmrp.ama_year 		
INNER JOIN (select chrg.account, amt.cpt4
            from chrg
            inner join amt on amt.chrg_num = chrg.chrg_num
            WHERE chrg.account = @acc
            GROUP BY chrg.account, amt.cpt4
            HAVING SUM(dbo.chrg.qty) > 0) ca
            ON ca.account = dbo.acc.account AND ca.cpt4 = dbo.vw_lmrp.cpt4
WHERE dbo.acc.account = @acc
AND acc.fin_code = 'A'
AND   COALESCE(dbo.vw_lmrp.expire_date,GETDATE() ) > acc.trans_date 
AND dbo.vw_lmrp.rb_date <= acc.trans_date 
		
)
SELECT @Result = ( 
SELECT TOP(1) 
	--cte.account --,
--		cte.cpt4 --,
--		cte.beg_icd9 ,
--		cte.end_icd9 ,
		CASE WHEN cte.lmrp IS NOT NULL THEN 'LMRP ERROR' 
			 WHEN gad.colDiagnosis IS NULL THEN 'NO DIAGNOSIS'
			 ELSE null END --,
		--cte.lmrp2 ,
--		cte.rb_date2 ,
--		cte.chk_for_bad ,
--		cte.ama_year ,
--		cte.expire_date ,
--		gad.colDiagnosis
		
FROM cte
CROSS APPLY dbo.GetAccDiagnosis(cte.account) gad 
--WHERE cte.chk_for_bad = 1

)

	
	-- Return the result of the function
	RETURN COALESCE(@Result,'NO DIAGNOSIS')

END
