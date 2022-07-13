CREATE TABLE [dbo].[dict_write_off_codes] (
    [write_off_code]        VARCHAR (4)   NOT NULL,
    [write_off_description] VARCHAR (255) NOT NULL,
    [mod_user]              VARCHAR (50)  CONSTRAINT [DF_write_off_codes_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]               VARCHAR (50)  CONSTRAINT [DF_write_off_codes_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_date]              DATETIME      CONSTRAINT [DF_write_off_codes_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_host]              VARCHAR (50)  CONSTRAINT [DF_write_off_codes_mod_host] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_dict_write_off_codes] PRIMARY KEY CLUSTERED ([write_off_code] ASC) WITH (FILLFACTOR = 90)
);

