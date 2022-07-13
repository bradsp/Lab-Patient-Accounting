-- =============================================
-- Author:		David
-- Create date: 10/03/2013
-- Description:	Check for duplicate accounts
-- =============================================
CREATE FUNCTION [dbo].[ufn_Duplicates] 
(
	-- Add the parameters for the function here
	@service_date [datetime],
	@fin_code [varchar](10) ,
	@pat_name [varchar](100),
	@pat_ssn [varchar](100),
	@unitno [varchar](50),
	@pat_dob [datetime]--,
--	@age_on_date_of_service  int 
)
RETURNS 
@Table_Var TABLE -- Add the column definitions for the TABLE variable here
(
	account varchar(15) 
)
AS
BEGIN 	-- Fill the table variable with the rows for your result set
	declare @count int
	set @count =  (select count(fin_code) from fin where fin_code = @fin_code)
	if (@count = 0)
	begin 
		set @fin_code = 'CLIENT'
	end
	;with cte as
	(
	SELECT account
			, service_date
			, fin_code
			, ltrim(rtrim(pat_name)) as [pat_name]
			, replace(pat_ssn,'-','') as [pat_ssn]
			, unitno, pat_dob
	FROM        vw_dup_acc_validation-- chrg
	WHERE  convert(datetime,convert(varchar(10),service_date,101))	
				= convert(datetime,convert(varchar(10),@service_date,101))
		and fin_code = @fin_code
		and ltrim(rtrim(pat_name)) = ltrim(rtrim(@pat_name))
		and replace(pat_ssn,'-','') = replace(@pat_ssn,'-','')
		and unitno = @unitno
		and convert(datetime,convert(varchar(10),pat_dob,101)) 
			= convert(datetime,convert(varchar(10),@pat_dob,101))
	
	group by account,service_date, fin_code,  pat_name, pat_ssn, unitno,  pat_dob
	--, location, responsiblephy, mt_mnem,  facility, order_site, age_on_date_of_service, qty

	)
	insert into @Table_Var(account)
	SELECT  distinct coalesce(acc_merges.account,chrg.account) as [account]
	FROM  vw_dup_acc_validation as chrg--       chrg
	left outer join acc_merges on acc_merges.dup_acc = chrg.account
	inner join cte on 
		convert(datetime,convert(varchar(10),cte.service_date,101))
			 = convert(datetime,convert(varchar(10),chrg.service_date,101))
		and cte.fin_code = chrg.fin_code
		and ltrim(rtrim(cte.pat_name)) = ltrim(rtrim(chrg.pat_name))
		and replace(cte.pat_ssn,'-','')  = replace(chrg.pat_ssn,'-','')
		and cte.unitno = chrg.unitno
		and convert(datetime,convert(varchar(10),cte.pat_dob,101))  
			= convert(datetime,convert(varchar(10),chrg.pat_dob,101))
	
return;
END

