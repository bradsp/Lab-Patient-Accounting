# Stored Procedure & Function Optimization Report

**Database:** LabPatientAccounting DB Build.sql
**Analysis Date:** 2026-02-03
**Analyst:** Claude (automated analysis)
**SQL Server Target:** 2014+

---

## Executive Summary

The LabPatientAccounting database contains **109 stored procedures** and **107 user-defined functions** (both scalar and table-valued). Analysis identified **critical performance anti-patterns** concentrated in a small number of high-impact procedures that are called from the application's core billing and HL7 processing workflows.

### Key Findings

| Severity | Count | Description |
|----------|-------|-------------|
| Critical | 7 | Scalar functions in SELECT lists causing RBAR, non-sargable date predicates, nested scalar calls |
| High | 11 | @@IDENTITY usage, missing transactions, SELECT * patterns, duplicate code |
| Medium | 18 | TOP(100) PERCENT, IN(SELECT) vs EXISTS, hardcoded values, commented-out code |
| Low | 15+ | Missing SET NOCOUNT ON, inconsistent error handling, empty procedures |

### Estimated Performance Impact

The top 3 critical optimizations alone could yield **10-100x improvement** for patient billing and HL7 processing operations:

1. **Replacing scalar functions with inline expressions** in `usp_prg_pat_bill_acct_verify` and `usp_prg_pat_bill_update_flags` - these call `GetAccBalByDate` (which itself has non-sargable date logic) per-row on potentially thousands of accounts.
2. **Inlining `GetNamePart`** in HL7 procedures - called 30+ times per row in `usp_infce_psa_demographics` and `infce.usp_infce_demo_orders_to_care360`.
3. **Fixing non-sargable date comparisons** in `GetAccBalByDate`, `GetAccBalByTransDate`, and `GetGCodeBillingCharges` - these wrap indexed columns in CONVERT functions, preventing index seeks.

---

## Phase 1: Complete Catalog

### Stored Procedures (109 total)

#### Core Application Procedures (Called from C#)

| Procedure | Lines | Called From | Purpose |
|-----------|-------|-------------|---------|
| `GetAccountSummary` | 22 | WinForms UI | Account summary with scalar function calls |
| `GetClientsToBill` | 32 | PatientBillingService | Client billing compilation |
| `GetNextNumber` | 12 | NumberRepository | Sequence number generation |
| `usp_prg_pat_bill_update_flags` | 150 | PatientBillingService | Patient billing flag updates |
| `usp_prg_pat_bill_compile` | 44 | PatientBillingService | Patient billing compilation |
| `usp_cerner_chrg_reprocess` | N/A | MessagesInboundRepository | Charge reprocessing |
| `MenuItemsByUser` | 50 | MainForm (StoredProcedure) | Menu items for user |

#### Patient Billing Procedures

| Procedure | Lines | Severity | Key Anti-Patterns |
|-----------|-------|----------|-------------------|
| `usp_prg_pat_bill_acct_verify` | 240 | **Critical** | 6+ scalar function calls per row, non-sargable dates, TOP(100)PERCENT |
| `usp_prg_pat_bill_actv` | 181 | **High** | Scalar function calls per row, TOP(100)PERCENT |
| `usp_prg_pat_bill_actv_display` | 169 | **High** | Similar patterns to actv |
| `usp_prg_pat_bill_actv_verify` | 174 | **High** | Similar patterns |
| `usp_prg_pat_bill_stmt` | 500 | **High** | Scalar functions, complex multi-CTE |
| `usp_prg_pat_bill_stmt_display` | 242 | Medium | Similar patterns |
| `usp_prg_pat_bill_stmt_errors` | 205 | Medium | Similar patterns |
| `usp_prg_pat_bill_stmt_small_balance` | 210 | Medium | Similar patterns |
| `usp_prg_pat_bill_stmt_verify` | 243 | Medium | Similar patterns |
| `usp_prg_pat_bill_update_flags` | 150 | **Critical** | GetAccBalByDate in WHERE + SELECT, per-row scalar evaluation |
| `usp_prg_pat_bill_compile` | 44 | Low | Simple |
| `usp_prg_pat_bill_acct` | 250 | **High** | Scalar functions per row |
| `usp_prg_pat_bill_enct` | 229 | Medium | Scalar functions |
| `usp_prg_pat_bill_enct_display` | 229 | Medium | Similar |
| `usp_prg_pat_bill_enct_verify` | 228 | Medium | Similar |

#### HL7 Interface Procedures

| Procedure | Lines | Severity | Key Anti-Patterns |
|-----------|-------|----------|-------------------|
| `usp_infce_psa_demographics` | 242 | **Critical** | GetNamePart 30+ times per row, correlated subqueries with FOR XML PATH |
| `usp_infce_psa_demographics_special` | 224 | **Critical** | Near-duplicate of above, hardcoded dates |
| `infce.usp_infce_demo_orders_to_care360` | 222 | **Critical** | GetNamePart 10+ times, GetPatIcd9 TVF per row, massive correlated subqueries |
| `infce.usp_mfn_hl7_processor` | 136 | Medium | SELECT * INTO #temp, proper TRY/CATCH |

