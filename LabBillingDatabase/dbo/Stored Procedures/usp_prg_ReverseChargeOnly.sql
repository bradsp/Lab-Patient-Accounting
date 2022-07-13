-- =============================================
-- Author:		David
-- Create date: 07/02/2014
-- Description:	Reverse Charge
-- =============================================
CREATE PROCEDURE usp_prg_ReverseChargeOnly 
	-- Add the parameters for the stored procedure here
	@chrgNum numeric(18,0) = 0
	, @comment VARCHAR(50)
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
		cdm ,(qty*-1) AS [qty] ,retail ,inp_price ,@comment 
		, GETDATE() AS [mod_date]
		, RIGHT(SUSER_SNAME(),50) AS [mod_user]
		, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
			'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
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
PRINT 'first insert into chrg'
PRINT @chrgNumNew


-- 3. insert the amount record that needs to be credited.
INSERT INTO amt
(chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg, deleted
,  modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, 
	bill_type, bill_method, pointer_set)
SELECT @chrgNumNew, cpt4, type, amount 
	, GETDATE() AS [mod_date]
	, RIGHT(SUSER_SNAME(),50) AS [mod_user]
	, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
	,  deleted,  modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, 
	bill_type, bill_method, 1 AS [pointer_set]	
	FROM amt WHERE chrg_num = @chrgNum

DECLARE	@Id NUMERIC(18,0)
SELECT @Id = SCOPE_IDENTITY()
PRINT 'first insert into amt'
PRINT @Id

-- 4. Update the old charge as credited
UPDATE dbo.chrg
SET credited = 1
WHERE chrg_num = @chrgNum



END
