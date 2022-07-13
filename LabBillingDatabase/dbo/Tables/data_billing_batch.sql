CREATE TABLE [dbo].[data_billing_batch] (
    [batch]       NUMERIC (10)     NOT NULL,
    [run_date]    DATETIME         NOT NULL,
    [run_user]    NVARCHAR (100)   DEFAULT (suser_sname()) NOT NULL,
    [x12_text]    NVARCHAR (MAX)   NULL,
    [claim_count] INT              NULL,
    [mod_date]    DATETIME         DEFAULT (getdate()) NOT NULL,
    [mod_user]    NVARCHAR (100)   DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]     NVARCHAR (100)   DEFAULT (app_name()) NOT NULL,
    [mod_host]    NVARCHAR (100)   DEFAULT (host_name()) NOT NULL,
    [rowguid]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_batch] PRIMARY KEY CLUSTERED ([batch] ASC)
);

