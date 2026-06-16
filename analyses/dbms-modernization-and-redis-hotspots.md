# DBMS Modernization & Redis Hotspot Analysis

> Planning-grade input for the SQL Server replacement roadmap and the Redis caching initiative.
> Scope: Lab Patient Accounting (HIPAA-regulated outreach-lab billing; HL7 ingestion, X12 837 claims).
> Evidence base: codebase inventory (June 2026, branch `billing-ui-to-blazor`) + current vendor/driver research.

---

## 1. Executive Summary

- **Recommended DBMS: PostgreSQL** (current stable 18.x / 17.x line; target the latest 17.x or 18.x). One sentence: it is the only open-source candidate that simultaneously matches SQL Server's relational/T-SQL feature depth this schema actually leans on (XML shredding, CTEs, window functions, scalar/table UDFs), ships as a **first-class, out-of-the-box provider in the exact ORM package this app already uses (`PetaPoco.Compiled`)**, and carries a signed-BAA managed offering from every major cloud (AWS RDS/Aurora, Azure, GCP, Crunchy Bridge, Aiven).

- **Top 3 Redis hotspots (priority order):**
  1. **Reference-data dictionary lookups** in `DictionaryService` (CPT/AMA, diagnosis, CDM, revenue, fin codes, client/insurance/mapping) — `LabBilling Library/Services/DictionaryService.cs`. Read-heavy, change-rarely, hit on nearly every account load, charge, and HL7 message.
  2. **Account-assembly fan-out** in `AccountService.GetAccount` — `LabBilling Library/Services/AccountService.cs:364`. A single load fans out into ~25–40 sub-queries (9 diagnosis lookups, per-charge CDM/revenue/CPT lookups). The *reference data* inside that fan-out is the cacheable part; the account record itself is not.
  3. **Auth/identity lookups** in the Blazor app — `LabOutreachUI/Authentication/CustomAuthenticationStateProvider.cs` calls `UserAccountRepository.GetByUsername` per auth-state check; today cached only per-circuit in a process-local `ConcurrentDictionary` (`LabOutreachUI/Services/UserCircuitHandler.cs:19`), which does not survive multi-node Blazor Server scale-out.

- **Single biggest migration risk:** the **in-database T-SQL business/reporting logic** — 155 stored procedures, 115 user-defined functions, and ~1,616 occurrences of SQL Server XML methods (`.value()`/`.nodes()`/`.query()`/`FOR XML PATH`) used to shred HL7 inside `usp_prg_Xml_Import*` and the SQL Agent nightly jobs. This is not a schema-translation problem (schema is portable); it is a procedural-code port. PostgreSQL has equivalents for all of it (`xpath()`/`xmltable()`, PL/pgSQL, `RETURNING`), but the XML-shredding procs and `sp_send_dbmail` reporting jobs are a genuine rewrite, not a tool-converts-it task.

- **Single biggest unknown to resolve before committing:** whether the **3 scalar SQL functions called inline from .NET SQL strings** (`dbo.GetAccBalance`, `dbo.GetAccBalByDate`, `dbo.GetAccClientBalance` — `LabBilling Library/Repositories/ClientRepository.cs:85,97,117`) and the **one stored proc called from .NET** (`usp_cerner_chrg_reprocess` — `MessagesInboundRepository.cs:90`; plus `GetNextNumber` via `NumberRepository.cs:28`) represent the *full* runtime coupling, or whether views/procs are invoked indirectly. If the app's runtime path only touches a handful of DB objects (likely), the migration's runtime-critical surface is small and the other ~150 procs are batch/reporting that can be ported incrementally. The smallest experiment is in §6.

---

## 2. Current State Inventory

### 2.1 SQL Server feature surface in use

Counts and citations from `LabBillingDatabase/` (SQL Server Database Project) and the SQL Agent jobs file.

