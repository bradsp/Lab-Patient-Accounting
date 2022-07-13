-- =============================================
-- Author:		David
-- Create date: 07/02/2014
-- Description:	Reverse Charge
-- =============================================
CREATE PROCEDURE usp_prg_ReCharge_JPG
	-- Add the parameters for the stored procedure here
	@chrgNum numeric(18,0) = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

DECLARE @chrgNumNew NUMERIC(18,0)


IF (NOT EXISTS(SELECT * FROM chrg WHERE chrg_num = @chrgNum AND credited = 0))
BEGIN
	RETURN;
END



DECLARE @acc VARCHAR(15)
SELECT @acc = (SELECT account FROM dbo.chrg WHERE chrg_num = @chrgNum)
DECLARE @accNew VARCHAR(15)
SELECT @accNew = REPLACE(REPLACE(REPLACE(@acc,'C','J'),'D','J'),'L','J')

IF (NOT EXISTS(SELECT account FROM acc WHERE account = @accNew))
BEGIN
	INSERT INTO dbo.acc (
	account ,	pat_name	,cl_mnem ,	fin_code ,	trans_date 
	,status ,	ssn ,		meditech_account ,original_fincode ,oereqno 
	,mri ,		ov_order_id ,	ov_pat_id ,
	mod_date ,				mod_user ,				mod_prg ,			mod_host ,
	bill_priority 
			)
	SELECT @accNew ,	pat_name	,'JPG' , 'Y'	 ,	trans_date 
	,'NEW' ,ssn ,	meditech_account ,original_fincode ,oereqno 
	,mri ,		ov_order_id ,	ov_pat_id 
	, GETDATE() AS [mod_date]
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT('usp_prg_ReCharge_JPG ' 
		+ CONVERT(VARCHAR(10),GETDATE(),112),50) AS [mod_prg]
	, RIGHT (HOST_NAME(),50)  AS [mod_host],
	bill_priority 
	FROM dbo.acc
	WHERE account = @acc
	
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
	          , RIGHT('SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112),50) 
	          , RIGHT (HOST_NAME(),50)
	        )
	
END 

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
			,RIGHT('usp_prg_ReCharge_JPG ' 
				+ CONVERT(VARCHAR(10),GETDATE(),112),50) AS [mod_prg]
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
			, RIGHT('usp_prg_ReCharge_JPG ' 
				+ CONVERT(VARCHAR(10),GETDATE(),112),50) AS [mod_prg]
			, RIGHT (HOST_NAME(),50)  AS [mod_host]
		FROM ins
		WHERE account = @acc--(SELECT account	FROM chrg WHERE chrg_num = @chrg_num)
	END
	
-- 1. INSERT the new credited record into the charge table
--		with the qty to reverse the orginal

INSERT INTO dbo.chrg
		(
			credited ,account ,status ,service_date ,hist_date ,
			cdm ,qty ,retail ,inp_price ,comment ,
			mod_date ,mod_user ,mod_prg ,mod_host ,
			net_amt ,fin_type ,mt_req_no ,fin_code ,
			lname ,fname ,mname ,name_suffix ,name_prefix ,
			pat_name ,order_site ,pat_ssn ,unitno ,location ,
			responsiblephy ,mt_mnem ,action ,facility ,referencereq ,
			pat_dob ,chrg_err 
		)
SELECT 1 ,account ,status ,service_date ,GETDATE(),
		cdm ,(qty*-1) AS [qty] ,retail ,inp_price ,comment 
		, GETDATE() AS [mod_date]
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT('usp_prg_ReCharge_JPG ' 
			+ CONVERT(VARCHAR(10),GETDATE(),112),50) AS [mod_prg]
		, RIGHT (HOST_NAME(),50)  AS [mod_host],
		net_amt ,fin_type ,mt_req_no ,fin_code ,
		lname ,fname ,mname ,name_suffix ,name_prefix ,
		pat_name ,order_site ,pat_ssn ,unitno ,location ,
		responsiblephy ,mt_mnem ,action ,facility ,referencereq ,
		pat_dob ,
		chrg_err  
