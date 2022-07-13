CREATE TABLE [dbo].[dict_job_scheduler] (
    [stored_procedure]     NVARCHAR (256) NOT NULL,
    [run_date_day_of_week] INT            NOT NULL,
    [run_date_last]        DATETIME       NOT NULL,
    [run_hour]             INT            NULL,
    [run_hour_last]        INT            NULL,
    [run_date_datepart]    VARCHAR (5)    NOT NULL,
    [paramater_list]       XML            NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'xml fragment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'dict_job_scheduler', @level2type = N'COLUMN', @level2name = N'paramater_list';

