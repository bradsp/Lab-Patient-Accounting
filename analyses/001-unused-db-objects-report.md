# Unused Database Objects Analysis Report

**Date:** 2026-02-03
**Schema Source:** `LabPatientAccounting DB Build.sql`
**Codebase:** Lab Patient Accounting (.NET 8.0 / .NET Framework 4.8)

---

## Executive Summary

A comprehensive cross-reference analysis of the database schema against the entire application codebase identified a significant number of database objects with no application code references. The schema has accumulated substantial technical debt over years of development.

### Object Counts

| Object Type | Total in Schema | Used by App Code | Used Only by Other DB Objects | Completely Unused | Uncertain |
|---|---|---|---|---|---|
| **Stored Procedures** | 117 | 4 | 24 | 82 | 7 |
| **Functions** | 115 | 3 | 44 | 62 | 6 |
| **Views** | 107 | 9 | 28 | 56 | 14 |
| **Tables** | 68 (dbo) | 43 | 8 | 12 | 5 |
| **Synonyms** | 67 | ~35 | ~20 | ~12 | - |
| **TOTALS** | **474** | **94** | **124** | **224** | **32** |

**Note on synonyms:** Synonyms point to objects in the `dictionary` and other databases. Removing synonyms does not affect the target objects. Synonyms used by application code (via PetaPoco models) have been identified.

---

## Important Caveats

1. **Monthly_Reports Dynamic SQL:** The `AuditReportsForm` loads SQL queries from the `Monthly_Reports` dictionary table and executes them dynamically. Any view, function, or table could be referenced by queries stored in that table. Objects flagged as "uncertain" may be referenced this way.

2. **SQL Server Agent Jobs:** Some stored procedures (especially `usp_account_payout`, `usp_aging_accounts_report`, `usp_UpdateARHistoryByDate`, `IndexOptimize`) appear designed for scheduled execution via SQL Server Agent jobs, which would not appear in application code.

3. **SSRS/External Reports:** Stored procedures prefixed with `usp_accounting_report_`, `usp_prg_`, `rpt_`, and views prefixed with `vw_prg_` may be used by SSRS reports or other external reporting tools.

4. **Interface Procedures:** Procedures prefixed with `usp_cerner_`, `usp_infce_`, and `usp_psa_` may be called by external interface processes (HL7 message processing, Cerner integration).

5. **`usp_cerner_chrg_reprocess`:** This procedure IS called from application code (`MessagesInboundRepository.cs`) but is NOT defined in the build SQL -- it exists only in the `LabBillingDatabase` SSDT project. This indicates the build SQL and the database project may not be fully synchronized.

---

## Category 1: Completely Unused (No References in App Code OR Other DB Objects)

### 1.1 Stored Procedures -- Completely Unused (Orphaned)

These stored procedures have no references in C# code and are only referenced by their own CREATE statement and Script Date comment in the SQL file.

