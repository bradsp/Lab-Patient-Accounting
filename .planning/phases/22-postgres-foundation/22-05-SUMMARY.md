# Phase 22 Plan 05: Smoke Verification Summary — PHASE 22 COMPLETE

**LabOutreachUI runs on PostgreSQL: connects, logs in (`bpowers`, ENTER/EDIT, admin), and auto-CRUD round-trips with `RETURNING`. The foundation is real and reproducible; remaining failures are isolated to documented Phase-23 T-SQL/identifier-quoting sites.**

## Accomplishments
- **PG seed** (`Database/Seed/seed-min-data.pg.sql`, idempotent): 188 `dbo."system"` params + 2 `dbo.emp` users (`bpowers`, `admin`; ENTER/EDIT, admin, RDS, SHA256 password via pgcrypto).
- **App boots against PostgreSQL** — no connection/SCRAM errors; `GET /` = **200**; login authorized via a real `emp` SELECT through PetaPoco's `PostgreSQLDatabaseProvider`.
- **Auto-CRUD verified** — INSERT (RETURNING key) → SELECT → UPDATE → DELETE round-trip green through the PG provider.
- **Reproducibility fixes folded into committed artifacts** (per user decision): missing model-mapped columns + `search_path` added to `schema-fixups.sql`; `POSTGRES_PASSWORD` set in `docker-compose.yml`.

## Files Created/Modified
- `Database/Seed/seed-min-data.pg.sql` (new) — PostgreSQL seed
- `Database/Postgres/schema-fixups.sql` — §5 added: `dbo.emp.access_random_drug_screen`, `dbo."system"."KeyName"`, `ALTER DATABASE … search_path`
- `docker-compose.yml` — `POSTGRES_PASSWORD: labpa_dev`
- `.planning/ROADMAP.md` — Phase 22 → Complete (5/5)
- `LabOutreachUI/appsettings.json` — pointed at PG (LOCAL/uncommitted, carries dev creds)

## Decisions Made
- Folded the 22-05 ad-hoc DB fixes into committed files so the foundation rebuilds from source.
- `appsettings.json` deliberately left uncommitted (tracked file carrying dev secrets) — keep local.

## Issues Encountered (deviations, auto-fixed)
- **search_path** required (models use unqualified names; 7 schemas) → `ALTER DATABASE` (folded into schema-fixups; Phase 23 may move it to the Npgsql connection string).
- **Model↔schema drift** (R3): canonical schema lacked `emp.access_random_drug_screen` and `system."KeyName"` → added in schema-fixups. Phase 23 should reconcile the broader drift.
- **Empty PG password** broke Npgsql SCRAM → role password `labpa_dev` set + docker-compose updated.

## Phase 23 hand-off — runtime failures still present (expected; SQL dialect not yet ported)
1. **Identifier quoting (dominant class):** `RepositoryCoreBase.GetRealColumn` emits unquoted mixed-case columns in hand-built WHEREs → PG lowercases → fails (fired 188× at boot via `SystemParametersRepository.cs:47,65`, caught/defaulted). Every hand-built WHERE on a mixed-case column needs quoting. Direct consequence of the 22-02 "preserve casing + quote-all" schema choice.
2. `UserProfileRepository.cs:34` — `TOP` → `LIMIT`.
3. `ClientRepository.cs:84,96` — inline UDFs `dbo.GetAccBalance`/`GetAccBalByDate`.
4. `NumberRepository.cs:29` — `;EXEC GetNextNumber @0,@1 OUTPUT`.
5. `ReportingRepository.cs:23,45` — `DATEADD`/`DATEDIFF`/`GetDate()`.
6. `PatientBillingService.cs:745,749`, `MessagesInboundRepository.cs:89` — `ExecuteNonQueryProc("usp_prg_*"/"usp_cerner_*")`.

## Next Step
**Phase 22 complete.** Ready to detail-plan Phase 23 (In-Database Logic & Repository Port) — the identifier-quoting fix (#1) is the highest-leverage item to add to 23-03's scope.
