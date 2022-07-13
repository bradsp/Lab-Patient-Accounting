CREATE TABLE [dbo].[cpt4_ama] (
    [cpt4]            VARCHAR (5)   NOT NULL,
    [short_desc]      VARCHAR (MAX) NULL,
    [med_description] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_cpt4_ama] PRIMARY KEY CLUSTERED ([cpt4] ASC) WITH (FILLFACTOR = 90)
);

