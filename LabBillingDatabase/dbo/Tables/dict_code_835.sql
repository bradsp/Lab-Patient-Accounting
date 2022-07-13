CREATE TABLE [dbo].[dict_code_835] (
    [code_type]          VARCHAR (50)   NOT NULL,
    [code]               VARCHAR (50)   NOT NULL,
    [code_description]   VARCHAR (2096) NOT NULL,
    [date_start]         DATETIME       NULL,
    [date_end]           DATETIME       NULL,
    [date_last_modified] DATETIME       NULL,
    [action]             VARCHAR (50)   NULL
);

