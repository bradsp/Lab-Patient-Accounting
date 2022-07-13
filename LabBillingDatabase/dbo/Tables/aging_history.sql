CREATE TABLE [dbo].[aging_history] (
    [account]   VARCHAR (15) NOT NULL,
    [datestamp] DATETIME     NOT NULL,
    [balance]   MONEY        NULL,
    [fin_code]  VARCHAR (10) NULL,
    [ins_code]  VARCHAR (10) NULL,
    [mod_date]  DATETIME     CONSTRAINT [DF_aging_history_mod_date] DEFAULT (getdate()) NULL,
    [mod_user]  VARCHAR (50) CONSTRAINT [DF__aging_his__mod_u__76625E5B] DEFAULT (right(suser_sname(),(50))) NULL,
    [mailer]    VARCHAR (1)  NULL,
    CONSTRAINT [PK_aging_history] PRIMARY KEY CLUSTERED ([account] ASC, [datestamp] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_datestamp INCLUDE account, balance]
    ON [dbo].[aging_history]([datestamp] ASC, [fin_code] ASC)
    INCLUDE([account], [balance], [ins_code], [mod_date], [mod_user], [mailer]) WITH (FILLFACTOR = 90);


GO
CREATE STATISTICS [_dta_stat_2021582240_7]
    ON [dbo].[aging_history]([mod_user]);


GO
-- =============================================
-- Author:		David Kelly
-- Create date: 12/3/2008
-- Description:	Tracks the mod data for the aging_history table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_DELETE] 
   ON  [dbo].[aging_history] 
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	-- record was deleted 
	insert into audit_aging_history
		  (account, datestamp, balance, fin_code, ins_code, mod_indicator, mod_date, mod_user, mod_prg, mod_host)
	select d.account, d.datestamp, d.balance, d.fin_code, d.ins_code, 'D', getdate(), suser_sname(), RIGHT(app_name(),50), host_name()
	from deleted d
		

END

GO
-- =============================================
-- Author:		David Kelly
-- Create date: 12/3/2008
-- Description:	Tracks the mod data for the aging_history table
-- =============================================
CREATE TRIGGER [dbo].[TRIGGER_INSERT_UPDATE] 
   ON  [dbo].[aging_history] 
   AFTER UPDATE, INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	if (columns_updated() & 7 ) > 0
	BEGIN
		-- record has changed put the old record into the table with the new mod info
		insert into audit_aging_history
			  (account, datestamp, balance,fin_code, ins_code, mod_indicator, mod_date, mod_user, mod_prg, mod_host)
		select d.account, d.datestamp, d.balance, d.fin_code, d.ins_code, 'MUD', getdate(), suser_sname(), RIGHT(app_name(),50), host_name()
		from deleted d
		
		DECLARE @acc varchar(15);
		set @acc = (select del.account from deleted del)
		if Len(@acc) > 0
			BEGIN
			-- also put the new information into the audit table
			insert into audit_aging_history
				  (account, datestamp, balance, fin_code, ins_code, mod_indicator, mod_date, mod_user, mod_prg, mod_host)
			select i.account, i.datestamp, i.balance, i.fin_code, i.ins_code,'MUI', getdate(), suser_sname(), RIGHT(app_name(),50), host_name()
			from inserted i
			END
		else
		BEGIN
		insert into audit_aging_history
			  (account, datestamp, balance, fin_code, ins_code, mod_indicator, mod_date, mod_user, mod_prg, mod_host)
		select i.account, i.datestamp, i.balance, i.fin_code, i.ins_code,'I', getdate(), suser_sname(), RIGHT(app_name(),50), host_name()
		from inserted i
		end
	END

END

GO
DISABLE TRIGGER [dbo].[TRIGGER_INSERT_UPDATE]
    ON [dbo].[aging_history];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100712 Added to collect fin_code on last day of month', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'aging_history', @level2type = N'COLUMN', @level2name = N'fin_code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'wdk 20100712 Added to collect ins_code on last day of month', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'aging_history', @level2type = N'COLUMN', @level2name = N'ins_code';

