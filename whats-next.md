<original_task>
Overarching thread (v1.1 "Platform Modernization"): migrate the Lab Patient Accounting app from
Microsoft SQL Server to PostgreSQL and introduce Redis caching, driven by the analysis in
`analyses/dbms-modernization-and-redis-hotspots.md`. The work is planned as roadmap phases 21–26 in
`.planning/ROADMAP.md` under a new milestone (separate from the in-flight v1.0 WinForms→Blazor UI work).

This session's concrete asks, in order:
1. Run prompt 001 → produce the DBMS-modernization + Redis-hotspot analysis.
2. Create a plan (`/create-plans`) to migrate to PostgreSQL + implement Redis → roadmap milestone v1.1 (phases 21–26) + detail-plan Phase 21.
3. Build the solution and fix build errors.
4. Remove the legacy WinForms UI project from the solution (prompt 002).
5. Resolve .NET 10 retarget build errors (prompt 003).
6. Generate a minimal DB seed script (prompt 004).
7. Set up + verify the dockerized SQL Server connection; verify end-to-end app login.
8. Execute Phase 21 (de-risking spikes, plans 21-01…21-04) and Phase 22 (PostgreSQL foundation, plans 22-01…22-05).
9. Detail-plan Phase 22.

Most recent explicit state: **Phase 22 is COMPLETE.** The natural next step (offered, not yet started) is to detail-plan Phase 23.
</original_task>

<work_completed>
PLANNING ARTIFACTS (.planning/):
- `analyses/dbms-modernization-and-redis-hotspots.md` — recommends PostgreSQL; top Redis hotspots; full §1–6.
- `.planning/ROADMAP.md` — v1.1 milestone added: Phase 21 (De-Risking Spikes, COMPLETE 4/4), Phase 22 (Postgres Foundation, COMPLETE 5/5), Phase 23 (In-Database Logic & Repository Port, NOT STARTED), Phase 24 (Jobs→Quartz), Phase 25 (Data Cutover), Phase 26 (Redis Caching). v1.0 UI phases 01–20 untouched (Phase 01 In Progress).
- Phase 21 spikes — all under `.planning/phases/21-derisking-spikes/`:
  - `FINDINGS.md` (consolidated; the Phase 22–25 sizing hand-off)
  - `21-findings-repo-tsql-backlog.md`, `21-findings-mars-audit.md`, `21-findings-runtime-surface.md`, `21-findings-xml-shredding.md`, `21-findings-hosting.md`
  - `spikes/` — XE trace scripts (`xe-runtime-surface-session.sql`, `xe-analyze-captured-objects.sql`), XML bake-off prototypes (`shred-prototype-pg.sql`, `ShredPrototypeCSharp.cs`, `xml-case-source-tsql.sql`, `sample-message.xml`)
  - SUMMARYs 21-01…21-04
- Phase 22 plans + summaries — `.planning/phases/22-postgres-foundation/` 22-01…22-05 PLAN + SUMMARY.
- Prompts archived under `prompts/completed/`: 001 (analysis), 002 (remove WinForms), 003 (.NET 10 build), 004 (seed).

