# Index Optimization Analysis Report

## Executive Summary

This report analyzes 80+ indexes across the Lab Patient Accounting database schema and cross-references them with actual application query patterns found in 20+ repository classes, 8+ service classes, and numerous stored procedures/views.

**Key Findings:**

1. **1 redundant index** on `acc_location` table where a wider covering index duplicates a narrower one with identical key columns
2. **3 missing indexes** for high-traffic query patterns in HL7 message processing and billing history date range queries
3. **2 suboptimal index definitions** on the `chrg` table where missing INCLUDE columns cause key lookups
4. **1 over-indexed table** (`chrg` with 6 nonclustered indexes) assessed but all serving distinct purposes
5. **1 notable clustered index design** (`chrg_details` has nonclustered PK on `uri` and separate clustered index on `chrg_num` -- intentional and correct)

**Estimated Impact:** Implementing the top 5 recommendations would improve performance for:
- HL7 message processing (batch operations processing thousands of messages)
- Claim generation workflow (iterates all billable accounts)
- Account search (most frequent user-facing operation)
- Billing activity lookups (frequent during claim processing)
- Payment/charge lookups by account (core application flow)

---

## Phase 1: Current Index Inventory

### Summary Statistics

| Category | Count |
|----------|-------|
| **Clustered indexes (via PK)** | ~40+ |
| **Nonclustered indexes** | 75+ |
| **Unique nonclustered indexes** | 8 |
| **Nonclustered with INCLUDE columns** | 35+ |
| **Filtered indexes** | 0 |
| **XML indexes** | 1 |
| **Schemas covered** | dbo, dictionary, infce, audit, zzz |

### Tables With Most Indexes

| Table | Nonclustered Index Count | Notes |
|-------|--------------------------|-------|
| `dbo.acc` | 7 | Core account table; all indexes well-utilized |
| `dbo.chrg` | 6 | Charge table; some overlap possible |
| `dbo.ins` | 5 | Insurance records; all support distinct patterns |
| `dbo.chk` | 5 | Payment/check records |
| `zzz.h1500` | 7 | Claim history; legacy table |
| `dictionary.phy` | 6 | Physician dictionary |
| `dbo.acc_location` | 3 | Two are redundant |
| `infce.messages_inbound` | 3 | HL7 message queue; missing key index |

### Complete Index Catalog

#### Schema: dbo

**Table: acc** (Primary Key: `account` CLUSTERED)
1. `[ix fin_code, trans_date, status INCLUDE ...]` -- (fin_code, trans_date, status) INCLUDE(deleted, account, pat_name, cl_mnem, cbill_date, ssn, num_comments, meditech_account, original_fincode, mod_date, mod_user, mod_prg, oereqno, mri)
2. `[IX_acc_status]` -- (status) INCLUDE(account, pat_name, cl_mnem, fin_code, trans_date)
3. `[ix_cli_mnem]` -- (cl_mnem) INCLUDE(account, fin_code)
4. `[ix_mri]` -- (mri)
5. `[ix_pat_name]` -- (last_name, first_name, middle_name, pat_name, birthdate)
6. `[ix_ssn]` -- (ssn)
7. `[ix_status_transdate_fincode]` -- (status, trans_date, fin_code) INCLUDE(account, mod_prg)

**Table: chrg** (Primary Key: `chrg_num` CLUSTERED)
1. `[account_fin_type_status_Includes]` -- (account, fin_type, status) INCLUDE(chrg_num, qty)
2. `[account_index]` -- (account) INCLUDE(status, service_date, cdm, qty, net_amt, credited, chrg_num, invoice)
3. `[credited_account_status_Includes]` -- (credited, account, status) INCLUDE(chrg_num, qty)
4. `[credited_cl_mnem_status_invoice_fin_code_Includes]` -- (credited, cl_mnem, status, invoice, fin_code) INCLUDE(account)
5. `[invoice_select_idx]` -- (fin_type, status, invoice) INCLUDE(account, fin_code, cl_mnem)

**Table: chrg_details** (Clustered Index: `chrg_num`; Primary Key: `uri` NONCLUSTERED)
1. `[ix_mod_date, type INCLUDES]` -- (mod_date, type) INCLUDE(chrg_num, amount, cpt4, modi, modi2)

