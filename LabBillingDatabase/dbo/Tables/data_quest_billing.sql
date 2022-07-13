CREATE TABLE [dbo].[data_quest_billing] (
    [deleted]         BIT            CONSTRAINT [DF_data_quest_billing_deleted] DEFAULT ((0)) NOT NULL,
    [status]          VARCHAR (50)   NULL,
    [req_no]          VARCHAR (50)   NULL,
    [uid]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [account]         VARCHAR (15)   NULL,
    [Patient]         VARCHAR (255)  NULL,
    [collection_date] DATETIME       NULL,
    [DOB]             DATETIME       NULL,
    [atl_user]        VARCHAR (50)   NULL,
    [cdm]             VARCHAR (7)    NULL,
    [quest_code]      VARCHAR (50)   NULL,
    [quest_desc]      VARCHAR (MAX)  NULL,
    [quest_cpt4]      VARCHAR (MAX)  NULL,
    [date_entered]    DATETIME       NULL,
    [SSN]             VARCHAR (11)   NULL,
    [post_date]       DATETIME       NULL,
    [invoice]         VARCHAR (50)   NULL,
    [comment]         VARCHAR (1024) NULL,
    [mod_date]        DATETIME       CONSTRAINT [DF_data_quest_billing_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]        VARCHAR (50)   CONSTRAINT [DF_data_quest_billing_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]         VARCHAR (50)   CONSTRAINT [DF_data_quest_billing_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]        VARCHAR (50)   CONSTRAINT [DF_data_quest_billing_mod_host] DEFAULT (host_name()) NOT NULL,
    [mod_file]        VARCHAR (255)  NOT NULL,
    CONSTRAINT [PK_data_quest_billing] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_data_quest_billing_quest_code]
    ON [dbo].[data_quest_billing]([account] ASC, [req_no] ASC, [quest_code] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'this is the PID from the File', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_quest_billing', @level2type = N'COLUMN', @level2name = N'account';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'this is seperated by a line feed between tests with the following format "867 - T4 (Thyroxine), Total
899 - Tsh
3259 - Draw Fee, Psc Specimen
6399 - Cbc (Includes Diff/plt)
10231 - Comprehensive Metabolic Panel"', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_quest_billing', @level2type = N'COLUMN', @level2name = N'quest_cpt4';

