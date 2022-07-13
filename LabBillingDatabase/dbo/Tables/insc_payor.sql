CREATE TABLE [dbo].[insc_payor] (
    [name]      VARCHAR (255) NOT NULL,
    [insc_code] VARCHAR (50)  NOT NULL,
    [fin_code]  VARCHAR (50)  NOT NULL,
    [uid]       INT           IDENTITY (1, 1) NOT NULL
);

