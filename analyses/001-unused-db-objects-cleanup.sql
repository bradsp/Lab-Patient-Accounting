/*
================================================================================
 Unused Database Objects Cleanup Script
 Lab Patient Accounting Database

 Generated: 2026-02-03
 Source Analysis: 001-unused-db-objects-report.md

 PURPOSE:
 Removes database objects identified as unused by cross-referencing the schema
 against the entire application codebase. Each DROP is wrapped in IF EXISTS
 for safe incremental execution.

 IMPORTANT WARNINGS:
 1. Before running, verify no SQL Server Agent jobs reference these objects
 2. Before running, verify no SSRS reports reference these objects
 3. Before running, verify no Monthly_Reports table entries reference these objects
 4. Execute in the order presented (functions/procedures first, then views, then tables)
 5. Each DROP statement is independent and can be run individually
 6. Take a backup before running this script

 EXECUTION APPROACH:
 - Run each section independently
 - Verify application functionality between sections
 - Rollback is only possible from backup
================================================================================
*/

USE [LabBillingProd]; -- Adjust database name as needed
GO

PRINT '========================================';
PRINT 'Phase 1: Remove Orphaned Functions';
PRINT '========================================';
GO

-- HIGH CONFIDENCE: Test/variant functions with no references anywhere
-- These are duplicates, test versions, or clearly unused variants

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormatPhoneNoTest]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[FormatPhoneNoTest];
GO -- Confidence: HIGH - Test version of FormatPhoneNo

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PadTextSave]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[PadTextSave];
GO -- Confidence: HIGH - Unused variant of PadText

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sfn_trim_zeros_mri]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[sfn_trim_zeros_mri];
GO -- Confidence: HIGH - MRI-specific variant, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPatIcd92]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetPatIcd92];
GO -- Confidence: HIGH - Variant 2, superseded

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udf_XML2Table]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[udf_XML2Table];
GO -- Confidence: HIGH - XML utility, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNumbersTable]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetNumbersTable];
GO -- Confidence: HIGH - Variant of NumbersTable, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnRemoveChildElementsNC]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[fnRemoveChildElementsNC];
GO -- Confidence: HIGH - No-case variant, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DelimitedSplit8K]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[DelimitedSplit8K];
GO -- Confidence: HIGH - String splitter, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArrayToTable]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[ArrayToTable];
GO -- Confidence: HIGH - Array utility, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAddedCDMs]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetAddedCDMs];
GO -- Confidence: HIGH - Added CDM lookup, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccBalByServiceDate]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetAccBalByServiceDate];
GO -- Confidence: HIGH - Balance by service date, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccBalByTransDate]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetAccBalByTransDate];
GO -- Confidence: HIGH - Balance by trans date, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccDiagnosis1]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetAccDiagnosis1];
GO -- Confidence: HIGH - Variant 1, superseded by GetAccDiagnosis

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccIns]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetAccIns];
GO -- Confidence: HIGH - Account insurance, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccTotalCount]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetAccTotalCount];
GO -- Confidence: HIGH - Total count, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAmountTotal]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetAmountTotal];
GO -- Confidence: HIGH - Amount total, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAmtRecords2]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetAmtRecords2];
GO -- Confidence: HIGH - Variant 2, superseded

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCernerPhyNPI]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetCernerPhyNPI];
GO -- Confidence: HIGH - Legacy Cerner integration

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetClientPrice]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetClientPrice];
GO -- Confidence: HIGH - Client pricing, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetClientsUnbilledCharges]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetClientsUnbilledCharges];
GO -- Confidence: HIGH - Unbilled charges, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetFeeSchedulePricewdk]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetFeeSchedulePricewdk];
GO -- Confidence: HIGH - WDK variant of fee schedule price

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetInvoiceCharges]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetInvoiceCharges];
GO -- Confidence: HIGH - Superseded by InvoiceChargeView

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastDateBilled]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetLastDateBilled];
GO -- Confidence: HIGH - Last date billed, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLastInvoiceDate]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetLastInvoiceDate];
GO -- Confidence: HIGH - Last invoice date, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMappedPhysician]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetMappedPhysician];
GO -- Confidence: HIGH - Mapped physician, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMultiples]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetMultiples];
GO -- Confidence: HIGH - Multiples detection, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewFeeSchedPrice]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetNewFeeSchedPrice];
GO -- Confidence: HIGH - New fee schedule price, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPatDiagnosis_icd]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetPatDiagnosis_icd];
GO -- Confidence: HIGH - ICD diagnosis, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPatDiagnosis2]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetPatDiagnosis2];
GO -- Confidence: HIGH - Variant 2, superseded

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPaymentsBeforeLastDataMailer]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetPaymentsBeforeLastDataMailer];
GO -- Confidence: HIGH - Legacy data mailer

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPaymentsByDate2]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetPaymentsByDate2];
GO -- Confidence: HIGH - Variant 2, superseded

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPaymentsTableSinceLastDataMailer]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetPaymentsTableSinceLastDataMailer];
GO -- Confidence: HIGH - Legacy data mailer

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetXmlDataForAcc2]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetXmlDataForAcc2];
GO -- Confidence: HIGH - XML variant 2, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetXmlDataForAccCharges]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetXmlDataForAccCharges];
GO -- Confidence: HIGH - XML charges, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetXmlDataForAccInfce]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetXmlDataForAccInfce];
GO -- Confidence: HIGH - XML interface, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetXmlDataForAccIns]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetXmlDataForAccIns];
GO -- Confidence: HIGH - XML insurance, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SplitCdmPriceByCpt2]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[SplitCdmPriceByCpt2];
GO -- Confidence: HIGH - Variant 2, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SplitCdmPriceByCpt3]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[SplitCdmPriceByCpt3];
GO -- Confidence: HIGH - Variant 3, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tvf_accounting_report_by_cdm_client]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[tvf_accounting_report_by_cdm_client];
GO -- Confidence: HIGH - Table-valued function, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_client]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[ufn_client];
GO -- Confidence: HIGH - Client function, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_Duplicates]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[ufn_Duplicates];
GO -- Confidence: HIGH - Duplicate detection, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAccountPayment]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[ufn_GetAccountPayment];
GO -- Confidence: HIGH - Account payment, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetQuestError]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[ufn_GetQuestError];
GO -- Confidence: HIGH - Quest error, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usf_AccCheckForLMRP]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[usf_AccCheckForLMRP];
GO -- Confidence: HIGH - LMRP check, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usf_account_report_client_by_physician]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[usf_account_report_client_by_physician];
GO -- Confidence: HIGH - Report by physician, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usf_account_report_client_qty]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[usf_account_report_client_qty];
GO -- Confidence: HIGH - Report quantity, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usf_prg_panels]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[usf_prg_panels];
GO -- Confidence: HIGH - Panel processing, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[XmlFormatPhysicianCerner]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[XmlFormatPhysicianCerner];
GO -- Confidence: HIGH - Legacy Cerner XML formatting

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Get_XMLCerner_data]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[fn_Get_XMLCerner_data];
GO -- Confidence: HIGH - Legacy Cerner XML data

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnConvert_TitleCase]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[fnConvert_TitleCase];
GO -- Confidence: HIGH - Title case conversion, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnParseStringTSQL]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[fnParseStringTSQL];
GO -- Confidence: HIGH - String parser, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnRandomizedText]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[fnRandomizedText];
GO -- Confidence: HIGH - Test/dev randomizer

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnStandardPhone]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[fnStandardPhone];
GO -- Confidence: HIGH - Standard phone format, no references

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBillMethod]') AND type IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION [dbo].[GetBillMethod];
GO -- Confidence: HIGH - Bill method, only CREATE reference in SQL

