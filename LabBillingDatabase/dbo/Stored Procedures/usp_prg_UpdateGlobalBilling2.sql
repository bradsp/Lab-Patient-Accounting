-- =============================================
-- Author:		David
-- Create date: 05/26/2014
-- Description:	update billing
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_UpdateGlobalBilling2] 
	-- Add the parameters for the stored procedure here
	--@chrg_num numeric(38,0),
	@endDate DATETIME
AS
BEGIN
	--RETURN;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- SET XACT_ABORT ON will cause the transaction to be uncommittable
	-- when the constraint violation occurs. 
	SET XACT_ABORT ON;
	
    -- Insert statements for procedure here
--1. set the end date according to the end of the month processing
    DECLARE @startDate DATETIME
	SET @startDate = CAST('01/20/2012 00:00' AS DATETIME) -- date program started
	
	DECLARE @DIM INT 
	SET @DIM = DATEPART(DAY,DATEADD(mm,DATEDIFF(m,0,GETDATE())+1, -.000003))-- Last day This Month
	DECLARE @TDAY INT
	SET @TDAY = DATEPART(DAY,GETDATE())
	SELECT @endDate = coalesce(@endDate,(select 
	CASE WHEN @DIM - @TDAY > 5
		THEN DATEADD(dd,-4,DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),getdate(),101))))
		ELSE DATEADD(dd,@TDAY-@DIM,DATEADD(ms,-3,CONVERT(DATETIME,convert(varchar(10),getdate(),101))))
		END AS [endDate]))
	DECLARE @chrg_num NUMERIC(38,0)
	
	
	BEGIN TRY
		BEGIN TRANSACTION

	TRUNCATE TABLE dbo.Temp_GlobalBilling

	INSERT INTO dbo.Temp_GlobalBilling
			(
				colClient ,colAcc ,colOrigChrgNum ,
				colCDM ,colCPT ,colQty ,colChrgAmt ,
				colDOS ,colDateEntered ,colPrice ,colError, colSite
			)
	SELECT colClient ,
			colAcc ,colChrgNum ,g.colCDM ,colCPT ,colQty ,colChrgAmt 
			,colDOS ,colDateEntered 
			, colPrice
			,colError , colSite
			FROM dbo.GetGlobalBillingCharges(@endDate) g
			CROSS APPLY dbo.GetPrice(g.colClient,g.colFinCode,g.colCDM,null) p
	
-------------------------------------------------------------
--1. Create Account, pat , acc_merge records for accounts if they do not exist
IF (NOT EXISTS(
	SELECT account FROM acc 
	LEFT outer JOIN dbo.temp_GlobalBilling t 
	ON REPLACE(REPLACE(REPLACE(colAcc,'c','j'),'d','j'),'l','j') = acc.account 
	WHERE acc.account IS null
	))
	BEGIN
		INSERT INTO dbo.acc
        ( account, pat_name, cl_mnem , fin_code,
          trans_date, cbill_date, status,ssn,
          meditech_account ,original_fincode ,oereqno ,mri ,ov_order_id ,
          ov_pat_id ,mod_date ,mod_user ,mod_prg ,mod_host ,bill_priority ,guarantorID
        )
		SELECT  REPLACE(REPLACE(REPLACE(acc.account,'C','J'),'D','J'),'L','J') 
		, pat_name ,  'JPG' ,  'Y' ,
				trans_date, cbill_date, 'NEW' as [status], ssn,   
				meditech_account ,original_fincode ,oereqno ,mri ,ov_order_id ,ov_pat_id 
				, GETDATE() AS [mod_date], RIGHT(SUSER_SNAME(),50) AS [mod_user]
				, RIGHT (
						ISNULL(
							OBJECT_NAME(@@PROCID)
							,'SQL PROC ' + convert(VARCHAR(10),GETDATE() ,112))
						,50
						) AS [mod_prg]
				, RIGHT (HOST_NAME(),50)  AS [mod_host],bill_priority ,guarantorID 
		FROM acc	
		WHERE account IN (SELECT DISTINCT colAcc FROM dbo.Temp_GlobalBilling t
						LEFT OUTER JOIN acc ON acc.account = 
							REPLACE(REPLACE(REPLACE(t.colAcc,'c','j'),'d','j'),'l','j')
						WHERE acc.account IS NULL)
