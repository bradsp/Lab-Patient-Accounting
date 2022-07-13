CREATE TABLE [dict].[menuprofile] (
    [profile_id]   INT            IDENTITY (1, 1) NOT NULL,
    [profile_name] NVARCHAR (100) NOT NULL,
    [mod_date]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [mod_user]     NVARCHAR (100) DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]      NVARCHAR (100) DEFAULT (app_name()) NOT NULL,
    [mod_host]     NVARCHAR (100) DEFAULT (host_name()) NOT NULL,
    PRIMARY KEY CLUSTERED ([profile_id] ASC)
);

