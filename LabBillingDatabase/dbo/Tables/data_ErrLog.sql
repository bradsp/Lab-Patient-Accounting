CREATE TABLE [dbo].[data_ErrLog] (
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_data_ErrLog_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]     BIT              CONSTRAINT [DF_data_ErrLog_deleted] DEFAULT ((0)) NOT NULL,
    [uri]         NUMERIC (18)     IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [App_Name]    VARCHAR (50)     CONSTRAINT [DF_data_ErrLog_App_Name] DEFAULT ('') NULL,
    [Error_Msg]   VARCHAR (1024)   CONSTRAINT [DF_data_ErrLog_Error_Msg] DEFAULT ('') NULL,
    [Error_Level] INT              CONSTRAINT [DF_data_ErrLog_Error_Level] DEFAULT ((0)) NULL,
    [Stack_Trace] VARCHAR (MAX)    NULL,
    [mod_date]    DATETIME         CONSTRAINT [DF_data_ErrLog_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]    VARCHAR (50)     CONSTRAINT [DF_data_ErrLog_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_host]    VARCHAR (50)     CONSTRAINT [DF_data_ErrLog_mod_host] DEFAULT (host_name()) NOT NULL,
    [mod_prg]     VARCHAR (50)     CONSTRAINT [DF_data_ErrLog_mod_prg] DEFAULT (app_name()) NOT NULL,
    CONSTRAINT [PK_data_ErrLog] PRIMARY KEY CLUSTERED ([rowguid] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '07/17/2008 wdk table added for RFClassLibrary ERR UpdateDataBase()', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_ErrLog', @level2type = N'COLUMN', @level2name = N'Error_Level';

