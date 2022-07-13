CREATE TABLE [dbo].[client] (
    [deleted]                 BIT           CONSTRAINT [DF_client_deleted_3__12] DEFAULT ((0)) NOT NULL,
    [cli_mnem]                VARCHAR (10)  NOT NULL,
    [cli_nme]                 VARCHAR (40)  NULL,
    [addr_1]                  VARCHAR (40)  NULL,
    [addr_2]                  VARCHAR (40)  NULL,
    [city]                    VARCHAR (30)  NULL,
    [st]                      VARCHAR (2)   NULL,
    [zip]                     VARCHAR (10)  NULL,
    [phone]                   VARCHAR (40)  NULL,
    [fax]                     VARCHAR (15)  NULL,
    [contact]                 VARCHAR (MAX) NULL,
    [comment]                 VARCHAR (MAX) NULL,
    [prn_cpt4]                BIT           CONSTRAINT [DF_client_prn_cpt4_8__12] DEFAULT ((0)) NOT NULL,
    [per_disc]                REAL          CONSTRAINT [DF_client_per_disc_1__11] DEFAULT ((0)) NULL,
    [date_ord]                BIT           CONSTRAINT [DF_client_date_ord_2__12] DEFAULT ((0)) NOT NULL,
    [print_cc]                BIT           CONSTRAINT [DF_client_print_cc_7__12] DEFAULT ((0)) NOT NULL,
    [bill_at_disc]            BIT           CONSTRAINT [DF_client_bill_at_disc_1__11] DEFAULT ((1)) NOT NULL,
    [do_not_bill]             BIT           CONSTRAINT [DF_client_do_not_bill_1__15] DEFAULT ((0)) NOT NULL,
    [type]                    INT           CONSTRAINT [DF_client_type_1__10] DEFAULT ((9)) NULL,
    [last_invoice]            VARCHAR (15)  NULL,
    [last_invoice_date]       DATETIME      NULL,
    [last_discount]           VARCHAR (15)  NULL,
    [mod_date]                DATETIME      CONSTRAINT [DF_client_mod_date_4__12] DEFAULT (getdate()) NULL,
    [mod_user]                VARCHAR (50)  CONSTRAINT [DF_client_mod_user_6__12] DEFAULT (suser_sname()) NULL,
    [mod_prg]                 VARCHAR (50)  CONSTRAINT [DF_client_mod_prg_5__12] DEFAULT (app_name()) NULL,
    [mod_host]                VARCHAR (50)  CONSTRAINT [DF_client_mod_host_1__36] DEFAULT (host_name()) NULL,
    [mro_name]                VARCHAR (40)  NULL,
    [mro_addr1]               VARCHAR (40)  NULL,
    [mro_addr2]               VARCHAR (40)  NULL,
    [mro_city]                VARCHAR (30)  NULL,
    [mro_st]                  VARCHAR (2)   NULL,
    [mro_zip]                 VARCHAR (10)  NULL,
    [prn_loc]                 VARCHAR (1)   CONSTRAINT [DF_client_prn_loc_1__25] DEFAULT ('Y') NULL,
    [route]                   VARCHAR (10)  NULL,
    [county]                  VARCHAR (30)  NULL,
    [email]                   VARCHAR (40)  NULL,
    [late_notice]             VARCHAR (1)   CONSTRAINT [DF_client_late_notice_1__15] DEFAULT ('N') NULL,
    [late_notice_date]        DATETIME      NULL,
    [statsFacility]           VARCHAR (15)  NULL,
    [fee_schedule]            TINYINT       CONSTRAINT [DF_client_fee_schedule_1] DEFAULT ((2)) NULL,
    [commission]              BIT           NULL,
    [client_class]            VARCHAR (5)   NULL,
    [client_maint_rep]        VARCHAR (30)  NULL,
    [client_sales_rep]        VARCHAR (30)  NULL,
    [outpatient_billing]      BIT           CONSTRAINT [DF_client_outpatient_billing] DEFAULT ((0)) NOT NULL,
    [electronic_billing_type] VARCHAR (50)  NULL,
    [gl_code]                 VARCHAR (10)  CONSTRAINT [DF_client_gl_code] DEFAULT ((7009)) NOT NULL,
    [facilityNo]              VARCHAR (50)  NULL,
    [bill_pc_charges]         VARCHAR (4)   CONSTRAINT [DF_client_bill_pc_charges] DEFAULT ('NO') NOT NULL,
    [notes]                   VARCHAR (MAX) NULL,
    [old_phone]               VARCHAR (40)  NULL,
    [old_fax]                 VARCHAR (15)  NULL,
    [bill_to_client]          VARCHAR (10)  NULL,
    CONSTRAINT [PK_client_1__13] PRIMARY KEY CLUSTERED ([cli_mnem] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_client_cli_nme]
    ON [dbo].[client]([cli_nme] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_deleted, cli_mnem]
    ON [dbo].[client]([deleted] ASC, [cli_mnem] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON);


GO
CREATE NONCLUSTERED INDEX [ix deleted, cli_nme]
    ON [dbo].[client]([deleted] ASC, [cli_nme] ASC) WITH (FILLFACTOR = 90, PAD_INDEX = ON);


GO
CREATE NONCLUSTERED INDEX [IX_bill_to_client]
    ON [dbo].[client]([bill_to_client] ASC, [cli_mnem] ASC) WITH (FILLFACTOR = 90);


GO
/****** Object:  Trigger dbo.tu_client    Script Date: 9/19/2001 10:37:43 AM ******/
CREATE TRIGGER [dbo].[tu_client] ON dbo.client 
FOR UPDATE 
AS
UPDATE client
SET client.mod_user = suser_sname(), client.mod_date = getdate(), client.mod_prg = app_name()
FROM inserted,client
WHERE inserted.cli_mnem = client.cli_mnem

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 12/08/2008
-- Description:	Track the modifications of the AMT table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_CLIENT] 
   ON  [dbo].[client] 
   AFTER INSERT,UPDATE, DELETE
