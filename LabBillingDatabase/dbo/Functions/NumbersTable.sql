CREATE FUNCTION [dbo].[NumbersTable] (
   @fromNumber int,
   @toNumber int,
   @byStep int
 ) RETURNS TABLE
 RETURN (

 WITH CTE_NumbersTable AS (
   SELECT @fromNumber AS i
   UNION ALL
   SELECT i + @byStep
   FROM CTE_NumbersTable
   WHERE
   (i + @byStep) <= @toNumber
 )
 SELECT * FROM CTE_NumbersTable

 ) 