#### Charge Processing Procedures

| Procedure | Lines | Severity | Key Anti-Patterns |
|-----------|-------|----------|-------------------|
| `usp_prg_Credit_Charge` | 139 | **High** | @@IDENTITY, SELECT *, no transaction, PRINT statements |
| `usp_prg_Drug_Screens` | 102 | **High** | No transaction for multi-table operations |
| `usp_prg_Drug_Screens_Reversal` | 112 | **High** | @@IDENTITY, no transaction, string parsing with CHARINDEX |
| `usp_prg_Drug_Screens2` | 104 | **High** | Similar patterns to Drug_Screens |
| `usp_prg_CreateAccount` | 89 | **High** | No transaction (race condition on sequence), no TRY/CATCH |
| `usp_prg_ReCharge` | 58 | Medium | Complex multi-step without transaction |
| `usp_prg_ReverseCharge` | 146 | Medium | Similar charge reversal patterns |
| `usp_prg_Quest_Charges` | 245 | Medium | Multiple NOT EXISTS checks, no transaction |
| `usp_prgProcessCharge` | 23 | Low | Simple wrapper |
| `usp_prgProcessChargeADT` | 51 | Low | Simple wrapper |

#### Accounting/Reporting Procedures

| Procedure | Lines | Severity | Key Anti-Patterns |
|-----------|-------|----------|-------------------|
| `usp_account_payout` | 193 | Medium | IN(SELECT) vs EXISTS, NOT(...IN), hardcoded email |
| `usp_account_payout_v2` | 202 | Low | Already optimized version |
| `usp_account_receivable_daily_reconciliation` | 63 | Medium | Non-sargable CONVERT in GROUP BY |
| `usp_accounting_report_by_cdm` | 70 | Low | SELECT * from cdm subquery |
| `usp_accounting_report_by_cdm_client` | 78 | Low | Similar |
| `usp_accounting_report_by_cpt4` | 81 | Low | Similar |
| `usp_accounting_report_by_gl` | 58 | Low | Simple |
| `usp_accounting_report_client` | 293 | Medium | Complex multi-CTE |
| `usp_accounting_report_clients_all` | 292 | Medium | Similar |
| `usp_accounting_report_data` | 69 | Low | Simple |
| `usp_accounting_report_refunds` | 69 | Low | Simple |
| `usp_accounting_summary_cumulative` | 119 | Low | Well-structured |
| `usp_accounting_summary_cumulative_by_client` | 187 | Low | Similar |
| `usp_accounting_summary_cumulative_by_insurance` | 181 | Low | Similar |
| `usp_accounting_summary_cumulative_by_insurance_by_client` | 185 | Low | Similar |
| `usp_accounting_summary_cumulative_by_selected_insurance` | 184 | Low | Similar |
| `usp_prg_accounting_report` | 42 | Low | Simple |
| `MonthlyARTotals` | 24 | Low | Simple |

#### Other Procedures

| Procedure | Lines | Severity | Key Anti-Patterns |
|-----------|-------|----------|-------------------|
| `GetAccountSummary` | 22 | **Critical** | 7 scalar function calls for a single row |
| `GetClientsToBill` | 32 | **High** | Scalar functions GetAccBalance and GetAccClientBalance in WHERE clause |
| `usp_prg_global_billing` | 51 | Medium | CROSS APPLY to GetGlobalBillingCharges TVF |
| `usp_prg_Xml_Account_Verify` | 298 | Medium | Complex XML processing |
| `usp_prg_Xml_Import_Insurance` | 511 | Medium | Large XML import |
| `usp_prg_Xml_Import_Insurance_Verify` | 412 | Medium | Verification |
| `sp_GenerateRefundExtract` | 342 | Medium | Well-structured but uses xp_cmdshell |
| `sp_GetMasterAccount` | 212 | Medium | Recursive CTE |
| `sp_job_out_of_balance` | 172 | Low | Maintenance |
| `sp_prg_clinic_contributions` | 45 | Low | Simple |
| `sp_prg_update_nh_bill_thru_date` | 129 | Low | Simple |
| `FixCreditedFlags` | 41 | Low | Proper TRY/CATCH/transaction |
| `IndexOptimize` | 2207 | N/A | Ola Hallengren maintenance (third-party) |
| `Indexes_Unused_Select` | 93 | N/A | Maintenance utility with cursor |
| `AuditProcedureRun` | 16 | Low | Simple audit |
| `ChangeSchema` | 21 | Low | DDL utility |
| `DataSourceCheck` | 73 | Low | Utility |
| `getName` | 8 | Low | Simple query |
| `TableSpaceUsed` | 47 | Low | Utility |
| `UndoClientInvoice` | 43 | Low | Simple |
| `medicare_random` | 16 | Low | SELECT INTO, DROP TABLE |
| `medicare_random_occ_bld` | 18 | Low | Similar |
| `usp_ExampleProc` | 14 | N/A | Example template |
| `usp_GetErrorInfo` | 23 | Low | Error info helper |
| `usp_HtmlTable` | 143 | Low | HTML generation utility |
| `utility.Log_ProcedureCall` | 8 | Low | Empty no-op |
| `usp_system_log_message` | 33 | Low | Logging |
| `usp_UpdateARHistoryByDate` | 104 | Low | Simple update |
| `usp_UpdateSystemValue` | 36 | Low | Simple update |
| `usp_ViewerAcc_Billing_Table` | 44 | Low | Simple select |
| `usp_WriteAccErrors` | 38 | Low | Simple insert |
| `usp_CopyBillingToTest` | 101 | Low | Data copy utility |
| `usp_GetAccBalanceAsOfDate` | 47 | Low | Wrapper |
| `usp_prg_ViewerAcc_Select` | 122 | Low | Account viewer |
| `usp_prg_ar_rebuild` | 83 | Low | AR rebuild |
| `usp_prg_BadDebt` | 49 | Low | Bad debt processing |
| `usp_prg_InvoiceReprint` | 96 | Low | Invoice reprint |
| `usp_prg_JobSummaryUtil` | 83 | Low | Job summary |
| `usp_prg_OutReach_Cost` | 83 | Low | Cost report |
| `usp_prg_billing_type_totals` | 57 | Low | Summary report |
| `usp_prg_cbill_client_orders` | 34 | Low | Client orders |
| `usp_cbill_hist` | 75 | Low | History lookup |
| `usp_added_cdms` | 32 | Low | CDM lookup |
| `usp_aging_accounts_report` | 56 | Low | Aging report |
| `usp_bnp_update` | 87 | Low | BNP update |
| `usp_care360_get_account_charges2` | 87 | Low | Care360 charges |
| `usp_psa_demo_activity_check` | 51 | Low | Activity check |
| `usp_quest_cbill` | 100 | Low | Quest billing |
| `usp_quest_cbill_by_invoice` | 150 | Low | Quest billing by invoice |

