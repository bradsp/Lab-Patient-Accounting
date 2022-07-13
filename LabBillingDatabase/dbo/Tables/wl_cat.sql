CREATE TABLE [dbo].[wl_cat] (
    [deleted]   BIT          CONSTRAINT [DF_wl_cat_deleted] DEFAULT ((0)) NOT NULL,
    [uri]       INT          IDENTITY (1, 1) NOT NULL,
    [type_desc] VARCHAR (40) NULL,
    [units]     VARCHAR (20) NULL,
    [mod_date]  DATETIME     CONSTRAINT [DF_wl_cat_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]  VARCHAR (20) CONSTRAINT [DF_wl_cat_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_prg]   VARCHAR (20) CONSTRAINT [DF_wl_cat_mod_prg] DEFAULT (app_name()) NULL,
    CONSTRAINT [PK_wl_cat] PRIMARY KEY NONCLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);