| Feature | Usage | Evidence |
|---|---|---|
| Stored procedures | **Heavy** — ~155 | `LabBillingDatabase/dbo/Stored Procedures/` (e.g. `usp_account_payout.sql`, `usp_prg_Xml_Import.sql`, `usp_cerner_acc_create.sql`) |
| Scalar + table-valued UDFs | **Heavy** — ~115 | `LabBillingDatabase/dbo/Functions/` — `GetMappingValue.sql`, `DelimitedSplit8K.sql` (TVF, ROW_NUMBER), `ArrayToTable.sql` (XML→TABLE), `GetAccBalance` family |
| Views | **Heavy** — ~209 | across `dbo`, `audit`, `infce`, `app`, `dict`, `dictionary` schemas |
| XML columns + XML methods | **Heavy** — ~1,616 occurrences / 38 files | `usp_prg_Xml_Import.sql:50,66`, `ArrayToTable.sql:9-11`, SQL Agent jobs shred HL7 via `.value()`/`.nodes()`/`CROSS APPLY` (`Lab Patient Accounting SQL Agent Jobs.sql:543-622`) |
| CTEs (`WITH … AS`) | **Moderate** — ~72 / 40 files | `usp_prg_Xml_Import.sql`; SQL Agent `cteQ`/`cteData`/`cteCdm`/`xCDM` chains |
| Window functions (`ROW_NUMBER OVER`) | **Moderate** — ~72 | `DelimitedSplit8K.sql:19,27`, `GenerateCalendar.sql`, agent-job report ordering |
| `OUTPUT` / `OUTPUT INTO` | **Light** — ~21 | XML import procs; also synthesized by the ORM provider (see §2.2) |
| Computed columns (`AS …`) | **Light** — ~31 | `chrg.sql:45-46` (`age_on_date_of_service`, `calc_amt`) |
| IDENTITY columns | **Heavy** — ~91 | every key table; e.g. `chrg.chrg_num` |
| Multiple schemas | **Moderate** — 7+ | `dbo`, `audit`, `infce`, `dict`, `dictionary`, `app`, `tst`/`zzz` (`LabBillingDatabase/Security/*.sql`) |
| `TOP` / bracketed identifiers | **Moderate** | `TOP` ~90; brackets pervasive (tooling-generated) |
| Triggers | **Light** — 1 DDL audit trigger | `LabBillingDatabase/Database Triggers/Audit.sql` (uses `EVENTDATA()` + XML). Note: 23 `audit.*` tables exist but are populated by procedures, **not** DML triggers. |
| `sp_send_dbmail` / Database Mail | **Light** — ~17 | `usp_account_payout.sql`, `sp_job_out_of_balance.sql`, and SQL Agent report steps |
| Linked servers (`OPENQUERY`) | **Light** — 4 | `usp_prg_Xml_Import_Charges_Verify.sql`, `…Accounts_Verify.sql` |
| MERGE | **None** (0) | — |
| ROWVERSION / TIMESTAMP | **None** (0) | concurrency is **not** rowversion-based |
| Table-valued parameters (`CREATE TYPE … AS TABLE`) | **None** (0) | the app passes sets as XML instead |
| JSON functions (`OPENJSON`/`JSON_VALUE`/`FOR JSON`) | **None** (0) | XML is used everywhere JSON would be |
| Full-text search | **None** (0) | — |
| SQLCLR (`CREATE ASSEMBLY`) | **None** (0) | — |
| Sequences (`CREATE SEQUENCE`) | **None** (0) | uses IDENTITY + a `GetNextNumber` proc |
| `OFFSET … FETCH` | **None** (0) | paging done via `TOP`/app-side |

**Read of the surface:** the *blockers* are XML shredding, the proc/UDF volume, computed columns, `sp_send_dbmail`, and linked-server verify steps. The *non-issues* (MERGE, ROWVERSION, TVPs, JSON, FTS, SQLCLR, sequences) are absent, which removes the features that are hardest to port to non-SQL-Server engines. The absence of ROWVERSION matters: there is no rowversion-based optimistic-concurrency contract to re-implement.

### 2.2 Data access layer & PetaPoco binding

- **ORM:** `PetaPoco.Compiled` **v6.0.683** (`LabBilling Library/LabBilling Core.csproj:52`; same in console/service csproj). This is the maintained CollaboratingPlatypus PetaPoco, not a dead fork.
- **Provider (today):** custom `CustomSqlMsDatabaseProvider : SqlServerMsDataDatabaseProvider` (`LabBilling Library/Repositories/CustomSqlDatabaseProvider.cs:9`). Its **only** behavior is rewriting PetaPoco's `OUTPUT INSERTED.[pk]` into a `@result` table-variable to dodge SQL Server **error 334** (`CustomSqlDatabaseProvider.cs:11-43`). This entire class is SQL-Server-specific and **disappears** on PostgreSQL, where PetaPoco's `PostgreSQLDatabaseProvider` returns the key via native `RETURNING`.
- **SqlClient:** `Microsoft.Data.SqlClient` **6.1.2** (`LabBilling Core.csproj:47`) — the modern, supported client. Connection strings built with `SqlConnectionStringBuilder` (`LabBilling Library/Repositories/AppEnvironment.cs:64-104`), `Encrypt=false;TrustServerCertificate=true;MARS=on` — i.e. **TLS is currently disabled in transit**, a HIPAA gap independent of the DBMS choice (see §4).
- **Two UoWs, identical construction:** `UnitOfWorkMain` (`LabBilling Library/UnitOfWork/UnitOfWorkMain.cs:98-108`) and `UnitOfWorkSystem` (`UnitOfWork/UnitOfWorkSystem.cs:36-46`) both `DatabaseConfiguration.Build().UsingProvider<CustomSqlMsDatabaseProvider>(…).UsingCommandTimeout(180).WithAutoSelect().UsingDefaultMapper<MyMapper>(…)`. Migration touches exactly these two sites plus the custom provider.
- **Raw T-SQL leakage into repositories (the real porting cost in .NET):**
  - `TOP n` string interpolation — `UserProfileRepository.cs:35`
  - stored-proc call with `OUTPUT` param — `NumberRepository.cs:28` (`;EXEC GetNextNumber @0, @1 OUTPUT`)
  - inline scalar-UDF calls — `ClientRepository.cs:85` (`dbo.GetAccBalance`), `:97` (`dbo.GetAccBalByDate`), `:117` (`dbo.GetAccClientBalance`)
  - `LEFT(col,3)` and single-quoted alias — `MessagesInboundRepository.cs:23`
  - proc invocation via PetaPoco — `MessagesInboundRepository.cs:90` (`ExecuteNonQueryProc("usp_cerner_chrg_reprocess", …)`)
  - bracket identifiers + `Sql.Builder` joins/`GROUP BY` across many repos (`ChrgRepository`, `InvoiceSelectRepository`, `ClientRepository`)
  Estimated ~40–50% of repositories embed hand-written `Sql.Builder` SQL; the remainder use PetaPoco auto-CRUD (portable for free).
