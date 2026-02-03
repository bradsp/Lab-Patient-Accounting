<objective>
Fix all HIGH priority SQL plan cache pollution issues identified in `Analysis/sql-plan-cache-pollution-analysis.md`. These fall into two categories: dynamic WHERE clause assembly (creating 2^N plan variants) and hardcoded string literals in SQL (defeating parameterization).
</objective>

<context>
Read `CLAUDE.md` for project conventions.

This is Phase 2 of a multi-phase remediation. Earlier phases added `WhereLike` and `WhereOptional` helper methods to `RepositoryCoreBase<TPoco>` and fixed all CRITICAL issues. You can use these helpers where appropriate.

**WhereOptional pattern:** `WhereOptional(sql, column, value)` appends `(@0 IS NULL OR column = @0)` — when value is null the condition is a no-op; when non-null it filters. This produces consistent SQL text regardless of which parameters are supplied.

**WhereLike pattern:** `WhereLike(sql, column, value)` appends `column like @0` with `value + "%"` passed as the parameter.

The codebase uses PetaPoco ORM. Parameters use `System.Data.SqlClient.SqlParameter` with explicit `SqlDbType`.

Read each file before modifying it to understand the full context.
</context>

<fixes>

<fix_group id="HIGH-1" title="Dynamic WHERE clause assembly">

<fix id="HIGH-1a">
<title>Standardize ChrgRepository.GetByAccount to consistent query shape</title>
<file>LabBilling Library/Repositories/ChrgRepository.cs</file>
<description>
The `GetByAccount` method has 4 optional conditions (`asOfDate`, `showCredited`, `includeInvoiced`, `excludeCBill`) that create up to 16 plan variants.

Read the method carefully and refactor the conditional WHERE clauses to always produce the same SQL text shape. Use the `WhereOptional` helper where the pattern fits (simple equality). For more complex conditions (IS NULL OR, inequality, etc.), use inline `(@0 IS NULL OR ...)` patterns.

**Strategy for each condition:**

1. `asOfDate` (DateTime?) — Use: `sql.Where($"(@0 IS NULL OR {_tableName}.{GetRealColumn(nameof(Chrg.UpdatedDate))} > @0)", asOfDate.HasValue ? (object)new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = asOfDate.Value } : DBNull.Value);`

   Actually, the simplest approach: pass `asOfDate` directly and let PetaPoco handle null:
   ```csharp
   sql.Where($"(@0 IS NULL OR {_tableName}.{GetRealColumn(nameof(Chrg.UpdatedDate))} > @0)", asOfDate);
   ```

2. `showCredited` (bool, default true) — When false, filter `IsCredited = 0`. Convert to: always include the clause, but make it a no-op when `showCredited` is true:
   ```csharp
   sql.Where($"(@0 = 1 OR {GetRealColumn(nameof(Chrg.IsCredited))} = 0)",
       new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = showCredited });
   ```
   When `showCredited=true`, `@0=1` is true so the OR short-circuits. When `showCredited=false`, `@0=1` is false so the column check applies.

3. `includeInvoiced` (bool, default true) — When false, filter invoice is null or empty. Convert similarly:
   ```csharp
   sql.Where($"(@0 = 1 OR {GetRealColumn(nameof(Chrg.Invoice))} is null OR {GetRealColumn(nameof(Chrg.Invoice))} = '')",
       new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = includeInvoiced });
   ```

4. `excludeCBill` (bool, default true) — When true, filter CDMCode <> value. Convert:
   ```csharp
   sql.Where($"(@0 = 0 OR {GetRealColumn(nameof(Chrg.CDMCode))} <> @1)",
       new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = excludeCBill },
       new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AppEnvironment.ApplicationParameters.ClientInvoiceCdm });
   ```
   When `excludeCBill=false`, `@0=0` is true so condition is a no-op. When true, the inequality check applies. Note: The CDM value parameter is always included even when the condition is a no-op — this is necessary for consistent query text.

**Important:** Verify the resulting SQL is logically equivalent to the original conditional approach by testing each boolean combination mentally.
</description>
</fix>

<fix id="HIGH-1b">
<title>Standardize AccountSearchRepository.GetBySearch (multi-param overload) to consistent query shape</title>
<file>LabBilling Library/Repositories/AccountSearchRepository.cs</file>
<description>
The `GetBySearch` method with 7 string parameters (lastName, firstName, mrn, ssn, dob, sex, account) creates up to 128 plan variants.

