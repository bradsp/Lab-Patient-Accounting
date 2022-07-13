CREATE TABLE [dbo].[acc_alert] (
    [account]  VARCHAR (15) NOT NULL,
    [alert]    BIT          NOT NULL,
    [mod_date] DATETIME     CONSTRAINT [DF_acc_alert_mod_date] DEFAULT (getdate()) NULL,
    [mod_user] VARCHAR (30) CONSTRAINT [DF_acc_alert_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_prg]  VARCHAR (30) CONSTRAINT [DF_acc_alert_mod_prg] DEFAULT (app_name()) NULL,
    [mod_host] VARCHAR (30) CONSTRAINT [DF_acc_alert_mod_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_acc_alert] PRIMARY KEY CLUSTERED ([account] ASC) WITH (FILLFACTOR = 90)
);

