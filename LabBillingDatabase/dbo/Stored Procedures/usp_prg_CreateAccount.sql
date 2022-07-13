-- =============================================
-- Author:		David
-- Create date: 03/09/2016
-- Description:	Creates a new account
-- =============================================
CREATE PROCEDURE usp_prg_CreateAccount 
	-- Add the parameters for the stored procedure here
	@client varchar(10) , 
	@dos datetime 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @client = COALESCE(NULLIF(@client,''),'TBL')
	SET @dos = COALESCE(
		CONVERT(DATETIME,convert(varchar(10),NULLIF(@dos,''),101) )
		,CONVERT(DATETIME,convert(varchar(10),GETDATE(),101) ))
    -- Insert statements for procedure here
	SELECT @client, @dos

	DECLARE @acc VARCHAR(15)
	
	SELECT @acc = (	
	SELECT cnt FROM dbo.number
	WHERE dbo.number.keyfield = 'account')

	UPDATE number 
	SET cnt = @acc+1
	WHERE dbo.number.keyfield = 'Account'
	
	SELECT @acc = STUFF(@acc,1,0,'D')
	
	SELECT @acc
	INSERT INTO dbo.acc
			(
				deleted ,
				account ,
				pat_name ,
				cl_mnem ,
				fin_code ,
				trans_date ,
				cbill_date ,
				status ,
				ssn ,
				num_comments ,
				meditech_account ,
				original_fincode ,
				oereqno ,
				mri ,
				post_date ,
				ov_order_id ,
				ov_pat_id ,
				mod_date ,
				mod_user ,
				mod_prg ,
				mod_host ,
				bill_priority ,
				guarantorID ,
				HNE_NUMBER ,
				trans_date_time ,
				tdate_update
			)
	VALUES	(
				0 , -- deleted - bit
				@acc , -- account - varchar(15)
				'' , -- pat_name - varchar(40)
				@client , -- cl_mnem - varchar(10)
				'' , -- fin_code - varchar(10)
				@dos , -- trans_date - datetime
				'' , -- cbill_date - datetime
				'' , -- status - varchar(8)
				'' , -- ssn - varchar(11)
				0 , -- num_comments - int
				'' , -- meditech_account - varchar(15)
				'' , -- original_fincode - varchar(1)
				'' , -- oereqno - varchar(15)
				'' , -- mri - varchar(25)
				NULL , -- post_date - datetime
				'' , -- ov_order_id - varchar(50)
				'' , -- ov_pat_id - varchar(50)
				GETDATE() ,
				RIGHT(SUSER_SNAME(),50) ,
				COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
						'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50),'NO APP IDENTIFIED') ,
				RIGHT (HOST_NAME(),50) , -- mod_date - datetime
				0 , -- bill_priority - int
				'' , -- guarantorID - varchar(50)
				'' , -- HNE_NUMBER - varchar(50)
				null , -- trans_date_time - datetime
				NULL  -- tdate_update - bit
			)
	
END
