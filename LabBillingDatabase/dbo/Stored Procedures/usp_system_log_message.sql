-- =============================================
-- Author:		Bradley Powers
-- Create date: 8/12/2013
-- Description:	Write message to system_log
-- =============================================
CREATE PROCEDURE usp_system_log_message 
	-- Add the parameters for the stored procedure here
	@log_text varchar(max) = NULL, 
	@account varchar(15) = NULL,
	@log_function varchar(50) = NULL,
	@log_program varchar(50) = NULL,
	@log_user varchar(50) = NULL,
	@log_workstation varchar(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT [dbo].[system_log]([log_text],[account],[log_function],[log_program],[log_user],[log_workstation])
	VALUES(@log_text, @account, @log_function, @log_program, @log_user, @log_workstation);


END