CODE / SCHEMA / INFRA CHANGES (committed):
- `18aad2f` — removed 4 WinForms projects from `Lab Billing.sln` (Lab PA WinForms UI, LabBilling Winforms Library, WinFormsLibrary, DataGridViewGrouper). Solution-file-only; project folders remain on disk.
- `8b40b7f` — .NET 10 retarget build fixes: renamed C# 14 `field` contextual-keyword locals in `j4jayant.HL7.Parser/{Segment,Field}.cs`; reverted SixLabors Fonts 3.0.0→2.1.3 and ImageSharp 4.0.0→3.1.11 (3.x/4.x enforce commercial license, fail build); added `Microsoft.NET.Test.Sdk 18.6.0` to LabBillingCore.UnitTests.
- `184f5a3` — `Database/Seed/seed-min-data.sql` (SQL Server seed: 188 system params + emp users; idempotent).
- `3f89cb7` (22-01) — added `postgres:17` service (`labpa_postgres`, host port 5434) to `docker-compose.yml`; loaded canonical schema into SQL Server DB `LabBillingCanonical` (270 tables) from the LabBillingDatabase DB-project DDL.
- `5dfcabc` (22-02) — `Database/Postgres/{pgloader.load, schema-tables.sql, schema-fixups.sql}` — pgloader translation of 270 tables to PostgreSQL; verified parity 270/270 tables, 3959/3959 columns.
- `7d161c5` (22-03) — repointed data-access core: added `Npgsql 10.0.3`, deleted `CustomSqlDatabaseProvider.cs`, switched 3 provider sites (`UnitOfWorkMain.cs`, `UnitOfWorkSystem.cs`, `AuthenticationService.cs:31`) to `PetaPoco.Providers.PostgreSQLDatabaseProvider`, rebuilt `AppEnvironment.cs` to `NpgsqlConnectionStringBuilder` (removed MARS; added configurable `SslMode` default Prefer + `RootCertificate`; host/port parsed from `ServerName`).
- `453a1bb` (22-04) — removed `Microsoft.Data.SqlClient` from data layer (58 files): SqlParameter/SqlDbType→PetaPoco-native or Npgsql; ReportingRepository raw ADO.NET→Npgsql; package removed from LabBilling Core; Utilities/Console/Service/Scheduler made SqlClient-free. SQL string contents deliberately UNCHANGED (Phase 23 boundary).
- `6401301` (22-05) — `Database/Seed/seed-min-data.pg.sql` (PostgreSQL seed); folded reproducibility fixes into committed files: `schema-fixups.sql §5` (added `dbo.emp.access_random_drug_screen`, `dbo."system"."KeyName"`, `ALTER DATABASE labbilling SET search_path=...`); `docker-compose.yml` POSTGRES_PASSWORD=labpa_dev.

VERIFICATION ACHIEVED:
- Solution builds GREEN on .NET 10 via VS MSBuild (0 errors) WITHOUT Microsoft.Data.SqlClient in the data layer.
- `LabOutreachUI` boots against PostgreSQL, `GET /` = 200, login `bpowers` authorized (ENTER/EDIT, admin), auto-CRUD INSERT→RETURNING→SELECT→UPDATE→DELETE round-trip passes through PetaPoco's PG provider.

MEMORY FILES written (C:\Users\brads\.claude\projects\C--Users-brads-source-repos-Lab-Patient-Accounting\memory\): `build-requires-vs-msbuild.md`, `local-dev-sqlserver-setup.md` (+ MEMORY.md index).
</work_completed>

