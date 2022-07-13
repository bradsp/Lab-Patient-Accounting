CREATE TABLE [dbo].[cpt4_link_cnb] (
    [CPT4]     VARCHAR (50)  NOT NULL,
    [link_cdm] VARCHAR (50)  NOT NULL,
    [descript] VARCHAR (MAX) NULL,
    [mod_user] VARCHAR (50)  NOT NULL,
    [mod_date] DATETIME      NOT NULL,
    CONSTRAINT [PK_cpt4_link_cnb] PRIMARY KEY CLUSTERED ([CPT4] ASC, [link_cdm] ASC) WITH (FILLFACTOR = 90)
);

