CREATE TABLE [dbo].[chk_electronic_cpt_adjustment_codes] (
    [pay_id]                 BIGINT          NOT NULL,
    [account]                VARCHAR (15)    NOT NULL,
    [cpt]                    VARCHAR (5)     NOT NULL,
    [cpt_modi]               VARCHAR (5)     NULL,
    [adjustment_group_code]  VARCHAR (2)     NOT NULL,
    [adjustment_reason_code] VARCHAR (5)     NOT NULL,
    [adjustment_amount]      NUMERIC (18, 2) NOT NULL,
    [adjustment_qty]         INT             NOT NULL,
    [uid]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_chk_electronic_cpt_adjustment_codes] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);

