create view [dbo].[Calendar]
as
with Dates as (
   select cast('2011-01-01' as datetime) as CalendarDate
   union all
   select dateadd(day , 1, CalendarDate) AS CalendarDate
   from Dates
   where dateadd (day, 1, CalendarDate)<= dateadd(year, 1, getdate())
)
select
   CalendarDate,
   CalendarYear=year(CalendarDate),
   CalendarQuarter=datename(quarter, CalendarDate),
   CalendarMonth=month(CalendarDate),
   CalendarWeek=datepart(wk, CalendarDate),
   CalendarDay=day(CalendarDate),
   CalendarMonthName=datename(month, CalendarDate),
   CalendarDayOfYear=datename(dayofyear, CalendarDate),
   Weekday=datename(weekday, CalendarDate),
   DayOfWeek=datepart(weekday, CalendarDate)
from Dates