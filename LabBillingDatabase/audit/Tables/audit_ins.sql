CREATE TABLE [audit].[audit_ins] (
    [ins_rowguid]          UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL,
    [deleted]              BIT              NOT NULL,
    [account]              VARCHAR (15)     NOT NULL,
    [ins_a_b_c]            VARCHAR (1)      NOT NULL,
    [holder_nme]           VARCHAR (40)     NULL,
    [holder_dob]           DATETIME         NULL,
    [plan_nme]             VARCHAR (45)     NULL,
    [plan_addr1]           VARCHAR (40)     NULL,
    [plan_addr2]           VARCHAR (40)     NULL,
    [p_city_st]            VARCHAR (40)     NULL,
    [policy_num]           VARCHAR (50)     NULL,
    [cert_ssn]             VARCHAR (15)     NULL,
    [grp_nme]              VARCHAR (50)     NULL,
    [grp_num]              VARCHAR (15)     NULL,
    [holder_sex]           VARCHAR (1)      NULL,
    [employer]             VARCHAR (25)     NULL,
    [e_city_st]            VARCHAR (35)     NULL,
    [fin_code]             VARCHAR (1)      NULL,
    [ins_code]             VARCHAR (10)     NULL,
    [relation]             VARCHAR (2)      NULL,
    [mod_date]             DATETIME         NOT NULL,
    [mod_user]             VARCHAR (50)     NOT NULL,
    [mod_prg]              VARCHAR (50)     NOT NULL,
    [mod_host]             VARCHAR (50)     NOT NULL,
    [plan_effective_date]  DATETIME         NULL,
    [plan_expiration_date] DATETIME         NULL
);


GO
CREATE CLUSTERED INDEX [IX_audit_ins]
    ON [audit].[audit_ins]([account] ASC, [ins_a_b_c] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_account_mod_date]
    ON [audit].[audit_ins]([account] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_audit_ins_rowguid]
    ON [audit].[audit_ins]([ins_rowguid] ASC) WITH (FILLFACTOR = 90);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'20100217 wdk added for new requirement from Bluecross', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_ins', @level2type = N'COLUMN', @level2name = N'holder_dob';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'20100217 wdk added for new requirement from Bluecross', @level0type = N'SCHEMA', @level0name = N'audit', @level1type = N'TABLE', @level1name = N'audit_ins', @level2type = N'COLUMN', @level2name = N'relation';

