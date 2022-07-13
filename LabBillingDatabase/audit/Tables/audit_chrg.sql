CREATE TABLE [audit].[audit_chrg] (
    [chrg_rowguid]    UNIQUEIDENTIFIER NULL,
    [credited]        BIT              NULL,
    [chrg_num]        NUMERIC (15)     NULL,
    [account]         VARCHAR (15)     NULL,
    [status]          VARCHAR (15)     NULL,
    [service_date]    DATETIME         NULL,
    [hist_date]       DATETIME         NULL,
    [cdm]             VARCHAR (7)      NULL,
    [qty]             NUMERIC (4)      NULL,
    [retail]          MONEY            NULL,
    [inp_price]       MONEY            NULL,
    [comment]         VARCHAR (50)     NULL,
    [invoice]         VARCHAR (15)     NULL,
    [mod_date]        DATETIME         NULL,
    [mod_user]        VARCHAR (50)     NULL,
    [mod_prg]         VARCHAR (50)     NULL,
    [net_amt]         MONEY            NULL,
    [fin_type]        VARCHAR (1)      NULL,
    [mod_host]        VARCHAR (50)     NULL,
    [uid]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [post_date]       DATETIME         NULL,
    [fin_code]        VARCHAR (10)     NULL,
    [performing_site] VARCHAR (50)     NULL,
    CONSTRAINT [PK_audit_chrg_uid] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_audit_chrg]
    ON [audit].[audit_chrg]([chrg_num] ASC, [uid] DESC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [idxAcc]
    ON [audit].[audit_chrg]([account] ASC, [chrg_num] DESC, [mod_date] DESC) WITH (FILLFACTOR = 90);

