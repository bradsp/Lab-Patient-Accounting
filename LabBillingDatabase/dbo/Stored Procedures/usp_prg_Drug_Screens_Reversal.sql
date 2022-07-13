-- =============================================
-- Author:		David
-- Create date: 01/02/2014
-- Description:	Handle Medicare drug screens
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_Drug_Screens_Reversal] 
	-- Add the parameters for the stored procedure here
	@OrigChrgNum numeric(18,0) =0  
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @chrgRowguid uniqueidentifier
	set @chrgRowguid = newid()
	declare @chrg_num numeric(18,0)

	--SELECT @OrigChrgNum, @chrgRowguid, @chrg_num

	declare @credited int
	set @credited = (select credited from chrg where chrg_num = @OrigChrgNum)
	if (@credited = 0)
	BEGIN

	update chrg
	set credited = 1
	where chrg_num = @OrigChrgNum	

	-- 1. insert the counter charge into the table
	INSERT INTO chrg
	(rowguid, credited, account, [status], service_date, hist_date, cdm, qty, retail, inp_price, comment
	,  mod_date, mod_user, mod_prg, net_amt, fin_type, mod_host, mt_req_no, fin_code, performing_site
	, bill_method)

	SELECT     
	@ChrgRowguid, credited, account, [status], service_date, getdate(), cdm, qty*-1, retail, inp_price, comment
	,  getdate(), right(suser_sname(),50)
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'SQL TRIGGER ' +CONVERT(VARCHAR(10),GETDATE(),112)),50)
	, net_amt, fin_type, right(mod_host,50), mt_req_no, fin_code, performing_site,  bill_method
	FROM         chrg AS chrg_1
	WHERE     (chrg_1.chrg_num = @OrigChrgNum)
	
	SET @chrg_num = @@IDENTITY
--	PRINT @chrg_num

	--set @chrg_num = (select chrg_num from chrg where rowguid = @ChrgRowguid)
	--PRINT @chrg_num
	
	--SELECT COALESCE(@chrg_num,'SELECT DID not work')
	INSERT INTO amt
	(chrg_num, cpt4, [type], amount, mod_date, mod_user, mod_prg, deleted,  modi, revcode, modi2
	, diagnosis_code_ptr,  mt_req_no, order_code, bill_type, bill_method)


	SELECT     
	@chrg_num, cpt4, [type], amount, getdate(), right(suser_sname(),50)
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'SQL TRIGGER ' +CONVERT(VARCHAR(10),GETDATE(),112)),50)
	, deleted,  modi, revcode, modi2
	, diagnosis_code_ptr,  mt_req_no, order_code, bill_type, bill_method
	FROM         amt AS amt_1
	WHERE     (amt_1.chrg_num = @OrigChrgNum)


	-- now add old drug screen back 
--	DECLARE @OrigChrgNum NUMERIC(18,0)
--SET @OrigChrgNum = 90465
--DECLARE @chrg_num NUMERIC(18,0)
--SET @chrg_num = 90466
--DECLARE @ChrgRowguid UNIQUEIDENTIFIER	
	set @ChrgRowguid = newid()
	DECLARE @origCDM VARCHAR(7)
	SET @origCDM = (SELECT CAST(SUBSTRING(comment,CHARINDEX('[',comment)+1,7)AS VARCHAR(7)) FROM chrg WHERE chrg_num = @OrigChrgNum)
	SELECT @origCDM
	INSERT INTO chrg
	(rowguid, credited, account, [status], service_date, hist_date, cdm, qty
	, retail, inp_price, comment
	,  mod_date, mod_user, mod_prg, net_amt, fin_type, mod_host, mt_req_no, fin_code, performing_site
	)
	SELECT     
	 @ChrgRowguid	 , 0, account, 'NEW', service_date, getdate(), @origCDM, qty
	 , (select sum(mprice) from cpt4 where cdm = @origCDM)
	, (select sum(zprice) from cpt4 where cdm = @origCDM)
	, 'G0143 ReConversion ['+@origCdm+']'
	,  getdate()
	,RIGHT(SUSER_SNAME(),50)
	,  RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'SQL TRIGGER ' +CONVERT(VARCHAR(10),GETDATE(),112)),50)
	, (select sum(mprice) from cpt4 where cdm = @origCDM), fin_type
	, right(HOST_NAME(),50), mt_req_no, fin_code, performing_site
	FROM         chrg AS chrg_1
	--inner join cpt4 on cpt4.cdm = @origCDM
	WHERE     (chrg_1.chrg_num = @OrigChrgNum)

	-- now add the new amt record
	set @chrg_num = (select chrg_num from chrg where rowguid = @ChrgRowguid)
	PRINT @chrg_num
	INSERT INTO amt
	(chrg_num, cpt4, [type], amount, mod_date, mod_user, mod_prg, deleted,  modi, revcode
	)

	SELECT     
	@chrg_num, cpt4, [type], mprice, getdate(), right(suser_sname(),50)
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'SQL TRIGGER ' +CONVERT(VARCHAR(10),GETDATE(),112)),50)
	, deleted,  modi, rev_code
	FROM   cpt4	
	WHERE     (cpt4.cdm = @origCDM)

	end

END