PRINT 'Phase 1 complete: Orphaned functions removed.';
GO

PRINT '========================================';
PRINT 'Phase 2: Remove Orphaned Stored Procedures';
PRINT '========================================';
GO

-- HIGH CONFIDENCE: Template, admin, and clearly unused procedures

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_ExampleProc]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_ExampleProc];
GO -- Confidence: HIGH - Template/example only

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChangeSchema]') AND type = N'P')
    DROP PROCEDURE [dbo].[ChangeSchema];
GO -- Confidence: HIGH - Admin schema change utility

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Compare_Table_Structure]') AND type = N'P')
    DROP PROCEDURE [dbo].[proc_Compare_Table_Structure];
GO -- Confidence: HIGH - Dev/admin table comparison

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TableSpaceUsed]') AND type = N'P')
    DROP PROCEDURE [dbo].[TableSpaceUsed];
GO -- Confidence: HIGH - Admin space reporting

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CopyBillingToTest]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_CopyBillingToTest];
GO -- Confidence: HIGH - Admin data copy utility

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FixCreditedFlags]') AND type = N'P')
    DROP PROCEDURE [dbo].[FixCreditedFlags];
GO -- Confidence: HIGH - One-time data fix

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[getName]') AND type = N'P')
    DROP PROCEDURE [dbo].[getName];
