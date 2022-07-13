-- =============================================
-- Author:		David
-- Create date: 04/11/2016
-- Description:	Gets the XML from the import file for an account
-- =============================================
CREATE FUNCTION GetXmlDataForAccInfce 
(
	-- Add the parameters for the function here
	--@acc varchar(15),
	@msg NUMERIC(18,0)
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAcc VARCHAR(15),
	colRank INT,
	colDiagnosis VARCHAR(7)
	--colXml xml
)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @acc VARCHAR(15)
	DECLARE @Result XML
	DECLARE @data VARCHAR(MAX)
	SET @data = 
	(select msgContent 
	FROM infce.messages_inbound 
	WHERE infce.messages_inbound.systemMsgId = @msg)
	--SELECT @data
	DECLARE @doc XML
	SET @doc = (SELECT CONVERT(XML,REPLACE(@data,'"','') ))
	--select @doc
	SET @acc = (
	SELECT 'L'+account_cerner FROM infce.messages_inbound
	WHERE infce.messages_inbound.systemMsgId = @msg)

	-- Add the T-SQL statements to compute the return value here
	INSERT INTO @Table_Var
			( colAcc, colRank, colDiagnosis )
	
	SELECT  DISTINCT @acc AS [account], 
	ROW_NUMBER() OVER (ORDER BY @acc),
	code.doc.value('fn:distinct-values(data(.))', 'nvarchar(10)') AS [ICD]
	--ind.alias.value('fn:distinct-values(data(.))', 'nvarchar(10)') AS [IND]
	from 
	@doc.nodes('//FT1.19.1[./../../FT1.1/FT1.1.1/. = "1"]') AS code(doc) 
	--outer APPLY	@xml.nodes('//FT1.19.3[./../../FT1.1/FT1.1.1/. = "1"]') AS ind(alias) 
	
--SELECT --ROW_NUMBER() OVER (ORDER BY cteDx.account) AS [rn],
--cteDX.account, 
----CASE WHEN ISNULL(cteDX.colIndicator,'I10') = 'I10'
----	THEN REPLACE(cteDX.colDiagnosis,'.','')
----	ELSE cteDX.colDiagnosis END AS [colDiagnosis]
----, ISNULL(cteDX.colIndicator,'I9') AS [cteDX.colIndicator]
--
--FROM cteDx

--SELECT * from #temp
	
	RETURN

END