--SELECT 'Acc: ' + CONVERT(VARCHAR(2),@@ROWCOUNT )
		INSERT INTO dbo.acc_merges
				( account ,dup_acc , mod_date ,mod_prg ,mod_user ,mod_host)
		SELECT acc.account
		,REPLACE(REPLACE(REPLACE(acc.account,'C','J'),'D','J'),'L','J') ,				
				  GETDATE() 
				  , RIGHT(SUSER_SNAME(),50)
				  , RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL PROC ' + CONVERT(VARCHAR(10),GETDATE(),112)),50)
				  , RIGHT (HOST_NAME(),50)		
		FROM acc	
		WHERE account IN (SELECT DISTINCT colAcc FROM dbo.Temp_GlobalBilling t
							LEFT OUTER JOIN acc ON acc.account = REPLACE(REPLACE(t.colAcc,'c','j'),'d','j')
							WHERE acc.account IS NULL)				
--SELECT 'Acc Merge: ' + CONVERT(VARCHAR(2),@@ROWCOUNT )		
	END



IF (NOT EXISTS(
SELECT account FROM dbo.pat
LEFT outer JOIN dbo.Temp_GlobalBilling t 
	ON REPLACE(REPLACE(REPLACE(colAcc,'c','j'),'d','j'),'l','j') = pat.account
	where pat.account is null))
	BEGIN
		INSERT INTO dbo.pat
        ( account ,pat_full_name ,ssn ,
          pat_addr1 ,pat_addr2 ,city_st_zip ,pat_city ,pat_state ,pat_zip ,
          dob_yyyy ,sex ,relation ,
          guarantor ,guar_addr ,g_city_st ,guar_city ,guar_state ,guar_zip ,
          pat_marital ,pat_race ,pat_phone ,
          icd9_1 ,icd9_2 ,icd9_3 ,icd9_4 ,icd9_5 ,icd9_6 ,icd9_7 ,icd9_8 ,icd9_9 ,
          phy_id ,  phy_comment ,
          guar_phone ,mod_date ,mod_user ,mod_prg ,mod_host ,hne_epi_number
        )
		SELECT  REPLACE(REPLACE(REPLACE(pat.account,'C','J'),'D','J'),'L','J') 
		,pat_full_name, pat.ssn ,
			pat_addr1 ,pat_addr2 ,city_st_zip, pat_city,pat_state, pat_zip
			,dob_yyyy ,sex ,relation ,
			guarantor ,guar_addr ,g_city_st , guar_city, guar_state,guar_zip
			, pat_marital , pat_race, pat_phone
			,icd9_1 ,icd9_2 ,icd9_3 ,icd9_4 ,icd9_5 ,icd9_6 ,icd9_7 ,icd9_8 ,icd9_9 
			,phy_id , phy_comment, guar_phone
			, GETDATE() AS [mod_date]
			, RIGHT(SUSER_SNAME(),50) AS [mod_user]
			, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL PROC ' + CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
			, RIGHT (HOST_NAME(),50)  AS [mod_host] , hne_epi_number 
		FROM pat
		WHERE account IN (SELECT DISTINCT colAcc FROM dbo.Temp_GlobalBilling t
		LEFT OUTER JOIN pat ON pat.account = 
		REPLACE(REPLACE(REPLACE(t.colAcc,'c','j'),'d','j'),'l','j')
		WHERE pat.account IS NULL )
		
		End

