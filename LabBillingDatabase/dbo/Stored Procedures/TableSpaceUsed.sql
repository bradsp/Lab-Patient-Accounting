CREATE PROCEDURE dbo.TableSpaceUsed
AS

-- Create the temporary table...
CREATE TABLE #tblResults
(
   [name]   nvarchar(20),
   [rows]   int,
   [reserved]   varchar(18),
   [reserved_int]   int default(0),
   [data]   varchar(18),
   [data_int]   int default(0),
   [index_size]   varchar(18),
   [index_size_int]   int default(0),
   [unused]   varchar(18),
   [unused_int]   int default(0)
)


-- Populate the temp table...
EXEC sp_MSforeachtable @command1=
         "INSERT INTO #tblResults
           ([name],[rows],[reserved],[data],[index_size],[unused])
          EXEC sp_spaceused '?'"
   
-- Strip out the " KB" portion from the fields
UPDATE #tblResults SET
   [reserved_int] = CAST(SUBSTRING([reserved], 1, 
                             CHARINDEX(' ', [reserved])) AS int),
   [data_int] = CAST(SUBSTRING([data], 1, 
                             CHARINDEX(' ', [data])) AS int),
   [index_size_int] = CAST(SUBSTRING([index_size], 1, 
                             CHARINDEX(' ', [index_size])) AS int),
   [unused_int] = CAST(SUBSTRING([unused], 1, 
                             CHARINDEX(' ', [unused])) AS int)
   
-- Return the results...
SELECT * FROM #tblResults

-- drop the temporary table
DROP TABLE #tblResults