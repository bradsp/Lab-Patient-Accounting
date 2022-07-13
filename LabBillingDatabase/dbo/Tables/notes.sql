CREATE TABLE [dbo].[notes] (
    [account]  VARCHAR (15)     NOT NULL,
    [mod_date] DATETIME         CONSTRAINT [DF_notes_mod_date] DEFAULT (getdate()) NULL,
    [mod_user] VARCHAR (50)     CONSTRAINT [DF_notes_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_prg]  VARCHAR (50)     CONSTRAINT [DF_notes_mod_prg] DEFAULT (app_name()) NULL,
    [mod_host] VARCHAR (50)     CONSTRAINT [DF_notes_mod_host] DEFAULT (host_name()) NULL,
    [comment]  VARCHAR (MAX)    NULL,
    [rowguid]  UNIQUEIDENTIFIER CONSTRAINT [DF_notes_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_rowguid] PRIMARY KEY NONCLUSTERED ([rowguid] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON)
);


GO
CREATE CLUSTERED INDEX [IX_account]
    ON [dbo].[notes]([account] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'20090209 wdk changed to varchar(MAX) from text billing Acc was thowing an unspecified error. "The data types text and varchar are incompatible in the equal operator."', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'notes', @level2type = N'COLUMN', @level2name = N'comment';

