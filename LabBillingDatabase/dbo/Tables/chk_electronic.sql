CREATE TABLE [dbo].[chk_electronic] (
    [account]             VARCHAR (15)    NOT NULL,
    [pay_id]              BIGINT          NOT NULL,
    [claim_status_code]   VARCHAR (2)     NOT NULL,
    [claim_facility_code] VARCHAR (2)     NOT NULL,
    [clp_chrg]            NUMERIC (18, 2) NOT NULL,
    [clp_paid]            NUMERIC (18, 2) NOT NULL,
    [clp_pat_resp]        NUMERIC (18, 2) NOT NULL,
    [clp_date]            DATETIME        NOT NULL,
    [pay_date]            DATETIME        NOT NULL,
    [pay_number]          VARCHAR (50)    NOT NULL,
    [payor_id]            VARCHAR (50)    NOT NULL,
    [payor_name]          VARCHAR (50)    NOT NULL,
    [mod_date]            DATETIME        CONSTRAINT [DF_chk_electronic_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]             VARCHAR (50)    CONSTRAINT [DF_chk_electronic_mod_prg] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]            VARCHAR (50)    CONSTRAINT [DF_chk_electronic_mod_user] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]            VARCHAR (50)    CONSTRAINT [DF_chk_electronic_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [uid]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_chk_electronic] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);