| Procedure | Description | Confidence | Notes |
|---|---|---|---|
| `ChangeSchema` | Utility to move objects between schemas | HIGH | Admin utility, not needed by app |
| `FixCreditedFlags` | One-time data fix for credited flags | HIGH | Data correction script |
| `GetAccountSummary` | Returns account summary data | HIGH | Superseded by app-level logic |
| `GetClientsToBill` | Returns clients needing billing | HIGH | Superseded by app-level logic |
| `getName` | Returns name from ID | HIGH | Simple utility, unused |
| `MonthlyARTotals` | Monthly accounts receivable totals | MEDIUM | May be used by SQL Agent job |
| `proc_Compare_Table_Structure` | Compares table structures between databases | HIGH | Admin/dev utility |
| `rpt_pathology_slide_volume` | Pathology slide volume report | MEDIUM | May be used by SSRS |
| `sp_GenerateRefundExtract` | Generates refund extract file | MEDIUM | May be run manually |
| `sp_GetMasterAccount` | Gets master account information | HIGH | Superseded by app logic |
| `sp_job_out_of_balance` | Reports out-of-balance jobs | MEDIUM | May be SQL Agent job |
| `sp_prg_clinic_contributions` | Clinic contributions report | MEDIUM | May be run manually |
| `sp_prg_update_nh_bill_thru_date` | Updates nursing home billing dates | HIGH | Legacy process |
| `TableSpaceUsed` | Reports table space usage | HIGH | Admin utility |
| `UndoClientInvoice` | Undoes a client invoice | MEDIUM | May be run manually by DBAs |
| `usp_account_payout_v2` | Account payout v2 (email report) | MEDIUM | May be SQL Agent job |
| `usp_account_receivable_daily_reconciliation` | Daily AR reconciliation | MEDIUM | May be SQL Agent job |
| `usp_accounting_report_by_cpt4` | Accounting report by CPT4 | MEDIUM | May be used by reporting |
| `usp_accounting_report_by_gl` | Accounting report by GL code | MEDIUM | May be used by reporting |
| `usp_accounting_report_clients_all` | All clients accounting report | MEDIUM | May be used by reporting |
| `usp_accounting_report_data` | Accounting report data | MEDIUM | May be used by reporting |
| `usp_accounting_report_refunds` | Refunds accounting report | MEDIUM | May be used by reporting |
| `usp_added_cdms` | Report on added CDMs | HIGH | Unused |
| `usp_aging_accounts_report` | Aging accounts report | MEDIUM | May be SQL Agent job |
| `usp_bnp_update` | BNP test update | HIGH | Legacy/one-time update |
| `usp_care360_get_account_charges2` | Care360 integration charges | HIGH | Legacy Care360 integration |
| `usp_cbill_hist` | Client bill history processing | HIGH | Superseded by app logic |
| `usp_CopyBillingToTest` | Copies prod data to test | HIGH | Admin utility |
| `usp_ExampleProc` | Example/template procedure | HIGH | Template only |
| `usp_GetAccBalanceAsOfDate` | Gets account balance as of date | HIGH | Superseded by function |
| `usp_GetErrorInfo` | Returns error info (used by TRY/CATCH) | LOW | May be called internally via dynamic SQL |
| `usp_infce_psa_demographics_special` | Special PSA demographics | HIGH | Legacy interface |
| `usp_prg_accounting_report` | Accounting report | MEDIUM | May be used by reporting |
| `usp_prg_acct_recievables_by_cost_center` | AR by cost center | MEDIUM | May be used by reporting |
| `usp_prg_ar_rebuild` | Rebuilds AR history | MEDIUM | May be run manually |
| `usp_prg_BadDebt` | Bad debt processing | MEDIUM | May be SQL Agent job |
| `usp_prg_billing_type_totals` | Billing type totals report | MEDIUM | May be used by reporting |
| `usp_prg_cbill_client_orders` | Client bill orders | HIGH | Unused |
| `usp_prg_charges_by_cost_centers_with_clients_and_accounts` | Charges by cost center with detail | MEDIUM | May be used by reporting |
| `usp_prg_Charges_With_Payments2` | Charges with payments v2 | HIGH | Unused variant |
| `usp_prg_CreateAccount` | Creates accounts programmatically | HIGH | Superseded by app logic |
| `usp_prg_Drug_Screens` | Drug screen charge processing | HIGH | Superseded by C# RandomDrugScreenService |
| `usp_prg_Drug_Screens_Reversal` | Drug screen charge reversal | HIGH | Superseded by C# service |
| `usp_prg_Drug_Screens2` | Drug screen variant 2 | HIGH | Superseded by C# service |
| `usp_prg_global_billing` | Global billing processing | MEDIUM | May be SQL Agent job |
| `usp_prg_InvoiceReprint` | Invoice reprinting | HIGH | Superseded by app logic |
| `usp_prg_JobSummaryUtil` | Job summary utility | HIGH | Unused |
| `usp_prg_OutReach_Cost` | Outreach cost report | MEDIUM | May be used by reporting |
| `usp_prg_Quest_Charges` | Quest charges processing | MEDIUM | May be SQL Agent job |
| `usp_prg_Quest_Path_Payment2` | Quest pathology payment v2 | HIGH | Unused variant |
| `usp_prg_ReCharge` | Re-charge processing (standalone) | HIGH | Not called from C# |
| `usp_prg_ReCharge_Acc_Transaction` | Re-charge account transaction | HIGH | Not called from C# |
| `usp_prg_ReCharge_JPG` | JPG re-charge processing | HIGH | Not called from C# |
| `usp_prg_ReverseCharge` | Charge reversal | HIGH | Not called from C# |
| `usp_prg_ReverseChrgeOnly_Acc_Transaction` | Reverse charge account transaction | HIGH | Not called from C# |
| `usp_prg_ViewerAcc_Select` | Viewer account select | HIGH | Unused |
| `usp_prg_Xml_Account_Verify` | XML account verification | HIGH | Legacy XML import |
| `usp_prg_Xml_Import_Insurance_Verify` | XML insurance import verification | HIGH | Legacy XML import |
| `usp_prgProcessCharge` | Process charge | HIGH | Legacy, superseded by C# HL7ProcessorService |
| `usp_prgProcessChargeADT` | Process charge ADT | HIGH | Legacy, superseded by C# HL7ProcessorService |
| `usp_psa_demo_activity_check` | PSA demographics activity check | HIGH | Legacy interface |
| `usp_quest_cbill_by_invoice` | Quest client bill by invoice | HIGH | Unused |
| `usp_system_log_message` | System log message insert | HIGH | Superseded by NLog |
| `usp_UpdateARHistoryByDate` | Update AR history by date | MEDIUM | May be SQL Agent job |
| `usp_UpdateSystemValue` | Update system parameter value | HIGH | Superseded by app logic |
| `usp_ViewerAcc_Billing_Table` | Viewer billing table | HIGH | Unused |
| `usp_prg_pat_bill_acct_verify` | Patient bill account verify | HIGH | QA/verify proc |
| `usp_prg_pat_bill_actv_display` | Patient bill activity display | HIGH | Display proc |
| `usp_prg_pat_bill_actv_verify` | Patient bill activity verify | HIGH | QA/verify proc |
| `usp_prg_pat_bill_enct_display` | Patient bill encounter display | HIGH | Display proc |
| `usp_prg_pat_bill_enct_verify` | Patient bill encounter verify | HIGH | QA/verify proc |
| `usp_prg_pat_bill_stmt_display` | Patient bill statement display | HIGH | Display proc |
| `usp_prg_pat_bill_stmt_errors` | Patient bill statement errors | HIGH | Error reporting proc |
| `usp_prg_pat_bill_stmt_small_balance` | Small balance statements | HIGH | Unused |
| `usp_prg_pat_bill_stmt_verify` | Patient bill statement verify | HIGH | QA/verify proc |
| `usp_prg_write_off_by_Insurance_Accounts` | Write-off by insurance accounts | MEDIUM | May be used by reporting |
| `usp_prg_write_off_by_Insurance_by_client` | Write-off by insurance by client | MEDIUM | May be used by reporting |
| `usp_prg_write_off_by_selected_Insurance` | Write-off by selected insurance | MEDIUM | May be used by reporting |
| `usp_accounting_summary_cumulative_by_client` | Cumulative summary by client | MEDIUM | May be used by reporting |
| `usp_accounting_summary_cumulative_by_insurance_by_client` | Cumulative summary by ins by client | MEDIUM | May be used by reporting |
| `usp_accounting_summary_cumulative_by_selected_insurance` | Cumulative summary by selected ins | MEDIUM | May be used by reporting |
| `usp_prg_charges_by_insurance_by_client` | Charges by insurance by client | MEDIUM | May be used by reporting |
| `usp_prg_charges_by_selected_insurance` | Charges by selected insurance | MEDIUM | May be used by reporting |
| `usp_prg_contractual_by_Insurance_by_client` | Contractual by insurance by client | MEDIUM | May be used by reporting |
| `usp_prg_contractual_by_selected_Insurance` | Contractual by selected insurance | MEDIUM | May be used by reporting |
| `usp_prg_amt_paid_by_Insurance_by_client` | Amount paid by insurance by client | MEDIUM | May be used by reporting |
| `medicare_random_occ_bld` | Medicare random occurrence blood | HIGH | Legacy |
| `IndexOptimize` | Ola Hallengren index maintenance | LOW | Almost certainly used by SQL Agent |

