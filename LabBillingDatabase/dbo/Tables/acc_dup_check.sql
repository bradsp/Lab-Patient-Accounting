CREATE TABLE [dbo].[acc_dup_check] (
    [master_account]   VARCHAR (15)  NULL,
    [account]          VARCHAR (15)  NULL,
    [client]           VARCHAR (256) NULL,
    [service_date]     DATETIME      NULL,
    [fin_code]         VARCHAR (50)  NULL,
    [pat_name]         VARCHAR (100) NULL,
    [pat_ssn]          VARCHAR (13)  NULL,
    [unitno]           VARCHAR (50)  NULL,
    [pat_dob]          DATETIME      NULL,
    [is_duplicate_acc] BIT           NULL,
    [mod_date]         DATETIME      CONSTRAINT [DF_acc_dup_check_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]         VARCHAR (50)  CONSTRAINT [DF_acc_dup_check_mod_user] DEFAULT (right(suser_sname(),(50))) NULL,
    [mod_prg]          VARCHAR (50)  CONSTRAINT [DF_acc_dup_check_mod_prg] DEFAULT (right(app_name(),(50))) NULL,
    [mod_host]         VARCHAR (50)  CONSTRAINT [DF_acc_dup_check_mod_host] DEFAULT (right(host_name(),(50))) NULL,
    [uid]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_acc_dup_check] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);

