CREATE TABLE [dbo].[cdm] (
    [deleted]           BIT             CONSTRAINT [DF_cdm_deleted_3__23] DEFAULT ((0)) NOT NULL,
    [cdm]               VARCHAR (7)     NOT NULL,
    [descript]          VARCHAR (50)    NULL,
    [mtype]             VARCHAR (10)    NULL,
    [m_pa_amt]          MONEY           CONSTRAINT [DF_cdm_m_pa_amt_4__23] DEFAULT ((0)) NULL,
    [ctype]             VARCHAR (10)    NULL,
    [c_pa_amt]          MONEY           CONSTRAINT [DF_cdm_c_pa_amt_2__23] DEFAULT ((0)) NULL,
    [ztype]             VARCHAR (10)    NULL,
    [z_pa_amt]          MONEY           CONSTRAINT [DF_cdm_z_pa_amt_9__23] DEFAULT ((0)) NULL,
    [orderable]         INT             CONSTRAINT [DF_cdm_orderable_1__20] DEFAULT ((1)) NULL,
    [cbill_detail]      INT             CONSTRAINT [DF_cdm_cbill_detail_1__35] DEFAULT ((0)) NULL,
    [comments]          VARCHAR (MAX)   NULL,
    [mnem]              VARCHAR (15)    NULL,
    [cost]              NUMERIC (18, 2) NULL,
    [ref_lab_id]        VARCHAR (50)    NULL,
    [ref_lab_bill_code] VARCHAR (50)    NULL,
    [ref_lab_payment]   NUMERIC (18, 2) NULL,
    [mod_date]          DATETIME        CONSTRAINT [DF_cdm_mod_date_5__23] DEFAULT (getdate()) NULL,
    [mod_user]          VARCHAR (50)    CONSTRAINT [DF_cdm_mod_user_8__23] DEFAULT (suser_sname()) NULL,
    [mod_prg]           VARCHAR (50)    CONSTRAINT [DF_cdm_mod_prg_7__23] DEFAULT (app_name()) NULL,
    [mod_host]          VARCHAR (50)    CONSTRAINT [DF_cdm_mod_host_6__23] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_cdm_1] PRIMARY KEY CLUSTERED ([cdm] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_cdm_descript]
    ON [dbo].[cdm]([descript] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_cdm_mnem]
    ON [dbo].[cdm]([mnem] ASC) WITH (FILLFACTOR = 90);


GO
/****** Object:  Trigger dbo.tu_cdm    Script Date: 8/4/98 9:04:05 AM ******/
CREATE TRIGGER [dbo].[tu_cdm] ON dbo.cdm 
FOR UPDATE 
AS
UPDATE cdm
SET cdm.mod_user = suser_sname(), cdm.mod_date = getdate(), cdm.mod_prg = app_name()
FROM inserted,cdm
WHERE inserted.cdm = cdm.cdm

GO
DISABLE TRIGGER [dbo].[tu_cdm]
    ON [dbo].[cdm];


GO
-- =============================================
-- Author:		David
-- Create date: 03/02/2012
-- Description:	New trigger for tracking changes in the data
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_CDM] 
   ON  [dbo].[cdm] 
   AFTER INSERT,DELETE,UPDATE
