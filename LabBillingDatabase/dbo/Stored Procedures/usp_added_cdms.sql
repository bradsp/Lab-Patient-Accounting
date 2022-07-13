-- =============================================
-- Author:		Bradley Powers
-- Create date: 5/12/2016
-- Description:	Pulls modified/added cdms
-- =============================================
CREATE PROCEDURE usp_added_cdms 
	-- Add the parameters for the stored procedure here
	@fromdate DATETIME = NULL, 
	@thrudate DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT cdm.cdm, cdm.descript, dbo.fnCptsForCdm(cdm.cdm) AS 'cpt'
	FROM cdm 
	WHERE cdm.deleted = 0 
	AND cdm.cdm = '5529299'
	ORDER BY cdm.cdm 
	
END
