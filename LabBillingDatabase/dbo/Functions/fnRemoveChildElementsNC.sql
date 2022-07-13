CREATE	 FUNCTION dbo.fnRemoveChildElementsNC(@xml XML)
RETURNS XML
WITH RETURNS NULL ON NULL INPUT,
	SCHEMABINDING AS 
BEGIN
	DECLARE @xmlReturn XML
	SET @xmlReturn = @xml
	DECLARE @count INT
	SET @count = -1

-- remove NC -- No charge segments
SET @count = 	
		(SELECT @xml.value('count(data(	//FT1/FT1.6/FT1.6.1)
			[.="NC"] )','int'))	
IF (@count > 0 )
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.6/FT1.6.1 = "NC"]')
	SELECT @xmlReturn = (SELECT @xml)	
END
-- handle NC segments

-- Handle IMMUNOGLOBULINS
SET @count = 	
		(SELECT @xml.value('count(data(	//FT1/FT1.7/FT1.7.1)
			[.="5602280"] )','int'))	
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5602282" or 
					./FT1.7/FT1.7.1 = "5602284" or 
					./FT1.7/FT1.7.1 = "5602286" ]')
	SELECT @xmlReturn = (SELECT @xml)	
END
-- end of IMMUNOGLOBULINS

-- Handle CYTOMEGALOVIRUS 
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5646144"] )','int'))
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 ="5646146" or 
					./FT1.7/FT1.7.1 ="5646148"]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of CYTOMEGALOVIRUS

-- Handle TOXOPLASMA wdk 01/29/2016 
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5529197"] )','int'))

--SELECT @count AS [TOXOPLASMA Count]
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 ="5525696" or 
					./FT1.7/FT1.7.1 ="5526038"]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of TOXOPLASMA

-- Handle Quad problem
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5525216"] )','int'))

--SELECT @count AS [Quad Count]
IF (@count = 1)
BEGIN	
	SET @xml.modify(
	'delete //FT1[./FT1.7/FT1.7.1 ="5528251" or 
				  ./FT1.7/FT1.7.1 ="5527824" or 
				  ./FT1.7/FT1.7.1 ="5527515" or 
				  ./FT1.7/FT1.7.1 ="5528253"]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of Handling	QUAD

-- Handle TESTOSTERONE TOTAL&FREE and TESTOSTERONE, FREE issue
set @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5525242"] )','int'))

--SELECT @count AS [Testosterone Count]
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 ="5527635" or 
					./FT1.7/FT1.7.1 ="5527636"]')
	SELECT @xmlReturn = (select @xml)
END
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5525242"] )','int'))

--SELECT @count AS [Testosterone Count] 02/25/2016
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 ="5527635"]')
	SELECT @xmlReturn = (select @xml)
END
-- End of Handling TESTOSTERONE TOTAL&FREE and TESTOSTERONE, FREE issue


-- handle Rickettsia 
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5525223"] )','int'))

--SELECT @count AS [Rickettsia Count 5525223]
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1[./FT1.7/FT1.7.1 ="5525674" or 
				  ./FT1.7/FT1.7.1 ="5528841"]')
	SELECT @xmlReturn = (select @xml)
	
END


SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5525674"] )','int'))

--SELECT @count AS [Rickessetta Count 5525674]
IF (@count = 1)
BEGIN
	
	SET @xml.modify(
	'delete //FT1[./FT1.7/FT1.7.1 ="5528841"]')
	SELECT @xmlReturn = (select @xml)
END
-- End of Handling	rickettsia

-- Handle MYASTHENIA GRAVIS, ADULT 02/24/2016
SET @count = -1
SET @count = 	
	(SELECT @xml.value('count(data(	//FT1/FT1.7/FT1.7.1)
		[.="5528633"] )','int'))	
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5528634" or 
					./FT1.7/FT1.7.1 = "5528634" or 
					./FT1.7/FT1.7.1 = "5527719" ]')
	SELECT @xmlReturn = (SELECT @xml)	
END
-- end of MYASTHENIA GRAVIS, ADULT

-- Handle INSULIN, FREE AND TOTAL 02/24/2016
SET @count = -1
SET @count = 	
	(SELECT @xml.value('count(data(	//FT1/FT1.7/FT1.7.1)
		[.="5529341"] )','int'))	
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5525624" or 
					./FT1.7/FT1.7.1 = "5527764" ]')
	SELECT @xmlReturn = (SELECT @xml)	
END
-- end of INSULIN, FREE AND TOTAL

-- Handle THYROGLOBULIN TUMOR-MAYO 02/25/2016
SET @count = -1
SET @count = 	
	(SELECT @xml.value('count(data(	//FT1/FT1.7/FT1.7.1)
		[.="5529849"] )','int'))	
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5528808" or 
					./FT1.7/FT1.7.1 = "5528806" ]')
	SELECT @xmlReturn = (SELECT @xml)	
END
-- end of THYROGLOBULIN TUMOR-MAYO

--SELECT @count AS [VMA RANDOM URINE 5529904]
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5529904"] )','int'))

IF (@count = 1)
BEGIN
	
	SET @xml.modify(
	'delete //FT1[./FT1.7/FT1.7.1 ="5528590"]')
	SELECT @xmlReturn = (select @xml)
END
-- End of Handling	VMA RANDOM URINE

------------------place removes above this line ----------------
SET @xml.modify(
	'delete //PR1')
	SELECT @xmlReturn = (select @xml)

-- Handle NO FT1's
SET @count = (SELECT @xml.value('count(data(//FT1) )','int'))
IF (@count = 0)
BEGIN	
	SELECT @xmlReturn = NULL
END

RETURN @xmlReturn
END
