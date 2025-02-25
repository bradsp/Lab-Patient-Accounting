﻿CREATE TABLE [dbo].[dict_C_MEEDIT] (
    [ME_1]                           NVARCHAR (50)   NOT NULL,
    [ME_2]                           NVARCHAR (50)   NOT NULL,
    [effective_date]                 NVARCHAR (50)   NULL,
    [deletion_date]                  NVARCHAR (50)   NULL,
    [prior_rebundled_code_indicator] NVARCHAR (50)   NULL,
    [standard_policy_statement]      NVARCHAR (1000) NULL,
    [cci_indicator]                  NVARCHAR (50)   NULL,
    CONSTRAINT [PK_me1_me2] PRIMARY KEY CLUSTERED ([ME_1] ASC, [ME_2] ASC) WITH (FILLFACTOR = 90)
);

