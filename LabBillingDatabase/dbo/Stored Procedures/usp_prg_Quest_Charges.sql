
-- Author:		David
-- Create date: 04/23/2014
-- Description:	Convert Charges for Quest
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_Quest_Charges] 
	-- Add the parameters for the stored procedure here
	@chrg_num numeric(38,0) = 0
	--, @p2 int = 0
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @acc VARCHAR(15)
	DECLARE @accNew varchar(15)
    -- Insert statements for procedure here
	SELECT @chrg_num
	IF ((SELECT credited FROM chrg WHERE chrg_num = @chrg_num) = 1)
	RETURN;
	
	IF (NOT EXISTS 
		(SELECT colPrice FROM dbo.GetQuestPrice(@chrg_num) 
			WHERE colPrice IS NOT null))
	RETURN;
		
	select @acc = (SELECT account FROM dbo.chrg WHERE chrg_num = @chrg_num)	
	select @accNew = REPLACE(REPLACE(REPLACE(@acc,'C','Q'),'D','Q'),'L','Q')

	IF (NOT EXISTS (SELECT account FROM acc 
		WHERE account = (SELECT @accNew
		FROM chrg WHERE chrg_num = @chrg_num)))
	BEGIN
	
	INSERT INTO dbo.acc
        ( account, pat_name, cl_mnem , fin_code,
          trans_date, cbill_date, status,ssn,
          meditech_account ,original_fincode ,oereqno ,mri ,ov_order_id ,
          ov_pat_id ,mod_date ,mod_user ,mod_prg ,mod_host ,bill_priority ,guarantorID
        )
		SELECT  @accNew AS [account] , pat_name ,  'QUESTR' ,  'Y' ,
				trans_date, cbill_date, 'NEW' as [status], ssn,   
				meditech_account ,original_fincode ,oereqno ,mri ,ov_order_id ,ov_pat_id 
				, GETDATE() AS [mod_date], RIGHT(SUSER_SNAME(),50) AS [mod_user]
				, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY'),50) AS [mod_prg]
				, RIGHT (HOST_NAME(),50)  AS [mod_host],bill_priority ,guarantorID 
		FROM acc	
		WHERE account =  @acc--(SELECT account FROM chrg WHERE chrg_num = @chrg_num)
		
	INSERT INTO dbo.acc_merges
	        ( account ,
	          dup_acc ,
	          mod_date ,
	          mod_prg ,
	          mod_user ,
	          mod_host
	        )
	VALUES  ( @acc , -- account - varchar(15)
	          @accNew, -- dup_acc - varchar(15)
	          GETDATE() 
			  , RIGHT(SUSER_SNAME(),50)
	          , RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY'),50)
	          , RIGHT (HOST_NAME(),50)
	        )
	END	
	ELSE
	BEGIN
		UPDATE acc
		SET status = 'NEW'
		WHERE account = @accNew
	END
		
	--SELECT @@ROWCOUNT
	IF (NOT EXISTS (SELECT account FROM pat
		WHERE account = @accNew))

		BEGIN
		INSERT INTO dbo.pat
        ( account ,pat_full_name ,ssn ,
          pat_addr1 ,pat_addr2 ,city_st_zip ,pat_city ,pat_state ,pat_zip ,
          dob_yyyy ,sex ,relation ,
          guarantor ,guar_addr ,g_city_st ,guar_city ,guar_state ,guar_zip ,
          pat_marital ,pat_race ,pat_phone ,
          icd9_1 ,icd9_2 ,icd9_3 ,icd9_4 ,icd9_5 ,icd9_6 ,icd9_7 ,icd9_8 ,icd9_9 ,
          phy_id ,  phy_comment ,
          guar_phone ,
          mod_date ,
          mod_user ,
          mod_prg ,
          mod_host ,
          hne_epi_number
        )
		SELECT  @accNew AS [account] ,pat_full_name, pat.ssn ,
			pat_addr1 ,pat_addr2 ,city_st_zip, pat_city,pat_state, pat_zip
			,dob_yyyy ,sex ,relation ,
			guarantor ,guar_addr ,g_city_st , guar_city, guar_state,guar_zip
			, pat_marital , pat_race, pat_phone
			,icd9_1 ,icd9_2 ,icd9_3 ,icd9_4 ,icd9_5 ,icd9_6 ,icd9_7 ,icd9_8 ,icd9_9 
			,phy_id , phy_comment, guar_phone
			, GETDATE() AS [mod_date]
			, RIGHT(SUSER_SNAME(),50) AS [mod_user]
			, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY'),50) AS [mod_prg]
			, RIGHT (HOST_NAME(),50)  AS [mod_host] , hne_epi_number 
		FROM pat
		WHERE account = @acc--(SELECT account	FROM chrg WHERE chrg_num = @chrg_num)
		END
	
	--SELECT @@ROWCOUNT
	IF (NOT EXISTS (SELECT account FROM ins
		WHERE account = @accNew))

		BEGIN
		INSERT INTO dbo.ins
        ( deleted, account, ins_a_b_c ,
          holder_nme ,holder_dob ,holder_sex ,holder_addr ,holder_city_st_zip ,
          plan_nme ,plan_addr1 ,plan_addr2 ,p_city_st ,
          policy_num ,cert_ssn ,
          grp_nme ,grp_num ,
          employer ,e_city_st ,
          fin_code ,ins_code ,
          relation ,
          mod_date ,
          mod_user ,
          mod_prg ,
          mod_host
        )
		SELECT  deleted ,@accNew AS [account] ,ins_a_b_c ,
			holder_nme ,holder_dob ,holder_sex ,holder_addr ,holder_city_st_zip ,
			plan_nme ,plan_addr1 ,plan_addr2 ,p_city_st ,
			policy_num ,cert_ssn ,
			grp_nme ,grp_num ,
			employer ,e_city_st ,
			'Y' ,'CLIENT' ,
			relation 
			, GETDATE() AS [mod_date]
			, RIGHT(SUSER_SNAME(),50) AS [mod_user]
			, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY'),50) AS [mod_prg]
			, RIGHT (HOST_NAME(),50)  AS [mod_host]
		FROM ins
		WHERE account = @acc--(SELECT account	FROM chrg WHERE chrg_num = @chrg_num)
	END
		
	--SELECT @@ROWCOUNT
	
	--1. Insert the new credited charge to get the chrg_num for the amt record
	--		before setting this records credited flag to credited.
