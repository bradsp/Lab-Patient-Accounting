CREATE TABLE [dbo].[job_track_wdk_del] (
    [uid]       INT            IDENTITY (1, 1) NOT NULL,
    [job_name]  NVARCHAR (150) NOT NULL,
    [job_count] NUMERIC (18)   NOT NULL,
    [mod_date]  DATETIME       CONSTRAINT [DF_job_track_wdk_mod_date] DEFAULT (getdate()) NOT NULL
);

