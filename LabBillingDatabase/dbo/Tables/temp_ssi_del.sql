CREATE TABLE [dbo].[temp_ssi_del] (
    [remit_date]    VARCHAR (10)  NULL,
    [lname]         VARCHAR (30)  NULL,
    [fname]         VARCHAR (30)  NULL,
    [init]          VARCHAR (5)   NULL,
    [covers_beg]    VARCHAR (10)  NULL,
    [account]       VARCHAR (15)  NULL,
    [balance]       VARCHAR (9)   NULL,
    [abn]           VARCHAR (1)   NULL,
    [co_ins]        VARCHAR (9)   NULL,
    [payment]       VARCHAR (9)   NULL,
    [contractual]   VARCHAR (9)   NULL,
    [covered_chrgs] VARCHAR (9)   NULL,
    [non_cov]       VARCHAR (9)   NULL,
    [third_party]   VARCHAR (9)   NULL,
    [deductable]    VARCHAR (9)   NULL,
    [type]          VARCHAR (20)  NULL,
    [batch]         NUMERIC (18)  NULL,
    [err_text]      VARCHAR (100) NULL,
    [uri]           NUMERIC (10)  IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK___1__24] PRIMARY KEY NONCLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE CLUSTERED INDEX [name_cdx]
    ON [dbo].[temp_ssi_del]([lname] ASC, [fname] ASC, [init] ASC) WITH (FILLFACTOR = 90);