INSERT INTO chrg
(credited, account, status, service_date, hist_date
, cdm, qty, retail, inp_price, comment, invoice
, mod_date, mod_user, mod_prg, mod_host
,net_amt, fin_type,  mt_req_no, post_date, fin_code, performing_site
, bill_method
)
SELECT  1 AS [credited],  account, status, service_date, CONVERT(VARCHAR(10),GETDATE(),101)
, cdm, qty*-1 AS [qty], retail, inp_price
, 'MOVED TO ['+@accNew+']' as [comment]
, NULL AS [invoice]
, GETDATE() AS [mod_date], RIGHT(SUSER_SNAME(),50) AS [mod_user]
,RIGHT(OBJECT_NAME(@@PROCID),50) AS [mod_prg]
, RIGHT (HOST_NAME(),50) AS [mod_host]
,net_amt, fin_type, mt_req_no, NULL AS post_date, 'Y', performing_site, bill_method
FROM chrg
WHERE chrg_num = @chrg_num
	

-- 2. get the new chrg_num for the amount record
DECLARE @Id NUMERIC(38,0)
SELECT @Id = SCOPE_IDENTITY()
--SELECT @Id

INSERT INTO amt
(
chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg, deleted
,  modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, 
	bill_type, bill_method, pointer_set
	)
SELECT @Id AS [chrg_num], cpt4, type, amount
, GETDATE() AS [mod_date], RIGHT(SUSER_SNAME(),50) AS [mod_user]
, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY'),50) AS [mod_prg]
, deleted,  modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, 
	bill_type, bill_method, pointer_set	
	FROM amt WHERE chrg_num = @chrg_num

SELECT @Id = SCOPE_IDENTITY()

UPDATE dbo.chrg
SET credited = 1
, comment =  'MOVED TO ['+@accNew+']' 
WHERE chrg_num = @chrg_num

SELECT @Id = SCOPE_IDENTITY()
--PRINT ''first update of chrg''
--PRINT @Id


INSERT INTO chrg
(credited, account, status, service_date, hist_date
, cdm, qty, retail, inp_price, comment, invoice
, mod_date, mod_user, mod_prg, mod_host
,net_amt, fin_type,  mt_req_no, post_date, fin_code, performing_site
, bill_method
)
SELECT  0 AS credited, @accNew AS [account], status, service_date
, CONVERT(VARCHAR(10),GETDATE(),101) AS [hist_date]
, cdm, qty, retail, inp_price
, 'MOVED FROM ['+account+']' AS [comment]
, NULL AS [invoice]
, GETDATE() AS [mod_date]
, RIGHT(SUSER_SNAME(),50) AS [mod_user]
, RIGHT(OBJECT_NAME(@@PROCID),50) AS [mod_prg]
, RIGHT (HOST_NAME(),50)  AS [mod_host]
,(SELECT colPrice FROM dbo.GetQuestPrice(@chrg_num)) AS [net_amt]
, 'C' AS [fin_type]
, mt_req_no, NULL AS [post_date], 'Y' as [fin_code], performing_site, bill_method
FROM chrg
WHERE chrg_num = @chrg_num

SELECT @Id = SCOPE_IDENTITY()
--PRINT ''second insert with new price''
--PRINT @Id

INSERT INTO amt
(chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg, deleted
,  modi, revcode, order_code )
SELECT  @Id, 
        colCpt ,
        colType ,
        colAmount ,
        colModDate ,
        colModUser ,
        colModPrg ,
        colDeleted ,
        colModi ,
        colRevcode ,
        colOrderCode FROM dbo.GetQuestAmtRecords(@chrg_num)

	
END
