-- =============================================
-- Author:		David
-- Create date: 06/08/2014
-- Description:	Gets the XML from the import file for an account
-- =============================================
CREATE FUNCTION GetXmlDataForAccCharges 
(
	-- Add the parameters for the function here
	@acc varchar(15) = 'C4086988',
	@file VARCHAR(50) = '14262MT.XML'
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colXml xml
)
AS
BEGIN
	-- Declare the return variable here
	--DECLARE @file VARCHAR(50)
	--SET @file = '14068mt.xml'
	DECLARE @Result XML
	DECLARE @data VARCHAR(MAX)
	SET @data = (select doc_data FROM dbo.XmlSourceTable WHERE import_file = @file)
	--SELECT @data
	DECLARE @doc XML
	SET @doc = (SELECT CONVERT(XML,@data))
	--select @doc
	SET @acc = (CASE WHEN @acc LIKE ('[C-D][1-9]%') THEN stuff(@acc,2,0,'0')
							ELSE @acc END)

	-- Add the T-SQL statements to compute the return value here
	INSERT INTO @Table_Var
			( colXml )
	
--	select @doc.query('
--	<Acc > 
--	{<file name = "{sql:variable("@file")}"/> }
--	{ for $f in .//encounter[@ID = sql:variable("@acc")]	
--		
--		return $f
--	}
--	</Acc>') as x 	
/* this returns the account with the charge data correctly	
	select @doc.query('
	<Acc > 
	{<file name = "{sql:variable("@file")}"/> }
	{ for $acc in .//encounter[@ID = sql:variable("@acc")]
		return $acc/charge
		
	}
	</Acc>') as x 	
	*/
	select @doc.query('
	<Acc number =  "{sql:variable("@acc")}"> 
	{<file name = "{sql:variable("@file")}"/> }
	{ for $acc in .//encounter[@ID = sql:variable("@acc")]
		
		return 		
			
			<CDM>{$acc/charge/billcode/text()}
				<qty>{$acc/charge/quantity/text()}</qty>
				
			</CDM>
		
	}
	
	</Acc>') as x 	
--	<mt_mnem>{$acc/charge/test/text()}</mt_mnem>
--				<psite>{$acc/charge/performingsite/text()}</psite>
	
	RETURN

END
