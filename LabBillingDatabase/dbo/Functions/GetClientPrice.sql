-- =============================================
-- Author:		David
-- Create date: 05/01/2014
-- Description:	Gets the client price for a cdm
-- =============================================
CREATE FUNCTION [dbo].[GetClientPrice] 
(
	-- Add the parameters for the function here
	@client varchar(10)
	, @cdm varchar(7)		-- one of these two have to be given
	, @chrg_num numeric(38,0) -- one of these two have to be given
	
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colCdm VARCHAR(7),
	colPrice money, 
	colDiscount numeric(18,2)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	
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
	
	--IF (EXISTS (SELECT cli_mnem FROM client 
	-- 1. USE THE HARD CODED PRICE IN ALL CASES WHERE IT EXISTS
--	IF (EXISTS	(SELECT price FROM cli_dis WHERE cli_mnem = @client
--					AND @cdm BETWEEN start_cdm AND end_cdm AND deleted = 0
--					AND price <> 0.00))
--		BEGIN
--			SELECT @price = 
--				(SELECT price FROM cli_dis WHERE cli_mnem = @client
--						AND @cdm BETWEEN start_cdm AND end_cdm AND deleted = 0)
--			SELECT @dis = 
--				(SELECT percent_ds FROM cli_dis WHERE cli_mnem = @client
--						AND @cdm BETWEEN start_cdm AND end_cdm AND deleted = 0)/100					
--			INSERT INTO	@Table_var(colCdm,colPrice, colDiscount)
--				VALUES (@cdm, @price,@dis)				
--
--			RETURN
--		END
--	ELSE -- NO HARD CODED PRICE SO CALCULATE USING THE CLIENTS FEE SCHEDULE AND CLIENT DISCOUNT
		BEGIN
			SELECT @price = (SELECT colClientPrice FROM dbo.GetFeeSchedulePrice(@client, @cdm))
			SET @price = ((100-@dis)*@price)/100
			select @dis = (SELECT coalesce(colCdmDiscount,colClientDiscount) FROM dbo.GetFeeSchedulePrice(@client, @cdm))
			INSERT INTO	@Table_var(colCdm,colPrice, colDiscount)
				VALUES (@cdm, @price,@dis)				

		RETURN 
		END

	INSERT INTO	@Table_var(colCdm,colPrice, colDiscount)
		VALUES (@cdm, @price,@dis)				

	RETURN 
END