IF (NOT EXISTS(
SELECT account FROM dbo.ins
LEFT outer JOIN dbo.Temp_GlobalBilling t ON 
REPLACE(REPLACE(REPLACE(colAcc,'c','j'),'d','j'),'l','j') = ins.account
where dbo.ins.account is null))
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
		SELECT  deleted ,
		REPLACE(REPLACE(REPLACE(ins.account,'C','J'),'D','J'),'L','J') 
		,ins_a_b_c ,
			holder_nme ,holder_dob ,holder_sex ,holder_addr ,holder_city_st_zip ,
			plan_nme ,plan_addr1 ,plan_addr2 ,p_city_st ,
			policy_num ,cert_ssn ,
			grp_nme ,grp_num ,
			employer ,e_city_st ,
			fin_code, ins_code,--'Y' ,'CLIENT' , -- wdk 20140604 leave as original record
			relation 
			, GETDATE() AS [mod_date]
			, RIGHT(SUSER_SNAME(),50) AS [mod_user]
			, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY'),50) AS [mod_prg]
			, RIGHT (HOST_NAME(),50)  AS [mod_host]
		FROM ins
		WHERE account IN (SELECT DISTINCT colAcc FROM dbo.Temp_GlobalBilling t
		LEFT OUTER JOIN dbo.ins ON dbo.ins.account = 
		replace(REPLACE(REPLACE(t.colAcc,'c','j'),'d','j'),'l','j')
		WHERE ins.account IS NULL)
	END
	

	--2. Insert the new credited charge to get the chrg_num for the amt record
	--		before setting this records credited flag to credited.
	INSERT INTO dbo.chrg
	(credited, account, service_date, hist_date
	, cdm, qty, retail, inp_price, comment--, invoice
	, mod_date, mod_user, mod_prg, mod_host
	,net_amt, fin_type,  mt_req_no, post_date, fin_code, performing_site
	, bill_method, rowguid
	)
	SELECT   --'insert chrg rev'AS [table],chrg.chrg_num, u.colChrgNum, 
	1 AS [credited],  account, service_date
	, CONVERT(VARCHAR(10),GETDATE(),101) AS [hist_date]
	, cdm, qty*-1 AS [qty], retail, inp_price
	, 'MOVED TO ['+
	REPLACE(REPLACE(REPLACE(account,'C','J'),'D','J'),'L','J')+']' as [comment]
	--, NULL AS [invoice]
	, GETDATE() AS [mod_date], RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, COALESCE(RIGHT(OBJECT_NAME(@@PROCID),50),'SQL PROC ' + CONVERT(VARCHAR(10),GETDATE(),112)) AS [mod_prg]
	, RIGHT (HOST_NAME(),50) AS [mod_host]
	,net_amt, fin_type, mt_req_no, NULL AS post_date, 'Y', performing_site, bill_method
	, u.rowguid
	FROM chrg
	CROSS APPLY dbo.Temp_GlobalBilling u
	WHERE chrg.chrg_num = u.colOrigChrgNum
		

	-- 3. get the new chrg_num for the amount record and store it for the amt record
	UPDATE dbo.Temp_GlobalBilling
	SET colNewChrgNum = c.chrg_num
	FROM dbo.Temp_GlobalBilling t
	CROSS APPLY dbo.chrg c 
	where c.rowguid = t.rowguid
		
	
	INSERT INTO dbo.amt
	(
	chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg, deleted
	,  modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, 
		bill_type, bill_method, pointer_set
	)
	SELECT dbo.Temp_GlobalBilling.colNewChrgNum AS [chrg_num]
	, cpt4, type, amount
	, GETDATE() AS [mod_date], RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, COALESCE(RIGHT(OBJECT_NAME(@@PROCID),50),'SQL PROC ' + CONVERT(VARCHAR(10),GETDATE(),112)) AS [mod_prg]
	, deleted,  modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, 
		bill_type, bill_method, pointer_set	
	FROM amt 
	CROSS APPLY dbo.Temp_GlobalBilling 
	WHERE amt.chrg_num = dbo.Temp_GlobalBilling.colOrigChrgNum
	
	
	-- 3. Insert the new charge record for the new client
	INSERT INTO dbo.chrg
	(
		rowguid,account, service_date, hist_date, cdm, qty, retail, comment
		, mod_date, mod_user, mod_prg, mod_host
		, net_amt, fin_type, fin_code, bill_method, performing_site
	)
	SELECT gb.rowguid
	, REPLACE(REPLACE(REPLACE(colAcc,'C','J'),'D','J'),'L','J')
	, colDOS, CONVERT(VARCHAR(10),GETDATE(),101) AS [hist_date]
	, colCDM, colQty, colQty*colChrgAmt, 'MOVED FROM ['+colAcc+']' AS [comment]
	, GETDATE() AS [mod_date], RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(OBJECT_NAME(@@PROCID),50) AS [mod_prg], RIGHT (HOST_NAME(),50)  AS [mod_host]
	, u.colPrice
	, 'C' AS [fin_type]
	, 'Y' as [fin_code]
	, 'JPG' --colClient AS [bill_method]
	, gb.colSite
	FROM dbo.temp_GlobalBilling gb
	CROSS APPLY dbo.GetPrice('JPG','CLIENT',gb.colCDM,null) u
	
	UPDATE dbo.Temp_GlobalBilling
	SET colNewChrgNum = c.chrg_num
	, colPrice = c.net_amt
	FROM dbo.Temp_GlobalBilling t
	CROSS APPLY dbo.chrg c 
	where c.rowguid = t.rowguid AND c.account = 
	REPLACE(REPLACE(REPLACE(t.colAcc,'C','J'),'D','J'),'L','J')	

