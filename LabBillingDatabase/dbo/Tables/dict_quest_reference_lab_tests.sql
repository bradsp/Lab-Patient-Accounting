CREATE TABLE [dbo].[dict_quest_reference_lab_tests] (
    [deleted]           BIT           CONSTRAINT [DF_dict_quest_reference_lab_tests_deleted] DEFAULT ((0)) NOT NULL,
    [start_date]        DATETIME      NOT NULL,
    [expire_date]       DATETIME      NULL,
    [mt_mnem]           VARCHAR (50)  NULL,
    [has_multiples]     BIT           CONSTRAINT [DF_dict_quest_reference_lab_tests_has_multiples] DEFAULT ((0)) NOT NULL,
    [cdm]               VARCHAR (50)  NOT NULL,
    [cdm_description]   VARCHAR (255) NULL,
    [cpt4]              VARCHAR (5)   NOT NULL,
    [cpt4_description]  VARCHAR (255) NULL,
    [link]              INT           NOT NULL,
    [quest_code]        VARCHAR (50)  NULL,
    [quest_description] VARCHAR (255) NULL,
    [mod_date]          DATETIME      CONSTRAINT [DF_dict_quest_reference_lab_tests_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]          VARCHAR (50)  CONSTRAINT [DF_dict_quest_reference_lab_tests_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]           VARCHAR (50)  CONSTRAINT [DF_dict_quest_reference_lab_tests_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]          VARCHAR (50)  CONSTRAINT [DF_dict_quest_reference_lab_tests_mod_host] DEFAULT (host_name()) NOT NULL,
    [uid]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [amendment]         INT           NULL,
    CONSTRAINT [PK_dict_quest_reference_lab_tests] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_dict_quest_ref_lab_code]
    ON [dbo].[dict_quest_reference_lab_tests]([quest_code] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_deleted_multiples_startdate_cdm]
    ON [dbo].[dict_quest_reference_lab_tests]([deleted] ASC, [has_multiples] ASC, [cdm] ASC, [link] ASC, [start_date] ASC)
    INCLUDE([expire_date], [quest_code], [quest_description]) WITH (FILLFACTOR = 90);

