CREATE TABLE [dbo].[dict_cpt4_warnings] (
    [deleted]     BIT              CONSTRAINT [DF_dict_cpt4_warnings_deleted] DEFAULT ((0)) NOT NULL,
    [cpt4]        VARCHAR (5)      CONSTRAINT [DF_dict_cpt4_warnings_cpt4] DEFAULT (NULL) NOT NULL,
    [note]        VARCHAR (1024)   NULL,
    [is_ssi_edit] BIT              CONSTRAINT [DF_dict_cpt4_warnings_is_ssi_edit] DEFAULT ((0)) NOT NULL,
    [mod_date]    DATETIME         CONSTRAINT [DF_dict_diagnosis_warnings_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prog]    VARCHAR (50)     CONSTRAINT [DF_dict_diagnosis_warnings_mod_prog] DEFAULT (app_name()) NOT NULL,
    [mod_user]    VARCHAR (50)     CONSTRAINT [DF_dict_diagnosis_warnings_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_host]    VARCHAR (50)     CONSTRAINT [DF_dict_diagnosis_warnings_mod_host] DEFAULT (host_name()) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER CONSTRAINT [DF_dict_diagnosis_warnings_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    CONSTRAINT [PK_dict_diagnosis_warnings] PRIMARY KEY CLUSTERED ([rowguid] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [CK_dict_cpt4_warnings] CHECK (len([cpt4])=(5))
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_dict_diagnosis_warnings]
    ON [dbo].[dict_cpt4_warnings]([cpt4] ASC) WITH (FILLFACTOR = 90);

