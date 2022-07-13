-- =============================================
-- Author:		David
-- Create date: 08/28/2013
-- Description:	Update the account status based on charges containing BNP
-- =============================================
CREATE PROCEDURE [dbo].[usp_bnp_update] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/* accounts needing BNP hold */
; with cteNeedHold
as
(
	select acc.cl_mnem, chrg.account, chrg.pat_name, service_date  --, acc.status
	,sum(qty) over (partition by chrg.account, cdm) as [qty]
	 from chrg
	inner join acc on acc.account = chrg.account
	where cdm  in ('5325048','5325094','5322126')
	and chrg.account in 
	(
	 SELECT ACCOUNT FROM ACC WHERE FIN_CODE = 'A' and status in ('UBOP','UB','1500', 'new')
	) 

) 
select * from cteNeedHold where qty <> 0

/* update accounts needing BNP hold */
; with cteNeedHold
as
(
	select acc.cl_mnem, chrg.account, acc.pat_name, service_date--, acc.status
	,sum(qty) over (partition by chrg.account, cdm) as [qty]
	 from chrg
	inner join acc on acc.account = chrg.account
	where cdm in ('5325048','5325094','5322126')
	and chrg.account in 
	(
	 SELECT ACCOUNT FROM ACC WHERE FIN_CODE = 'A' and status in ('UPOP','UB','1500', 'new' )
--	 and not account in ('c3832711','C3833083')
	) 
	
) 

update acc
set status = 'BNP HOLD'
from acc
where acc.account in (select account from cteNeedHold where qty <> 0)


/* account needing BNP hold removed*/

; with cteNeedHoldRemoved
as
(
select acc.cl_mnem, chrg.account, acc.pat_name, chrg.service_date  --, acc.status
	,sum(qty) over (partition by chrg.account, cdm) as [qty]
from chrg
inner join acc on acc.account = chrg.account
where cdm in ('5325048','5325094','5322126')
and chrg.account in 
(
	SELECT ACCOUNT FROM ACC WHERE  fin_code = 'A' and status in ('BNP HOLD'))
	
)
select * from cteNeedHoldRemoved where qty = 0

; with cteRemoveHold
as
(
select acc.cl_mnem, chrg.account, acc.pat_name, chrg.service_date   --, acc.status
,sum(qty) over (partition by chrg.account, cdm) as [qty]
 from chrg
inner join acc on acc.account = chrg.account
where cdm in ('5325048','5325094','5322126')
and chrg.account in 
(
	SELECT ACCOUNT FROM ACC WHERE  fin_code = 'A' and status in ('BNP HOLD'))	
)
update acc
set status = 'NEW'
from acc
where acc.account in (select account from cteRemoveHold where qty = 0)

END
