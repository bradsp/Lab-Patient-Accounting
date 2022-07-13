-- =============================================
-- Author:		Bradley Powers
-- Create date: 1/9/2021
-- Description:	Gets last number based on number key and returns next number. Increments value.
-- =============================================
CREATE PROCEDURE [dbo].[GetNextNumber]
	-- Add the parameters for the stored procedure here
( @keyfield varchar(15),
  @NextSequence int OUTPUT )
AS
	SET NOCOUNT ON
    UPDATE number SET
    @NextSequence = cnt,
    cnt = cnt + 1
	where keyfield = @keyfield

RETURN @NextSequence + 1

