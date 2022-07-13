CREATE TABLE [dbo].[chk_tests_cnt_cnb] (
    [specimen_tot]   VARCHAR (50) NULL,
    [specimen_hit]   VARCHAR (50) NULL,
    [rundatetime]    DATETIME     NULL,
    [cmp_rp_hit]     VARCHAR (50) NULL,
    [cmp_hfp_hit]    VARCHAR (50) NULL,
    [chkdatetime]    DATETIME     NULL,
    [runondatetime]  DATETIME     NULL,
    [missed_spc_hit] VARCHAR (50) NULL
);


GO
CREATE CLUSTERED INDEX [IX_chk_tests_cnt_cnb]
    ON [dbo].[chk_tests_cnt_cnb]([rundatetime] ASC) WITH (FILLFACTOR = 90);

