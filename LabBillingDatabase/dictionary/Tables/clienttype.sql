CREATE TABLE [dictionary].[clienttype] (
    [type]        INT          NOT NULL,
    [description] VARCHAR (30) NULL,
    CONSTRAINT [PK_clienttype] PRIMARY KEY CLUSTERED ([type] ASC) WITH (FILLFACTOR = 90)
);

