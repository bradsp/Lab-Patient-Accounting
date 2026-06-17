# Phase 21 — De-Risking Spikes: Consolidated FINDINGS

> Hand-off document. Rolls up the four Phase 21 spikes into locked decisions, per-phase
> sizing, and carried-forward risks — enough to detail-plan Phases 22–25 without re-reading
> the individual findings. Sources: `21-findings-repo-tsql-backlog.md`, `21-findings-mars-audit.md`,
> `21-findings-runtime-surface.md`, `21-findings-xml-shredding.md`, `21-findings-hosting.md`,
> and SUMMARYs 21-01…21-04.

## 1. Decisions locked

| # | Decision | Outcome | Source |
|---|---|---|---|
| D1 | DBMS (pre-Phase-21) | **PostgreSQL** (17.x/18.x line) | analysis §3 |
| D2 | HL7 XML-shredding destination | **C# / j4jayant (app tier)** — move shredding out of the DB; do NOT port the ~1,616 XML methods to `xmltable()`. A narrow `xmltable()`/`XDocument` fallback only if a pipe-less XML-only source is found. | 21-03 |
| D3 | Target managed hosting SKU | **AWS RDS for PostgreSQL** (PG 17, Multi-AZ, KMS CMK + `rds.force_ssl` + pgaudit) — primary; **Azure Flexible Server** fallback. **Validation DEFERRED** (see R1). | 21-04 hosting |
| D4 | MARS load-bearing? | **No** — incidental; 0 refactor sites. Just delete `MultipleActiveResultSets=true`. | 21-01 MARS |
| D5 | ORM | **Keep PetaPoco** — built-in `PostgreSQLDatabaseProvider`; delete the SQL-Server-only `CustomSqlDatabaseProvider`. | analysis §3.5 |

## 2. Sizing inputs per downstream phase

### Phase 22 — PostgreSQL Schema & Data-Access Foundation
- **Schema port:** ~500 tables (production); IDENTITY→`GENERATED … AS IDENTITY`, computed columns→`GENERATED ALWAYS AS … STORED`, 7 schemas, ~209 views (production). Bracket→double-quote handled by the PG provider.
- **Provider swap:** `Microsoft.Data.SqlClient` → `Npgsql`; PetaPoco `UsingProvider<CustomSqlMsDatabaseProvider>` → built-in `PostgreSQLDatabaseProvider` at the 2 UoW sites (`UnitOfWorkMain.cs:98-108`, `UnitOfWorkSystem.cs:36-46`); **delete** `CustomSqlDatabaseProvider.cs` (native `RETURNING`).
- **MARS:** delete `MultipleActiveResultSets=true` at `AppEnvironment.cs:83` and `:132` — **0** code-path refactors needed (D4).
- **TLS:** rebuild `AppEnvironment` connection construction for Npgsql; set `SSL Mode=VerifyFull` + `Root Certificate=<RDS CA bundle>` + `Channel Binding=Require`. **Closes the current `Encrypt=false;TrustServerCertificate=true` HIPAA gap** (R6).
- **Repo T-SQL backlog (from 21-01):** **10 of 67 repository files (~15%)**, **18 live call sites** — much smaller than the analysis's 40–50% estimate. P1 sites: `ClientRepository.cs:85,97,117`, `NumberRepository.cs:28`, `MessagesInboundRepository.cs:90` (some land in Phase 23).