GO -- Confidence: HIGH - Simple name lookup, unused

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAccountSummary]') AND type = N'P')
    DROP PROCEDURE [dbo].[GetAccountSummary];
GO -- Confidence: HIGH - Superseded by app logic

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetClientsToBill]') AND type = N'P')
    DROP PROCEDURE [dbo].[GetClientsToBill];
GO -- Confidence: HIGH - Superseded by app logic

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetMasterAccount]') AND type = N'P')
    DROP PROCEDURE [dbo].[sp_GetMasterAccount];
GO -- Confidence: HIGH - Superseded by app logic

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_prg_update_nh_bill_thru_date]') AND type = N'P')
    DROP PROCEDURE [dbo].[sp_prg_update_nh_bill_thru_date];
GO -- Confidence: HIGH - Legacy nursing home billing

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UndoClientInvoice]') AND type = N'P')
    DROP PROCEDURE [dbo].[UndoClientInvoice];
GO -- Confidence: MEDIUM - Manual DBA proc, verify first

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAccBalanceAsOfDate]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_GetAccBalanceAsOfDate];
GO -- Confidence: HIGH - Superseded by GetAccBalByDate function

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetErrorInfo]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_GetErrorInfo];
GO -- Confidence: MEDIUM - May be called from TRY/CATCH blocks

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_system_log_message]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_system_log_message];
GO -- Confidence: HIGH - Superseded by NLog

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateSystemValue]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_UpdateSystemValue];
GO -- Confidence: HIGH - Superseded by app logic

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_ViewerAcc_Billing_Table]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_ViewerAcc_Billing_Table];
GO -- Confidence: HIGH - Unused viewer proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ViewerAcc_Select]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ViewerAcc_Select];
GO -- Confidence: HIGH - Unused viewer proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_bnp_update]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_bnp_update];
GO -- Confidence: HIGH - One-time BNP update

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_care360_get_account_charges2]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_care360_get_account_charges2];
GO -- Confidence: HIGH - Legacy Care360 integration

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_cbill_hist]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_cbill_hist];
GO -- Confidence: HIGH - Superseded by app logic

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_added_cdms]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_added_cdms];
GO -- Confidence: HIGH - Unused CDM report

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_cbill_client_orders]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_cbill_client_orders];
GO -- Confidence: HIGH - Unused client orders proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_JobSummaryUtil]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_JobSummaryUtil];
GO -- Confidence: HIGH - Unused job summary

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_Charges_With_Payments2]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_Charges_With_Payments2];
GO -- Confidence: HIGH - Unused variant

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_Quest_Path_Payment2]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_Quest_Path_Payment2];
GO -- Confidence: HIGH - Unused variant

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_quest_cbill_by_invoice]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_quest_cbill_by_invoice];
GO -- Confidence: HIGH - Unused Quest cbill

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_psa_demo_activity_check]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_psa_demo_activity_check];
GO -- Confidence: HIGH - Legacy PSA interface

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_infce_psa_demographics_special]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_infce_psa_demographics_special];
GO -- Confidence: HIGH - Legacy PSA interface

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prgProcessCharge]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prgProcessCharge];
GO -- Confidence: HIGH - Superseded by C# HL7ProcessorService

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prgProcessChargeADT]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prgProcessChargeADT];
GO -- Confidence: HIGH - Superseded by C# HL7ProcessorService

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_CreateAccount]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_CreateAccount];
GO -- Confidence: HIGH - Superseded by app logic

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_Drug_Screens]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_Drug_Screens];
GO -- Confidence: HIGH - Superseded by C# RandomDrugScreenService

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_Drug_Screens_Reversal]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_Drug_Screens_Reversal];
GO -- Confidence: HIGH - Superseded by C# service

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_Drug_Screens2]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_Drug_Screens2];
GO -- Confidence: HIGH - Superseded by C# service

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_InvoiceReprint]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_InvoiceReprint];
GO -- Confidence: HIGH - Superseded by app logic

