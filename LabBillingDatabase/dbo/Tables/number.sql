CREATE TABLE [dbo].[number] (
    [keyfield] VARCHAR (15) NOT NULL,
    [cnt]      NUMERIC (15) NULL,
    CONSTRAINT [PK_number_1__16] PRIMARY KEY CLUSTERED ([keyfield] ASC) WITH (FILLFACTOR = 90)
);

