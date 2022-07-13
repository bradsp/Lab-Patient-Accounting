-- =============================================
-- Author:		David
-- Create date: 01/28/2016
-- Description:	Gets the Quest error codes
-- =============================================
CREATE FUNCTION ufn_GetQuestError 
(
	-- Add the parameters for the function here
	@acc varchar(15),
	@startDate DATETIME,
	@endDate DATETIME
	
)
RETURNS varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(50)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = @acc

;with cteAcc as (  
SELECT colStatus AS [status],colAccount AS [account], 
colAge AS [Age], CASE WHEN colBillingtype = 'OUTPATIENT' 
THEN 'UBOP'  
ELSE '1500' 
END AS [SSI type] 
,colFinCode 
FROM ufn_quest_processing(@startDate, @endDate) ufn
WHERE colStatus = 'QUEST' 
AND ufn.colAccount = @acc)  
 , cteChrg as 
 (  
	 select cteAcc.status, chrg.account, chrg.chrg_num, qty, cdm , cpt4, [Age]  
	 , convert(datetime,convert(varchar(10),service_date,101)) as [DOS]  
	 , amt.uri  
	 , [SSI type]   
	 from chrg  
	 inner join cteAcc on cteAcc.account = chrg.account  
	 inner join amt on amt.chrg_num = chrg.chrg_num  
	 where credited = 0 and (invoice is null or invoice = '')  
	 and not chrg.cdm in (select cdm from cdm where  
	 (cdm between '5520000' and '5527417'  
	 or cdm between '5527420' and '552ZZZZ') 
	 )
 )
 SELECT  @Result = (  
 select distinct TOP(1) 
-- cteChrg.status, cteChrg.account, cteChrg.chrg_num, cteChrg.cdm
-- , cteChrg.cpt4  
-- , cteChrg.qty, cteChrg.dos  
-- , [Age]  
-- ,case when dd.cpt is null  
-- then 'GAP'  
--  else case when (age > 11 and age_appropriate = 1)  
--  then 'GAP' else  
-- 'EXCLUSION'  
--  end  
-- end as [Bill Type] , 
  coalesce(dt.quest_code,dt2.quest_code) as [Quest Code]  
-- , coalesce(dt.quest_description,dt2.quest_description) as [Quest Description]  
-- , cteChrg.uri as [amt_uri] 
-- , [SSI type] 
 from cteChrg  
 left outer join dict_quest_exclusions_final_draft dd on dd.cpt = cteChrg.cpt4 and outpatient_surgery = 0  
  and (cteChrg.dos >= dd.start_date  and cteChrg.dos <= isnull(dd.expire_date,getdate()))   
 left outer join dict_quest_reference_lab_tests dt on dt.cdm = cteChrg.cdm and dt.has_multiples = 0 and dt.deleted = 0  
 and (cteChrg.dos >= dt.start_date   
  and cteChrg.dos <= isnull(dt.expire_date,getdate()))  
 left outer join dict_quest_reference_lab_tests dt2 on  dt2.cdm = cteChrg.cdm  and dt2.link = cteChrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0  
 and (cteChrg.dos >= dt2.start_date   
 and cteChrg.dos <= isnull(dt2.expire_date,getdate()))  
 where case when dd.cpt is null then 'GAP'  
  else case when (age > 11 and age_appropriate = 1)  
  then 'GAP' else 'EXCLUSION' end end = 'GAP'  
-- order by [DOS],cteChrg.status, account, cdm, cteChrg.cpt4 
)


	-- Return the result of the function
	RETURN COALESCE(@Result,'NO QUEST CODE')

END
