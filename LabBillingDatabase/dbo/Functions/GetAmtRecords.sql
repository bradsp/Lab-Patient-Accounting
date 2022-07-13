-- =============================================
-- Author:		David
-- Create date: 05/05/2014
-- Description:	Gets the amount records for charges
-- =============================================
CREATE FUNCTION [dbo].[GetAmtRecords] 
(
	-- Add the parameters for the function here
	@client VARCHAR(10)
	,@cdm varchar(7)		  -- one or the other
	,@chrg_num numeric(38,0) -- one or the other
	,@price NUMERIC(18,2)
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
	colCptDescription VARCHAR(50),
	colOrderCode VARCHAR(7)--, colNum int
	--colBillType VARCHAR(50), 
	--colBillMethod VARCHAR(50), 
	--colPointerSet BIT
	--colModi2 VARCHAR(5), 
	--colDiagnosisCodePtr VARCHAR(50), 
	--colMtReqNo VARCHAR(50), 
	
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	SELECT @cdm = coalesce(@cdm,(SELECT cdm FROM chrg WHERE chrg_num = @chrg_num))
	DECLARE @num INT 
	SELECT @num = (SELECT count(link) FROM cpt4 WHERE cpt4.deleted = 0 AND cdm = @cdm
					and TYPE <> 'PC')
	IF (@num = 0)
	BEGIN
		INSERT INTO @Table_Var
				(
					colAmount ,
					colModDate ,
					colModUser ,
					colLink ,
					colCpt ,
					colType ,
					colModPrg ,
					colCptDescription ,
					colOrderCode ,
					colDeleted ,
					colModi ,
					colRevcode
				)
		VALUES	(
					NULL , -- colAmount - money
					GETDATE() , -- colModDate - datetime
					RIGHT(SUSER_SNAME(),50) , -- colModUser - varchar(50)
					0 , -- colLink - int
					'ERR' , -- colCpt - varchar(5)
					'ERR' , -- colType - varchar(6)
					RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
						'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) , -- colModPrg - varchar(50)
					'NO CPT 4 Records exist for this CDM' , -- colCptDescription - varchar(50)
					@cdm , -- colOrderCode - varchar(7)
					NULL , -- colDeleted - bit
					'' , -- colModi - varchar(5)
					''  -- colRevcode - varchar(5)
				)
		RETURN
	END
	IF (@num = 1)
	BEGIN
	INSERT INTO @Table_Var
			(
				colLink ,
				colAmount ,
				colModDate ,
				colModUser ,
				colCpt ,
				colCptDescription,
				colType ,
				colRevcode ,
				colOrderCode ,
				colModPrg ,
				colDeleted ,
				colModi
			)
	SELECT ROW_NUMBER() OVER (PARTITION BY cpt4.billcode order BY link) AS [rn],
	@price, GETDATE(),RIGHT(SUSER_SNAME(),50),
		--link ,
		cpt4, descript,type ,rev_code , @cdm -- cpt4.cdm ??
		
		 ,RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
				'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]					
		,deleted, modi 
			 
	FROM cpt4 
	WHERE cpt4.deleted = 0 AND cdm = @cdm AND type <> 'PC'
	RETURN
	END			
	
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
		 WHERE cdm = @cdm AND deleted = 0	AND type <> 'PC'	
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
	
	SELECT cte.link,	cte.cpt4,	cte.TYPE ,  x.amt
	, RIGHT(SUSER_SNAME(),50),GETDATE(),
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
	--where c2.rn-1 = c1.rn 
	where c2.rn = c1.rn 
	) x on x.link = cte.link
	
	WHERE EXISTS (SELECT amt FROM cte WHERE amt IS NOT NULL)
	
	
	RETURN 
END
