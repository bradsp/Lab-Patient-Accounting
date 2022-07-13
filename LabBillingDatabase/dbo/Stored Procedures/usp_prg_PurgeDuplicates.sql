-- =============================================
-- Author:		David
-- Create date: 06/25/2014
-- Description:	Remove the duplicate diagnois from the patdx table
-- =============================================
CREATE PROCEDURE usp_prg_PurgeDuplicates 
	-- Add the parameters for the stored procedure here
	@file varchar(128) = NULL
	--, 	@p2 int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- Insert statements for procedure here
	--SELECT @file--, @p2
; with cteDx
as
(
select account, StartDx=MIN(dx_number), EndDx=MAX(dx_number)--, diagnosis
from
	(
		select account, dx_number, rn= dx_number-row_number() over (partition by account order by dx_number)		
		from patdx
		--WHERE import_file = @file
	) dx
group by account, rn
)
--select a.*, patdx.account, patdx.dx_number 
update patdx
set dx_number = a.StartDx
from (
select distinct  a.account,a.StartDx-1 as [StartDx], a.StartDx as [dx_number]

from cteDx a
cross join cteDx b 
where (a.account = b.account and a.StartDx > b.StartDx 
)
) a
inner join patdx on patdx.account = a.account and patdx.dx_number = a.dx_number

;with cte
as
(
select row_number() over (partition by deleted,account,diagnosis,version  order by dx_number) as [rn]
,account, deleted, diagnosis, version, dx_number, [uid]
from patdx

)
delete from patdx
--select * from cte
where uid in (
select uid from cte
where rn > 1)
    
END
