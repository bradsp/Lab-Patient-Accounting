CREATE TABLE [dbo].[chrg_err] (
    [account]         VARCHAR (15)  NOT NULL,
    [pat_name]        VARCHAR (40)  NULL,
    [cl_mnem]         VARCHAR (10)  NULL,
    [fin_code]        VARCHAR (10)  NULL,
    [cdm]             VARCHAR (7)   NULL,
    [cpt4]            VARCHAR (5)   NULL,
    [amount]          MONEY         NULL,
    [trans_date]      DATETIME      NULL,
    [service_date]    DATETIME      NULL,
    [qty]             INT           NULL,
    [type]            VARCHAR (50)  NULL,
    [error]           VARCHAR (100) NULL,
    [uri]             NUMERIC (15)  IDENTITY (1, 1) NOT NULL,
    [deleted]         BIT           CONSTRAINT [DF_chrg_err_deleted_2__11] DEFAULT ((0)) NOT NULL,
    [mt_reqno]        VARCHAR (50)  NULL,
    [location]        VARCHAR (50)  NULL,
    [performing_site] VARCHAR (50)  NULL,
    [mod_date]        DATETIME      CONSTRAINT [DF_chrg_err_mod_date] DEFAULT (getdate()) NULL,
    [mod_prg]         VARCHAR (50)  CONSTRAINT [DF_chrg_err_mod_prg] DEFAULT (app_name()) NULL,
    [mod_user]        VARCHAR (50)  CONSTRAINT [DF_chrg_err_mod_user] DEFAULT (suser_sname()) NULL,
    [mod_host]        VARCHAR (50)  CONSTRAINT [DF_chrg_err_mod_host] DEFAULT (host_name()) NULL,
    CONSTRAINT [PK_chrg_err_1__11] PRIMARY KEY CLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [account_cdx]
    ON [dbo].[chrg_err]([account] ASC) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		Rick and David
-- Create date: 12/20/2011
-- Description:	Track who is deleting records from this table.
-- =============================================
CREATE TRIGGER [dbo].[chrg_err_delete] 
   ON  dbo.chrg_err 
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	insert into audit_chrg_err
	(account, pat_name, cl_mnem, fin_code, cdm, cpt4, amount, trans_date, service_date, qty, type, error, uri, deleted, mt_reqno, location
	, performing_site, mod_date, 
                      mod_prg, mod_user, mod_host)
	select
	  I.account, I.pat_name, I.cl_mnem, I.fin_code, I.cdm, I.cpt4, I.amount, I.trans_date, I.service_date, 
		I.qty, I.type, I.error, I.uri, I.deleted, I.mt_reqno, I.location,I.performing_site,
		getdate(), 'D~'+app_name(), suser_sname(), host_name()
	from deleted I


END

GO
-- =============================================
-- Author:		David
-- Create date: 8/29/2011
-- Description:	Insert records into a tracking table
-- =============================================
CREATE TRIGGER [dbo].[trigger_insert] 
   ON  dbo.chrg_err 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	insert into audit_chrg_err
	(account, pat_name, cl_mnem, fin_code, cdm, cpt4, amount, trans_date, service_date, qty, type
	, error, uri, deleted, mt_reqno, location, performing_site,mod_date, 
                      mod_prg, mod_user, mod_host)
	select
	  I.account, I.pat_name, I.cl_mnem, I.fin_code, I.cdm, I.cpt4, I.amount, I.trans_date, I.service_date, 
		I.qty, I.type, I.error, I.uri, I.deleted, I.mt_reqno, I.location, I.performing_site,I.mod_date, 
                      RIGHT(I.mod_prg,50), RIGHT(I.mod_user,50), RIGHT(I.mod_host,50)
	from Inserted I

END
