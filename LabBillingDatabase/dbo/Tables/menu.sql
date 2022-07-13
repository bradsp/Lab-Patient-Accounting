CREATE TABLE [dbo].[menu] (
    [menuid]      VARCHAR (15)    NOT NULL,
    [itemno]      DECIMAL (18, 4) NOT NULL,
    [description] VARCHAR (30)    NULL,
    [command]     VARCHAR (15)    NULL,
    [argument]    VARCHAR (100)   NULL,
    [mod_date]    DATETIME        CONSTRAINT [DF_menu_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]    VARCHAR (50)    CONSTRAINT [DF_menu_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_prg]     VARCHAR (50)    CONSTRAINT [DF_menu_mod_prg] DEFAULT (app_name()) NULL,
    [mod_host]    VARCHAR (50)    CONSTRAINT [DF_menu_mod_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_menu] PRIMARY KEY CLUSTERED ([menuid] ASC, [itemno] ASC) WITH (FILLFACTOR = 90)
);

