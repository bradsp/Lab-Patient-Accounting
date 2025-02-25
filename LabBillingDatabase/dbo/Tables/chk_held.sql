﻿CREATE TABLE [dbo].[chk_held] (
    [rowguid]              UNIQUEIDENTIFIER NOT NULL,
    [deleted]              BIT              NOT NULL,
    [account]              VARCHAR (15)     NOT NULL,
    [chk_date]             DATETIME         NULL,
    [date_rec]             DATETIME         NULL,
    [chk_no]               VARCHAR (25)     NULL,
    [amt_paid]             MONEY            NULL,
    [write_off]            MONEY            NULL,
    [contractual]          MONEY            NULL,
    [status]               VARCHAR (15)     NULL,
    [source]               VARCHAR (50)     NULL,
    [fin_code]             VARCHAR (10)     NULL,
    [w_off_date]           DATETIME         NULL,
    [invoice]              VARCHAR (15)     NULL,
    [batch]                NUMERIC (15)     NULL,
    [comment]              VARCHAR (50)     NULL,
    [bad_debt]             BIT              NOT NULL,
    [mod_date]             DATETIME         NOT NULL,
    [mod_user]             VARCHAR (50)     NOT NULL,
    [mod_prg]              VARCHAR (50)     NOT NULL,
    [mod_host]             VARCHAR (50)     NOT NULL,
    [mod_date_audit]       DATETIME         NOT NULL,
    [cpt4Code]             VARCHAR (50)     NULL,
    [post_file]            VARCHAR (256)    NULL,
    [chrg_rowguid]         UNIQUEIDENTIFIER NULL,
    [write_off_code]       VARCHAR (4)      NULL,
    [eft_date]             DATETIME         NULL,
    [eft_number]           VARCHAR (50)     NULL,
    [post_date]            DATETIME         NULL,
    [ins_code]             VARCHAR (10)     NULL,
    [claim_adj_code]       VARCHAR (50)     NULL,
    [claim_adj_group_code] VARCHAR (50)     NULL,
    [facility_code]        VARCHAR (50)     NULL,
    [claim_no]             VARCHAR (50)     NULL
);

