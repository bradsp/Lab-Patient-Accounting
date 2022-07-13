-- =============================================
-- Author:		David
-- Create date: 05/23/2014
-- Description:	Process Charge changes
-- =============================================
CREATE PROCEDURE usp_prgProcessChargeADT 
	-- Add the parameters for the stored procedure here
	@acc  VARCHAR(15)
	--,@client VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	CREATE TABLE #tempChrg (chrg_num NUMERIC)

	INSERT INTO #tempChrg
			( chrg_num )

	SELECT chrg_num FROM dbo.chrg
	WHERE dbo.chrg.account = @acc 
	AND chrg.credited = 0

	SELECT * FROM #tempChrg

	DECLARE @chrg_num INT
	SELECT @chrg_num = (SELECT TOP(1) chrg_num FROM #tempChrg)

	WHILE (@chrg_num IS NOT NULL)
	BEGIN
		PRINT @chrg_num
		-- check fin code to decide if client or insurance
		EXEC dbo.usp_prg_ReCharge_transaction @chrgNum = @chrg_num -- numeric
		DELETE #tempChrg WHERE #tempChrg.chrg_num = @chrg_num	
		SELECT @chrg_num = (SELECT TOP(1) chrg_num FROM #tempChrg)

	END 

	DROP TABLE #tempChrg
END
