-- =============================================
-- Author:		Bradley Powers
-- Create date: 02/09/2015
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[GetNewFeeSchedPrice] 
(
	-- Add the parameters for the function here
	@cdm varchar(7),
	@feesched VARCHAR(50)
)
RETURNS NUMERIC(18,2)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @price MONEY;
	SET @price = 0.00;
	-- Add the T-SQL statements to compute the return value here
	IF(@feesched = 'BASE')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_client_base WHERE colCdm = @cdm
	END
	
	IF(@feesched = 'MEDICARE')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_medicare WHERE colCdm = @cdm
	END
	
	IF(@feesched = 'INDUSTRY')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_industry WHERE colCdm = @cdm
	END

	IF(@feesched = 'CAPITATED')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_capitated WHERE colCdm = @cdm
	END

	IF(@feesched = 'JPG')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_jpg WHERE colCdm = @cdm
	END

	IF(@feesched = 'MOSAIC')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_mgps WHERE colCdm = @cdm
	END

	IF(@feesched = 'QUESTR')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_questr WHERE colCdm = @cdm
	END

	IF(@feesched = 'SPEC_CLINIC')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_spec_clinics WHERE colCdm = @cdm
	END

	IF(@feesched = 'TPG')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_tpg WHERE colCdm = @cdm
	END

	IF(@feesched = 'HOSPITAL')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_spec_hospital WHERE colCdm = @cdm
	END

	IF(@feesched = 'WC')
	BEGIN
		SELECT @price = colClientPrice FROM tst.feeschedule_spec_wc WHERE colCdm = @cdm
	END


	-- Return the result of the function
	RETURN @price

END
