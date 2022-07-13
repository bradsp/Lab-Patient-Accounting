CREATE TABLE [dbo].[unapp_panels] (
    [deleted]     BIT          CONSTRAINT [DF_unapp_pane_deleted_1__10] DEFAULT ((0)) NOT NULL,
    [profile_cdm] VARCHAR (10) NOT NULL,
    [comp_cdm]    VARCHAR (10) NOT NULL,
    [mod_date]    DATETIME     CONSTRAINT [DF_unapp_pane_mod_date_2__10] DEFAULT (getdate()) NULL,
    [mod_user]    VARCHAR (20) CONSTRAINT [DF_unapp_pane_mod_user_4__10] DEFAULT (suser_sname()) NULL,
    [mod_prg]     VARCHAR (20) CONSTRAINT [DF_unapp_pane_mod_prg_3__10] DEFAULT (app_name()) NULL,
    CONSTRAINT [PK_unapp_panels] PRIMARY KEY CLUSTERED ([profile_cdm] ASC, [comp_cdm] ASC) WITH (FILLFACTOR = 90)
);


GO
/****** Object:  Trigger dbo.tu_unapp_panels    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_unapp_panels] ON [dbo].[unapp_panels] 
FOR UPDATE
AS

UPDATE unapp_panels
SET unapp_panels.mod_user = suser_sname(), unapp_panels.mod_date = getdate(), unapp_panels.mod_prg = left(app_name(),19)
FROM inserted,unapp_panels
WHERE inserted.profile_cdm = unapp_panels.profile_cdm
and inserted.comp_cdm = unapp_panels.comp_cdm
