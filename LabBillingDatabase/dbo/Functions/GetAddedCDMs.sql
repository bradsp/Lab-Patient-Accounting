-- =============================================
-- Author:		Bradley Powers
-- Create date: 5/12/2016
-- Description:	Get added/modified CDMs
-- =============================================
CREATE FUNCTION GetAddedCDMs 
(	
	-- Add the parameters for the function here
	@fromdate DATETIME, 
	@thrudate DATETIME
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	SELECT cdm.cdm, cdm.descript, dbo.fnCptsForCdm(cdm.cdm) AS 'cpt'
	FROM cdm 
	WHERE cdm.deleted = 0 
	AND (cdm.mod_date BETWEEN @fromdate AND @thrudate 
		OR cdm.cdm IN (SELECT cdm FROM cpt4 WHERE mod_date BETWEEN @fromdate AND @thrudate))
)