**Table: chk** (Primary Key: `pay_no` CLUSTERED)
1. `[batch_idx]` -- (batch)
2. `[chk_no_idx]` -- (chk_no)
3. `[invoice_idx]` -- (invoice)
4. `[IX_account]` -- (account, status, mod_date) INCLUDE(amt_paid, write_off, contractual)
5. `[IX_chk_mod_date_status]` -- (mod_date, status) INCLUDE(account, amt_paid, write_off, contractual)

**Table: ins** (Primary Key: `account, ins_a_b_c` CLUSTERED)
1. `[ix_abc Include account, ins_code]` -- (ins_a_b_c) INCLUDE(account, ins_code, policy_num, deleted)
2. `[IX_deleted]` -- (deleted) INCLUDE(account)
3. `[IX_ins_code]` -- (ins_code) INCLUDE(account, ins_a_b_c, policy_num)
4. `[ix_rowguid-...]` -- UNIQUE (rowguid) INCLUDE(account, ins_a_b_c)
5. `[policy_num_idx]` -- (policy_num)

**Table: pat** (Primary Key: `account` CLUSTERED)
1. `[ix_mailer INCLUDE account]` -- (mailer) INCLUDE(account)
2. `[phy_id_idx]` -- (phy_id, account)

**Table: notes**
1. `[account_index]` -- (account)

**Table: data_billing_history**
1. `[IX_batch_w_includes]` -- (batch) INCLUDE(rowguid, deleted, account, ins_abc, pat_name, fin_code, ins_code, trans_date, run_date, printed, run_user, ebill_status, ebill_batch, text, ins_complete, mod_date, mod_user, mod_prg, mod_host, claim_amount)

**Table: data_EOB**
1. `[ix_account_bill_cycle_date_check_no_claim_status_deleted]` -- (account, bill_cycle_date, check_no, claim_status, deleted)
2. `[IX_EOB_rowguid]` -- (rowguid)

**Table: data_EOB_Detail**
1. `[IX_EOB_detail_rowguid]` -- (rowguid)

**Table: acc_location**
1. `[IX_acc_location]` -- (location, surveydate)
2. `[IX_acc_location_ov_acct]` -- (ov_acct, ov_mri)
3. `[ix_location, surveydate INCLUDE ...]` -- (location, surveydate) INCLUDE(account, pt_type, mod_date, mod_user, mod_prg, mod_host, ov_acct, ov_mri)

**Table: acc_status_updates**
1. `[IX_account]` -- (account) INCLUDE(acc_status)
2. `[IX_emailed]` -- (emailed) INCLUDE(account, acc_status, trans_date, mod_date)

**Table: acc_lock**
1. `[account_no_idx]` -- (AccountNo)

**Table: ACC_LMRP**
1. `[IX_acc_lmrp_account]` -- (account)

**Table: chrg_diag_pointer**
1. `[account_idx]` -- (account) INCLUDE(cdm, cpt, diag_ptr)

**Table: chrg_err**
1. `[account_cdx]` -- (account)

**Table: chrg_pa**
1. `[IX_chrg_pa_batch]` -- (batch)
2. `[mt_req_no_idx]` -- (mt_req_no)

**Table: patbill_acc**
1. `[IX_account_id]` -- (account_id)
2. `[IX_batch_id_datesent]` -- (batch_id, date_sent, account_id) INCLUDE(statement_number)
3. `[IX_STATEMENT]` -- (batch_id, statement_number, record_cnt_acct)

**Table: patbill_enctr_actv**
1. `[IX_statement_batch]` -- (statement_number, batch_id)

**Table: patbill_stmt**
1. `[IX_statement_number]` -- (statement_number)

**Table: patdx**
1. `[IX_patdx_account]` -- (account)

**Table: bad_debt**
1. `[date_sent_idx]` -- (date_sent)

**Table: cbill_hist**
1. `[invoice_idx]` -- (invoice)
2. `[IX_thru_date]` -- (thru_date) INCLUDE(cl_mnem, invoice, bal_forward, total_chrg, discount, balance_due, payments, true_balance_due, mod_user, mod_date, mod_prg, mod_host)

**Table: aging_history** (Primary Key: `account, datestamp` CLUSTERED)
1. `[ix_datestamp INCLUDE account, balance]` -- (datestamp, fin_code) INCLUDE(account, balance, ins_code, mod_date, mod_user, mailer)

**Table: ssi_remittance**
1. `[ix_pcn]` -- (pcn)

