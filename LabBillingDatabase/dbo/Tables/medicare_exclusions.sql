CREATE TABLE [dbo].[medicare_exclusions] (
    [cpt_code]       VARCHAR (5)  NOT NULL,
    [effective_date] DATETIME     NOT NULL,
    [mod_date]       DATETIME     CONSTRAINT [DF_medicare_exclusions_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]       VARCHAR (50) CONSTRAINT [DF_medicare_exclusions_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_prg]        VARCHAR (50) CONSTRAINT [DF_medicare_exclusions_mod_prg] DEFAULT (app_name()) NULL,
    [mod_host]       VARCHAR (50) CONSTRAINT [DF_medicare_exclusions_mod_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_medicare_exclusions] PRIMARY KEY CLUSTERED ([cpt_code] ASC, [effective_date] ASC) WITH (FILLFACTOR = 90)
);

