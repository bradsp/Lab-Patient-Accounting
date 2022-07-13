-- =============================================
-- Author:		David
-- Create date: 10/27/2014
-- Description:	Gets the patients diagnosis into a table from a row
-- =============================================
CREATE FUNCTION GetPatDiagnosis
(
	-- Add the parameters for the function here
	@acc varchar(10) 
	 
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAccount varchar(15), 
	colDxNum INT,
	colDiagnosis VARCHAR(10),
	colIndicator varchar(10)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
; WITH cteIcd9
AS
(
	SELECT dbo.pat.account, icd9_1, icd9_2, icd9_3, icd9_4
	, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9
	, dbo.pat.icd_indicator
	FROM pat 
	WHERE pat.account = @acc
)
INSERT INTO @Table_Var (colAccount, colDxNum, colDiagnosis, colIndicator)

SELECT account, RIGHT(dx_num,1) AS [dx_num]
, diagnosis, icd_indicator
FROM 
(
SELECT account ,
		icd9_1 ,
		icd9_2 ,
		icd9_3 ,
		icd9_4 ,
		icd9_5 ,
		icd9_6 ,
		icd9_7 ,
		icd9_8 ,
		icd9_9 ,
		icd_indicator
		FROM cteIcd9
)pvt
UNPIVOT
( diagnosis FOR dx_num IN (icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9)
)
AS unpvt
	RETURN 
END
