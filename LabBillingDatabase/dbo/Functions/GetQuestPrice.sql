-- =============================================
-- Author:		David
-- Create date: 04/24/2014
-- Description:	Gets the price based on a change of client
-- =============================================
CREATE FUNCTION [dbo].[GetQuestPrice] 
(
	-- Add the parameters for the function here
	@chrg_num numeric(38,0)
	--, @p2 char
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colChrgNum numeric(38,0),
	colPrice money, 
	colDiscount numeric(18,2)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	DECLARE @cdm varchar(7)
	SELECT @cdm = (SELECT cdm FROM chrg WHERE chrg_num = @chrg_num)
	DECLARE @dis numeric(18,2)
	SELECT @dis = 0.00 -- clients discount
	DECLARE @price numeric(18,2)
	SELECT @dis = 0.00 -- clients price for quest if this is not established we have an error
	IF (EXISTS	(SELECT price FROM cli_dis WHERE cli_mnem = 'QUESTR'
					AND @cdm BETWEEN start_cdm AND end_cdm AND deleted = 0))
	BEGIN
		SELECT @price = 
			(SELECT price FROM cli_dis WHERE cli_mnem = 'QUESTR'
					AND @cdm BETWEEN start_cdm AND end_cdm AND deleted = 0)
		SELECT @dis = 
			(SELECT percent_ds FROM cli_dis WHERE cli_mnem = 'QUESTR'
					AND @cdm BETWEEN start_cdm AND end_cdm AND deleted = 0)/100					
	END
	INSERT INTO	@Table_var(colChrgNum,colPrice, colDiscount)
	VALUES (@chrg_num,@price,@dis)				
	RETURN 
END
