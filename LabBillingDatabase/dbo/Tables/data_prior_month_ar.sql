CREATE TABLE [dbo].[data_prior_month_ar] (
    [uid]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [prior_month_ar] NUMERIC (18, 2) NOT NULL,
    [prior_month]    DATETIME        NOT NULL,
    [mod_date]       DATETIME        CONSTRAINT [DF_data_prior_month_ar_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]       VARCHAR (50)    CONSTRAINT [DF_data_prior_month_ar_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]        VARCHAR (50)    CONSTRAINT [DF_data_prior_month_ar_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]       VARCHAR (50)    CONSTRAINT [DF_data_prior_month_ar_mod_host] DEFAULT (host_name()) NOT NULL
);

