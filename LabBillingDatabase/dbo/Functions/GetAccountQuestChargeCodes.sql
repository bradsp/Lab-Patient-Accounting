-- =============================================
-- Author:		Bradley Powers
-- Create date: 01/28/2014
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetAccountQuestChargeCodes] 
(	
	-- Add the parameters for the function here
	@account varchar(15),
	@cytohist VARCHAR(9) = '[CHFalse]'
	 
)
RETURNS TABLE 
AS
RETURN 
(
						
    -- Insert statements for procedure here
	with cteAcc as 
	( 
		select status, acc.account, datediff(year,dob_yyyy,trans_date) as [Age] from acc 
		left outer join pat on pat.account = acc.account
		where acc.account = @account

	) --SELECT * FROM cteAcc

	, cteChrg AS
	( 
		select cteAcc.status, chrg.account, chrg.chrg_num, qty, cdm , cpt4, [Age] 
		, convert(datetime,convert(varchar(10),service_date,101)) as [DOS] 
		, amt.uri from chrg 
		inner join cteAcc on cteAcc.account = chrg.account
		inner join amt on amt.chrg_num = chrg.chrg_num 
		where credited = 0 
			AND chrg.cdm <> 'CBILL'
			and not chrg.cdm in (select cdm from cdm where (cdm between '5520000' and '5527417' or cdm between '5527420' and '552ZZZZ')) AND
			((@cytohist='[CHTrue]' AND (cdm BETWEEN '5920000' AND '594ZZZZ' or cdm = '5686046')) OR
			(@cytohist='[CHFalse]' AND ((NOT cdm BETWEEN '5920000' AND '594ZZZZ') AND cdm <> '5686046')))
	)  

	select distinct 
		  cteChrg.status
		  , cteChrg.account
		  , cteChrg.chrg_num
		  , cteChrg.cdm
		  , cteChrg.qty
		  , cteChrg.dos 
		  , [Age] 
		  ,case when dd.cpt is null 
				then 'GAP'  
				else 
					  case when (age > 11 and age_appropriate = 1)  
					  then 'GAP' 
					  else 'EXCLUSION'  
				end 
		  end as [Bill Type] 
		  , coalesce(dt.quest_code,dt2.quest_code) as [Quest Code] 
		  , coalesce(dt.quest_description,dt2.quest_description) as [Quest Description] 
	from cteChrg 
	left outer join dict_quest_exclusions_final_draft dd 
		  on dd.cpt = cteChrg.cpt4 and outpatient_surgery = 0  
				and (cteChrg.dos >= dd.start_date  
				and cteChrg.dos <= isnull(dd.expire_date,getdate())) 

	left outer join dict_quest_reference_lab_tests dt 
		  on dt.cdm = cteChrg.cdm 
				and dt.has_multiples = 0 
				and dt.deleted = 0 
				and (cteChrg.dos >= dt.start_date  and cteChrg.dos <= isnull(dt.expire_date,getdate())) 

	left outer join dict_quest_reference_lab_tests dt2 
		  on  dt2.cdm = cteChrg.cdm  
				and dt2.link = cteChrg.qty 
				and dt2.has_multiples = 1 
				and dt2.deleted = 0 
				and (cteChrg.dos >= dt2.start_date  and cteChrg.dos <= isnull(dt2.expire_date,getdate())) 

	where 
		  case when dd.cpt is null 
		  then 'GAP'  
		  else 
				case when (age > 11 and age_appropriate = 1)  
				then 'GAP' 
				else 'EXCLUSION'  
				end 
		  end = 'GAP' 
	--order by [DOS],cteChrg.status, account, cdm
) 
