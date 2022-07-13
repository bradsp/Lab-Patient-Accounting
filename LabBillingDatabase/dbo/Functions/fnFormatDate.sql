CREATE FUNCTION [dbo].[fnFormatDate] (@Datetime DATETIME, @FormatMask VARCHAR(32))
RETURNS VARCHAR(32)
AS
BEGIN
    DECLARE @StringDate VARCHAR(32);
    SET @StringDate = @FormatMask;
    IF (CHARINDEX ('YYYY',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'YYYY', DATENAME(YY, @Datetime));
    IF (CHARINDEX ('YY',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'YY', RIGHT(DATENAME(YY, @Datetime),2));
    IF (CHARINDEX ('Month',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'Month', DATENAME(MM, @Datetime));
    IF (CHARINDEX ('MON',@StringDate COLLATE SQL_Latin1_General_CP1_CS_AS)>0)
       SET @StringDate = REPLACE(@StringDate, 'MON', LEFT(UPPER(DATENAME(MM, @Datetime)),3));
    IF (CHARINDEX ('Mon',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'Mon', LEFT(DATENAME(MM, @Datetime),3));
    IF (CHARINDEX ('MM',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'MM', RIGHT(N'0'+CONVERT(VARCHAR,DATEPART(MM, @Datetime)),2));
    IF (CHARINDEX ('M',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'M', CONVERT(VARCHAR,DATEPART(MM, @Datetime)));
    IF (CHARINDEX ('DD',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'DD', RIGHT(DATENAME(DD, @Datetime),2));
    IF (CHARINDEX ('D',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'D', DATENAME(DD, @Datetime));
	IF (CHARINDEX ('hh', @StringDate) > 0)
		SET @StringDate = REPLACE(@StringDate, 'hh', RIGHT(N'0'+CAST(DATEPART(hour, @Datetime) AS VARCHAR),2));
	IF (CHARINDEX ('ii', @StringDate) > 0)
		SET @StringDate = REPLACE(@StringDate, 'ii', RIGHT(N'0'+CAST(DATEPART(minute, @Datetime) AS VARCHAR),2));
RETURN @StringDate;
END