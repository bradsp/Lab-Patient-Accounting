CREATE TABLE [dbo].[pth] (
    [deleted]  BIT          CONSTRAINT [DF_pth_deleted_2__12] DEFAULT ((0)) NOT NULL,
    [pc_code]  INT          IDENTITY (1, 1) NOT NULL,
    [name]     VARCHAR (30) NULL,
    [mc_pin]   VARCHAR (7)  NULL,
    [bc_pin]   VARCHAR (7)  NULL,
    [tlc_num]  VARCHAR (7)  NULL,
    [mod_date] DATETIME     CONSTRAINT [DF_pth_mod_date_3__12] DEFAULT (getdate()) NULL,
    [mod_user] VARCHAR (50) CONSTRAINT [DF_pth_mod_user_4__12] DEFAULT (right(suser_sname(),(50))) NULL,
    [mod_prg]  VARCHAR (50) CONSTRAINT [DF_pth_mod_prg_5__12] DEFAULT (right(app_name(),(50))) NULL,
    CONSTRAINT [PK_pth_1__12] PRIMARY KEY CLUSTERED ([pc_code] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [name_idx]
    ON [dbo].[pth]([name] ASC) WITH (FILLFACTOR = 90);


GO
/****** Object:  Trigger dbo.tu_pth    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_pth] ON [dbo].[pth] 
FOR UPDATE 
AS
UPDATE pth
SET pth.mod_user = suser_sname(), pth.mod_date = getdate(), pth.mod_prg = app_name()
FROM inserted,pth
WHERE inserted.pc_code = pth.pc_code
