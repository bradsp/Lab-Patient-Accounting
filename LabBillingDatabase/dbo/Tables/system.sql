CREATE TABLE [dbo].[system] (
    [key_name]         VARCHAR (25)   NOT NULL,
    [value]            VARCHAR (8000) NULL,
    [programs]         VARCHAR (50)   NULL,
    [restricted_users] VARCHAR (8000) NULL,
    [comment]          VARCHAR (1024) NULL,
    [update_prg]       VARCHAR (255)  NULL,
    [button]           VARCHAR (50)   NULL,
    [mod_date]         DATETIME       CONSTRAINT [DF_system_mod_date_2__26] DEFAULT (getdate()) NOT NULL,
    [mod_user]         VARCHAR (50)   CONSTRAINT [DF_system_mod_user_5__26] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_prg]          VARCHAR (50)   CONSTRAINT [DF_system_mod_prg_4__26] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_host]         VARCHAR (50)   CONSTRAINT [DF_system_mod_host_3__26] DEFAULT (right(host_name(),(50))) NOT NULL,
    [category]         VARCHAR (50)   NULL,
    [description]      VARCHAR (150)  NULL,
    [dataType]         VARCHAR (25)   NULL,
    CONSTRAINT [PK___1__26] PRIMARY KEY CLUSTERED ([key_name] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Added 09/19/2008 to track changes to these values as WHY?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'system', @level2type = N'COLUMN', @level2name = N'comment';