- **Async:** partial (~10–15% of repos use true `…Async`; several "async" methods wrap sync calls in `Task.Run`, e.g. `AccountRepository.cs`). Relevant because Blazor Server wants real async I/O; Npgsql has first-class async, so the migration is also an opportunity to fix this.
- **In-memory caching today:** essentially none in the shared library — only static lookup dictionaries (`LabBilling Library/Models/Dictionaries.cs`) and a **WinForms-only** `LazyCache` layer (`Lab PA WinForms UI/DataCache.cs`) caching fins/insc/phy/client/cdm/revcode. The Blazor app does **not** share that cache — directly relevant to §5.

### 2.3 Background workloads & SQL Agent dependence

The nightly/periodic automation lives in **SQL Server Agent**, which has no open-source equivalent. From `Lab Patient Accounting SQL Agent Jobs.sql`:

| Job | What it does | Replacement implication |
|---|---|---|
| Accounts Aging Payout | `EXEC dbo.usp_account_payout` nightly | Move scheduling to **Quartz.NET** (already in `Lab Patient Accounting Job Scheduler`); port proc to PL/pgSQL or to a C# service method |
| PROD BadDebt Account Writeoff | monthly `INSERT chk … sp_send_dbmail` | Quartz job + app-layer email (SMTP) — `sp_send_dbmail` has **no** PostgreSQL equivalent |
| PROD Daily AM Run (9 steps) | HL7/CDM/demographics reconciliation, XML shredding, HTML emails via `sp_send_dbmail` | Largest port: XML logic → `xmltable()`/app code; email → app SMTP; schedule → Quartz |
| nLog Table Purge | `DELETE FROM Logs WHERE CreatedOn < getdate()-7` | Trivial: Quartz job or `pg_cron` |

