CREATE ROLE [BillingDictionaryEdit]
    AUTHORIZATION [dbo];


GO
ALTER ROLE [BillingDictionaryEdit] ADD MEMBER [WTHMC\Outreach Billing];


GO
ALTER ROLE [BillingDictionaryEdit] ADD MEMBER [WTHMC\mclbill];


GO
ALTER ROLE [BillingDictionaryEdit] ADD MEMBER [WTHMC\LISANALYSTS];