### Phase 23 — In-Database Logic & Repository Port
- **Runtime-critical procedural surface = 7 objects** (4 procs + 3 scalar UDFs) + **~8–10 views**, vs ~278 procs/UDFs + 100 views in the catalog. **~271 procs/UDFs and ~90 views are DEFERRABLE** to post-cutover (21-02).
  - **23-01:** `GetAccBalance`, `GetAccBalByDate`, `GetAccClientBalance` (scalar UDFs) + `GetNextNumber` (proc) → PL/pgSQL.
  - **23-02:** HL7 shredding → **C#/j4jayant** (D2): move the import procs' surrounding logic (dup-check/merge, `GetMasterAccount`, `GetMappingValue`, dedupe) into services, wrap derived inserts in the `UnitOfWork` transaction; covers `usp_cerner_chrg_reprocess`, `usp_prg_pat_bill_update_flags`, `usp_prg_pat_bill_compile`.
  - **23-03:** repository T-SQL rewrite — 10 files / 18 sites (`TOP`→`LIMIT`, inline UDF/proc calls, single-quoted aliases); plus `ReportingRepository.cs` raw `SqlConnection`/`SqlDataAdapter` full Npgsql repoint (R5).
  - **23-04:** ~10 runtime views (`AccountSearchView`, `vw_cbill_select`, `vw_chrg_bill`, `cpt4`×6, `StatementChargeView`, `InvoiceChargeView`).
- **Required first step:** compute the **dependency closure** (`sys.sql_expression_dependencies`) of the 7 entry points — transitive in-DB deps (e.g. `DelimitedSplit8K`, `GetMappingValue`) are runtime-critical but invisible to the static call-graph (R2).

### Phase 24 — Background Jobs → Quartz.NET
- Unchanged scope: **4 SQL Agent jobs** → Quartz; `sp_send_dbmail` (~17 uses) → app SMTP; retire `H:\sqlText` Windows-path coupling. The CDM/XML-import job logic moves to C# alongside D2 (don't re-port it to PL/pgSQL).

### Phase 25 — Data Migration & Cutover
- **Target:** AWS RDS for PostgreSQL 17 (D3, pending R1). Bulk load via pgloader / AWS DMS; row-count + checksum parity vs SQL Server; dual-run verification.

## 3. Open risks carried forward

| # | Risk | Impact | Action |
|---|---|---|---|
| R1 | **Managed-SKU validation DEFERRED** — AWS RDS PG 17 (BAA + CMK + `rds.force_ssl` + pgaudit + Npgsql `VerifyFull`) not yet stood up. | Gates the security review and the Phase 25 target. | Validate early in Phase 22 (procurement/legal + a throwaway instance). Steps + Npgsql settings in `21-findings-hosting.md`. |
| R2 | Runtime surface is **STATIC-ONLY** confidence (no execution trace). | Deferrable set could omit objects reached only at runtime; transitive deps uncounted. | Run the committed `spikes/xe-*.sql` (HIPAA-safe) against a data-rich env; compute dependency closure in Phase 23. |
| R3 | **Test-DB schema drift** — `LabBillingTest` is an older/partial snapshot (missing `acc_lock`, `cpt4_ama`, `chrg_diag_pointer`, `remittance_*`, `StatementChargeView`, etc.). | Catalog counts here ≠ production. | Size Phase 23 against the **production** catalog; reconcile the model↔DB drift. |
| R4 | C# shredding (D2) assumes the pipe `HL7Message` column is universally present/authoritative. | If a source populates only XML `msgContent`, the C# path misses it. | Confirm in Phase 23-02; keep the narrow `xmltable()`/`XDocument` fallback. |
| R5 | `ReportingRepository.cs` uses raw `SqlConnection`/`SqlDataAdapter` (bypasses PetaPoco). | Not covered by the provider swap; needs hand-port. | Full Npgsql repoint in Phase 23-03. |
| R6 | `Encrypt=false;TrustServerCertificate=true` today — TLS-in-transit disabled. | HIPAA in-transit gap (independent of engine). | Close in Phase 22 (Npgsql `SSL Mode=VerifyFull`). |

## 4. Bottom line
The migration's blocking surface is **far smaller** than the raw catalog implies: ~15% of repositories carry T-SQL, MARS is a no-op, and the runtime-critical procedural surface is **~7 objects + ~10 views** (the rest deferrable). The two big shapes are decided (PostgreSQL via PetaPoco's built-in provider; HL7 shredding consolidated into C#). The one true gate is **R1** — validating the managed HIPAA SKU — which is procurement/legal, not engineering. Phases 22–25 can be detail-planned from this document.
