﻿CREATE TABLE [dbo].[data_h1500_to_SSI] (
    [rowguid]            UNIQUEIDENTIFIER NULL,
    [deleted]            BIT              NOT NULL,
    [account]            VARCHAR (15)     NOT NULL,
    [ins_abc]            VARCHAR (1)      NOT NULL,
    [pat_name]           VARCHAR (40)     NULL,
    [fin_code]           VARCHAR (1)      NULL,
    [claimsnet_payer_id] VARCHAR (50)     NULL,
    [trans_date]         DATETIME         NULL,
    [run_date]           DATETIME         NULL,
    [printed]            BIT              NOT NULL,
    [run_user]           VARCHAR (20)     NOT NULL,
    [batch]              NUMERIC (10)     NOT NULL,
    [date_sent]          DATETIME         NULL,
    [sent_user]          VARCHAR (20)     NULL,
    [ebill_status]       VARCHAR (5)      NULL,
    [ebill_batch]        NUMERIC (10)     NULL,
    [text]               VARCHAR (MAX)    NULL,
    [cold_feed]          DATETIME         NULL,
    [mod_date]           DATETIME         NOT NULL,
    [mod_user]           VARCHAR (50)     NOT NULL,
    [mod_prg]            VARCHAR (50)     NOT NULL,
    [mod_host]           VARCHAR (50)     NOT NULL
);


GO
CREATE CLUSTERED INDEX [IX_data_h1500_to_SSI]
    ON [dbo].[data_h1500_to_SSI]([account] ASC, [ins_abc] ASC) WITH (FILLFACTOR = 90);
