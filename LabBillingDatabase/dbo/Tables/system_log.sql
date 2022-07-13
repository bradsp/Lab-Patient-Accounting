CREATE TABLE [dbo].[system_log] (
    [log_date]        DATETIME         CONSTRAINT [DF_system_log_log_date] DEFAULT (getdate()) NOT NULL,
    [log_text]        VARCHAR (MAX)    NULL,
    [account]         VARCHAR (15)     NULL,
    [log_function]    VARCHAR (50)     NULL,
    [log_program]     VARCHAR (50)     CONSTRAINT [DF_system_log_log_program] DEFAULT (right(app_name(),(50))) NULL,
    [log_user]        VARCHAR (50)     CONSTRAINT [DF_system_log_log_user] DEFAULT (right(suser_sname(),(50))) NULL,
    [log_workstation] VARCHAR (50)     CONSTRAINT [DF_system_log_log_workstation] DEFAULT (right(host_name(),(50))) NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_system_log_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_system_log] PRIMARY KEY CLUSTERED ([rowguid] ASC) WITH (FILLFACTOR = 90)
);