AS 
DECLARE @mod_date datetime
set @mod_date = getdate()
DECLARE @mod_host varchar(50)
set @mod_host = host_name()
DECLARE @mod_prg varchar(50)
set @mod_prg = app_name()
DECLARE @mod_user varchar(50)
set @mod_user = suser_sname()
DECLARE @mod_indicator varchar(10)
set @mod_indicator = null
DECLARE @deleted int
set @deleted = 0
DECLARE @inserted int
set @inserted = 0
DECLARE @cdm numeric(15,0)
set @cdm = null
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
 -- Insert statements for trigger here
	-- if @inserted > 0 and @deleted = 0 we are inserting the record for the first time.
	-- if @inserted > 0 and @deleted > 0 we are updating a record
	-- if @inserted = 0 and @deleted > 0 we are deleting the record
	-- if @chrg_num is NULL we have not previously added this charge to the audit table so do so 
	select @inserted = (select count(*) from inserted)
	select @deleted = (select count(*) from deleted)

	if (@inserted > 0 and @deleted = 0) -- first insert
	begin
		set @mod_indicator = 'IO' -- inserted origional
		insert into audit_cdm
			(	deleted, cdm, descript, mtype, m_pa_amt, 
				ctype, c_pa_amt, ztype, z_pa_amt, orderable, 
				cbill_detail, comments, mnem, cost, 
				ref_lab_id, ref_lab_bill_code, ref_lab_payment,
				mod_date, mod_user, mod_prg, mod_host, mod_indicator			)
		select
			ins.deleted, ins.cdm, ins.descript, ins.mtype, ins.m_pa_amt, 
			ins.ctype, ins.c_pa_amt, ins.ztype, ins.z_pa_amt, ins.orderable, 
			ins.cbill_detail, ins.comments, ins.mnem, ins.cost, 
			ins.ref_lab_id, ins.ref_lab_bill_code, ins.ref_lab_payment,
			@mod_date, @mod_user, @mod_prg, @mod_host,
			@mod_indicator -- this is the inserted record 
	from inserted ins
	end -- first insert

	if (@inserted > 0 and @deleted > 0) -- modification
	begin
		set @mod_indicator = 'MUD'
		insert into audit_cdm
			(	deleted, cdm, descript, mtype, m_pa_amt, 
				ctype, c_pa_amt, ztype, z_pa_amt, orderable, 
				cbill_detail, comments, mnem, cost,
				ref_lab_id, ref_lab_bill_code, ref_lab_payment, 
				mod_date, mod_user, mod_prg, mod_host, mod_indicator			)
		select
			del.deleted, del.cdm, del.descript, del.mtype, del.m_pa_amt, 
			del.ctype, del.c_pa_amt, del.ztype, del.z_pa_amt, del.orderable, 
			del.cbill_detail, del.comments, del.mnem, del.cost, 
			del.ref_lab_id, del.ref_lab_bill_code, del.ref_lab_payment,
			@mod_date, @mod_user, @mod_prg, @mod_host,
			@mod_indicator -- this is the inserted record that was changed from the table
		from deleted del

		set @mod_indicator = 'MUI'
		insert into audit_cdm
			(	deleted, cdm, descript, mtype, m_pa_amt, 
				ctype, c_pa_amt, ztype, z_pa_amt, orderable, 
				cbill_detail, comments, mnem, cost,
				ref_lab_id, ref_lab_bill_code, ref_lab_payment, 
				mod_date, mod_user, mod_prg, mod_host,mod_indicator			)
		select
			ins.deleted, ins.cdm, ins.descript, ins.mtype, ins.m_pa_amt, 
			ins.ctype, ins.c_pa_amt, ins.ztype, ins.z_pa_amt, ins.orderable, 
			ins.cbill_detail, ins.comments, ins.mnem, ins.cost,
			ins.ref_lab_id, ins.ref_lab_bill_code, ins.ref_lab_payment, 
			@mod_date, @mod_user, @mod_prg, @mod_host,
			@mod_indicator -- this is the inserted record 
	from inserted ins
	end -- modifications

	if (@inserted = 0 and @deleted > 0) -- we are deleting
	begin
	
	select @cdm = audit.cdm
		from	 audit_cdm audit inner join deleted d on audit.cdm = d.cdm

	if (@cdm is NULL) -- copy the original record first
		begin
		set @mod_indicator = 'DUI'
		insert into audit_cdm
			(	deleted, cdm, descript, mtype, m_pa_amt, 
				ctype, c_pa_amt, ztype, z_pa_amt, orderable, 
				cbill_detail, comments, mnem, cost, 
				ref_lab_id, ref_lab_bill_code, ref_lab_payment,
				mod_date, mod_user, mod_prg, mod_host,mod_indicator			)
		select
			del.deleted, del.cdm, del.descript, del.mtype, del.m_pa_amt, 
			del.ctype, del.c_pa_amt, del.ztype, del.z_pa_amt, del.orderable, 
			del.cbill_detail, del.comments, del.mnem, del.cost,
			del.ref_lab_id, del.ref_lab_bill_code, del.ref_lab_payment, 
			del.mod_date, del.mod_user, del.mod_prg, del.mod_host,
			@mod_indicator -- this is the inserted record that was deleted from the table
		from deleted del
		
		end
		-- now copy with the audit info for who deleted it
		set @mod_indicator = 'DUD'
		insert into audit_cdm
			(	deleted, cdm, descript, mtype, m_pa_amt, 
				ctype, c_pa_amt, ztype, z_pa_amt, orderable, 
				cbill_detail, comments, mnem, cost, 
				ref_lab_id, ref_lab_bill_code, ref_lab_payment,
				mod_date, mod_user, mod_prg, mod_host,mod_indicator			)
		select
			del.deleted, del.cdm, del.descript, del.mtype, del.m_pa_amt, 
			del.ctype, del.c_pa_amt, del.ztype, del.z_pa_amt, del.orderable, 
			del.cbill_detail, del.comments, del.mnem, del.cost,
			del.ref_lab_id, del.ref_lab_bill_code, del.ref_lab_payment, 
			@mod_date, @mod_user, 'D~'+@mod_prg, @mod_host,
			@mod_indicator -- this is the inserted record that was changed from the table
		from deleted del

end


















/*
deleted, cdm, descript, mtype, m_pa_amt, 
ctype, c_pa_amt, ztype, z_pa_amt, orderable, 
cbill_detail, comments, mnem, cost, 
mod_date, mod_user, mod_prg, mod_host
*/




END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20120302 changed from text to varchar(max) for auditing triggers.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cdm', @level2type = N'COLUMN', @level2name = N'comments';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20121017 added', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cdm', @level2type = N'COLUMN', @level2name = N'ref_lab_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20121017 added', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cdm', @level2type = N'COLUMN', @level2name = N'ref_lab_bill_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20121017 added', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'cdm', @level2type = N'COLUMN', @level2name = N'ref_lab_payment';

