-- =============================================
-- Author:		David
-- Create date: 03/13/2014
-- Description:	Returns Quest Accounts
-- =============================================
CREATE FUNCTION ufn_quest_processing 
(
	-- Add the parameters for the function here
	@startDate datetime, 
	@endDate DATETIME
	
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colFinCode VARCHAR(10),
	colAccount varchar(15), 
	colAge INT,
	colStatus VARCHAR(8),
	colPatName VARCHAR(40),
	colInsCode	VARCHAR(10),
	colTransDate DATETIME,
	colBillingType VARCHAR(20),
	colBillForm	VARCHAR(10),
	colClientType INT,
	colSSIType VARCHAR(20),
	colError VARCHAR(8000)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	DECLARE @minDate DATETIME
	select @minDate = (SELECT CONVERT(DATETIME,'01/01/1753'))
	DECLARE @outpatientDate DATETIME
	SET @outpatientDate = '04/01/2012 00:00:00'
	SET @startDate = ISNULL(@startDate,CONVERT(datetime,'12/21/2011 00:00'))
	DECLARE @startquestDate DATETIME
	SET @startquestDate = '10/01/2012 00:00'
	DECLARE @startbluecDate DATETIME
	SET @startbluecDate = '01/01/2014 00:00'
	
;with cteAcc as 
( 
	select acc.account, acc.fin_code, 
	datediff(year,ISNULL(pat.dob_yyyy,@minDate),trans_date) as [Age],
	acc.status,
	ISNULL(acc.pat_name,'NO PAT RECORD') AS [PatName],
	ISNULL(ins.ins_code,'NO INS REC') AS [InsCode],
	trans_date
	, case when outpatient_billing = 1 and trans_date BETWEEN @outpatientDate AND '12/31/2016 23:59:59'
			and acc.fin_code <> 'A'
		
		then 'OUTPATIENT'  
		else 'REF LAB'  
	end as [billing type] 
	---
	, case when outpatient_billing = 1 and trans_date BETWEEN @outpatientDate AND '12/31/2016 23:59:59'
		then 
		case when (trans_date >= @startquestDate AND (acc.fin_code = 'D')) OR
				  (trans_date >= @startbluecDate AND (acc.fin_code = 'B' AND ins.policy_num LIKE 'ZXK%')) 
			then 'QUEST'
		else
		case when acc.fin_code = 'A'
			then 'UB'
			else
			case when acc.fin_code in ('X','Y','CLIENT')
				then 'CBILL'
				else 
					case when acc.fin_code in ('S','E')
					then 'PATBILL'
					else 'UBOP' 
			end
		end
		end
		end
	else
		case when (trans_date >= @startquestDate AND (acc.fin_code = 'D')) OR
				  (trans_date >= @startbluecDate AND (acc.fin_code = 'B' AND ins.policy_num LIKE 'ZXK%')) 
			then 'QUEST'
		else
		case when acc.fin_code = 'A'
		then 'UB'
		else 
		case when acc.fin_code in ('X','Y','CLIENT')
		then 'CBILL'
		else 
			case when acc.fin_code in ('S','E')
			then 'PATBILL'
			else insc.bill_form 
			end
		end
		end
		end 
end as [bill_form]
	---
	,client.type AS [clientType]
	
	,CASE WHEN COALESCE(acc.pat_name,ins.ins_code,CONVERT(VARCHAR(10),pat.dob_yyyy,101)) IS NULL
				THEN 'CAN NOT PROCESS ACCOUNT'
	END AS [Errors]
	
	from acc 
	LEFT OUTER join pat on pat.account = acc.account AND pat.deleted = 0
	LEFT OUTER JOIN ins ON ins.account = acc.account AND ins_a_b_c = 'A' AND ins.deleted = 0
	LEFT OUTER JOIN insc ON insc.code = ins.ins_code AND insc.deleted = 0
	LEFT OUTER JOIN client ON client.cli_mnem = acc.cl_mnem AND client.deleted = 0
	
	WHERE
	(acc.deleted = 0 AND acc.status <> 'PAID_OUT')
	AND  (
	(	acc.fin_code = 'D' AND trans_date between @startDate and @endDate) -- and status = 'QUEST')
		OR 
	(acc.fin_code = 'B' AND trans_date BETWEEN @startDate AND @endDate -- AND status = 'QUEST'
			AND ins.policy_num LIKE 'ZXK%')) -- effective 01/01/2014
)
--, cteChrg AS
--(
--	
--) 
INSERT INTO @Table_Var
        ( colFinCode, 
			colAccount ,
			colAge ,
			colStatus ,
			colPatName,
			colInsCode,
			colTransDate,
			colBillingType,
			colBillForm,
			colClientType,
			colError
        )
SELECT   fin_code, account ,
        Age ,
        status ,
        PatName ,
        InsCode ,
        trans_date,
        [billing type],
        [bill_form],
        clientType,
        Errors
FROM cteAcc

	RETURN 
END
