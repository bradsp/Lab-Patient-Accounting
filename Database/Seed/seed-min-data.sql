/* ============================================================================
   seed-min-data.sql  --  Minimum dev/test seed data for Lab Patient Accounting
   ----------------------------------------------------------------------------
   PURPOSE
     Populates the two tables the application requires before it can be used:
       * [system]  - key/value application parameters (PetaPoco model SysParameter,
                     materialized into ApplicationParameters at startup via
                     SystemService.LoadSystemParameters / SystemParametersRepository).
       * [emp]     - user accounts (PetaPoco model UserAccount). The Blazor
                     DatabaseUser authorization policy and the WinForms/service auth
                     paths require a matching, access-enabled row here to log in.

   TARGET
     A schema-complete billing database (e.g. LabBillingTest on the dockerized
     SQL Server). Tables [system] and [emp] must already exist.

   PROPERTIES
     * Idempotent / re-runnable: keyed on the natural keys
       ([system].KeyName, [emp].name) via MERGE / IF NOT EXISTS.
       It never TRUNCATEs or DELETEs and never overwrites an existing row's value.
     * DEV/TEST ONLY. All values are synthetic. NO PHI. Do not run in production.

   HOW TO RUN (against the docker container)
     docker exec mssql_server /opt/mssql-tools18/bin/sqlcmd \
       -S localhost -U sysapp -P '<DatabasePassword>' \
       -d LabBillingTest -C -i Database/Seed/seed-min-data.sql
     (The sysapp password is in LabOutreachUI/appsettings.json -> AppSettings:DatabasePassword.
      It is intentionally NOT hardcoded in this script.)

   PARAMETER DERIVATION
     The [system] rows below are the COMPLETE set of public properties of
     LabBilling.Core.Models.ApplicationParameters (all 13 ApplicationParameters-*.cs
     partials), 188 in total. At startup LoadParameters() iterates these properties
     and looks each up in [system] by the KeyName column (= the property name), then
     parses [value] into the property's .NET type. Each row here therefore carries a
     type-safe value so parsing never throws and the app does not silently fall back
     to defaults. Values are grouped/commented by their source partial.

     NOTE on key_name vs KeyName: the physical primary key column is key_name
     (varchar(25)); the app looks up by KeyName (varchar(50)). For property names
     longer than 25 chars, key_name holds a 25-char prefix (it is not used for
     lookups and the prefixes are collision-free) while KeyName holds the full,
     exact property name the app actually queries.
   ============================================================================ */

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;  -- required for the XML base64 cast used to hash the seed password
GO

PRINT '--- Seeding [system] application parameters ---';
GO

/* MERGE on KeyName (the column the app queries). Insert missing rows only;
   do not overwrite an existing parameter's value on re-run. */