**Table: ssi_remittance_charges**
1. `[ix_icn INCLUDE ...]` -- (icn) INCLUDE(cpt_code, rev_code, reported_amt, allowed_amt)

**Table: chk_batch_details**
1. `[ix_chk_batch_details_batch_id]` -- (batch, id)

**Table: data_quest_360**
1. `[IX_data_quest_360_account]` -- (account, bill_type, patid)
2. `[IX_data_quest_360_error]` -- (pre360_error, bill_code_error, entered, bill_type) INCLUDE(date_of_service)

**Table: data_quest_billing**
1. `[IX_data_quest_billing_quest_code]` -- (account, req_no, quest_code)

**Table: data_tier_pricing**
1. `[ndx_data_tier_pricing_primary]` -- (CLIENT, CDM)

**Table: patbill_enctr** (Clustered Index: `statement_number, enctr_nbr, pft_encntr_id, place_of_service`)

**Table: rds** (Clustered Index: `name`)

#### Schema: dictionary

**Table: cdm** (Primary Key: `cdm` CLUSTERED)
1. `[IX_cdm_descript]` -- (descript)
2. `[IX_cdm_mnem]` -- (mnem)

**Table: client** (Primary Key: `id` CLUSTERED)
1. `[Client_Mnem_IX]` -- UNIQUE (cli_mnem) INCLUDE(cli_nme, id)
2. `[ix deleted, cli_nme]` -- (cli_nme, deleted)
3. `[IX_bill_to_client]` -- (bill_to_client)

**Table: fin** (Primary Key: `fin_code` CLUSTERED)

**Table: phy** (Primary Key: `tnh_num` via nonclustered unique)
1. `[IX_phy_billing_npi]` -- (billing_npi)
2. `[IX_phy_mt_mnem]` -- (mt_mnem)
3. `[IX_phy_tnh_num]` -- UNIQUE (tnh_num) INCLUDE(billing_npi, last_name, first_name, mid_init, mt_mnem)
4. `[IX_phy_upin]` -- (upin)
5. `[name_idx]` -- (last_name, first_name, mid_init)

**Table: phy_sanc**
1. `[IX_phy_sanc_name]` -- (lastname, firstname)
2. `[IX_phy_sanc_upin]` -- (upin)
3. `[npi_IX]` -- (npi)

**Table: mapping**
1. `[IX_mapping_lookup]` -- (return_value_type, sending_system, sending_value) INCLUDE(return_value)

**Table: icd9desc**
1. `[IX_icd9desc_icd9_desc]` -- (icd9_desc) INCLUDE(AMA_year)
2. `[IX_icd9num_amayear]` -- UNIQUE (icd9_num, AMA_year, version) INCLUDE(icd9_desc)

**Table: lmrp** (Primary Key: `uid` CLUSTERED)
1. `[ix cpt4, ama_year, ...]` -- (ama_year, cpt4, rb_date, expiration_date, beg_icd9, end_icd9) INCLUDE(payor, fincode, mod_user, mod_date, mod_prg, lmrp, lmrp2, rb_date2, chk_for_bad, uid)

**Table: insc**
1. `[name_idx]` -- (name)

**Table: cli_dis**
1. `[IX_cli_dis_1]` -- UNIQUE (cli_mnem, start_cdm, end_cdm)
2. `[IX_cli_dis_deleted_cli]` -- (deleted, cli_mnem) INCLUDE(start_cdm, end_cdm, percent_ds, price)

**Table: cdw**
1. `[IX_cdw_hosp_mnem]` -- (hosp_mnem, meditech_mnem)

**Table: dict_acc_validation_criteria**
1. `[rule_id-...]` -- (rule_id)

**Table: dict_C_MEEDIT**
1. `[ME1_ME2_Unique_idx]` -- UNIQUE (ME_1, ME_2)

**Table: dict_cpt4_warnings**
1. `[IX_dict_diagnosis_warnings]` -- UNIQUE (cpt4)

**Table: dict_ncd**
1. `[IX_ncd_cpt]` -- (ncd_id, cpt)

**Table: dict_quest_reference_lab_tests**
1. `[IX_deleted_multiples_startdate_cdm]` -- (deleted, has_multiples, cdm, link, start_date) INCLUDE(expire_date, quest_code, quest_description)
2. `[IX_dict_quest_ref_lab_code]` -- (quest_code)

**Table: pth**
1. `[name_idx]` -- (name)

