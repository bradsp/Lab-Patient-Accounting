CREATE TABLE [dbo].[data_EOB_Detail] (
    [rowguid]         UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [deleted]         BIT              CONSTRAINT [DF_data_EOB_Detail_deleted] DEFAULT ((0)) NOT NULL,
    [account]         VARCHAR (15)     NOT NULL,
    [claim_status]    VARCHAR (50)     NOT NULL,
    [ServiceCode]     VARCHAR (50)     NOT NULL,
    [rev_code]        VARCHAR (50)     NULL,
    [units]           INT              NULL,
    [apc_nr]          VARCHAR (50)     NULL,
    [allowed_amt]     MONEY            NULL,
    [stat]            VARCHAR (50)     NULL,
    [wght]            VARCHAR (5)      CONSTRAINT [DF_data_EOB_Detail_wght] DEFAULT ((0.00)) NULL,
    [date_of_service] DATETIME         NULL,
    [charge_amt]      MONEY            NULL,
    [paid_amt]        MONEY            NULL,
    [reason_type]     VARCHAR (5)      NULL,
    [reason_code]     VARCHAR (5)      NULL,
    [adj_amt]         MONEY            NULL,
    [other_adj_amt]   MONEY            NULL,
    [mod_date]        DATETIME         CONSTRAINT [DF_data_EOB_Detail_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]        VARCHAR (50)     CONSTRAINT [DF_data_EOB_Detail_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]         VARCHAR (50)     CONSTRAINT [DF_data_EOB_Detail_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]        VARCHAR (50)     CONSTRAINT [DF_data_EOB_Detail_mod_host] DEFAULT (host_name()) NOT NULL,
    [uid]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [bill_cycle_date] DATETIME         NOT NULL,
    [check_no]        VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_data_EOB_Detail] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_data_EOB_Detail_data_EOB_Detail] FOREIGN KEY ([uid]) REFERENCES [dbo].[data_EOB_Detail] ([uid])
);


GO
CREATE NONCLUSTERED INDEX [IX_EOB_detail_rowguid]
    ON [dbo].[data_EOB_Detail]([rowguid] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'link to data_EOB', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SVC01', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'ServiceCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SVC04', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'rev_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SVC05', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'units';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'04/21/2008 not used', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'apc_nr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SVC03', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'allowed_amt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'04/21/2008 not used', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'stat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'04/21/2008 not used', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'wght';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DTM*232', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'date_of_service';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SVC02', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'charge_amt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SVC03', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'paid_amt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CAS01', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'reason_type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CAS02', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'reason_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CAS03', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'adj_amt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/27/2008 rgc/wdk added for non CO/45''s', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB_Detail', @level2type = N'COLUMN', @level2name = N'other_adj_amt';

