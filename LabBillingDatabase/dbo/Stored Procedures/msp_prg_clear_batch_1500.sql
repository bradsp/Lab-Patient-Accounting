CREATE PROCEDURE [dbo].[msp_prg_clear_batch_1500] 
	-- Add the parameters for the stored procedure here
	@BatchNo int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
--	SELECT * from vw_prg_clear_batch_1500
--	where batch = @BatchNo
UPDATE pat
set pat.h1500_date = NULL, pat.ebill_batch_1500 = NULL
FROM pat INNER JOIN
	vw_prg_clear_batch_1500 as vw ON vw.account = pat.account
where vw.batch = @BatchNo

-- 08/30/2007 wdk/rgc added to complete process of clearing the batch number
DELETE FROM h1500
WHERE  batch = @BatchNo
END
