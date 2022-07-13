CREATE TABLE [dbo].[emp] (
    [name]                   VARCHAR (20)  NOT NULL,
    [access]                 VARCHAR (20)  CONSTRAINT [DF_emp_access_2__12] DEFAULT ('NONE') NULL,
    [mainmenu]               VARCHAR (15)  NULL,
    [access_edit_dictionary] BIT           CONSTRAINT [DF_emp_access_edit_dictio5__12] DEFAULT ((0)) NOT NULL,
    [access_bad_debt]        BIT           CONSTRAINT [DF_emp_access_bad_debt_3__12] DEFAULT ((0)) NOT NULL,
    [access_billing]         BIT           CONSTRAINT [DF_emp_access_billing_4__12] DEFAULT ((0)) NOT NULL,
    [access_fin_code]        BIT           CONSTRAINT [DF_emp_access_fin_code_6__12] DEFAULT ((0)) NOT NULL,
    [add_chrg]               BIT           CONSTRAINT [DF_emp_reserve1_10__12] DEFAULT ((0)) NOT NULL,
    [add_chk]                BIT           CONSTRAINT [DF_emp_reserve2_11__12] DEFAULT ((0)) NOT NULL,
    [add_chk_amt]            BIT           CONSTRAINT [DF_emp_reserve3_12__12] DEFAULT ((0)) NOT NULL,
    [reserve4]               BIT           CONSTRAINT [DF_emp_reserve4_13__12] DEFAULT ((0)) NOT NULL,
    [reserve5]               BIT           CONSTRAINT [DF_emp_reserve5_14__12] DEFAULT ((0)) NOT NULL,
    [reserve6]               BIT           CONSTRAINT [DF_emp_reserve6_15__12] DEFAULT ((0)) NOT NULL,
    [mod_user]               VARCHAR (50)  CONSTRAINT [DF_emp_mod_user_9__12] DEFAULT (suser_sname()) NULL,
    [mod_prg]                VARCHAR (50)  CONSTRAINT [DF_emp_mod_prg_8__12] DEFAULT (app_name()) NULL,
    [mod_date]               DATETIME      CONSTRAINT [DF_emp_mod_date_7__12] DEFAULT (getdate()) NULL,
    [full_name]              VARCHAR (35)  NULL,
    [password]               VARCHAR (100) NULL,
    [menu_profile_id]        INT           NULL,
    [impersonate]            BIT           CONSTRAINT [DF_emp_impersonate] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_emp_1__12] PRIMARY KEY CLUSTERED ([name] ASC) WITH (FILLFACTOR = 90)
);


GO
/****** Object:  Trigger dbo.tu_emp    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_emp] ON [dbo].[emp] 
FOR UPDATE
AS
UPDATE emp
SET emp.mod_user = suser_sname(), emp.mod_date = getdate(), emp.mod_prg = app_name()
FROM inserted,emp
WHERE inserted.name = emp.name
