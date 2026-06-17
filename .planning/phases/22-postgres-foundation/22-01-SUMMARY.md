# Phase 22 Plan 01: PostgreSQL + Canonical Schema Source Summary

**PG17 up on 5434; `LabBillingCanonical` loaded with 270 tables from the `LabBillingDatabase` DB-project table DDL (not the drifted root build script).**

## Accomplishments

- **PostgreSQL 17 container** — added a `postgres:17` service to `docker-compose.yml`
  (`container_name: labpa_postgres`, restart `unless-stopped`, named volume `pgdata`,
  host port **5434**→5432 to avoid the other project's 5433). Env: `POSTGRES_DB=labbilling`,
  `POSTGRES_USER=labpa`, `POSTGRES_PASSWORD` (local dev only). Verified running and reachable —
  `select version()` returns **PostgreSQL 17.10**. The empty `labbilling` database exists and is the
  pgloader target for 22-02. The existing `mssql` service is unchanged and still starts.

- **Canonical SQL Server schema source** — created a fresh `LabBillingCanonical` DB on the existing
  `mssql_server` container and loaded the canonical **table** DDL. Final catalog: **270 tables across
  11 schemas** (`dbo`, `audit`, `dict`, `dictionary`, `infce`, `tst`, `zzz`, plus `app` + system),
  materially larger than `LabBillingTest`'s 163. The `dbo.chrg` table is present.

- **Load path used: DB-project table DDL (the plan's documented fallback).** See decisions below for why
  the 4.3 MB root build script was rejected as the source.

## Files Created/Modified

- `docker-compose.yml` — added the `postgres` service and the `pgdata` named volume.
- `.planning/phases/22-postgres-foundation/22-01-SUMMARY.md` — this summary.
- `.planning/ROADMAP.md` — Phase 22 progress row `Not Started | 0/5` → `In Progress | 1/5`.

(No application/source files changed. `LabBillingCanonical` and `labpa_postgres` are container state, not repo files.)

## Decisions Made / Issues Encountered

- **Root build script is itself a drifted snapshot (risk R3) — rejected as the source.** I first loaded
  `LabPatientAccounting DB Build.sql` (4.3 MB) into `LabBillingCanonical` (redirecting its single
  `USE [LabBillingTest]` to `USE [LabBillingCanonical]`). It produced only **163 tables** — identical to
  `LabBillingTest` — because the script is the same older 2023 snapshot (`Script Date 5/14/2023`), not the
  production catalog. Its 209 errors were all on views/procs (collation/missing-object), zero on
  `CREATE TABLE`. This confirms FINDINGS R3: the root build script must NOT be the translation source.

- **Switched to the `LabBillingDatabase` SQL Server DB project as the canonical source** (the plan's
  documented fallback). Dropped and recreated `LabBillingCanonical`, then concatenated all **270 per-table
  `.sql` files** (under `*/Tables/`) preceded by `CREATE SCHEMA` for the non-`dbo` schemas. This is the
  authoritative, version-controlled table catalog.

- **Load result: 269/270 tables on the first pass, then 270 after one targeted fix.** Non-fatal errors:
  - 27× `Msg 15135` (extended-property `MS_Description` calls) — benign, ignored.
  - Several `Msg 8197/1088` on `CREATE TRIGGER ... dbo.chrg` blocks embedded in table files — benign for
    Phase 22 (triggers are out of scope; only the TABLE structure is needed for the pgloader source).
  - `Msg 4121`: `dbo.chrg` failed to create because its computed column
    `[calc_amt] AS ([dbo].[GetAmountTotal]([chrg_num])*[qty])` references a UDF that wasn't present.
    **Fix (auto-applied, documented):** created a stub `dbo.GetAmountTotal(@chrg_num) RETURNS MONEY`,
    then created `chrg` (table + indexes, excluding its trigger blocks). `chrg` now exists; final count **270**.
    The real UDF/computed-column logic is deferred — on the PG side `calc_amt` becomes a
    `GENERATED ALWAYS AS ... STORED` column in 22-02 (per FINDINGS sizing), so the stub is throwaway.

- **`LabBillingTest` was not altered** — confirmed still 163 tables after all work. All new state is
  additive (new PG container + new `LabBillingCanonical` DB).

- **~500 vs 270:** FINDINGS' "~500 tables (production)" appears to count tables + ~209 views + synonyms.
  The DB project's authoritative **table** catalog is 270; views/procs/synonyms are deferred to later
  phases (23). 270 satisfies the plan's "materially larger than 163 / target ~500 range" intent as the
  best version-controlled source available in-repo.

## Next Step

Ready for 22-02-PLAN.md (table-schema translation): run pgloader from `LabBillingCanonical`
(270 tables on `mssql_server`) into the empty `labbilling` database on `labpa_postgres` (port 5434),
applying IDENTITY → `GENERATED ... AS IDENTITY`, computed columns → `GENERATED ALWAYS AS ... STORED`
(incl. `chrg.calc_amt`), and bracket → double-quote fix-ups.
