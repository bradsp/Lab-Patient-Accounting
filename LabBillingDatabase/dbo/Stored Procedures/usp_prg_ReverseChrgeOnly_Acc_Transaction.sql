-- =============================================
-- Author:		David
-- Create date: 07/02/2014
-- Description:	Reverse Charge
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_ReverseChrgeOnly_Acc_Transaction]
	-- Add the parameters for the stored procedure here
	@acc VARCHAR(15) = 0,
	@comment varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON; -- this allows the transaction code to work.

    -- Insert statements for procedure here
    IF (NOT EXISTS(SELECT * FROM chrg 
		WHERE chrg.account = @acc AND credited = 0))
	BEGIN
		RETURN;
	END
	
	-- store the accounts non credited charges in a temp table
	CREATE TABLE #tempChrg (chrg_num NUMERIC)
	INSERT INTO #tempChrg 	( chrg_num )
	SELECT chrg_num FROM dbo.chrg
	WHERE dbo.chrg.account = @acc 
	AND chrg.credited = 0


	DECLARE @chrg_num NUMERIC
	SELECT @chrg_num = (SELECT TOP(1) chrg_num FROM #tempChrg)
	WHILE (@chrg_num IS NOT NULL)
	BEGIN
		EXEC dbo.usp_prg_ReverseChargeOnly @chrgNum = @chrg_num -- numeric
			, @comment = @comment
		DELETE #tempChrg WHERE #tempChrg.chrg_num = @chrg_num	
		SELECT @chrg_num = (SELECT TOP(1) chrg_num FROM #tempChrg)


	END 

	IF (EXISTS (SELECT account FROM acc WHERE dbo.acc.account = @acc
		AND status = 'PAID_OUT'))
		BEGIN
			UPDATE dbo.acc
			SET status = ISNULL(NULLIF(status,'PAID_OUT'),'NEW')
			WHERE account = @acc
		END		
	DROP TABLE #tempChrg

END

