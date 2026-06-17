# Phase 22 Plan 03: Provider + Connection Swap to Npgsql Summary

**Data-access core repointed to PostgreSQL: PetaPoco now uses the built-in `PostgreSQLDatabaseProvider`, `CustomSqlDatabaseProvider` deleted, `AppEnvironment` rebuilt on `NpgsqlConnectionStringBuilder` with a configurable `SslMode` and no MARS. Solution builds green (Npgsql 10.0.3 + SqlClient still referenced); the semantic SqlClient API repoint stays in 22-04.**

## Accomplishments

### Task 1 — driver package + provider swap
- Added `Npgsql` **10.0.3** (latest stable, no prerelease) to `LabBilling Library/LabBilling Core.csproj`.
- **Deleted** `LabBilling Library/Repositories/CustomSqlDatabaseProvider.cs` (the `CustomSqlMsDatabaseProvider` whose sole purpose was rewriting SQL Server's `OUTPUT INSERTED` for error 334 — PostgreSQL uses native `RETURNING` via the PG provider, so it is removed, not ported).
- Confirmed the exact provider type by reflecting the installed `PetaPoco.Compiled 6.0.683` assembly: **`PetaPoco.Providers.PostgreSQLDatabaseProvider`** (fully-qualified, no extra package needed).
- Repointed **all three** live `DatabaseConfiguration` sites to the PG provider:
  - `LabBilling Library/UnitOfWork/UnitOfWorkMain.cs` (`Initialize`)
  - `LabBilling Library/UnitOfWork/UnitOfWorkSystem.cs` (`Initialize`)
  - `LabBilling Library/Services/AuthenticationService.cs` (ctor `_db` build) — **third site the plan did not enumerate; auto-fixed** (it referenced the deleted type and would otherwise break compilation). See Deviations.
- Kept `Microsoft.Data.SqlClient` (7.0.1) referenced as instructed — removing it now would break the un-swept API usages (that removal is 22-04).

### Task 2 — AppEnvironment rebuilt for Npgsql + TLS, MARS dropped
- `LabBilling Library/Repositories/AppEnvironment.cs`: swapped `Microsoft.Data.SqlClient` for `Npgsql`; both `ConnectionString` and `LogConnectionString` now build via a shared `BuildConnection(database)` helper using `NpgsqlConnectionStringBuilder`.
- Mapping applied: `DataSource`→`Host`(+`Port`), `InitialCatalog`→`Database`, `UserID`/`Password`→`Username`/`Password`, `ConnectTimeout`→`Timeout`; pooling (`Pooling`/`MinPoolSize`/`MaxPoolSize`) and `ApplicationName` carried over; command-timeout behavior unchanged (still `UsingCommandTimeout(180)` at the UoW sites).
- **`MultipleActiveResultSets` removed** — Npgsql has no MARS and FINDINGS D4 confirms 0 code paths depend on it.
- **`_dbEncrypt`/`_dbTrustServerCert` consts replaced** by a configurable **`SslMode` property** (Npgsql `SslMode` enum), defaulted to a dev-safe **`SslMode.Prefer`** for the local docker PG. Added a `RootCertificate` property (applied only when set) so the managed/prod target (AWS RDS) can require `SslMode.VerifyFull` + the RDS CA bundle. This replaces the old `Encrypt=false;TrustServerCertificate=true` HIPAA in-transit gap (R6) with an explicit, config-settable TLS policy.
- Host/port/db/user come from the existing config fields (`ServerName`/`DatabaseName`/`LogDatabaseName`/`UserName`/`Password`), **not hardcoded**. Added `ResolveHostAndPort()` which parses a bare host or `host:port`/`host,port` and falls back to the local dev PG port **5434** (the Phase 22 docker container) when no port is configured. Pointed at the local PG (`labbilling`) when `ServerName`/`DatabaseName` are configured accordingly.

## Files Created/Modified
- `LabBilling Library/LabBilling Core.csproj` — added `Npgsql` 10.0.3
- `LabBilling Library/Repositories/CustomSqlDatabaseProvider.cs` — **deleted** (`git rm`)
- `LabBilling Library/UnitOfWork/UnitOfWorkMain.cs` — provider swap
- `LabBilling Library/UnitOfWork/UnitOfWorkSystem.cs` — provider swap
- `LabBilling Library/Services/AuthenticationService.cs` — provider swap (deviation auto-fix)
- `LabBilling Library/Repositories/AppEnvironment.cs` — Npgsql builder, `SslMode`/`RootCertificate`, MARS removed, host:port parsing
- `.planning/phases/22-postgres-foundation/22-03-SUMMARY.md` — this file
- `.planning/ROADMAP.md` — Phase 22 → 3/5

## Build State
Built with VS MSBuild 18 (`/p:Platform=x64`, Debug) — both `LabBilling Core.csproj` (clean Rebuild) and the full `Lab Billing.sln`.

**Result: build green — 0 errors** across the whole solution (warnings only: NU1510 prune advisories, NPOI EULA notice, MSB3270 MSIL/x64 ref-arch mismatch, a pre-existing unused-`_appParms` field).

**Note — "build green deferred to 22-04" caveat:** the plan anticipated remaining `Microsoft.Data.SqlClient` compile errors here. They did **not** materialize because the SqlClient package is (intentionally) still referenced, so every existing `SqlParameter`/`SqlConnection`/`SqlDataAdapter` API call still compiles. 22-04's work is therefore **semantic, not compile-driven**: repoint those ~51 files' SqlClient API usages to Npgsql equivalents (`NpgsqlParameter`/`NpgsqlConnection`/`NpgsqlDataAdapter`, incl. the raw `ReportingRepository` per R5), then remove the `Microsoft.Data.SqlClient` package. Until that semantic sweep lands, the code compiles but those un-swept code paths will not actually run against PostgreSQL — so the "wired correctly" gate is met, and runtime-green is deferred to 22-04/22-05.

## Decisions Made
- **Provider:** `PetaPoco.Providers.PostgreSQLDatabaseProvider` (built into PetaPoco.Compiled 6.0.683 — no extra provider package), referenced fully-qualified to avoid touching `using` lists. Per the 22-02 identifier policy (preserve casing + quote-all), the PG provider's identifier escaping double-quotes names and applies **no** lowercasing, matching the byte-identical PG/`[Table]`/`[Column]` names.
- **Npgsql version:** 10.0.3 (latest stable on NuGet at execution; restore resolved it cleanly).
- **SslMode default:** `Prefer` (dev-safe for local docker, which may not present a cert) rather than `Disable`, so TLS is used opportunistically; prod sets `VerifyFull` + `RootCertificate` via config.
- **Integrated auth:** Npgsql 10 has no builder-level `IntegratedSecurity` property, so for the SQL-auth dev path only `Username`/`Password` are set; Kerberos/integrated security against a Windows-hosted PG would be plumbed via the `Integrated Security=true` keyword later (not needed for the docker dev target).

## Deviations
- **Third provider site (auto-fixed blocker):** `AuthenticationService.cs:31` also called `.UsingProvider<CustomSqlMsDatabaseProvider>(...)`. The plan named only the two UoW sites. Leaving it would reference the just-deleted type and break compilation, so it was repointed to `PostgreSQLDatabaseProvider` alongside the UoW sites. No other live references to `CustomSqlMsDatabaseProvider`/`CustomSqlDatabaseProvider` remain (only planning/analysis docs mention them).
- **`IntegratedSecurity` builder property dropped:** plan suggested mapping `IntegratedSecurity`; Npgsql 10's `NpgsqlConnectionStringBuilder` does not expose it (initial attempt failed CS1061). Replaced with the Username/Password-only dev path as the plan itself allowed ("for SQL-auth dev just set Username/Password").

## Next Step
Ready for 22-04-PLAN.md (Microsoft.Data.SqlClient API sweep — repoint ~51 files to Npgsql, then remove the SqlClient package; build/runtime green).
