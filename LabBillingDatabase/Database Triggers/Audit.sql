CREATE TRIGGER [Audit] ON DATABASE
FOR DDL_DATABASE_LEVEL_EVENTS
AS
DECLARE @data XML
DECLARE @cmd NVARCHAR(1000)
DECLARE @posttime NVARCHAR(24)
DECLARE @spid NVARCHAR(6)
DECLARE @loginname NVARCHAR(100)
DECLARE @hostname NVARCHAR(100)
SET @data = EVENTDATA()
SET @cmd = @data.value('(/EVENT_INSTANCE/TSQLCommand/CommandText)[1]', 'NVARCHAR(1000)')
SET @cmd = LTRIM(RTRIM(REPLACE(@cmd,'','')))
SET @posttime = @data.value('(/EVENT_INSTANCE/PostTime)[1]', 'NVARCHAR(24)')
SET @spid = @data.value('(/EVENT_INSTANCE/SPID)[1]', 'nvarchar(6)')
SET @loginname = @data.value('(/EVENT_INSTANCE/LoginName)[1]',
    'NVARCHAR(100)')
SET @hostname = HOST_NAME()
INSERT INTO dbo.AuditLog(Command, PostTime,HostName,LoginName)
 VALUES(@cmd, @posttime, @hostname, @loginname)


