# Phase 22 Plan 04: SqlClient API Sweep → Npgsql-native Data Layer Summary

**The data-access layer is now `Microsoft.Data.SqlClient`-free. All ~51 LabBilling Library files plus two dependent-project files had their SqlClient API usages (`SqlParameter`/`SqlDbType`/`SqlConnection`/`SqlDataAdapter`/`SqlCommand`) replaced with PetaPoco-native positional args or Npgsql-native types, the `Microsoft.Data.SqlClient` PackageReference was removed from `LabBilling Core.csproj` (and swapped to Npgsql in `Utilities` and `Job Scheduler`), and the FULL solution builds GREEN with VS MSBuild x64 — 0 errors — WITHOUT the SqlClient package. SQL string contents were NOT rewritten (T-SQL → PG dialect stays Phase 23-03).**

## Files Touched

**56 files changed** (55 source/project files + this is reported via `git diff --stat`):
- **50 `LabBilling Library` files** (`.cs`): 48 repositories + `AccountService.cs` + `PatientBillingService.cs`, all converted to PetaPoco-native/Npgsql params; `using Microsoft.Data.SqlClient;` removed.
- **1 `LabBilling Library` project file**: `LabBilling Core.csproj` — removed `<PackageReference Include="Microsoft.Data.SqlClient" Version="7.0.1" />`.
- **`LabBillingConsole/Utilities.cs`** — 3 PetaPoco param sites in `FixDrugScreenCharges()` converted to plain values; using removed.
- **`Utilities/ConnectionStringExtensions.cs`** — `SqlConnectionStringBuilder` → `NpgsqlConnectionStringBuilder` (`.InitialCatalog`→`.Database`, `.DataSource`→`.Host`).
- **`Utilities/Utilities.csproj`** — SqlClient package swapped to `Npgsql` 10.0.3.
- **`Lab Patient Accounting Job Scheduler.csproj`** — unused SqlClient package swapped to `Npgsql` 10.0.3.
- **`LabBilling Library/Repositories/InsCoverage.cs`** — removed a now-dangling `using Microsoft.IdentityModel.Tokens;` (see Deviations).

## Sites That Needed Real Refactors (beyond mechanical param-stripping)

1. **`ReportingRepository.cs`** — the only raw ADO.NET site. `SqlConnection`/`SqlCommand`/`SqlDataAdapter` → `NpgsqlConnection`/`NpgsqlCommand`/`NpgsqlDataAdapter` (both `GetARByFinCode` and `GetARByFinCodeList`). SQL text left as T-SQL (`DATEADD`/`DATEDIFF`/`GetDate`, quoted `'Financial Class'` aliases) with a `// TODO Phase 23-03` flag on each query. **Will fail at runtime until Phase 23-03.**
2. **`NumberRepository.cs`** — genuine provider-param object required: the `@NextSequence` **OUTPUT** parameter (uses `.Direction = ParameterDirection.Output` and `.Value` read-back) → `NpgsqlParameter` + `NpgsqlDbType.Integer`. The positional `key` arg became a plain value. SQL `;EXEC GetNextNumber @0, @1 OUTPUT` is T-SQL stored-proc syntax — left untouched with a `// NOTE Phase 23-03` flag. **Runtime-deferred.**
3. **`PatientBillingService.cs`** — `ExecuteNonQueryProc(...)` named-param sites (`@thrudate`/`@batchNo`/`@endDate`) genuinely use `ParameterName`, so → `NpgsqlParameter` with `NpgsqlDbType`. One positional `Execute(sql, @0)` site became a plain value. Stored-proc names (`usp_prg_*`) are T-SQL, runtime-deferred to Phase 23.
4. **`AccountSearchRepository.cs`** — removed the private `SqlDbType GetType(Type)` helper entirely (its sole purpose was building `SqlParameter.SqlDbType`); all `command.Where(..., new SqlParameter{...})` calls became positional `command.Where(..., value)`. PetaPoco infers types.
5. **`ClientRepository.cs`** — two `ParameterName`-bearing `SqlParameter`s for `dbo.GetAccBalByDate(@0, @1)`; since PetaPoco binds positionally the names were dropped and the values passed directly. SQL `dbo.GetAccBalance(@0)` / `dbo.GetAccBalByDate(@0, @1)` left as-is (Phase 23).
6. **`PatRepository.cs`** — two sites used `DbType = DbType.String` (System.Data form) rather than `SqlDbType`; converted to plain values.
7. **`FinRepository.cs`** — one site used `SqlValue =` (not `Value =`); converted to plain value.