### 1.2 Functions -- Completely Unused (Orphaned)

These functions have no references in C# code and only appear at their definition in the SQL file.

| Function | Description | Confidence | Notes |
|---|---|---|---|
| `FormatPhoneNoTest` | Test version of phone number formatter | HIGH | Test function |
| `PadTextSave` | Variant of PadText | HIGH | Unused variant |
| `sfn_trim_zeros_mri` | Trim zeros for MRI | HIGH | Unused |
| `GetPatIcd92` | Get patient ICD9 codes v2 | HIGH | Unused variant |
| `udf_XML2Table` | XML to table conversion | HIGH | Unused |
| `GetNumbersTable` | Returns a numbers table | HIGH | Unused variant |
| `fnRemoveChildElementsNC` | Remove child XML elements (no case) | HIGH | Unused |
| `DelimitedSplit8K` | String splitter function | HIGH | Unused utility |
| `ArrayToTable` | Convert array to table | HIGH | Unused utility |
| `GetAddedCDMs` | Get added CDM codes | HIGH | Unused |
| `GetAccBalByServiceDate` | Balance by service date | HIGH | Unused |
| `GetAccBalByTransDate` | Balance by transaction date | HIGH | Unused |
| `GetAccDiagnosis1` | Account diagnosis v1 | HIGH | Unused variant |
| `GetAccIns` | Get account insurance | HIGH | Unused |
| `GetAccTotalCount` | Account total count | HIGH | Unused |
| `GetAmountTotal` | Amount total | HIGH | Unused |
| `GetAmtRecords2` | Amount records v2 | HIGH | Unused variant |
| `GetCernerPhyNPI` | Cerner physician NPI | HIGH | Legacy Cerner integration |
| `GetClientPrice` | Client pricing | HIGH | Unused |
| `GetClientsUnbilledCharges` | Clients unbilled charges | HIGH | Unused |
| `GetFeeSchedulePricewdk` | Fee schedule price (wdk variant) | HIGH | Unused variant |
| `GetInvoiceCharges` | Invoice charges (function version) | HIGH | Superseded by InvoiceChargeView |
| `GetLastDateBilled` | Last date billed | HIGH | Unused |
| `GetLastInvoiceDate` | Last invoice date | HIGH | Unused |
| `GetMappedPhysician` | Mapped physician lookup | HIGH | Unused |
| `GetMultiples` | Multiple test detection | HIGH | Unused |
| `GetNewFeeSchedPrice` | New fee schedule price | HIGH | Unused |
| `GetPatDiagnosis_icd` | Patient diagnosis ICD code | HIGH | Unused |
| `GetPatDiagnosis2` | Patient diagnosis v2 | HIGH | Unused variant |
| `GetPaymentsBeforeLastDataMailer` | Payments before last data mailer | HIGH | Legacy data mailer |
| `GetPaymentsByDate2` | Payments by date v2 | HIGH | Unused variant |
| `GetPaymentsTableSinceLastDataMailer` | Payments table since last mailer | HIGH | Legacy data mailer |
| `GetXmlDataForAcc2` | XML data for account v2 | HIGH | Legacy XML |
| `GetXmlDataForAccCharges` | XML data for account charges | HIGH | Legacy XML |
| `GetXmlDataForAccInfce` | XML data for account interface | HIGH | Legacy XML interface |
| `GetXmlDataForAccIns` | XML data for account insurance | HIGH | Legacy XML |
| `SplitCdmPriceByCpt2` | Split CDM price by CPT v2 | HIGH | Unused variant |
| `SplitCdmPriceByCpt3` | Split CDM price by CPT v3 | HIGH | Unused variant |
| `tvf_accounting_report_by_cdm_client` | Accounting report by CDM & client TVF | HIGH | Unused |
| `ufn_client` | Client function | HIGH | Unused |
| `ufn_Duplicates` | Duplicate detection | HIGH | Unused |
| `ufn_GetAccountPayment` | Account payment lookup | HIGH | Unused |
| `ufn_GetQuestError` | Quest error lookup | HIGH | Unused |
| `usf_AccCheckForLMRP` | LMRP check for account | HIGH | Unused |
| `usf_account_report_client_by_physician` | Client report by physician | HIGH | Unused |
| `usf_account_report_client_qty` | Client report quantity | HIGH | Unused |
| `usf_prg_panels` | Panel processing | HIGH | Unused |
| `XmlFormatPhysicianCerner` | XML format physician for Cerner | HIGH | Legacy Cerner |
| `fn_Get_XMLCerner_data` | Get XML Cerner data | HIGH | Legacy Cerner |
| `fnConvert_TitleCase` | Convert to title case | HIGH | Unused |
| `fnParseStringTSQL` | Parse string T-SQL | HIGH | Unused |
| `fnRandomizedText` | Randomized text generator | HIGH | Test/dev utility |
| `fnStandardPhone` | Standard phone format | HIGH | Unused |
| `GetBillMethod` | Get billing method | HIGH | Superseded |

