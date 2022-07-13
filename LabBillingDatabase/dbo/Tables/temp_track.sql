CREATE TABLE [dbo].[temp_track] (
    [comment]    VARCHAR (8000) NOT NULL,
    [row_count]  INT            NULL,
    [error]      VARCHAR (8000) CONSTRAINT [DF_temp_track_error] DEFAULT (getdate()) NULL,
    [start_time] DATETIME       CONSTRAINT [DF_temp_track_start_time] DEFAULT (getdate()) NOT NULL,
    [mod_date]   DATETIME       CONSTRAINT [DF_temp_track_mod_date] DEFAULT (getdate()) NOT NULL
);


GO
-- =============================================
-- Author:		David
-- Create date: 07/01/2014
-- Description:	Get the Error Message for the error number provided
-- =============================================
CREATE TRIGGER dbo.TEMP_TRACK_GET_ERROR 
   ON  dbo.temp_track 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
    UPDATE dbo.temp_track
    SET error = sys.messages.text
    FROM dbo.temp_track
    INNER JOIN INSERTED ON INSERTED.comment = dbo.temp_track.comment
    INNER JOIN sys.messages ON sys.messages.message_id = INSERTED.ERROR
		AND language_id = 1033 


END

GO
DISABLE TRIGGER [dbo].[TEMP_TRACK_GET_ERROR]
    ON [dbo].[temp_track];

