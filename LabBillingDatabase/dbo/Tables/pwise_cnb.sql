CREATE TABLE [dbo].[pwise_cnb] (
    [TheAcct]  VARCHAR (15) NOT NULL,
    [TheDate]  CHAR (20)    NOT NULL,
    [Mod_date] CHAR (20)    NOT NULL,
    CONSTRAINT [PK_pwise_cnb] PRIMARY KEY CLUSTERED ([TheAcct] ASC, [TheDate] ASC, [Mod_date] ASC) WITH (FILLFACTOR = 90)
);

