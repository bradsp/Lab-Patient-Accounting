CREATE TABLE [dbo].[pat] (
    [rowguid]                   UNIQUEIDENTIFIER CONSTRAINT [DF_pat_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [deleted]                   BIT              CONSTRAINT [DF_pat_deleted_1__10] DEFAULT ((0)) NOT NULL,
    [account]                   VARCHAR (15)     NOT NULL,
    [ssn]                       VARCHAR (11)     NULL,
    [pat_addr1]                 VARCHAR (40)     NULL,
    [pat_addr2]                 VARCHAR (40)     NULL,
    [city_st_zip]               VARCHAR (40)     NULL,
    [dob_yyyy]                  DATETIME         NULL,
    [sex]                       VARCHAR (1)      NULL,
    [relation]                  VARCHAR (2)      NULL,
    [guarantor]                 VARCHAR (40)     NULL,
    [guar_addr]                 VARCHAR (40)     NULL,
    [g_city_st]                 VARCHAR (50)     NULL,
    [pat_marital]               VARCHAR (1)      CONSTRAINT [DF_pat_pat_marital] DEFAULT ('U') NULL,
    [icd9_1]                    VARCHAR (7)      NULL,
    [icd9_2]                    VARCHAR (7)      NULL,
    [icd9_3]                    VARCHAR (7)      NULL,
    [icd9_4]                    VARCHAR (7)      NULL,
    [icd9_5]                    VARCHAR (7)      NULL,
    [icd9_6]                    VARCHAR (7)      NULL,
    [icd9_7]                    VARCHAR (7)      NULL,
    [icd9_8]                    VARCHAR (7)      NULL,
    [icd9_9]                    VARCHAR (7)      NULL,
    [icd_indicator]             VARCHAR (3)      CONSTRAINT [DF_pat_icd_indicator] DEFAULT ('I10') NULL,
    [pc_code]                   INT              NULL,
    [mailer]                    VARCHAR (1)      CONSTRAINT [DF_pat_mailer_1__15] DEFAULT ('N') NULL,
    [first_dm]                  DATETIME         NULL,
    [last_dm]                   DATETIME         NULL,
    [min_amt]                   MONEY            CONSTRAINT [DF_pat_min_amt_2__10] DEFAULT ((0.00)) NULL,
    [phy_id]                    VARCHAR (15)     NULL,
    [dbill_date]                DATETIME         NULL,
    [ub_date]                   DATETIME         NULL,
    [h1500_date]                DATETIME         NULL,
    [ssi_batch]                 VARCHAR (50)     NULL,
    [colltr_date]               DATETIME         NULL,
    [baddebt_date]              DATETIME         NULL,
    [batch_date]                DATETIME         NULL,
    [guar_phone]                VARCHAR (13)     NULL,
    [bd_list_date]              DATETIME         NULL,
    [ebill_batch_date]          DATETIME         NULL,
    [ebill_batch_1500]          DATETIME         NULL,
    [e_ub_demand]               BIT              CONSTRAINT [DF_pat_e_ub_demand] DEFAULT ((0)) NOT NULL,
    [e_ub_demand_date]          DATETIME         NULL,
    [claimsnet_1500_batch_date] DATETIME         NULL,
    [claimsnet_ub_batch_date]   DATETIME         NULL,
    [mod_date]                  DATETIME         CONSTRAINT [DF_pat_mod_date_2__15] DEFAULT (getdate()) NOT NULL,
    [mod_user]                  VARCHAR (50)     CONSTRAINT [DF_pat_mod_user_4__15] DEFAULT (right(suser_sname(),(50))) NOT NULL,
    [mod_prg]                   VARCHAR (50)     CONSTRAINT [DF_pat_mod_prg_3__15] DEFAULT (right(app_name(),(50))) NOT NULL,
    [mod_host]                  VARCHAR (50)     CONSTRAINT [DF_pat_mod_host] DEFAULT (right(host_name(),(50))) NOT NULL,
    [hne_epi_number]            VARCHAR (50)     NULL,
    [pat_full_name]             VARCHAR (128)    NULL,
    [pat_city]                  VARCHAR (50)     NULL,
    [pat_state]                 VARCHAR (2)      NULL,
    [pat_zip]                   VARCHAR (10)     NULL,
    [guar_city]                 VARCHAR (50)     NULL,
    [guar_state]                VARCHAR (2)      NULL,
    [guar_zip]                  VARCHAR (10)     NULL,
    [pat_race]                  VARCHAR (5)      NULL,
    [pat_phone]                 VARCHAR (25)     NULL,
    [phy_comment]               VARCHAR (128)    NULL,
    [location]                  VARCHAR (50)     NULL,
    [pat_email]                 VARCHAR (256)    NULL,
    [dx_update_prg]             VARCHAR (50)     NULL,
    CONSTRAINT [PK_pat_3__10] PRIMARY KEY CLUSTERED ([account] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_account_include_billtracking]
    ON [dbo].[pat]([account] ASC)
    INCLUDE([mailer], [phy_id], [dbill_date], [ub_date], [h1500_date], [batch_date], [ebill_batch_date]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [batch_date_idx]
    ON [dbo].[pat]([batch_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ebill_batch_1500]
    ON [dbo].[pat]([ebill_batch_1500] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ebill_batch_date_idx]
    ON [dbo].[pat]([ebill_batch_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_pat_last_dm]
    ON [dbo].[pat]([last_dm] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_rowguid]
    ON [dbo].[pat]([rowguid] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_mailer INCLUDE account]
    ON [dbo].[pat]([mailer] ASC)
    INCLUDE([account]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_ssi_batch]
    ON [dbo].[pat]([ssi_batch] ASC)
    INCLUDE([account], [ub_date], [h1500_date]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_pat_demographics]
    ON [dbo].[pat]([ssn] ASC, [dob_yyyy] ASC, [sex] ASC, [icd9_1] ASC, [relation] ASC, [account] ASC, [mod_date] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [phy_id_idx]
    ON [dbo].[pat]([phy_id] ASC, [account] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [guarantor_idx]
    ON [dbo].[pat]([guarantor] ASC, [account] ASC) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/01/2007
-- Description:	Updates the audit_pat table when changes are made.
-- Modification:
-- 01/31/2008 rgc/wdk Added two fields e_ub_demand (bit) and e_ub_demand_date (datetime)
--		So we modified this trigger to handle the new fields
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_pat] 
   ON  dbo.pat 
   AFTER UPDATE
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
	select @rowguid = p.pat_rowguid
	from	 audit_pat p inner join inserted i on p.pat_rowguid = i.rowguid

	if (@rowguid is NULL)
	begin
			Insert into audit_pat
	(pat_rowguid, deleted, account, ssn, pat_addr1, pat_addr2, city_st_zip,
		dob_yyyy, sex, relation, guarantor, guar_addr, g_city_st, pat_marital, icd9_1,
	    icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9, pc_code,
		mailer, first_dm, last_dm, min_amt, phy_id, dbill_date, ub_date, h1500_date, 
		colltr_date, baddebt_date, batch_date, mod_date, mod_user, mod_prg,
		guar_phone, bd_list_date, ebill_batch_date, ebill_batch_1500, mod_host,
		e_ub_demand, e_ub_demand_date, claimsnet_1500_batch_date, claimsnet_ub_batch_date, hne_epi_number
, ssi_batch)
	select
	del.rowguid, del.deleted, del.account, del.ssn, del.pat_addr1, 
	del.pat_addr2, del.city_st_zip, del.dob_yyyy, del.sex, del.relation, 
	del.guarantor, del.guar_addr, del.g_city_st, del.pat_marital, del.icd9_1, 
	del.icd9_2, del.icd9_3, del.icd9_4, del.icd9_5, del.icd9_6, del.icd9_7, 
	del.icd9_8, del.icd9_9, del.pc_code, del.mailer, del.first_dm, del.last_dm, 
	del.min_amt, del.phy_id, del.dbill_date, del.ub_date, del.h1500_date, 
	del.colltr_date, del.baddebt_date, del.batch_date, del.mod_date, 
	del.mod_user, del.mod_prg, del.guar_phone, del.bd_list_date, del.ebill_batch_date,
	del.ebill_batch_1500, del.mod_host,del.e_ub_demand, del.e_ub_demand_date,
	del.claimsnet_1500_batch_date, del.claimsnet_ub_batch_date, del.hne_epi_number
	, del.ssi_batch

	from deleted del
	end


/* update the audit_XXX fields */
	Insert into audit_pat
	(pat_rowguid, deleted, account, ssn, pat_addr1, pat_addr2, city_st_zip,
		dob_yyyy, sex, relation, guarantor, guar_addr, g_city_st, pat_marital, icd9_1,
	    icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9, pc_code,
		mailer, first_dm, last_dm, min_amt, phy_id, dbill_date, ub_date, h1500_date, 
		colltr_date, baddebt_date, batch_date, mod_date, mod_user, mod_prg,
		guar_phone, bd_list_date, ebill_batch_date, ebill_batch_1500, mod_host,
		e_ub_demand, e_ub_demand_date, claimsnet_1500_batch_date, claimsnet_ub_batch_date, hne_epi_number
	,ssi_batch)
	select
	ins.rowguid, ins.deleted, ins.account, ins.ssn, ins.pat_addr1, 
	ins.pat_addr2, ins.city_st_zip, ins.dob_yyyy, ins.sex, ins.relation, 
	ins.guarantor, ins.guar_addr, ins.g_city_st, ins.pat_marital, ins.icd9_1, 
	ins.icd9_2, ins.icd9_3, ins.icd9_4, ins.icd9_5, ins.icd9_6, ins.icd9_7, 
	ins.icd9_8, ins.icd9_9, ins.pc_code, ins.mailer, ins.first_dm, ins.last_dm, 
	ins.min_amt, ins.phy_id, ins.dbill_date, ins.ub_date, ins.h1500_date, 
	ins.colltr_date, ins.baddebt_date, ins.batch_date, getdate(), 
	right(suser_sname(),50)
	,COALESCE(RIGHT(APP_NAME(),50),RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
		'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50),'NO APP IDENTIFIED')
	--, right(app_name(),50)
	, ins.guar_phone, ins.bd_list_date, ins.ebill_batch_date,
	ins.ebill_batch_1500, right(host_name(),50), ins.e_ub_demand, ins.e_ub_demand_date,
	ins.claimsnet_1500_batch_date, ins.claimsnet_ub_batch_date, ins.hne_epi_number
	, ins.ssi_batch

	from inserted ins
	
--GETDATE() AS [mod_date],
--RIGHT(SUSER_SNAME(),50) AS [mod_user],
--RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
--	'SQL PROC ' +CONVERT(VARCHAR(10),GETDATE(),112)),50) AS [mod_prg],
--RIGHT (HOST_NAME(),50)  AS [mod_host]


END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 06/01/2007
-- Description:	Trigger to move deleted records to the audit table
-- Modification:
-- 01/31/2008 rgc/wdk Added two fields e_ub_demand (bit) and e_ub_demand_date (datetime)
--		So we modified this trigger to handle the new fields
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_AUDIT_DELETE_pat] 
   ON  dbo.pat 
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

-- old record with mod fields from delete
	Insert into audit_pat
	(pat_rowguid, deleted, account, ssn, pat_addr1, pat_addr2, city_st_zip,
		dob_yyyy, sex, relation, guarantor, guar_addr, g_city_st, pat_marital, icd9_1,
	    icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9, pc_code,
		mailer, first_dm, last_dm, min_amt, phy_id, dbill_date, ub_date, h1500_date, 
		colltr_date, baddebt_date, batch_date, mod_date, mod_user, mod_prg,
		guar_phone, bd_list_date, ebill_batch_date, ebill_batch_1500, mod_host,
		e_ub_demand, e_ub_demand_date, claimsnet_1500_batch_date, claimsnet_ub_batch_date, hne_epi_number,
		ssi_batch )
	select
	del.rowguid, del.deleted, del.account, del.ssn, del.pat_addr1, 
	del.pat_addr2, del.city_st_zip, del.dob_yyyy, del.sex, del.relation, 
	del.guarantor, del.guar_addr, del.g_city_st, del.pat_marital, del.icd9_1, 
	del.icd9_2, del.icd9_3, del.icd9_4, del.icd9_5, del.icd9_6, del.icd9_7, 
	del.icd9_8, del.icd9_9, del.pc_code, del.mailer, del.first_dm, del.last_dm, 
	del.min_amt, del.phy_id, del.dbill_date, del.ub_date, del.h1500_date, 
	del.colltr_date, del.baddebt_date, del.batch_date, 
	getdate(), 
	right(suser_sname(),50), 
	right('D~'+app_name(),50), -- 'D~' in the audit_XXX indicats the record was REMOVED from the XXX table.
	 del.guar_phone, del.bd_list_date, del.ebill_batch_date, del.ebill_batch_1500,
	right(host_name(),50), del.e_ub_demand, del.e_ub_demand_date, del.claimsnet_1500_batch_date, del.claimsnet_ub_batch_date, 
	del.hne_epi_number, del.ssi_batch

	from deleted del



END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20120217 added for tracking SSI UB''s and 1500''s in conjuction with the ub_date and h1500_date fields', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'ssi_batch';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This is the date we placed in the h1500 table for billing thru Claimsnet 07/04/2008 wdk/rgc', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'claimsnet_1500_batch_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'11/17/2008 wdk this is the date and time we placed the ub in the ub table for claimsnet billing.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'claimsnet_ub_batch_date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100915 Added for new demo tfxr file', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'hne_epi_number';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'pat_full_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'pat_city';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'pat_state';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'pat_zip';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'guar_city';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'guar_state';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'guar_zip';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'pat_race';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'pat_phone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404025 to update ViewerQuest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'pat', @level2type = N'COLUMN', @level2name = N'phy_comment';

