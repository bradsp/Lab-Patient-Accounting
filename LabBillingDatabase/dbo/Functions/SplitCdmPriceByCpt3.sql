-- =============================================
-- Author:		David
-- Create date: 05/29/2014
-- Description:	Splits the cpt4 costs evenly and adds the remainder to the last cpt
--				if the fin type is "C" 
-- =============================================
CREATE FUNCTION SplitCdmPriceByCpt3 
(
	-- Add the parameters for the function here
	@cdm varchar(7),
	@price NUMERIC(18,2),
	@finType VARCHAR(1)
	
	--, @links int
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colCpt VARCHAR(5),
	colCptPrice numeric(18,2),
	colType VARCHAR(4),
	colModi VARCHAR(2),
	colRevCode VARCHAR(4),
	colCptDescription VARCHAR(50),
	colLink INT 
	
	--, c2 int
)
AS

BEGIN


	SET @cdm = REPLACE(@cdm,'"','')
	SET @price = REPLACE(@price,'"','')
	SET @finType = REPLACE(@finType,'"','')
	-- Fill the table variable with the rows for your result set
	--DECLARE @price NUMERIC(18,2)
	--SELECT @price = (select colPrice FROM GetQuestPrice(@chrg_num_old))
--	DECLARE @cdm VARCHAR(7)
--	SELECT @cdm = (SELECT cdm FROM chrg WHERE chrg_num = @chrg_num_old)
--	DECLARE @links INT 
--	SELECT @links = (SELECT count(link) FROM cpt4 WHERE cpt4.deleted = 0 AND cdm = @cdm
--		AND type <> 'PC')
	DECLARE @links INT 
	SELECT @links = (SELECT count(link) FROM cpt4 WHERE cpt4.deleted = 0 AND cdm = @cdm
		AND type <> 'PC')
	
	IF (@finType = 'M' AND @links = 1)
	BEGIN
		INSERT INTO @Table_Var
	        ( 
				colCpt,
				colCptPrice,
				colType, 
				colModi ,
				colRevcode,
				colCptDescription,
				colLink
	        )
	     SELECT cpt4 ,@price ,type ,modi ,rev_code ,descript 
	     ,ROW_NUMBER() OVER (PARTITION BY cdm ORDER BY link) AS [link]
			FROM cpt4 WHERE cdm = @cdm AND type <> 'PC'
		RETURN
	END 
	
	
	IF (@links > 0)
	BEGIN
	;with cte as
	(                      
		select top(@links) row_number() over (order by cdm) as [link]	
		--,@links as [number]
		,round(@price/@links,2) as [amount]
		--,link
		, cpt4, type, descript,
		modi, rev_code,	
		cdm AS [order_code] 
		
		 from cpt4
		 WHERE cdm = @cdm AND deleted = 0
		 AND type <> 'PC'
		
	) 
	INSERT INTO @Table_Var
	        ( 
				colCpt,
				colCptPrice,
				colType, 
				colModi ,
				colRevcode,
				colCptDescription,
				colLink
				
	        )
	SELECT cte.cpt4,x.amt,cte.TYPE ,  cte.modi,	cte.rev_code, cte.descript
	,cte.link--,cte.rn
	FROM cte
	inner join (
	select case when c1.link = @links
		then c1.amount + @price - (c1.amount *c1.link)
		else c1.amount end as [amt]
	--, c1.rn as [link]
	,c1.link
	from cte c1
	cross join cte c2
	where c2.link = c1.link
	) x on x.link = cte.link
	END
	ELSE
	BEGIN	
		INSERT INTO @Table_Var
				(
					colType ,
					colCptPrice ,
					colCpt ,
					colModi ,
					colLink ,
					colCptDescription ,
					colRevCode
				)
		VALUES	(
					'' , -- colType - varchar(4)
					NULL , -- colCptPrice - numeric
					'ERR' , -- colCpt - varchar(5)
					'' , -- colModi - varchar(2)
					0 , -- colLink - int
					'XML ERROR -- DIVIDE BY ZERO' , -- colCptDescription - varchar(50)
					''  -- colRevCode - varchar(4)
				)
	END 
RETURN 
END
