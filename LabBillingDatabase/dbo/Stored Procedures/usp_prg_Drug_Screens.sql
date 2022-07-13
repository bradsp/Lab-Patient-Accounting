-- =============================================
-- Author:		David
-- Create date: 01/02/2014
-- Description:	Handle Medicare drug screens
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_Drug_Screens] 
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

	SELECT @OrigChrgNum, @ChrgRowguid

	declare @credited int
	set @credited = (select credited from chrg where chrg_num = @OrigChrgNum)
	if (@credited = 0)
	begin


	update chrg
	set credited = 1
	where chrg_num = @OrigChrgNum

	INSERT INTO chrg
	(rowguid, credited, account, [status], service_date, hist_date, cdm, qty, retail, inp_price, comment
	,  mod_date, mod_user, mod_prg
	, net_amt
	, fin_type, mod_host, mt_req_no, fin_code, performing_site
	, bill_method)

	SELECT     
	@ChrgRowguid, credited, account, [status], service_date, getdate(), cdm, qty*-1, retail, inp_price, comment
	,  getdate(), right(suser_sname(),50), right('usp_prg_Drug_Screen '+convert(varchar(10),getdate(),112),50)
	, net_amt
	, fin_type, right(mod_host,50), mt_req_no, fin_code, performing_site,  bill_method
	
	FROM         chrg AS chrg_1
	WHERE     (chrg_1.chrg_num = @OrigChrgNum)

		
--	select chrg_num from chrg
--	where rowguid = @rowguid

	set @chrg_num = (select chrg_num from chrg where rowguid = @ChrgRowguid)
	INSERT INTO amt
	(chrg_num, cpt4, [type], amount, mod_date, mod_user, mod_prg, deleted,  modi, revcode, modi2
	, diagnosis_code_ptr,  mt_req_no, order_code, bill_type, bill_method
	)

	SELECT     
	@chrg_num, cpt4, [type], amount, getdate(), right(suser_sname(),50)
	, right('usp_prg_Drug_Screen '+convert(varchar(10),getdate(),112),50), deleted,  modi, revcode, modi2
	, diagnosis_code_ptr,  mt_req_no, order_code, bill_type, bill_method
	
	FROM         amt AS amt_1
	WHERE     (amt_1.chrg_num = @OrigChrgNum)

	-- now add the medicare drug screen 
	set @ChrgRowguid = newid()

	INSERT INTO chrg
	(rowguid, credited, account, [status], service_date, hist_date, cdm, qty
	, retail, inp_price, comment
	,  mod_date, mod_user, mod_prg
	, net_amt
	, fin_type, mod_host, mt_req_no, fin_code, performing_site
	, bill_method)
	SELECT     
	 @ChrgRowguid, 0, account, 'NEW', service_date, getdate(), '5362566', 1
	 , (select sum(mprice) from cpt4 where cdm = '5362566')
	 , (select sum(zprice) from cpt4 where cdm = '5362566')
	 , 'G0143 Conversion'
	,  getdate(), right(suser_sname(),50), right('usp_prg_Drug_Screen '+convert(varchar(10),getdate(),112),50)
	, (select sum(mprice) from cpt4 where cdm = '5362566'), fin_type, right(chrg_1.mod_host,50), mt_req_no, fin_code, performing_site,  bill_method
	
	FROM         chrg AS chrg_1
	inner join cpt4 on cpt4.cdm = '5362566'
	WHERE     (chrg_1.chrg_num = @OrigChrgNum)

	-- now add the new amt record
	set @chrg_num = (select chrg_num from chrg where rowguid = @ChrgRowguid)
	
	INSERT INTO amt
	(chrg_num, cpt4, [type], amount, mod_date, mod_user, mod_prg, deleted,  modi, revcode
	--, diagnosis_code_ptr,  mt_req_no, order_code, bill_type, bill_method
	)

	SELECT     
	@chrg_num, cpt4, [type], mprice, getdate(), right(suser_sname(),50)
	, right('usp_prg_Drug_Screen '+convert(varchar(10),getdate(),112),50), deleted,  modi, rev_code
	---, diagnosis_code_ptr,  mt_req_no, order_code, bill_type, bill_method
	FROM   cpt4	
	WHERE     (cpt4.cdm = '5362566')

	end

END
