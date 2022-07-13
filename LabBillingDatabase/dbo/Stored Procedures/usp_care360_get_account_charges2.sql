-- =============================================
-- Author:		Bradley Powers
-- Create date: 11/7/2013
-- Description:	Gets the Care360 charges for an account
-- =============================================
CREATE PROCEDURE [dbo].[usp_care360_get_account_charges2] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	with cteAcc as 
	( 
	select status, acc.account, datediff(year,dob_yyyy,trans_date) as [Age] from acc 
	left outer join pat on pat.account = acc.account
	where acc.account in (select account from data_quest_360 where bill_type = 'Q' and entered = 0)
	--fin_code = 'Y' and trans_date between '10/1/2012 00:00' and '10/23/2013 23:59:59' and status = 'QUEST' 

	) 

	, cteChrg as ( 
	select cteAcc.status, chrg.account, chrg.chrg_num, qty, cdm , cpt4, [Age] 
	, convert(datetime,convert(varchar(10),service_date,101)) as [DOS] 
	, amt.uri from chrg 
	inner join cteAcc on cteAcc.account = chrg.account
	inner join amt on amt.chrg_num = chrg.chrg_num 
	where credited = 0 and coalesce(invoice ,'') = '' 
		and not chrg.cdm in (select cdm from cdm where (cdm between '5520000' and '5527417' or cdm between '5527420' and '552ZZZZ'))) 

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
	order by [DOS],cteChrg.status, account, cdm

END