AS 

DECLARE @mod_date datetime
set @mod_date = getdate()
DECLARE @mod_host varchar(50)
set @mod_host = RIGHT (HOST_NAME(),50)
DECLARE @mod_prg varchar(50)
set @mod_prg = RIGHT(ISNULL(OBJECT_NAME(@@PROCID),'SQL QUERY'),50)
DECLARE @mod_user varchar(50)
DECLARE @mod_indicator varchar(10)
set @mod_indicator = null
DECLARE @deleted int
set @deleted = 0
DECLARE @inserted int
set @inserted = 0
DECLARE @client numeric(10,0)
set @client = null

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

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
		insert into audit_client
			(deleted, audit_cli_mnem, cli_nme, addr_1, addr_2, city, st, zip, 
					phone, fax, contact, comment, prn_cpt4, 
					per_disc, date_ord, print_cc, bill_at_disc, do_not_bill, 
                      type, last_invoice, last_invoice_date, last_discount, mod_date, mod_user, mod_prg, mod_host, mro_name, mro_addr1, mro_addr2, mro_city, mro_st, 
                      mro_zip, prn_loc, route, county, email, late_notice, late_notice_date, statsFacility, fee_schedule, commission, client_class, client_maint_rep, 
                      client_sales_rep, mod_indicator, bill_pc_charges) 
		select
			ins.deleted, ins.cli_mnem, ins.cli_nme, ins.addr_1, ins.addr_2, ins.city, ins.st, ins.zip, 
				ins.phone, ins.fax, ins.contact, ins.comment, ins.prn_cpt4, 
				ins.per_disc, ins.date_ord, ins.print_cc, ins.bill_at_disc, ins.do_not_bill, 
                ins.type, ins.last_invoice, ins.last_invoice_date, ins.last_discount, 
				@mod_date, @mod_user, @mod_prg, @mod_host, 
				ins.mro_name, ins.mro_addr1, ins.mro_addr2, ins.mro_city, ins.mro_st, 
                ins.mro_zip, ins.prn_loc, ins.route, ins.county, ins.email, ins.late_notice, 
				ins.late_notice_date, ins.statsFacility, ins.fee_schedule, ins.commission, 
				ins.client_class, ins.client_maint_rep, ins.client_sales_rep,
				@mod_indicator, ins.bill_pc_charges
		from inserted ins

		-- add mapping entries for the new clients
		insert dictionary.mapping (return_value, return_value_type, sending_system, sending_value)
		select inserted.cli_mnem, 'CLIENT', 'CERNER', inserted.cli_mnem 
		from inserted left outer join dictionary.mapping map on inserted.cli_mnem = map.sending_value
		where map.sending_value is null

	end
	
	if (@inserted > 0 and @deleted > 0) -- modification
	begin
		set @mod_indicator = 'MUD'
		insert into audit_client
			(deleted, audit_cli_mnem, cli_nme, addr_1, addr_2, city, st, zip, 
					phone, fax, contact, comment, prn_cpt4, 
					per_disc, date_ord, print_cc, bill_at_disc, do_not_bill, 
                      type, last_invoice, last_invoice_date, last_discount, mod_date, mod_user, mod_prg, mod_host, mro_name, mro_addr1, mro_addr2, mro_city, mro_st, 
                      mro_zip, prn_loc, route, county, email, late_notice, late_notice_date, statsFacility, fee_schedule, commission, client_class, client_maint_rep, 
                      client_sales_rep, mod_indicator, bill_pc_charges)
		select
			del.deleted, del.cli_mnem, del.cli_nme, del.addr_1, del.addr_2, del.city, del.st, del.zip, 
				del.phone, del.fax, del.contact, del.comment, del.prn_cpt4, 
				del.per_disc, del.date_ord, del.print_cc, del.bill_at_disc, del.do_not_bill, 
                del.type, del.last_invoice, del.last_invoice_date, del.last_discount, 
				@mod_date, @mod_user, @mod_prg, @mod_host, 
				del.mro_name, del.mro_addr1, del.mro_addr2, del.mro_city, del.mro_st, 
                del.mro_zip, del.prn_loc, del.route, del.county, del.email, del.late_notice, 
				del.late_notice_date, del.statsFacility, del.fee_schedule, del.commission, 
				del.client_class, del.client_maint_rep, del.client_sales_rep,
				@mod_indicator, del.bill_pc_charges
		from deleted del

		set @mod_indicator = 'MUI'
		insert into audit_client
			(deleted, audit_cli_mnem, cli_nme, addr_1, addr_2, city, st, zip, 
					phone, fax, contact, comment, prn_cpt4, 
					per_disc, date_ord, print_cc, bill_at_disc, do_not_bill, 
                      type, last_invoice, last_invoice_date, last_discount, mod_date, mod_user, mod_prg, mod_host, mro_name, mro_addr1, mro_addr2, mro_city, mro_st, 
                      mro_zip, prn_loc, route, county, email, late_notice, late_notice_date, statsFacility, fee_schedule, commission, client_class, client_maint_rep, 
                      client_sales_rep, mod_indicator, bill_pc_charges)
		select
			ins.deleted, ins.cli_mnem, ins.cli_nme, ins.addr_1, ins.addr_2, ins.city, ins.st, ins.zip, 
				ins.phone, ins.fax, ins.contact, ins.comment, ins.prn_cpt4, 
				ins.per_disc, ins.date_ord, ins.print_cc, ins.bill_at_disc, ins.do_not_bill, 
                ins.type, ins.last_invoice, ins.last_invoice_date, ins.last_discount, 
				@mod_date, @mod_user, @mod_prg, @mod_host, 
				ins.mro_name, ins.mro_addr1, ins.mro_addr2, ins.mro_city, ins.mro_st, 
                ins.mro_zip, ins.prn_loc, ins.route, ins.county, ins.email, ins.late_notice, 
				ins.late_notice_date, ins.statsFacility, ins.fee_schedule, ins.commission, 
				ins.client_class, ins.client_maint_rep, ins.client_sales_rep,
				@mod_indicator, bill_pc_charges
		from inserted ins
	end
	
	if (@inserted = 0 and @deleted > 0) -- we are deleting
	begin
	
	select @client = audit.audit_cli_mnem
		from	 audit_client audit inner join deleted d on audit.audit_cli_mnem = d.cli_mnem

	if (@client is NULL) -- copy the original record first
		begin
			insert into audit_client
			(deleted, audit_cli_mnem, cli_nme, addr_1, addr_2, city, st, zip, 
					phone, fax, contact, comment, prn_cpt4, 
					per_disc, date_ord, print_cc, bill_at_disc, do_not_bill, 
                      type, last_invoice, last_invoice_date, last_discount, mod_date, mod_user, mod_prg, mod_host, mro_name, mro_addr1, mro_addr2, mro_city, mro_st, 
                      mro_zip, prn_loc, route, county, email, late_notice, late_notice_date, statsFacility, fee_schedule, commission, client_class, client_maint_rep, 
                      client_sales_rep, mod_indicator, bill_pc_charges)
			select
			del.deleted, del.cli_mnem, del.cli_nme, del.addr_1, del.addr_2, del.city, del.st, del.zip, 
				del.phone, del.fax, del.contact, del.comment, del.prn_cpt4, 
				del.per_disc, del.date_ord, del.print_cc, del.bill_at_disc, del.do_not_bill, 
                del.type, del.last_invoice, del.last_invoice_date, del.last_discount, 
				@mod_date, @mod_user, 'D~'+@mod_prg, @mod_host, 
				del.mro_name, del.mro_addr1, del.mro_addr2, del.mro_city, del.mro_st, 
                del.mro_zip, del.prn_loc, del.route, del.county, del.email, del.late_notice, 
				del.late_notice_date, del.statsFacility, del.fee_schedule, del.commission, 
				del.client_class, del.client_maint_rep, del.client_sales_rep,
				'CO', del.bill_pc_charges

			from deleted del
		end
	
	-- now copy with the audit info for who deleted it
		insert into audit_client
			(deleted, audit_cli_mnem, cli_nme, addr_1, addr_2, city, st, zip, 
					phone, fax, contact, comment, prn_cpt4, 
					per_disc, date_ord, print_cc, bill_at_disc, do_not_bill, 
                      type, last_invoice, last_invoice_date, last_discount, mod_date, mod_user, mod_prg, mod_host, mro_name, mro_addr1, mro_addr2, mro_city, mro_st, 
                      mro_zip, prn_loc, route, county, email, late_notice, late_notice_date, statsFacility, fee_schedule, commission, client_class, client_maint_rep, 
                      client_sales_rep, mod_indicator, bill_pc_charges)
			select
			del.deleted, del.cli_mnem, del.cli_nme, del.addr_1, del.addr_2, del.city, del.st, del.zip, 
				del.phone, del.fax, del.contact, del.comment, del.prn_cpt4, 
				del.per_disc, del.date_ord, del.print_cc, del.bill_at_disc, del.do_not_bill, 
                del.type, del.last_invoice, del.last_invoice_date, del.last_discount, 
				@mod_date, @mod_user, @mod_prg, @mod_host, 
				del.mro_name, del.mro_addr1, del.mro_addr2, del.mro_city, del.mro_st, 
                del.mro_zip, del.prn_loc, del.route, del.county, del.email, del.late_notice, 
				del.late_notice_date, del.statsFacility, del.fee_schedule, del.commission, 
				del.client_class, del.client_maint_rep, del.client_sales_rep,
				'DUD', del.bill_pc_charges
				
			from deleted del
	
	end 		
