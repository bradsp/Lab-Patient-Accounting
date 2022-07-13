CREATE TABLE [dictionary].[icd9gem] (
    [version] VARCHAR (2)  NOT NULL,
    [icd9]    NVARCHAR (7) NOT NULL,
    [icd10]   NVARCHAR (9) NULL,
    [flags]   NVARCHAR (7) NULL,
    [uid]     NUMERIC (18) NOT NULL
);

