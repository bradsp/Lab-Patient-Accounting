-- =============================================
-- Author:		DAVID
-- Create date: 08/21/2014
-- Description:	Returns the checks before last DataMailer
-- =============================================
CREATE FUNCTION [dbo].[GetPaymentsTableSinceLastDataMailer] 
(
	-- Add the parameters for the function here
	@account varchar(15)
)
RETURNS @Table_Var TABLE
(
	colCHK VARCHAR(85)
)
AS
BEGIN
	--gets payment records this should be done after checks are posted
	--schedule daily at 8 pm 
	WITH cteChk (rDate, source, [ChkTotal])
	AS
	(
	select --[chk].[account], 
	COALESCE(chk.date_rec,chk.mod_date) AS [rDate],
	CASE WHEN dbo.chk.write_off_code = 1400
		THEN REPLACE(dbo.PadText('SP DISC',20),' ',' ') 
		ELSE REPLACE(dbo.PadText([chk].[source],20),' ',' ') END AS [source]
	, (ISNULL(NULLIF([amt_paid],''),0.00)+
		ISNULL(NULLIF([write_off],''),0.00)+
		ISNULL(NULLIF([contractual],''),0.00)) as 'chktotal'
		from chk
		INNER JOIN pat ON dbo.pat.account = dbo.chk.account
		WHERE 
			[chk].[status] <> 'CBILL' AND [chk].[mod_date] > pat.last_dm
			and [chk].[account] = @account
		--GROUP BY [chk].[account]
	)

	--SELECT @chkTotal =  ISNULL(cteChk.ChkTotal,0.00)
	INSERT INTO @Table_Var
			( colCHK )
	SELECT SPACE(5)+CONVERT(VARCHAR(8),cteChk.rDate,1)
	+' '+
		dbo.PadText(cteChk.source,24)
		--+':'+
--		--SPACE(30-LEN(cteChk.source)+(10-LEN(CAST((cteChk.ChkTotal*-1) AS VARCHAR(10)))))+
--		--CAST((cteChk.ChkTotal*-1) AS VARCHAR(10))+
		+dbo.PadText(CAST(ISNULL(cteChk.ChkTotal*-1,0.00)AS VARCHAR(12)),-18)
				
	FROM  cteChk 

	-- Return the result of the function
	RETURN --@chkTotal

END
