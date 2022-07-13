--IF NOT EXISTS (SELECT * FROM sys.objects 
--WHERE object_id = OBJECT_ID(N'[dbo].[OVNumberTrim]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
--BEGIN
-- =============================================
-- Author:		David
-- Create date: 11/18/2013
-- Description:	Trim leading zeros from account number
-- =============================================
CREATE FUNCTION [dbo].[OVNumberTrim] 
(
	-- Add the parameters for the function here
	@num varchar(50)
)
RETURNS varchar(15)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(15)
	SET @num = REPLACE(REPLACE(@num,'"',''),'&QUOT;','')

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = rtrim(upper( 
		case 
		when patindex('0000020%',upper(@num)) = 1
			then stuff(@num,PATINDEX('0000020%',@num),7,'')
		when patindex('00000MS20%',upper(@num)) = 1
			then stuff(@num,PATINDEX('00000MS20%',@num),9,'MS')
		when patindex('00000UC20%',upper(@num)) = 1
			then stuff(@num,PATINDEX('00000UC20%',@num),9,'UC')
		when patindex('00000RS20%',upper(@num)) = 1
			then stuff(@num,PATINDEX('00000RS20%',@num),9,'RS')	
		when patindex('00000FL20%',upper(@num)) = 1
			then stuff(@num,PATINDEX('00000FL20%',@num),9,'FL')	
		when patindex('00000 P20%',upper(@num)) = 1
			then stuff(@num,PATINDEX('00000 P20%',@num),9,' P')	
		when patindex('00000GN20%',upper(@num)) = 1
			then stuff(@num,PATINDEX('00000GN20%',@num),9,'GN')	
		when patindex('00000BC20%',upper(@num)) = 1
			then stuff(@num,PATINDEX('00000BC20%',@num),9,'BC')	
		when patindex('00000[A-Z][A-Z]20%',upper(@num)) = 1
			then stuff(@num,PATINDEX('00000[A-Z][A-Z]20%',@num),9
			,SUBSTRING(@num,6,2))	
			else
		@num			
--			case when patindex('%20%',upper(@num)) = 1
--				then stuff(@num,0,patindex('%20%',upper(@num)),'')
--				else
--			case when patindex('000000%',upper(@num)) = 1
--				then stuff(@num,3,1,'b')
--			else @num
--			end
--			end
			
		end)) 
	--select @Result
	-- Return the result of the function
	
	RETURN replace(STUFF(STUFF(@Result,3,0,'-'),7,0,'-'),'"','')

END