#### Legacy/Archive (zzz schema)

| Procedure | Lines | Notes |
|-----------|-------|-------|
| `zzz.usp_prg_UpdateGCodeBilling` | 375 | Legacy G-code billing |
| `zzz.usp_prg_UpdateGlobalBilling` | 377 | Legacy global billing |
| `zzz.usp_prg_UpdateGlobalBilling2` | 352 | Legacy global billing v2 |
| `zzz.usp_prg_Xml_Import` | 1522 | Legacy XML import |
| `zzz.usp_prg_Xml_Import_Accounts` | 383 | Legacy accounts import |
| `zzz.usp_prg_Xml_Import_Accounts_Verify` | 327 | Legacy verify |
| `zzz.usp_prg_Xml_Import_Charges` | 363 | Legacy charges import |
| `zzz.usp_prg_Xml_Import_Charges_Verify` | 253 | Legacy verify |
| `zzz.usp_prg_Xml_Import_Location` | 172 | Legacy location import |
| `zzz.usp_prg_Xml_Import_Patients` | 428 | Legacy patients import |
| `zzz.usp_prg_Xml_Import_Patients_Verify` | 281 | Legacy verify |
| `zzz.usp_prg_Xml_Import_UnProcessed_Charges` | 162 | Legacy charges |
| `zzz.usp_prg_Xml_Import_UnProcessed_Charges_Verify` | 50 | Legacy verify |

### User-Defined Functions (107 total)

#### Critical Scalar Functions (Called per-row in stored procedures)

| Function | Return Type | Lines | Severity | Key Anti-Patterns |
|----------|-------------|-------|----------|-------------------|
| `GetAccBalByDate` | NUMERIC(18,2) | 46 | **Critical** | Non-sargable: `CONVERT(DATETIME,convert(varchar(10),chrg_details.mod_date,101))`. Called 6+ times per row in patient billing |
| `GetAccBalByTransDate` | NUMERIC(18,2) | 46 | **Critical** | Same non-sargable CONVERT pattern on service_date |
| `GetAccTotalCharges` | NUMERIC(18,2) | 32 | **Critical** | Calls `GetChrgDetailTotal` (another scalar) inside SUM() = O(n^2) |
| `GetNamePart` | VARCHAR(100) | 66 | **Critical** | Pure string manipulation called 30+ times per row in HL7 procedures. Uses PARSENAME (4-part max) |
| `GetAccBalance` | NUMERIC(18,2) | 42 | **High** | Scalar aggregation function called per-row in GetClientsToBill WHERE clause |
| `GetAccClientBalance` | NUMERIC(18,2) | 32 | **High** | Scalar function called per-row in GetClientsToBill |
| `GetChrgDetailTotal` | NUMERIC(18,2) | 18 | **High** | Called inside SUM in GetAccTotalCharges (nested scalar) |
| `GetPaymentsSinceLastDataMailer` | NUMERIC(18,2) | 36 | **High** | JOINs chk and pat per invocation. Called per-row in patient billing |
| `GetContractualByAccount` | NUMERIC(18,2) | 38 | **High** | JOINs chk and pat per invocation. Called per-row |
| `GetDaysSinceLastPayment` | INT | 31 | **High** | Aggregation per invocation |
| `GetBillMethod` | VARCHAR | 132 | **High** | Very complex logic with multiple table lookups and nested function calls |
| `GetInsCode` | VARCHAR(50) | 84 | Medium | Multiple NOT EXISTS checks, sequential logic |
| `GetMappingValue` | VARCHAR(MAX) | 42 | Medium | Called from GetMappedPhysician which calls it 4 times |
| `GetMappedPhysician` | VARCHAR(15) | 76 | Medium | Calls GetMappingValue up to 4 times per invocation |
| `GetNewFeeSchedPrice` | NUMERIC(18,2) | 73 | Medium | 12 IF branches querying different tables |
| `GetSystemValue` | VARCHAR(8000) | 30 | Low | Simple lookup |
| `GetMailProfile` | VARCHAR(8000) | 13 | Low | Simple lookup |
| `GetAMAYear` | INT | 32 | Low | Date calculation |
| `GetAmountTotal` | NUMERIC(18,2) | 18 | Low | Simple SUM |
| `GetLastInvoiceDate` | DATETIME | 20 | Low | Simple MAX |
| `GetLastDateBilled` | DATETIME | 56 | Medium | Complex UNPIVOT |

