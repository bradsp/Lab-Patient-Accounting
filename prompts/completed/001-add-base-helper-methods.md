<objective>
Add two new helper methods to RepositoryCoreBase<TPoco> that standardize parameterized LIKE queries and optional WHERE conditions. These helpers will be used by subsequent fix phases to eliminate SQL plan cache pollution patterns across the codebase.
</objective>

<context>
Read `CLAUDE.md` for project conventions.

This is Phase 0 of a multi-phase remediation of SQL Server plan cache pollution issues identified in `Analysis/sql-plan-cache-pollution-analysis.md`.

The base class file is: `LabBilling Library/Repositories/RepositoryCoreBase.cs`

This class is:
```csharp
public abstract class RepositoryCoreBase<TPoco> where TPoco : class
```

It already contains `GetRealColumn()` methods that resolve C# property names to SQL column names via PetaPoco `[Column]` attributes. The codebase uses `PetaPoco.Sql.Builder` extensively with `System.Data.SqlClient.SqlParameter` for parameterized queries.
</context>

<requirements>
Add the following two protected helper methods to `RepositoryCoreBase<TPoco>`:

1. **WhereLike** - Appends a parameterized LIKE clause to a PetaPoco Sql builder, with the wildcard `%` appended to the parameter value in C# (not in SQL). This prevents the `@0+'%'` anti-pattern that defeats SQL Server index seeks.

```csharp
/// <summary>
/// Appends a parameterized LIKE clause with trailing wildcard. Appends '%' to the value
/// in C# rather than using SQL concatenation (@0+'%'), which allows SQL Server to use
/// index seeks and avoids plan cache pollution.
/// </summary>
protected PetaPoco.Sql WhereLike(PetaPoco.Sql sql, string column, string value)
{
    return sql.Where($"{column} like @0",
        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = value + "%" });
}
```

2. **WhereOptional** - Appends a WHERE clause that is effectively a no-op when the value is null, but filters when a value is provided. This produces a consistent SQL query shape regardless of which optional parameters are supplied, enabling plan reuse.

```csharp
/// <summary>
/// Appends an optional WHERE condition that produces consistent SQL text regardless of
/// whether the value is null. When value is null, the condition short-circuits via
/// @0 IS NULL. This prevents plan cache pollution from dynamic WHERE clause assembly.
/// </summary>
protected PetaPoco.Sql WhereOptional(PetaPoco.Sql sql, string column, object value)
{
    return sql.Where($"(@0 IS NULL OR {column} = @0)", value);
}
```

Place these methods after the existing `GetRealColumn` methods (after line ~101) in the class.

Ensure the required `using System.Data;` directive is present at the top of the file (for `SqlDbType`). The file likely already has `using System.Data.SqlClient;` — verify and add `using System.Data;` only if not already present.
</requirements>

<constraints>
- Do NOT modify any existing methods in the file.
- Do NOT add any other methods or refactor anything beyond what is specified.
- Match the existing code style (indentation, brace placement, naming conventions) of the file.
- Use `System.Data.SqlClient.SqlParameter` consistent with the rest of the codebase.
</constraints>

<verification>
After making changes:
1. Run `dotnet build "Lab Billing.sln"` to verify the solution compiles.
2. Run `dotnet test "LabBillingCore.UnitTests/LabBillingCore.UnitTests.csproj"` to verify no regressions.
3. Confirm both methods are accessible from a derived repository class by checking that the access modifier is `protected`.
</verification>

<success_criteria>
- `RepositoryCoreBase<TPoco>` contains both `WhereLike` and `WhereOptional` methods.
- Solution builds without errors.
- All existing tests pass.
</success_criteria>
