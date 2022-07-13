CREATE TABLE [dbo].[cdm_map] (
    [vendor]      VARCHAR (30) NOT NULL,
    [vendor_code] VARCHAR (30) NOT NULL,
    [cdm]         VARCHAR (7)  NULL,
    [mod_date]    DATETIME     CONSTRAINT [DF_cdm_map_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]    VARCHAR (50) CONSTRAINT [DF_cdm_map_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_prg]     VARCHAR (50) CONSTRAINT [DF_cdm_map_mod_prg] DEFAULT (app_name()) NULL,
    [mod_host]    VARCHAR (50) CONSTRAINT [DF_cdm_map_mod_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_cdm_map] PRIMARY KEY CLUSTERED ([vendor] ASC, [vendor_code] ASC) WITH (FILLFACTOR = 90)
);

