CREATE TABLE [dbo].[icd9desc] (
    [deleted]   BIT          CONSTRAINT [DF_icd9desc_deleted_2__10] DEFAULT ((0)) NOT NULL,
    [version]   VARCHAR (50) CONSTRAINT [DF_icd9desc_version] DEFAULT ((18)) NOT NULL,
    [icd9_num]  VARCHAR (7)  NOT NULL,
    [icd9_desc] VARCHAR (50) NULL,
    [mod_date]  DATETIME     CONSTRAINT [DF_icd9desc_mod_date_3__10] DEFAULT (getdate()) NULL,
    [mod_user]  VARCHAR (40) CONSTRAINT [DF_icd9desc_mod_user_5__10] DEFAULT (right(suser_sname(),(40))) NULL,
    [mod_prg]   VARCHAR (40) CONSTRAINT [DF_icd9desc_mod_prg_4__10] DEFAULT (right(app_name(),(40))) NULL,
    [AMA_year]  VARCHAR (6)  NOT NULL,
    [id]        BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_icd9desc] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_icd9desc_icd9_desc]
    ON [dbo].[icd9desc]([icd9_desc] ASC)
    INCLUDE([AMA_year]) WITH (FILLFACTOR = 90);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_icd9num_amayear]
    ON [dbo].[icd9desc]([icd9_num] ASC, [AMA_year] ASC, [version] ASC)
    INCLUDE([icd9_desc]) WITH (FILLFACTOR = 90);


GO
/****** Object:  Trigger dbo.tu_icd9desc    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_icd9desc] ON dbo.icd9desc 
FOR UPDATE 
AS
UPDATE icd9desc
SET icd9desc.mod_user = RIGHT(suser_sname(),40), icd9desc.mod_date = getdate(), icd9desc.mod_prg = RIGHT(app_name(),40)
FROM inserted,icd9desc
WHERE inserted.icd9_num = icd9desc.icd9_num
