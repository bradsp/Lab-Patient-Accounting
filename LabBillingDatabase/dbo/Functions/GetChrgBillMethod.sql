-- =============================================
-- Author:		David
-- Create date: 06/09/2014
-- Description:	Gets the bill code for a charge
-- =============================================
CREATE FUNCTION GetChrgBillMethod 
(
	-- Add the parameters for the function here
	@client VARCHAR(10),
	@cdm varchar(15), 
	@fin_code varchar(10),
	@bill_to VARCHAR(10), -- client or ins_code or special handling as determined below
--	@location varchar(50),	@ins_policy VARCHAR(50),	@ins_group VARCHAR(50),
	@age int
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colClient VARCHAR(10),
	colBillMethod varchar(50),
	colCdm VARCHAR(7),
	colFinCode VARCHAR(10),
	colCdmPrice numeric(18,2),
	colError varchar(1024)
)
AS
BEGIN
	
	DECLARE @billMethod VARCHAR(50)
	SET @billMethod = @bill_to
	DECLARE @err VARCHAR(1024)
	
	-- Fill the table variable with the rows for your result set
-- 1. Handle the special cases not related to Quest, Global Billing, or capitated contracts
	SET @client = REPLACE(@client,'"','')
	SET @cdm = REPLACE(@cdm,'"','')
	SET @fin_code = REPLACE(@fin_code,'"','')
	SET @bill_to = REPLACE(@bill_to,'"','')
	SET @age = REPLACE(@age,'"','')
	-- special cases
	IF(UPPER(@client) IN ('BCH','CGH','COM'))
	BEGIN
		INSERT INTO @Table_Var
				(
					colCdm ,colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
				)
		SELECT TOP(1)  colCdm ,colClient ,colClient,colClientPrice, @fin_code
		, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
			FROM dbo.GetFeeSchedulePrice(@client,@cdm)		
		RETURN 
	END	
	
	IF (UPPER(@cdm) IN ('5929144','5929146'))
	BEGIN
		INSERT INTO @Table_Var
				(
					colCdm ,colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
				)
		SELECT TOP(1)  colCdm ,'MC HOLD' ,colClient, colMPrice, @fin_code
		, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
			FROM dbo.GetFeeSchedulePrice(@client,@cdm)		
		RETURN 
	END	

	IF (UPPER(@fin_code) IN ('LMBC','LCBC'))
	BEGIN
		IF (EXISTS(SELECT cdm FROM dbo.dict_Madison_County_Capitated_Contract 
				WHERE cdm = @cdm))
		BEGIN
			INSERT INTO @Table_Var
				(
					colCdm ,colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
				)
			SELECT TOP(1)  colCdm ,@fin_code ,colClient,colClientPrice, @fin_code
			, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
				FROM dbo.GetFeeSchedulePrice(@client,@cdm)		
		END	
		ELSE
		BEGIN
			INSERT INTO @Table_Var
				(
					colCdm ,colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
				)
			SELECT TOP(1)  colCdm ,@fin_code ,colClient,colMPrice, @fin_code
			, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
				FROM dbo.GetFeeSchedulePrice(@client,@cdm)		
		END	
		RETURN 	
	END	
	
	IF (UPPER(@fin_code) IN ('BCA'))
	BEGIN
	INSERT INTO @Table_Var
			(
				colCdm ,colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
			)
		SELECT TOP(1)  colCdm ,@fin_code ,colClient,colMPrice, @fin_code
		, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
			FROM dbo.GetFeeSchedulePrice(@client,@cdm)		
		RETURN 	
	END

	IF (@fin_code in ('E','S','PCP'))
	BEGIN
		INSERT INTO @Table_Var
		(
			colCdm ,colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
		)
		SELECT TOP(1)  colCdm ,@bill_to ,colClient,colMPrice, @fin_code
		, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
			FROM dbo.GetFeeSchedulePrice(@client,@cdm)
		RETURN
	END

	IF (@fin_code IN ('W','X','Y','Z','CLIENT'))
	BEGIN
		INSERT INTO @Table_Var
		(
			colCdm ,colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
		)
		SELECT TOP(1)  colCdm ,colClient ,colClient,colClientPrice, @fin_code
		, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
			FROM dbo.GetFeeSchedulePrice(@client,@cdm)
		
	RETURN
	END
	-- end of special cases



-- 2. Reference lab for Bluecare
	IF (@fin_code = 'D' 
			AND (@cdm BETWEEN '5520000' AND '5527417' 
					OR @cdm BETWEEN '5527420' AND '552ZZZZ'))
	BEGIN
		SET @billMethod = 'QUESTREF'	
		INSERT INTO @Table_Var
		(
			colCdm ,colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
		)
		SELECT TOP(1)  colCdm , @billMethod ,colClient
		,colMPrice,@fin_code--,colClientPrice, 'Y'
		, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
			FROM dbo.GetFeeSchedulePrice('QUESTREF',@cdm)
		RETURN
	END
