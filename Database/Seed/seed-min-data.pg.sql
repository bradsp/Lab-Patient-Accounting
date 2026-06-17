/* ============================================================================
   seed-min-data.pg.sql  --  PostgreSQL port of the minimum dev/test seed
   ----------------------------------------------------------------------------
   Ported from Database/Seed/seed-min-data.sql (SQL Server / T-SQL) for the
   Phase 22 PostgreSQL foundation. Seeds the two tables the app needs to boot:
     * dbo."system"  - 188 application parameters (PetaPoco model SysParameter).
                       Looked up by the "KeyName" column at startup.
     * dbo.emp       - dev user accounts (PetaPoco model UserAccount). Needs a row
                       per login identity with access <> 'NONE'.

   TARGET: labbilling database on the Phase 22 docker PostgreSQL (localhost:5434).
           Schema (270 tables) already loaded; identifiers are case-preserved/quoted.

   PG TRANSLATION NOTES
     * T-SQL MERGE / IF NOT EXISTS  -> INSERT ... ON CONFLICT DO NOTHING (idempotent).
     * GETDATE() / HASHBYTES+XML base64 cast -> now() / pgcrypto
       encode(digest(...,'sha256'),'base64').
     * Quoted, case-preserved identifiers (dbo."system", "KeyName", dbo.emp).
     * NOT NULL audit columns on system (mod_user/mod_prg/mod_host) seeded explicitly.

   DEV/TEST ONLY. Synthetic data, no PHI. Seeded SQL-auth password: 'Password1'.
   ============================================================================ */

