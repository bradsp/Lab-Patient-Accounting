CREATE TABLE [tst].[data_patBill] (
    [account]                   VARCHAR (15) NOT NULL,
    [pat_name]                  VARCHAR (40) NULL,
    [trans_date]                DATETIME     NULL,
    [fin_code]                  VARCHAR (10) NULL,
    [cl_mnem]                   VARCHAR (10) NULL,
    [status]                    VARCHAR (8)  NULL,
    [dbill_date]                DATETIME     NULL,
    [ub_date]                   DATETIME     NULL,
    [h1500_date]                DATETIME     NULL,
    [batch_date]                DATETIME     NULL,
    [ebill_batch_date]          DATETIME     NULL,
    [mailer]                    VARCHAR (1)  NULL,
    [h1500]                     VARCHAR (1)  NULL,
    [ub92]                      VARCHAR (1)  NULL,
    [phy_id]                    VARCHAR (15) NULL,
    [ebill_batch_1500]          DATETIME     NULL,
    [last_dm]                   DATETIME     NULL,
    [bd_list_date]              DATETIME     NULL,
    [claimsnet_1500_batch_date] DATETIME     NULL
);

