-- =============================================
-- Author:		David
-- Create date: 04/11/2016
-- Description:	Gets the XML from the import file for an account
-- =============================================
CREATE FUNCTION GetXmlDataForAccIns 
(
	-- Add the parameters for the function here
	@acc varchar(15)
	--,@msg NUMERIC(18,0)
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAcc VARCHAR(15),
	colInsOVCode VARCHAR(50),
	colInsName VARCHAR(50),
	colInsCode VARCHAR(25),
	colFinCode VARCHAR(10)
	
)
AS
BEGIN
	-- Declare the return variable here
	--DECLARE @acc VARCHAR(15)
	DECLARE @Result XML
	DECLARE @data VARCHAR(MAX)
	SET @data = 
	(select TOP(1) msgContent 
	FROM infce.messages_inbound 
	WHERE 'L'+infce.messages_inbound.account_cerner = @acc
		ORDER BY infce.messages_inbound.msgDate)
	--SELECT @data
	DECLARE @doc XML
	SET @doc = (SELECT CONVERT(XML,REPLACE(@data,'"','') ))
	--select @doc
--	SET @acc = (
--	SELECT 'L'+account_cerner FROM infce.messages_inbound
--	WHERE infce.messages_inbound.systemMsgId = @msg)

	-- Add the T-SQL statements to compute the return value here
	INSERT INTO @Table_Var
			(
				colAcc ,
				colInsOVCode ,
				
				colInsName ,
				colFinCode ,
				colInsCode
			)

	
	SELECT  DISTINCT @acc AS [account], 
	code.doc.value('(//IN1.2.1/text() )[1]','varchar(50)') AS [InsOVCode],
	code.doc.value('(//IN1.2.2/text() )[1]','varchar(50)') AS [InsName],
	dbo.GetMappingValue('FIN_CODE','CERNER',code.doc.value('(//IN1.2.1/text() )[1]','varchar(50)') ) AS [MappedFinCode],
	dbo.GetMappingValue('INS_CODE','CERNER',code.doc.value('(//IN1.2.1/text() )[1]','varchar(50)') ) AS [MappedInsCode]
	--code.doc.value('fn:distinct-values(data(.))', 'nvarchar(10)') AS [OV_INS_CODE]
	--,code.doc.value('fn:distinct-values(data(./IN1.2.2/.))', 'nvarchar(50)') AS [INS_NAME]																			

	--ind.alias.value('fn:distinct-values(data(.))', 'nvarchar(10)') AS [IND]
	from 
		@doc.nodes('//IN1') AS code(doc)
		-- @doc.nodes('//IN1.2.1[./../../IN1.1/IN1.1.1/. = "1"]') AS code(doc)
		--@doc.nodes('//IN1.2.1') AS code(doc)

	
	RETURN

END
