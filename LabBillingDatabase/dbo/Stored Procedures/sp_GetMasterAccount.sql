-- =============================================
-- Author:		David
-- Create date: 12/13/2013
-- Description:	Gets master account
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetMasterAccount] 
	-- Add the parameters for the stored procedure here
	@acc varchar(15) 
	  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
;with mainAcc (rn, account, dup_acc, Level)
as
(
	select row_number() over (order by account) as [RN],
    coalesce(account,@acc) as [account], dup_acc, 0 as Level from acc_merges as AM
	where not account in (select dup_acc from acc_merges)
	and mod_date > getdate()- 30
	union all
	select [rn],coalesce(AM1.account,@acc), AM1.dup_acc, Level+1 from acc_merges as AM1
	inner join mainAcc as MA on coalesce(AM1.account,@acc) = MA.dup_acc
	where mod_date > getdate()- 30

)
select distinct isnull(account,@acc)
from 
(
select coalesce(MA.ACCOUNT,@acc) as [account], MA.DUP_ACC, LEVEL from mainAcc as MA
inner join acc_merges as AM on AM.account = MA.account
where (AM.mod_date > getdate()-30) and (AM.dup_acc = @acc
or ( level = 0 and MA.RN = (select MA.RN from mainAcc as MA where dup_acc = @acc)))
) a
where level = 0
END