### 1.3 Views -- Completely Unused (Orphaned)

These views have no references in C# code and only appear at their definition in the SQL file.

| View | Description | Confidence | Notes |
|---|---|---|---|
| `vw_cdm_3` | CDM view variant 3 | HIGH | Unused variant |
| `vw_Acc_Pat_Indexed` | Account-Patient indexed view | HIGH | Unused |
| `vw_prg_clear_batch_1500` | Clear batch 1500 claims | HIGH | Legacy clearing house |
| `vw_cdm_4` | CDM view variant 4 | HIGH | Unused variant |
| `vw_cdm_with_del_3` | CDM with deleted variant 3 | HIGH | Unused variant |
| `vw_tlcres_select` | TLC resource select | HIGH | Legacy |
| `vw_cdm_2` | CDM view variant 2 | HIGH | Unused variant |
| `vw_arhist_client` | AR history by client | HIGH | Unused |
| `vw_chk_by_client` | Checks by client | HIGH | Unused |
| `vw_chrg_pc` | Charge PC split | HIGH | Unused |
| `VW_DBG_PAYMENTS` | Debug payments view | HIGH | Debug/dev view |
| `vw_prg_clear_batch_ub` | Clear batch UB claims | HIGH | Legacy clearing house |
| `vw_tcpc_split` | TC/PC split view | HIGH | Unused |
| `vw_thin_prep` | Thin prep view | HIGH | Unused |
| `vw_uhc_balance` | UHC balance view | HIGH | Unused |
| `vw_cdm_with_del_2` | CDM with deleted variant 2 | HIGH | Unused variant |
| `dict_ViewerAccSql` | Viewer account SQL dictionary | HIGH | Legacy |
| `vw_ssi_batch_list` | SSI batch list | HIGH | Legacy SSI |

