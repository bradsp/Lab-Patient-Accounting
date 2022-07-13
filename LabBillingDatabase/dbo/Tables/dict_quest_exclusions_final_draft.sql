CREATE TABLE [dbo].[dict_quest_exclusions_final_draft] (
    [CPT]                VARCHAR (5)     NULL,
    [Description]        NVARCHAR (255)  NULL,
    [age_appropriate]    BIT             NULL,
    [outpatient_surgery] BIT             NULL,
    [test_cost]          NUMERIC (18, 2) NULL,
    [start_date]         DATETIME        NOT NULL,
    [expire_date]        DATETIME        NULL
);

