CREATE TABLE [dbo].[acc_validation_status]
(
	[account] VARCHAR(15) NOT NULL PRIMARY KEY, 
    [validation_text] VARCHAR(MAX) NULL, 
    [mod_date] DATETIME NULL DEFAULT getdate(), 
    [mod_user] VARCHAR(100) NULL DEFAULT suser_sname(), 
    [mod_prg] VARCHAR(100) NULL DEFAULT app_name(), 
    [mod_host] VARCHAR(100) NULL DEFAULT host_name()
)