<work_remaining>
IMMEDIATE NEXT STEP: detail-plan **Phase 23 (In-Database Logic & Repository Port)** via `/create-plans` (route: plan-phase), sized from `.planning/phases/21-derisking-spikes/FINDINGS.md` and the Phase 22-05 hand-off. Roadmap-sketched plans: 23-01 (runtime scalar UDFs + GetNextNumber), 23-02 (XML-shredding→C#/j4jayant + cerner import procs), 23-03 (repository T-SQL rewrite), 23-04 (view definitions port). 

Phase 23 must address the catalogued runtime-failure sites (these THROW today against PostgreSQL — expected, SQL dialect not yet ported):
1. **Identifier quoting — DOMINANT class, highest leverage.** `RepositoryCoreBase.GetRealColumn` (~lines 76-100) emits UNQUOTED mixed-case column names in hand-built WHEREs → PostgreSQL folds to lowercase → fails vs the case-preserved/quoted schema (fired 188× at boot via `SystemParametersRepository.cs:47,65`, caught/defaulted). Fix once (quote identifiers in GetRealColumn / Sql.Builder) to unblock the most repos. ADD THIS TO 23-03 SCOPE as the top item.
2. `UserProfileRepository.cs:34` — `TOP` → `LIMIT`.
3. `ClientRepository.cs:84,96` — inline UDFs `dbo.GetAccBalance`/`GetAccBalByDate` (+ `GetAccClientBalance` ~:117).
4. `NumberRepository.cs:28-29` — `;EXEC GetNextNumber @0,@1 OUTPUT` (T-SQL proc + OUTPUT param).
5. `ReportingRepository.cs:23,45` — `DATEADD`/`DATEDIFF`/`GetDate()` + quoted aliases (raw Npgsql ADO.NET now).
6. `PatientBillingService.cs:745,749`, `MessagesInboundRepository.cs:89-90` — `ExecuteNonQueryProc("usp_prg_pat_bill_*"/"usp_cerner_chrg_reprocess")`.

Phase 23 procedural ports (runtime-critical set from 21-02 — only ~7 objects): port `GetAccBalance`/`GetAccBalByDate`/`GetAccClientBalance` + `GetNextNumber` to PL/pgSQL or app code; implement HL7 XML shredding in C#/j4jayant (DECIDED in 21-03 — do NOT port the ~1,616 XML methods to xmltable()); port the 2 patient-billing procs + cerner reprocess. Compute the dependency closure (`sys.sql_expression_dependencies`) of the 7 entry points first. Also restore the 2 deferred computed columns (`chrg.calc_amt`, `chrg.age_on_date_of_service`) once `GetAmountTotal` is ported.

THEN: Phase 24 (SQL Agent jobs→Quartz, sp_send_dbmail→SMTP), Phase 25 (data cutover to PostgreSQL; hosting target AWS RDS PG 17 — VALIDATE THE SKU first, open risk R1), Phase 26 (Redis caching of the §5 hotspots).

DEFERRED FROM PHASE 22 (carry into Phase 23): views (~209 — 23-04), procs/functions, triggers; broader model↔schema drift reconciliation (R3); minor numeric typemod precision losses in the translated schema (e.g. `chrg.qty` became bare `numeric`).
</work_remaining>

<attempted_approaches>
- Canonical schema source: the 4.3MB `LapPatientAccounting DB Build.sql` is ITSELF a drifted 2023 snapshot (only 163 tables, same as LabBillingTest) — DEAD END as a translation source. The authoritative source is the version-controlled `LabBillingDatabase` DB-project per-table .sql files (270 tables). Use those.
- `dotnet build "Lab Billing.sln"` does NOT work (despite CLAUDE.md documenting it): `Utilities.csproj` has a `VBIDE` COM reference → MSB4803 on .NET Core MSBuild. MUST use VS MSBuild: `C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe` with `/p:Platform=x64`.
- pgloader `--network host` does NOT bridge sibling containers on Docker Desktop/Windows → use `host.docker.internal:<port>` hostnames; needed `TDS_MAX_CONN=512`.
- 22-03 expected a red build after the provider swap but it stayed GREEN (SqlClient still referenced) — so 22-04's real forcing function was REMOVING the package (not chasing compile errors). Removing it surfaced one hidden site (`InsCoverage.cs` dead `using`).
- Empty `POSTGRES_PASSWORD` breaks Npgsql SCRAM auth ("No password has been provided") — must set a non-empty password (now `labpa_dev`).
- Initial seed user guess (`brads`) was WRONG — the dev identity is `WTHMC\bpowers` (from tracked `appsettings.Development.json` DevelopmentUser), which `UserAccountRepository.GetByUsername` resolves to `bpowers` (domain stripped). Seed must include `bpowers`.
- `access_random_drug_screen` column: mapped by `UserAccount` model but absent from both LabBillingTest and the canonical/translated schema → SqlException 207 at login until added (now in schema-fixups §5).
</attempted_approaches>

<critical_context>
BUILD: VS MSBuild only (see above), `/p:Platform=x64`. `dotnet build`/`dotnet run` fail on the Utilities COM ref. Build output for LabOutreachUI landed in BOTH `bin/Debug/net10.0` and `bin/x64/Debug/net10.0` across runs — verify which is current before running. To run the app: copy current `appsettings.json` into the output dir (build copy goes stale), `ASPNETCORE_ENVIRONMENT=Development`, `dotnet LabOutreachUI.dll`; listens https://localhost:7063 (curl -k). Stop via Stop-Process on the port-7063 owner.

CONTAINERS (running): `mssql_server` (SQL Server 2022, localhost:1433, SA `YourStrong!Passw0rd`; DBs: LabBillingTest [drifted 163-table dev snapshot, sysapp login], LabBillingCanonical [270-table translation source]). `labpa_postgres` (PostgreSQL 17, localhost:5434, db `labbilling`, user `labpa`/`labpa_dev`). Run sqlcmd in-container via PowerShell tool (Git Bash mangles `/opt/...`): `docker exec mssql_server /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'YourStrong!Passw0rd' -C -Q "..."`. psql: `docker exec labpa_postgres psql -U labpa -d labbilling -c "..."`.

PostgreSQL schema specifics: 270 tables across 7 schemas (dbo 224, audit 25, dictionary 7, infce 5, dict 4, zzz 4, tst 1 — tst/zzz are scratch). **Identifier policy: original casing PRESERVED, all identifiers QUOTED** (e.g. `dbo."chrg"`, `"AccountSearchView"`). The .NET layer must quote mixed-case identifiers to match (the GetRealColumn gap is the consequence). Apply order: `schema-tables.sql` then `schema-fixups.sql` (both ON_ERROR_STOP, 0 errors from scratch). Numeric IDENTITY cols (27) use owned bigint sequences (not GENERATED AS IDENTITY); int identities (53) use GENERATED BY DEFAULT AS IDENTITY. `search_path` set at DB level (could move to Npgsql connection string in Phase 23).

KEY DECISIONS (locked in FINDINGS): DBMS=PostgreSQL; hosting=AWS RDS PG 17 (fallback Azure Flexible Server; validation DEFERRED = risk R1); HL7 XML shredding→C#/j4jayant (NOT xmltable()); MARS NOT load-bearing (removed, 0 refactors); KEEP PetaPoco (built-in PG provider). Repo T-SQL backlog: only ~10/67 files (~15%), not the analysis's 40-50% estimate. Runtime-critical procedural surface: only ~7 objects + ~10 views (rest deferrable).

HIPAA/SECURITY: synthetic/dev data only; the SQL Server `Encrypt=false` gap is replaced by configurable Npgsql `SslMode` (dev default Prefer; prod VerifyFull + RootCertificate). Seed password is `Password1` (synthetic).

GIT HYGIENE: NEVER commit `LabOutreachUI/appsettings.json` (tracked file now carrying dev DB creds — keep local) or `CLAUDE.md` (unrelated pre-existing change). Both are intentionally left as working-tree `M`. Commits stage only specific files, never `git add .`. Co-author trailer: `Co-Authored-By: Claude Opus 4.8 (1M context) <noreply@anthropic.com>`.

WORKFLOW: phases executed via `/run-plan <phase>/<plan>-PLAN.md`. Autonomous plans (no checkpoints) → single subagent (Strategy A). human-verify checkpoints → Strategy B (subagent for auto block, then user-verify in main). decision/human-action → Strategy C (main context). Plan files ARE the prompts. `/create-plans` for planning new phases.
</critical_context>

<current_state>
COMPLETE: Phase 21 (4/4) and Phase 22 (5/5). The app demonstrably runs on PostgreSQL for connect + login + auto-CRUD paths. All work committed through `6401301` on branch `billing-ui-to-blazor`.

WORKING TREE: only `CLAUDE.md` (M) and `LabOutreachUI/appsettings.json` (M, points at PostgreSQL with dev creds) — both intentionally uncommitted. No other pending changes. Containers `mssql_server` and `labpa_postgres` are up.

NOT STARTED: Phase 23 (detail-planning not begun — roadmap has only the indicative plan sketch), Phases 24–26.

OPEN RISKS carried forward (from FINDINGS + Phase 22): R1 managed-PostgreSQL HIPAA SKU validation deferred (gates Phase 25 + security review); R2 runtime surface is static-only confidence (XE scripts ready to upgrade); R3 model↔schema drift (broader than the 2 columns already fixed); identifier-quoting class (the dominant Phase-23 fix); deferred views/procs/functions/triggers; 2 deferred computed columns.

OPEN QUESTION / PENDING DECISION: whether to proceed straight to detail-planning Phase 23 (offered at session end, user invoked /whats-next instead). The recommended Phase 23 sequencing puts the identifier-quoting fix first in 23-03 as the highest-leverage unblocker. No decision has been made to deviate from the roadmap's 23-01…23-04 sketch.
</current_state>
