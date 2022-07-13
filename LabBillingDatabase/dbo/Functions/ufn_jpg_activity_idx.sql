-- =============================================
-- Author:		Bradley Powers
-- Create date: 09/15/2015
-- Description:	Return accounts eligible for demographic
--              transmission for an activity period
-- =============================================
CREATE FUNCTION [dbo].[ufn_jpg_activity_idx] 
(	
	-- Add the parameters for the function here
	@activity_date DATETIME
	 
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
WITH    cteA_index
          AS ( 
          SELECT DISTINCT acc.account, acc.cl_mnem 
		FROM acc
			LEFT OUTER JOIN pat ON pat.account = acc.account
			LEFT OUTER JOIN ins ON ins.account = acc.account
		WHERE
			acc.mod_date >= @activity_date OR
			pat.mod_date >= @activity_date OR
			ins.mod_date >= @activity_date AND
			acc.cl_mnem <> 'TBL'
        )
        , cteCDM_Index
        AS
        (
        SELECT  cdm.cdm
                    FROM    cdm
                            JOIN cpt4 ON dbo.cpt4.cdm = dbo.cdm.cdm
                    WHERE   cpt4.type = 'PC'
                            OR cpt4.cpt4 IN ( '83020', '84165', '84166',
                                              '86077', '86334', '86335', '88341' )
                    GROUP BY cdm.cdm
        )
        SELECT  DISTINCT cteA_index.account
        FROM    cteA_index
                JOIN chrg ON cteA_index.account = chrg.account
                JOIN amt ON chrg.chrg_num = amt.chrg_num
        WHERE   
            chrg.cdm IN (SELECT cdm FROM cteCDM_Index)
)
