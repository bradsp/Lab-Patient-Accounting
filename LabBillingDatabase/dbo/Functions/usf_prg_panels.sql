-- =============================================
-- Author:		David
-- Create date: 09/11/2015
-- Description:	Checks an account for the panels
-- =============================================
CREATE FUNCTION usf_prg_panels 
(
	-- Add the parameters for the function here
	@acc varchar(15) = '', 
	@cdm varchar(7) = ''
	
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAccount varchar(15), 
	colCdm varchar(7),
	colMnemonic VARCHAR(25),
	colDescription VARCHAR(125),
	colBillcodes VARCHAR(8000),
	colChrgNums	varchar(8000)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
-----------------------------------------------------
; WITH ctePanel
AS
(
select 
         ISNULL(cdm.mnem,'') as Mnemonic
       , cdm.cdm
       , cdm.descript as 'Panel Description'
       , (STUFF(ISNULL((SELECT '; ' + ISNULL(dbo.cpt4.billcode,'') --+ COALESCE('-' + cpt4.modi,'') 
              --+ CASE WHEN COUNT(cpt4) > '1' THEN ' x ' + CAST(COUNT(cpt4) AS VARCHAR) ELSE '' END
              from cpt4
              WHERE cpt4.cdm = cdm.cdm and cpt4.deleted = 0
              GROUP BY billcode--cpt4,modi
              ORDER by billcode--cpt4
              FOR XML PATH(''), TYPE, ROOT).value('root[1]','nvarchar(max)'),'**'),1,2,''))
              as billcodes--cptcodes
from cdm
WHERE NULLIF(cdm.mnem,'') IS NOT NULL AND deleted = 0
--AND  cdm = @cdm
)
, cteAcc
AS
(
	SELECT DISTINCT account, 
	(STUFF(ISNULL((SELECT '; '+ISNULL(cdm,'')
	FROM dbo.chrg
	WHERE dbo.chrg.account = @acc AND dbo.chrg.credited = 0
		AND cdm = ANY (
			SELECT billcode FROM cpt4 WHERE cdm = @cdm
			AND NULLIF(cdm,'') IS NOT NULL
			AND dbo.cpt4.DELETED = 0
			)
		AND NULLIF(cdm,'') IS NOT NULL
	GROUP BY dbo.chrg.account,cdm
	ORDER BY dbo.chrg.account,cdm 
	FOR XML PATH(''),TYPE,ROOT).value('root[1]','nvarchar(max)'),'**'),1,2,''))
		AS billcodes
	
	,(STUFF(ISNULL((SELECT ', '+ISNULL(CAST(
		CASE WHEN cdm = @cdm THEN NULL	
			ELSE chrg_num END AS VARCHAR),'')
	FROM dbo.chrg
	WHERE dbo.chrg.account = @acc AND dbo.chrg.credited = 0
		AND cdm = ANY (
			SELECT billcode FROM cpt4 WHERE cdm = @cdm
			AND NULLIF(cdm,'') IS NOT NULL
			AND dbo.cpt4.DELETED = 0
			)
		AND NULLIF(cdm,'') IS NOT NULL
	GROUP BY dbo.chrg.account,chrg_num, dbo.chrg.cdm
	ORDER BY dbo.chrg.account,chrg_num 
	FOR XML PATH(''),TYPE,ROOT).value('root[1]','nvarchar(max)'),'**'),1,2,''))
		AS chrg_nums
		
	FROM dbo.chrg
	WHERE dbo.chrg.account = @acc
)
	INSERT INTO @Table_Var
			(
				colAccount ,
				colCdm ,
				colMnemonic ,
				colBillcodes ,
				colDescription,
				colChrgNums
			)


SELECT TOP(1) @acc, @cdm, 
	ctePanel.Mnemonic, ctePanel.billcodes,
	ctePanel.[Panel Description], cteAcc.chrg_nums
FROM cteAcc
INNER JOIN ctePanel ON ctePanel.billcodes = cteAcc.billcodes
 AND ( NULLIF(ctePanel.billcodes,'') IS NOT NULL
	OR 
	 NULLIF(cteAcc.billcodes,'') IS NOT NULL )
WHERE cteAcc.account = @acc




-----------------------------------------------------
	
			
	RETURN 
END
