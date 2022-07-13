
-- =============================================
-- Author:		David
-- Create date: 05/22/2014
-- Description:	Replacement for Global Billing Application 
--	filters the data for charges we need to bill globally
-- =============================================
CREATE FUNCTION [dbo].[GetGCodeBillingCharges] 
(
	-- Add the parameters for the function here
	@endDate DATETIME  -- give the meditech operators time to credit original test
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colClient VARCHAR(10), 
	colAcc VARCHAR(15),
	colChrgNum NUMERIC(38,0),
	colCDM varchar(7),
	colCPT VARCHAR(5),
	colQty INT,
	colChrgAmt NUMERIC(18,2),
	colDOS DATETIME,
	colDateEntered DATETIME,
	colFinCode VARCHAR(10),
	colClType int,
	colError varchar(50)
	--, rowguid [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()) 
)
AS
BEGIN
	DECLARE @DIM INT 
	SET @DIM = DATEPART(DAY,DATEADD(mm,DATEDIFF(m,0,GETDATE()), -.000003))-- Last day This Month
	DECLARE @TDAY INT
	SET @TDAY = DATEPART(DAY,GETDATE())
	SELECT @endDate = coalesce(@endDate,(select 
	CASE WHEN @DIM - @TDAY > 5
		THEN DATEADD(dd,-4,DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),getdate(),101))))
		ELSE DATEADD(dd,@TDAY-@DIM,DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),getdate(),101))))
		END AS [endDate]))
	
	-- Fill the table variable with the rows for your result set
	DECLARE @startDate datetime
	SET @startDate = CAST('01/20/2012 00:00' AS datetime) -- date program started 
	
	INSERT INTO @Table_Var
	        ( colClient ,
	          colAcc ,
	          colChrgNum ,
	          colCDM ,
	          colCPT ,
	          colQty ,
	          colChrgAmt,
	          colDOS ,
	          colDateEntered ,
	          colFinCode ,
	          colClType,
	          colError 
	        )
-- accounts that have cdms that contain the global billing cpt4s

select acc.cl_mnem, chrg.account, chrg.chrg_num as [chrg_num], cdm, cpt4
	, sum(qty) as [qty]
	, sum(qty*amount) as [charges]
	, convert(datetime,convert(varchar(10),service_date,101)) as [DOS]
	, convert(datetime,convert(varchar(10),amt.mod_date,101)) as [Date Chrg Entered]
	, acc.fin_code, client.type
	, case when datediff(dd,				          
				convert(datetime,convert(varchar(10),service_date,101))	,
					getdate()) >= 90 THEN 'DO NOT BILL -- TOO OLD'
			  WHEN acc.fin_code = 'Y' THEN 'DO NOT BILL -- FinCode "Y"'
			 -- WHEN (acc.fin_code in ('B') and NOT exists(select account from ins where ins_a_b_c = 'A' and policy_num like 'ZXK%'  AND account = chrg.account)) THEN 'DO NOT BILL -- FinCode B with no Ins Record' 
			  ELSE NULL END
from chrg
inner join amt on amt.chrg_num = chrg.chrg_num
inner join acc on acc.account = chrg.account
inner join client on client.cli_mnem = acc.cl_mnem
--where cdm in(select cdm from dict_global_billing_cdms)
where (cdm IN (select cdm from cpt4 WHERE cpt4 IN ('G0461','G0462'))
OR cdm IN (select cdm from dict_global_billing_cdms))

and service_date between @startDate and @endDate
and credited = 0
and coalesce(invoice,'') = ''
--and coalesce(dbo.chrg.bill_method,'') = ''
and 
(
	client.type in ('0','1','2')  and (acc.fin_code NOT in ('D','A','M','X','Y'))
	or (
		client.type NOT IN ('0','1','2') and ((acc.fin_code NOT IN ('D')
		or (acc.fin_code in ('B') 
		and NOT exists(select account 
						from ins 
						where ins_a_b_c = 'A' and 
						policy_num like 'ZXK%'
						AND account = chrg.account)))))
)
--and (
--	client.type in ('0','1','2') and (acc.fin_code not in ('D','A','M','X','Y'))
--	or (
--		client.type NOT IN ('0','1','2') and ((not acc.fin_code in ('D')
--		
---- if cover kids billing with blank ins is ok to bill to pathologists leave this line as it is
--	or (acc.fin_code in ('B') and NOT exists(select account from ins where ins_a_b_c = 'A' and policy_num like 'ZXK%' AND account = chrg.account))
---- if cover kids billing with blank ins is NOT ok to bill to pathologist remove above
----	 then uncomment below line and remove the comment marks on thelast when in the error select above ???
----and (acc.fin_code in ('B') and exists(select account from ins where ins_a_b_c = 'A' and policy_num like 'ZXK%' AND account = chrg.account))
--)
--))
and (not chrg.account like '[Q]%')
and client.cli_mnem not in ('HC','JPG', 'LEW','TPG','TPG2','TPG3','BMUC','MGPS') -- wdk 20190603 added 'HC'
group by acc.cl_mnem, convert(datetime,convert(varchar(10),service_date,101)), chrg.account, cdm, cpt4
	, chrg.chrg_num, chrg.credited,  convert(datetime,convert(varchar(10),amt.mod_date,101))
	, acc.fin_code, client.type
having sum(qty*amount) <> 0
order by convert(datetime,convert(varchar(10),service_date,101)), chrg.account, cdm, cpt4

	
	
	
	RETURN 
END