**Table: system**
1. `[IX_KeyName]` -- (KeyName)

**Table: Monthly_Reports**
1. `[ReportName_IX]` -- (mi_name)

#### Schema: infce

**Table: messages_inbound**
1. `[IX_account_cerner]` -- (account_cerner, DOS)
2. `[IX_PROC_DFT]` -- (msgType, processFlag, msgDate)
3. `[IX_sourceMsgId]` -- (sourceMsgId, sourceInfce, msgType)

**Table: messages_inbound_adt**
1. `[IX_account_cerner]` -- (account_cerner, msgType, msgDate)

**Table: patient_demographics**
1. `[idx_xmlContent]` -- PRIMARY XML INDEX

#### Schema: audit

**Table: Audit**
1. `[IX_TableName_FieldName_UpdateDate]` -- (TableName, FieldName, UpdateDate) INCLUDE(OldValue, NewValue)

#### Schema: zzz (legacy/archive)

**Table: ub**
1. `[PK_account, ins_abc]` -- UNIQUE CLUSTERED (account, ins_abc)

**Table: dbill**
1. `[batch_idx]` -- (batch, run_date)
2. `[IX_dbill_fin_code_name]` -- (fin_code, pat_name)
3. `[IX_dbill_pat_name]` -- (pat_name)

**Table: h1500**
1. `[ebill_batch]` -- (ebill_batch)
2. `[ebill_status_idx]` -- (ebill_status)
3. `[fin_code_idx]` -- (fin_code)
4. `[ix_rowguid INCLUDE account, ins_abc]` -- (rowguid) INCLUDE(account, ins_abc)
5. `[pat_name_idx]` -- (pat_name)
6. `[PK_account, ins_abc]` -- UNIQUE (account, ins_abc)
7. `[run_date_idx]` -- (run_date)

---

## Phase 2: Application Query Pattern Analysis

### High-Traffic Query Patterns

#### 1. Account Lookup (AccountRepository)
- **GetByAccount(account)**: PK lookup on `acc.account` -- well indexed
- **GetByStatus(status)**: Filters on `acc.status`, selects `account` -- covered by `IX_acc_status`

#### 2. Account Search (AccountSearchRepository -> AccountSearchView)
- Searches on: `last_name LIKE`, `first_name LIKE`, `account =`, `mri =`, `ssn =`, `sex =`, `birthdate =`
- ORDER BY `trans_date DESC` or `pat_name`
- The view joins `acc`, `pat`, `ins`, `dictionary.fin`, `acc_validation_status`
- Filtering happens on `acc` table columns -- existing indexes on `acc` support these patterns

#### 3. Worklist Queries (WorklistService -> AccountSearchView)
- Heavy filtering on `fin_code =`, `status <>` (multiple), `trans_date <=`, `ThirdPartyBalance <> 0`
- ORDER BY `pat_name`
- Uses composite (fin_code, trans_date, status) pattern -- partially covered by existing composite index

#### 4. Charge Lookups (ChrgRepository)
- **GetByAccount(account)**: Filters on `chrg.account` with optional filters on `credited`, `status`, `invoice`, `cdm`
- **GetChargesForInvoiceSelect**: Joins `chrg` to `acc`, filters on `cl_mnem`, `trans_date <=`, `status NOT IN`, `invoice IS NULL`, `fin_type =`
- Heavy use of `account` column -- covered by `account_index`

#### 5. Insurance Lookups (InsRepository)
- **GetByAccount(account)**: Uses clustered PK (account, ins_a_b_c)
- Filters on `deleted = 0` combined with account

#### 6. HL7 Message Processing (MessagesInboundRepository)
- **GetUnprocessedMessages()**: Filters on `processFlag = 'N'` ORDER BY `msgDate` -- **CRITICAL: missing dedicated index**
- **GetQueueCounts()**: Groups by `msgType, processFlag` with COUNT -- partially covered by `IX_PROC_DFT`
- **GetByDateRange()**: Filters on `msgDate BETWEEN` -- partially covered

#### 7. Payment Lookups (ChkRepository)
- **GetByAccount(account)**: Filters on `chk.account` -- covered by `IX_account`
- **GetByBatch(batch)**: Filters on `chk.batch` -- covered by `batch_idx`

