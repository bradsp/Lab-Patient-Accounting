﻿CREATE TABLE [dbo].[chrg_unprocessed] (
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_unchrg_unprocessed_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [account]         VARCHAR (15)     NULL,
    [status]          VARCHAR (15)     CONSTRAINT [DF_unchrg_status_1__10] DEFAULT ('NEW') NULL,
    [service_date]    DATETIME         CONSTRAINT [DF_unchrg_service_date_1__11] DEFAULT (getdate()) NULL,
    [cdm]             VARCHAR (7)      NULL,
    [qty]             NUMERIC (4)      NULL,
    [mod_date]        DATETIME         CONSTRAINT [DF_unchrg_mod_date_3__12] DEFAULT (getdate()) NULL,
    [mod_user]        VARCHAR (50)     CONSTRAINT [DF_unchrg_mod_user_5__12] DEFAULT (suser_sname()) NULL,
    [mod_prg]         VARCHAR (50)     CONSTRAINT [DF_unchrg_mod_prg_4__12] DEFAULT (app_name()) NULL,
    [mod_host]        VARCHAR (50)     CONSTRAINT [DF_unchrg_mod_host_1__16] DEFAULT (host_name()) NULL,
    [mt_req_no]       VARCHAR (50)     NULL,
    [performing_site] VARCHAR (50)     NULL,
    [post_file]       VARCHAR (50)     NULL,
    [lname]           VARCHAR (50)     NULL,
    [fname]           VARCHAR (100)    NULL,
    [mname]           VARCHAR (100)    NULL,
    [name_suffix]     VARCHAR (100)    NULL,
    [name_prefix]     VARCHAR (100)    NULL,
    [pat_name]        VARCHAR (100)    NULL,
    [order_site]      VARCHAR (100)    NULL,
    [location]        VARCHAR (50)     NULL,
    [responsiblephy]  VARCHAR (50)     NULL,
    [mt_mnem]         VARCHAR (50)     NULL,
    [action]          VARCHAR (50)     NULL,
    [facility]        VARCHAR (50)     NULL,
    [pat_dob]         DATETIME         NULL,
    [chrg_err]        VARCHAR (8000)   NULL
);
