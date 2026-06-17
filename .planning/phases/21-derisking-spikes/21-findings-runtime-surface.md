# 21-02 Findings — Runtime DB-Object Surface

> **Method:** static call-graph (the plan's documented fallback). The live dev DB
> (`LabBillingTest`) has no business data and the LabBillingService/Quartz scheduler
> were not running, so a representative Extended Events trace was not feasible. Instead
> the .NET layer was scanned for every literal proc/UDF invocation and every PetaPoco
> `[TableName]` (table/view) reference, cross-checked against the live catalog.
> **Confidence: STATIC-ONLY, unverified by execution.** The XE capture/analysis scripts
> (`spikes/xe-*.sql`) are committed and ready to run the moment a data-rich, safeguarded
> environment is available — that would upgrade this to traced confidence.

## Headline

- **Runtime-critical procedural surface (called directly from .NET) = 7 objects**: 4 stored procedures + 3 scalar functions. All 7 confirmed present in the catalog.
- **Runtime-critical views = ~8–10**: 8 confirmed in this DB + 2 more that are views by definition but absent from this (drifted) snapshot.
- **Catalog total:** 158 procs + 62 scalar FN + 7 inline-TVF + 51 TVF = **278 procs/UDFs**, plus **100 views** → 378 catalog objects.
- **Deferrable:** ~271 of 278 procs/UDFs and ~90 of 100 views are **never invoked directly from application code** — they are SQL-Agent/batch/reporting/in-DB-only and can be ported incrementally **after** cutover.

This confirms the §6 hypothesis in `analyses/dbms-modernization-and-redis-hotspots.md`: **the runtime-blocking procedural surface is tiny (~7 objects), not ~270.** Phase 23 can port the runtime set first and defer the rest.

## (A) Runtime-critical objects (entry points from .NET)

| Object | Type | Called from (file:line) | Purpose | Phase 23 plan |
|---|---|---|---|---|
| `dbo.GetAccBalance` | scalar UDF | `ClientRepository.cs:85` | Account balance (inline `SELECT`) | **23-01** |
| `dbo.GetAccBalByDate` | scalar UDF | `ClientRepository.cs:97` | Account balance as-of-date | **23-01** |
| `dbo.GetAccClientBalance` | scalar UDF | `ClientRepository.cs:117` | Client balance (inline in `Sql.Builder`) | **23-01** |
| `GetNextNumber` | proc | `NumberRepository.cs:28` (`;EXEC … OUTPUT`) | Number/batch allocation | **23-01** |
| `usp_cerner_chrg_reprocess` | proc | `MessagesInboundRepository.cs:90` (`ExecuteNonQueryProc`) | HL7/Cerner charge reprocess | **23-02** |
| `usp_prg_pat_bill_update_flags` | proc | `PatientBillingService.cs:745` | Patient-billing batch (flag update) | **23-02** |
| `usp_prg_pat_bill_compile` | proc | `PatientBillingService.cs:749` | Patient-billing batch (compile) | **23-02** |

**Runtime-critical views** (referenced via PetaPoco `[TableName]` models → Phase **23-04**):

| View | Model | Present in test DB? |
|---|---|---|
| `dbo.AccountSearchView` | `AccountSearch` | yes |
| `dbo.vw_cbill_select` | `InvoiceSelect`, `UnbilledAccounts` | yes |
| `dbo.vw_chrg_bill` | `ClaimChargeView` | yes |
| `dictionary.cpt4`, `cpt4_2`, `cpt4_3`, `cpt4_4`, `cpt4_5` | `CdmDetail` | yes (6 CDM/CPT lookup views) |
| `StatementChargeView` | `StatementChargeView` | no (drifted — see caveats) |
| `InvoiceChargeView` | `InvoiceChargeView` | no (drifted) |

## (B) Deferrable surface (never invoked directly from .NET)

- **~154 stored procedures** (158 − 4): SQL-Agent nightly jobs, XML-import/reconciliation, reporting, `sp_send_dbmail` steps. Port incrementally post-cutover (Phase 24 moves the SQL-Agent jobs to Quartz; the procs they call follow).
- **~59 scalar functions, 7 inline-TVFs, 51 TVFs** (e.g. `DelimitedSplit8K`, `ArrayToTable`, `GetMappingValue`): not called from .NET. **Caveat:** several are called *transitively* by the runtime procs above (in-DB), so they become runtime-critical via dependency closure — see caveats.
- **~92 views**: reporting/audit/dictionary views not bound to any runtime model.

## (C) Phase 23 sizing takeaway

Phase 23 is reducible to: **port 7 procedural objects + ~10 views first (the runtime-blocking set), then sweep the rest incrementally.** Combined with 21-01 (only ~10 of 67 repo files carry hand-written T-SQL) and the MARS = not-load-bearing finding, the .NET-side and runtime-procedural cutover is far smaller than the raw catalog counts suggested.

## Caveats / confidence limits (STATIC-ONLY)

1. **No execution trace.** Pure static analysis. The committed `spikes/xe-runtime-surface-session.sql` + `xe-analyze-captured-objects.sql` (HIPAA-safe: object identity only, no statement text/PHI) will confirm this against a real workload when a data-rich environment exists.
2. **Transitive in-DB dependencies not captured.** The 7 runtime procs/UDFs call other procs/UDFs internally (XML shredding, `DelimitedSplit8K`, `GetMappingValue`, etc.). Those are runtime-critical by closure but invisible to a .NET-only grep. Phase 23 must compute the dependency closure (`sys.sql_expression_dependencies`) of the 7 entry points before declaring the deferrable set safe.
3. **Raw-SQL / dynamic paths.** `ReportingRepository.cs` uses raw `SqlConnection`/`SqlDataAdapter` (flagged in 21-01) and may reference objects not captured by the `[TableName]`/`EXEC` greps. Re-scan during Phase 23.
4. **Test-DB schema drift.** `LabBillingTest` is an older/partial snapshot: referenced objects `acc_lock`, `cpt4_ama`, `chrg_diag_pointer`, `remittance_claim*`, `remittance_file`, `StatementChargeView`, `InvoiceChargeView`, `LMRPRuleDefinition`, `Logs` (lives in the `NLog` DB) are **absent here but exist in production**. Catalog counts (278/100) are this snapshot's; production may differ. Size Phase 23 against the production catalog, not this snapshot.
