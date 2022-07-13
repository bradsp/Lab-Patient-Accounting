CREATE TABLE [dbo].[data_reports] (
    [account]    VARCHAR (15)  NULL,
    [report]     VARCHAR (MAX) NOT NULL,
    [batch]      VARCHAR (50)  NULL,
    [batch_time] VARCHAR (4)   NULL,
    [mod_date]   DATETIME      CONSTRAINT [DF_data_reports_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]   VARCHAR (50)  CONSTRAINT [DF_data_reports_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_prg]    VARCHAR (50)  CONSTRAINT [DF_data_reports_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_host]   VARCHAR (50)  CONSTRAINT [DF_data_reports_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL
);

