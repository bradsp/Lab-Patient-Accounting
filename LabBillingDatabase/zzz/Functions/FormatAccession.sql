CREATE FUNCTION [zzz].[FormatAccession]
(
    @accessionNo varchar(255)
)
RETURNS VARCHAR(20)
AS
BEGIN
	DECLARE @accessionReturn varchar(20)

	IF len(@accessionNo) > 13 and left(@accessionNo,4) = '<FT1'
	BEGIN
		set @accessionReturn = (select substring(@accessionNo,charindex('>',@accessionNo,1)+1,
					charindex('</FT1',@accessionNo,1)-charindex('>',@accessionNo,1)-1));
		set @accessionReturn = (select SUBSTRING(@accessionReturn,PATINDEX('%[^0 ]%',@accessionReturn+' '),len(@accessionReturn)));
		if len(@accessionReturn) = 13
		begin
			if left(@accessionReturn,2) = '20'
			begin
				set @accessionReturn = (select STUFF(@accessionReturn,8,0,'-'))
				set @accessionReturn = (select STUFF(@accessionReturn,5,0,'-'))
			end
			else
			begin
				set @accessionReturn = (select STUFF(@accessionReturn,7,0,'-'))
				set @accessionReturn = (select STUFF(@accessionReturn,3,0,'-'))
			end
		end
	END
	ELSE
	BEGIN
		SET @accessionReturn = @accessionNo	
	END

    RETURN @accessionReturn

END