#### Table-Valued Functions

| Function | Lines | Severity | Key Anti-Patterns |
|----------|-------|----------|-------------------|
| `GetFeeSchedulePrice` | 167 | **High** | Called extensively from GetChrgBillMethod, GetClientPrice, GetPrice. 4x UNION ALL for fee schedules |
| `GetFeeSchedulePricewdk` | 167 | Medium | Near-duplicate of GetFeeSchedulePrice |
| `GetChrgBillMethod` | 278 | **High** | Calls GetFeeSchedulePrice repeatedly, 465 lines of complex logic |
| `GetClientPrice` | 64 | Medium | Calls GetFeeSchedulePrice multiple times |
| `GetGCodeBillingCharges` | 116 | Medium | Non-sargable date CONVERTs, IN(SELECT), hardcoded start date |
| `GetGlobalBillingCharges` | 155 | Medium | Similar to GCode, improved but still has non-sargable dates |
| `GetInsurance` | 102 | Low | Simple SELECT |
| `GetAccDiagnosis` | 28 | Medium | Calls GetPatDiagnosis (nested TVF) |
| `GetAccDiagnosis1` | 41 | Medium | Calls GetPatDiagnosis (nested TVF) |
| `GetAccIns` | 75 | Low | CTE with CASE logic |
| `GetPatDiagnosis` | 52 | Low | UNPIVOT pattern, clean |
| `GetPatDiagnosis_icd` | 51 | Low | Similar UNPIVOT |
| `GetPatDiagnosis2` | 52 | Low | Duplicate of GetPatDiagnosis |
| `GetPatIcd9` | ~283 | Medium | Referenced in HL7 procedures per-row |
| `GetAccountQuestChargeCodes` | ~95 | Medium | Referenced in Care360 per-row |
| `GetAmtRecords` | 172 | Medium | Complex charge amount generation |
| `GetAmtRecords2` | 221 | Medium | Variant of AmtRecords |
| `GetClientsUnbilledCharges` | 82 | Medium | TOP(100) PERCENT in CTE |
| `GetInvoiceCharges` | 66 | Low | Simple charge lookup |
| `GetMasterAcc` | 38 | Low | Recursive CTE |
| `GetMasterAccount` | 12 | Low | Simple lookup, calls AccountTrim |
| `GetMultiples` | ~50 | Low | Charge multiples |
| `GetPhyNPI` | 46 | Low | Physician NPI lookup |
| `GetPrice` | 67 | Medium | Calls GetFeeSchedulePrice 2x |
| `GetQuestAmtRecords` | ~120 | Medium | Quest-specific charge records |
| `GetQuestPrice` | ~50 | Medium | Quest pricing |
| `GetPaymentsByAccount` | 44 | Low | Simple aggregate |
| `GetPaymentsByClient` | 52 | Low | Date range aggregate |
| `GetPaymentsByDate` | 41 | Low | Date-based payments |
| `GetPaymentsByDate2` | 109 | Low | Complex payment breakdown |
| `GetPaymentsBeforeLastDataMailer` | 36 | Medium | JOINs chk and pat |
| `GetPaymentsTableSinceLastDataMailer` | 49 | Medium | Calls PadText scalar function |
| `GetCernerPhyNPI` | 33 | Medium | Calls ufn_Split_Name TVF inside scalar |
| `GetCptList` | 33 | Low | FOR XML PATH string aggregation |
| `GetWriteOffByAccount` | ~50 | Low | Simple aggregate |

#### Utility/Formatting Functions

