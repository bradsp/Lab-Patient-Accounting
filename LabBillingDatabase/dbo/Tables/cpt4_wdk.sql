CREATE TABLE [dbo].[cpt4_wdk] (
    [rowguid]  UNIQUEIDENTIFIER CONSTRAINT [DF_cpt4_wdk_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]  BIT              CONSTRAINT [DF_cpt4_wdk_deleted_3__23] DEFAULT ((0)) NOT NULL,
    [cdm]      VARCHAR (7)      NOT NULL,
    [link]     INT              NOT NULL,
    [cpt4]     VARCHAR (5)      NULL,
    [descript] VARCHAR (50)     NULL,
    [mprice]   MONEY            CONSTRAINT [DF_cpt4_wdk_mprice_8__23] DEFAULT ((0)) NULL,
    [cprice]   MONEY            CONSTRAINT [DF_cpt4_wdk_cprice_2__23] DEFAULT ((0)) NULL,
    [zprice]   MONEY            CONSTRAINT [DF_cpt4_wdk_zprice_9__23] DEFAULT ((0)) NULL,
    [rev_code] VARCHAR (4)      NULL,
    [type]     VARCHAR (4)      NULL,
    [modi]     VARCHAR (2)      NULL,
    [billcode] VARCHAR (7)      NULL,
    [mod_date] DATETIME         CONSTRAINT [DF_cpt4_wdk_mod_date_4__23] DEFAULT (getdate()) NULL,
    [mod_user] VARCHAR (50)     CONSTRAINT [DF_cpt4_wdk_mod_user_7__23] DEFAULT (suser_sname()) NULL,
    [mod_prg]  VARCHAR (50)     CONSTRAINT [DF_cpt4_wdk_mod_prg_6__23] DEFAULT (app_name()) NULL,
    [mod_host] VARCHAR (50)     CONSTRAINT [DF_cpt4_wdk_mod_host_5__23] DEFAULT (host_name()) NULL,
    [cost]     DECIMAL (18, 2)  NULL,
    CONSTRAINT [PK_cpt4_wdk_1__23] PRIMARY KEY CLUSTERED ([cdm] ASC, [link] ASC) WITH (FILLFACTOR = 90)
);

