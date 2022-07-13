-- =============================================
-- Author:		David
-- Create date: 12/28/2011
-- Description:	Create OutReach Costs Summary Report
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_OutReach_Cost] 
	-- Add the parameters for the stored procedure here
	@startDate datetime = 0, 
	@endDate datetime = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT @startDate, @endDate

select [client] as [Supply User],[test] as [type],
[JAN],[FEB],[MAR],[APR],[MAY],[JUN],[JUL],[AUG],[SEP],[OCT],[NOV],[DEC]
from
(

select  left(datename(month,chrg.mod_date),3) as [Month]
		, case 
			when chrg.cdm between '5520000' and '5529999'
				then 'Supplies'
				else 'Medical Services (Reference)'
			end as 'Test'
,  case 	when client.type = '7' 
				then '17001 Nursing Homes'
				else
				
			case when cli_mnem in ('DMG','DMGH','MSH','MSHH')
				then '17002 Medsouth Dyersburg'
				else 
			case 
				when cli_mnem = 'PCC'
				then '17003 Primary Care Clinic'
				else
			case when cli_mnem = 'RMC'
				then '17005 Ripley Medical Clinic'
				else 
			case when cli_mnem = 'RCL'
				then '17004 Rhea Medical Clinic'
				else
			case when cli_mnem = 'WC'
				then '17006 Woman''s Clinic'
				else
				'17009 Lab Outreach Primary'
			end			end			end			end			end		
			end as [client]
	
	, (isnull(cdm.cost,0)*sum(qty)) as [Extend Cost]
from chrg
left outer join cdm on cdm.cdm = chrg.cdm
left outer join acc on acc.account = chrg.account
left outer join client on client.cli_mnem = acc.cl_mnem
where chrg.chrg_num in (select chrg_num from amt where mod_date between @startDate and @endDate)
and chrg.cdm <> 'cbill'
group by cli_mnem,datename(month,chrg.mod_date),-- datepart(year, trans_date) ,
 chrg.cdm
, client.type, cdm.cost
--having sum(qty) <> 0
--order by cli_mnem, datename(month,trans_date)-- , datepart(year, trans_date) , chrg.cdm
) a
pivot
(
	sum([Extend Cost])
	for [Month] in ([JAN],[FEB],[MAR],[APR],[MAY],[JUN],[JUL],[AUG],[SEP],[OCT],[NOV],[DEC])
)
as p
order by [client], [type] 
END