-- Legacy XML import procedures (entire chain unused from app)
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_Xml_Account_Verify]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_Xml_Account_Verify];
GO -- Confidence: HIGH - Legacy XML import verification

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_Xml_Import_Insurance_Verify]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_Xml_Import_Insurance_Verify];
GO -- Confidence: HIGH - Legacy XML import verification

-- Standalone ReCharge/Reverse procedures (not called from C# code)
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ReCharge]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ReCharge];
GO -- Confidence: HIGH - Not called from C# code

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ReCharge_Acc_Transaction]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ReCharge_Acc_Transaction];
GO -- Confidence: HIGH - Not called from C# code

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ReverseChrgeOnly_Acc_Transaction]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ReverseChrgeOnly_Acc_Transaction];
GO -- Confidence: HIGH - Not called from C# code

-- Patient billing display/verify procs (unused QA/display variants)
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_acct_verify]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_acct_verify];
GO -- Confidence: HIGH - QA verification proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_actv_display]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_actv_display];
GO -- Confidence: HIGH - Display variant

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_actv_verify]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_actv_verify];
GO -- Confidence: HIGH - QA verification proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_enct_display]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_enct_display];
GO -- Confidence: HIGH - Display variant

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_enct_verify]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_enct_verify];
GO -- Confidence: HIGH - QA verification proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_stmt_display]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_stmt_display];
GO -- Confidence: HIGH - Display variant

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_stmt_errors]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_stmt_errors];
GO -- Confidence: HIGH - Error reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_stmt_small_balance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_stmt_small_balance];
GO -- Confidence: HIGH - Small balance proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_pat_bill_stmt_verify]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_pat_bill_stmt_verify];
GO -- Confidence: HIGH - QA verification proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[medicare_random_occ_bld]') AND type = N'P')
    DROP PROCEDURE [dbo].[medicare_random_occ_bld];
GO -- Confidence: HIGH - Legacy Medicare random

PRINT 'Phase 2 complete: Orphaned stored procedures removed.';
GO

-- MEDIUM CONFIDENCE: Reporting/accounting procedures
-- WARNING: Verify these are not used by SQL Agent jobs or SSRS before removing

PRINT '========================================';
PRINT 'Phase 2b: Remove Reporting Procedures';
PRINT 'WARNING: Verify SQL Agent jobs first!';
PRINT '========================================';
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MonthlyARTotals]') AND type = N'P')
    DROP PROCEDURE [dbo].[MonthlyARTotals];
