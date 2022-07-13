-- =============================================
-- Author:		David
-- Create date: 07/24/2013
-- Description:	Quest CBILL accounts needing attention
-- =============================================
CREATE PROCEDURE usp_quest_cbill 
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
;with cte
as
(
select --account
case when chrg.account = 'QUESTR'
	then substring(chrg.comment -- expression
			,charindex('[',chrg.comment,0)+1 -- start
			,charindex(']',chrg.comment,0)-charindex('[',chrg.comment,0))
	else chrg.account
	end as [account]
, cdm, 
case when account = 'QUESTR' 
	then sum(qty*net_amt) over (partition by account, cdm, comment)
	else sum(qty*net_amt) over (partition by account, cdm)
end  as [charge] 
, convert(varchar(10),invoice) + ' - '+comment as [notes]
, service_date as [DOS]
from chrg 
where coalesce(isnull(invoice,''),invoice) = '' and service_date between @startDate and @endDate
and cdm <> 'CBILL'
and chrg.account in (select account from acc where cl_mnem = 'QUESTR')
) --select * from cte
, cteQB
as
(
	SELECT  distinct req_no, REPLACE(account,'c','q') AS [ACCOUNT]
		, Patient
		, collection_date
		, DOB
		, data.quest_code
		, data.quest_desc
		, dict.cdm
		, date_entered
	FROM data_quest_billing data
	inner join dict_quest_reference_lab_tests dict on dict.quest_code = data.quest_code and data.deleted = 0
	
)
, cteQBCredit
as
(
	SELECT  distinct req_no, REPLACE(account,'c','q') AS [ACCOUNT]
		, Patient
		, collection_date
		, DOB
		, data.quest_code
		, data.quest_desc
		, dict.cdm
		, date_entered
	FROM data_quest_billing data
	inner join dict_quest_reference_lab_tests dict on dict.quest_code = data.quest_code and data.deleted = 1
	
)

select --distinct 

--replace(cte.account,']','CR') as [PID],
replace(cte.account,']','CR') as [ACCOUNT]
, 'ERROR' as [status]
, coalesce(cteQB.cdm, cteQBCredit.cdm,cte.cdm) as [CDM]
, coalesce(cteQB.patient, cteQBCredit.patient) as [pat_name]
, coalesce(cteQB.req_no, cteQBCredit.req_no) as [req_no]
, convert(varchar(10),coalesce(cteQB.collection_date, cteQBCredit.collection_date,cte.dos),101) as [trans_date]
, coalesce(cteQB.DOB, cteQBCredit.DOB) as [Date of Birth]
, coalesce(cteQB.quest_code, cteQBCredit.quest_code) as [quest_code]
, coalesce(cteQB.quest_desc, cteQBCredit.quest_desc) as [quest_desc]
--, coalesce(cteQB.charges, cteQBCredit.charges) as [Charges]
,cte.charge
--, coalesce(cteQB.notes, cteQBCredit.notes) as [Notes]
,cte.notes
from cte 
left outer join cteQB on cteQB.account = cte.account and cteQB.cdm = cte.cdm
left outer join cteQBCredit on cteQBCredit.account+']' = cte.account and cteQBCredit.cdm = cte.cdm
where [charge] <> 0 
and coalesce(cteQB.cdm, cteQBCredit.cdm)  is null
order by cte.dos,cte.account

END
