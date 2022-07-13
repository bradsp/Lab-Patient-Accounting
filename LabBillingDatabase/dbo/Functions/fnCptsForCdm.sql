-- =============================================
-- Author:		Bradley Powers
-- Create date: 01/31/2014
-- Description:	Returns a string of cpt codes for 
--       given cdm.
-- =============================================
CREATE FUNCTION [dbo].[fnCptsForCdm] 
(
	-- Add the parameters for the function here
	@cdm varchar(7)
)
RETURNS VARCHAR(255)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @cptlist VARCHAR(255)  

	-- Add the T-SQL statements to compute the return value here
SELECT  @cptlist = ( STUFF(ISNULL((SELECT  '; ' + ISNULL(cpt4.cpt4, '') + COALESCE('-'
                                                              + cpt4.modi, '')
                                + CASE WHEN COUNT(cpt4.cpt4) > '1'
                                       THEN ' x '
                                            + CAST(COUNT(cpt4.cpt4) AS VARCHAR)
                                       ELSE ''
                                  END
                        FROM    dbo.cpt4
                        WHERE   cpt4.cdm = cdm.cdm
                                AND cpt4.deleted = 0
                        GROUP BY cpt4.cpt4 ,
                                cpt4.modi
                        ORDER BY cpt4.cpt4
                FOR    XML PATH('') ,
                           TYPE ,
                           ROOT).value('root[1]', 'nvarchar(max)'), '**'), 1,
                2, '') )
FROM    dbo.cdm
WHERE   cdm.cdm = @cdm
	-- Return the result of the function
	RETURN @cptlist

END
