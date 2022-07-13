
CREATE FUNCTION dbo.GetAccMcloeDiagnosis
  (@acc VARCHAR(15)
  )
RETURNS @return_variable TABLE --table type definition
--WITH ENCRYPTION|SCHEMABINDING, ...
	(colAccount varchar(15),
	colDxNum int,
	colDiagnosis varchar(10))
AS
BEGIN
  -- Function body here
  INSERT INTO @return_variable
   ( colAccount, colDxNum, colDiagnosis)
   
  SELECT --MIN(dx.DxNum) AS dxnum,
  @acc 
  ,ROW_NUMBER() OVER (ORDER BY MIN(dx.DxNum)) AS [newDxNum]
  , dx.colDiagnosis
FROM
(
	SELECT 100+colDxNum AS DxNum, colDiagnosis 
	FROM dbo.GetPatDiagnosis(@acc)
	
	UNION ALL

--	SELECT 200+dx_number AS DxNum, diagnosis
--	FROM dbo.patdx WHERE account = @acc
SELECT --account, 
	200+ [patIcd].dx_num, [patIcd].icd9
FROM 
(
	SELECT --account,
	 ROW_NUMBER() OVER (PARTITION BY account ORDER BY account) AS [dx_num]
	, icd9
	FROM 
	(
		SELECT REPLACE(wreq.account,'L0','L') AS [account]
		,replace(wreq.icd9_1,'.','') AS icd9_1
		,REPLACE(wreq.icd9_2,'.','') AS icd9_2
		,REPLACE(wreq.icd9_3,'.','') AS icd9_3
		,REPLACE(wreq.icd9_4,'.','') AS icd9_4
		,REPLACE(wreq.icd9_5,'.','') AS icd9_5
		,REPLACE(wreq.icd9_6,'.','') AS icd9_6
		,REPLACE(wreq.icd9_7,'.','') AS icd9_7
		,REPLACE(wreq.icd9_8,'.','') AS icd9_8
		,REPLACE(wreq.icd9_9,'.','') AS icd9_9
		FROM mcloe.GOMCLLIVE.dbo.wreq
		WHERE REPLACE(wreq.account,'L0','L') = @acc
	) AS pat_icd9
	UNPIVOT 
	(
		icd9 FOR icd9s IN (icd9_1, icd9_2,icd9_3, icd9_4,	icd9_5, icd9_6, icd9_7, icd9_8, icd9_9)
	) AS UP
) [patIcd]
) dx
	GROUP BY dx.colDiagnosis
	ORDER BY MIN(dx.DxNum)
  RETURN
END