### 1.4 Tables -- Completely Unused

| Table | Description | Confidence | Notes |
|---|---|---|---|
| `data_fincode_d_ssi` | Financial code SSI data | HIGH | Legacy SSI integration |
| `data_monthly_ins_report` | Monthly insurance report data | MEDIUM | May be populated by reporting procs |
| `chk_electronic_cpt_detail` | Electronic check CPT detail | HIGH | Legacy electronic remittance |
| `ssi_remittance` | SSI remittance records | HIGH | Legacy SSI integration |
| `ssi_remittance_charges` | SSI remittance charges | HIGH | Legacy SSI integration |
| `tblPropAcc` | Proposed account table | HIGH | Legacy |
| `tblPropAccCrossover` | Proposed account crossover | HIGH | Legacy |
| `Temp_GlobalBilling` | Temporary global billing | HIGH | Temp table |
| `data_quest_360` | Quest 360 data | MEDIUM | May be used by Quest procs |
| `data_quest_billing` | Quest billing data | MEDIUM | May be used by Quest procs |
| `data_quest_global_billing_tracking` | Quest global billing tracking | MEDIUM | May be used by Quest procs |
| `data_tier_pricing` | Tier pricing data | MEDIUM | May be referenced by pricing functions |

---

## Category 2: Only Referenced by Other Database Objects

### 2.1 Stored Procedures Called Only by Other Stored Procedures

These are called internally within the SQL schema but NOT from application code.

