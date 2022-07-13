-- =============================================
-- Author:		David
-- Create date: 07/17/2014
-- Description:	Get the Data for the bad debt file
-- =============================================
CREATE PROCEDURE usp_prg_BadDebt 
	-- Add the parameters for the stored procedure here
	@date varchar(10) = null, 
	@outfile varchar(50) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT @date, @outfile
	
	SELECT 	dbo.PadText(debtor_last_name,20) AS [debtor_last_name] , --
		dbo.PadText(debtor_first_name,15) AS [debtor_first_name] ,--
		dbo.PadText(st_addr_1,25) AS [st_addr_1],
		dbo.PadText(st_addr_2,25) AS [st_addr_2] ,
		dbo.PadText(city,18) AS [city] ,
		dbo.PadText(state_zip,15) AS [state_zip],
		dbo.PadText(spouse,15) AS [spouse] ,
		dbo.PadText(phone,12) AS [phone] ,
		dbo.PadText(soc_security,10) AS [soc_security] ,
		dbo.PadText(license_number,20) AS [license_number]  ,
		dbo.PadText(employment,35) AS [employment] ,
		dbo.PadText(remarks,35) AS [remarks] ,
		dbo.PadText(account_no,25) AS [account_no} ,
		dbo.PadText(patient_name,20) as [patient_name] ,
		dbo.PadText(remarks2,35) AS [remarks2] ,
		dbo.PadText(misc,29) AS [misc] ,
		REPLACE(CONVERT(VARCHAR(8),service_date,1),'/','') AS [service_date], --
		SPACE(6) AS [payment_date], --REPLACE(CONVERT(VARCHAR(8),payment_date,1),'/','') as [payment_date], --
		dbo.PadText(convert(VARCHAR(20),balance,0),10) AS [balance],
		date_entered ,
		date_sent 
FROM dbo.bad_debt 
WHERE date_sent IS NULL AND DELETED = 0

END
