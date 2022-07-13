

CREATE PROCEDURE getName
@sp VARCHAR(25)
,@ssn VARCHAR(9)
AS
SELECT account, trans_date, pat_name, ssn FROM dbo.acc
WHERE pat_name LIKE @sp AND ssn = @ssn
