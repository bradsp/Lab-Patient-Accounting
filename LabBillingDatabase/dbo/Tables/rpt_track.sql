CREATE TABLE [dbo].[rpt_track] (
    [uri]          INT          IDENTITY (1, 1) NOT NULL,
    [mod_date]     DATETIME     CONSTRAINT [DF_rpt_track_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]     VARCHAR (30) CONSTRAINT [DF_rpt_track_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_host]     VARCHAR (30) CONSTRAINT [DF_rpt_track_mod_host] DEFAULT (host_name()) NULL,
    [mod_app]      VARCHAR (30) CONSTRAINT [DF_rpt_track] DEFAULT (app_name()) NULL,
    [form_printed] VARCHAR (10) NULL,
    [cli_nme]      VARCHAR (40) NULL,
    [qty_printed]  INT          NULL,
    [printer_name] VARCHAR (80) NULL,
    CONSTRAINT [PK_rpt_track] PRIMARY KEY NONCLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);

