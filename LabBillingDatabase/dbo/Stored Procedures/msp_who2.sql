-- =============================================
-- Author:		Rick and David
-- Create date: 07/11/2007
-- Description:	Try to track the deadlock issue
-- =============================================
CREATE PROCEDURE [dbo].[msp_who2] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	drop table my_who2
	CREATE TABLE my_who2 (
	spid int,
	status varchar(255),
	login varchar(255),
	hostname varchar(255),
	blkby varchar(255),
	dbname varchar(255),
	command varchar(2048),
	CPUTime int,
	DiskIO int,
	LastBatch varchar(255),
	programname varchar(255),
	spid2 int
)

	INSERT INTO my_who2 EXEC ('sp_who2')
	


END
