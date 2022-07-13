CREATE FUNCTION ArrayToTable
(  
@TheArray XML
)
RETURNS TABLE
AS
RETURN
(
SELECT   x.y.value('seqno[1]', 'INT') AS [seqno],
        x.y.value('item[1]', 'VARCHAR(200)') AS [item]
FROM     @TheArray.nodes('//stringarray/element') AS x (y)
)
