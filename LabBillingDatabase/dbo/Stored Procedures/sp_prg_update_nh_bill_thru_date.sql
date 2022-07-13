
CREATE PROCEDURE [dbo].[sp_prg_update_nh_bill_thru_date]	
@date DATETIME
AS
--SELECT  key_name ,
--        value ,
--        programs ,
--        mod_date ,
--        mod_user ,
--        mod_prg ,
--        mod_host ,
--        comment FROM dbo.system
--WHERE key_name = 'nh_bill_thru'

set @date = dateadd(ms,-3,dateadd(mm,datediff(m,0,@date )+1,0))

UPDATE dbo.system
SET value = @date
,mod_date = GETDATE()
,mod_user = RIGHT(SUSER_SNAME(),50)
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY '+CONVERT(VARCHAR(10),GETDATE(),112)),50)
,mod_host = RIGHT (HOST_NAME(),50)
WHERE key_name = 'nh_bill_thru' 
	AND DATEDIFF(MONTH, CAST(value AS DATETIME),@date) <=1

SELECT 'Rows updated = '+CAST(@@ROWCOUNT AS VARCHAR(2))

