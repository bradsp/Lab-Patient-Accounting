CREATE TABLE [dbo].[perform_site] (
    [site_code]     VARCHAR (10) NOT NULL,
    [site_name]     VARCHAR (50) NULL,
    [site_address]  VARCHAR (30) NULL,
    [site_city]     VARCHAR (30) NULL,
    [site_st]       VARCHAR (2)  NULL,
    [site_zip]      VARCHAR (10) NULL,
    [site_phone]    VARCHAR (20) NULL,
    [site_director] VARCHAR (50) NULL,
    [site_clia]     VARCHAR (20) NULL,
    [mod_date]      VARCHAR (50) NULL,
    [mod_user]      VARCHAR (50) NULL,
    [mod_prg]       VARCHAR (50) NULL,
    [mod_host]      VARCHAR (50) NULL,
    [referral_lab]  BIT          NULL,
    CONSTRAINT [PK_perform_site] PRIMARY KEY CLUSTERED ([site_code] ASC) WITH (FILLFACTOR = 90)
);