Refactor each conditional WHERE to always be present using `(@0 IS NULL OR column = @0)` pattern. For the LIKE conditions, use `(@0 IS NULL OR column like @0)`.

For each parameter, replace:
```csharp
if (!String.IsNullOrEmpty(searchText))
    command.Where($"{GetRealColumn(...)} like @0", new SqlParameter() { ... Value = searchText + "%" });
```
With:
```csharp
command.Where($"(@0 IS NULL OR {GetRealColumn(...)} like @0)",
    string.IsNullOrEmpty(searchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = searchText + "%" });
```

For equality conditions (account, mrn, ssn, sex), use the same pattern but without the `%`:
```csharp
command.Where($"(@0 IS NULL OR {GetRealColumn(...)} = @0)",
    string.IsNullOrEmpty(searchText) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = searchText });
```

For the `dobSearch` (date) parameter:
```csharp
DateTime? dobDt = null;
if (!string.IsNullOrEmpty(dobSearch))
    DateTime.TryParse(dobSearch, out DateTime parsed) ? dobDt = parsed : dobDt = null;

command.Where($"(@0 IS NULL OR {GetRealColumn(nameof(AccountSearch.DateOfBirth))} = @0)",
    dobDt.HasValue ? (object)new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = dobDt.Value } : DBNull.Value);
```

**Important:** Read the existing method carefully. The exact column names from `GetRealColumn()` calls must be preserved exactly as they are. Only change the conditional structure, not the column references.
</description>
</fix>

<fix id="HIGH-1c">
<title>Standardize InvoiceHistoryRepository.GetWithSort to consistent query shape</title>
<file>LabBilling Library/Repositories/InvoiceHistoryRepository.cs</file>
<description>
The `GetWithSort` method has 3 optional conditions creating up to 8 plan variants.

Refactor using the `(@0 IS NULL OR ...)` pattern:

1. `clientMnem` — Replace conditional with:
   ```csharp
   sql.Where($"(@0 IS NULL OR cl_mnem = @0)",
       string.IsNullOrEmpty(clientMnem) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem });
   ```

2. `fromDate`/`throughDate` — These are used together with BETWEEN. Replace with:
   ```csharp
   sql.Where($"(@0 IS NULL OR @1 IS NULL OR {_tableName}.mod_date between @0 and @1)",
       fromDate.HasValue ? (object)new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = fromDate.Value } : DBNull.Value,
       throughDate.HasValue ? (object)new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = throughDate.Value } : DBNull.Value);
   ```

3. `invoice` — Replace conditional with:
   ```csharp
   sql.Where($"(@0 IS NULL OR {GetRealColumn(nameof(InvoiceHistory.InvoiceNo))} = @0)",
       string.IsNullOrEmpty(invoice) ? (object)DBNull.Value : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = invoice });
   ```
</description>
</fix>

<fix id="HIGH-1d">
<title>Standardize InsCompanyRepository.GetAll to consistent query shape</title>
<file>LabBilling Library/Repositories/InsCompanyRepository.cs</file>
<description>
The `GetAll(bool excludeDeleted)` method has a conditional WHERE creating 2 plan variants.

Replace:
```csharp
var sql = Sql.Builder;
if(excludeDeleted)
    sql.Where($"{GetRealColumn(nameof(InsCompany.IsDeleted))} = @0",
        new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = false } );
```

With:
```csharp
var sql = Sql.Builder;
sql.Where($"(@0 = 0 OR {GetRealColumn(nameof(InsCompany.IsDeleted))} = @1)",
    new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = excludeDeleted },
    new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = false });
```

When `excludeDeleted=false`, `@0=0` is true (Bit false = 0) so condition is a no-op. When `excludeDeleted=true`, `@0=0` is false so the IsDeleted check applies.
</description>
</fix>

</fix_group>

<fix_group id="HIGH-2" title="Hardcoded string literals in WHERE clauses">

<fix id="HIGH-2a">
<title>Parameterize hardcoded status values in ClientRepository.GetUnbilledAccounts</title>
<file>LabBilling Library/Repositories/ClientRepository.cs</file>
<description>
In the `GetUnbilledAccounts` method, replace hardcoded literals with parameters:

1. Replace `not in ('CBILL','N/A')` with parameterized IN:
   ```csharp
   .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Status))} not in (@0, @1)",
       new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "CBILL" },
       new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "N/A" })
   ```

