CREATE TABLE [dict].[menuitems] (
    [item_id]     INT            IDENTITY (1, 1) NOT NULL,
    [description] NVARCHAR (50)  NULL,
    [application] NVARCHAR (100) NOT NULL,
    [arguments]   NVARCHAR (100) NULL,
    [apptype]     NVARCHAR (50)  NULL,
    [appcategory] NVARCHAR (50)  NULL,
    [mod_date]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [mod_user]    NVARCHAR (100) DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]     NVARCHAR (100) DEFAULT (app_name()) NOT NULL,
    [mod_host]    NVARCHAR (100) DEFAULT (host_name()) NOT NULL,
    PRIMARY KEY CLUSTERED ([item_id] ASC)
);

