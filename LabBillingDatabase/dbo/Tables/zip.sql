CREATE TABLE [dbo].[zip] (
    [zip]      VARCHAR (10) NOT NULL,
    [city]     VARCHAR (30) NULL,
    [st]       VARCHAR (2)  NULL,
    [mod_date] DATETIME     CONSTRAINT [DF_zip2_mod_date_2__18] DEFAULT (getdate()) NULL,
    [mod_user] VARCHAR (20) CONSTRAINT [DF_zip2_mod_user_4__18] DEFAULT (right(suser_sname(),(20))) NULL,
    [mod_prg]  VARCHAR (20) CONSTRAINT [DF_zip2_mod_prg_3__18] DEFAULT (right(app_name(),(20))) NULL,
    CONSTRAINT [PK___1__18] PRIMARY KEY CLUSTERED ([zip] ASC) WITH (FILLFACTOR = 90)
);


GO
/****** Object:  Trigger dbo.tu_zip    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_zip] ON [dbo].[zip] 
FOR UPDATE
AS
UPDATE zip
SET zip.mod_user = suser_sname(), zip.mod_date = getdate(), zip.mod_prg = app_name()
FROM inserted,zip
WHERE inserted.zip = zip.zip
