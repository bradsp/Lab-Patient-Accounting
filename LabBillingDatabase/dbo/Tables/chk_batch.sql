CREATE TABLE [dbo].[chk_batch] (
    [BatchNo]   INT           IDENTITY (1, 1) NOT NULL,
    [User]      NVARCHAR (50) NOT NULL,
    [BatchDate] DATE          NOT NULL,
    [BatchData] XML           NULL,
    [mod_date]  DATETIME      CONSTRAINT [DF_chk_batch_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]  VARCHAR (50)  CONSTRAINT [DF_chk_batch_mod_user] DEFAULT (right(suser_sname(),(50))) NULL,
    [mod_prg]   VARCHAR (50)  CONSTRAINT [DF_chk_batch_mod_prg] DEFAULT (right(app_name(),(50))) NULL,
    [mod_host]  VARCHAR (50)  CONSTRAINT [DF_chk_batch_mod_host] DEFAULT (right(host_name(),(50))) NULL,
    CONSTRAINT [PK_chk_batch] PRIMARY KEY CLUSTERED ([BatchNo] ASC)
);

