CREATE TABLE [dbo].[ins] (
    [rowguid]              UNIQUEIDENTIFIER CONSTRAINT [DF_ins_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]              BIT              CONSTRAINT [DF_ins_deleted_1__15] DEFAULT ((0)) NOT NULL,
    [account]              VARCHAR (15)     NOT NULL,
    [ins_a_b_c]            VARCHAR (1)      NOT NULL,
    [holder_nme]           VARCHAR (40)     NULL,
    [holder_dob]           DATETIME         NULL,
    [holder_sex]           VARCHAR (1)      NULL,
    [holder_addr]          VARCHAR (40)     NULL,
    [holder_city_st_zip]   VARCHAR (40)     NULL,
    [plan_nme]             VARCHAR (45)     NULL,
    [plan_addr1]           VARCHAR (40)     NULL,
    [plan_addr2]           VARCHAR (40)     NULL,
    [p_city_st]            VARCHAR (40)     NULL,
    [policy_num]           VARCHAR (50)     NULL,
    [cert_ssn]             VARCHAR (15)     NULL,
    [grp_nme]              VARCHAR (50)     NULL,
    [grp_num]              VARCHAR (15)     NULL,
    [employer]             VARCHAR (25)     NULL,
    [e_city_st]            VARCHAR (35)     NULL,
    [fin_code]             VARCHAR (1)      NULL,
    [ins_code]             VARCHAR (10)     NULL,
    [relation]             VARCHAR (2)      NULL,
    [mod_date]             DATETIME         CONSTRAINT [DF_ins_mod_date_3__11] DEFAULT (getdate()) NOT NULL,
    [mod_user]             VARCHAR (50)     CONSTRAINT [DF_ins_mod_user_5__11] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]              VARCHAR (50)     CONSTRAINT [DF_ins_mod_prg_4__11] DEFAULT (app_name()) NOT NULL,
    [mod_host]             VARCHAR (50)     CONSTRAINT [DF_ins_mod_host] DEFAULT (host_name()) NOT NULL,
    [holder_lname]         VARCHAR (40)     NULL,
    [holder_fname]         VARCHAR (40)     NULL,
    [holder_mname]         VARCHAR (40)     NULL,
    [plan_effective_date]  DATETIME         NULL,
    [plan_expiration_date] DATETIME         NULL,
    CONSTRAINT [PK_account_abc] PRIMARY KEY CLUSTERED ([account] ASC, [ins_a_b_c] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [policy_num_idx]
    ON [dbo].[ins]([policy_num] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_rowguid INCLUDE account, ins_a_b_c]
    ON [dbo].[ins]([rowguid] ASC)
    INCLUDE([account], [ins_a_b_c]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_abc Include account, ins_code]
    ON [dbo].[ins]([ins_a_b_c] ASC)
    INCLUDE([account], [ins_code], [policy_num]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_deleted]
    ON [dbo].[ins]([deleted] ASC)
    INCLUDE([account]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_ins_code]
    ON [dbo].[ins]([ins_code] ASC)
    INCLUDE([account], [ins_a_b_c], [policy_num]) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/01/2007
-- Description:	Move deleted records to the audit table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_DELETED_ins] 
   ON  [dbo].[ins] 
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
-- old record before delete
--	insert into audit_ins
--	(ins_rowguid, deleted, account, ins_a_b_c, holder_nme, plan_nme, plan_addr1, 
--		plan_addr2, p_city_st, policy_num, cert_ssn, grp_nme, grp_num, 
--		holder_sex, employer, e_city_st, fin_code, ins_code, 
--		mod_date, mod_user, mod_prg, mod_host)
--	select 
--		del.rowguid, del.deleted, del.account, del.ins_a_b_c, del.holder_nme, 
--		del.plan_nme, del.plan_addr1, del.plan_addr2, del.p_city_st, del.policy_num,
--		del.cert_ssn, del.grp_nme, del.grp_num, del.holder_sex, del.employer,
--		del.e_city_st, del.fin_code, del.ins_code, 
--		del.mod_date, del.mod_user, del.mod_prg, del.mod_host
--
--	from deleted del
-- old record with mod fields from delete
insert into audit_ins
	(ins_rowguid, deleted, account, ins_a_b_c, holder_nme, holder_dob, plan_nme, plan_addr1, 
		plan_addr2, p_city_st, policy_num, cert_ssn, grp_nme, grp_num, 
		holder_sex, employer, e_city_st, fin_code, ins_code, relation,
		mod_date, mod_user, mod_prg, mod_host)
	select 
		del.rowguid, del.deleted, del.account, del.ins_a_b_c, del.holder_nme, del.holder_dob,
		del.plan_nme, del.plan_addr1, del.plan_addr2, del.p_city_st, del.policy_num,
		del.cert_ssn, del.grp_nme, del.grp_num, del.holder_sex, del.employer,
		del.e_city_st, del.fin_code, del.ins_code,  del.relation,
		getdate(),
		right(suser_sname(),50),
		right('D~'+app_name(),50), -- 'D~' in the audit_XXX indicats the record was REMOVED from the XXX table.
		right(host_name(),50)

	from deleted del
