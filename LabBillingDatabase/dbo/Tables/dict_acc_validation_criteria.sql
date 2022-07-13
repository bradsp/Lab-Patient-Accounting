CREATE TABLE [dbo].[dict_acc_validation_criteria] (
    [rule_id]        INT          NOT NULL,
    [fin_code]       VARCHAR (10) NULL,
    [ins_code]       VARCHAR (50) NULL,
    [bill_form]      VARCHAR (50) NULL,
    [effective_date] DATETIME     NOT NULL,
    [expire_date]    DATETIME     NULL,
    [uid]            INT          IDENTITY (1, 1) NOT NULL,
    [mod_date]       DATETIME     DEFAULT (getdate()) NOT NULL,
    [mod_prg]        VARCHAR (50) DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]       VARCHAR (50) DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]       VARCHAR (50) DEFAULT (right(host_name(),(50))) NOT NULL,
    PRIMARY KEY CLUSTERED ([uid] ASC)
);


GO
CREATE NONCLUSTERED INDEX [rule_id-20220627-161011]
    ON [dbo].[dict_acc_validation_criteria]([rule_id] ASC);

