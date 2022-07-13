
CREATE FUNCTION dbo.GetAccDiagnosis
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
(SELECT 100+colDxNum AS DxNum, colDiagnosis 
FROM dbo.GetPatDiagnosis(@acc)
UNION ALL
SELECT 200+dx_number AS DxNum, diagnosis
FROM dbo.patdx WHERE account = @acc) dx
GROUP BY dx.colDiagnosis
ORDER BY MIN(dx.DxNum)
  RETURN
END
