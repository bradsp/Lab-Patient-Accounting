-- =============================================
-- Author:		David
-- Create date: 11/18/2013
-- Description:	Gets a table with name parts
-- =============================================
CREATE FUNCTION [dbo].[ufn_Split_Name] 
(
	-- Add the parameters for the function here
	@fullname varchar(256)
	 
)
RETURNS 
 @Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	cFNAME varchar(100), 
	cMNAME varchar(100),
	cLNAME varchar(100),
	cSUFFIX varchar(50),
	cPREFIX varchar(50) -- not implemented at creation time	
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	--assumes the comma is ALWAYS in the data:
SET @fullname = REPLACE(@fullname,'"','')	
if (charindex(',',@fullname) <=0 or @fullname like '%,%,%')
begin
	insert into @Table_Var (cLNAME)
	values (@fullname)
end
else
begin
insert into @Table_Var (cSUFFIX,cLNAME,cFNAME,cMNAME)
select 
upper(case when patindex('%I',substring(@fullname,0,charindex(',',@fullname))) >0
			or patindex('%II',substring(@fullname,0,charindex(',',@fullname)))>0
			or patindex('%III',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%IV',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%V',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%JR',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%SR',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%MD',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%VI',substring(@fullname,0,charindex(',',@fullname))) > 0
	then reverse(substring(reverse(substring(@fullname, 0, charindex(',',@fullname))),0,charindex(' ',reverse(substring(@fullname, 0, charindex(',',@fullname))))))
	else ''
end)
as [SUFFIX],  

upper(case when patindex('%I',substring(@fullname,0,charindex(',',@fullname))) >0
			or patindex('%II',substring(@fullname,0,charindex(',',@fullname)))>0
			or patindex('%III',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%IV',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%V',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%JR',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%SR',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%MD',substring(@fullname,0,charindex(',',@fullname))) > 0
			or patindex('%VI',substring(@fullname,0,charindex(',',@fullname))) > 0
	then reverse(substring(reverse(substring(@fullname,0,charindex(',',@fullname))),charindex(' ',reverse(substring(@fullname,0,charindex(',',@fullname)))),100))
	else substring(@fullname,0,charindex(',',@fullname))
end)
as [LAST NAME],
upper(
case 
WHEN CHARINDEX(' ',SUBSTRING(@fullname,CHARINDEX(',',@fullname)+ 1,30)) > 1 
then
substring (
				@fullname,
				charindex(',', @fullname,0)+1,
				charindex(' '
					, substring(@fullname,charindex(',',@fullname)+1,100)
					,charindex(' ', @fullname,charindex(',',@fullname)+1)- charindex(',', @fullname,0))
		  )
else
	SUBSTRING(@fullname,CHARINDEX(',',@fullname)+ 1,30)
	end)
	 as [FNAME]	
 , upper(CASE 
    --if there is a space, ASSUME the second word is a middle name, and not a two part first name
    WHEN CHARINDEX(' ',SUBSTRING(@fullname,CHARINDEX(',',@fullname)+ 1,30)) > 1 
    THEN SUBSTRING(SUBSTRING(@fullname,CHARINDEX(',',@fullname)+ 1,30),CHARINDEX(' ',SUBSTRING(@fullname,CHARINDEX(',',@fullname)+ 1,30)),30)
    ELSE ''
  END )AS MNAME
end

	RETURN 
END
