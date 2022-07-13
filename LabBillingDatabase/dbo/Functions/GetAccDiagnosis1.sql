
CREATE FUNCTION dbo.GetAccDiagnosis1
  (@acc VARCHAR(15)
  )
RETURNS @return_variable TABLE --table type definition
--WITH ENCRYPTION|SCHEMABINDING, ...
	(
	colAccount varchar(15),
	colDxNum int,
	colDiagnosis varchar(10),
	colDescription varchar(150)
	)
AS
BEGIN
  -- Function body here
  INSERT INTO @return_variable
   ( colAccount, colDxNum, colDiagnosis, colDescription)
   
  SELECT --MIN(dx.DxNum) AS dxnum,
  @acc 
  ,ROW_NUMBER() OVER (ORDER BY MIN(dx.DxNum)) AS [newDxNum]
  , dx.colDiagnosis, dx.Description
FROM
(SELECT 100+colDxNum AS DxNum, colDiagnosis 
, dbo.icd9desc.icd9_desc AS [Description]
FROM dbo.GetPatDiagnosis(@acc) gpd
INNER JOIN dbo.icd9desc ON dbo.icd9desc.icd9_num = 
	gpd.colDiagnosis
UNION ALL
SELECT 200+dx_number AS DxNum, diagnosis
, dbo.icd9desc.icd9_desc AS [Description]
FROM dbo.patdx 
INNER JOIN dbo.icd9desc ON dbo.icd9desc.icd9_num
	= patdx.diagnosis
WHERE account = @acc


) dx
GROUP BY dx.colDiagnosis, dx.Description
ORDER BY MIN(dx.DxNum)
  RETURN
END
