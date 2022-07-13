CREATE TABLE [dbo].[data_HL7_Msg] (
    [rowguid]              UNIQUEIDENTIFIER CONSTRAINT [DF_data_HL7_Msg_rowguid] DEFAULT (newid()) NOT NULL,
    [deleted]              BIT              CONSTRAINT [DF_data_HL7_Msg_deleted] DEFAULT ((0)) NOT NULL,
    [cli_mnem]             VARCHAR (15)     NULL,
    [cli_printed]          BIT              NULL,
    [msg_type]             VARCHAR (50)     NULL,
    [msg]                  VARCHAR (MAX)    NOT NULL,
    [msg_status]           VARCHAR (50)     NULL,
    [msg_status_reason]    VARCHAR (256)    CONSTRAINT [DF_data_HL7_Msg_msg_status_reason] DEFAULT ('') NULL,
    [continuation_rowguid] UNIQUEIDENTIFIER NOT NULL,
    [service_tx_date]      DATETIME         NULL,
    [service_rv_date]      DATETIME         CONSTRAINT [DF_data_HL7_Msg_service_rv_date] DEFAULT (getdate()) NULL,
    [msg_control_ID]       VARCHAR (50)     CONSTRAINT [DF_data_HL7_Msg_msg_control_ID] DEFAULT ('') NULL,
    [ov_order_id]          VARCHAR (50)     NULL,
    [ov_pat_id]            VARCHAR (50)     NULL,
    [msg_sequence_nr]      VARCHAR (50)     NULL,
    [resulted_datetime]    DATETIME         NULL,
    [wreq_rowguid]         UNIQUEIDENTIFIER NULL,
    [mod_date]             DATETIME         CONSTRAINT [DF_data_HL7_Msg_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_user]             VARCHAR (50)     CONSTRAINT [DF_data_HL7_Msg_mod_user] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]              VARCHAR (50)     CONSTRAINT [DF_data_HL7_Msg_mod_prg] DEFAULT (app_name()) NOT NULL,
    [mod_host]             VARCHAR (50)     CONSTRAINT [DF_data_HL7_Msg_mod_host] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_data_HL7_Msg] PRIMARY KEY CLUSTERED ([rowguid] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20091029 added for EHS upgrades before being pushed live.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'cli_mnem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20091029 added for EHS upgrades before being pushed live.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'cli_printed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20090820 changed size from 8000 to MAX', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'msg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NEW/SENT/REJECTED/PROCESSED/FAILED', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'msg_status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'failed reason or other comment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'msg_status_reason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rowguid to tie subsequent records to the original MSH', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'continuation_rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'service transfer date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'service_tx_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'service receive date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'service_rv_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identifys the message back to the sender.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'msg_control_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20090622 added for Result tracking', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'ov_order_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20090622 added  for Result tracking', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'ov_pat_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Meditech will often leave this blank, HC does not leave blank', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'msg_sequence_nr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'08/19/2008 wdk/rgc added for transferring results to WResults and WResultsdata', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'data_HL7_Msg', @level2type = N'COLUMN', @level2name = N'resulted_datetime';