#### 8. Billing Activity (BillingActivityRepository -> data_billing_history)
- **GetByAccount(account)**: Filters on `data_billing_history.account` -- covered by clustered PK (account, run_date)
- **GetByRunDate(fromDate, thruDate)**: Filters on `run_date BETWEEN` -- **NOT covered: run_date is 2nd column of PK**
- **GetBatch(batch)**: Filters on `batch` -- covered by `IX_batch_w_includes`

#### 9. Patient Statement Accounts (PatientStatementAccountRepository)
- **GetByBatch(batch_id)**: Filters on `batch_id` WHERE `date_sent IS NULL`
- **GetByAccount(account_id)**: Filters on `account_id`
- **GetByStatement(statement_number)**: Filters on `statement_number`

#### 10. Claim Generation (ClaimGeneratorService -> AccountService.GetClaimItems)
- Complex query joining `chrg` to `acc`, filtering on `fin_type`, `status`, `credited`, `invoice IS NULL`
- Uses `invoice_select_idx` on chrg

---

## Phase 3: Cross-Reference Analysis

### 1. Redundant Indexes

#### R1: `dbo.acc_location` -- `IX_acc_location` is redundant with wider covering index

| Property | Narrow Index | Wide Index |
|----------|-------------|------------|
| **Name** | `IX_acc_location` | `ix_location, surveydate INCLUDE ...` |
| **Key Columns** | (location, surveydate) | (location, surveydate) |
| **Include Columns** | None | account, pt_type, mod_date, mod_user, mod_prg, mod_host, ov_acct, ov_mri |

**Analysis:** Both indexes have identical key columns (location, surveydate). The wider index can serve all queries the narrow one handles, plus provides covering index benefit. The narrow index serves no unique purpose.

**Recommendation:** DROP `IX_acc_location`
**Impact:** LOW -- `acc_location` is not a high-traffic table; saves minor write overhead and storage.
**Trade-off:** None -- the wider index fully subsumes the narrower one.

#### R2: `dbo.acc` -- `IX_acc_status` partial overlap with composite index

| Property | Status-Only Index | Composite Index |
|----------|------------------|-----------------|
| **Name** | `IX_acc_status` | `ix_status_transdate_fincode` |
| **Key Columns** | (status) | (status, trans_date, fin_code) |
| **Include Columns** | account, pat_name, cl_mnem, fin_code, trans_date | account, mod_prg |

**Analysis:** The single-column `status` index can be satisfied by the leading column of the composite index for simple equality lookups. However, the `IX_acc_status` index includes more columns (pat_name, cl_mnem, fin_code, trans_date) making it a covering index for `AccountSearchView` queries that filter by status. The composite index only includes (account, mod_prg). **These serve different purposes and are NOT truly redundant.**

**Recommendation:** KEEP BOTH -- they serve distinct query patterns. The status-only index covers more columns needed by the search view.

---

### 2. Missing Indexes

#### M1: `infce.messages_inbound` -- Missing index for GetUnprocessedMessages() (HIGH IMPACT)

**Query Pattern:** `WHERE processFlag = 'N' ORDER BY msgDate`
This is called by `HL7ProcessorService.ProcessMessages()` which runs continuously as a background service processing thousands of messages.

**Current Indexes:**
- `IX_PROC_DFT`: (msgType, processFlag, msgDate) -- processFlag is the 2nd column, requiring scan of all msgType values
- `IX_sourceMsgId`: (sourceMsgId, sourceInfce, msgType) -- irrelevant
- `IX_account_cerner`: (account_cerner, DOS) -- irrelevant

**Recommendation:** Create `IX_messages_inbound_processFlag_msgDate` on (processFlag, msgDate) with appropriate INCLUDE columns
**Impact:** HIGH -- This is the main driver for HL7 processing batch operations. Currently requires scanning through the entire `IX_PROC_DFT` index or table scan.

#### M2: `dbo.data_billing_history` -- Account lookups already covered (NO ACTION)

**Query Pattern:** `WHERE account = @account`
Called by `BillingActivityRepository.GetByAccount()` which is used during account detail loading.

**Verification:** The clustered PK on (account, run_date) already supports this query efficiently since `account` is the leading column. No additional index needed.

#### M3: `dbo.chrg` -- Missing composite index for credited + account + cdm filtering (MEDIUM IMPACT)

**Query Pattern:** Multiple stored procedures and views filter on `chrg.credited = 0 AND chrg.account = @account AND chrg.cdm <> 'CBILL'`

