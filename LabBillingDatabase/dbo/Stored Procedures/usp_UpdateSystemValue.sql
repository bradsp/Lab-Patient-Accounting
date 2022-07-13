-- =============================================
-- Author:		David
-- Create date: 05/26/2014
-- Description:	Update the System table with new authorized users
-- =============================================
CREATE PROCEDURE usp_UpdateSystemValue 
	-- Add the parameters for the stored procedure here
	@value VARCHAR(8000), 
	@key VARCHAR(8000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT @value, @key

UPDATE dbo.system
SET value = @value
,mod_date = GETDATE()
,mod_user = RIGHT(SUSER_SNAME(),50)
,mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL PROC '+CONVERT(VARCHAR(10),GETDATE(),112)),50)
,mod_host = RIGHT (HOST_NAME(),50)
WHERE comment = @key 

	

--SELECT 'Rows updated = '+CAST(@@ROWCOUNT AS VARCHAR(2))
END
