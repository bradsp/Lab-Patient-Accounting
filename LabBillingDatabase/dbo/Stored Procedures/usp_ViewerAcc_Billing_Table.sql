-- =============================================
-- Author:		David
-- Create date: 13/11/2013
-- Description:	fill billing table for ViewerAcc
-- =============================================
CREATE PROCEDURE usp_ViewerAcc_Billing_Table 
	-- Add the parameters for the stored procedure here
	@acc varchar(15) = 0 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
with cte as 
( 
	select  chrg.account, sum(qty) over (partition by chrg.account,cpt4,modi,modi2)  as [qty]
	, cpt4, type, sum(amount)  over (partition by chrg.account,cpt4,modi,modi2 )as [amount]  
	, case when modi <> ''  then modi  
		else  
		case when lmrp is null  
			then null 
			else  
			case when lmrp = 0  then 'GA' else 'GZ' end  end end as modi 
, revcode, modi2, diagnosis_code_ptr 
 from chrg   
inner join amt on amt.chrg_num = chrg.chrg_num   
left outer join abn on abn.account = chrg.account and chrg.cdm = abn.cdm  
where chrg.account = @acc  and chrg.cdm <> 'CBILL' and credited = 0  
) 
select * from cte 
where qty <> 0 and qty*amount > 0 
order by account, cpt4, modi, modi2


end