DECLARE @fintype VARCHAR(1)
SET @fintype = 'C'
INSERT INTO dbo.amt
(
	chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg
	,  modi, revcode, order_code 
)
SELECT t.colNewChrgNum, t.colCPT,s.colType,s.colCptPrice
, GETDATE() AS [mod_date], RIGHT(SUSER_SNAME(),50) AS [mod_user]
, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY'),50) AS [mod_prg]
,s.colModi, s.colRevCode, t.colCdm
FROM dbo.temp_GlobalBilling t
CROSS APPLY dbo.SplitCdmPriceByCpt(t.colCdm,t.colPrice,@fintype) s

-- if the "J" account is paid out update the status to 'NEW'
	UPDATE dbo.acc
	SET status = 'NEW'
--	SELECT * FROM dbo.acc
	WHERE account 
		IN (SELECT REPLACE(REPLACE(REPLACE(colAcc,'c','J'),'d','J'),'l','J') FROM dbo.Temp_GlobalBilling)
	AND status = 'PAID_OUT'

-- do this last because if nothing else works we should roll back the changes and 
-- want to have the charge available to retry.
	UPDATE dbo.chrg
	SET bill_method = 'JPG'
		,credited =  1
		--, comment = 'MOVED TO ['+REPLACE(REPLACE(account,'C','J'),'D','J')+']'
	FROM dbo.Temp_GlobalBilling
	WHERE chrg_num IN (SELECT colOrigChrgNum FROM dbo.Temp_GlobalBilling)


-------------------------------------------------------------	
		COMMIT TRANSACTION;
	END TRY
	
	BEGIN CATCH
    
	IF (XACT_STATE()) = -1
    BEGIN
        PRINT
            N'The transaction is in an uncommittable state.' +
            'Rolling back transaction.'
        ROLLBACK TRANSACTION;
        
        	INSERT INTO dbo.error_prg
            ( error_type,
            [app_name] ,
              app_module ,
              error ,
              mod_date ,
              mod_prg ,
              mod_user ,
              mod_host
            )
SELECT	'SQL',
            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 )AS [mod_prg],
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_MESSAGE() AS ErrorMessage,
            GETDATE(),
            RIGHT(COALESCE(OBJECT_NAME(@@PROCID),ERROR_PROCEDURE(),app_name()),50 ) AS ErrorProcedure,
            RIGHT(SUSER_SNAME(),50),
            RIGHT (HOST_NAME(),50);
        
        
    END;

    -- Test whether the transaction is committable.
    IF (XACT_STATE()) = 1
    BEGIN
--        PRINT
--            N'The transaction is committable.' +
--            'Committing transaction.'
        COMMIT TRANSACTION;   
    END;

        
END CATCH;

	


END
