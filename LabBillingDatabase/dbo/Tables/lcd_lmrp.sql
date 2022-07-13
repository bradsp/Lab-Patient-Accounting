CREATE TABLE [dbo].[lcd_lmrp] (
    [lcd_cpt4]            VARCHAR (5) NOT NULL,
    [lcd_beg_icd9]        VARCHAR (7) NOT NULL,
    [lcd_end_icd9]        VARCHAR (7) NOT NULL,
    [lcd_ama_year]        VARCHAR (6) NOT NULL,
    [lcd_chk_for_bad]     INT         NULL,
    [lcd_expiration_date] DATETIME    NULL
);

