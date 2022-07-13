-- =============================================
-- Author:		David Kelly
-- Create date: 08/14/2014
-- Description:	Returns the account balance based on the date
-- =============================================
CREATE FUNCTION [dbo].[GetPaymentsByDate2] 
(
	-- Add the parameters for the function here
	@account varchar(15),
	@LastDataMailerDate DATETIME
)
--RETURNS NUMERIC(18,2)
RETURNS @tableVar TABLE
(
	colSource varchar(100),
	colPaidDate datetime,
	colPaidPrev numeric(7,2),
	colPaidCur numeric(7,2)
)
AS
BEGIN

	--get total of payment records this should be done after checks are posted
--	WITH cteChkPrev ([account], [source],[PriorChkTotal] )
--	AS
--	(
--	select [chk].[account]
--	, CASE WHEN source IN ('sp','',NULL,'1500 SELF PAY',
--	'1500 SELF PAY DISC','20% OFF SELF PAY','25% SELF  DISC','25% SELF PAY',
--	'25% SELF PAY DISC','25% SELF PAY DISC.','25% SELF PAY DISCOUN','25% SELF-PAY DISC',
--	'25% SELFPAY DISCOUNT','25% SP DISCOUNT','32% SP DISCOUNT','32% SP DISCOUNT','PATIENT',
--	'SELF PAY','SELP PAY','SELF PAY','SELF PAY & PROMPT ','SELF PAY 25%','SELF PAY ADJ','SELF PAY BILL AT MCR','SELF PAY CORRECTION',
--'SELF PAY DIS','SELF PAY DISC','SELF PAY DISC 40%','SELF PAY DISC. 25%',
--'SELF PAY DISCOUN','SELF PAY DISCOUNT','SELF PAY DISOUNT','SELF PAY/EMPLOYEE',
--'SELFPAY','SELF-PAY','SELF-PAY DISCOUNT','SELFPAY/EARLY PAY DI') THEN 'SP'
--ELSE dbo.chk.source END AS [source]
--	
--		, SUM(ISNULL([amt_paid],0.00)+ISNULL([write_off],0.00)+ISNULL([contractual],0.00)) 
--			 as 'chktotal'
--		from chk
--		WHERE 
--			[chk].[status] <> 'CBILL' AND [chk].[mod_date] <= @LastDataMailerDate
--			and [chk].[account] = @account
--		GROUP BY [chk].[account],source--,CONVERT(varchar(10)--, [chk].[date_rec],101)
--	)
--	, cteChkCur ([account], [source],[dateRec],[ChkTotal] )
--	AS
--	(
--	select [chk].[account], source
--		, CONVERT(varchar(10), [chk].[date_rec],101) AS [dateRec]
--		, SUM(ISNULL([amt_paid],0.00)+ISNULL([write_off],0.00)+ISNULL([contractual],0.00)) 
--			 as 'chktotal'
--		from chk
--		WHERE 
--			[chk].[status] <> 'CBILL' AND [chk].[mod_date] > @LastDataMailerDate
--			and [chk].[account] = @account
--		GROUP BY [chk].[account],source,CONVERT(varchar(10), [chk].[date_rec],101)
--	)
--	, cteChks (account, source, dateRec, chkTotal)
--	AS
--	(
--		SELECT ccp.account, ccp.source, @LastDataMailerDate, ccp.[priorchktotal] FROM cteChkPrev ccp
--		UNION ALL
--		SELECT ccc.* FROM  cteChkCur ccc
--	)
	; WITH cte AS
	(
	SELECT 	dbo.chk.account ,
			CASE WHEN dbo.chk.date_rec <= @LastDataMailerDate THEN @LastDataMailerDate
			ELSE chk.date_rec END AS [dateRec],
			(dbo.chk.amt_paid +		dbo.chk.write_off +		dbo.chk.contractual) AS [paid] ,
			COALESCE(CASE WHEN source IN ('sp','',NULL,'1500 SELF PAY',
		'1500 SELF PAY DISC','20% OFF SELF PAY','25% SELF  DISC','25% SELF PAY',
		'25% SELF PAY DISC','25% SELF PAY DISC.','25% SELF PAY DISCOUN','25% SELF-PAY DISC',
		'25% SELFPAY DISCOUNT','25% SP DISCOUNT','32% SP DISCOUNT','32% SP DISCOUNT','PATIENT',
		'SELF PAY','SELP PAY','SELF PAY','SELF PAY & PROMPT ','SELF PAY 25%','SELF PAY ADJ','SELF PAY BILL AT MCR','SELF PAY CORRECTION',
	'SELF PAY DIS','SELF PAY DISC','SELF PAY DISC 40%','SELF PAY DISC. 25%',
	'SELF PAY DISCOUN','SELF PAY DISCOUNT','SELF PAY DISOUNT','SELF PAY/EMPLOYEE',
	'SELFPAY','SELF-PAY','SELF-PAY DISCOUNT','SELFPAY/EARLY PAY DI') THEN 'SP'
	ELSE dbo.chk.source END,NULLIF(dbo.chk.write_off_code,''),NULLIF(dbo.chk.ins_code,'') ) AS [source] 
	FROM dbo.chk
	WHERE dbo.chk.account = @account -- payment received after pat bills and not in this months bills!
	--'c7115836' --payment received on the date of pat bills not in last months file !
	--'c2516543'
	--'C4019666'--
	--'c3724659' --
	--'c3994518'-- 
	)
--	SELECT 	cte.dateRec ,
--			SUM(cte.paid) AS [Paid] ,
--			cte.source 
--	FROM cte
--	GROUP BY cte.source,cte.dateRec
--	ORDER BY cte.dateRec
	INSERT INTO @tableVar(colSource, colPaidDate,colPaidPrev, colPaidCur)
	SELECT 	cte.source ,cte.dateRec ,
			CASE WHEN cte.dateRec <= @LastDataMailerDate THEN SUM(cte.paid)
				ELSE 0.00 END
			,CASE WHEN cte.dateRec <= @LastDataMailerDate THEN 0.00
				ELSE SUM(cte.paid) end
			
	FROM cte
	GROUP BY cte.source,cte.dateRec
	ORDER BY cte.dateRec
--	SELECT source, dateRec
--	, CASE WHEN dateRec <= @LastDataMailerDate THEN ISNULL(chktotal,0.00) ELSE 0.00 END	
--	, CASE WHEN daterec > @LastDataMailerDate THEN ISNULL(chktotal,0.00) ELSE 0.00 END	
--	FROM  cteChks 
--	order BY dateRec,source

	-- Return the result of the function	
	RETURN 

END
