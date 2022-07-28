CREATE TABLE [dbo].[dict_claim_validation_rules]
(
	[RuleId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RuleName] NVARCHAR(80) NULL, 
    [Description] NVARCHAR(100) NULL, 
    [ErrorText] NVARCHAR(100) NULL, 
    [EffectiveDate] DATE NULL, 
    [EndEffectiveDate] DATE NULL, 
    [mod_user] NVARCHAR(100) NULL DEFAULT suser_sname(), 
    [mod_prg] NVARCHAR(100) NULL DEFAULT app_name(), 
    [mod_date] DATETIME NULL DEFAULT getdate(), 
    [mod_host] NVARCHAR(100) NULL DEFAULT host_name()
)

GO
