﻿CREATE TABLE [dbo].[chrg_test] (
    [rowguid]                UNIQUEIDENTIFIER NULL,
    [credited]               BIT              NULL,
    [chrg_num]               NUMERIC (15)     IDENTITY (1, 1) NOT NULL,
    [account]                VARCHAR (15)     NULL,
    [status]                 VARCHAR (15)     NULL,
    [service_date]           DATETIME         NULL,
    [hist_date]              DATETIME         NULL,
    [cdm]                    VARCHAR (7)      NULL,
    [qty]                    NUMERIC (4)      NULL,
    [retail]                 MONEY            NULL,
    [inp_price]              MONEY            NULL,
    [comment]                VARCHAR (50)     NULL,
    [invoice]                VARCHAR (15)     NULL,
    [mod_date]               DATETIME         NULL,
    [mod_user]               VARCHAR (50)     NULL,
    [mod_prg]                VARCHAR (50)     NULL,
    [net_amt]                MONEY            NULL,
    [fin_type]               VARCHAR (1)      NULL,
    [mod_host]               VARCHAR (50)     NULL,
    [mt_req_no]              VARCHAR (50)     NULL,
    [post_date]              DATETIME         NULL,
    [fin_code]               VARCHAR (50)     NULL,
    [performing_site]        VARCHAR (50)     NULL,
    [bill_method]            VARCHAR (50)     NULL,
    [post_file]              VARCHAR (50)     NULL,
    [lname]                  VARCHAR (100)    NULL,
    [fname]                  VARCHAR (100)    NULL,
    [mname]                  VARCHAR (100)    NULL,
    [name_suffix]            VARCHAR (100)    NULL,
    [name_prefix]            VARCHAR (100)    NULL,
    [pat_name]               VARCHAR (100)    NULL,
    [order_site]             VARCHAR (100)    NULL,
    [pat_ssn]                VARCHAR (100)    NULL,
    [unitno]                 VARCHAR (50)     NULL,
    [location]               VARCHAR (50)     NULL,
    [responsiblephy]         VARCHAR (50)     NULL,
    [mt_mnem]                VARCHAR (50)     NULL,
    [action]                 VARCHAR (50)     NULL,
    [facility]               VARCHAR (50)     NULL,
    [referencereq]           VARCHAR (50)     NULL,
    [pat_dob]                DATETIME         NULL,
    [chrg_err]               VARCHAR (8000)   NULL,
    [istemp]                 VARCHAR (50)     NULL,
    [age_on_date_of_service] INT              NULL,
    [calc_amt]               NUMERIC (24, 4)  NULL
);
