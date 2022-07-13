CREATE TABLE [dbo].[dict_ncd] (
    [ncd_id]          NVARCHAR (25)  NOT NULL,
    [F2]              NVARCHAR (255) NULL,
    [cpt]             NVARCHAR (15)  NULL,
    [cpt_eff_date]    DATETIME       NULL,
    [cpt_term_date]   DATETIME       NULL,
    [icd10]           NVARCHAR (25)  NOT NULL,
    [icd10_eff_date]  DATETIME       NULL,
    [icd10_term_date] DATETIME       NULL,
    [resolution_code] NVARCHAR (1)   NULL,
    CONSTRAINT [PK_dict_ncd] PRIMARY KEY CLUSTERED ([ncd_id] ASC, [icd10] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_ncd_cpt]
    ON [dbo].[dict_ncd]([ncd_id] ASC, [cpt] ASC) WITH (FILLFACTOR = 90);

