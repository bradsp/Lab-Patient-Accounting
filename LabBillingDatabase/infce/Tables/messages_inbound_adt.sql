﻿CREATE TABLE [infce].[messages_inbound_adt] (
    [account_cerner]      VARCHAR (50)  NULL,
    [sourceMsgId]         NUMERIC (18)  NULL,
    [sourceInfce]         VARCHAR (50)  NULL,
    [msgType]             VARCHAR (20)  NULL,
    [msgDate]             DATETIME      NULL,
    [msgContent]          VARCHAR (MAX) NULL,
    [processFlag]         VARCHAR (5)   CONSTRAINT [DF_LIVE_messages_adt___proce__5931EE27] DEFAULT ('N') NULL,
    [processStatusMsg]    VARCHAR (250) NULL,
    [systemMsgId]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [dx_processed]        BIT           NULL,
    [mod_date]            DATETIME      DEFAULT (getdate()) NULL,
    [order_pat_id]        VARCHAR (50)  NULL,
    [order_visit_id]      VARCHAR (50)  NULL,
    [DOS]                 DATETIME      NULL,
    [dx_processed_method] VARCHAR (50)  NULL,
    CONSTRAINT [PK_messages_inbound_adt] PRIMARY KEY CLUSTERED ([systemMsgId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_account_cerner]
    ON [infce].[messages_inbound_adt]([account_cerner] ASC, [msgType] ASC, [msgDate] ASC) WITH (FILLFACTOR = 90);
