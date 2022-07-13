
-- =============================================
-- Author:		David
-- Create date: FormatFinCode
-- Description:	formats the FinCode from the ADT/ORM/DFT message input
-- =============================================
CREATE FUNCTION [dbo].[FormatFinCode] 
(
	-- Add the parameters for the function here
	@client VARCHAR(50),
	@fin varchar(50) 
)
RETURNS VARCHAR(10)

AS
BEGIN
	-- Fill the table variable with the rows for your result set
--DECLARE @finMapped VARCHAR(10)
SET @fin = dbo.GetMappingValue('fin_code','cerner',@fin)
-- client bill only
;WITH cteY
AS
(
SELECT dbo.client.cli_mnem --,		dbo.client.cli_nme ,		dbo.client.type  
FROM dbo.client WHERE dbo.client.cli_mnem IN(
'EHS' -- Employee Health
,'EMIV'-- –Employee Intervention
,'WTTC'-- – West Tennessee Transitional Care
,'JAH'-- – Hospice
,'LLBS'-- – Lifeline Blood Services
,'LLWEP' --– Living Wellness
,'MAC'-- – Mac Screenings
,'CANCW'-- – Cancer Care Wellness account
,'OHBC'-- - Oak Hill Behavioral center
,'CLRS' -- Clinical Research Solutions
)
AND dbo.client.deleted = 0
--ORDER BY dbo.client.cli_mnem
UNION 
SELECT dbo.client.cli_mnem --,		dbo.client.cli_nme ,		dbo.client.type 
FROM dbo.client WHERE type = 6
AND dbo.client.deleted = 0
UNION 

SELECT 
		dbo.client.cli_mnem --,		dbo.client.cli_nme ,		dbo.client.type 
		 FROM dbo.client WHERE 
dbo.client.cli_mnem LIKE '%2' AND type <> 6
AND dbo.client.deleted = 0

UNION
SELECT 
		dbo.client.cli_mnem --,		dbo.client.cli_nme ,		dbo.client.type 
		 FROM dbo.client WHERE 
dbo.client.cli_mnem LIKE 'MP%' AND type <> 6
AND dbo.client.deleted = 0

)
Select @fin =  COALESCE(
(SELECT CASE WHEN cteY.cli_mnem IS NULL	THEN @fin
	else 'Y' end
 FROM cteY WHERE cteY.cli_mnem = @client),@fin)
---- end of client bill only

-- never a client bill
IF (EXISTS (SELECT cli_mnem FROM client 
			WHERE dbo.client.cli_mnem = @client
			AND type = 3 AND dbo.client.cli_mnem 
			NOT IN ('EHS','EMIV','JAH','MAC','HEAW') ) )
BEGIN
 SET @fin = COALESCE(NULLIF(@fin,'Y'),'E')
-- ISNULL(NULLIF(dbo.GetMappingValue('FIN_CODE','CERNER',
--  (SELECT TOP(1) 
--		CAST(infce.messages_inbound.msgContent AS XML).value(
--		'data(//IN1.2.1/text()) [1]','varchar(10)')
--		from infce.messages_inbound
--	WHERE infce.messages_inbound.account_cerner = @cAcc
-- AND infce.messages_inbound.msgType = 'ADT-A04'
-- ORDER BY infce.messages_inbound.systemMsgId desc)
-- ),'K'),'E')
END	 
-- end of never a client bill
	
	

	
	RETURN LTRIM(RTRIM(UPPER(@fin)))
END