**Current Indexes:** The `account_index` on (account) covers the account filter but not the `credited` and `cdm` filters simultaneously. The `credited_account_status_Includes` covers (credited, account, status) but not cdm.

**Recommendation:** Add `cdm` to the INCLUDE columns of `credited_account_status_Includes` index
**Impact:** MEDIUM -- Avoids key lookups for the most common charge query pattern.

#### M4: `dbo.data_billing_history` -- Missing index on run_date for date range queries (MEDIUM IMPACT)

**Query Pattern:** `WHERE run_date BETWEEN @fromDate AND @thruDate` from `BillingActivityRepository.GetByRunDate()`

**Current Indexes:**
- Clustered PK on (account, run_date) -- supports account lookups but NOT standalone run_date range queries
- `IX_batch_w_includes` on (batch) -- irrelevant for date range queries

**Note:** `GetByAccount(account)` is already well-served by the clustered PK since `account` is the leading column.

**Recommendation:** Create nonclustered index on (run_date) INCLUDE(account, ins_abc, fin_code, batch, ebill_status, claim_amount)
**Impact:** MEDIUM -- Used for billing activity reports and auditing by date range.

#### M5: `infce.messages_inbound` -- Missing index for GetByDateRange (LOW-MEDIUM IMPACT)

**Query Pattern:** `WHERE msgDate BETWEEN @fromDate AND @thruDate` from `MessagesInboundRepository.GetByDateRange()` and `GetByMessageType()`.

**Current Indexes:**
- `IX_PROC_DFT`: (msgType, processFlag, msgDate) -- msgDate is the 3rd column, usable only when msgType and processFlag are both specified
- `IX_account_cerner`: (account_cerner, DOS) -- irrelevant

**Recommendation:** Create nonclustered index on (msgDate) for standalone date range queries. The existing `IX_PROC_DFT` covers the `GetByMessageType()` pattern since it filters on msgType first.
**Impact:** LOW-MEDIUM -- Used for message browsing/auditing UI.

#### M6: `dbo.acc_validation_status` -- Already indexed via PK (NO ACTION)

**Query Pattern:** `WHERE account = @account` from `AccountValidationStatusRepository.GetByAccount()`

**Verification:** PK is already CLUSTERED on `account`. No additional index needed.

---

### 3. Suboptimal Index Definitions

#### S1: `dbo.chrg` `account_index` -- Missing INCLUDE columns for common query (MEDIUM IMPACT)

**Current:** (account) INCLUDE(status, service_date, cdm, qty, net_amt, credited, chrg_num, invoice)
**Query Pattern:** `ChrgRepository.GetByAccount()` frequently also needs `fin_code`, `cl_mnem`, and `fin_type` which are not in the INCLUDE list, causing key lookups.

**Recommendation:** Add `fin_code`, `cl_mnem`, `fin_type` to the INCLUDE columns
**Impact:** MEDIUM -- Eliminates key lookups for the most common charge retrieval pattern.
**Trade-off:** Slightly wider index pages, minimal write impact.

#### S2: `dbo.chrg` `credited_account_status_Includes` -- Missing INCLUDE columns (LOW IMPACT)

**Current:** (credited, account, status) INCLUDE(chrg_num, qty)
**Query Pattern:** Queries using this index often also need `cdm`, `service_date`, `net_amt`

**Recommendation:** Add `cdm`, `service_date`, `net_amt` to INCLUDE columns
**Impact:** LOW-MEDIUM -- Reduces key lookups for charge summary queries.

#### S3: `infce.messages_inbound` `IX_PROC_DFT` -- Suboptimal column order for unprocessed message retrieval (HIGH IMPACT)

**Current:** (msgType, processFlag, msgDate)
**Primary Query Pattern:** `WHERE processFlag = 'N' ORDER BY msgDate` -- processFlag is not the leading column
**Secondary Pattern:** `WHERE msgType = @type AND processFlag = @flag ORDER BY msgDate` -- this pattern IS well served

**Analysis:** The index was designed for the secondary pattern (filtering by message type first). For the primary "get unprocessed messages" pattern, a separate index with processFlag as the leading column would be more efficient.

**Recommendation:** Rather than modifying this index (which serves the secondary pattern), create a new index as described in M1.
**Impact:** HIGH -- The primary batch processing loop queries unprocessed messages continuously.

---

### 4. Over-Indexed Tables

#### `dbo.chrg` -- 6 Nonclustered Indexes

