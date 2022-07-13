CREATE TABLE [dbo].[dict_reference_business] (
    [deleted]       BIT             CONSTRAINT [DF_dict_outreach_supplies_deleted] DEFAULT ((0)) NOT NULL,
    [cdm]           VARCHAR (7)     NOT NULL,
    [description]   VARCHAR (256)   NOT NULL,
    [cost_per_test] NUMERIC (18, 2) NOT NULL,
    [performed_by]  VARCHAR (50)    CONSTRAINT [DF_dict_outreach_supplies_performed_by] DEFAULT ('QUEST') NOT NULL,
    [mod_date]      DATETIME        CONSTRAINT [DF_dictionary.outreach_supplies_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]       VARCHAR (50)    CONSTRAINT [DF_dictionary.outreach_supplies_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]      VARCHAR (50)    CONSTRAINT [DF_dictionary.outreach_supplies_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]      VARCHAR (50)    CONSTRAINT [DF_dictionary.outreach_supplies_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL
);