GO -- Confidence: MEDIUM - May be SQL Agent job. Verify first.

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_accounting_report]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_accounting_report];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_acct_recievables_by_cost_center]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_acct_recievables_by_cost_center];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ar_rebuild]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ar_rebuild];
GO -- Confidence: MEDIUM - AR rebuild

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_billing_type_totals]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_billing_type_totals];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_charges_by_cost_centers_with_clients_and_accounts]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_charges_by_cost_centers_with_clients_and_accounts];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_global_billing]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_global_billing];
GO -- Confidence: MEDIUM - May be SQL Agent job

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_OutReach_Cost]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_OutReach_Cost];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_Quest_Charges]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_Quest_Charges];
GO -- Confidence: MEDIUM - May be SQL Agent job

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_account_payout_v2]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_account_payout_v2];
GO -- Confidence: MEDIUM - May be SQL Agent job (sends email)

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_account_receivable_daily_reconciliation]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_account_receivable_daily_reconciliation];
GO -- Confidence: MEDIUM - May be SQL Agent job

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_report_by_cpt4]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_report_by_cpt4];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_report_by_gl]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_report_by_gl];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_report_clients_all]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_report_clients_all];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_report_data]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_report_data];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_report_refunds]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_report_refunds];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rpt_pathology_slide_volume]') AND type = N'P')
    DROP PROCEDURE [dbo].[rpt_pathology_slide_volume];
GO -- Confidence: MEDIUM - May be used by SSRS

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GenerateRefundExtract]') AND type = N'P')
    DROP PROCEDURE [dbo].[sp_GenerateRefundExtract];
GO -- Confidence: MEDIUM - May be run manually

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_job_out_of_balance]') AND type = N'P')
    DROP PROCEDURE [dbo].[sp_job_out_of_balance];
GO -- Confidence: MEDIUM - May be SQL Agent job

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_prg_clinic_contributions]') AND type = N'P')
    DROP PROCEDURE [dbo].[sp_prg_clinic_contributions];
GO -- Confidence: MEDIUM - Reporting proc

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_BadDebt]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_BadDebt];
GO -- Confidence: MEDIUM - May be SQL Agent job

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_aging_accounts_report]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_aging_accounts_report];
GO -- Confidence: MEDIUM - May be SQL Agent job

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateARHistoryByDate]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_UpdateARHistoryByDate];
GO -- Confidence: MEDIUM - May be SQL Agent job

PRINT 'Phase 2b complete: Reporting procedures removed.';
GO

-- Accounting summary procedure chains (parent procs that call sub-procs)
-- Remove parents first, then children

PRINT '========================================';
PRINT 'Phase 2c: Remove Accounting Summary Chain';
PRINT '========================================';
GO

-- Parent procs first
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_summary_cumulative]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_summary_cumulative];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_summary_cumulative_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_summary_cumulative_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_summary_cumulative_by_insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_summary_cumulative_by_insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_summary_cumulative_by_insurance_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_summary_cumulative_by_insurance_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_summary_cumulative_by_selected_insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_summary_cumulative_by_selected_insurance];
GO

-- Child procs (verify no other callers before removing)
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_charges_by_cost_centers]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_charges_by_cost_centers];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_amt_paid_by_cost_center]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_amt_paid_by_cost_center];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_contractual_by_cost_center]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_contractual_by_cost_center];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_write_off_by_cost_center]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_write_off_by_cost_center];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_charges_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_charges_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_amt_paid_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_amt_paid_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_contractual_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_contractual_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_write_off_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_write_off_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_charges_by_insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_charges_by_insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_amt_paid_by_Insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_amt_paid_by_Insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_contractual_by_Insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_contractual_by_Insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_write_off_by_Insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_write_off_by_Insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_charges_by_insurance_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_charges_by_insurance_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_amt_paid_by_Insurance_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_amt_paid_by_Insurance_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_contractual_by_Insurance_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_contractual_by_Insurance_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_write_off_by_Insurance_by_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_write_off_by_Insurance_by_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_charges_by_selected_insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_charges_by_selected_insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_amt_paid_by_selected_insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_amt_paid_by_selected_insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_contractual_by_selected_Insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_contractual_by_selected_Insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_write_off_by_selected_Insurance]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_write_off_by_selected_Insurance];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_write_off_by_Insurance_Accounts]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_write_off_by_Insurance_Accounts];
GO