| Index | Key Columns | Used By |
|-------|-------------|---------|
| `account_fin_type_status_Includes` | (account, fin_type, status) | Claim item selection |
| `account_index` | (account) | General account lookups |
| `credited_account_status_Includes` | (credited, account, status) | Uncredited charge queries |
| `credited_cl_mnem_status_invoice_fin_code_Includes` | (credited, cl_mnem, status, invoice, fin_code) | Invoice selection |
| `invoice_select_idx` | (fin_type, status, invoice) | Invoice batch processing |

**Analysis:** All 5 indexes appear to serve distinct query patterns. The `account_index` and `account_fin_type_status_Includes` have overlapping leading columns, but the composite adds fin_type and status to support specific claim queries.

**Recommendation:** The `account_fin_type_status_Includes` index could potentially be replaced by enhancing `account_index` with additional INCLUDE columns, but the different key column combinations serve different seek patterns. **No immediate consolidation recommended.**

**Note:** Any INSERT/UPDATE to `chrg` must maintain all 6 indexes. For a high-volume transaction table, this is a consideration but within acceptable limits (typical guidance suggests concern at 8-10+ indexes).

---

### 5. Clustered Index Review

#### Good Clustered Index Choices:
- `dbo.acc` -- PK on `account` (varchar) -- natural key, used in virtually all joins. Good choice.
- `dbo.chrg` -- PK on `chrg_num` (identity/sequential) -- narrow, ever-increasing. Excellent choice.
- `dbo.ins` -- PK on `(account, ins_a_b_c)` -- composite natural key, supports the primary access pattern. Good choice.
- `dbo.chk` -- PK on `pay_no` -- sequential. Good choice.
- `dictionary.client` -- PK on `id` (identity). Good choice; also has unique index on `cli_mnem` for mnemonic lookups.

#### Potential Concerns:
- `dbo.chrg_details` -- Has a NONCLUSTERED PK on `uri` and a separate CLUSTERED index on `chrg_num`. This is intentional and correct: the clustered index on `chrg_num` ensures that detail rows for the same charge are physically co-located, which optimizes the most common access pattern (get all details for a charge).

- `dbo.patbill_enctr` -- Clustered on (statement_number, enctr_nbr, pft_encntr_id, place_of_service). This is a wide clustered key (4 columns). Every nonclustered index row will carry this key. If there are many nonclustered indexes on this table, the overhead could be significant. Currently only referenced by `IX_statement_batch` which is on a different table.

- `dbo.rds` -- Clustered on `name`. Acceptable for a small reference table.

#### No Heap Tables Identified:
All transactional tables have appropriate clustered indexes.

---

## Priority-Ordered Recommendations

| Priority | Type | Table | Action | Impact | Effort |
|----------|------|-------|--------|--------|--------|
| 1 | Missing | `infce.messages_inbound` | Create index on (processFlag, msgDate) | HIGH | Low |
| 2 | Suboptimal | `dbo.chrg` `account_index` | DROP+CREATE with fin_code, cl_mnem, fin_type added to INCLUDE | MEDIUM | Low |
| 3 | Missing | `dbo.data_billing_history` | Create index on (run_date) with INCLUDEs | MEDIUM | Low |
| 4 | Suboptimal | `dbo.chrg` `credited_account_status_Includes` | DROP+CREATE with cdm, service_date, net_amt added to INCLUDE | MEDIUM | Low |
| 5 | Missing | `infce.messages_inbound` | Create index on (msgDate) for date range queries | LOW-MEDIUM | Low |
| 6 | Redundant | `dbo.acc_location` | DROP IX_acc_location | LOW | Low |

---

## Trade-off Analysis

### Write Performance Impact
- Adding 2-3 new indexes to `messages_inbound`: Minimal impact since HL7 messages are inserted once and updated a few times.
- Widening INCLUDE columns on `chrg` indexes: No key column changes, only wider leaf pages. Negligible write impact.
- Dropping the redundant `acc_location` index: Small write performance improvement.

### Storage Impact
- New indexes are estimated to add < 1% to total database size given the focused nature of the recommendations.
- Widening INCLUDE lists is the most storage-intensive change but still modest.

### Risk Assessment
- All recommendations are additive or non-destructive (adding INCLUDE columns requires DROP+CREATE but preserves functionality).
- The redundant index DROP has been verified not to affect any unique constraint or foreign key.
- No recommendations modify clustered indexes or primary keys.