**Strategy:** because Quartz.NET already exists in-repo, the cleanest path is to **consolidate all SQL Agent jobs into the existing Quartz scheduler** (jobs call C#/repository code or PL/pgSQL), rather than adopt `pgAgent`/`pg_cron`. This also retires the `H:\sqlText\*.txt` Windows-path output coupling and the `sp_send_dbmail` dependency in one move.

### 2.4 Schema shape (relational vs. document fit)

~500 normalized, foreign-key-rich relational tables across 7 schemas, IDENTITY PKs, audit-table mirrors, and a small number of XML-typed columns used as message payload envelopes (`infce.messages_inbound.msgContent`). This is a **textbook relational OLTP + reporting** schema. There is **no document-oriented access pattern**: the app reads/writes wide normalized rows and joins heavily (account → pat → chrg → chrgdetail → dx → ins). The only "document" is the inbound HL7/XML blob, which is parsed *into* relational tables, not queried as a document. This shape is the decisive evidence that **MongoDB is out** (see §3.3).

---

## 3. DBMS Recommendation

### 3.1 Recommendation

**PostgreSQL** (target the current 17.x or 18.x stable line; PostgreSQL 17.10 / 18.4 are the current patch releases as of mid-2026 — https://www.postgresql.org/about/news/postgresql-184-1710-1614-1518-and-1423-released-3297/). One DBMS, no hedging.

### 3.2 Why PostgreSQL wins on each criterion

**Performance.** This is an OLTP + heavy-reporting workload (account assembly, claim batches, nightly aggregates). PostgreSQL's planner, CTE/window-function support, partial/expression indexes, and `xmltable()`-based XML shredding cover every hot path in this app. The reporting jobs that today run as multi-CTE T-SQL port to equivalent PostgreSQL CTEs with no semantic loss. No workload here (no FTS, no analytic column-store need, no document store) argues against Postgres.

**Stability & maturity.** PostgreSQL ships predictable quarterly minor releases across all supported majors (e.g. the coordinated 18.4/17.10/16.14/15.18/14.23 release — https://www.postgresql.org/about/news/postgresql-184-1710-1614-1518-and-1423-released-3297/), each rolling up security and bug fixes; 5-year major support windows. 25+ years of production use in regulated/financial settings. No single-vendor licensing risk.

**HIPAA suitability (summary; full matrix §4).** TLS in transit (native), filesystem/volume encryption-at-rest plus emerging native TDE via Percona `pg_tde` (https://percona.community/blog/2025/09/01/encrypting-postgresql-tables-with-pg_tde-step-by-step-guide-for-beginners/), `pgaudit` for native audit logging, mature **row-level security** (a feature SQL Server only matched recently and which this app does not yet use but could adopt for client/tenant isolation), and KMS/HSM integration via the managed offerings. Decisively: **every major managed provider signs a BAA for PostgreSQL** (§4).

**.NET driver & PetaPoco fit (the clincher).** Two independently strong facts:
- **PetaPoco.Compiled — the exact package already referenced — ships a `PostgreSQLDatabaseProvider` in its default provider set** (SQL Server, SQLite, MySQL, MariaDB, Firebird, **PostgreSQL**, Oracle) — https://github.com/CollaboratingPlatypus/PetaPoco/wiki/DB-Providers . No fork, no community adapter, no ORM swap required for the provider itself.
- **Npgsql** is the mature, first-class .NET PostgreSQL driver with full async/await, `NpgsqlDataSource` pooling, and OpenTelemetry metrics — exactly what Blazor Server wants (https://www.npgsql.org/doc/release-notes/8.0.html , https://npgsql.com/). The custom `OUTPUT INSERTED` workaround class is deleted, not ported, because Postgres uses native `RETURNING`.

**Ecosystem & support.** Largest open-source RDBMS ecosystem: extensions (`pgaudit`, `pg_tde`, `pg_cron`, PostGIS if ever needed), mature migration tooling (pgloader, AWS SCT/DMS — both explicitly support SQL Server→PostgreSQL schema + procedure conversion: https://docs.aws.amazon.com/dms/latest/userguide/schema-conversion-sql-server-postgresql.html), broad managed availability, and the deepest hiring pool of the open-source candidates.

**Migration cost from the current T-SQL surface.** Lowest *total* cost of the candidates because (a) the ORM provider is built-in, (b) the absent features (MERGE/ROWVERSION/TVP/JSON/FTS/SQLCLR/sequences) mean nothing hard needs re-engineering, and (c) PostgreSQL is the only candidate with credible automated T-SQL→procedural conversion (SCT/DMS "convert procedures to functions"). The XML-shredding procs remain a manual rewrite on *any* target, so they don't differentiate against Postgres.

### 3.3 Runner-ups and why they lost

| Candidate | Verdict | Specific reason grounded in this codebase |
|---|---|---|
| **MariaDB** | Runner-up | Best *governance* story (GPLv2, MariaDB Foundation, no Oracle re-licensing risk — https://mariadb.com/database-topics/mariadb-vs-mysql/) and PetaPoco supports it. But MariaDB's **XML handling is weak** — it has `ExtractValue`/`UpdateXML` with XPath subset, nothing equivalent to SQL Server's `.nodes()`/`CROSS APPLY` or Postgres `xmltable()`. With **~1,616 XML-method occurrences** shredding HL7 (`usp_prg_Xml_Import*`, the SQL Agent CDM jobs), MariaDB forces those into application code regardless. It loses to Postgres on procedural fidelity and on the absence of a strong managed-BAA story comparable to RDS-for-PostgreSQL. |
| **MySQL Community** | Rejected | Same XML weakness as MariaDB, **plus** Oracle's dual-license model reserves audit logging, thread pooling, PAM, and data masking for the paid Enterprise Edition (https://www.integrate.io/blog/mariadb-vs-mysql-everything-you-need-to-know/). For a HIPAA workload that needs native audit logging, "audit is an Enterprise-only feature" is disqualifying versus Postgres's free `pgaudit`. If choosing the MySQL family at all, MariaDB strictly dominates it here. |
| **MongoDB Community** | Rejected | The schema is ~500 normalized, FK-rich relational tables with heavy multi-table joins for account assembly (§2.4); there is **no document access pattern** — the only document (inbound HL7 XML) is parsed *into* relational tables, not stored/queried as documents. Re-modeling this into collections would be a ground-up rewrite of `AccountService`/repositories with no payoff. Licensing (SSPL) further complicates self-hosting, and while MongoDB **Atlas** does sign a BAA (https://www.mongodb.com/products/platform/trust/hipaa), that doesn't rescue a fundamental data-model mismatch. Out on architecture, not on HIPAA. |

(No additional candidate — e.g. SQLite, CockroachDB, YugabyteDB — is viable: SQLite is single-writer/embedded; the distributed-SQL options add operational cost with no requirement here driving them.)

### 3.4 T-SQL feature → PostgreSQL feature map

| SQL Server feature in use | Codebase locations | PostgreSQL equivalent | Effort signal |
|---|---|---|---|
| Stored procedures (~155) | `LabBillingDatabase/dbo/Stored Procedures/` | PL/pgSQL functions/procedures; SCT/DMS auto-converts many | **High (volume)** — port incrementally; runtime-critical set is tiny (see §6) |
| Scalar/Table UDFs (~115) incl. `GetMappingValue`, `DelimitedSplit8K`, `ArrayToTable` | `LabBillingDatabase/dbo/Functions/`; called inline at `ClientRepository.cs:85,97,117` | PL/pgSQL functions; `string_to_array`/`regexp_split_to_table` replace `DelimitedSplit8K`; `xmltable()` replaces `ArrayToTable` | **Medium** |
| XML methods `.value()`/`.nodes()`/`.query()`/`FOR XML PATH` (~1,616) | `usp_prg_Xml_Import*.sql`; SQL Agent steps (`SQL Agent Jobs.sql:543-622`) | `xpath()`, `xmltable()`, `xmlelement`/`xmlforest`; or move shredding into C# (j4jayant parser already in app) | **High** — the principal rewrite |
| Views (~209) | all schemas | CREATE VIEW (portable); fix schema-qualified names + bracket→double-quote | **Low–Medium** |
| CTEs / window functions (~72 each) | XML import procs; SQL Agent reports | Native, near-identical syntax | **Low** |
| Computed columns (`chrg.calc_amt`, `age_on_date_of_service`) | `chrg.sql:45-46` | `GENERATED ALWAYS AS … STORED` (deterministic) or view-computed | **Low–Medium** |
| IDENTITY (~91) | all key tables | `GENERATED … AS IDENTITY` (or `serial`) | **Low** (mechanical) |
| `OUTPUT INSERTED` (PK retrieval) | synthesized by `CustomSqlDatabaseProvider.cs` | native `RETURNING` (PetaPoco PG provider built-in) — **custom class deleted** | **Negative cost** (removal) |
| `OUTPUT INTO` in procs (~21) | XML import procs | `RETURNING … INTO` in PL/pgSQL | **Medium** |
| Multiple schemas (7) | `Security/*.sql` | PostgreSQL schemas (1:1) | **Low** |
| `TOP n` | `UserProfileRepository.cs:35` | `LIMIT n` | **Low** |
| Bracket identifiers `[x]` | pervasive in views/procs/`Sql.Builder` | double-quote `"x"` or unquote | **Low** (find/replace, but broad) |
| DDL audit trigger (`EVENTDATA()`) | `Database Triggers/Audit.sql` | event triggers + `pg_event_trigger_ddl_commands()` | **Medium** |
| `sp_send_dbmail` (~17) | `usp_account_payout.sql`, SQL Agent steps | **No DB equivalent** → move email to app SMTP (Quartz jobs) | **Medium** |
| Linked server `OPENQUERY` (4) | `…_Verify.sql` procs | `postgres_fdw`/`tds_fdw` or app-layer ETL | **Medium** |
| `GetNextNumber` proc + `EXEC … OUTPUT` | `NumberRepository.cs:28` | PL/pgSQL function with `OUT` param, or PostgreSQL `SEQUENCE` | **Low–Medium** |
| MERGE / ROWVERSION / TVP / JSON / FTS / SQLCLR / SEQUENCE / OFFSET-FETCH | **none in use** | n/a | **None** |

### 3.5 ORM strategy

**Keep PetaPoco.** No ORM replacement is required: `PetaPoco.Compiled` already supports PostgreSQL via its built-in `PostgreSQLDatabaseProvider` (https://github.com/CollaboratingPlatypus/PetaPoco/wiki/DB-Providers), the repository + UnitOfWork pattern is preserved, and the custom SQL-Server provider class is *deleted* (RETURNING is native). The work is: swap the provider in the two UoW construction sites (`UnitOfWorkMain.cs:98-108`, `UnitOfWorkSystem.cs:36-46`), swap `Microsoft.Data.SqlClient` for `Npgsql`, rewrite the ~40–50% of repositories that embed hand-written `Sql.Builder`/T-SQL strings (brackets→quotes, `TOP`→`LIMIT`, inline UDF/proc calls), and rebuild connection-string construction (`AppEnvironment.cs:64-104`) for Npgsql. *If* a future decision rejects PetaPoco for unrelated reasons, the two .NET 8 / repository-pattern / Blazor-friendly fits are **NPoco** (PetaPoco-compatible API, strong Postgres support — minimal porting) or **Dapper** (if moving to hand-written SQL + thin mapping); both are async-capable. Recommendation: **do not** take on an ORM swap during a DBMS migration.

### 3.6 SQL Server Agent replacement

**Consolidate into the existing Quartz.NET scheduler** (`Lab Patient Accounting Job Scheduler`). Each SQL Agent job (§2.3) becomes a Quartz job that calls repository/service code (or a PL/pgSQL function); `sp_send_dbmail` is replaced by app-layer SMTP. Avoid introducing `pgAgent`/`pg_cron` — Quartz already exists, keeps logic in version-controlled C#, and removes the Windows-path (`H:\sqlText`) and Database-Mail coupling. (`pg_cron` remains a fallback for trivial purge jobs like nLog if desired.)

### 3.7 Migration tooling shortlist

- **AWS SCT + DMS** — schema + procedure conversion (SQL Server→PostgreSQL/Aurora) with "convert procedures to functions" and ongoing CDC data sync; best fit if landing on RDS/Aurora PostgreSQL (https://docs.aws.amazon.com/dms/latest/userguide/schema-conversion-sql-server-postgresql.html , https://docs.aws.amazon.com/SchemaConversionTool/latest/userguide/CHAP_Source.SQLServer.ToPostgreSQL.html).
- **pgloader** — fast one-shot data + schema for the simpler tables/indexes/FKs; good for the bulk relational load, weak on procedural logic (https://dbconvert.com/blog/migrate-data-from-sql-server-to-postgresql/).
- **Babelfish for Aurora PostgreSQL** — a T-SQL/TDS compatibility layer (v5.3, Nov 2025) that *could* run T-SQL/`Microsoft.Data.SqlClient` against Postgres with reduced rewrite. **Treat as a transitional crutch, not the target**: documented gaps include unsupported `sp_configure`, stored-proc ownership-chain differences, no SCRAM, and unsupported extensions (https://docs.aws.amazon.com/AmazonRDS/latest/AuroraUserGuide/babelfish-compatibility.tsql.limitations.html). Useful to de-risk a phased cutover; not a reason to keep T-SQL forever.

---

## 4. HIPAA Feature Comparison

Comparison across the finalists (SQL Server today; recommended PostgreSQL; runner-up MariaDB). This is a feature comparison, not a §164.312 controls map.

| Criterion | SQL Server (today) | **PostgreSQL (recommended)** | MariaDB (runner-up) |
|---|---|---|---|
| **Encryption at rest** | Native TDE (Enterprise/all editions in recent versions) | Volume/filesystem (LUKS/BitLocker) + emerging native TDE via Percona `pg_tde`; managed offerings provide transparent at-rest encryption (https://percona.community/blog/2025/09/01/encrypting-postgresql-tables-with-pg_tde-step-by-step-guide-for-beginners/) | Native data-at-rest encryption included in community GPLv2 build (a notable MariaDB advantage over MySQL CE) |
| **TLS in transit** | Supported — **but disabled in this app** (`Encrypt=false;TrustServerCertificate=true`, `AppEnvironment.cs`) — fix on migration | Native SSL/TLS; Npgsql supports it | Native SSL/TLS |
| **Audit logging** | Native SQL Server Audit | `pgaudit` extension (free, mature) (https://www.enterprisedb.com/postgresql-best-practices-encryption-monitoring) | Native audit plugin in community build (Enterprise-only in MySQL CE) |
| **RBAC granularity** | Roles, schema/object perms (already modeled — `LabBillingDatabase/Security/*.sql`) | Roles, schema/object/column privileges (direct 1:1 mapping of existing roles) | Roles + privileges |
| **Row-level security** | Native (security policies) | Native RLS — mature; available for future client/tenant isolation | **Not** a native equivalent (must be enforced in app/views) |
| **Key management (KMS/HSM)** | EKM / Azure Key Vault | Managed: AWS KMS, Azure Key Vault, GCP KMS via the managed services; self-host via `pg_tde` keyrings | Managed-provider KMS; key-management plugin |
| **BAA availability (managed)** | Azure SQL / RDS for SQL Server | **AWS RDS & Aurora for PostgreSQL (HIPAA-eligible under AWS BAA), Azure Database for PostgreSQL, GCP Cloud SQL, Crunchy Bridge, Aiven** (https://aws.amazon.com/about-aws/whats-new/2017/12/amazon-aurora-with-postgresql-compatibility-achieves-hipaa-eligibility/ , https://aws.amazon.com/rds/features/security/) | AWS RDS for MariaDB is HIPAA-eligible under the AWS BAA; Azure offers MariaDB-compatible/MySQL services (verify SKU) |

**BAA bottom line:** PostgreSQL has the broadest signed-BAA managed footprint of the candidates — all of AWS, Azure, GCP plus Postgres-specialist managed vendors (Crunchy Bridge, Aiven) — so a HIPAA-compliant hosted deployment is available without self-hosting. MariaDB has a narrower but real managed-BAA path (notably AWS RDS). MongoDB's only BAA path is Atlas. **Note:** the current app's `Encrypt=false` connection setting is a HIPAA in-transit gap that must be closed during migration regardless of engine.

---

## 5. Redis Hotspot Survey

Hotspot identification only — no key shapes, TTLs, or invalidation design.

### 5.1 High-value hotspots

| Hotspot | Location in code | Why cacheable | Expected impact |
|---|---|---|---|
| **Reference dictionaries** (CPT/AMA, diagnosis, CDM, revenue, fin codes) | `LabBilling Library/Services/DictionaryService.cs` — `GetCptAmaDescription:22`, `GetCdm:49`, `GetDiagnosis:353`, `GetRevenueCode:338`, `GetFinCode:278` | Code tables that change ~annually (AMA year) but are read on nearly every account load, charge, and claim line; no cache today | **High** — eliminates the bulk of repeated point lookups; e.g. `GetPatByAccount` issues up to 9 `GetDiagnosis` calls per account (`AccountService.cs:204-258`) |
| **Client / insurance-company / mapping lookups** | `DictionaryService.GetInsCompany:107`, `GetAllClients:161`, `GetClient:197`; `InsCompanyRepository.GetByCode:19`; `MappingRepository.GetMappings:43`, `GetMappingsBySendingValue:58` | Small, stable tables hit per HL7 message for client/insurer/`CLIENT→CERNER` validation | **Medium–High** — HL7 ingestion repeatedly validates the same ~10–20 clients/insurers; mirrors the existing WinForms `DataCache` (`Lab PA WinForms UI/DataCache.cs`) which already proves these are cache-worthy |
| **Reference data *inside* account assembly** | `AccountService.GetAccount:364` fan-out → per-charge `GetCdm`, `RevenueCode`, `CptAmaRepository` lookups (`AccountService.cs:1236-1251`), plus the 9 diagnosis lookups | The reference lookups inside the fan-out are repeated across accounts and rarely change | **High** — caching the *reference* portion collapses much of the ~25–40-query-per-account cost; the account row itself stays uncached (see §5.3) |
| **Auth/identity lookup** | `LabOutreachUI/Authentication/CustomAuthenticationStateProvider.cs:131,287` → `UserAccountRepository.GetByUsername`; today process-local only in `LabOutreachUI/Services/UserCircuitHandler.cs:19` | User/role/policy data changes infrequently; queried per auth-state check | **Medium** — biggest payoff is *correctness under scale-out* (shared across nodes), not just latency (see §5.2) |

### 5.2 Shared-state caches across web + service + scheduler

These are about *shared state*, not data-cache latency — they matter because LabOutreachUI (web), LabBillingService (Topshelf), and the Quartz scheduler are separate processes/hosts.

- **Distributed user/session state.** `UserCircuitHandler` caches user info in a process-local `ConcurrentDictionary` (`LabOutreachUI/Services/UserCircuitHandler.cs:19`) and `CustomAuthenticationStateProvider` re-queries per auth check. A shared cache (Redis) is the natural home if Blazor Server is ever load-balanced across nodes, enabling session/role continuity and failover. **Shared-state candidate, high relevance to the Blazor scale-out goal.**
- **HL7 idempotency / dedupe tokens.** `HL7ProcessorService.ProcessMessage:105` / `GetUnprocessedMessages:169` and `MessagesInboundRepository.GetUnprocessedMessages:34` show **no explicit duplicate-control-ID check** before processing — a resubmitted CERNER message can be processed twice. A shared "recently-processed control-ID" set in Redis is a clean cross-process idempotency guard. **Shared-state candidate, medium relevance.**
- **Distributed locks for cross-process collisions.**
  - *Account locks*: `AccountService.GetAccountLock:283` + `AccountLockRepository.GetLock:16` are DB-backed pessimistic locks; correctness currently rests on DB serialization across web/service/scheduler. A Redis distributed lock would make cross-host coordination explicit. **Medium (only if scaling horizontally).**
  - *Number/batch generation*: `NumberRepository.GetNumber:18` (`GetNextNumber` proc) is the allocation point for batch/account numbers; under concurrent web+service+scheduler calls a Redis `INCR`/lock (or a Postgres `SEQUENCE`) is safer than relying on proc-internal locking. **Medium.** (Note: this is a *coordination* primitive, not a data cache.)

### 5.3 Tempting-but-bad candidates

| Candidate | Location | Why it's a bad Redis target |
|---|---|---|
| **Account financial balance** | `AccountService.GetBalance:477` (live SUM of charges − payments) | Mutates on every charge/payment/write-off; billing accuracy and compliance demand the latest value. Cache invalidation cost outweighs benefit. **Do not cache.** |
| **The account aggregate itself** (full `GetAccount` result) | `AccountService.GetAccount:364` | Contains live financial state and PHI; freshness-sensitive and frequently written (insurance swap, charge moves — `AccountService.cs:845,1266`). Cache the *reference data inside* it (§5.1), not the assembled account. |
| **Transactional write paths** | `InsuranceSwap:845`, charge move/reprocess `:1266-1301` | Multi-step transactional updates — caching adds invalidation/coordination complexity with no read-latency win. |
| **Raw PHI demographics as a convenience cache** | patient/demographic records | Cacheable in principle but raises HIPAA surface (encryption-at-rest/in-transit for the cache, access logging). Not worth it versus caching non-PHI reference codes; if ever done, scope tightly and treat the cache as PHI. |

---

## 6. Open Questions & Recommended Spikes

- **Q: What is the *runtime-critical* DB-object surface (procs/UDFs/views the live app actually calls), vs. the batch/reporting surface?**
  Why it matters: determines whether the migration's blocking path is ~5 objects (the 3 scalar UDFs at `ClientRepository.cs:85,97,117`, `usp_cerner_chrg_reprocess`, `GetNextNumber`) plus the views PetaPoco auto-selects against — in which case ~150 procs/UDFs are portable *after* cutover — or whether hidden indirect calls exist.
  Smallest experiment: enable SQL Server Extended Events / a server-side trace in a representative test environment, exercise the full Blazor + service + scheduler workflow for a day, and diff the captured `object_id` set against the static catalog. A short list confirms a thin runtime surface.

- **Q: Will the XML-shredding logic be ported to PostgreSQL `xmltable()` or moved into C# (the j4jayant parser already parses HL7 in-app)?**
  Why it matters: it is the single largest rewrite (~1,616 XML-method occurrences) and the choice drives effort sizing and where HL7 logic lives long-term.
  Smallest experiment: take one representative proc (`usp_prg_Xml_Import.sql`) plus the SQL Agent "CERNER DAILY CDM" step and re-implement it two ways — once as `xmltable()` PL/pgSQL, once as a C# transform over the existing parser — on a sample message set; compare output parity and effort.

- **Q: Does `Microsoft.Data.SqlClient`'s `MultipleActiveResultSets=true` (set in `AppEnvironment.cs`) hide places where code holds an open reader while issuing nested queries?**
  Why it matters: Npgsql has no MARS; any MARS-dependent code path will break and must be refactored to fetch-then-iterate or use separate connections.
  Smallest experiment: grep repositories/services for nested `Query`/`Fetch` inside an active enumeration; or run the test suite against a connection string with MARS disabled and catalog the failures.

- **Q: Can the target managed PostgreSQL offering meet the BAA + at-rest + in-transit + audit requirements in *one* SKU, and does it support the chosen at-rest mechanism (provider-managed vs. `pg_tde`)?**
  Why it matters: §4 shows broad BAA availability, but SKU/feature-level eligibility and KMS integration vary; this gates the security review.
  Smallest experiment: stand up a single managed PostgreSQL instance on the leading candidate (e.g. AWS RDS for PostgreSQL), execute the BAA, enable TLS-required + `pgaudit` + provider at-rest encryption + KMS, and confirm Npgsql connects with `SSL Mode=Require`.

- **Q: How many repositories embed bracket-identifier / `TOP` / inline-UDF T-SQL that needs hand-porting, precisely?**
  Why it matters: this is the bulk of the .NET-side effort (§2.2 estimates 40–50%); an exact count sizes the rewrite.
  Smallest experiment: a scripted scan over `LabBilling Library/Repositories/**/*.cs` for `Sql.Builder` usages containing `[`, ` TOP `, `dbo.`, `OUTPUT`, `EXEC`, and tally per file — yields a concrete porting backlog.

---

*Prepared as planning input. Codebase citations are `path:line` against branch `billing-ui-to-blazor`. External claims cite current vendor/driver sources inline; verify version-sensitive items at execution time.*