-- end of Reference lab for Bluecare
	
--3. Not Reference lab for Bluecare
	IF (@fin_code = 'D') 
	BEGIN
			--4. Not on the exclusions list -- needs some age work 
			IF (EXISTS(SELECT start_cdm FROM cli_dis WHERE deleted = 0 
					AND cli_mnem = 'QUESTR'
						AND @cdm BETWEEN start_cdm AND end_cdm))
			BEGIN
				SELECT @billMethod = 'QUESTR'
				INSERT INTO @Table_Var
				(
					colCdm , colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
				)
				SELECT TOP(1)  colCdm ,@billMethod ,colClient
				,  colMPrice--colClientPrice--colCPrice 
				, @fin_code--'Y'
				, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
					FROM dbo.GetFeeSchedulePrice('QUESTR',@cdm)
					return
			END 
			ELSE --5. On the exclusions list we bill -- needs some age work above
			BEGIN
				INSERT INTO @Table_Var
				(
					colCdm , colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
				)
				SELECT  colCdm ,@billMethod ,colClient
				,  colMPrice
				, @fin_code
				, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
					FROM dbo.GetFeeSchedulePrice(@client,@cdm)

				RETURN
			END
		END
	
	IF (EXISTS(SELECT cdm FROM dbo.dict_global_billing_cdms WHERE cdm = @cdm) 
	AND @client IN ('LEW','TPG','TPG2','TPG3','BMUC','MGPS'))
	BEGIN
		SET @billMethod = @client
		INSERT INTO @Table_Var
	(
		colCdm , colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
	)
	SELECT TOP(1)  colCdm ,@billMethod ,colClient
	,  colMPrice
	, @fin_code
	, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
		FROM dbo.GetFeeSchedulePrice(@client,@cdm)
	return
	END
	
	IF (EXISTS(SELECT cdm FROM dbo.dict_global_billing_cdms WHERE cdm = @cdm))
	BEGIN
	SET @billMethod = 'JPG'
		INSERT INTO @Table_Var
	(
		colCdm , colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
	)
	SELECT TOP(1)  colCdm ,@billMethod ,colClient
	,  colMPrice--colClientPrice
	, @fin_code --'Y'
	, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
		FROM dbo.GetFeeSchedulePrice('JPG',@cdm)
	return
	END

--	IF (@fin_code = 'LMBC')
--	BEGIN
--		SET @billMethod = 'LIFTMCE'
--		INSERT INTO @Table_Var
--	(
--		colCdm , colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
--	)
--	SELECT TOP(1)  colCdm ,@billMethod ,colClient
--	,  colClientPrice
--	, 'Y'
--	, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
--		FROM dbo.GetFeeSchedulePrice(@billMethod,@cdm)
--	return
--	END	
	
--	IF (@fin_code = 'LCBC')
--	BEGIN
--		SET @billMethod = 'LIFTCOJ'
--		INSERT INTO @Table_Var
--	(
--		colCdm , colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
--	)
--	SELECT  TOP(1) colCdm ,@billMethod ,colClient
--	,  colClientPrice
--	, 'Y'
--	, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
--		FROM dbo.GetFeeSchedulePrice(@billMethod,@cdm)
--	return
--	END	

	IF ((@fin_code = 'H' OR @fin_code = 'L') OR 
		(@bill_to = 'COMM.H' OR @bill_to = 'COMM.L'))
	BEGIN
		SET @billMethod = COALESCE(@bill_to,@fin_code)
		INSERT INTO @Table_Var
	(
		colCdm , colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
	)
	SELECT TOP(1)  colCdm ,@billMethod ,colClient
	,  colMPrice
	, @fin_code
	, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
		FROM dbo.GetFeeSchedulePrice(@client,@cdm)
	RETURN
	END	
	

	BEGIN
		
		SET @billMethod = COALESCE(@bill_to,(SELECT CASE WHEN @fin_code = 'B' THEN 'BC'
											ELSE @fin_code END) )
		INSERT INTO @Table_Var
	(
		colCdm , colBillMethod ,colClient ,colCdmPrice ,colFinCode,colError 
	)
	SELECT  TOP(1) colCdm ,@billMethod ,colClient
	 , colMPrice 
	, @fin_code
	, COALESCE(colCdm, 'XML ERROR: ['+@cdm+'] or ['+@client+'] has an error.')
		FROM dbo.GetFeeSchedulePrice(@client,@cdm)
	RETURN
	END	


	RETURN 
	
END
