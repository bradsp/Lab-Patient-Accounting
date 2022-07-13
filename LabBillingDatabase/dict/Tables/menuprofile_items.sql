CREATE TABLE [dict].[menuprofile_items] (
    [profile_id] INT            NOT NULL,
    [item_id]    INT            NOT NULL,
    [mod_date]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [mod_user]   NVARCHAR (100) DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]    NVARCHAR (100) DEFAULT (app_name()) NOT NULL,
    [mod_host]   NVARCHAR (100) DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_menuprofile_items] PRIMARY KEY CLUSTERED ([profile_id] ASC, [item_id] ASC)
);