END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/01/2007
-- Description:	Provide audition for the ins table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_ins] 
   ON  [dbo].[ins] 
   FOR UPDATE
AS 
DECLARE @rowguid uniqueidentifier
set @rowguid = null

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for trigger here
	-- First check the audit_XXX table for the rowguid of the record being
	-- inserted. If it is not there insert the original recored from the 
	-- XXX table into the audit_XXX table.
	select @rowguid = ins.ins_rowguid
	from	 audit_ins ins inner join inserted i on ins.ins_rowguid = i.rowguid
		
	if (@rowguid is NULL)
	begin
		insert into audit_ins
	(ins_rowguid, deleted, account, ins_a_b_c, holder_nme, holder_dob, plan_nme, plan_addr1, 
		plan_addr2, p_city_st, policy_num, cert_ssn, grp_nme, grp_num, 
		holder_sex, employer, e_city_st, fin_code, ins_code, relation, 
		mod_date, mod_user, mod_prg, mod_host)
	select 
		del.rowguid, del.deleted, del.account, del.ins_a_b_c, del.holder_nme, del.holder_dob,
		del.plan_nme, del.plan_addr1, del.plan_addr2, del.p_city_st, del.policy_num,
		del.cert_ssn, del.grp_nme, del.grp_num, del.holder_sex, del.employer,
		del.e_city_st, del.fin_code, del.ins_code, del.relation,
		del.mod_date, right(del.mod_user,50), right(del.mod_prg,50), right(del.mod_host,50)

	from deleted del
	end

	insert into audit_ins
	(ins_rowguid, deleted, account, ins_a_b_c, holder_nme, holder_dob, plan_nme, plan_addr1, 
		plan_addr2, p_city_st, policy_num, cert_ssn, grp_nme, grp_num, 
		holder_sex, employer, e_city_st, fin_code, ins_code, relation,
		mod_date, mod_user, mod_prg, mod_host)
	select 
		ins.rowguid, ins.deleted, ins.account, ins.ins_a_b_c, ins.holder_nme, ins.holder_dob,
		ins.plan_nme, ins.plan_addr1, ins.plan_addr2, ins.p_city_st, ins.policy_num,
		ins.cert_ssn, ins.grp_nme, ins.grp_num, ins.holder_sex, ins.employer,
		ins.e_city_st, ins.fin_code, ins.ins_code, ins.relation,
		getdate(), right(suser_sname(),50), right(app_name(),50), right(host_name(),50)

	from inserted ins


/* update the audit_ins fields before changing the ins's mod_XXX fields */



END

GO
-- =============================================
-- Author:		David
-- Create date: 9/5/2013
-- Description:	Remove the - from the zip code as the last digit
-- =============================================
create TRIGGER dbo.TRIGGER_FIX_ZIP 
   ON  dbo.ins 
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	if (update(p_city_st))
	begin
		update ins
		set p_city_st = left(i.p_city_st,len(i.p_city_st)-1)
		from inserted i
		inner join ins on ins.account = i.account and ins.ins_a_b_c = i.ins_a_b_c
		where charindex('-',i.p_city_st,0) = len(i.p_city_st) and coalesce(i.p_city_st,'') <> ''
		
	end
	
	if (update(holder_city_st_zip))
	begin
			update ins
		set holder_city_st_zip = left(i.holder_city_st_zip,len(i.holder_city_st_zip)-1)
		from inserted i
		inner join ins on ins.account = i.account and ins.ins_a_b_c = i.ins_a_b_c
		where charindex('-',i.holder_city_st_zip,0) = len(i.holder_city_st_zip) and coalesce(i.holder_city_st_zip,'') <> ''
	end

	if (update(e_city_st))
	begin
			update ins
		set e_city_st = left(i.e_city_st,len(i.e_city_st)-1)
		from inserted i
		inner join ins on ins.account = i.account and ins.ins_a_b_c = i.ins_a_b_c
		where charindex('-',i.e_city_st,0) = len(i.e_city_st) and coalesce(i.e_city_st,'') <> ''
	end

END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'20100217 wdk added for new requirement from Bluecross', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ins', @level2type = N'COLUMN', @level2name = N'holder_dob';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20110928 added for compatability with SSI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ins', @level2type = N'COLUMN', @level2name = N'holder_addr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20110928 added for compatability with SSI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ins', @level2type = N'COLUMN', @level2name = N'holder_city_st_zip';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'20100217 wdk added for new requirement from Bluecross', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ins', @level2type = N'COLUMN', @level2name = N'relation';

