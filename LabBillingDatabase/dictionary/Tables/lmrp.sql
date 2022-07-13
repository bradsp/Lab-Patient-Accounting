CREATE TABLE [dictionary].[lmrp] (
    [cpt4]            VARCHAR (5)  NOT NULL,
    [beg_icd9]        VARCHAR (7)  NOT NULL,
    [end_icd9]        VARCHAR (7)  NOT NULL,
    [payor]           VARCHAR (30) CONSTRAINT [DF_lmrp_payor] DEFAULT ('MEDICARE') NULL,
    [fincode]         VARCHAR (10) CONSTRAINT [DF_lmrp_fincode] DEFAULT ('A') NULL,
    [mod_user]        VARCHAR (20) CONSTRAINT [DF_lmrp_mod_user] DEFAULT (right(suser_sname(),(20))) NULL,
    [mod_date]        DATETIME     CONSTRAINT [DF_lmrp_mod_date] DEFAULT (getdate()) NULL,
    [mod_prg]         VARCHAR (20) CONSTRAINT [DF_lmrp_mod_prg] DEFAULT (right(app_name(),(20))) NULL,
    [rb_date]         DATETIME     NULL,
    [lmrp]            VARCHAR (25) NULL,
    [lmrp2]           VARCHAR (25) NULL,
    [rb_date2]        DATETIME     NULL,
    [chk_for_bad]     INT          NULL,
    [ama_year]        VARCHAR (6)  NULL,
    [uid]             DECIMAL (18) IDENTITY (1, 1) NOT NULL,
    [expiration_date] DATETIME     NULL,
    CONSTRAINT [PK_lmrp] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_lmrp_all]
    ON [dictionary].[lmrp]([ama_year] ASC, [cpt4] ASC, [beg_icd9] ASC, [end_icd9] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_cpt4, ama_year, beg_icd9, end_icd9, rb_date INCLUDE payor, fincode, mod_user, mod_prog, plus more]
    ON [dictionary].[lmrp]([cpt4] ASC, [ama_year] ASC, [beg_icd9] ASC, [end_icd9] ASC, [rb_date] ASC)
    INCLUDE([payor], [fincode], [mod_user], [mod_date], [mod_prg], [lmrp], [lmrp2], [rb_date2], [chk_for_bad], [uid], [expiration_date]) WITH (FILLFACTOR = 90);

