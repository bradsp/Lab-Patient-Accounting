CREATE TABLE [dbo].[dict_general_ledger_codes] (
    [gl_account_code] VARCHAR (3)   CONSTRAINT [DF_dict_general_ledger_codes_gl_account_code] DEFAULT ('001') NOT NULL,
    [level_1]         VARCHAR (4)   NOT NULL,
    [level_2]         VARCHAR (4)   CONSTRAINT [DF_dict_general_ledger_codes_level_2] DEFAULT ('0000') NOT NULL,
    [level_3]         VARCHAR (4)   CONSTRAINT [DF_dict_general_ledger_codes_level_3] DEFAULT ('0000') NOT NULL,
    [description]     VARCHAR (250) NOT NULL
);

