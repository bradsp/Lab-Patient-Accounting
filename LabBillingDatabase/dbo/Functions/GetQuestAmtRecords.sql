-- =============================================
-- Author:		David
-- Create date: 04/24/2014
-- Description:	Gets the amount records for the Quest Charge
-- =============================================
CREATE FUNCTION [dbo].[GetQuestAmtRecords] 
(
	-- Add the parameters for the function here
	@chrg_num_old numeric(38,0)--, 
	--@chrg_num_new numeric(38,0)
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	--colChrgNum numeric(38,0),
	colLink INT, 
	colCpt varchar(5),
	colType VARCHAR(6), 
	colAmount MONEY, 
	colModDate DATETIME, 
	colModUser VARCHAR(50), 
	colModPrg VARCHAR(50), 
	colDeleted BIT,  
	colModi VARCHAR(5), 
	colRevcode VARCHAR(5), 
	--colModi2 VARCHAR(5), 
	--colDiagnosisCodePtr VARCHAR(50), 
	--colMtReqNo VARCHAR(50), 
	colOrderCode VARCHAR(7)--, 
	--colBillType VARCHAR(50), 
	--colBillMethod VARCHAR(50), 
	--colPointerSet BIT
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	DECLARE @price NUMERIC(18,2)
	SELECT @price = (select colPrice FROM GetQuestPrice(@chrg_num_old))
	DECLARE @cdm VARCHAR(7)
	SELECT @cdm = (SELECT cdm FROM chrg WHERE chrg_num = @chrg_num_old)
	DECLARE @num INT 
	SELECT @num = (SELECT MAX(link) FROM cpt4 WHERE cpt4.deleted = 0 AND cdm = @cdm)
	
	
		;with cte as
	(                      
		select top(@num+1) row_number() over (order by cdm) as rn	
		,@num as [number]
		,round(@price/@num,2) as [amount]
		,link
		, cpt4, type,
		modi, rev_code,	
		cdm AS [order_code] 
		
		 from cpt4
		 WHERE cdm = @cdm AND deleted = 0
		
	) 
	INSERT INTO @Table_Var
	        ( 
				--colChrgNum , 
				colLink,
				colCpt,
				colType, 
				colAmount, 
				colModUser ,
	          colModDate ,
	          colDeleted ,
	          colModPrg ,
	          colModi ,
	          colRevcode ,
	          colOrderCode 
	          --colPointerSet ,
	          --colBillMethod ,
	          --colModi2 ,
	          --colMtReqNo ,
	          --colDiagnosisCodePtr
	        )
	SELECT cte.link,	cte.cpt4,	cte.TYPE ,  x.amt, RIGHT(SUSER_SNAME(),50),GETDATE(),
	0, RIGHT(OBJECT_NAME(@@PROCID),50)
	,	cte.modi,	cte.rev_code,	cte.order_code
	FROM cte
	inner join (
	select case when c1.rn = @num
		then c1.amount + @price - (c1.amount *c1.rn)
		else c1.amount end as [amt]
	, c1.rn as [link]
	from cte c1
	cross join cte c2
	where c2.rn = c1.rn 
	) x on x.link = cte.link
	WHERE EXISTS (SELECT amt FROM cte WHERE amt IS NOT NULL)
   
--	VALUES  ( '' , -- colModUser - varchar(50)
--	          '2014-04-24 16:30:04' , -- colModDate - datetime
--	          NULL , -- colDeleted - bit
--	          '' , -- colModPrg - varchar(50)
--	          '' , -- colCpt - varchar(5)
--	          NULL , -- colChrgNum - numeric
--	          @price , -- colAmount - money
--	          '' , -- ColType - varchar(6)
--	          '' , -- colModi - varchar(5)
--	          '' , -- colBillType - varchar(50)
--	          '' , -- colOrderCode - varchar(7)
--	          NULL , -- colPointerSet - bit
--	          '' , -- colBillMethod - varchar(50)
--	          '' , -- colModi2 - varchar(5)
--	          '' , -- colRevcode - varchar(5)
--	          '' , -- colMtReqNo - varchar(50)
--	          ''  -- colDiagnosisCodePtr - varchar(50)
--	        )
	RETURN 
END