| Function | Type | Lines | Notes |
|----------|------|-------|-------|
| `AccountTrim` | Scalar | 41 | String trimming |
| `array` | TVF | 63 | String splitting |
| `AttendPhyTrim` | Scalar | 28 | String trimming |
| `CamelCase` | Scalar | 24 | String formatting |
| `ConvertName` | Scalar | 34 | Name formatting |
| `DateOfBirthFix` | Scalar | 58 | Date validation |
| `DateRange` | TVF | 45 | Date range generation |
| `fn_Get_XMLCerner_data` | Scalar | 29 | XML extraction |
| `fnConvert_TitleCase` | Scalar | 41 | Title case conversion |
| `fnCptsForCdm` | Scalar | 41 | CPT concatenation |
| `fnFormatDate` | Scalar | 36 | Date formatting |
| `fnParseStringTSQL` | TVF | 24 | String parsing |
| `fnRandomizedText` | Scalar | 39 | Text randomization |
| `fnStandardPhone` | Scalar | 44 | Phone formatting |
| `FormatClient` | Scalar | 78 | Client formatting |
| `FormatFinCode` | Scalar | 94 | Fincode formatting |
| `FormatPhoneNo` | Scalar | 50 | Phone formatting |
| `FormatPhoneNoTest` | Scalar | 65 | Phone formatting (test) |
| `FormatRelation` | Scalar | 44 | Relation code formatting |
| `FormatSSN` | Scalar | 35 | SSN formatting |
| `OVNumberTrim` | Scalar | 64 | Number trimming |
| `PadText` | Scalar | 38 | Text padding |
| `PadTextSave` | Scalar | 36 | Text padding (backup) |
| `PatternSplitLoop` | TVF | 70 | Pattern splitting |
| `sfn_trim_zeros` | Scalar | 34 | Zero trimming |
| `sfn_trim_zeros_mri` | Scalar | 30 | Zero trimming (MRI) |
| `SplitCdmPriceByCpt` | TVF | 140 | CDM price splitting |
| `SplitCdmPriceByCpt2` | TVF | 141 | Variant |
| `SplitCdmPriceByCpt3` | TVF | 142 | Variant |
| `SplitCITY_ST_ZIP` | Scalar | 63 | Address parsing |
| `XmlFormatPhysicianCerner` | Scalar | 71 | XML physician formatting |
| `zzz.FormatAccession` | Scalar | ~50 | Legacy accession formatting |
| `tvf_accounting_report_by_cdm_client` | TVF | 103 | Accounting report |
| `ufn_client` | TVF | 185 | Client unbilled charges |
| `ufn_Duplicates` | TVF | 73 | Duplicate detection |
| `ufn_GetAccountPayment` | TVF | 90 | Account payment |
| `ufn_GetQuestError` | TVF | 90 | Quest error detection |
| `ufn_quest_processing` | TVF | 153 | Quest processing |
| `ufn_Split_Name` | TVF | 93 | Name splitting |
| `ufn_jpg_activity_idx` | TVF | 47 | JPG activity index |
| `usf_AccCheckForLMRP` | TVF | 99 | LMRP check |
| `usf_account_report_client` | TVF | 69 | Client account report |
| `usf_account_report_client_by_physician` | TVF | 249 | Physician report |
| `usf_account_report_client_qty` | TVF | 100 | Quantity report |
| `usf_prg_panels` | TVF | 118 | Panel processing |
| `GetXmlDataForAcc` | TVF | 54 | XML account data |
| `GetXmlDataForAcc2` | TVF | 54 | XML account data v2 |
| `GetXmlDataForAccCharges` | TVF | 81 | XML charge data |
| `GetXmlDataForAccInfce` | TVF | 70 | XML interface data |
| `GetXmlDataForAccIns` | TVF | 80 | XML insurance data |
| `ArrayToTable` | TVF | 19 | Array parsing |
| `DelimitedSplit8K` | TVF | 43 | Jeff Moden's splitter |
| `fnRemoveChildElements` | Scalar | ~435 | XML manipulation |
| `fnRemoveChildElementsNC` | Scalar | ~221 | XML manipulation (no case) |
| `GenerateCalendar` | TVF | 111 | Calendar generation |
| `GetAddedCDMs` | TVF | 23 | Added CDMs |
| `GetNumbersTable` | TVF | 19 | Numbers table |
| `NumbersTable` | TVF | 31 | Numbers table variant |
| `udf_XML2Table` | TVF | ~50 | XML to table |
| `GetAmtPaidByAccount` | Scalar | ~48 | Payment total |
| `GetAmt` | TVF | ~48 | Amount records |
| `GetTotalPayments` | Scalar | ~41 | Total payments |
| `GetWriteOffByAccount` | Scalar | ~50 | Write-off total |

---

## Phase 2: Anti-Pattern Analysis

### CRITICAL Anti-Patterns

#### C1: Scalar Functions in SELECT Lists (RBAR - Row By Agonizing Row)

**Impact:** Each scalar function call in a SELECT list forces row-by-row evaluation, preventing set-based query optimization. When a procedure calls 6+ scalar functions per row across thousands of rows, the result is O(n*m) complexity where m is the number of table scans inside each function.

**Affected Procedures:**

