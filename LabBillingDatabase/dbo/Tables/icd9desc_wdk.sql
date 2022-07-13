CREATE TABLE [dbo].[icd9desc_wdk] (
    [deleted]   BIT          NULL,
    [icd9_num]  VARCHAR (7)  NOT NULL,
    [icd9_desc] VARCHAR (50) NULL,
    [mod_date]  DATETIME     NULL,
    [mod_user]  VARCHAR (40) NULL,
    [mod_prg]   VARCHAR (40) NULL,
    [AMA_year]  VARCHAR (6)  NOT NULL
);