All remaining ~45 files were the uniform mechanical pattern `new SqlParameter() { SqlDbType = SqlDbType.X, Value = expr }` → `expr` passed positionally to PetaPoco `Where`/`Append`/`SingleOrDefault`/`Execute`/`Delete`/`Update`/`Fetch`.

## Projects Still on SqlClient (with reason)

**None in the data-access scope.** The `Microsoft.Data.SqlClient` package now appears only in the **legacy WinForms presentation tier** (`Lab PA WinForms UI.csproj` and the `Microsoft.Data.SqlClient.SNI.runtime` in `LabBilling Winforms Library.csproj`), which is the deprecated desktop client being replaced by the Blazor migration — explicitly out of scope for the data layer and not on the PostgreSQL build path. Those WinForms `.cs` SqlClient usages (`SqlParameterConverter.cs`, several Forms) were left untouched; the WinForms tier is not part of the Phase 22 PostgreSQL foundation and will be addressed (or dropped) with the Blazor port. `Utilities`, `LabBillingConsole`, `LabBillingService`, and `Lab Patient Accounting Job Scheduler` are all SqlClient-free.

## Package Removal Confirmation

- `grep -rl "Microsoft.Data.SqlClient" "LabBilling Library"` → **zero files**.
- `grep` for `SqlParameter|SqlDbType|SqlConnection|SqlDataAdapter|SqlCommand|SqlDataReader` across `LabBilling Library/**/*.cs` → **zero matches**.
- `<PackageReference Include="Microsoft.Data.SqlClient" .../>` removed from `LabBilling Core.csproj`.

## Build State

Built the FULL `Lab Billing.sln` with **VS MSBuild 18** (`C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe`, `/p:Platform=x64 /t:Build`).

**Result: 0 errors** (exit code 0, `/clp:ErrorsOnly` → "NO COMPILER ERRORS"), WITHOUT the `Microsoft.Data.SqlClient` package on the data layer. Warnings only (pre-existing NU1510 prune advisories, NPOI EULA notice, CS8632/CS8765/CS0114/CS8981 nullability/hiding warnings, unused-variable warnings).

## Decisions Made

- **Prefer PetaPoco-native positional args over provider param objects** (per plan): `SqlDbType` metadata dropped because PetaPoco/Npgsql infer parameter types from the CLR value. Provider param objects (`NpgsqlParameter`) used **only** where genuinely needed — output params (`NumberRepository`) and `ExecuteNonQueryProc` named params (`PatientBillingService`).
- **Npgsql 10.0.3** reused (latest stable, already in solution from 22-03) for `Utilities` and `Job Scheduler` package swaps — no new/prerelease packages introduced.
- **`ConnectionString` helper** repointed to `NpgsqlConnectionStringBuilder` rather than deleted (it's a public helper; preserved its API).

## Deviations

- **`InsCoverage.cs` dangling using (auto-fixed blocker):** removing the `Microsoft.Data.SqlClient` package dropped its transitive `Microsoft.IdentityModel.*` dependency, which `InsCoverage.cs` was relying on via an **unused** `using Microsoft.IdentityModel.Tokens;`. This surfaced as the single post-removal compile error (CS0234). Since no `IdentityModel` type is referenced in the file, the unused using was removed (auto-fix). This is exactly the "removing the package surfaces hidden sites" forcing function the plan intended.
- **`ReportingRepository.cs` trailing whitespace:** the `DATEADD(...GetDate())` SQL line had a single trailing space before the newline trimmed by the editor. SQL semantics are byte-identical; no T-SQL→PG rewrite occurred. Phase 23 boundary preserved.
- **WinForms tier left on SqlClient (documented above):** the plan's data-layer goal is met; the deprecated WinForms presentation project legitimately retains the package and is out of scope.

## Verification Checklist (from plan)

- [x] Zero `Microsoft.Data.SqlClient` / `SqlParameter` / `SqlDbType` references in `LabBilling Library` — grep returns zero.
- [x] Full solution builds with VS MSBuild: **0 errors**.
- [x] SQL string contents unchanged (T-SQL→PG dialect deferred to Phase 23-03) — verified via diff; only param/using/type changes (plus one benign trailing-whitespace trim).
- [x] No prerelease packages introduced — Npgsql 10.0.3 (stable) reused only.

## Next Step

Ready for **22-05-PLAN.md** (smoke verification).
