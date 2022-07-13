-- =============================================
-- Author:		Bradley Powers
-- Create date: 11/5/2013
-- Description:	Returns a table of icd9 codes for a patient
-- =============================================
CREATE FUNCTION [dbo].[GetPatIcd92] 
(	
	-- Add the parameters for the function here
	@account varchar(15) 
	 
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	SELECT [pati].[account], [pati].[link],[pati].icd_indicator, [pati].[icd9], [icd9desc].[icd9_desc]
	FROM
	(
		select account, ROW_NUMBER() OVER(PARTITION BY account ORDER BY account) as link
		, icd_indicator,icd9
		from
		(
			select account, dbo.pat.icd_indicator, icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 from pat
			where pat.account = @account
		) as pat_icd9
		UNPIVOT
		(
			icd9 FOR icd9s IN (icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9)
		) AS UP
	) [pati]
	JOIN [acc] on [pati].[account] = [acc].[account]
	LEFT OUTER JOIN [icd9desc] on [pati].[icd9] = [icd9desc].[icd9_num] and [icd9desc].[AMA_year] = CONVERT(VARCHAR(6), [dbo].GetAMAYear([acc].[trans_date]))
	WHERE [pati].[account] = @account

)
