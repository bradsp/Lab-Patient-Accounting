CREATE TABLE [dbo].[emp_removed] (
    [name]                   VARCHAR (20) NOT NULL,
    [access]                 VARCHAR (20) NULL,
    [mainmenu]               VARCHAR (15) NULL,
    [access_edit_dictionary] BIT          NOT NULL,
    [access_bad_debt]        BIT          NOT NULL,
    [access_billing]         BIT          NOT NULL,
    [access_fin_code]        BIT          NOT NULL,
    [add_chrg]               BIT          NOT NULL,
    [add_chk]                BIT          NOT NULL,
    [add_chk_amt]            BIT          NOT NULL,
    [reserve4]               BIT          NOT NULL,
    [reserve5]               BIT          NOT NULL,
    [reserve6]               BIT          NOT NULL,
    [mod_user]               VARCHAR (50) NULL,
    [mod_prg]                VARCHAR (50) NULL,
    [mod_date]               DATETIME     NULL,
    [full_name]              VARCHAR (35) NULL
);

