<objective>
Fix all 5 CRITICAL SQL plan cache pollution issues identified in `Analysis/sql-plan-cache-pollution-analysis.md`. These issues create unique SQL plans per execution, wasting SQL Server memory and degrading performance.
</objective>

<context>
Read `CLAUDE.md` for project conventions.

This is Phase 1 of a multi-phase remediation. Phase 0 (prompt 001) added `WhereLike` and `WhereOptional` helper methods to `RepositoryCoreBase<TPoco>`. You can use these helpers where appropriate.

The codebase uses PetaPoco ORM with `PetaPoco.Sql.Builder` for query construction. Parameters should use `System.Data.SqlClient.SqlParameter` with explicit `SqlDbType` as is convention in this codebase.

Read each file before modifying it to understand the full context.
</context>

<fixes>

<fix id="CRITICAL-1" priority="1">
<title>Parameterize IN clause in AccountSearchRepository</title>
<file>LabBilling Library/Repositories/AccountSearchRepository.cs</file>
<description>
In the `GetBySearch` method that takes a tuple array parameter (around lines 31-109), find the block:
```csharp
if (op == "in")
{
    command.Where($"{propName} {op} ({searchText})");
}
```

This directly interpolates user-provided search text into SQL without parameterization, creating a new plan for every unique set of IN values. This is also a SQL injection vulnerability.

Replace the `if (op == "in")` block (and add handling for `"not in"` as well) with parameterized array expansion:

```csharp
if (op == "in" || op == "not in")
{
    var values = searchText.Split(',').Select(s => s.Trim().Trim('\'')).ToArray();
    command.Where($"{propName} {op} (@0)", values);
}
```

Also verify whether there is a separate `else if` or case for `"not in"` that needs the same treatment. The `operation` enum includes `NotOneOf` which maps to `"not in"`.
</description>
</fix>

<fix id="CRITICAL-2" priority="2">
<title>Fix LIKE parameter arithmetic in PhyRepository and SanctionedProviderRepository</title>
<file>LabBilling Library/Repositories/PhyRepository.cs</file>
<file>LabBilling Library/Repositories/SanctionedProviderRepository.cs</file>
<description>
Both files have the same anti-pattern in their `GetByName` methods. The `@0+'%'` pattern in SQL prevents index seeks.

**PhyRepository.cs** - In `GetByName(string lastName, string firstName)`, replace:
```csharp
.Where($"{this.GetRealColumn(typeof(Phy), nameof(Phy.LastName))} like @0+'%'",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastName })
.Where($"{this.GetRealColumn(typeof(Phy), nameof(Phy.FirstName))} like @0+'%'",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = firstName })
```
With calls to the new `WhereLike` helper:
```csharp
WhereLike(sql, this.GetRealColumn(typeof(Phy), nameof(Phy.LastName)), lastName);
WhereLike(sql, this.GetRealColumn(typeof(Phy), nameof(Phy.FirstName)), firstName);
```
Note: Check the variable name used for the Sql builder (it may be `sql` or `command` — use whatever is in the existing code). The `WhereLike` method returns the Sql object, so you can chain or assign as needed. If the existing code uses fluent chaining (`.Where(...).Where(...)`), you may need to adapt — either use the return value or call it as a statement.

**SanctionedProviderRepository.cs** - Apply the identical fix in its `GetByName` method:
```csharp
WhereLike(sql, this.GetRealColumn(nameof(SanctionedProvider.LastName)), lastName);
WhereLike(sql, this.GetRealColumn(nameof(SanctionedProvider.FirstName)), firstName);
```
</description>
</fix>

<fix id="CRITICAL-3" priority="3">
<title>Parameterize interpolated values in RandomDrugScreenPersonRepository UPDATE statements</title>
<file>LabBilling Library/Repositories/RandomDrugScreenPersonRepository.cs</file>
<description>
Two methods — `SoftDeleteByClientAsync` and `MarkMissingAsDeletedAsync` — interpolate `Environment.UserName`, `Utilities.OS.GetAppName()`, and `Environment.MachineName` directly into SQL UPDATE statements. Each unique user/machine/app combination creates a different cached plan. This is also a SQL injection risk.

In both methods, replace the three interpolated `.Append()` lines:
```csharp
.Append($"mod_user = '{Environment.UserName}',")
.Append($"mod_prg = '{Utilities.OS.GetAppName()}',")
.Append($"mod_host = '{Environment.MachineName}'")
```

With parameterized versions:
```csharp
.Append("mod_user = @0,", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.UserName })
.Append("mod_prg = @0,", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Utilities.OS.GetAppName() })
.Append("mod_host = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName })
```

Apply this fix to BOTH methods in the file.
</description>
</fix>

<fix id="CRITICAL-4" priority="4">
<title>Parameterize table name in RepositoryBase INFORMATION_SCHEMA query</title>
<file>LabBilling Library/Repositories/RepositoryBase.cs</file>
<description>
In the `CheckDBFieldLengths` method, the `_tableName` value is interpolated directly into the INFORMATION_SCHEMA query, creating 67+ unique plans (one per repository type).

Find:
```csharp
var maxLengths = Context.Fetch<ColumnInfo>(
    $"SELECT COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{_tableName}'")
    .ToDictionary(x => x.COLUMN_NAME, x => x.CHARACTER_MAXIMUM_LENGTH);
```

Replace with:
```csharp
var maxLengths = Context.Fetch<ColumnInfo>(
    "SELECT COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @0",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _tableName })
    .ToDictionary(x => x.COLUMN_NAME, x => x.CHARACTER_MAXIMUM_LENGTH);
```
</description>
</fix>

<fix id="CRITICAL-5" priority="5">
<title>Parameterize TOP clause in UserProfileRepository</title>
<file>LabBilling Library/Repositories/UserProfileRepository.cs</file>
<description>
In the `GetRecentAccount` method, the `numEntries` integer is concatenated into the SELECT clause. SQL Server's FORCED parameterization exempts TOP from auto-parameterization, so each distinct value creates a new plan.

Find:
```csharp
string select = "TOP " + numEntries + " *";
```

Replace the entire approach. Instead of concatenating TOP into the select string, use a parameterized TOP with parentheses (required by SQL Server for parameterized TOP):

```csharp
string select = "TOP (@0) *";
```

Then pass `numEntries` as a parameter to the `.Select()` call. Check how PetaPoco's `.Select()` method handles parameters. If `.Select()` does not accept parameters directly, restructure to use raw SQL:

```csharp
var command = PetaPoco.Sql.Builder
    .Append($"SELECT TOP (@0) * FROM {_tableName}", numEntries)
    .Where("UserName = @0 ", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = user })
    .Where("Parameter = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "RecentAccount" })
    .OrderBy($"{sortColumn} desc");

return Context.Fetch<UserProfile>(command);
```

Note: Using `.Append()` instead of `.Select().From()` allows passing a parameter to the TOP clause. Verify this compiles correctly.
</description>
</fix>

</fixes>

<verification>
After all fixes:
1. Run `dotnet build "Lab Billing.sln"` — must compile without errors.
2. Run `dotnet test "LabBillingCore.UnitTests/LabBillingCore.UnitTests.csproj"` — all tests must pass.
3. Briefly review each modified file to ensure no accidental changes to surrounding code.
</verification>

<success_criteria>
- All 5 CRITICAL issues are fixed with parameterized alternatives.
- No SQL injection vulnerabilities remain in the modified code.
- Solution builds cleanly.
- All tests pass.
</success_criteria>
