-- =============================================
-- Author:		David Kelly
-- Create date: 8/2/2012
-- Description:	Provide selections criteria for ViewerAccount
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_ViewerAcc_Select] 
	-- Add the parameters for the stored procedure here
	@startDate datetime = '12/21/2011 00:00',
	@thruDate datetime,
	@outpatientDate datetime,
	@questStartDate datetime = '10/01/2012 00:00',
	@questEndDate datetime = '05/31/2020 23:59'
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
; WITH cteBilling AS 
(
	SELECT TOP(100) PERCENT acc.status,  
  acc.account, acc.pat_name, ins_code, acc.cl_mnem,  
  acc.fin_code  
  ,convert(datetime,convert(varchar(10),acc.trans_date,101)) as [trans_date]  
  /*20140320 modified to only bill Bluecross as outpatient*/--, case when outpatient_billing = 1 and trans_date > @outpatientDate and acc.fin_code <> 'A'
  
, case when 
		outpatient_billing = 1 
		THEN 'REF LAB'
--			and trans_date between @outpatientDate AND '12/31/2016 23:59:59' --wdk 20170109
--			AND ins_code NOT IN ('BC','UMR', 'GOLDEN1', 'HP','AETNA','AG','HUM','SECP','BCA')-- exclude insurance from outpatient billing
--			-- added AG 20150126 and the one below
--		then 'OUTPATIENT' 
		else 'REF LAB'
	end as [billing type] 

  , CASE 
--		WHEN outpatient_billing = 1 -- wdk 21070119 no longer doing outpatient
--					AND trans_date between @outpatientDate AND '12/31/2016 23:59:59' --wdk 20170109 >= @outpatientDate 
--					AND trans_date >= @questStartDate
--					AND acc.fin_code = 'D'
--					THEN  'OUTPATIENT' -- outpatient elimante QUEST and COVER KIDS 
	     WHEN outpatient_billing = 1 
					AND trans_date between @outpatientDate AND '12/31/2016 23:59:59' --wdk 20170109>= @outpatientDate 
					AND trans_date >= @questStartDate AND trans_date <= @questEndDate
					AND (acc.fin_code = 'B' 
							AND ins.policy_num LIKE 'ZXK%')
					THEN 'QUEST'
		 WHEN outpatient_billing = 1 
					AND trans_date between @outpatientDate AND '12/31/2016 23:59:59' --wdk 20170109>= @outpatientDate 
					AND trans_date >= @questStartDate AND trans_date <= @questEndDate
					AND ins_code NOT IN ('BC','UMR', 'GOLDEN1','HP','AETNA','AG','HUM','SECP','BCA')
					THEN 'UBOP'
		WHEN outpatient_billing = 1 
					AND trans_date between @outpatientDate AND '12/31/2016 23:59:59' --wdk 20170109>= @outpatientDate 
					AND trans_date >= @questStartDate AND trans_date <= @questEndDate
					AND acc.fin_code = 'D'
					THEN 'QUEST'
			
		ELSE  -- not outpatient
			case WHEN trans_date >= @questStartDate AND trans_date <= @questEndDate
					AND (acc.fin_code = 'B' 
							AND ins.policy_num LIKE 'ZXK%')
					THEN 'QUEST'
			WHEN trans_date >= @questStartDate AND trans_date <= @questEndDate
					AND acc.fin_code = 'D'
					THEN 'QUEST'
			when COALESCE(insc.bill_form,'') = ''
				then 'ERR'
			else insc.bill_form
			end
  END AS [bill_form]
, client.type
 FROM acc  
 inner join pat on pat.account = acc.account  
 left outer join ins on ins.account = acc.account and ins.ins_a_b_c = 'A'  
 left outer join client on client.cli_mnem = acc.cl_mnem  
  left outer join insc on insc.code = ins.ins_code 
  WHERE 
	case when client.type <> '7' 
		 then trans_date
	else 
		case when convert(datetime, trans_date) 
				<= convert(DATETIME
				,(select value from system where key_name = 'nh_bill_thru'))
		then trans_date
	end 
 end   <= trans_date
and 
acc.trans_date between 
convert(datetime,@startDate) AND 
convert(datetime,@thruDate)   
 and pat.mailer = 'N'  
  and 
  COALESCE(dbo.pat.ub_date, dbo.pat.h1500_date, dbo.pat.colltr_date, 
			dbo.pat.baddebt_date, dbo.pat.batch_date, dbo.pat.bd_list_date,
			dbo.pat.ebill_batch_date, dbo.pat.ebill_batch_1500,
			dbo.pat.e_ub_demand_date, dbo.pat.claimsnet_1500_batch_date,
			dbo.pat.claimsnet_ub_batch_date) IS NULL  
  and acc.status = 'NEW'
 order by convert(datetime,convert(varchar(10),acc.trans_date,101)), pat_name,acc.account
)
SELECT  status ,
        account ,
        pat_name ,
        ins_code ,
        cl_mnem ,
        fin_code ,
        trans_date ,
        [billing type] ,
        bill_form ,
        type FROM cteBilling	
     
 
END
