-- =============================================
-- Author:		Bradley Powers
-- Create date: 6/9/2017
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[MenuItemsByUser] 
	-- Add the parameters for the stored procedure here
	@username varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @menu VARCHAR(50);

	SELECT @menu = emp.mainmenu
	FROM emp 
	WHERE emp.NAME = @username

	;WITH cteMenu
	AS
	(
	SELECT 
		   CAST('' AS VARCHAR(30)) AS 'ParentMenu',
		   menuid ,
		   itemno ,
		   description ,
		   command ,
		   argument 
	FROM dbo.menu
	WHERE menuid = @menu AND (itemno = 0 OR command = 'Menu')
	UNION ALL
	SELECT 
		CAST(cteMenu.menuid AS VARCHAR(30)) AS 'ParentMenu',
		menu.menuid, 
		menu.itemno, 
		menu.description, 
		menu.command, 
		menu.argument
	FROM cteMenu INNER JOIN menu ON cteMenu.argument = menu.menuid AND cteMenu.command = 'Menu'

	)

	SELECT DISTINCT
		cteMenu.ParentMenu,
		cteMenu.menuid ,
		cteMenu.itemno ,
		cteMenu.description ,
		cteMenu.command ,
		cteMenu.argument 
	FROM cteMenu
	WHERE cteMenu.itemno = 0
END
