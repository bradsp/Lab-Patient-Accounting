CREATE TABLE [dbo].[data_monthly_ins_report] (
    [account]        VARCHAR (15)  NOT NULL,
    [pat_name]       VARCHAR (256) NOT NULL,
    [pat_sex]        VARCHAR (1)   NOT NULL,
    [ins_holder_sex] VARCHAR (1)   NOT NULL,
    [ins_code]       VARCHAR (50)  NULL,
    [ins_plan_name]  VARCHAR (256) NULL,
    [reported_date]  DATETIME      NOT NULL
);

