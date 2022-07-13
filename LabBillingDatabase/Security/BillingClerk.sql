CREATE ROLE [BillingClerk]
    AUTHORIZATION [dbo];


GO
ALTER ROLE [BillingClerk] ADD MEMBER [WTHMC\Pathology];


GO
ALTER ROLE [BillingClerk] ADD MEMBER [WTHMC\Outreach];


GO
ALTER ROLE [BillingClerk] ADD MEMBER [WTHMC\Outreach Billing];


GO
ALTER ROLE [BillingClerk] ADD MEMBER [WTHMC\mclbill];


GO
ALTER ROLE [BillingClerk] ADD MEMBER [WTHMC\LISANALYSTS];