-- Client report procs
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_report_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_report_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_report_by_cdm]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_report_by_cdm];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_accounting_report_by_cdm_client]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_accounting_report_by_cdm_client];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_charges_by_cost_centers_with_clients]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_charges_by_cost_centers_with_clients];
GO

PRINT 'Phase 2c complete: Accounting summary chain removed.';
GO

-- Remove procs that depend on other procs being removed first
-- usp_prg_ReCharge_JPG calls usp_prg_ReCharge_transaction
-- usp_prg_ReverseCharge depends on usp_prg_ReverseChargeOnly

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ReCharge_JPG]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ReCharge_JPG];
GO -- Must be dropped before usp_prg_ReCharge_transaction

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ReCharge_transaction]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ReCharge_transaction];
GO -- Now safe to drop

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ReverseCharge]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ReverseCharge];
GO -- Must be dropped before usp_prg_ReverseChargeOnly

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_prg_ReverseChargeOnly]') AND type = N'P')
    DROP PROCEDURE [dbo].[usp_prg_ReverseChargeOnly];
GO -- Now safe to drop

PRINT 'Phase 2 fully complete.';
GO

PRINT '========================================';
PRINT 'Phase 3: Remove Orphaned Views';
PRINT '========================================';
GO

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_cdm_3]'))
    DROP VIEW [dbo].[vw_cdm_3];
GO -- Confidence: HIGH - Unused CDM variant

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Acc_Pat_Indexed]'))
    DROP VIEW [dbo].[vw_Acc_Pat_Indexed];
GO -- Confidence: HIGH - Unused indexed view

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_prg_clear_batch_1500]'))
    DROP VIEW [dbo].[vw_prg_clear_batch_1500];
GO -- Confidence: HIGH - Legacy clearing house

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_cdm_4]'))
    DROP VIEW [dbo].[vw_cdm_4];
GO -- Confidence: HIGH - Unused CDM variant

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_cdm_with_del_3]'))
    DROP VIEW [dbo].[vw_cdm_with_del_3];
GO -- Confidence: HIGH - Unused CDM variant

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_tlcres_select]'))
    DROP VIEW [dbo].[vw_tlcres_select];
GO -- Confidence: HIGH - Legacy TLC resource

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_cdm_2]'))
    DROP VIEW [dbo].[vw_cdm_2];
GO -- Confidence: HIGH - Unused CDM variant

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_arhist_client]'))
    DROP VIEW [dbo].[vw_arhist_client];
GO -- Confidence: HIGH - Unused AR history

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_chk_by_client]'))
    DROP VIEW [dbo].[vw_chk_by_client];
GO -- Confidence: HIGH - Unused check view

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_chrg_pc]'))
    DROP VIEW [dbo].[vw_chrg_pc];
GO -- Confidence: HIGH - Unused PC split

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[VW_DBG_PAYMENTS]'))
    DROP VIEW [dbo].[VW_DBG_PAYMENTS];
GO -- Confidence: HIGH - Debug view

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_prg_clear_batch_ub]'))
    DROP VIEW [dbo].[vw_prg_clear_batch_ub];
GO -- Confidence: HIGH - Legacy clearing house

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_tcpc_split]'))
    DROP VIEW [dbo].[vw_tcpc_split];
GO -- Confidence: HIGH - Unused TC/PC split

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_thin_prep]'))
    DROP VIEW [dbo].[vw_thin_prep];
GO -- Confidence: HIGH - Unused thin prep

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_uhc_balance]'))
    DROP VIEW [dbo].[vw_uhc_balance];
GO -- Confidence: HIGH - Unused UHC balance

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_cdm_with_del_2]'))
    DROP VIEW [dbo].[vw_cdm_with_del_2];
