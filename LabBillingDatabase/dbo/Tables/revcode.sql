CREATE TABLE [dbo].[revcode] (
    [code]        VARCHAR (4)  NOT NULL,
    [description] VARCHAR (25) NULL,
    [mod_date]    DATETIME     CONSTRAINT [DF_revcode_mod_date_3__11] DEFAULT (getdate()) NULL,
    [mod_user]    VARCHAR (20) CONSTRAINT [DF_revcode_mod_user_6__11] DEFAULT (suser_sname()) NULL,
    [mod_prg]     VARCHAR (20) CONSTRAINT [DF_revcode_mod_prg_5__11] DEFAULT (app_name()) NULL,
    [mod_host]    VARCHAR (20) CONSTRAINT [DF_revcode_mod_host_4__11] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK___2__11] PRIMARY KEY CLUSTERED ([code] ASC) WITH (FILLFACTOR = 90)
);


GO
/****** Object:  Trigger dbo.tu_revcode    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_revcode] ON [dbo].[revcode] 
FOR UPDATE
AS
UPDATE revcode
SET revcode.mod_user = suser_sname(), revcode.mod_date = getdate(), revcode.mod_prg = app_name()
FROM inserted,revcode
WHERE revcode.code = inserted.code
