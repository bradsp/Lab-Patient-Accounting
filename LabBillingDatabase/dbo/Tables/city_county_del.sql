CREATE TABLE [dbo].[city_county_del] (
    [county]   VARCHAR (100) NOT NULL,
    [city]     VARCHAR (100) NOT NULL,
    [mod_user] VARCHAR (50)  CONSTRAINT [DF_city_county_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_date] VARCHAR (50)  CONSTRAINT [DF_city_county_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]  VARCHAR (50)  CONSTRAINT [DF_city_county_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host] VARCHAR (50)  CONSTRAINT [DF_city_county_mod_host] DEFAULT (host_name()) NOT NULL
);

