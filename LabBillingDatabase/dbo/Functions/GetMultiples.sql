-- =============================================
-- Author:		David
-- Create date: 11/16/2015
-- Description:	returns a table with an accounts multiples that need to be fixed.
-- =============================================
CREATE FUNCTION GetMultiples 
(
	-- Add the parameters for the function here
	@acc varchar(15)
	
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAccount varchar(15), 
	colQty	INT,
	colCpt4 varchar(5),
	colType VARCHAR(6),
	colAmount NUMERIC(18,2),
	colModi VARCHAR(5),
	colRevcode VARCHAR(5),
	colModi2 VARCHAR(5),
	colDiagnosis_code_ptr VARCHAR(50)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
;with cte as 
( 
	select  chrg.account, 
	sum(qty) over (partition by chrg.account,cpt4,modi,modi2)  as [qty]
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
where chrg.account = @acc  
and chrg.cdm <> 'CBILL' and credited = 0  
)
INSERT INTO @Table_Var
	(
		colAccount ,
		colQty ,
		colCpt4 ,
		colType ,
		colAmount ,
		colModi ,
		colRevcode,
		colModi2 ,
		colDiagnosis_code_ptr 	
	)
select DISTINCT cte.account ,
		cte.qty ,
		cte.cpt4 ,
		cte.type ,
		cte.amount ,
		cte.modi ,
		cte.revcode ,
		cte.modi2 ,
		cte.diagnosis_code_ptr 
		from cte 
--		INNER JOIN dbo.dict_multiple_unit_cpt4s
--			ON dbo.dict_multiple_unit_cpt4s.cpt4 = cte.cpt4
--			AND cte.qty <> 1
		LEFT OUTER JOIN dbo.dict_multiple_unit_cpt4s
			ON dbo.dict_multiple_unit_cpt4s.cpt4 = cte.cpt4
			AND cte.qty <> 1
where qty <> 0 and qty*amount > 0 
AND cte.cpt4 IS null
order by account, cpt4, modi, modi2
	
	
	RETURN 
END