2. Replace `= 'C'` with parameter:
   ```csharp
   .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.FinancialType))} = @0",
       new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "C" })
   ```

3. Replace `not like '%HOLD%'` with parameter:
   ```csharp
   .Where($"{accTableName}.{GetRealColumn(typeof(Account), nameof(Account.Status))} not like @0",
       new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "%HOLD%" })
   ```

4. The `is null or ... = ''` clause on Invoice can remain as-is since those are structural conditions, not value literals. However, parameterize the empty string if feasible:
   ```csharp
   .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Invoice))} is null or {chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Invoice))} = @0",
       new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "" })
   ```
</description>
</fix>

<fix id="HIGH-2b">
<title>Parameterize hardcoded 'N' flag in MessagesInboundRepository</title>
<file>LabBilling Library/Repositories/MessagesInboundRepository.cs</file>
<description>
Two methods use hardcoded `= 'N'`. Replace with parameters in both:

1. `GetQueueCounts` method — Replace:
   ```csharp
   .Where($"{GetRealColumn(nameof(MessageInbound.ProcessFlag))} = 'N'")
   ```
   With:
   ```csharp
   .Where($"{GetRealColumn(nameof(MessageInbound.ProcessFlag))} = @0",
       new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "N" })
   ```

2. `GetUnprocessedMessages` method — Same fix:
   ```csharp
   command.Where($"{GetRealColumn(nameof(MessageInbound.ProcessFlag))} = @0",
       new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "N" });
   ```
</description>
</fix>

<fix id="HIGH-2c">
<title>Parameterize hardcoded date literal in PatientStatementRepository</title>
<file>LabBilling Library/Repositories/PatientStatementRepository.cs</file>
<description>
In the `GetByBatch` method, the date `'01/01/1900'` is hardcoded. Replace:

```csharp
sql.Where($"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} is null or " +
    $"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} = '01/01/1900'");
```

With:
```csharp
sql.Where($"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} is null or " +
    $"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} = @0",
    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = new DateTime(1900, 1, 1) });
```
</description>
</fix>

<fix id="HIGH-2d">
<title>Parameterize literal in AccountService.GetClaimItems JOIN ON clause</title>
<file>LabBilling Library/Services/AccountService.cs</file>
<description>
In the `GetClaimItems` method, the `InsCoverage.Primary` value (which resolves to a string like `'A'`) is interpolated directly into the JOIN ON clause. FORCED parameterization does not parameterize JOIN ON literals.

This fix is more complex because PetaPoco's `.On()` method may not support parameters directly. Read the method to understand the full JOIN structure.

**Approach:** Move the coverage filter from the ON clause to a WHERE clause:

Replace:
```csharp
.On($"{insTableName}.{uow.InsRepository.GetRealColumn(nameof(Ins.Account))} = {accTableName}.{uow.AccountRepository.GetRealColumn(nameof(Account.AccountNo))} and {uow.InsRepository.GetRealColumn(nameof(Ins.Coverage))} = '{InsCoverage.Primary}'")
```

With:
```csharp
.On($"{insTableName}.{uow.InsRepository.GetRealColumn(nameof(Ins.Account))} = {accTableName}.{uow.AccountRepository.GetRealColumn(nameof(Account.AccountNo))}")
```

Then add a WHERE clause for the coverage filter:
```csharp
.Where($"{uow.InsRepository.GetRealColumn(nameof(Ins.Coverage))} = @0",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = InsCoverage.Primary })
```

**Important semantic note:** Moving a filter from INNER JOIN ON to WHERE is semantically equivalent for INNER JOINs. However, if this were a LEFT/RIGHT JOIN, moving the condition to WHERE would change the semantics. Verify the join type is INNER before applying this change.
</description>
</fix>

</fix_group>

<verification>
After all fixes:
1. Run `dotnet build "Lab Billing.sln"` — must compile without errors.
2. Run `dotnet test "LabBillingCore.UnitTests/LabBillingCore.UnitTests.csproj"` — all tests must pass.
3. Review each modified file to verify logical equivalence with the original behavior.
</verification>

<success_criteria>
- All HIGH-1 dynamic WHERE methods produce consistent SQL text regardless of parameter combinations.
- All HIGH-2 hardcoded literals are replaced with parameterized values.
- Solution builds cleanly.
- All tests pass.
- No behavioral changes — the queries return the same results as before.
</success_criteria>
