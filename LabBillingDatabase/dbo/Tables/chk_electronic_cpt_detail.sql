CREATE TABLE [dbo].[chk_electronic_cpt_detail] (
    [account]     VARCHAR (15)    NOT NULL,
    [pay_id]      BIGINT          NOT NULL,
    [cpt]         VARCHAR (5)     NOT NULL,
    [cpt_modi]    VARCHAR (5)     NULL,
    [cpt_charges] NUMERIC (18, 2) NOT NULL,
    [cpt_paid]    NUMERIC (18, 2) NOT NULL,
    [cpt_qty]     INT             NOT NULL
);

