CREATE TABLE [dbo].[chrg_pathorders] (
    [ACCOUNT]         NVARCHAR (50) NULL,
    [PATIENT_NAME]    NVARCHAR (50) NULL,
    [REG_DATE]        NVARCHAR (50) NULL,
    [ENCOUNTER_TYPE]  NVARCHAR (50) NULL,
    [LOCATION]        NVARCHAR (50) NULL,
    [ORIG_ORDER_DTTM] NVARCHAR (50) NULL,
    [ORDER_DESC]      NVARCHAR (50) NULL,
    [ORDER_STATUS]    NVARCHAR (50) NULL,
    [ORDER_ID]        NUMERIC (18)  NULL,
    [mod_date]        DATETIME      CONSTRAINT [DF_chrg_pathorders_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]         VARCHAR (50)  CONSTRAINT [DF_chrg_pathorders_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]        VARCHAR (50)  CONSTRAINT [DF_chrg_pathorders_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]        VARCHAR (50)  CONSTRAINT [DF_chrg_pathorders_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [mod_file_date]   VARCHAR (50)  CONSTRAINT [DF_chrg_pathorders_mod_file_date] DEFAULT ('pathorders '+CONVERT([varchar],getdate(),(112))) NOT NULL
);