MERGE [dbo].[system] AS tgt
USING
(
    VALUES
        -- Category: Accounting (ApplicationParameters-Accounting.cs)
        (N'BankAccountNumber', N'BankAccountNumber', N'', N'Accounting', N'String'),
        (N'BankName', N'BankName', N'', N'Accounting', N'String'),
        (N'BankRoutingNumber', N'BankRoutingNumber', N'', N'Accounting', N'String'),
        -- Category: Billing (ApplicationParameters-Billing.cs)
        (N'AuthorizedToRunClientBill', N'AuthorizedToRunClientBills', N'brads|admin', N'Billing', N'String'),  -- dev: allow seeded users to run client bills
        (N'BHGroupNo', N'BHGroupNo', N'', N'Billing', N'String'),
        (N'BHProviderNo', N'BHProviderNo', N'', N'Billing', N'String'),
        (N'BillingInitialHoldDays', N'BillingInitialHoldDays', N'10', N'Billing', N'Int32'),  -- DefaultValue(10)
        (N'BillingReceiverId', N'BillingReceiverId', N'', N'Billing', N'String'),
        (N'BillingReceiverName', N'BillingReceiverName', N'', N'Billing', N'String'),
        (N'BlueCrossProvNo', N'BlueCrossProvNo', N'', N'Billing', N'String'),
        (N'BlueCrossReceiverNo', N'BlueCrossReceiverNo', N'', N'Billing', N'String'),
        (N'BundleInsurance', N'BundleInsurance', N'', N'Billing', N'String'),
        (N'ChampusGroupNo', N'ChampusGroupNo', N'', N'Billing', N'String'),
        (N'CNetReceiverId', N'CNetReceiverId', N'', N'Billing', N'String'),
        (N'CodeStats', N'CodeStats', N'', N'Billing', N'String'),
        (N'CollectionsRun', N'CollectionsRun', N'1900-01-01', N'Billing', N'DateTime'),
        (N'DBMailProfile', N'DBMailProfile', N'', N'Billing', N'String'),
        (N'Diagnosis', N'Diagnosis', N'false', N'Billing', N'Boolean'),
        (N'DiagnosisCodePointerSelec', N'DiagnosisCodePointerSelect', N'true', N'Billing', N'String'),  -- DefaultValue(true)
        (N'ElectronicBillDate', N'ElectronicBillDate', N'1900-01-01', N'Billing', N'DateTime'),
        (N'EobAddress', N'EobAddress', N'', N'Billing', N'String'),
        (N'GlobalBillingStartDate', N'GlobalBillingStartDate', N'1900-01-01', N'Billing', N'DateTime'),
        (N'InstitutionalClaimFileLoc', N'InstitutionalClaimFileLocation', N'C:\LabBilling\claims\837i', N'Billing', N'String'),  -- dev path
        (N'MaxClaimsInClaimBatch', N'MaxClaimsInClaimBatch', N'0', N'Billing', N'Int32'),  -- 0=unlimited
        (N'MedicaidProviderId', N'MedicaidProviderId', N'', N'Billing', N'String'),
        (N'MedicareBProviderNo', N'MedicareBProviderNo', N'', N'Billing', N'String'),
        (N'MedicareElectronicReceive', N'MedicareElectronicReceiverId', N'', N'Billing', N'String'),
        (N'MedicareProviderNo', N'MedicareProviderNo', N'', N'Billing', N'String'),
        (N'MedicareReceiverId', N'MedicareReceiverId', N'', N'Billing', N'String'),
        (N'MedicareSubmitterId', N'MedicareSubmitterId', N'', N'Billing', N'String'),
        (N'NursingHomeBillThruDate', N'NursingHomeBillThruDate', N'', N'Billing', N'String'),
        (N'OutpatientBillStart', N'OutpatientBillStart', N'1900-01-01', N'Billing', N'DateTime'),
        (N'ProfessionalClaimFileLoca', N'ProfessionalClaimFileLocation', N'C:\LabBilling\claims\837p', N'Billing', N'String'),  -- dev path
        (N'ProfessionalClaimStartDat', N'ProfessionalClaimStartDate', N'1900-01-01', N'Billing', N'DateTime'),
        (N'ProviderTaxonomyCode', N'ProviderTaxonomyCode', N'282N00000X', N'Billing', N'String'),  -- DefaultValue
        (N'RemitImportDirectory', N'RemitImportDirectory', N'C:\LabBilling\remit\import', N'Billing', N'String'),  -- dev path
        (N'RemitPostingDate', N'RemitPostingDate', N'1900-01-01', N'Billing', N'DateTime'),
        (N'RemitProcessingDirectory', N'RemitProcessingDirectory', N'C:\LabBilling\remit\processing', N'Billing', N'String'),  -- dev path
        (N'RemitDocumentDirectory', N'RemitDocumentDirectory', N'C:\LabBilling\remit\835pdf', N'Billing', N'String'),  -- dev path
        (N'SecondaryBilling', N'SecondaryBilling', N'1900-01-01', N'Billing', N'DateTime'),
        (N'SmallBalanceAmount', N'SmallBalanceAmount', N'0', N'Billing', N'Double'),
        (N'SSIStartDate', N'SSIStartDate', N'1900-01-01', N'Billing', N'DateTime'),
        (N'TLCProviderId', N'TLCProviderId', N'', N'Billing', N'String'),
        (N'UHCProviderId', N'UHCProviderId', N'', N'Billing', N'String'),
        (N'UseBillMethod', N'UseBillMethod', N'0', N'Billing', N'Int32'),
        -- Category: Charging (ApplicationParameters-Charging.cs)
        (N'FeeSchedules', N'FeeSchedules', N'', N'Charging', N'String'),
        (N'GeneralHealthPanelInsuran', N'GeneralHealthPanelInsurances', N'', N'Charging', N'String'),
        (N'GeneralHealthPanelTests', N'GeneralHealthPanelTests', N'', N'Charging', N'String'),
        (N'OBPanelInsurances', N'OBPanelInsurances', N'', N'Charging', N'String'),
        (N'OBPanelTests', N'OBPanelTests', N'', N'Charging', N'String'),
        (N'QuestBillingEmailRecipien', N'QuestBillingEmailRecipients', N'', N'Charging', N'String'),
        -- Category: Collections (ApplicationParameters-Collections.cs)
        (N'CollectionsFileLocation', N'CollectionsFileLocation', N'C:\LabBilling\collections', N'Collections', N'String'),  -- dev path
        (N'CollectionsSftpPassword', N'CollectionsSftpPassword', N'', N'Collections', N'String'),
        (N'CollectionsSftpServer', N'CollectionsSftpServer', N'', N'Collections', N'String'),
        (N'CollectionsSftpUsername', N'CollectionsSftpUsername', N'', N'Collections', N'String'),
        (N'CollectionsSftpUploadPath', N'CollectionsSftpUploadPath', N'', N'Collections', N'String'),
        (N'StatementsFileLocation', N'StatementsFileLocation', N'C:\LabBilling\statements', N'Collections', N'String'),  -- dev path
        (N'StatementsSftpPassword', N'StatementsSftpPassword', N'', N'Collections', N'String'),
        (N'StatementsSftpServer', N'StatementsSftpServer', N'', N'Collections', N'String'),
        (N'StatementsSftpUsername', N'StatementsSftpUsername', N'', N'Collections', N'String'),
        (N'StatementsSftpUploadPath', N'StatementsSftpUploadPath', N'', N'Collections', N'String'),
        (N'NumberOfStatementsBeforeC', N'NumberOfStatementsBeforeCollection', N'3', N'Collections', N'Int32'),  -- dev sensible default
        -- Category: Company (ApplicationParameters-Company.cs)
        (N'BillingContact', N'BillingContact', N'', N'Company', N'String'),
        (N'BillingEntityCounty', N'BillingEntityCounty', N'', N'Company', N'String'),
        (N'BillingEmail', N'BillingEmail', N'', N'Company', N'String'),
        (N'BillingEntityCity', N'BillingEntityCity', N'', N'Company', N'String'),
        (N'BillingEntityFax', N'BillingEntityFax', N'', N'Company', N'String'),
        (N'BillingEntityFedTaxId', N'BillingEntityFedTaxId', N'', N'Company', N'String'),
        (N'BillingEntityName', N'BillingEntityName', N'Dev Outreach Laboratory', N'Company', N'String'),  -- dev placeholder
        (N'BillingEntityPhone', N'BillingEntityPhone', N'', N'Company', N'String'),
        (N'BillingEntityState', N'BillingEntityState', N'', N'Company', N'String'),
        (N'BillingEntityStreet', N'BillingEntityStreet', N'', N'Company', N'String'),
        (N'BillingEntityZip', N'BillingEntityZip', N'', N'Company', N'String'),
        (N'BillingPhone', N'BillingPhone', N'', N'Company', N'String'),
        (N'Company2Address', N'Company2Address', N'', N'Company', N'String'),
        (N'Company2City', N'Company2City', N'', N'Company', N'String'),
        (N'Company2CityStateZip', N'Company2CityStateZip', N'', N'Company', N'String'),
        (N'Company2Contact', N'Company2Contact', N'', N'Company', N'String'),
        (N'Company2Fax', N'Company2Fax', N'', N'Company', N'String'),
        (N'Company2Name', N'Company2Name', N'', N'Company', N'String'),
        (N'Company2Phone', N'Company2Phone', N'', N'Company', N'String'),
        (N'Company2State', N'Company2State', N'', N'Company', N'String'),
        (N'Company2Zip', N'Company2Zip', N'', N'Company', N'String'),
        (N'CompanyAddress', N'CompanyAddress', N'', N'Company', N'String'),
        (N'CompanyCity', N'CompanyCity', N'', N'Company', N'String'),
        (N'CompanyCityStateZip', N'CompanyCityStateZip', N'', N'Company', N'String'),
        (N'CompanyContact', N'CompanyContact', N'', N'Company', N'String'),
        (N'CompanyFax', N'CompanyFax', N'', N'Company', N'String'),
        (N'CompanyName', N'CompanyName', N'Dev Outreach Laboratory', N'Company', N'String'),  -- dev placeholder
        (N'CompanyPhone', N'CompanyPhone', N'', N'Company', N'String'),
        (N'CompanyState', N'CompanyState', N'', N'Company', N'String'),
        (N'CompanyZip', N'CompanyZip', N'', N'Company', N'String'),
        (N'FacilityAddress', N'FacilityAddress', N'', N'Company', N'String'),
        (N'FacilityCity', N'FacilityCity', N'', N'Company', N'String'),
        (N'FacilityName', N'FacilityName', N'Dev Outreach Laboratory', N'Company', N'String'),  -- dev placeholder
        (N'FacilityState', N'FacilityState', N'', N'Company', N'String'),
        (N'FacilityZip', N'FacilityZip', N'', N'Company', N'String'),
        (N'FederalTaxId', N'FederalTaxId', N'', N'Company', N'String'),
        (N'InvoiceCompanyAddress', N'InvoiceCompanyAddress', N'', N'Company', N'String'),
        (N'InvoiceCompanyCity', N'InvoiceCompanyCity', N'', N'Company', N'String'),
        (N'InvoiceCompanyName', N'InvoiceCompanyName', N'', N'Company', N'String'),
        (N'InvoiceCompanyPhone', N'InvoiceCompanyPhone', N'', N'Company', N'String'),
        (N'InvoiceCompanyState', N'InvoiceCompanyState', N'', N'Company', N'String'),
        (N'InvoiceCompanyZipCode', N'InvoiceCompanyZipCode', N'', N'Company', N'String'),
        (N'InvoiceLogoImagePath', N'InvoiceLogoImagePath', N'', N'Company', N'String'),
        (N'NPINumber', N'NPINumber', N'', N'Company', N'String'),
        (N'PrimaryCliaNo', N'PrimaryCliaNo', N'', N'Company', N'String'),
        (N'RemitToAddress', N'RemitToAddress', N'', N'Company', N'String'),
        (N'RemitToCity', N'RemitToCity', N'', N'Company', N'String'),
        (N'RemitToCountry', N'RemitToCountry', N'', N'Company', N'String'),
        (N'RemitToOrganizationName', N'RemitToOrganizationName', N'', N'Company', N'String'),
        (N'RemitToState', N'RemitToState', N'', N'Company', N'String'),
        (N'RemitToStreet', N'RemitToStreet', N'', N'Company', N'String'),
        (N'RemitToStreet2', N'RemitToStreet2', N'', N'Company', N'String'),
        (N'RemitToZip', N'RemitToZip', N'', N'Company', N'String'),
        (N'WTHNPI', N'WTHNPI', N'', N'Company', N'String'),
        -- Category: DocumentationSite (ApplicationParameters-Documentation.cs)
        (N'AccountChargeEntryUrl', N'AccountChargeEntryUrl', N'', N'DocumentationSite', N'String'),
        (N'AccountManagementUrl', N'AccountManagementUrl', N'', N'DocumentationSite', N'String'),
        (N'BatchRemittanceUrl', N'BatchRemittanceUrl', N'', N'DocumentationSite', N'String'),
        (N'ChargeMasterMaintenanceUr', N'ChargeMasterMaintenanceUrl', N'', N'DocumentationSite', N'String'),
        (N'ClaimsManagementUrl', N'ClaimsManagementUrl', N'', N'DocumentationSite', N'String'),
        (N'ClientInvoicingUrl', N'ClientInvoicingUrl', N'', N'DocumentationSite', N'String'),
        (N'ClientMaintenanceUrl', N'ClientMaintenanceUrl', N'', N'DocumentationSite', N'String'),
        (N'DocumentationSiteUrl', N'DocumentationSiteUrl', N'', N'DocumentationSite', N'String'),
        (N'InsurancePlanMaintenanceU', N'InsurancePlanMaintenanceUrl', N'', N'DocumentationSite', N'String'),
        (N'LatestUpdatesUrl', N'LatestUpdatesUrl', N'', N'DocumentationSite', N'String'),
        (N'PatientCollectionsUrl', N'PatientCollectionsUrl', N'', N'DocumentationSite', N'String'),
        (N'PatientStatementsUrl', N'PatientStatementsUrl', N'', N'DocumentationSite', N'String'),
        (N'PhysicianMaintenanceUrl', N'PhysicianMaintenanceUrl', N'', N'DocumentationSite', N'String'),
        (N'WorklistUrl', N'WorklistUrl', N'', N'DocumentationSite', N'String'),
        -- Category: Environment (ApplicationParameters-Environment.cs)
        (N'Default1500Printer', N'Default1500Printer', N'', N'Environment', N'String'),
        (N'DefaultClientRequisitionP', N'DefaultClientRequisitionPrinter', N'', N'Environment', N'String'),
        (N'DefaultCytologyRequisitio', N'DefaultCytologyRequisitionPrinter', N'', N'Environment', N'String'),
        (N'DefaultDetailBillPrinter', N'DefaultDetailBillPrinter', N'', N'Environment', N'String'),
        (N'DefaultFilePath', N'DefaultFilePath', N'C:\LabBilling\files', N'Environment', N'String'),  -- dev path
        (N'DefaultMedicareBillPath', N'DefaultMedicareBillPath', N'', N'Environment', N'String'),
        (N'DefaultNursingHomePrinter', N'DefaultNursingHomePrinter', N'', N'Environment', N'String'),
        (N'DefaultPathologyReqPrinte', N'DefaultPathologyReqPrinter', N'', N'Environment', N'String'),
        (N'DefaultSpoolFileDrive', N'DefaultSpoolFileDrive', N'C:', N'Environment', N'String'),  -- dev path
        (N'DefaultSpoolFilePath', N'DefaultSpoolFilePath', N'C:\LabBilling\spool', N'Environment', N'String'),  -- dev path
        (N'DefaultUBPrinter', N'DefaultUBPrinter', N'', N'Environment', N'String'),
        (N'UseDotMatrixRawPrinting', N'UseDotMatrixRawPrinting', N'false', N'Environment', N'Boolean'),
        -- Category: Invoicing (ApplicationParameters-Invoicing.cs)
        (N'ClientBillFilter', N'ClientBillFilter', N'', N'Invoicing', N'String'),
        (N'InvoiceFileLocation', N'InvoiceFileLocation', N'C:\LabBilling\invoices', N'Invoicing', N'String'),  -- dev path
        -- Category: Operations (ApplicationParameters-Operations.cs)
        (N'DemoDays', N'DemoDays', N'', N'Operations', N'String'),
        (N'FileMaintenanceAdtMessage', N'FileMaintenanceAdtMessages', N'30', N'Operations', N'Int32'),  -- dev: keep 30 days of ADT
        (N'FileMaintenanceMessagesIn', N'FileMaintenanceMessagesInbound', N'30', N'Operations', N'Int32'),  -- dev: keep 30 days inbound
        (N'RollupCpt4s', N'RollupCpt4s', N'', N'Operations', N'String'),
        -- Category: Other (ApplicationParameters-Other.cs)
        (N'ChargeTotalApp', N'ChargeTotalApp', N'', N'Other', N'String'),
        (N'ClientAccountFinCode', N'ClientAccountFinCode', N'CLIENT', N'Other', N'String'),  -- DefaultValue
        (N'ClientInvoiceCdm', N'ClientInvoiceCdm', N'CBILL', N'Other', N'String'),  -- DefaultValue
        (N'ChargeInvoiceStatus', N'ChargeInvoiceStatus', N'CBILL', N'Other', N'String'),  -- DefaultValue
        (N'CapitatedChargeStatus', N'CapitatedChargeStatus', N'CAP', N'Other', N'String'),  -- DefaultValue
        (N'NotApplicableChargeStatus', N'NotApplicableChargeStatus', N'N/A', N'Other', N'String'),  -- DefaultValue
        (N'NewChargeStatus', N'NewChargeStatus', N'NEW', N'Other', N'String'),  -- DefaultValue
        (N'ClientFinancialTypeCode', N'ClientFinancialTypeCode', N'C', N'Other', N'String'),  -- DefaultValue
        (N'PatientFinancialTypeCode', N'PatientFinancialTypeCode', N'M', N'Other', N'String'),  -- DefaultValue
        (N'ZFinancialTypecode', N'ZFinancialTypecode', N'Z', N'Other', N'String'),  -- DefaultValue
        (N'BillToClientInvoiceDefaul', N'BillToClientInvoiceDefaultFinCode', N'Y', N'Other', N'String'),  -- DefaultValue
        (N'InvalidFinancialCode', N'InvalidFinancialCode', N'K', N'Other', N'String'),  -- DefaultValue
        (N'InvalidClientCode', N'InvalidClientCode', N'K', N'Other', N'String'),  -- DefaultValue
        (N'SelfPayFinancialCode', N'SelfPayFinancialCode', N'E', N'Other', N'String'),  -- DefaultValue
        (N'PathologyBillingClientExc', N'PathologyBillingClientException', N'HC', N'Other', N'String'),  -- DefaultValue
        -- Category: PathologyGroup (ApplicationParameters-PathologyGroup.cs)
        (N'PathologyGroupClientMnem', N'PathologyGroupClientMnem', N'', N'PathologyGroup', N'String'),
        (N'PathologyGroupBillsProfes', N'PathologyGroupBillsProfessional', N'false', N'PathologyGroup', N'Boolean'),
        -- Category: System (ApplicationParameters-System.cs)
        (N'AccessMedPlusProvNo', N'AccessMedPlusProvNo', N'', N'System', N'String'),
        (N'AllowChargeEntry', N'AllowChargeEntry', N'true', N'System', N'Boolean'),  -- DefaultValue(true)
        (N'AllowEditing', N'AllowEditing', N'true', N'System', N'Boolean'),  -- DefaultValue(true)
        (N'AllowPaymentAdjustmentEnt', N'AllowPaymentAdjustmentEntry', N'true', N'System', N'Boolean'),  -- DefaultValue(true)
        (N'AllowSelMove', N'AllowSelMove', N'', N'System', N'String'),
        (N'ArchiveDB', N'ArchiveDB', N'', N'System', N'String'),
        (N'DatabaseEnvironment', N'DatabaseEnvironment', N'Test', N'System', N'String'),  -- dev: non-production
        (N'ICDVersion', N'ICDVersion', N'10', N'System', N'String'),  -- dev: ICD-10
        (N'LabDirector', N'LabDirector', N'Dev Lab Director', N'System', N'String'),  -- dev placeholder
        (N'ProcessPCCharges', N'ProcessPCCharges', N'false', N'System', N'Boolean'),  -- DefaultValue(false)
        (N'ReportingPortalUrl', N'ReportingPortalUrl', N'', N'System', N'String'),
        (N'SystemVersion', N'SystemVersion', N'', N'System', N'String'),
        (N'TabsOpenLimit', N'TabsOpenLimit', N'4', N'System', N'Int32'),  -- DefaultValue(4)
        (N'ServiceUser', N'ServiceUser', N'', N'System', N'String'),
        (N'ServiceUserPassword', N'ServiceUserPassword', N'', N'System', N'String'),
        (N'LogLevel', N'LogLevel', N'Error', N'System', N'String'),  -- DefaultValue(Error)
        (N'LogLocation', N'LogLocation', N'Database', N'System', N'String'),  -- DefaultValue(Database)
        (N'LogFilePath', N'LogFilePath', N'', N'System', N'String'),
        -- Category: ViewerSlides (ApplicationParameters-ViewerSlides.cs)
        (N'IHCStainsQuery', N'IHCStainsQuery', N'', N'ViewerSlides', N'String'),
        (N'SlidesQuery', N'SlidesQuery', N'', N'ViewerSlides', N'String'),
        (N'SlidesStartDate', N'SlidesStartDate', N'1900-01-01', N'ViewerSlides', N'DateTime'),
        (N'SpecialClientsQuery', N'SpecialClientsQuery', N'', N'ViewerSlides', N'String'),
        (N'SpecialStainsQuery', N'SpecialStainsQuery', N'', N'ViewerSlides', N'String')
) AS src ([key_name], [KeyName], [value], [category], [dataType])
    ON tgt.[KeyName] = src.[KeyName]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([key_name], [KeyName], [value], [category], [dataType])
    VALUES (src.[key_name], src.[KeyName], src.[value], src.[category], src.[dataType]);

