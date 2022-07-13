-- =============================================
-- Author:		David Kelly
-- Create date: 07/13/2007
-- Description:	Remove UB billing dates by Batch number
-- =============================================
CREATE PROCEDURE [dbo].[msp_prg_clear_batch_ub] 
	-- Add the parameters for the stored procedure here
	@BatchNo int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
--	SELECT * from vw_prg_clear_batch_ub
--where batch = @BatchNo
UPDATE pat
set pat.ub_date = NULL, pat.ebill_batch_date = NULL
FROM pat INNER JOIN
	vw_prg_clear_batch_ub as vw ON vw.account = pat.account
where vw.batch = @BatchNo

END