END

GO
-- =============================================
-- Author:		David
-- Create date: 9/3/2013
-- Description:	If GL CODE IS not entered send and email
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_GL_CODE] 
   ON  [dbo].[client] 
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @emailbody varchar(8000)
    -- Insert statements for trigger here
	if (update(gl_code))
	
	begin
--format the email body
		SET @emailbody = N'New Client info for ' + (select cli_mnem from inserted);

		--send the email
		EXEC msdb.dbo.sp_send_dbmail
			@recipients=N'david.kelly@wth.org',--'bradley.powers@wth.org;christopher.burton@wth.org;david.kelly@wth.org',  
			@body=@emailbody,
			@body_format = 'HTML',
			@subject ='CLIENT GL_CODE NEEDED',
			@profile_name ='WTHMCLBILL';
		
			INSERT INTO dictionary.mapping
					(
						return_value ,
						return_value_type ,
						sending_system ,
						sending_value ,
						mod_date
					)
			SELECT UPPER(INSERTED.cli_mnem),'CLIENT','CERNER'
				, UPPER(INSERTED.cli_mnem), GETDATE()
			FROM INSERTED
	END
	

END

GO
DISABLE TRIGGER [dbo].[TRIGGER_GL_CODE]
    ON [dbo].[client];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'09/11/2008 rgc/wdk changed default to 1 for CBILL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client', @level2type = N'COLUMN', @level2name = N'bill_at_disc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'09/03/2008 wdk added for special handling of new vs old fee schedules tables for cdm and others will have to be created.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client', @level2type = N'COLUMN', @level2name = N'fee_schedule';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'rgc/wdk 20120229 added new field to allow new billing method for WTH owned clinics.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client', @level2type = N'COLUMN', @level2name = N'outpatient_billing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140505 to determine if the client is billed pc charges ''YES'' or ''NO'',''ONLY'' ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'client', @level2type = N'COLUMN', @level2name = N'bill_pc_charges';

