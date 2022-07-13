-- =============================================
-- Author:		David
-- Create date: 04/01/2015
-- Description:	Gets the physicians NPI from either the cerner phy number or name
-- =============================================
CREATE FUNCTION GetCernerPhyNPI 
(
	-- Add the parameters for the function here
	@ov_code varchar(50),
	@phy_name VARCHAR(90)
)
RETURNS varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(50)

	-- Add the T-SQL statements to compute the return value here
	IF (EXISTS(SELECT tnh_num FROM phy WHERE ov_code = @ov_code))
	BEGIN
		SELECT @Result = (SELECT tnh_num FROM phy WHERE ov_code = @ov_code)
	END
	ELSE
	BEGIN
		SELECT @Result = (SELECT tnh_num FROM phy WHERE
		 phy.last_name 
			= (SELECT dbo.ufn_Split_Name.cLNAME FROM dbo.ufn_Split_Name(@phy_name))
		 AND phy.first_name = (SELECT dbo.ufn_Split_Name.cFNAME FROM dbo.ufn_Split_Name(@phy_name))
		 AND (phy.mid_init = (SELECT dbo.ufn_Split_Name.cMNAME FROM dbo.ufn_Split_Name(@phy_name))
				or NULLIF(phy.mid_init,'') is NULL)
		 )

	END

	-- Return the result of the function
	RETURN @Result

END
