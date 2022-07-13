CREATE TABLE [dbo].[bad_debt] (
    [rowguid]           UNIQUEIDENTIFIER CONSTRAINT [DF_bad_debt_rowguid_1] DEFAULT (newid()) NOT NULL,
    [deleted]           BIT              CONSTRAINT [DF_bad_debt_deleted] DEFAULT ((0)) NOT NULL,
    [debtor_last_name]  VARCHAR (20)     NULL,
    [debtor_first_name] VARCHAR (15)     NULL,
    [st_addr_1]         VARCHAR (25)     NULL,
    [st_addr_2]         VARCHAR (25)     NULL,
    [city]              VARCHAR (18)     NULL,
    [state_zip]         VARCHAR (15)     NULL,
    [spouse]            VARCHAR (15)     NULL,
    [phone]             VARCHAR (12)     NULL,
    [soc_security]      VARCHAR (10)     NULL,
    [license_number]    VARCHAR (20)     NULL,
    [employment]        VARCHAR (35)     NULL,
    [remarks]           VARCHAR (35)     NULL,
    [account_no]        VARCHAR (25)     NOT NULL,
    [patient_name]      VARCHAR (20)     NULL,
    [remarks2]          VARCHAR (35)     NULL,
    [misc]              VARCHAR (29)     NULL,
    [service_date]      DATETIME         NULL,
    [payment_date]      DATETIME         NULL,
    [balance]           MONEY            NULL,
    [date_entered]      DATETIME         CONSTRAINT [DF_bad_debt_date_entered_2__35] DEFAULT (getdate()) NOT NULL,
    [date_sent]         DATETIME         NULL,
    [mod_date]          DATETIME         CONSTRAINT [DF_bad_debt_mod_date_3__35] DEFAULT (getdate()) NULL,
    [mod_user]          VARCHAR (50)     CONSTRAINT [DF_bad_debt_mod_user_6__35] DEFAULT (suser_sname()) NULL,
    [mod_host]          VARCHAR (50)     CONSTRAINT [DF_bad_debt_mod_host_4__35] DEFAULT (host_name()) NULL,
    [mod_prg]           VARCHAR (50)     CONSTRAINT [DF_bad_debt_mod_prg_5__35] DEFAULT (app_name()) NULL,
    [date_transmitted]  DATETIME         NULL,
    CONSTRAINT [PK_bad_debt_1__27] PRIMARY KEY CLUSTERED ([account_no] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [date_sent_idx]
    ON [dbo].[bad_debt]([date_sent] ASC) WITH (FILLFACTOR = 90);


GO
/****** Object:  Trigger dbo.tu_bad_debt    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_bad_debt] ON dbo.bad_debt 
FOR UPDATE
AS
UPDATE bad_debt
SET bad_debt.mod_user = suser_sname(), bad_debt.mod_date = getdate(),
	bad_debt.mod_prg = app_name(), bad_debt.mod_host = host_name()
FROM inserted,bad_debt
WHERE inserted.account_no = bad_debt.account_no

GO
DISABLE TRIGGER [dbo].[tu_bad_debt]
    ON [dbo].[bad_debt];


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 02/06/2008
-- Description:	Update the audit_bad_debt table
-- =============================================
CREATE TRIGGER [dbo].[AUDIT_TRIGGER_update] 
   ON  [dbo].[bad_debt] 
    AFTER UPDATE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
	-- First check the audit_XXX table for the rowguid of the record being
	-- inserted. If it is not there insert the original recored from the 
	-- XXX table into the audit_XXX table.
	select @rowguid = b.baddebt_rowguid
	from	 audit_bad_debt b  inner join inserted i on b.baddebt_rowguid = i.rowguid

	if (@rowguid is NULL)
	begin
	Insert into audit_bad_debt
	(baddebt_rowguid, deleted, debtor_last_name, debtor_first_name, 
				st_addr_1, st_addr_2, city, state_zip,
				spouse, phone, soc_security, license_number, 
                employment, remarks, account_no, patient_name, 
				remarks2, misc, service_date, payment_date,
				balance, date_entered, date_sent, 
				mod_date, mod_user, mod_host, mod_prg)
	select
	del.rowguid, del.deleted, del.debtor_last_name, del.debtor_first_name, 
				del.st_addr_1, del.st_addr_2, del.city, del.state_zip,
				del.spouse, del.phone, del.soc_security, del.license_number, 
                del.employment, del.remarks, del.account_no, del.patient_name, 
				del.remarks2, del.misc, del.service_date, del.payment_date,
				del.balance, del.date_entered, del.date_sent, 
				del.mod_date, del.mod_user, del.mod_host, del.mod_prg

	from deleted del
	end


/* update the audit_XXX fields */
	Insert into audit_bad_debt
	(baddebt_rowguid, deleted, debtor_last_name, debtor_first_name, 
				st_addr_1, st_addr_2, city, state_zip,
				spouse, phone, soc_security, license_number, 
                employment, remarks, account_no, patient_name, 
				remarks2, misc, service_date, payment_date,
				balance, date_entered, date_sent, 
				mod_date, mod_user, mod_host, mod_prg)
	select
	ins.rowguid, ins.deleted, ins.debtor_last_name, ins.debtor_first_name, 
				ins.st_addr_1, ins.st_addr_2, ins.city, ins.state_zip,
				ins.spouse, ins.phone, ins.soc_security, ins.license_number, 
                ins.employment, ins.remarks, ins.account_no, ins.patient_name, 
				ins.remarks2, ins.misc, ins.service_date, ins.payment_date,
				ins.balance, ins.date_entered, ins.date_sent, 
				getdate(), suser_sname(), host_name(),app_name()

	from inserted ins
	


GO
-- =============================================
-- Author:		David and Rick
-- Create date: 02/07/2008
-- Description:	Monitors deletions from the bad_debt table by placing them into the audit table
-- =============================================
CREATE TRIGGER [dbo].[AUDIT_TRIGGER_delete] 
   ON  [dbo].[bad_debt] 
   AFTER DELETE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null
	-- First check the audit_XXX table for the rowguid of the record being
	-- inserted. If it is not there insert the original recored from the 
	-- XXX table into the audit_XXX table.
	select @rowguid = b.baddebt_rowguid
	from	 audit_bad_debt b  inner join deleted d on b.baddebt_rowguid = d.rowguid

	if (@rowguid is NULL)
	begin
	Insert into audit_bad_debt
	(baddebt_rowguid, deleted, debtor_last_name, debtor_first_name, 
				st_addr_1, st_addr_2, city, state_zip,
				spouse, phone, soc_security, license_number, 
                employment, remarks, account_no, patient_name, 
				remarks2, misc, service_date, payment_date,
				balance, date_entered, date_sent, 
				mod_date, mod_user, mod_host, mod_prg)
	select
	del.rowguid, del.deleted, del.debtor_last_name, del.debtor_first_name, 
				del.st_addr_1, del.st_addr_2, del.city, del.state_zip,
				del.spouse, del.phone, del.soc_security, del.license_number, 
                del.employment, del.remarks, del.account_no, del.patient_name, 
				del.remarks2, del.misc, del.service_date, del.payment_date,
				del.balance, del.date_entered, del.date_sent, 
				del.mod_date, del.mod_user, del.mod_host, del.mod_prg

	from deleted del
	end


/* update the audit_XXX fields */
	Insert into audit_bad_debt
	(baddebt_rowguid, deleted, debtor_last_name, debtor_first_name, 
				st_addr_1, st_addr_2, city, state_zip,
				spouse, phone, soc_security, license_number, 
                employment, remarks, account_no, patient_name, 
				remarks2, misc, service_date, payment_date,
				balance, date_entered, date_sent, 
				mod_date, mod_user, mod_host, mod_prg)
	select
	del.rowguid, del.deleted, del.debtor_last_name, del.debtor_first_name, 
				del.st_addr_1, del.st_addr_2, del.city, del.state_zip,
				del.spouse, del.phone, del.soc_security, del.license_number, 
                del.employment, del.remarks, del.account_no, del.patient_name, 
				del.remarks2, del.misc, del.service_date, del.payment_date,
				del.balance, del.date_entered, del.date_sent, 
				getdate(), suser_sname(), host_name(), 
				'D~'+app_name() -- 'D~' in the audit_XXX indicats the record was REMOVED from the XXX table.

	from deleted del
	


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 02/07/2008 added for audit table/history viewer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'bad_debt', @level2type = N'COLUMN', @level2name = N'rowguid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 02/06/2008 modified to make the length 50', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'bad_debt', @level2type = N'COLUMN', @level2name = N'mod_user';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 02/06/2008 modified to make the length 50', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'bad_debt', @level2type = N'COLUMN', @level2name = N'mod_host';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 02/06/2008 modified to make the length 50', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'bad_debt', @level2type = N'COLUMN', @level2name = N'mod_prg';