BEGIN;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- ---------------------------------------------------------------------------
-- dbo."system" application parameters (188 rows)
-- Idempotent: ON CONFLICT on the key_name primary key does nothing on re-run.
-- ---------------------------------------------------------------------------
INSERT INTO dbo."system" (key_name, "KeyName", value, category, "dataType", mod_user, mod_prg, mod_host, mod_date)
VALUES
    ('BankAccountNumber', 'BankAccountNumber', '', 'Accounting', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BankName', 'BankName', '', 'Accounting', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BankRoutingNumber', 'BankRoutingNumber', '', 'Accounting', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('AuthorizedToRunClientBill', 'AuthorizedToRunClientBills', 'bpowers|admin', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BHGroupNo', 'BHGroupNo', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BHProviderNo', 'BHProviderNo', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingInitialHoldDays', 'BillingInitialHoldDays', '10', 'Billing', 'Int32', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingReceiverId', 'BillingReceiverId', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingReceiverName', 'BillingReceiverName', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BlueCrossProvNo', 'BlueCrossProvNo', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BlueCrossReceiverNo', 'BlueCrossReceiverNo', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BundleInsurance', 'BundleInsurance', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ChampusGroupNo', 'ChampusGroupNo', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CNetReceiverId', 'CNetReceiverId', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CodeStats', 'CodeStats', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CollectionsRun', 'CollectionsRun', '1900-01-01', 'Billing', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DBMailProfile', 'DBMailProfile', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Diagnosis', 'Diagnosis', 'false', 'Billing', 'Boolean', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DiagnosisCodePointerSelec', 'DiagnosisCodePointerSelect', 'true', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ElectronicBillDate', 'ElectronicBillDate', '1900-01-01', 'Billing', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('EobAddress', 'EobAddress', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('GlobalBillingStartDate', 'GlobalBillingStartDate', '1900-01-01', 'Billing', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InstitutionalClaimFileLoc', 'InstitutionalClaimFileLocation', 'C:\LabBilling\claims\837i', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('MaxClaimsInClaimBatch', 'MaxClaimsInClaimBatch', '0', 'Billing', 'Int32', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('MedicaidProviderId', 'MedicaidProviderId', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('MedicareBProviderNo', 'MedicareBProviderNo', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('MedicareElectronicReceive', 'MedicareElectronicReceiverId', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('MedicareProviderNo', 'MedicareProviderNo', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('MedicareReceiverId', 'MedicareReceiverId', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('MedicareSubmitterId', 'MedicareSubmitterId', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('NursingHomeBillThruDate', 'NursingHomeBillThruDate', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('OutpatientBillStart', 'OutpatientBillStart', '1900-01-01', 'Billing', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ProfessionalClaimFileLoca', 'ProfessionalClaimFileLocation', 'C:\LabBilling\claims\837p', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ProfessionalClaimStartDat', 'ProfessionalClaimStartDate', '1900-01-01', 'Billing', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ProviderTaxonomyCode', 'ProviderTaxonomyCode', '282N00000X', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitImportDirectory', 'RemitImportDirectory', 'C:\LabBilling\remit\import', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitPostingDate', 'RemitPostingDate', '1900-01-01', 'Billing', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitProcessingDirectory', 'RemitProcessingDirectory', 'C:\LabBilling\remit\processing', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitDocumentDirectory', 'RemitDocumentDirectory', 'C:\LabBilling\remit\835pdf', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SecondaryBilling', 'SecondaryBilling', '1900-01-01', 'Billing', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SmallBalanceAmount', 'SmallBalanceAmount', '0', 'Billing', 'Double', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SSIStartDate', 'SSIStartDate', '1900-01-01', 'Billing', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('TLCProviderId', 'TLCProviderId', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('UHCProviderId', 'UHCProviderId', '', 'Billing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('UseBillMethod', 'UseBillMethod', '0', 'Billing', 'Int32', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FeeSchedules', 'FeeSchedules', '', 'Charging', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('GeneralHealthPanelInsuran', 'GeneralHealthPanelInsurances', '', 'Charging', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('GeneralHealthPanelTests', 'GeneralHealthPanelTests', '', 'Charging', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('OBPanelInsurances', 'OBPanelInsurances', '', 'Charging', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('OBPanelTests', 'OBPanelTests', '', 'Charging', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('QuestBillingEmailRecipien', 'QuestBillingEmailRecipients', '', 'Charging', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CollectionsFileLocation', 'CollectionsFileLocation', 'C:\LabBilling\collections', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CollectionsSftpPassword', 'CollectionsSftpPassword', '', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CollectionsSftpServer', 'CollectionsSftpServer', '', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CollectionsSftpUsername', 'CollectionsSftpUsername', '', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CollectionsSftpUploadPath', 'CollectionsSftpUploadPath', '', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('StatementsFileLocation', 'StatementsFileLocation', 'C:\LabBilling\statements', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('StatementsSftpPassword', 'StatementsSftpPassword', '', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('StatementsSftpServer', 'StatementsSftpServer', '', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('StatementsSftpUsername', 'StatementsSftpUsername', '', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('StatementsSftpUploadPath', 'StatementsSftpUploadPath', '', 'Collections', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('NumberOfStatementsBeforeC', 'NumberOfStatementsBeforeCollection', '3', 'Collections', 'Int32', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingContact', 'BillingContact', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityCounty', 'BillingEntityCounty', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEmail', 'BillingEmail', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityCity', 'BillingEntityCity', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityFax', 'BillingEntityFax', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityFedTaxId', 'BillingEntityFedTaxId', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityName', 'BillingEntityName', 'Dev Outreach Laboratory', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityPhone', 'BillingEntityPhone', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityState', 'BillingEntityState', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityStreet', 'BillingEntityStreet', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingEntityZip', 'BillingEntityZip', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillingPhone', 'BillingPhone', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2Address', 'Company2Address', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2City', 'Company2City', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2CityStateZip', 'Company2CityStateZip', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2Contact', 'Company2Contact', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2Fax', 'Company2Fax', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2Name', 'Company2Name', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2Phone', 'Company2Phone', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2State', 'Company2State', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Company2Zip', 'Company2Zip', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyAddress', 'CompanyAddress', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyCity', 'CompanyCity', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyCityStateZip', 'CompanyCityStateZip', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyContact', 'CompanyContact', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyFax', 'CompanyFax', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyName', 'CompanyName', 'Dev Outreach Laboratory', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyPhone', 'CompanyPhone', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyState', 'CompanyState', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CompanyZip', 'CompanyZip', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FacilityAddress', 'FacilityAddress', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FacilityCity', 'FacilityCity', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FacilityName', 'FacilityName', 'Dev Outreach Laboratory', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FacilityState', 'FacilityState', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FacilityZip', 'FacilityZip', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FederalTaxId', 'FederalTaxId', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvoiceCompanyAddress', 'InvoiceCompanyAddress', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvoiceCompanyCity', 'InvoiceCompanyCity', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvoiceCompanyName', 'InvoiceCompanyName', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvoiceCompanyPhone', 'InvoiceCompanyPhone', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvoiceCompanyState', 'InvoiceCompanyState', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvoiceCompanyZipCode', 'InvoiceCompanyZipCode', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvoiceLogoImagePath', 'InvoiceLogoImagePath', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('NPINumber', 'NPINumber', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('PrimaryCliaNo', 'PrimaryCliaNo', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitToAddress', 'RemitToAddress', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitToCity', 'RemitToCity', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitToCountry', 'RemitToCountry', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitToOrganizationName', 'RemitToOrganizationName', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitToState', 'RemitToState', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitToStreet', 'RemitToStreet', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitToStreet2', 'RemitToStreet2', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RemitToZip', 'RemitToZip', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('WTHNPI', 'WTHNPI', '', 'Company', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('AccountChargeEntryUrl', 'AccountChargeEntryUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('AccountManagementUrl', 'AccountManagementUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BatchRemittanceUrl', 'BatchRemittanceUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ChargeMasterMaintenanceUr', 'ChargeMasterMaintenanceUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ClaimsManagementUrl', 'ClaimsManagementUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ClientInvoicingUrl', 'ClientInvoicingUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ClientMaintenanceUrl', 'ClientMaintenanceUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DocumentationSiteUrl', 'DocumentationSiteUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InsurancePlanMaintenanceU', 'InsurancePlanMaintenanceUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('LatestUpdatesUrl', 'LatestUpdatesUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('PatientCollectionsUrl', 'PatientCollectionsUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('PatientStatementsUrl', 'PatientStatementsUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('PhysicianMaintenanceUrl', 'PhysicianMaintenanceUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('WorklistUrl', 'WorklistUrl', '', 'DocumentationSite', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('Default1500Printer', 'Default1500Printer', '', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultClientRequisitionP', 'DefaultClientRequisitionPrinter', '', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultCytologyRequisitio', 'DefaultCytologyRequisitionPrinter', '', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultDetailBillPrinter', 'DefaultDetailBillPrinter', '', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultFilePath', 'DefaultFilePath', 'C:\LabBilling\files', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultMedicareBillPath', 'DefaultMedicareBillPath', '', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultNursingHomePrinter', 'DefaultNursingHomePrinter', '', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultPathologyReqPrinte', 'DefaultPathologyReqPrinter', '', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultSpoolFileDrive', 'DefaultSpoolFileDrive', 'C:', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultSpoolFilePath', 'DefaultSpoolFilePath', 'C:\LabBilling\spool', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DefaultUBPrinter', 'DefaultUBPrinter', '', 'Environment', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('UseDotMatrixRawPrinting', 'UseDotMatrixRawPrinting', 'false', 'Environment', 'Boolean', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ClientBillFilter', 'ClientBillFilter', '', 'Invoicing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvoiceFileLocation', 'InvoiceFileLocation', 'C:\LabBilling\invoices', 'Invoicing', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DemoDays', 'DemoDays', '', 'Operations', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FileMaintenanceAdtMessage', 'FileMaintenanceAdtMessages', '30', 'Operations', 'Int32', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('FileMaintenanceMessagesIn', 'FileMaintenanceMessagesInbound', '30', 'Operations', 'Int32', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('RollupCpt4s', 'RollupCpt4s', '', 'Operations', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ChargeTotalApp', 'ChargeTotalApp', '', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ClientAccountFinCode', 'ClientAccountFinCode', 'CLIENT', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ClientInvoiceCdm', 'ClientInvoiceCdm', 'CBILL', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ChargeInvoiceStatus', 'ChargeInvoiceStatus', 'CBILL', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('CapitatedChargeStatus', 'CapitatedChargeStatus', 'CAP', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('NotApplicableChargeStatus', 'NotApplicableChargeStatus', 'N/A', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('NewChargeStatus', 'NewChargeStatus', 'NEW', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ClientFinancialTypeCode', 'ClientFinancialTypeCode', 'C', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('PatientFinancialTypeCode', 'PatientFinancialTypeCode', 'M', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ZFinancialTypecode', 'ZFinancialTypecode', 'Z', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('BillToClientInvoiceDefaul', 'BillToClientInvoiceDefaultFinCode', 'Y', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvalidFinancialCode', 'InvalidFinancialCode', 'K', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('InvalidClientCode', 'InvalidClientCode', 'K', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SelfPayFinancialCode', 'SelfPayFinancialCode', 'E', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('PathologyBillingClientExc', 'PathologyBillingClientException', 'HC', 'Other', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('PathologyGroupClientMnem', 'PathologyGroupClientMnem', '', 'PathologyGroup', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('PathologyGroupBillsProfes', 'PathologyGroupBillsProfessional', 'false', 'PathologyGroup', 'Boolean', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('AccessMedPlusProvNo', 'AccessMedPlusProvNo', '', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('AllowChargeEntry', 'AllowChargeEntry', 'true', 'System', 'Boolean', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('AllowEditing', 'AllowEditing', 'true', 'System', 'Boolean', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('AllowPaymentAdjustmentEnt', 'AllowPaymentAdjustmentEntry', 'true', 'System', 'Boolean', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('AllowSelMove', 'AllowSelMove', '', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ArchiveDB', 'ArchiveDB', '', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('DatabaseEnvironment', 'DatabaseEnvironment', 'Test', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ICDVersion', 'ICDVersion', '10', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('LabDirector', 'LabDirector', 'Dev Lab Director', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ProcessPCCharges', 'ProcessPCCharges', 'false', 'System', 'Boolean', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ReportingPortalUrl', 'ReportingPortalUrl', '', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SystemVersion', 'SystemVersion', '', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('TabsOpenLimit', 'TabsOpenLimit', '4', 'System', 'Int32', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ServiceUser', 'ServiceUser', '', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('ServiceUserPassword', 'ServiceUserPassword', '', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('LogLevel', 'LogLevel', 'Error', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('LogLocation', 'LogLocation', 'Database', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('LogFilePath', 'LogFilePath', '', 'System', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('IHCStainsQuery', 'IHCStainsQuery', '', 'ViewerSlides', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SlidesQuery', 'SlidesQuery', '', 'ViewerSlides', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SlidesStartDate', 'SlidesStartDate', '1900-01-01', 'ViewerSlides', 'DateTime', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SpecialClientsQuery', 'SpecialClientsQuery', '', 'ViewerSlides', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now()),
    ('SpecialStainsQuery', 'SpecialStainsQuery', '', 'ViewerSlides', 'String', 'seed', 'seed-min-data.pg.sql', 'localhost', now())
ON CONFLICT (key_name) DO NOTHING;

-- ---------------------------------------------------------------------------
-- dbo.emp dev users. access=ENTER/EDIT (full), reserve4=true (IsAdministrator =>
-- Administrator role + RandomDrugScreen policy), access_random_drug_screen=true.
-- password = Base64(SHA256(UTF8('Password1'))) so the SQL-auth path also works.
--   bpowers - dev login identity (WTHMC\bpowers -> bpowers after domain strip).
--   admin   - portable generic administrator.
-- ---------------------------------------------------------------------------
INSERT INTO dbo.emp
    (name, full_name, access, password,
     access_edit_dictionary, access_bad_debt, access_billing, access_fin_code,
     add_chrg, add_chk, add_chk_amt, reserve4, reserve5, reserve6, impersonate,
     access_random_drug_screen, mod_user, mod_prg, mod_date)
VALUES
    ('bpowers', 'Brad Powers (Dev)', 'ENTER/EDIT', encode(digest('Password1','sha256'),'base64'),
     true, true, true, true,
     true, true, true, true, false, false, true,
     true, 'seed', 'seed-min-data.pg.sql', now()),
    ('admin', 'System Administrator (Dev)', 'ENTER/EDIT', encode(digest('Password1','sha256'),'base64'),
     true, true, true, true,
     true, true, true, true, false, false, true,
     true, 'seed', 'seed-min-data.pg.sql', now())
ON CONFLICT (name) DO NOTHING;

COMMIT;

-- Verify (informational):
--   SELECT count(*) FROM dbo."system";   -- expect 188
--   SELECT name, access FROM dbo.emp;   -- expect bpowers, admin
