CREATE TABLE [dbo].[acc_track] (
    [account]    VARCHAR (15) NOT NULL,
    [track_code] VARCHAR (15) NOT NULL,
    [event_date] DATETIME     CONSTRAINT [DF_acc_track_event_date] DEFAULT (getdate()) NULL,
    [event_user] VARCHAR (50) CONSTRAINT [DF_acc_track_event_user] DEFAULT (suser_sname()) NULL,
    [event_prg]  VARCHAR (50) CONSTRAINT [DF_acc_track_event_prg] DEFAULT (app_name()) NULL,
    [event_host] VARCHAR (50) CONSTRAINT [DF_acc_track_event_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_acc_track] PRIMARY KEY CLUSTERED ([account] ASC, [track_code] ASC) WITH (FILLFACTOR = 90)
);

