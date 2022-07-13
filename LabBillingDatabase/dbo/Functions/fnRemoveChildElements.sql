







CREATE FUNCTION [dbo].[fnRemoveChildElements](@xml XML)
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


---- remove PC -- PC Segments on Cyto. c/v thin lay
--SET @count = 	
--		(SELECT @xml.value('count(data(	//FT1/FT1.7/FT1.7.1)
--			[.="5949016"] )','int'))	
--IF (@count > 0 )
--BEGIN
--	
--	SET @xml.modify(
--	'delete //FT1/.[./FT1.3/FT1.3.1 = "PC632"]')
--	SELECT @xmlReturn = (SELECT @xml)	
--END
---- handle PC segments

-- wdk 20180521
-- ANTI-CARDILLIPIN PROFILE
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.2)[.="Anti-Cardiolipin Profile"] )','int'))
--while (@count > 1 )
if (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.2 = "Cardiolipin AB IGA" or 
					./FT1.7/FT1.7.2 = "Cardiolipin AB IGM" ]')
	

	SELECT @xmlReturn = (select @xml)

	
	
END

-- End of ANTI-CARDILLIPIN PROFILE

-- wdk 20180910
-- Testosterone2
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5525242"] )','int'))
IF (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5646124"  ]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of Testosterone2

-- wdk 20180822
-- Testosterone
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5527635"] )','int'))
IF (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5527636"  ]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of Testosterone

-- wdk 20180417
-- VASCP
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5525459"] )','int'))
IF (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5527651"  ]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of VASCP


-- replace 5939337 with 5929336 at post time.
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5939337"] )','int')) 
IF (@count >= 1)
BEGIN
--SET xmlContent.modify('replace value of 
	--(//PID.18.1[1]/text()) [1]  with "5105222" ' )
	
	SET @xml.modify('replace value of 
	(//FT1/FT1.7/FT1.7.1[.="5939337"]/text()) [3]  with "5929336" ' )
	SET @xml.modify('replace value of 
	(//FT1/FT1.7/FT1.7.1[.="5939337"]/text()) [2]  with "5929336" ' )
	SET @xml.modify('replace value of 
	(//FT1/FT1.7/FT1.7.1[.="5939337"]/text()) [1]  with "5929336" ' )
--	SET @xml.modify('replace value of 
--	(//FT1/FT1.7/FT1.7.1[.="5939337"]/text()) [4]  with "5929336" ' )
END
-- end of replace 5939337 with 5929336 at post time.

-- wdk 20170317
-- VASCP
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5687084"] )','int'))
IF (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5687080" or 
					./FT1.7/FT1.7.1 = "5687078" or 
					./FT1.7/FT1.7.1 = "5687088" ]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of VASCP


-- wdk 20170211
-- IGM ABS EVAL
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5529607"] )','int'))
IF (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5527651" or 
					./FT1.7/FT1.7.1 = "5528886" or 
					./FT1.7/FT1.7.1 = "5526038" or 
					./FT1.7/FT1.7.1 = "5529073" ]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of IGM ABS EVAL

-- wdk 20160531
-- Handle Hepatitis B E antibody
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5525992"] )','int'))
IF (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5527661" or 
					./FT1.7/FT1.7.1 = "5527662" ]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of Hepatitis B E antibody

-- wdk 20160531
-- Handle Chronic Hepatitis B
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5527800"] )','int'))
IF (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5527797" or 
					./FT1.7/FT1.7.1 = "5527661" or 
					./FT1.7/FT1.7.1 = "5527662"]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of Chronic Hepatitis B 

-- wdk 20160523
-- Handle RAST ALLERG IGE QN SQ EA
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5528739"] )','int'))
IF (@count >= 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 ="5528739"]')
	SELECT @xmlReturn = (select @xml)
	
END
-- End of RAST ALLERG IGE QN SQ EA

-- wdk 20160422
-- Handle Heavy Metals Occ Bld
SET @count = 	
		(SELECT @xml.value('count(data(	//FT1/FT1.7/FT1.7.1)
			[.="5525515"] )','int'))	
IF (@count = 1)
BEGIN
	SET @xml.modify(
	'delete //FT1/.[./FT1.7/FT1.7.1 = "5525712" or 
					./FT1.7/FT1.7.1 = "5527138" or 
					./FT1.7/FT1.7.1 = "5527714" or 
					./FT1.7/FT1.7.1 = "5527715" or 
					./FT1.7/FT1.7.1 = "5527716" or 
					./FT1.7/FT1.7.1 = "5527717" ]')
	SELECT @xmlReturn = (SELECT @xml)	
END
-- end of Heavy Metals

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

--SELECT @count AS [X-RAy Surgical 5939033] wdk 20160401
SET @count = -1
SET @count = (SELECT @xml.value('count(data(
	//FT1/FT1.7/FT1.7.1)[.="5939033"] )','int'))

IF (@count >= 1)
BEGIN
	
	SET @xml.modify(
	'delete //FT1[./FT1.7/FT1.7.1 ="5939033"]')
	SELECT @xmlReturn = (select @xml)
END
-- End of Handling	5939033

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