GO -- Confidence: HIGH - Unused CDM variant

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[dict_ViewerAccSql]'))
    DROP VIEW [dbo].[dict_ViewerAccSql];
GO -- Confidence: HIGH - Legacy viewer SQL

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_ssi_batch_list]'))
    DROP VIEW [dbo].[vw_ssi_batch_list];
GO -- Confidence: HIGH - Legacy SSI batch list

PRINT 'Phase 3 complete: Orphaned views removed.';
GO

PRINT '========================================';
PRINT 'Phase 4: Remove Unused Tables';
PRINT 'WARNING: Verify no triggers or FK references first!';
PRINT '========================================';
GO

-- HIGH CONFIDENCE: Legacy integration tables

IF OBJECT_ID(N'[dbo].[ssi_remittance_charges]', N'U') IS NOT NULL
    DROP TABLE [dbo].[ssi_remittance_charges];
GO -- Confidence: HIGH - Legacy SSI, drop child first (FK)

IF OBJECT_ID(N'[dbo].[ssi_remittance]', N'U') IS NOT NULL
    DROP TABLE [dbo].[ssi_remittance];
GO -- Confidence: HIGH - Legacy SSI

IF OBJECT_ID(N'[dbo].[data_fincode_d_ssi]', N'U') IS NOT NULL
    DROP TABLE [dbo].[data_fincode_d_ssi];
GO -- Confidence: HIGH - Legacy SSI financial code

IF OBJECT_ID(N'[dbo].[tblPropAccCrossover]', N'U') IS NOT NULL
    DROP TABLE [dbo].[tblPropAccCrossover];
GO -- Confidence: HIGH - Legacy proposed account crossover

IF OBJECT_ID(N'[dbo].[tblPropAcc]', N'U') IS NOT NULL
    DROP TABLE [dbo].[tblPropAcc];
GO -- Confidence: HIGH - Legacy proposed account

IF OBJECT_ID(N'[dbo].[Temp_GlobalBilling]', N'U') IS NOT NULL
    DROP TABLE [dbo].[Temp_GlobalBilling];
GO -- Confidence: HIGH - Temporary table

IF OBJECT_ID(N'[dbo].[chk_electronic_cpt_detail]', N'U') IS NOT NULL
    DROP TABLE [dbo].[chk_electronic_cpt_detail];
GO -- Confidence: HIGH - Legacy electronic check detail

IF OBJECT_ID(N'[dbo].[data_monthly_ins_report]', N'U') IS NOT NULL
    DROP TABLE [dbo].[data_monthly_ins_report];
GO -- Confidence: MEDIUM - May be populated by reporting procs

PRINT 'Phase 4 complete: Unused tables removed.';
GO

PRINT '========================================';
PRINT 'CLEANUP COMPLETE';
PRINT '========================================';
PRINT 'Objects NOT removed (require further investigation):';
PRINT '  - IndexOptimize (likely SQL Agent maintenance job)';
PRINT '  - MenuItemsByUser (may be used by security/menu system)';
PRINT '  - medicare_random (may be SQL Agent job)';
PRINT '  - usp_account_payout (may be SQL Agent job)';
PRINT '  - usp_prg_Credit_Charge (may be manual DBA operation)';
PRINT '  - usp_prg_Charges_With_Payments (may be reporting)';
PRINT '  - usp_quest_cbill (may be Quest integration)';
PRINT '  - usp_infce_psa_demographics (may be interface proc)';
PRINT '  - usp_prg_Quest_Path_Payment (may be Quest integration)';
PRINT '  - usp_prg_Xml_Import_Insurance (XML import chain)';
PRINT '  - All functions used by remaining views/procs';
PRINT '  - All tables that may be populated by triggers';
PRINT '';
PRINT 'NEXT STEPS:';
PRINT '  1. Check SQL Server Agent for jobs referencing removed objects';
PRINT '  2. Check SSRS reports for references to removed objects';
PRINT '  3. Query Monthly_Reports table for references to removed objects';
PRINT '  4. Test application functionality thoroughly';
GO
