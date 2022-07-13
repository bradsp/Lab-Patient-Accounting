CREATE TABLE [dbo].[cdm_link_cnb] (
    [CDM]      VARCHAR (50) NULL,
    [link_cnt] VARCHAR (50) NOT NULL,
    [mod_user] VARCHAR (50) NOT NULL,
    [mod_date] DATETIME     NOT NULL
);


GO
CREATE CLUSTERED INDEX [IX_cdm_link_cnb]
    ON [dbo].[cdm_link_cnb]([CDM] ASC, [link_cnt] ASC) WITH (FILLFACTOR = 90);

