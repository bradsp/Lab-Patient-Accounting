CREATE TABLE [infce].[messages_inbound] (
    [account_cerner]      VARCHAR (50)  NULL,
    [sourceMsgId]         NUMERIC (18)  NULL,
    [sourceInfce]         VARCHAR (50)  NULL,
    [msgType]             VARCHAR (20)  NULL,
    [msgDate]             DATETIME      NULL,
    [msgContent]          VARCHAR (MAX) NULL,
    [processFlag]         VARCHAR (5)   CONSTRAINT [DF_LIVE_messages___proce__5931EE27] DEFAULT ('N') NULL,
    [processStatusMsg]    VARCHAR (250) NULL,
    [systemMsgId]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [dx_processed]        BIT           NULL,
    [mod_date]            DATETIME      DEFAULT (getdate()) NULL,
    [order_pat_id]        VARCHAR (50)  NULL,
    [order_visit_id]      VARCHAR (50)  NULL,
    [DOS]                 DATETIME      NULL,
    [dx_processed_method] VARCHAR (50)  NULL,
    [ins_fin_code]        VARCHAR (50)  NULL,
    [HL7Message]          VARCHAR (MAX) NULL,
    CONSTRAINT [PK_messages_inbound] PRIMARY KEY CLUSTERED ([systemMsgId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_PROC]
    ON [infce].[messages_inbound]([msgType] ASC, [processFlag] ASC, [processStatusMsg] ASC, [systemMsgId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_account_cerner]
    ON [infce].[messages_inbound]([account_cerner] ASC, [DOS] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_sourceMsgId]
    ON [infce].[messages_inbound]([sourceMsgId] ASC, [sourceInfce] ASC, [msgType] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_PROC_DFT]
    ON [infce].[messages_inbound]([msgType] ASC, [processFlag] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_msgtype_date]
    ON [infce].[messages_inbound]([msgType] ASC, [msgDate] ASC, [processFlag] ASC)
    INCLUDE([account_cerner], [order_pat_id], [order_visit_id]) WITH (FILLFACTOR = 90);


GO
CREATE TRIGGER [infce].trigger_insert ON infce.messages_inbound FOR INSERT
AS
BEGIN

	INSERT INTO tblPropAcc 
	 SELECT p.*
	 FROM INSERTED AS I
	CROSS APPLY dbo.udf_XML2Table(I.systemMsgId
	, CAST(REPLACE(I.msgContent,'"','') AS XML )) AS p
	WHERE LEFT(I.msgType,3) = 'DFT'

END
GO
DISABLE TRIGGER [infce].[trigger_insert]
    ON [infce].[messages_inbound];

