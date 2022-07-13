CREATE TABLE [dbo].[amt] (
    [chrg_num]           NUMERIC (15) NOT NULL,
    [cpt4]               VARCHAR (5)  NULL,
    [type]               VARCHAR (6)  NULL,
    [amount]             MONEY        NULL,
    [mod_date]           DATETIME     CONSTRAINT [DF_amt_mod_date_3__12] DEFAULT (getdate()) NULL,
    [mod_user]           VARCHAR (50) CONSTRAINT [DF_amt_mod_user_1__10] DEFAULT (right(suser_sname(),(50))) NULL,
    [mod_prg]            VARCHAR (50) CONSTRAINT [DF_amt_mod_prg_4__12] DEFAULT (right(app_name(),(50))) NULL,
    [deleted]            BIT          CONSTRAINT [DF_amt_deleted_2__12] DEFAULT ((0)) NOT NULL,
    [uri]                NUMERIC (15) IDENTITY (100, 1) NOT NULL,
    [modi]               VARCHAR (5)  NULL,
    [revcode]            VARCHAR (5)  NULL,
    [modi2]              VARCHAR (5)  NULL,
    [diagnosis_code_ptr] VARCHAR (50) NULL,
    [mt_req_no]          VARCHAR (50) NULL,
    [order_code]         VARCHAR (7)  NULL,
    [bill_type]          VARCHAR (50) NULL,
    [bill_method]        VARCHAR (50) NULL,
    [pointer_set]        BIT          CONSTRAINT [df_col_diag_code_ptr_set] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_amt_1__10] PRIMARY KEY NONCLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE CLUSTERED INDEX [chrg_num_cdx]
    ON [dbo].[amt]([chrg_num] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_mod_date, type INCLUDE chrg_num, amount]
    ON [dbo].[amt]([mod_date] ASC, [type] ASC)
    INCLUDE([chrg_num], [amount], [cpt4]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [ix_cpt4_include_chrg_num]
    ON [dbo].[amt]([cpt4] ASC)
    INCLUDE([chrg_num]) WITH (FILLFACTOR = 90);


GO
-- =============================================
-- Author:		David
-- Create date: 08/20/2013
-- Description:	Updates the account if the CDM inserted if for a BNP
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_BNP] 
   ON  [dbo].[amt] 
   AFTER INSERT--,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	-- THIS DOES NOT WORK SEE THE BNP's JOB IN THE DAILY UPDATE JOBS












/*
	declare @rowguid uniqueidentifier
	set @rowguid = newid();
	declare @chrgnumOrig numeric(15,0)
	declare @chrgnumInsert numeric(15,0)
	declare @cdm varchar(7), 
		@account varchar(15), 
		@fincode varchar(10),
		@inserted int, 
		@deleted int, 
		@credited bit,
		@qty int,
		@app varchar(50)

	select @chrgnumInsert = (select top(1) i.chrg_num	from inserted i)
	select @cdm = (select top(1) cdm from chrg 
					inner join inserted on inserted.chrg_num = chrg.chrg_num)


	select @inserted = (select count(*) from inserted)
	select @deleted  = (select count(*) from deleted)
	

	select @qty = (select top(1) qty from chrg where chrg_num = @chrgnumInsert)
	select @app = (select top(1) mod_prg from chrg where chrg_num = @chrgnumInsert)
	if (@qty < 0 )--and not (@app like '%chrg_err%' or @app like '%PostChrg%'))
	begin
		return;
	end

	select @fincode = (select top(1) fin_code from acc where account in (select account from chrg where chrg_num = @chrgnumInsert))
	if (@fincode <> 'A')
	begin
		return;
	end


if (@inserted > 0 and @deleted = 0) -- first insert 
	begin

--	declare @strBody varchar(256)
--	select @strBody = stuff(convert(varchar(10),@cdm) ,len(@cdm),0, convert(varchar(10), @chrgnumInsert))
--	EXEC msdb.dbo.sp_send_dbmail
--	@recipients = N'david.kelly@wth.org',
--	@body= @strBody,
--	@body_format = 'HTML',
--	@subject = 'trigger_bnp insert',
--	@profile_name = 'Outlook';
		
	if (@cdm in ('5325048','5325094','5322126','RL00181'))
		begin
			--RAISERROR(@qty,16,1)
--		insert into chrg (rowguid,account,qty,credited,cdm,retail,inp_price,net_amt,mod_prg,comment)
--		select @rowguid,account,qty*-1,1,cdm,retail,inp_price,net_amt, 'TRIGGER_BNP',
--		'BNP Credited per Ed Hughes and Patricia Puckett'
--		from chrg where chrg_num = @chrgnumInsert

--		set @chrgnumOrig = (select chrg_num from chrg where rowguid = @rowguid)


		UPDATE    chrg
		SET credited = 1,
			net_amt = 0
		,comment = 'BNP no charge per Ed Hughes and Patricia Puckett'
		FROM  chrg 
		where chrg_num = @chrgnumInsert
		
--		insert into amt (chrg_num, cpt4,[type],amount,modi,revcode,modi2,diagnosis_code_ptr,
--				mod_date, mod_user, mod_prg)
--		select @chrgnumOrig, cpt4, [type], amount, modi,revcode,modi2,diagnosis_code_ptr,
--				getdate(), suser_sname(), 'TRIGGER_BNP'
--		from inserted
		update amt
		set amount = 0.0000
		from amt
		where chrg_num = @chrgnumInsert
	

		insert into notes (account,comment,mod_prg)
		select account,'BNP no charge per Ed Hughes and Patricia Puckett','TRIGGER_BNP'
		from chrg where chrg.rowguid = @rowguid
		
		end
	end	
*/		 

END

GO
DISABLE TRIGGER [dbo].[TRIGGER_BNP]
    ON [dbo].[amt];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20140220 Length changed from 30 to 50', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'mod_user';


GO
EXECUTE sp_addextendedproperty @name = N'Caption', @value = '20090119 wdk shows relation between cpt4 and diagnosis', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'diagnosis_code_ptr';


GO
EXECUTE sp_addextendedproperty @name = N'Description', @value = '20090119 wdk shows relation between cpt4 and diagnosis', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'diagnosis_code_ptr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404018 set default', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'diagnosis_code_ptr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130526 Added to flatten charges into the amt table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'mt_req_no';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130526 Added to flatten charges into the amt table using order_code to prevent interference with the entire billing system.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'order_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130526 Added to flatten charges into the amt table. Bill_type will be used by fee scheudles.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'bill_type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20130808 to split location where charges can be billed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'bill_method';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 201404018 set default', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'amt', @level2type = N'COLUMN', @level2name = N'pointer_set';

