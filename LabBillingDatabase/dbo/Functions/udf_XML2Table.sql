--
--CREATE TABLE tblPropAcc (propPK int
--, propClient VARCHAR(10)
--, propFinCode VARCHAR(10))
--
--SELECT * FROM tblPropAcc

CREATE FUNCTION udf_XML2Table(@pk INT, @xCol XML )
RETURNS TABLE WITH SCHEMABINDING
AS RETURN
(
SELECT @pk AS PropPk
, nref.value('(//PV1.3.4/text() )[1]','varchar(10)') AS [propClient]
, nref.value('(//PV1.20.1/text() )[1]','varchar(10)') AS [propFinCode]
FROM @xCol.nodes('//HL7Message') R(nref)
)
