-- =============================================
-- Author:		David
-- Create date: 06/27/2014
-- Description:	Provide Billing the capability of processing Quest Bill
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_Quest_Path_Payment2] 
	-- Add the parameters for the stored procedure here
	@startDate datetime = 0, 
	@endDate datetime = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @mailprofile varchar(8000);
	select @mailprofile = dbo.GetMailProfile();

    -- Insert statements for procedure here
	
	/* Step 1 Insert accounts into the table that are not already there 
		for each month, this includes credited charges that have already been
		included in a previous payment.
	*/

	; WITH cteCharges
AS
(
SELECT 	TOP(100) PERCENT dbo.chrg.credited ,
		dbo.chrg.chrg_num ,		dbo.chrg.account ,
		dbo.chrg.status ,		dbo.chrg.service_date ,
		dbo.chrg.cdm ,		--dbo.chrg.qty ,
		dbo.chrg.invoice ,
		COALESCE(NULLIF(dbo.chrg.pat_name,''), acc.pat_name) AS [pat_name] ,
		dbo.chrg.calc_amt ,
		CASE WHEN dbo.chrg.credited = 1 
			THEN chrg.mod_date
			ELSE NULL 
			END AS [credited_date]
FROM chrg
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account AND acc.cl_mnem = 'QUESTR'
INNER JOIN amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num
LEFT OUTER JOIN dbo.data_quest_global_billing_tracking t 
	ON	dbo.chrg.chrg_num = t.chrg_num
WHERE chrg.mod_date <= @endDate
AND t.chrg_num is NULL
AND chrg.cdm <> 'CBILL'
AND dbo.amt.cpt4 IN (SELECT cpt4 FROM cpt4 WHERE cdm IN (SELECT cdm FROM dbo.dict_global_billing_cdms))
ORDER BY t.date_of_service,t.account, t.cdm
)
insert into data_quest_global_billing_tracking (
chrg_num,account, pat_name,date_of_service,credited,credited_date,cdm
--,Qaccount_invoice
,amt)

SELECT  cteCharges.chrg_num, cteCharges.account ,
	cteCharges.pat_name,
	cteCharges.service_date ,
	cteCharges.credited ,
	cteCharges.credited_date,
		cteCharges.cdm ,		
		--cteCharges.invoice ,
		cteCharges.calc_amt 
		FROM cteCharges


--	select * from data_quest_global_billing_tracking
--	where qaccount_invoice = '60857'--is null
--	order by account


--	

--RETURN;

	--/* once this is done run this query and send to Jan Smith
	declare @count int
	declare @tableHtml nvarchar(max);
	declare @sub varchar(250)

	select @tableHtml = (
	N'<H1> GLOBAL BILLING for PATHOLOGIST </H1>'+
	N'<table border = "1" bordercolor ="blue">'+
	N'<tr bgcolor ="blue"><th>ACCOUNT</th><th>PATIENT</th><th>DOS</th><th>CREDITED</th>'+
	N'<th>CDM</th><th>INVOICE</th><th>TOTAL AMOUNT</th></tr>' +

	CAST (( select td = t.account,'', 
				   td = t.pat_name,'',
				   td = convert(varchar(10),date_of_service,101),'',
				   td = t.credited,'',
				   td = t.cdm,'',
				   td = coalesce(Qaccount_invoice,invoice),'',
				   td = amt,''
	from data_quest_global_billing_tracking t
	INNER JOIN chrg ON dbo.chrg.chrg_num = t.chrg_num
		AND chrg.invoice IS NOT NULL
	where (Qaccount_invoice is null) 
	--or (date_of_service between @startDate and @endDate)
	order by date_of_service,t.account, t.cdm

	for XML PATH('tr'), TYPE) as NVARCHAR(MAX))+
	N'</Table>')

	set @count = len(@tableHtml)
	if (len(@tableHtml) > 0)
	begin
	set @sub = 'PATHOLOGIST Payment for Quest as of  ' + convert(varchar(10),getdate(),101)
	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = @mailprofile,
	@recipients = 'david.kelly@wth.org',
	--@recipients = 'carol.sellars@wth.org;jan.smith@wth.org',
	--@copy_recipients=N'bradley.powers@wth.org; david.kelly@wth.org',
	@body = @tableHtml,
	@subject = @sub,
	@body_format = 'HTML';
	end

UPDATE    data_quest_global_billing_tracking
SET              Qaccount_invoice = invoice 
FROM dbo.data_quest_global_billing_tracking t
INNER JOIN chrg ON dbo.chrg.chrg_num = t.chrg_num AND chrg.invoice IS NOT NULL
WHERE t.Qaccount_invoice IS NULL 
--OR (t.date_of_service BETWEEN @startDate AND @endDate)


	
END
