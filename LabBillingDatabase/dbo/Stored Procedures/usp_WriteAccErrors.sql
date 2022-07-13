-- =============================================
-- Author:		David
-- Create date: 06/08/2014
-- Description:	Write account errors into chrg_err table
-- =============================================
CREATE PROCEDURE usp_WriteAccErrors 
	-- Add the parameters for the stored procedure here
	  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
-- write client errors for clients not found in acc_dup_check
INSERT INTO chrg_err 
	(account, pat_name, cl_mnem, fin_code, trans_date, service_date, error
	,mod_date, mod_user, mod_prg, mod_host
	)
SELECT COALESCE(master_account, account) , pat_name ,
		LEFT(client,5), fin_code ,service_date ,service_date ,
		client,mod_date ,		mod_prg , mod_user ,		mod_host 
		FROM acc_dup_check
		WHERE LEN(LTRIM(RTRIM(client))) > 10
    
-- write different client errors here
; WITH cte AS 
(
SELECT DISTINCT
DENSE_RANK() OVER (PARTITION BY pat_name,CASE WHEN client not in ('COM','BCH','CGH') then service_date	ELSE NULL end
		ORDER BY account,fin_code
		,CASE WHEN client not in ('COM','BCH','CGH') then service_date	ELSE NULL end
) AS [rn],
master_account ,
		account ,
		CASE WHEN client not in ('COM','BCH','CGH') then service_date ELSE NULL END AS [dos],
		client,
		fin_code ,
		pat_name ,
		pat_ssn ,
		pat_dob  FROM dbo.acc_dup_check
)
INSERT INTO chrg_err 
	(account, pat_name, cl_mnem, fin_code, trans_date, service_date, error
	,mod_date, mod_user, mod_prg, mod_host
	)
SELECT cteA.sAcc,cteA.sPatName,cteA.sCli,cteA.sFinCode,cteA.sDos,cteA.sDos,
'XML ERROR: More charges on account '+cteA.fAcc+' for client '+cteA.fCli+' for fin_code '+ cteA.fFinCode
 , GETDATE() AS [mod_date]
, RIGHT(SUSER_SNAME(),50) AS [mod_user]
, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
	'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
, RIGHT (HOST_NAME(),50)  AS [mod_host]
FROM  
(
	SELECT TOP(100) PERCENT c2.account AS [sAcc],
		c2.dos  AS [sDos],c2.client AS [sCli],c2.fin_code AS [sFinCode],c2.pat_name AS [sPatName],
		c2.pat_ssn AS [sPatSSN],c2.pat_dob AS [sPatDob],
				c1.account AS [fAcc],c1.client AS [fCli],c1.fin_code AS [fFinCode] 								 
	FROM cte c1
	INNER JOIN cte c2  ON c2.rn+1 = c1.rn AND c1.pat_name = c2.pat_name
	AND c1.dos = c2.dos AND c1.pat_ssn = c2.pat_ssn AND c1.pat_dob = c2.pat_dob
	AND c1.fin_code = c2.fin_code
	WHERE c1.client <> c2.client
	ORDER BY COALESCE(c1.pat_name,c2.pat_name)
) cteA

-- write different fin_code errors
; WITH cte AS 
(
SELECT DISTINCT
DENSE_RANK() OVER (PARTITION BY pat_name,CASE WHEN client not in ('COM','BCH','CGH') then service_date	ELSE NULL end
		ORDER BY account,fin_code
		,CASE WHEN client not in ('COM','BCH','CGH') then service_date	ELSE NULL end
) AS [rn],
master_account ,
		account ,
		CASE WHEN client not in ('COM','BCH','CGH') then service_date ELSE NULL END AS [dos],
		client,
		fin_code ,
		pat_name ,
		pat_ssn ,
		pat_dob  FROM dbo.acc_dup_check
)
INSERT INTO chrg_err 
	(account, pat_name, cl_mnem, fin_code, trans_date, service_date, error
	,mod_date, mod_user, mod_prg, mod_host
	)
SELECT cteA.sAcc,cteA.sPatName,cteA.sCli,cteA.sFinCode,cteA.sDos,cteA.sDos,
'XML ERROR: More charges on account '+cteA.fAcc+' for client '+cteA.fCli+' with a different fin_code ' + cteA.fFinCode
 , GETDATE() AS [mod_date]
, RIGHT(SUSER_SNAME(),50) AS [mod_user]
, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
	'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
, RIGHT (HOST_NAME(),50)  AS [mod_host]
FROM  
(
	SELECT TOP(100) PERCENT c2.account AS [sAcc],
		c2.dos  AS [sDos],
		c2.client AS [sCli],
		c2.fin_code AS [sFinCode],
		c2.pat_name AS [sPatName],
		c2.pat_ssn AS [sPatSSN],
		c2.pat_dob AS [sPatDob],
				c1.account AS [fAcc],c1.client AS [fCli],c1.fin_code AS [fFinCode] 								 
	FROM cte c1
	INNER JOIN cte c2  ON c2.rn+1 = c1.rn AND c1.pat_name = c2.pat_name
	AND c1.dos = c2.dos AND c1.pat_ssn = c2.pat_ssn AND c1.pat_dob = c2.pat_dob
	AND c1.client = c2.client
	WHERE c1.fin_code <> c2.fin_code
	ORDER BY COALESCE(c1.pat_name,c2.pat_name)
) cteA

-- Write Duplicate account errors 
; WITH cte AS 
(
SELECT DISTINCT
DENSE_RANK() OVER (PARTITION BY pat_name,CASE WHEN client not in ('COM','BCH','CGH') then service_date	ELSE NULL end
		ORDER BY account,fin_code
		,CASE WHEN client not in ('COM','BCH','CGH') then service_date	ELSE NULL end
) AS [rn],
master_account ,
		account ,
		CASE WHEN client not in ('COM','BCH','CGH') then service_date ELSE NULL END AS [dos],
		client,
		fin_code ,
		pat_name ,
		pat_ssn ,
		pat_dob  FROM dbo.acc_dup_check
)
INSERT INTO chrg_err 
	(account, pat_name, cl_mnem, fin_code, trans_date, service_date, error
	,mod_date, mod_user, mod_prg, mod_host
	)
SELECT cteA.sAcc,cteA.sPatName,cteA.sCli,cteA.sFinCode,cteA.sDos,cteA.sDos,
'XML ERROR: Duplicate account '+cteA.fAcc+' for same client '+cteA.fCli+' with the same fin_code '+ cteA.fFinCode
 , GETDATE() AS [mod_date]
, RIGHT(SUSER_SNAME(),50) AS [mod_user]
, RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
	'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg]
, RIGHT (HOST_NAME(),50)  AS [mod_host]
FROM  
(
	SELECT TOP(100) PERCENT c2.account AS [sAcc],
		c2.dos  AS [sDos],
		c2.client AS [sCli],
		c2.fin_code AS [sFinCode],
		c2.pat_name AS [sPatName],
		c2.pat_ssn AS [sPatSSN],
		c2.pat_dob AS [sPatDob],
				c1.account AS [fAcc],c1.client AS [fCli],c1.fin_code AS [fFinCode] 								 
	FROM cte c1
	INNER JOIN cte c2  ON c2.rn+1 = c1.rn AND c1.pat_name = c2.pat_name
	AND c1.dos = c2.dos AND c1.pat_ssn = c2.pat_ssn AND c1.pat_dob = c2.pat_dob
	AND c1.client = c2.client AND c1.fin_code = c2.fin_code
	ORDER BY COALESCE(c1.pat_name,c2.pat_name)
) cteA

END

