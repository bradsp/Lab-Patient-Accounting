
--SELECT * FROM  dbo.GenerateCalendar (getdate(),30)

CREATE FUNCTION [dbo].[GenerateCalendar] 
        (
        @FromDate   DATETIME 
        ,@NoDays    INT			
        )
-- Generates a calendar table with sequential day numbering (@FromDate = SeqNo 1).
-- See RETURNS table (comments) for meaning of each column.
-- Notes:       1) Max for NoDays is 65536, which runs in just over 2 seconds.
--
-- Example calls to generate the calendar:
-- 1) Forward for 365 days starting today:
--             DECLARE @Date DATETIME
--             SELECT @Date = GETDATE()
--             SELECT * 
--             FROM dbo.GenerateCalendar(@Date, 365) 
--             ORDER BY SeqNo;
-- 2) Backwards for 365 days back starting today:
--             DECLARE @Date DATETIME
--             SELECT @Date = GETDATE()
--             SELECT * 
--             FROM dbo.GenerateCalendar(@Date, -365) 
--             ORDER BY SeqNo;
-- 3) For only the FromDate:
--             DECLARE @Date DATETIME
--             SELECT @Date = GETDATE()
--             SELECT * 
--             FROM dbo.GenerateCalendar(@Date, 1);
-- 4) Including only the last week days of each month:
--             Note: Seq no in this case are as if all dates were generated
--             DECLARE @Date DATETIME
--             SELECT @Date = GETDATE()
--             SELECT * 
--             FROM dbo.GenerateCalendar(@Date, 365) 
--             WHERE Last = 1 ORDER BY SeqNo;
RETURNS TABLE WITH SCHEMABINDING AS
 RETURN
--===== High speed code provided courtesy of SQL MVP Jeff Moden (idea by Dwain Camps)
--===== Generate sequence numbers from 1 to 65536 (credit to SQL MVP Itzik Ben-Gen)
   WITH  E1(N) AS (SELECT 1 UNION ALL SELECT 1), --2 rows
         E2(N) AS (SELECT 1 FROM E1 a, E1 b),    --4 rows
         E4(N) AS (SELECT 1 FROM E2 a, E2 b),    --16 rows
         E8(N) AS (SELECT 1 FROM E4 a, E4 b),    --256 rows
        E16(N) AS (SELECT 1 FROM E8 a, E8 b),    --65536 rows
   cteTally(N) AS (
SELECT TOP (ABS(@NoDays)) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM E16)
        -- [SeqNo]=Sequential day number (@FromDate always=1) forward or backwards
 SELECT [SeqNo]     = t.N,
        -- [Date]=Date (with 00:00:00.000 for the time component)                              
        [Date]      = dt.DT,  
        -- [Year]=Four digit year                                  
        [Year]      = dp.YY,
        -- [YrNN]=Two digit year                                    
        [YrNN]      = dp.YY % 100,
        -- [YYYYMM]=Integer YYYYMM (year * 100 + month)                              
        [YYYYMM]    = dp.YY * 100 + dp.MM,
        -- [BuddhaYr]=Year in Buddhist calendar                      
        [BuddhaYr]  = dp.YY + 543, 
        -- [Month]=Month (as an INT)                             
        [Month]     = dp.MM, 
        -- [Day]=Day (as an INT)                                   
        [Day]       = dp.DD,
        -- [WkDNo]=Week day number (based on @@DATEFIRST)                                    
        [WkDNo]     = DATEPART(dw,dt.DT),
        -- Next 3 columns dependent on language setting so may not work for non-English  
        -- [WkDName]=Full name of the week day, e.g., Monday, Tuesday, etc.                     
        [WkDName]   = CONVERT(NCHAR(9),dp.DW), 
        -- [WkDName2]=Two characters for the week day, e.g., Mo, Tu, etc.                 
        [WkDName2]  = CONVERT(NCHAR(2),dp.DW),  
        -- [WkDName3]=Three characters for the week day, e.g., Mon, Tue, etc.                
        [WkDName3]  = CONVERT(NCHAR(3),dp.DW),  
        -- [JulDay]=Julian day (day number of the year)                
        [JulDay]    = dp.DY,
        -- [JulWk]=Week number of the year                                    
        [JulWk]     = dp.DY/7+1,
        -- [WkNo]=Week number                                
        [WkNo]      = dp.DD/7+1,
        -- [Qtr]=Quarter number (of the year)                                
        [Qtr]       = DATEPART(qq,dt.Dt),                       
        -- [Last]=Number the weeks for the month in reverse      
        [Last]      = (DATEPART(dd,dp.LDtOfMo)-dp.DD)/7+1,
        -- [LdOfMo]=Last day of the month                  
        [LdOfMo]    = DATEPART(dd,dp.LDtOfMo),
        -- [LDtOfMo]=Last day of the month as a DATETIME
        [LDtOfMo]   = dp.LDtOfMo                                
   FROM cteTally t
  CROSS APPLY 
  ( --=== Create the date
        SELECT DT = DATEADD(dd,(t.N-1)*SIGN(@NoDays),@FromDate)
  ) dt
  CROSS APPLY 
  ( --=== Create the other parts from the date above using a "cCA"
    -- (Cascading CROSS APPLY (cCA), courtesy of Chris Morris)
        SELECT YY        = DATEPART(yy,dt.DT), 
                MM        = DATEPART(mm,dt.DT), 
                DD        = DATEPART(dd,dt.DT), 
                DW        = DATENAME(dw,dt.DT),
                Dy        = DATEPART(dy,dt.DT),
                LDtOfMo   = DATEADD(mm,DATEDIFF(mm,-1,dt.DT),-1)

  ) dp;
