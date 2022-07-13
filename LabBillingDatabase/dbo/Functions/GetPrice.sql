-- =============================================
-- Author:		David
-- Create date: 05/29/2014
-- Description:	Gets Price
-- =============================================
CREATE FUNCTION [dbo].[GetPrice] 
(
	-- Add the parameters for the function here
	@client varchar(10), 
	@fin_code VARCHAR(10), --to get the fin_type from the fin table  'C' for Client 'M' for patient/insurance based on fin_code
	@cdm varchar(7), -- one of these has to be given
	@chrg_num NUMERIC(38,0)-- one of these has to be given
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	--colCdm VARCHAR(7), 
	colPrice NUMERIC(18,2),
	colRetail NUMERIC(18,2),
	colInpPrice NUMERIC(18,2)
	--colDiscount NUMERIC(18,2)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
Declare @fin_type varchar(1)
select @fin_type = (select type from fin where deleted = 0 and fin_code = @fin_code)

SELECT @cdm = (SELECT COALESCE(@cdm,
		(SELECT cdm FROM chrg WHERE chrg_num = @chrg_num)))
	DECLARE @dis numeric(18,2)
	SELECT @dis = NULL -- START WITH NO DISCOUNT
	DECLARE @price numeric(18,2)
	SELECT @dis = (SELECT  per_disc from dbo.client 
		WHERE cli_mnem = @client AND deleted = 0) -- GET THE CLIENTS DISCOUNT
	DECLARE @fs VARCHAR(2)
	SELECT @fs = (SELECT  fee_schedule from dbo.client 
		 WHERE cli_mnem = @client AND deleted = 0) -- GET THE CLIENTS FEE SCHEDULE

SELECT @price = (SELECT 
CASE WHEN UPPER(@fin_type) = 'C' THEN colClientPrice 
	WHEN UPPER(@fin_type) = 'M' THEN colMprice
ELSE NULL END FROM dbo.GetFeeSchedulePrice(@client, @cdm))
		--	SET @price = ((100-@dis)*@price)/100
select @dis = (SELECT coalesce(colCdmDiscount,colClientDiscount) FROM dbo.GetFeeSchedulePrice(@client, @cdm))
--			INSERT INTO	@Table_var(colCdm,colPrice, colDiscount)
--				VALUES (@cdm, @price,@dis)	

INSERT INTO @Table_Var
		( --colCdm,
		 colPrice,-- colDiscount 
		 [@Table_Var].colRetail,
		 [@Table_Var].colInpPrice
		 )
--VALUES	(-- @cdm, -- colCdm - varchar(7)
--			@price--, -- colPrice - numeric
--		--	10.00  -- colDiscount - numeric
--			)
SELECT 
CASE WHEN UPPER(@fin_type) = 'C' THEN gfs.colClientPrice 
	WHEN UPPER(@fin_type) = 'M' THEN gfs.colMprice 
ELSE NULL END 
AS [Price]
	, gfs.colClientPrice
	, gfs.colZPrice
FROM dbo.GetFeeSchedulePrice(@client, @cdm) gfs

	
	RETURN 
END
