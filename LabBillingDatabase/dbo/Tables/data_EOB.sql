CREATE TABLE [dbo].[data_EOB] (
    [rowguid]                UNIQUEIDENTIFIER NOT NULL,
    [deleted]                BIT              CONSTRAINT [DF_data_EOB_deleted] DEFAULT ((0)) NOT NULL,
    [payor]                  VARCHAR (50)     NULL,
    [account]                VARCHAR (15)     NOT NULL,
    [subscriberID]           VARCHAR (50)     NULL,
    [subscriberName]         VARCHAR (150)    NULL,
    [date_of_service]        DATETIME         NOT NULL,
    [ICN]                    VARCHAR (50)     NULL,
    [PatStat]                VARCHAR (50)     CONSTRAINT [DF_data_EOB_PatStat] DEFAULT ('0') NULL,
    [claim_status]           VARCHAR (50)     NULL,
    [claim_type]             VARCHAR (50)     NULL,
    [charges_reported]       MONEY            NULL,
    [charges_noncovered]     MONEY            NULL,
    [charges_denied]         MONEY            NULL,
    [pat_lib_coinsurance]    MONEY            NULL,
    [pat_lib_noncovered]     MONEY            NULL,
    [pay_data_reimb_rate]    VARCHAR (5)      NULL,
    [pay_data_msp_prim_pay]  MONEY            NULL,
    [pay_data_hcpcs_amt]     MONEY            NULL,
    [pay_data_cont_adj_amt]  MONEY            NULL,
    [pay_data_pat_refund]    MONEY            NULL,
    [pay_data_per_diem_rate] VARCHAR (5)      NULL,
    [pay_data_net_reimb_amt] MONEY            NULL,
    [claim_forwarded_to]     VARCHAR (100)    NULL,
    [claim_forwarded_id]     VARCHAR (50)     NULL,
    [eft_file]               VARCHAR (255)    NOT NULL,
    [eft_number]             VARCHAR (50)     NOT NULL,
    [eft_date]               DATETIME         NOT NULL,
    [eob_print_date]         DATETIME         NULL,
    [mod_date]               DATETIME         CONSTRAINT [DF_data_EOB_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]                VARCHAR (50)     CONSTRAINT [DF_data_EOB_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_user]               VARCHAR (50)     CONSTRAINT [DF_data_EOB_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_host]               VARCHAR (50)     CONSTRAINT [DF_data_EOB_mod_host] DEFAULT (host_name()) NOT NULL,
    [bill_cycle_date]        DATETIME         NOT NULL,
    [check_no]               VARCHAR (50)     NOT NULL,
    [provider_id]            VARCHAR (50)     NULL,
    [uid]                    BIGINT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_data_EOB] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_EOB_rowguid]
    ON [dbo].[data_EOB]([rowguid] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'size the same as acc.account', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'account';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HIC from 835''s NM109', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'subscriberID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'not used as of 04/21/2008', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'PatStat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'for Medicare this is 141 (CLP 8 and 9 together)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'claim_type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'clp 02', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'charges_reported';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reason codes CO/96', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'charges_noncovered';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'04/21/2008 have not seen yet', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'charges_denied';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reason codes PR/2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'pat_lib_coinsurance';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reason Code PR/45', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'pat_lib_noncovered';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Percent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'pay_data_reimb_rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'04/24/2008 wdk should be total of OA/23s', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'pay_data_msp_prim_pay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Total of paids from detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'pay_data_hcpcs_amt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'total of details AdjAmt for CO/45''s', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'pay_data_cont_adj_amt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'04/22/2008 rgc/wdk CAS*PC ???? (Patient Credit)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'pay_data_pat_refund';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'percent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'pay_data_per_diem_rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'file name without path', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'eft_file';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'05/22/2008 rgc/wdk added to identify the payor. Each payer gives us different provider id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_EOB', @level2type = N'COLUMN', @level2name = N'provider_id';