FROM chrg WHERE chrg_num = @chrgNum
AND credited = 0

-- 2. get the new chrg_num for the amount record
SELECT @chrgNumNew = SCOPE_IDENTITY()
--PRINT 'first insert into chrg'
--PRINT @chrgNumNew


-- 3. insert the amount record that needs to be credited.
INSERT INTO amt
(chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg, deleted
,  modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, 
	bill_type, bill_method, pointer_set)
SELECT @chrgNumNew, cpt4, type, amount 
	, GETDATE() AS [mod_date]
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT('usp_prg_ReCharge_JPG ' 
		+ CONVERT(VARCHAR(10),GETDATE(),112),50) AS [mod_prg]
	,  deleted,  modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, 
	bill_type, bill_method, 1 AS [pointer_set]	
	FROM amt WHERE chrg_num = @chrgNum

DECLARE	@Id NUMERIC(18,0)
SELECT @Id = SCOPE_IDENTITY()
--PRINT 'first insert into amt'
--PRINT @Id

-- 4. Update the old charge as credited
UPDATE dbo.chrg
SET credited = 1
WHERE chrg_num = @chrgNum


-- 4. insert the new chrg and amt
INSERT INTO dbo.chrg
		(
			credited ,account ,status ,service_date ,hist_date ,
			cdm ,qty ,retail ,inp_price ,comment ,
			mod_date ,mod_user ,mod_prg ,mod_host ,
			net_amt ,fin_type ,mt_req_no ,fin_code ,
			lname ,fname ,mname ,name_suffix ,name_prefix ,
			pat_name ,order_site ,pat_ssn ,unitno ,location ,
			responsiblephy ,mt_mnem ,action ,facility ,referencereq ,
			pat_dob ,chrg_err 
		)
SELECT 0 ,@accNew ,chrg.status ,service_date ,GETDATE(),
		cdm ,qty ,retail ,inp_price 
		,RIGHT('REVERSED by usp_prg_ReCharge_JPG ' 
			+ CONVERT(VARCHAR(10),GETDATE(),112),50)
		, GETDATE() AS [mod_date]
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT('usp_prg_ReCharge_JPG ' 
			+ CONVERT(VARCHAR(10),GETDATE(),112),50) AS [mod_prg]
		, RIGHT (HOST_NAME(),50)  AS [mod_host]
		, colClientPrice  AS [net_amt]
--		CASE WHEN fin_type = 'M' THEN colMPrice
--			ELSE colCPrice END AS [net_amt] 
		,'C' ,mt_req_no ,acc.fin_code ,
		lname ,fname ,mname ,name_suffix ,name_prefix ,
		chrg.pat_name ,order_site ,pat_ssn ,unitno ,location ,
		responsiblephy ,mt_mnem ,action ,facility ,referencereq ,
		pat_dob ,
		chrg_err 
FROM chrg
INNER JOIN acc ON dbo.acc.account = dbo.chrg.account
CROSS APPLY dbo.GetFeeSchedulePrice('JPG', chrg.cdm) 
WHERE chrg_num = @chrgNum
--
SELECT @chrgNumNew = SCOPE_IDENTITY()
--PRINT 'second insert with new price'
--PRINT @Id


; WITH cteAmt
AS
(
SELECT *
from
( SELECT cl_mnem, cdm, net_amt FROM acc 
INNER JOIN chrg ON dbo.chrg.account = dbo.acc.account
WHERE chrg_num = @chrgNumNew) AS [client]
CROSS APPLY dbo.GetAmtRecords(cl_mnem,cdm,NULL,net_amt)
)
INSERT INTO amt
(chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg, deleted
,  modi, revcode, order_code )
SELECT  @chrgNumNew AS [newChrgNum], colCpt ,colType ,colAmount ,colModDate ,
	colModUser ,colModPrg ,colDeleted ,colModi ,colRevcode ,colOrderCode  
	FROM cteAmt

END
