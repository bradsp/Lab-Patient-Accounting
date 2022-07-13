-- =============================================
-- Author:		Bradley Powers
-- Create date: 07/20/2015
-- Description:	This procedure queries PSA 
--              demographic activity for the day 
--              and sends an email if there is 
--              no activity.
-- =============================================
CREATE PROCEDURE [dbo].[usp_psa_demo_activity_check] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @beginDate DATETIME;
	DECLARE @endDate DATETIME;
	DECLARE @todayDate DATETIME;
	DECLARE @emailbody varchar(3000);
	
	SET @todayDate = GETDATE();
	--SET @todayDate = CONVERT(DATETIME,'2015-07-17 07:00');
	SET @beginDate = DATEADD(Day,0,DATEDIFF(Day,0,@todayDate-5));
	SET @endDate = DATEADD(Day, 0, DATEDIFF(Day, 0, @todayDate-4));

	IF NOT EXISTS (SELECT 1 FROM dbo.ufn_jpg_activity_idx(GETDATE()-1))
	BEGIN
		--PRINT 'no records found'
		
		SET @emailbody = 
		N'There is no activity for MCL_DEMO file date  '+ CONVERT(varchar(10),@todayDate, 10) +
		N'.<br /> No file was generated.' +
		N'<br />For further information, contact Bradley Powers (bradley.powers@wth.org)';
		--+ N'<br /><br />This is a test. Please disregard this message.';

		--send the email
		EXEC msdb.dbo.sp_send_dbmail
			@recipients=N'audra.steen@mckesson.com;blenda.wilson@mckesson.com;Kensley.Ledford@McKesson.com;Leigh.Hall@McKesson.com',
			@copy_recipients=N'bradley.powers@wth.org',
			@body=@emailbody,
			@body_format = 'HTML',
			@subject ='Medical Center Laboratory - NO MCL Demo activity',
			@profile_name ='WTHMCLBILL';
		
	END
END