| Procedure | Scalar Functions Called Per Row | Estimated Impact |
|-----------|-------------------------------|-----------------|
| `usp_prg_pat_bill_acct_verify` | `GetAccBalByDate` (6x), `GetAccTotalCharges`, `GetPaymentsSinceLastDataMailer`, `GetContractualByAccount`, `GetDaysSinceLastPayment` | **50-100x slower** than inline |
| `usp_prg_pat_bill_update_flags` | `GetAccBalByDate` (2x in WHERE + SELECT) | **10-50x slower** |
| `GetAccountSummary` | `GetAccTotalCharges`, `GetAmtPaidByAccount`, `GetContractualByAccount`, `GetWriteOffByAccount`, `GetBadDebtByAccount`, `GetAccBalByDate` | **7 scalar calls for 1 row** |
| `GetClientsToBill` | `GetAccBalance` (2x in WHERE), `GetAccClientBalance` (in SUM) | **Scalar in WHERE prevents optimization** |
| `usp_prg_pat_bill_actv` | `GetChrgDetailTotal` (2x per row) | **10-20x slower** |

**Before (Current):**
```sql
-- In usp_prg_pat_bill_acct_verify
SELECT ...
    dbo.GetAccBalByDate(acc.account, @date1) AS [Balance1],
    dbo.GetAccBalByDate(acc.account, @date2) AS [Balance2],
    dbo.GetAccTotalCharges(acc.account) AS [TotalCharges],
    dbo.GetPaymentsSinceLastDataMailer(acc.account) AS [Payments],
    dbo.GetContractualByAccount(acc.account) AS [Contractual],
    dbo.GetDaysSinceLastPayment(acc.account) AS [DaysSincePayment]
FROM acc ...
```

**After (Recommended):**
```sql
-- Inline the scalar functions as LEFT JOINs or CROSS APPLY
SELECT ...
    bal1.Balance AS [Balance1],
    bal2.Balance AS [Balance2],
    tc.TotalCharges,
    pmt.Payments,
    con.Contractual,
    dsp.DaysSincePayment
FROM acc
CROSS APPLY (
    SELECT SUM(chrg.qty * cd.amount) - ISNULL(SUM(chk.amt_paid + chk.write_off + chk.contractual), 0)
    FROM chrg JOIN chrg_details cd ON cd.chrg_num = chrg.chrg_num
    LEFT JOIN chk ON chk.account = chrg.account AND chk.status <> 'CBILL'
    WHERE chrg.account = acc.account AND cd.mod_date <= @date1
) bal1(Balance)
...
```

#### C2: Non-Sargable Date Comparisons

**Impact:** Wrapping indexed columns in functions (CONVERT, CAST) prevents the query optimizer from using index seeks, forcing full table scans.

**Affected Functions/Procedures:**

| Object | Line | Anti-Pattern | Sargable Fix |
|--------|------|-------------|--------------|
| `GetAccBalByDate` | ~1455 | `CONVERT(DATETIME,convert(varchar(10),chrg_details.mod_date,101)) <= CONVERT(DATETIME,convert(varchar(10),@effDate,101))` | `chrg_details.mod_date < DATEADD(DAY, 1, CAST(@effDate AS DATE))` |
| `GetAccBalByTransDate` | ~1560 | `CONVERT(DATETIME,convert(varchar(10),chrg.service_date,101)) <= ...` | `chrg.service_date < DATEADD(DAY, 1, CAST(@effDate AS DATE))` |
| `GetGCodeBillingCharges` | ~3780 | `convert(datetime,convert(varchar(10),service_date,101))` in SELECT and GROUP BY | Use `CAST(service_date AS DATE)` |
| `GetGlobalBillingCharges` | ~3922 | Same CONVERT pattern | Same fix |
| `usp_account_receivable_daily_reconciliation` | ~21930 | `CONVERT(CHAR(8),amt.mod_date,10)` in GROUP BY | Use `CAST(amt.mod_date AS DATE)` |

**Before:**
```sql
CONVERT(DATETIME,convert(varchar(10),chrg_details.mod_date,101)) <= CONVERT(DATETIME,convert(varchar(10),@effDate,101))
```

**After:**
```sql
chrg_details.mod_date < DATEADD(DAY, 1, CAST(@effDate AS DATE))
```

#### C3: Nested Scalar Function Calls

**Impact:** `GetAccTotalCharges` calls `GetChrgDetailTotal` inside a SUM(), creating O(n^2) behavior - for each charge row, a separate scalar function queries chrg_details.

**Affected:**
```sql
-- GetAccTotalCharges calls GetChrgDetailTotal per row:
SELECT @chrgTotal = SUM(chrg.qty * dbo.GetChrgDetailTotal(chrg.chrg_num))
FROM chrg WHERE chrg.account = @account
```

**Fix:** Inline the nested function:
```sql
SELECT @chrgTotal = SUM(chrg.qty * cd.amount)
FROM chrg
JOIN chrg_details cd ON cd.chrg_num = chrg.chrg_num
WHERE chrg.account = @account AND chrg.status NOT IN ('CBILL','CAP','N/A')
```

#### C4: GetNamePart Called 30+ Times Per Row in HL7 Procedures

