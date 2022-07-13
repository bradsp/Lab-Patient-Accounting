-- =============================================
-- Author:		Bradley Powers
-- Create date: 3/9/2016
-- Description:	Inserts clients into the CLOE client table based on new clients added to the billing client table.
-- =============================================
CREATE PROCEDURE usp_cloe_client_sync 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT mcloe.GOMCLLIVE.dbo.wcli
        ( cli_mnem ,
          name ,
          address1 ,
          address2 ,
          city ,
          state ,
          zip ,
          phone ,
          attention ,
          r1 ,
          r2
        )
	SELECT cli_mnem,
			cli_nme,
			addr_1,
			addr_2,
			city,
			st,
			zip,
			LEFT(phone,13),
			LEFT(contact,50),
			type,
			'1'		
	FROM client 
	WHERE cli_mnem NOT IN (SELECT cli_mnem FROM MCLOE.GOMCLLIVE.dbo.wcli)
		AND deleted = 0
END
