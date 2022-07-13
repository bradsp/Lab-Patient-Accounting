-- =============================================
-- Author:		David
-- Create date: 05/23/2014
-- Description:	Process Charge changes
-- =============================================
CREATE PROCEDURE usp_prgProcessCharge 
	-- Add the parameters for the stored procedure here
	--@tblChrg  VARCHAR(150)
	--, @p2 int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	SELECT * FROM [dbo].GlobalBillingCharges gbs
	INNER JOIN chrg ON chrg.chrg_num = gbs.colChrgNum
END
