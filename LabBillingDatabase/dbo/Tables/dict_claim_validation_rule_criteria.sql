CREATE TABLE [dbo].[dict_claim_validation_rule_criteria]
(
	[RuleId] INT NOT NULL , 
    [LineType] NVARCHAR(50) NULL, 
    [GroupId] INT NULL, 
    [ParentGroupId] INT NULL, 
    [Class] NVARCHAR(50) NULL,
    [MemberName] NVARCHAR(50) NULL, 
    [Operator] NVARCHAR(50) NULL, 
    [TargetValue] NVARCHAR(50) NULL, 
    [RuleCriteriaId] INT NOT NULL IDENTITY, 
    CONSTRAINT [PK_dict_claim_validation_rule_criteria] PRIMARY KEY ([RuleCriteriaId]),
    [mod_user] NVARCHAR(100) NULL DEFAULT suser_sname(), 
    [mod_prg] NVARCHAR(100) NULL DEFAULT app_name(), 
    [mod_date] DATETIME NULL DEFAULT getdate(), 
    [mod_host] NVARCHAR(100) NULL DEFAULT host_name()
)