**Impact:** `GetNamePart` is a pure string-manipulation scalar function using PARSENAME. In `usp_infce_psa_demographics`, it is called approximately 30 times per patient row (once for each name field - LAST, FIRST, MIDDLE, SUFFIX for patient, guarantor, attending physician, referring physician, etc.). For 1000 patients, that is 30,000 function invocations.

**Fix:** Replace with inline CROSS APPLY to a single CTE or inline TVF that parses all name parts at once:
```sql
CROSS APPLY (
    SELECT
        PARSENAME(REPLACE(REPLACE(REPLACE(REPLACE(pat_name,'.',''),', ','.'),',',' '),' ','.'),4) AS LastName,
        PARSENAME(REPLACE(REPLACE(REPLACE(REPLACE(pat_name,'.',''),', ','.'),',',' '),' ','.'),2) AS FirstName,
        PARSENAME(REPLACE(REPLACE(REPLACE(REPLACE(pat_name,'.',''),', ','.'),',',' '),' ','.'),1) AS MiddleName,
        PARSENAME(REPLACE(REPLACE(REPLACE(REPLACE(pat_name,'.',''),', ','.'),',',' '),' ','.'),3) AS Suffix
) patName
```

### HIGH Anti-Patterns

#### H1: @@IDENTITY Instead of SCOPE_IDENTITY()

**Impact:** `@@IDENTITY` returns the last identity value generated in ANY scope (including triggers). If a trigger on the `chrg` table inserts into another table with an identity column, `@@IDENTITY` will return the wrong value.

**Affected Procedures:**

| Procedure | Line | Fix |
|-----------|------|-----|
| `usp_prg_Credit_Charge` | 26752 | Replace `@@IDENTITY` with `SCOPE_IDENTITY()` |
| `usp_prg_Drug_Screens_Reversal` | 27006 | Replace `@@IDENTITY` with `SCOPE_IDENTITY()` |

#### H2: Missing Transaction Wrapping

**Impact:** Multi-table INSERT/UPDATE operations without transactions can leave data in an inconsistent state if any step fails.

**Affected Procedures:**

| Procedure | Operations Without Transaction |
|-----------|-------------------------------|
| `usp_prg_Credit_Charge` | INSERT chrg, INSERT amt, UPDATE chrg |
| `usp_prg_Drug_Screens` | UPDATE chrg, INSERT chrg, INSERT amt, INSERT chrg, INSERT amt |
| `usp_prg_Drug_Screens_Reversal` | UPDATE chrg, INSERT chrg, INSERT amt, INSERT chrg, INSERT amt |
| `usp_prg_Drug_Screens2` | Similar multi-table operations |
| `usp_prg_CreateAccount` | UPDATE number, INSERT acc (race condition on sequence) |
| `usp_prg_pat_bill_update_flags` | TRY/CATCH exists but NO TRANSACTION wrapping the multiple updates |

#### H3: SELECT * for Existence Checks

**Impact:** `SELECT *` retrieves all columns when only existence needs to be checked.

```sql
-- Current (usp_prg_Credit_Charge):
IF (NOT EXISTS(SELECT * FROM chrg WHERE chrg_num = @chrgNum AND credited = 0))

-- Fix:
IF (NOT EXISTS(SELECT 1 FROM chrg WHERE chrg_num = @chrgNum AND credited = 0))
```

#### H4: Duplicate Procedures

**Impact:** `usp_infce_psa_demographics_special` is a near-duplicate of `usp_infce_psa_demographics` with a hardcoded date filter. This creates maintenance burden and doubles the bug surface area.

**Fix:** Add an optional parameter to the base procedure or create a wrapper.

### MEDIUM Anti-Patterns

#### M1: TOP(100) PERCENT in CTEs

Used in `usp_prg_pat_bill_acct_verify`, `usp_prg_pat_bill_actv`, `GetClientsUnbilledCharges`, `GetInvoiceCharges`. TOP(100) PERCENT with ORDER BY in a CTE is meaningless - the optimizer ignores it.

#### M2: IN(SELECT...) Instead of EXISTS or JOIN

Used in `usp_account_payout` (lines 21582, 21591). `EXISTS` is generally more efficient as it can short-circuit.

#### M3: Hardcoded Values

| Procedure | Hardcoded Value | Recommended |
|-----------|----------------|-------------|
| `usp_infce_psa_demographics_special` | `'05/10/2015'` date filter | Parameter or system value |
| `usp_account_payout` | `bradley.powers@wth.org` email | System value lookup |
| `GetGCodeBillingCharges` | `'01/20/2012 00:00'` start date | System value (GetGlobalBillingCharges already fixed this) |
| Multiple procedures | Hardcoded client mnemonics like `'HC','JPG','LEW'` | Reference table |

#### M4: Missing SET NOCOUNT ON

Several procedures lack `SET NOCOUNT ON`, which causes extra network round-trips for row count messages.

#### M5: PRINT Statements in Production Code

`usp_prg_Credit_Charge` and `usp_prg_Drug_Screens_Reversal` contain PRINT statements that should be removed or replaced with proper logging.