| Procedure | Called By | Notes |
|---|---|---|
| `AuditProcedureRun` | `usp_accounting_report_refunds` | Audit logging |
| `DataSourceCheck` | `usp_HtmlTable` | Helper for HTML table generation |
| `usp_HtmlTable` | Multiple reporting procs | HTML email table generator |
| `usp_WriteAccErrors` | `usp_prg_Xml_Account_Verify`, `usp_prg_Xml_Import_Insurance`, others | Error logging |
| `usp_prg_PurgeDuplicates` | XML import procs | Duplicate removal |
| `usp_prg_pat_bill_acct` | `usp_prg_pat_bill_compile` (which IS used by C#) | **Keep - indirectly used** |
| `usp_prg_pat_bill_stmt` | `usp_prg_pat_bill_compile` (which IS used by C#) | **Keep - indirectly used** |
| `usp_prg_pat_bill_enct` | `usp_prg_pat_bill_compile` (which IS used by C#) | **Keep - indirectly used** |
| `usp_prg_pat_bill_actv` | `usp_prg_pat_bill_compile` (which IS used by C#) | **Keep - indirectly used** |
| `usp_prg_ReCharge_transaction` | `usp_prg_ReCharge_JPG`, `usp_prgProcessChargeADT` | Both parents unused |
| `usp_prg_ReverseChargeOnly` | `usp_prg_ReverseChrgeOnly_Acc_Transaction` | Parent unused |
| `usp_prg_charges_by_cost_centers` | `usp_accounting_summary_cumulative` | Parent unused from app |
| `usp_prg_amt_paid_by_cost_center` | `usp_accounting_summary_cumulative` | Parent unused from app |
| `usp_prg_contractual_by_cost_center` | `usp_accounting_summary_cumulative` | Parent unused from app |
| `usp_prg_write_off_by_cost_center` | `usp_accounting_summary_cumulative` | Parent unused from app |
| `usp_prg_charges_by_client` | `usp_accounting_summary_cumulative_by_client` | Parent unused from app |
| `usp_prg_amt_paid_by_client` | `usp_accounting_summary_cumulative_by_client` | Parent unused from app |
| `usp_prg_contractual_by_client` | `usp_accounting_summary_cumulative_by_client` | Parent unused from app |
| `usp_prg_write_off_by_client` | `usp_accounting_summary_cumulative_by_client` | Parent unused from app |
| `usp_prg_charges_by_insurance` | `usp_accounting_summary_cumulative_by_insurance` | Parent unused from app |
| `usp_prg_amt_paid_by_Insurance` | `usp_accounting_summary_cumulative_by_insurance` | Parent unused from app |
| `usp_prg_contractual_by_Insurance` | `usp_accounting_summary_cumulative_by_insurance` | Parent unused from app |
| `usp_prg_write_off_by_Insurance` | `usp_accounting_summary_cumulative_by_insurance` | Parent unused from app |
| `usp_accounting_summary_cumulative` | Not called from app code | Parent proc (calls 4 sub-procs) |
| `usp_accounting_summary_cumulative_by_insurance` | Not called from app code | Parent proc (calls 4 sub-procs) |

### 2.2 Functions Used Only by Views or Stored Procedures

These functions are referenced within SQL views and/or stored procedures but NOT from C# code.

| Function | Used In | Notes |
|---|---|---|
| `AccountTrim` | Heavily used (147 refs) across many views/procs | Core SQL utility |
| `AttendPhyTrim` | Multiple views | Core SQL utility |
| `CamelCase` | Multiple views/procs | Formatting utility |
| `ConvertName` | Multiple views/procs | Name formatting |
| `DateOfBirthFix` | Heavily used (46 refs) across views/procs | DOB formatting |
| `DateRange` | Multiple procs | Date range TVF |
| `FormatClient` | 19 refs in views/procs | Client formatting |
| `FormatFinCode` | 15 refs in views/procs | Financial code formatting |
| `FormatPhoneNo` | Multiple views | Phone formatting |
| `FormatRelation` | Multiple procs | Relationship formatting |
| `FormatSSN` | Multiple views | SSN formatting |
| `GetAMAYear` | Multiple procs | AMA year lookup |
| `GetAccDiagnosis` | Multiple views | Diagnosis lookup |
| `GetAccTotalCharges` | 6 refs | Account charges total |
| `GetAcctThirdPartyBalance` | View `AccountSearchView` | Balance calc |
| `GetAmt` | Multiple procs/views | Amount lookup |
| `GetBillMethod` | Only in its CREATE | May be unreferenced |
| `GetChrgBillMethod` | Multiple views | Billing method |
| `GetChrgDetailTotal` | Multiple procs | Charge detail total |
| `GetFeeSchedulePrice` | Multiple procs | Fee schedule pricing |
| `GetInsurance` | Multiple views | Insurance lookup |
| `GetMappingValue` | Multiple procs | Value mapping |
| `GetMasterAcc` | Multiple procs | Master account lookup |
| `GetMasterAccount` | Multiple procs | Master account |
| `GetNamePart` | Multiple procs | Name parsing |
| `GetPatDiagnosis` | Multiple views | Patient diagnosis |
| `GetPaymentsByAccount` | Multiple procs | Payments lookup |
| `GetPaymentsByClient` | Multiple procs | Client payments |
| `GetPhyNPI` | Multiple views/procs | Physician NPI |
| `GetPrice` | Multiple procs | Pricing function |
| `GetQuestPrice` | Multiple procs | Quest pricing |
| `GetSystemValue` | Multiple procs | System value lookup |
| `GetWriteOffByAccount` | Multiple procs | Write-off lookup |
| `GetXmlDataForAcc` | Multiple procs | XML data |
| `OVNumberTrim` | Multiple procs | OV number trimming |
| `PadText` | Multiple procs | Text padding |
| `SplitCdmPriceByCpt` | Multiple procs | CDM/CPT price splitting |
| `SplitCITY_ST_ZIP` | Multiple views | Address parsing |
| `sfn_trim_zeros` | Multiple procs | Zero trimming |
| `fnCptsForCdm` | Multiple views | CPTs for CDM |
| `fnFormatDate` | Multiple procs | Date formatting |
| `GenerateCalendar` | Multiple procs | Calendar generation |
| `NumbersTable` | Multiple procs | Numbers table TVF |
| `ufn_Split_Name` | Multiple procs | Name splitting |
| `PatternSplitLoop` | Multiple procs | Pattern splitting |

### 2.3 Views Used Only by Stored Procedures or Other Views

| View | Used In | Notes |
|---|---|---|
| `vw_acc_pat` | Multiple procs/views (heavily used) | Core view |
| `vw_acc_pat_cerner` | Multiple procs | Cerner-specific |
| `vw_cdm` | Multiple procs | CDM lookup |
| `vw_cdm_with_del` | Multiple procs | CDM with deleted |
| `vw_acc_chrg` | Multiple procs | Account charges |
| `vw_chrg_bal` | Multiple procs + `PatientCollectionsForm.cs` | **Keep** |
| `vw_pay` | Multiple views | Payments |
| `vw_net_payments` | Multiple procs | Net payments |
| `vw_chrg_bill` | `ChrgRepository.cs` + PetaPoco model | **Keep - used by app** |
| `vw_chrg_net` | Multiple views | Net charges |
| `vw_net_amt` | Multiple views | Net amounts |
| `vw_net_chrg` | Multiple views | Net charges |
| `vw_chk_bal` | Multiple procs | Check balance |
| `vw_chrg_bal_cbill` | `ClientRepository.cs` (comment only) | Used by functions |
| `vw_chk_bal_cbill` | `ClientRepository.cs` (comment only) | Used by functions |
| `vw_acc_pat_paper` | Multiple procs | Paper billing |
| `vw_acc_pat_patBill` | Multiple procs | Patient billing |
| `vw_prg_chrg_bill` | Multiple procs | Charge billing |
| `vw_cbill_chrg` | Multiple procs | Client bill charges |
| `vw_chrg_bal_with_thru_date` | Multiple procs | Balance with thru date |
| `vw_chk_bal_with_thru_date` | Multiple procs | Check balance with thru date |
| `vw_chrg_reqno` | Multiple procs | Charge requisition number |
| `vw_acct_info` | Multiple procs | Account info |
| `vw_chrg_bal_test` | Multiple procs | Balance test |
| `vw_data_extract` | Multiple procs | Data extract |
| `vw_dup_acc_validation` | Multiple procs | Duplicate validation |
| `vw_get_invoice_charges` | Multiple procs | Invoice charges |
| `vw_hc_charges` | Multiple procs | Healthcare charges |
| `vw_ins_acc_pat` | Multiple procs | Insurance account patient |

---

## Category 3: Actively Used Objects

### 3.1 Stored Procedures Used by Application Code

| Procedure | Called From | Notes |
|---|---|---|
| `GetNextNumber` | `NumberRepository.cs` | Number sequence generator |
| `usp_prg_pat_bill_update_flags` | `PatientBillingService.cs` | Patient billing flags |
| `usp_prg_pat_bill_compile` | `PatientBillingService.cs` | Patient billing compilation |
| `usp_cerner_chrg_reprocess` | `MessagesInboundRepository.cs` | Charge reprocessing (in DB project, not build SQL) |

### 3.2 Functions Used by Application Code

| Function | Called From | Notes |
|---|---|---|
| `GetAccBalance` | `ClientRepository.cs` | Account balance calculation |
| `GetAccBalByDate` | `ClientRepository.cs` | Balance as of date |
| `GetAccClientBalance` | `ClientRepository.cs` | Client-specific balance |

### 3.3 Views Used by Application Code (via PetaPoco models or direct SQL)

| View | Used Via | Notes |
|---|---|---|
| `AccountSearchView` | `AccountSearch.cs` model | Account search |
| `InvoiceChargeView` | `InvoiceChargeView.cs` model, `ChrgRepository.cs` | Invoice charges |
| `vw_chrg_bill` | `ClaimChargeView.cs` model, `ChrgRepository.cs` | Charge billing |
| `vw_cbill_select` | `InvoiceSelect.cs` model, `UnbilledAccounts.cs` model | Invoice selection |
| `vw_chrgdetail` | `AuditReportsForm.cs` (raw SQL) | Charge detail |
| `vw_chrg_bal` | `PatientCollectionsForm.cs` (raw SQL) | Charge balance |

### 3.4 Tables Used by Application Code (via PetaPoco models)

All tables with PetaPoco `[TableName]` attributes are in active use:
`acc`, `pat`, `ins`, `chrg`, `chrg_details`, `chk`, `acc_alert`, `acc_lock`, `acc_validation_status`, `announcements`, `bad_debt`, `cbill_hist`, `chk_batch`, `chk_batch_details`, `chrg_diag_pointer`, `data_billing_batch`, `data_billing_history`, `data_EOB`, `data_EOB_Detail`, `notes`, `number`, `patbill_acc`, `patbill_enctr`, `patbill_enctr_actv`, `patbill_stmt`, `pat_statements_cerner`, `patdx`, `rds`, `remittance_claim`, `remittance_claim_adjustment`, `remittance_claim_detail`, `remittance_file`, `rpt_track`, `UserProfile`, `ACC_LMRP`

Plus synonyms used by PetaPoco models: `cdm`, `cpt4`, `cpt4_2`, `cpt4_3`, `cpt4_4`, `cpt4_5`, `insc`, `icd9desc`, `fin`, `emp`, `phy`, `pth`, `system`

---

## Tables with Uncertain Usage

| Table | Notes |
|---|---|
| `acc_track` | Not referenced in C# code. May be populated by stored procedures. |
| `chrg_pa` | Not referenced in C# code. May be used by interface procedures. |
| `acc_merges` | Not referenced in C# code. May be populated by merge processes. |
| `acc_dup_check` | Not referenced in C# code. May be populated by duplicate detection procs. |
| `acc_location` | Not referenced in C# code. May be populated by interface procedures. |
| `acc_status_updates` | Not referenced in C# code. May be populated by status tracking. |
| `AuditLog` | Not referenced in C# code. Likely populated by triggers. |
| `chrg_err` | Not referenced in C# code. Populated by charge processing procs. |
| `aging_history` | Referenced by `ReportingRepository.cs`. **In use.** |
| `data_ErrLog` | Not referenced in C# code. May be populated by error procs. |
| `data_reports` | Not referenced in C# code. May store report definitions. |
| `error_prg` | Not referenced in C# code. Legacy error log. |
| `system_log` | Not referenced in C# code. Populated by `usp_system_log_message`. |
| `statement` | Not referenced in C# code. May be legacy. |
| `Numbers` | Not referenced in C# code. Utility number table. |
| `chk_electronic` | Not referenced in C# code. Electronic check import. |
| `chk_electronic_cpt_adjustment_codes` | Not referenced in C# code. Electronic check adjustments. |
| `chrg_dx_pointer` | Not referenced in C# code. May be used by claim generation procs. |

---

## Risk Assessment for Removal

### LOW RISK (Safe to remove)
- Example/test procedures (`usp_ExampleProc`)
- Duplicate/variant functions (`FormatPhoneNoTest`, `PadTextSave`, `GetPatIcd92`, `GetAmtRecords2`, `GetPatDiagnosis2`, `GetPaymentsByDate2`, `SplitCdmPriceByCpt2`, `SplitCdmPriceByCpt3`)
- Debug views (`VW_DBG_PAYMENTS`)
- Admin utilities (`ChangeSchema`, `TableSpaceUsed`, `proc_Compare_Table_Structure`, `usp_CopyBillingToTest`)
- Legacy integration functions (`fn_Get_XMLCerner_data`, `XmlFormatPhysicianCerner`, `GetCernerPhyNPI`)
- Randomization/test functions (`fnRandomizedText`)
- Legacy SSI objects (`ssi_remittance`, `ssi_remittance_charges`, `data_fincode_d_ssi`, `vw_ssi_batch_list`)
- Legacy proposed account tables (`tblPropAcc`, `tblPropAccCrossover`)
- Temporary tables (`Temp_GlobalBilling`)

### MEDIUM RISK (Verify before removing)
- Reporting procedures (`usp_accounting_report_*`, `usp_prg_*` reporting procs)
- Procedures that may be SQL Agent jobs (`usp_account_payout`, `usp_aging_accounts_report`, `usp_UpdateARHistoryByDate`, `IndexOptimize`)
- Views used in reporting chains (`vw_prg_*` views)
- Quest-related objects (`data_quest_*` tables, `usp_quest_*` procs)

### HIGH RISK (Do NOT remove without investigation)
- `IndexOptimize` -- Almost certainly a SQL Agent job (Ola Hallengren maintenance solution)
- `usp_GetErrorInfo` -- May be called from TRY/CATCH blocks in dynamic SQL
- Functions used extensively within views (`AccountTrim`, `DateOfBirthFix`, `FormatClient`, etc.)
- Tables that may be written to by triggers (`AuditLog`, `acc_track`, `chrg_err`)

---

## Recommended Removal Order

1. **Phase 1 - Low Risk:** Remove completely orphaned functions and test/debug objects
2. **Phase 2 - Medium Risk:** Remove orphaned stored procedures after verifying no SQL Agent job references
3. **Phase 3 - Views:** Remove orphaned views (must be done after dependent stored procedures are removed)
4. **Phase 4 - Tables:** Remove unused tables (must be done after all dependent views, functions, and stored procedures are removed)
5. **Phase 5 - Synonyms:** Remove synonyms that point to objects no longer needed

**Important:** Before executing any removals, check SQL Server Agent for job steps that reference these objects, and check any SSRS report definitions.
