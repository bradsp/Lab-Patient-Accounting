-- =============================================
-- Author:		David
-- Create date: FormatClient
-- Description:	formats the client from the charge file input
-- =============================================
CREATE FUNCTION FormatClient 
(
	-- Add the parameters for the function here
	@facility varchar(12), 
	@attendphy varchar(12)
)
RETURNS VARCHAR(256)
--@Table_Var TABLE 
--(
--	-- Add the column definitions for the TABLE variable here
--	colClient varchar(10)
--	
--)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	DECLARE @error VARCHAR(512)
	
	SET @facility =  LTRIM(RTRIM(REPLACE(@facility,'"','')))
	SET @attendphy = LTRIM(RTRIM(REPLACE(@attendphy,'"','')))
	
	IF (@facility = 'MRP')
	BEGIN
		SELECT @facility = (SELECT CASE 
				WHEN ASCII(LEFT(@attendphy,1)) = 122 
				THEN UPPER(SUBSTRING(@attendphy,2,LEN(@attendphy)) )
			 	ELSE UPPER(@attendphy)
				END)
	IF (NOT EXISTS(SELECT cli_mnem FROM client 
			WHERE deleted = 0 AND cli_mnem = @facility))
	BEGIN
		SELECT @facility = 'ERROR: Neither ['+@facility+'] nor [' + @attendphy+'] are valid clients.'
	END	
				
	RETURN @facility
	END						
--	SELECT @facility = (SELECT CASE
--							WHEN @facility = 'MRP' THEN   
--								CASE 
--									WHEN ASCII(LEFT(@attendphy,1)) = 122 
--									THEN LTRIM(RTRIM(UPPER(SUBSTRING(@attendphy,2,LEN(@attendphy)) )))
--									ELSE UPPER(LTRIM(RTRIM(@attendphy)))
--								END
--								ELSE UPPER(LTRIM(RTRIM(@facility)))
--							END)
							
		

	SELECT @facility = (SELECT CASE WHEN @facility = 'BG' THEN 'BCH'
								  WHEN @facility = 'CG' THEN 'CGH'
								  WHEN @facility = 'MG' THEN 'COM'
--							ELSE CASE WHEN ASCII(LEFT(@facility,1)) = 122
--									THEN LTRIM(RTRIM(UPPER(SUBSTRING(@facility,2,LEN(@facility)))))
--									ELSE UPPER(LTRIM(RTRIM(@facility))) END
							END)

	
	
--	INSERT INTO @Table_Var
--			( colClient )
--	
--	
--	VALUES (@facility)
	
	RETURN LTRIM(RTRIM(@facility))
END