DECLARE @sysCount int = (SELECT COUNT(*) FROM [dbo].[system]);
PRINT 'system rows after merge: ' + CAST(@sysCount AS varchar(10));
GO

PRINT '--- Seeding [emp] admin users ---';
GO

/* -------------------------------------------------------------------------
   Users. Authorization facts (traced from code):
     * Login requires an [emp] row whose [name] matches the username and whose
       [access] is NOT 'NONE'. Highest access level is 'ENTER/EDIT'
       (UserStatus.EnterEdit) -> full create/modify.
     * reserve4 = 1  => UserAccount.IsAdministrator. The Blazor middleware maps
       this to the Administrator role AND grants the RandomDrugScreen policy
       (RandomDrugScreenAuthorizationHandler succeeds for administrators), so a
       single admin row reaches every module.
     * Windows/dev auth (DevelopmentAuthenticationHandler -> AuthenticateIntegrated)
       does NOT check the password - only that the row exists with access<>NONE.
     * Username/password auth (AuthenticationService.Authenticate) compares
       [password] to Base64( SHA256(UTF8(password)) ). We seed that hash so the
       SQL-auth path also works. Seeded plaintext password is 'Password1'.
   The two seeded users:
     * brads  - matches the current Windows dev user (Environment.UserName) so the
                Blazor dev-auth login works with no appsettings change.
     * admin  - portable generic administrator for any machine.
   All NOT NULL [emp] columns (the bit permission flags) are set explicitly; the
   rest carry their schema defaults. We do not write access_random_drug_screen:
   that column does not exist in the target schema (RDS access comes from reserve4).
   ------------------------------------------------------------------------- */