#### M6: Empty/No-Op Procedures

`utility.Log_ProcedureCall` is called from nearly every procedure but has an empty body. This adds overhead for every procedure call.

### LOW Anti-Patterns

- Inconsistent naming conventions (sp_ prefix, mixed case)
- Commented-out debugging code left in production procedures
- Inconsistent use of schema qualification (some use `dbo.`, some don't)
- VARCHAR lengths inconsistent for same logical fields across procedures

---

## Phase 3: Optimization Priority Matrix

### Priority 1 - Critical (Immediate Action)

| Rank | Object | Anti-Patterns | Est. Improvement | Risk |
|------|--------|--------------|-----------------|------|
| 1 | `GetAccBalByDate` | Non-sargable dates, scalar function | 10-50x faster date comparisons | Low - same logic, sargable predicates |
| 2 | `usp_prg_pat_bill_update_flags` | Scalar function in WHERE clause | 10-50x for qualifying filter | Medium - complex update logic |
| 3 | `GetAccTotalCharges` | Nested scalar function call | 5-10x for charge calculation | Low - inline join |
| 4 | `usp_prg_pat_bill_acct_verify` | 6+ scalar functions per row | 50-100x for entire procedure | Medium - requires careful testing |
| 5 | `usp_infce_psa_demographics` | GetNamePart 30x per row | 10-30x for HL7 generation | Medium - HL7 format must be preserved exactly |
| 6 | `GetAccountSummary` | 7 scalar functions for 1 row | 5-7x improvement | Low - single row result |
| 7 | `infce.usp_infce_demo_orders_to_care360` | GetNamePart + TVFs per row | 10-20x for Care360 generation | Medium - interface format critical |

### Priority 2 - High (Plan Soon)

| Rank | Object | Fix | Est. Improvement | Risk |
|------|--------|-----|-----------------|------|
| 8 | `usp_prg_Credit_Charge` | SCOPE_IDENTITY(), transaction, remove PRINT | Correctness fix | Low |
| 9 | `usp_prg_Drug_Screens_Reversal` | SCOPE_IDENTITY(), transaction | Correctness fix | Low |
| 10 | `usp_prg_Drug_Screens` | Add transaction wrapping | Correctness fix | Low |
| 11 | `usp_prg_CreateAccount` | Add transaction (race condition fix) | Correctness fix | Low |
| 12 | `GetClientsToBill` | Inline scalar functions from WHERE | 5-10x | Medium |
| 13 | `usp_prg_pat_bill_actv` | Inline GetChrgDetailTotal | 5-10x | Low |
| 14 | `GetChrgDetailTotal` | Inline wherever called | Cascade improvement | Low |

### Priority 3 - Medium (Scheduled Maintenance)

| Rank | Object | Fix |
|------|--------|-----|
| 15 | `usp_infce_psa_demographics_special` | Consolidate with base procedure |
| 16 | `usp_account_payout` | IN->EXISTS, minor cleanup |
| 17 | `GetGCodeBillingCharges` | Sargable date comparisons |
| 18 | `GetFeeSchedulePrice` / `GetFeeSchedulePricewdk` | Remove duplicate |
| 19 | Patient billing display procedures | Inline scalar functions |
| 20 | `utility.Log_ProcedureCall` | Implement or remove |

---

## Risk Assessment

### Low Risk Optimizations (Can deploy immediately)
- Fixing non-sargable date comparisons in scalar functions
- Replacing @@IDENTITY with SCOPE_IDENTITY()
- Removing PRINT statements
- Adding SET NOCOUNT ON
- Replacing SELECT * with SELECT 1 in EXISTS

### Medium Risk Optimizations (Requires testing)
- Inlining scalar functions in patient billing procedures
- Adding transaction wrapping to charge processing
- Modifying HL7 generation procedures (format must be exact)

### High Risk Optimizations (Requires staging environment validation)
- Consolidating duplicate procedures
- Restructuring GetFeeSchedulePrice
- Modifying usp_prg_pat_bill_update_flags (affects billing workflow)

---

## Recommendations

1. **Start with `GetAccBalByDate`** - This function is the single biggest performance bottleneck. Fixing the non-sargable date comparison alone will cascade improvements to every procedure that calls it.

2. **Add transactions to charge processing** - `usp_prg_Credit_Charge`, `usp_prg_Drug_Screens`, and `usp_prg_Drug_Screens_Reversal` can leave orphaned records on failure. This is a correctness issue, not just performance.

3. **Replace @@IDENTITY immediately** - This is a data integrity bug waiting to happen if triggers are ever added to the chrg table.

4. **Inline `GetNamePart` in HL7 procedures** - This is the biggest win for HL7 processing throughput. The function is pure string manipulation that can be done inline with CROSS APPLY.

5. **Consider moving patient billing scalar functions to inline TVFs** - SQL Server can inline table-valued functions but not scalar functions (prior to SQL Server 2019 scalar UDF inlining, and even that has limitations).

6. **Implement or remove `utility.Log_ProcedureCall`** - Currently adds overhead to every procedure for no benefit.
