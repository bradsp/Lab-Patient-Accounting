CREATE FUNCTION [dbo].[GetNumbersTable] (
   @toNumber int
 ) RETURNS TABLE

 RETURN (



SELECT TOP (select case when @toNumber > 2147483647 THEN 2147483647 ELSE @toNumber end) ROW_NUMBER() OVER (ORDER BY a.[object_id])  AS colRow
FROM sys.columns a
CROSS JOIN sys.columns b

 ) 
