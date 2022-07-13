CREATE TABLE [dbo].[data_monitor_360] (
    [user360]  VARCHAR (50) NOT NULL,
    [app_date] DATETIME     NOT NULL
);


GO
CREATE CLUSTERED INDEX [IX_data_monitor_360]
    ON [dbo].[data_monitor_360]([user360] ASC) WITH (FILLFACTOR = 90);

