CREATE TABLE [dbo].[dict_acc_validation] (
    [rule_id]    INT            IDENTITY (1, 1) NOT NULL,
    [type_check] VARCHAR (50)   NULL,
    [valid]      BIT            NOT NULL,
    [strSql]     VARCHAR (8000) NOT NULL,
    [error]      VARCHAR (256)  NULL,
    [mod_date]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [mod_prg]    VARCHAR (50)   DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_user]   VARCHAR (50)   DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_host]   VARCHAR (50)   DEFAULT (right(host_name(),(50))) NOT NULL,
    PRIMARY KEY CLUSTERED ([rule_id] ASC)
);

