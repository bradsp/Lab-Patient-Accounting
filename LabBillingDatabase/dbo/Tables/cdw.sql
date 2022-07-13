CREATE TABLE [dbo].[cdw] (
    [deleted]       BIT          CONSTRAINT [DF_cdw_deleted_2__12] DEFAULT ((0)) NOT NULL,
    [cdm]           VARCHAR (7)  NOT NULL,
    [hosp_mnem]     VARCHAR (10) NOT NULL,
    [descript]      VARCHAR (35) NULL,
    [type]          VARCHAR (10) NULL,
    [price]         MONEY        NULL,
    [retail]        MONEY        NULL,
    [inp_price]     MONEY        NULL,
    [mod_date]      DATETIME     CONSTRAINT [DF_cdw_mod_date_3__12] DEFAULT (getdate()) NULL,
    [mod_user]      VARCHAR (50) CONSTRAINT [DF_cdw_mod_user_5__12] DEFAULT (suser_sname()) NULL,
    [mod_prg]       VARCHAR (50) CONSTRAINT [DF_cdw_mod_prg_4__12] DEFAULT (app_name()) NULL,
    [meditech_mnem] VARCHAR (15) NULL,
    [pa_amount]     MONEY        NULL,
    [mod_host]      VARCHAR (50) CONSTRAINT [DF_cdw_mod_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_cdw_1__12] PRIMARY KEY CLUSTERED ([cdm] ASC, [hosp_mnem] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_cdw_hosp_mnem]
    ON [dbo].[cdw]([hosp_mnem] ASC, [meditech_mnem] ASC) WITH (FILLFACTOR = 90);


GO
/****** Object:  Trigger dbo.tu_cdw    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_cdw] ON dbo.cdw 
FOR UPDATE 
AS
UPDATE cdw
SET cdw.mod_user = suser_sname(), cdw.mod_date = getdate(), cdw.mod_prg = app_name()
FROM inserted,cdw
WHERE inserted.cdm = cdw.cdm

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'12/16/2008 wdk changed to varchar(50) vice 30 to prevent trigger from failing.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cdw', @level2type = N'COLUMN', @level2name = N'mod_user';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'12/16/2008 wdk changed to varchar(50) vice 30 to prevent trigger from failing.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cdw', @level2type = N'COLUMN', @level2name = N'mod_prg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'12/16/2008 wdk Added for tracking.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cdw', @level2type = N'COLUMN', @level2name = N'mod_host';

