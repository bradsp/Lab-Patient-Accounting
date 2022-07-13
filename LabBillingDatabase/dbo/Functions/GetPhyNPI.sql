-- =============================================
-- Author:		David
-- Create date: 06/19/2014
-- Description:	return the NPI for post charge phy
-- =============================================
CREATE FUNCTION GetPhyNPI 
(
	-- Add the parameters for the function here
	@phyMnem varchar(25)
	--, @phyName varchar(50)
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colNPI varchar(25), 
	colName VARCHAR(50)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	SET @phyMnem = REPLACE(@phyMnem,'"','')
	IF (NULLIF(@phyMnem,'') IS NULL)
	BEGIN
	INSERT INTO @Table_Var
			( colNPI, colName )
	VALUES	( @phyMnem,'ERROR PHY [ '+@phyMnem+' ] does not exist'
			)
	RETURN
	END

	IF (EXISTS(SELECT billing_npi FROM phy WHERE mt_mnem = @phyMnem AND deleted = 0))
		BEGIN
		DECLARE @bNpi VARCHAR(25)
		SELECT @bNpi = (SELECT billing_npi FROM phy WHERE mt_mnem = @phyMnem AND deleted = 0)
		INSERT INTO @Table_Var ( colNPI, colName )
		SELECT TOP(1) @bNpi,UPPER(last_name+', '+first_name+' '+COALESCE(mid_init,'')) 
			FROM phy WHERE tnh_num = @bNpi AND deleted = 0
				
		END
	ELSE
	
		BEGIN
		INSERT INTO @Table_Var
				( colNPI, colName )
		VALUES	( @phyMnem,'ERROR PHY [ '+@phyMnem+' ] does not exist'
				)
		END
	RETURN
END
