<objective>
Fix all MEDIUM priority SQL plan cache pollution issues identified in `Analysis/sql-plan-cache-pollution-analysis.md`. These are lower-impact but still represent poor practices: conditional WHERE variants in drug screen queries, raw SQL string construction, and a mismatched database provider in LogRepository.
</objective>

<context>
Read `CLAUDE.md` for project conventions.

This is Phase 3 (final phase) of a multi-phase remediation. Earlier phases added `WhereLike`/`WhereOptional` helpers to `RepositoryCoreBase<TPoco>`, fixed all CRITICAL issues, and fixed all HIGH priority issues. You can use the helpers where appropriate.

The codebase uses PetaPoco ORM with `PetaPoco.Sql.Builder` for query construction. Parameters use `System.Data.SqlClient.SqlParameter` with explicit `SqlDbType`.

Read each file before modifying it to understand the full context.
</context>

<fixes>

<fix id="MEDIUM-1">
<title>Standardize RandomDrugScreenPersonRepository conditional WHERE clauses</title>
<file>LabBilling Library/Repositories/RandomDrugScreenPersonRepository.cs</file>
<description>
Multiple methods have optional WHERE conditions creating 2-4 plan variants each. Standardize them to produce consistent query shapes using the `(@0 IS NULL OR ...)` pattern or `(@0 = 0 OR ...)` for boolean flags.

Read the file and find these methods:
1. `GetByClientAsync` — has optional `deleted = 0` condition based on an `includeDeleted` parameter
2. `GetByClientAndShiftAsync` — has optional `deleted = 0` condition
3. `GetDistinctShiftsAsync` — has optional `cli_mnem = @0` condition
4. `GetCandidateCountAsync` — has optional `shift` and `deleted` conditions

For each, apply the same pattern as the HIGH-1 fixes:
- Boolean flags: `(@0 = 0 OR column = @1)` where @0 is the flag
- Optional string values: `(@0 IS NULL OR column = @0)` passing DBNull.Value when the string is null/empty
- Optional deleted filter: `(@0 = 1 OR deleted = 0)` where @0 is the includeDeleted flag (when true, skip the filter)
</description>
</fix>

<fix id="MEDIUM-2">
<title>Convert MappingRepository raw SQL to Sql.Builder</title>
<file>LabBilling Library/Repositories/MappingRepository.cs</file>
<description>
Two methods use raw interpolated SQL strings instead of Sql.Builder:

```csharp
sql = $"select DISTINCT {GetRealColumn(nameof(Mapping.SystemType))} from {_tableName}";
// and
sql = $"select DISTINCT {GetRealColumn(nameof(Mapping.InterfaceName))} from {_tableName}";
```

Convert to Sql.Builder pattern. Read the file to understand how the results are consumed (likely `Context.Fetch<string>(sql)` or similar). Convert to:

```csharp
var sql = PetaPoco.Sql.Builder
    .Select($"DISTINCT {GetRealColumn(nameof(Mapping.SystemType))}")
    .From(_tableName);
```

And:
```csharp
var sql = PetaPoco.Sql.Builder
    .Select($"DISTINCT {GetRealColumn(nameof(Mapping.InterfaceName))}")
    .From(_tableName);
```

Then use `Context.Fetch<string>(sql)` or whatever the existing consumption pattern is.
</description>
</fix>

<fix id="MEDIUM-3">
<title>Convert PhyRepository and AuditReportRepository raw SQL to Sql.Builder</title>
<file>LabBilling Library/Repositories/PhyRepository.cs</file>
<file>LabBilling Library/Repositories/AuditReportRepository.cs</file>
<description>
**PhyRepository.cs** — `GetActive` method uses:
```csharp
string sql = $"SELECT * FROM {_tableName} where deleted = 0";
```

Convert to Sql.Builder and parameterize the literal:
```csharp
var sql = PetaPoco.Sql.Builder
    .Where($"{GetRealColumn(nameof(Phy.IsDeleted))} = @0",
        new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = false });
```

Note: With PetaPoco's `WithAutoSelect()` enabled, starting with `.Where()` will auto-prepend the SELECT and FROM for the entity type.

Read the method to verify this pattern works with the existing fetch call.

**AuditReportRepository.cs** — `GetMenus` method uses:
```csharp
var list = Context.Fetch<string>($"SELECT DISTINCT {GetRealColumn(nameof(AuditReport.Button))} FROM {TableName}");
```

Convert to Sql.Builder:
```csharp
var sql = PetaPoco.Sql.Builder
    .Select($"DISTINCT {GetRealColumn(nameof(AuditReport.Button))}")
    .From(TableName);
var list = Context.Fetch<string>(sql);
```

Note: Since this selects a single string column (not the full entity), `WithAutoSelect()` won't apply — the explicit `.Select().From()` is needed.
</description>
</fix>

<fix id="MEDIUM-4">
<title>Standardize LogRepository to use shared database provider</title>
<file>LabBilling Library/Repositories/LogRepository.cs</file>
<description>
LogRepository creates its own `PetaPoco.Database` instance using `SqlServerMsDataDatabaseProvider` instead of `CustomSqlMsDatabaseProvider`, and does NOT use `MyMapper`. This means queries from LogRepository may generate different SQL text than equivalent queries through the main UnitOfWork.

Read the file to understand:
1. Why it creates its own Database instance (likely because it logs to a different database or needs isolation)
2. Whether changing the provider would break anything

If the LogRepository uses the same database as the main application:
- Change `SqlServerMsDataDatabaseProvider` to `CustomSqlMsDatabaseProvider`
- Add the `MyMapper` to the database configuration

If it uses a separate database (different connection string), the different provider is acceptable but document why with a code comment.

Read the file carefully before making changes. If the separate instance exists for a valid architectural reason (e.g., writing to a logging database), add a comment explaining the rationale rather than changing the provider.
</description>
</fix>

</fixes>

<verification>
After all fixes:
1. Run `dotnet build "Lab Billing.sln"` — must compile without errors.
2. Run `dotnet test "LabBillingCore.UnitTests/LabBillingCore.UnitTests.csproj"` — all tests must pass.
3. Review each modified file to verify logical equivalence with the original behavior.
</verification>

<success_criteria>
- All MEDIUM priority issues are addressed.
- Raw SQL strings are converted to Sql.Builder pattern where practical.
- Conditional WHERE clauses produce consistent query shapes.
- LogRepository database provider is either standardized or documented.
- Solution builds cleanly.
- All tests pass.
</success_criteria>
