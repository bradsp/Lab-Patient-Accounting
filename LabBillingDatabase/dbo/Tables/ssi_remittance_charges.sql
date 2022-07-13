CREATE TABLE [dbo].[ssi_remittance_charges] (
    [file_date]    DATETIME     NOT NULL,
    [icn]          VARCHAR (15) NOT NULL,
    [chrg_line]    INT          NOT NULL,
    [cpt_code]     VARCHAR (5)  NULL,
    [rev_code]     VARCHAR (3)  NULL,
    [reported_amt] MONEY        NULL,
    [allowed_amt]  MONEY        CONSTRAINT [DF_ssi_remittance_charges_allowed_amt] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ssi_remittance_charges] PRIMARY KEY CLUSTERED ([file_date] ASC, [icn] ASC, [chrg_line] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_icn INCLUDE cpt_code, rev_code, reported_amt, allowed_amt]
    ON [dbo].[ssi_remittance_charges]([icn] ASC)
    INCLUDE([cpt_code], [rev_code], [reported_amt], [allowed_amt]) WITH (FILLFACTOR = 90);

