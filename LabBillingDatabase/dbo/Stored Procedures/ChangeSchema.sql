-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.ChangeSchema
	-- Add the parameters for the stored procedure here
	@curschema varchar(50),
	@tablename varchar(50),
	@newschema varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @command varchar(200);
	set @command = N'alter schema ' + @newschema + ' transfer ' + @curschema + '.' + @tablename + '; ' +
		'create synonym ' + @curschema + '.' + @tablename + ' for ' + @newschema + '.' + @tablename + '; ';

	print(@command);

	execute(@command)
END