/* Compute Base64( SHA256( UTF8('Password1') ) ) entirely in T-SQL so the stored
   hash matches AuthenticationService.EncryptPassword exactly. For the ASCII string
   'Password1' the varchar (DB code page) bytes equal the UTF-8 bytes, and the XML
   xs:base64Binary cast yields the same Base64 string the app's
   Convert.ToBase64String produces. Plaintext password for SQL-auth login: 'Password1'. */
DECLARE @pwdHash varchar(100) =
    (SELECT CAST(N'' AS xml).value(
                'xs:base64Binary(xs:hexBinary(sql:column("h")))', 'varchar(100)')
     FROM (SELECT HASHBYTES('SHA2_256', CAST(N'Password1' AS varchar(100))) AS h) AS t);

IF NOT EXISTS (SELECT 1 FROM [dbo].[emp] WHERE [name] = N'brads')
    INSERT INTO [dbo].[emp]
        ([name], [full_name], [access], [password],
         [access_edit_dictionary], [access_bad_debt], [access_billing], [access_fin_code],
         [add_chrg], [add_chk], [add_chk_amt], [reserve4], [reserve5], [reserve6], [impersonate])
    VALUES
        (N'brads', N'Brad Powers (Dev)', N'ENTER/EDIT', @pwdHash,
         1, 1, 1, 1,
         1, 1, 1, 1, 0, 0, 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[emp] WHERE [name] = N'admin')
    INSERT INTO [dbo].[emp]
        ([name], [full_name], [access], [password],
         [access_edit_dictionary], [access_bad_debt], [access_billing], [access_fin_code],
         [add_chrg], [add_chk], [add_chk_amt], [reserve4], [reserve5], [reserve6], [impersonate])
    VALUES
        (N'admin', N'System Administrator (Dev)', N'ENTER/EDIT', @pwdHash,
         1, 1, 1, 1,
         1, 1, 1, 1, 0, 0, 1);

DECLARE @empCount int = (SELECT COUNT(*) FROM [dbo].[emp] WHERE [name] IN (N'brads', N'admin'));
PRINT 'emp admin rows present: ' + CAST(@empCount AS varchar(10));
GO

PRINT '--- Seed complete ---';
GO
