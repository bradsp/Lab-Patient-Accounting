/*
Author: Leigh Haynes
Date: February 2015
Notes: Takes a table name as string parameter and returns a string that contains HTML markup to display the table contents as an HTML table.

The input table should be sorted before invoking HtmlTable.

*/
CREATE PROCEDURE [dbo].[usp_HtmlTable]
    @data_source varchar (100) = NULL,
    @tableHTML varchar(max) OUTPUT
AS

BEGIN    

SET NOCOUNT ON;

DECLARE 
    @db varchar(50), 
    @table varchar(100), 
    @cmd varchar(400), 
    @rcd_cnt int,
    @sql nvarchar(1000);

--use procedure DataSourceCheck to see if @data_source is valid
EXEC dbo.DataSourceCheck @data_source, @db output, @table output;

IF @db is NULL --if the data source is not good, @db comes back NULL, and @table holds info as to the problem (either the table does not exist, or it is empty).
BEGIN
    SET @tableHtml = @table;
    RETURN;
END;

--We have a good table. Use information_schema metadata for table to get column names.
IF OBJECT_ID ('tempdb..##columnNames') IS not null DROP TABLE ##columnNames;
CREATE table ##columnNames (column_name varchar(50), position int identity);

SET @sql = 'USE ' + @db + '; INSERT into ##columnNames SELECT column_name from information_schema.columns where table_name = ''' + @table + ''' order by ordinal_position';
EXEC master.sys.sp_executesql @sql;

--use ##columnNames to create table ##columnPivot with the proper number of fields to hold data
IF OBJECT_ID ('tempdb..##columnPivot') IS not null DROP TABLE ##columnPivot;
CREATE table ##columnPivot (f1 varchar(200));

DECLARE @i int;
SET @i = 2;
DECLARE @fieldct INT;
DECLARE @column varchar(50); 
DECLARE @field varchar(200);
DECLARE @value varchar(100);
DECLARE @html varchar(max);
SET @html = '';

SET @fieldct = (SELECT COUNT(*) from ##columnNames);
WHILE @i <= @fieldct --loop through adding a field to ##columnPivot for each column. Max field len is 200.
BEGIN
    SET @sql = 'ALTER table ##columnPivot ADD f' + cast (@i as varchar(2)) + ' varchar(200)';
    EXEC master.sys.sp_executesql @sql;
    SET @i = @i + 1;
END
--##columnPivot is constructed but empty. Columns are named f1, f2, f3, etc

--construct dynamic SQL string that will be executed to populate ##columnPivot
SET @sql = 'INSERT into ##columnPivot SELECT ';
SET @i = 1;
SET @fieldct = (SELECT count(*) from ##columnNames);

WHILE @i <= @fieldct - 1
BEGIN
    SET @column = (SELECT top 1 column_name from ##columnNames where position = cast (@i as varchar(2)));
    SET @field = 'CAST([' + @column + '] as varchar(200)),';
    SET @sql = @sql + @field;
    SET @i = @i + 1;
END

SET @column = (SELECT top 1 column_name from ##columnNames where position = @fieldct);
SET @field = 'CAST([' + @column + '] as varchar(200)) FROM ' + @data_source;
SET @sql = @sql + @field; --@sql now contains the SQL statement that will insert data from @data_source into ##columnPivot

--execute @sql to insert into ##columnPivot the data from @data_source table
EXEC master.sys.sp_executesql @sql;

--format the output
IF OBJECT_ID ('tempdb..#columns') IS not null DROP TABLE #columns;
--use a copy of ##columnNames, because next steps delete from this table, and ##columnNames data is needed below. Does not need to be a global temp.
SELECT *
into #columns
from ##columnNames
order by position;

SET @fieldct = (SELECT count(*) from #columns);
SET @i = 1;

--create the header row for the table containing column names from the @data_source
WHILE @i <= @fieldct 
BEGIN
    SET @field = (SELECT top 1 column_name from #columns order by position);
    SET @html = @html + '<td bgcolor="#dedede"><b>' + @field + '</b></td>';
    SET @i = @i + 1;
    DELETE from #columns where column_name = @field;
END

SET @html = '<tr>' + @html + '</tr>'; --now @html contains the header row of the output table


--populate ##columnPivot by working through the data row by row. 
ALTER table ##columnPivot add id_key int identity;

DECLARE @j INT;
SET @j = 1;
DECLARE @fieldcnt INT;
DECLARE @cell varchar(100);
DECLARE @row varchar(500);
SET @row = '';

SET @i = 1;
SET @fieldct = (SELECT count(*) from ##columnNames);
SET @rcd_cnt = (SELECT count(*) from ##columnPivot);

WHILE @i <= @rcd_cnt --this loop executes one time for each row of data
BEGIN
    SET @j = 1;
    WHILE @j <= @fieldcnt --this loop executes one time for each column (cell) of data
    BEGIN
        SET @sql = 'SELECT @value = f' + cast (@j as varchar(2)) + ' from ##columnPivot where id_key = ' + cast (@i as varchar(2));
        EXEC master.sys.sp_executesql @sql, N'@value varchar(200) OUTPUT', @value OUTPUT;
        SET @cell = '<td>' + ISNULL (@value, '<br>') + '</td>'; --need to use <br> if the cell is empty
        SET @row = @row + @cell;
        SET @j = @j + 1;
    END
    SET @row = '<tr>' + @row + '</tr>';     
    SET @html = @html + @row;
    SET @row = '';
    DELETE from ##columnPivot where id_key = cast (@i as varchar(2));
    SET @i = @i + 1;
END

SET @tableHTML = '<table border="1" cellspacing="0" cellpadding="5">' + @html + '</table><br>'; 

END