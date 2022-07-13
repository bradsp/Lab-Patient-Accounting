CREATE TABLE [dbo].[fin] (
    [deleted]    BIT          CONSTRAINT [DF_fin_deleted_2__13] DEFAULT ((0)) NOT NULL,
    [fin_code]   VARCHAR (10) NOT NULL,
    [res_party]  VARCHAR (40) NULL,
    [form_type]  VARCHAR (30) NULL,
    [chrgsource] VARCHAR (20) NULL,
    [type]       VARCHAR (1)  NULL,
    [h1500]      VARCHAR (1)  NULL,
    [ub92]       VARCHAR (1)  NULL,
    [mod_date]   DATETIME     CONSTRAINT [DF_fin_mod_date_3__13] DEFAULT (getdate()) NULL,
    [mod_user]   VARCHAR (50) CONSTRAINT [DF_fin_mod_user_5__13] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]    VARCHAR (50) CONSTRAINT [DF_fin_mod_prg_4__13] DEFAULT (app_name()) NOT NULL,
    CONSTRAINT [PK_fin_1__13] PRIMARY KEY CLUSTERED ([fin_code] ASC) WITH (FILLFACTOR = 90)
);


GO
/****** Object:  Trigger dbo.tu_fin    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_fin] ON dbo.fin 
FOR UPDATE
AS
UPDATE fin
SET fin.mod_user = suser_sname(), fin.mod_date = getdate(), fin.mod_prg = app_name()
FROM inserted,fin
WHERE inserted.fin_code = fin.fin_code

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 07/20/2007 changed from varchar(20) to varchar(50)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'fin', @level2type = N'COLUMN', @level2name = N'mod_user';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 07/20/2007 changed from varchar(20) to varchar(50)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'fin', @level2type = N'COLUMN', @level2name = N'mod_prg';

