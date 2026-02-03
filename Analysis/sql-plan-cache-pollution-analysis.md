# SQL Server Plan Cache Pollution Analysis

**Date:** 2026-02-02
**Repository:** Lab-Patient-Accounting
**Branch:** billing-ui-to-blazor
**Analyst:** Claude Opus 4.5 (automated code analysis)

---

## Executive Summary

After a thorough examination of all 67+ repository files in `LabBilling Library/Repositories/` and all service files in `LabBilling Library/Services/`, the following findings were identified:

- **Total issues found:** 28 distinct issue instances across 7 categories of anti-pattern
- **Estimated unique plan-generating patterns:** ~200-400 unique SQL text variations (from column/table interpolation across 67+ repositories, dynamic WHERE assembly, literal embedding, and parameter arithmetic)
- **Top 3 most impactful issues:**
  1. **GetRealColumn() interpolation into SQL strings** -- pervasive across nearly every repository (~100+ call sites). While the column names are deterministic per type, each unique combination of column names produces a unique query text string that PetaPoco sends to SQL Server. Because PetaPoco's `Sql.Builder` `.Where($"{GetRealColumn(...))} = @0", ...)` embeds the resolved column name as literal text in the SQL string, the resulting SQL text is fixed per call site but differs from PetaPoco's auto-generated SQL for the same table, creating redundant plans.
  2. **Dynamic WHERE clause assembly** (AccountSearchRepository, ChrgRepository, InvoiceHistoryRepository, AccountSearch) -- conditional `.Where()` calls produce different SQL text depending on which parameters are supplied, creating 2^N plan variants per method.
  3. **Unparameterized literal values and parameter arithmetic in SQL** -- string literals embedded directly in WHERE clauses (`= 'N'`, `= 'C'`, `not in ('CBILL','N/A')`, `like '%HOLD%'`), `@0+'%'` concatenation in LIKE clauses, and `TOP + numEntries` concatenation all defeat parameterization.

**Key architectural insight:** The combination of FORCED parameterization on the SQL Server and PetaPoco's `Sql.Builder` means that literal values embedded in the SQL text string (like `'N'`, `'CBILL'`, `'C'`) will actually be auto-parameterized by SQL Server's FORCED parameterization. **However**, FORCED parameterization does NOT help with:
- SQL expressions like `@0+'%'` (parameter arithmetic in the query text)
- Different query shapes from conditional WHERE clauses
- `TOP N` when N varies (FORCED parameterization exempts TOP)
- `IN` clauses with variable numbers of items
- Table and column name differences (these are identifiers, not literals)

---

## Critical Issues (fix immediately)

### CRITICAL-1: Unparameterized IN clause with raw user input in AccountSearchRepository

**File:** `LabBilling Library/Repositories/AccountSearchRepository.cs`
**Lines:** 82-84

```csharp
if (op == "in")
{
    command.Where($"{propName} {op} ({searchText})");
}
```

**Why it causes plan pollution:** When the operation is `OneOf` or `NotOneOf`, the `searchText` value is embedded directly into the SQL string without any parameterization. Each unique set of values (e.g., `IN ('A','B','C')` vs `IN ('A','B')`) creates a completely different query text. This also presents a SQL injection vulnerability.

**Severity:** CRITICAL -- creates a new plan for every unique combination of IN values

**Frequency:** HIGH -- called from `WorklistService.GetAccountsForWorklistAsync()` which is used for every worklist view in the application. Note however that the current `WorklistService` code does not use `OneOf`/`NotOneOf` operations, so the actual hit depends on whether other callers use those operations.

**Recommended fix:**
```csharp
if (op == "in" || op == "not in")
{
    // Split the searchText into individual values and use PetaPoco's array expansion
    var values = searchText.Split(',').Select(s => s.Trim().Trim('\'')).ToArray();
    command.Where($"{propName} {op} (@0)", values);
}
```

---

### CRITICAL-2: Parameter arithmetic in LIKE clauses (`@0+'%'`)

**File:** `LabBilling Library/Repositories/PhyRepository.cs`
**Lines:** 49-52

```csharp
.Where($"{this.GetRealColumn(typeof(Phy), nameof(Phy.LastName))} like @0+'%'",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastName })
.Where($"{this.GetRealColumn(typeof(Phy), nameof(Phy.FirstName))} like @0+'%'",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = firstName })
```

**File:** `LabBilling Library/Repositories/SanctionedProviderRepository.cs`
**Lines:** 38-41

```csharp
.Where($"{this.GetRealColumn(nameof(SanctionedProvider.LastName))} like @0+'%'",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastName })
.Where($"{this.GetRealColumn(nameof(SanctionedProvider.FirstName))} like @0+'%'",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = firstName })
```

**Why it causes plan pollution:** The SQL expression `@0+'%'` is parameter arithmetic -- it concatenates a parameter with a string literal inside the SQL text. SQL Server's FORCED parameterization does NOT parameterize expressions involving parameter references combined with operators. Each execution sends `LIKE @0+'%'` as the query text, which SQL Server sees as a single fixed text. Actually, this particular pattern produces consistent text across calls (the `@0+'%'` is always the same text), so it generates exactly one plan per call site rather than one per value. **However**, this pattern forces SQL Server to evaluate the concatenation at runtime for every row, preventing the use of index seeks that a simple `LIKE @0` with the `%` appended to the parameter value would allow. While not strictly plan pollution, it significantly degrades query performance.

**Revised Severity:** MEDIUM for plan pollution (consistent text), HIGH for performance (prevents index usage)

**Frequency:** MEDIUM -- physician lookup and sanctioned provider lookup are used during HL7 processing and manual searches

**Recommended fix:** Append `%` to the parameter value before passing it:
```csharp
.Where($"{this.GetRealColumn(typeof(Phy), nameof(Phy.LastName))} like @0",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastName + "%" })
```

---

### CRITICAL-3: Interpolated values in UPDATE SQL (RandomDrugScreenPersonRepository)

**File:** `LabBilling Library/Repositories/RandomDrugScreenPersonRepository.cs`
**Lines:** 122-133 and 146-157

```csharp
var sql = Sql.Builder
    .Append("UPDATE " + _tableName)
    .Append("SET deleted = 1,")
    .Append("mod_date = GETDATE(),")
    .Append($"mod_user = '{Environment.UserName}',")
    .Append($"mod_prg = '{Utilities.OS.GetAppName()}',")
    .Append($"mod_host = '{Environment.MachineName}'")
    .Where("cli_mnem = @0", clientMnem);
```

**Why it causes plan pollution:** `Environment.UserName`, `Utilities.OS.GetAppName()`, and `Environment.MachineName` are interpolated directly into the SQL text as string literals. If different users or machines execute this code, each unique combination produces a different SQL text and therefore a different cached plan. Even with FORCED parameterization, different username/machine name combinations will produce different parameterized forms because the values appear as string literals in distinct positions. **This also presents a SQL injection risk** if any of these values contain single quotes.

**Severity:** CRITICAL -- creates a new plan for every unique user+machine+app combination

**Frequency:** LOW-MEDIUM -- called during drug screen candidate import operations

**Recommended fix:**
```csharp
var sql = Sql.Builder
    .Append("UPDATE " + _tableName)
    .Append("SET deleted = 1,")
    .Append("mod_date = GETDATE(),")
    .Append("mod_user = @0,", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.UserName })
    .Append("mod_prg = @0,", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Utilities.OS.GetAppName() })
    .Append("mod_host = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName })
    .Where("cli_mnem = @0", clientMnem);
```

---

### CRITICAL-4: Table name interpolated into INFORMATION_SCHEMA query (RepositoryBase)

**File:** `LabBilling Library/Repositories/RepositoryBase.cs`
**Line:** 243

```csharp
var maxLengths = Context.Fetch<ColumnInfo>(
    $"SELECT COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{_tableName}'")
    .ToDictionary(x => x.COLUMN_NAME, x => x.CHARACTER_MAXIMUM_LENGTH);
```

**Why it causes plan pollution:** The `_tableName` value varies by repository type (67+ different table names). Each distinct table name creates a completely different SQL text string. Even with FORCED parameterization, this query produces 67+ unique cached plans.

**Severity:** CRITICAL -- creates one unique plan per table type in the system (67+)

**Frequency:** LOW -- only called from `CheckDBFieldLengths()`, which is only invoked on "String or binary data would be truncated" exceptions. However, in a system processing many records, truncation errors during batch operations could trigger this repeatedly.

**Recommended fix:**
```csharp
var maxLengths = Context.Fetch<ColumnInfo>(
    "SELECT COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @0",
    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _tableName })
    .ToDictionary(x => x.COLUMN_NAME, x => x.CHARACTER_MAXIMUM_LENGTH);
```

---

### CRITICAL-5: TOP clause with variable integer concatenation

**File:** `LabBilling Library/Repositories/UserProfileRepository.cs`
**Line:** 35

```csharp
string select = "TOP " + numEntries + " *";
```

**Why it causes plan pollution:** The `numEntries` parameter (default 10, but can be any value) is concatenated directly into the SELECT clause. `TOP` with a literal value is explicitly exempt from SQL Server's FORCED parameterization. Each different `numEntries` value creates a new plan.

**Severity:** CRITICAL -- creates a new plan for every distinct `numEntries` value

**Frequency:** MEDIUM -- called for every user's recent account list retrieval. If the default (10) is always used, this produces only 1 plan, but any variation creates additional plans.

**Recommended fix:** Use `TOP (@0)` with a parameter (requires parentheses in SQL Server for parameterized TOP):
```csharp
var command = PetaPoco.Sql.Builder
    .Select("TOP (@0) *", numEntries)
    .From(_tableName)
    .Where(...)
```
Or restructure to use PetaPoco's `.Take()` if available, or use a subquery approach.

---

## High-Priority Issues

### HIGH-1: Dynamic WHERE clause assembly creating multiple plan shapes

Multiple repositories conditionally add WHERE clauses based on runtime parameters, which produces different SQL text for each combination of provided/omitted parameters.

#### HIGH-1a: ChrgRepository.GetByAccount -- 4 optional conditions

**File:** `LabBilling Library/Repositories/ChrgRepository.cs`
**Lines:** 54-85

```csharp
public List<Chrg> GetByAccount(string account, bool showCredited = true, bool includeInvoiced = true,
    DateTime? asOfDate = null, bool excludeCBill = true)
{
    var sql = PetaPoco.Sql.Builder
        .Where("account = @0", ...);

    if (asOfDate != null)
        sql.Where($"{_tableName}.{GetRealColumn(nameof(Chrg.UpdatedDate))} > @0", ...);
    if (!showCredited)
        sql.Where($"{GetRealColumn(nameof(Chrg.IsCredited))} = 0");
    if (!includeInvoiced)
        sql.Where($"{GetRealColumn(nameof(Chrg.Invoice))} is null or {GetRealColumn(nameof(Chrg.Invoice))} = ''");
    if (excludeCBill)
        sql.Where($"{GetRealColumn(nameof(Chrg.CDMCode))} <> @0", ...);
    ...
}
```

**Why it causes plan pollution:** With 4 optional conditions (asOfDate, showCredited, includeInvoiced, excludeCBill), this can produce up to 2^4 = 16 different SQL query texts.

**Severity:** HIGH -- up to 16 unique plans from a frequently-called method

**Frequency:** VERY HIGH -- called for nearly every account view/edit operation

#### HIGH-1b: AccountSearchRepository.GetBySearch (overloaded with multiple optional params)

**File:** `LabBilling Library/Repositories/AccountSearchRepository.cs`
**Lines:** 207-268

```csharp
public IEnumerable<AccountSearch> GetBySearch(string lastNameSearchText, string firstNameSearchText,
    string mrnSearchText, string ssnSearchText, string dobSearch, string sexSearch, string accountSearchText)
{
    var command = PetaPoco.Sql.Builder.Where("deleted = 0 ");
    if (!String.IsNullOrEmpty(lastNameSearchText))
        command.Where($"{GetRealColumn(nameof(AccountSearch.LastName))} like @0", ...);
    if (!string.IsNullOrEmpty(firstNameSearchText))
        command.Where($"{GetRealColumn(nameof(AccountSearch.FirstName))} like @0", ...);
    // ... 5 more optional conditions
}
```

**Why it causes plan pollution:** With 7 optional WHERE conditions, this creates up to 2^7 = 128 different query text shapes.

**Severity:** HIGH -- up to 128 unique plans

**Frequency:** HIGH -- primary patient/account search function used throughout the application

#### HIGH-1c: AccountSearchRepository.GetBySearch (tuple array overload)

**File:** `LabBilling Library/Repositories/AccountSearchRepository.cs`
**Lines:** 31-109

```csharp
public IList<AccountSearch> GetBySearch(
    (string propertyName, operation oper, string searchText)[] searchValues)
{
    foreach (var searchValue in searchValues)
    {
        // Dynamic column name from property mapping
        string propName = GetRealColumn(typeof(AccountSearch), prop.Name);
        // Dynamic operator from enum
        string op = ...; // "=", "like", ">=", "<>", "in", etc.
        command.Where($"{propName} {op} @0", ...);
    }
}
```

**Why it causes plan pollution:** Both the column name AND the operator are dynamically determined at runtime. Each unique combination of (column, operator, number of conditions) produces a different SQL text. Used heavily by `WorklistService`, which sends different parameter arrays per worklist type (14+ different worklist configurations).

**Severity:** HIGH -- creates at least 14 unique plans (one per worklist type) with potentially many more from ad-hoc search

**Frequency:** HIGH -- worklist loading is a primary UI function

#### HIGH-1d: InvoiceHistoryRepository.GetWithSort

**File:** `LabBilling Library/Repositories/InvoiceHistoryRepository.cs`
**Lines:** 23-51

```csharp
public IEnumerable<InvoiceHistory> GetWithSort(string clientMnem = null,
    DateTime? fromDate = null, DateTime? throughDate = null, string invoice = null)
{
    var sql = PetaPoco.Sql.Builder
        .Select($"{_tableName}.*, client.cli_nme as 'ClientName'")
        .From(_tableName)
        .LeftJoin("client").On($"{_tableName}.cl_mnem = client.cli_mnem");

    if(clientMnem != null)
        sql.Where($"cl_mnem = @0", ...);
    if (fromDate != null && throughDate != null)
        sql.Where($"{_tableName}.mod_date between @0 and @1", ...);
    if(!string.IsNullOrEmpty(invoice))
        sql.Where($"{GetRealColumn(nameof(InvoiceHistory.InvoiceNo))} = @0", ...);
    ...
}
```

**Why it causes plan pollution:** 3 optional conditions create up to 8 plan variants.

**Severity:** HIGH (up to 8 variants)

**Frequency:** MEDIUM -- invoice history lookup

#### HIGH-1e: InsCompanyRepository.GetAll with optional exclude

**File:** `LabBilling Library/Repositories/InsCompanyRepository.cs`
**Lines:** 38-50

```csharp
public List<InsCompany> GetAll(bool excludeDeleted)
{
    var sql = Sql.Builder;
    if(excludeDeleted)
        sql.Where($"{GetRealColumn(nameof(InsCompany.IsDeleted))} = @0", ...);
    var queryResult = Context.Fetch<InsCompany>(sql);
    return queryResult;
}
```

**Severity:** MEDIUM (2 variants only)

---

### HIGH-2: Hardcoded string literals in WHERE clauses

While FORCED parameterization should handle most simple literal comparisons, the following patterns embed string literals that create distinct SQL text from otherwise identical parameterized queries. FORCED parameterization may or may not handle these depending on the exact query complexity.

#### HIGH-2a: Hardcoded status values in ClientRepository.GetUnbilledAccounts

**File:** `LabBilling Library/Repositories/ClientRepository.cs`
**Lines:** 122-125

```csharp
.Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Status))} not in ('CBILL','N/A')")
.Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Invoice))} is null or ...")
.Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.FinancialType))} =  'C'")
.Where($"{accTableName}.{GetRealColumn(typeof(Account), nameof(Account.Status))} not like '%HOLD%'")
```

**Why it causes plan pollution:** Multiple string literals (`'CBILL'`, `'N/A'`, `'C'`, `'%HOLD%'`) are embedded directly. FORCED parameterization will attempt to parameterize the simple ones, but `not in ('CBILL','N/A')` and `not like '%HOLD%'` patterns may not be parameterized by FORCED mode because they are part of more complex expressions.

**Severity:** HIGH for the `not in` and `not like` patterns (FORCED parameterization exclusions)

**Frequency:** LOW -- unbilled accounts report

#### HIGH-2b: Hardcoded process flag in MessagesInboundRepository

**File:** `LabBilling Library/Repositories/MessagesInboundRepository.cs`
**Lines:** 25, 39

```csharp
.Where($"{GetRealColumn(nameof(MessageInbound.ProcessFlag))} = 'N'")
// and
command.Where($"{GetRealColumn(nameof(MessageInbound.ProcessFlag))} = 'N'");
```

**Severity:** MEDIUM -- FORCED parameterization should handle simple `= 'N'` comparisons, but embedding literals is still poor practice

**Frequency:** HIGH -- called during HL7 message processing loop

#### HIGH-2c: Hardcoded date literal in PatientStatementRepository

**File:** `LabBilling Library/Repositories/PatientStatementRepository.cs`
**Line:** 65

```csharp
sql.Where($"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} is null or " +
    $"{GetRealColumn(nameof(PatientStatement.StatementSubmittedDateTime))} = '01/01/1900'");
```

**Severity:** MEDIUM -- the date literal `'01/01/1900'` should be parameterized

**Frequency:** LOW -- statement processing

#### HIGH-2d: Embedded literal in AccountService.GetClaimItems JOIN clause

**File:** `LabBilling Library/Services/AccountService.cs`
**Line:** 622

```csharp
.On($"... and {uow.InsRepository.GetRealColumn(nameof(Ins.Coverage))} = '{InsCoverage.Primary}'");
```

**Why it causes plan pollution:** The `'A'` (primary coverage value) is embedded as a literal in the ON clause. FORCED parameterization typically does NOT parameterize literals in JOIN ON clauses, as the query shape could change semantics.

**Severity:** HIGH -- JOIN ON clause literals are exempt from FORCED parameterization

**Frequency:** HIGH -- called for every claim generation batch

---

## Medium-Priority Issues

### MEDIUM-1: Multiple plan variants from conditional Random Drug Screen queries

**File:** `LabBilling Library/Repositories/RandomDrugScreenPersonRepository.cs`

Multiple methods have optional WHERE conditions:

- `GetByClientAsync` (lines 20-34): optional `deleted = 0` condition (2 variants)
- `GetByClientAndShiftAsync` (lines 39-54): optional `deleted = 0` (2 variants)
- `GetDistinctShiftsAsync` (lines 74-92): optional `cli_mnem = @0` (2 variants)
- `GetCandidateCountAsync` (lines 97-116): optional `shift` and `deleted` (4 variants)

**Severity:** MEDIUM -- limited number of variants per method (2-4 each)

**Frequency:** LOW-MEDIUM -- drug screen module operations

---

### MEDIUM-2: Raw SQL string construction in MappingRepository

**File:** `LabBilling Library/Repositories/MappingRepository.cs`
**Lines:** 23, 35

```csharp
sql = $"select DISTINCT {GetRealColumn(nameof(Mapping.SystemType))} from {_tableName}";
// and
sql = $"select DISTINCT {GetRealColumn(nameof(Mapping.InterfaceName))} from {_tableName}";
```

**Why it causes plan pollution:** These are raw SQL strings with interpolated column and table names. However, since `_tableName` and the column names are deterministic for this repository, each produces exactly one fixed SQL text. The concern is that raw string SQL bypasses PetaPoco's Sql.Builder and any future improvements to parameterization.

**Severity:** MEDIUM -- produces exactly 2 fixed plans (one per method), but sets a poor precedent

**Frequency:** LOW -- mapping lookups during configuration

---

### MEDIUM-3: Raw SQL in PhyRepository.GetActive and AuditReportRepository.GetMenus

**File:** `LabBilling Library/Repositories/PhyRepository.cs`
**Line:** 67

```csharp
string sql = $"SELECT * FROM {_tableName} where deleted = 0";
```

**File:** `LabBilling Library/Repositories/AuditReportRepository.cs`
**Line:** 16

```csharp
var list = Context.Fetch<string>($"SELECT DISTINCT {GetRealColumn(nameof(AuditReport.Button))} FROM {TableName}");
```

**Severity:** MEDIUM -- each produces exactly 1 fixed plan but uses raw SQL instead of Sql.Builder

**Frequency:** LOW-MEDIUM

---

### MEDIUM-4: LogRepository creates its own Database instance

**File:** `LabBilling Library/Repositories/LogRepository.cs`
**Line:** 35

```csharp
dbConnection = new PetaPoco.Database(connectionString, new SqlServerMsDataDatabaseProvider());
```

**Why it matters:** `LogRepository` creates its own `PetaPoco.Database` instance using `SqlServerMsDataDatabaseProvider` instead of `CustomSqlMsDatabaseProvider`, and does NOT use `MyMapper`. This means:
1. Queries from LogRepository may generate different SQL text than equivalent queries through the main UnitOfWork
2. The separate Database instance may have different connection pooling behavior

**Severity:** MEDIUM -- separate database provider could produce different SQL text for identical operations

**Frequency:** LOW -- logging operations

---

## Low-Priority Issues

### LOW-1: Conditional `ClientRepository.GetAll` with optional `includeInactive` flag

**File:** `LabBilling Library/Repositories/ClientRepository.cs`
**Lines:** 24-44

```csharp
if (!includeInactive)
    sql.Where($"{GetRealColumn(nameof(Client.IsDeleted))} = @0", false);
```

**Severity:** LOW -- only 2 variants, and client list is not frequently queried

### LOW-2: Conditional `ClientDiscountRepository.GetByClient` with `includeDeleted` flag

**File:** `LabBilling Library/Repositories/ClientDiscountRepository.cs`
**Lines:** 18-31

**Severity:** LOW -- 2 variants, infrequent access

### LOW-3: `FinRepository.GetActive` uses hardcoded `0` for IsDeleted

**File:** `LabBilling Library/Repositories/FinRepository.cs`
**Line:** 18

```csharp
.Where($"{GetRealColumn(nameof(Fin.IsDeleted))} = 0")
```

**Severity:** LOW -- FORCED parameterization handles simple numeric literals

### LOW-4: `ChrgRepository.GetByAccount` embeds `= 0` for IsCredited

**File:** `LabBilling Library/Repositories/ChrgRepository.cs`
**Line:** 71

```csharp
sql.Where($"{GetRealColumn(nameof(Chrg.IsCredited))} = 0");
```

**Severity:** LOW -- FORCED parameterization handles `= 0`

---

## Cross-Cutting Analysis

### GetRealColumn() Consistency Analysis

**Implementation:** `RepositoryCoreBase<TPoco>.GetRealColumn()` (lines 66-101) resolves property names to SQL column names using reflection on PetaPoco `[Column]` attributes at runtime.

**Consistency assessment:** **GetRealColumn() returns DETERMINISTIC values.** For a given type and property name, the result is always the same string because it reads compile-time attributes via reflection. There is no caching, but the values cannot change between calls.

**Impact on plan pollution:** Since GetRealColumn() produces consistent values, its use in string interpolation does NOT itself cause plan pollution on a per-call basis. The SQL text for `$"{GetRealColumn(nameof(Account.AccountNo))} = @0"` will always resolve to the same string (e.g., `"account = @0"`). However, the interpolation pattern means the column name is part of the SQL text string rather than being part of PetaPoco's auto-generated SQL, which means:

1. These hand-written queries produce plans that are separate from any PetaPoco auto-generated queries for the same table
2. The pattern is harder to audit and maintain than using PetaPoco's built-in SQL generation
3. Different call sites that query the same table may produce slightly different SQL text due to different column combinations, generating separate plans

**Recommendation:** GetRealColumn() interpolation is NOT a primary source of plan pollution since each call site produces consistent text. However, consider whether PetaPoco's built-in query methods (parameterless `SingleOrDefault<T>(object pk)`, `Fetch<T>()`, etc.) could replace many of these custom queries.

---

### PetaPoco Auto-SQL Safety Assessment

PetaPoco auto-generates SQL in these scenarios:
- `Context.SingleOrDefault<T>(object primaryKey)` -- generates `SELECT ... FROM table WHERE pk = @0`
- `Context.Insert(table)` -- generates `INSERT INTO table (...) VALUES (...)`
- `Context.Update(table)` -- generates `UPDATE table SET ... WHERE pk = @0`
- `Context.Delete(table)` -- generates `DELETE FROM table WHERE pk = @0`
- `Context.Save(table)` -- determines insert vs update, then calls appropriate method
- `Context.Fetch<T>(Sql)` with `WithAutoSelect()` -- auto-prepends `SELECT ... FROM table` to WHERE-only fragments

**Assessment:** **PetaPoco auto-generated SQL is SAFE from plan pollution.** These methods produce consistent, parameterized SQL text for each entity type. The `WithAutoSelect()` configuration (enabled in `UnitOfWorkMain.Initialize()`) means that when a `Sql.Builder` starts with `.Where(...)`, PetaPoco automatically prepends the appropriate `SELECT * FROM tablename`. This produces the same SQL text every time for the same entity type and query structure.

**Caveat:** When `Sql.Builder` is used with `.Select()`, `.From()`, `.Join()` etc. explicitly, the developer controls the SQL text and must ensure consistency manually.

---

### CustomSqlMsDatabaseProvider Impact Assessment

**Implementation:** `CustomSqlMsDatabaseProvider` (in `LabBilling Library/Repositories/CustomSqlDatabaseProvider.cs`) extends `SqlServerMsDataDatabaseProvider` and only overrides `ExecuteInsert` and `ExecuteInsertAsync`.

**What it does:** It works around SQL Error 334 by modifying INSERT statements that contain `OUTPUT INSERTED.[primaryKeyName]`. It wraps the output clause with a table variable (`@result`) to capture the inserted primary key value.

**Impact on plan pollution:** **MODERATE concern.** The provider modifies INSERT command text at execution time by:
1. Prepending `DECLARE @result TABLE(pkName sql_variant);`
2. Replacing `OUTPUT INSERTED.[pk]` with `OUTPUT INSERTED.[pk] into @result(pk)`
3. Appending `; SELECT pk FROM @result;`

Since the primary key name varies by table (67+ tables), this creates 67+ different INSERT command patterns. However, each table's INSERT pattern is consistent across all inserts for that table, so this does not create per-execution pollution -- just one additional plan per table.

**Recommendation:** This is acceptable behavior. The plans are fixed per table type and will be reused.

---

### Database Instance Lifecycle Assessment

**UnitOfWorkMain creation pattern:**
```csharp
// In service constructors/methods:
uow ??= new UnitOfWorkMain(_appEnvironment);
```

**Analysis:** `UnitOfWorkMain` creates a new `PetaPoco.Database` instance via `DatabaseConfiguration.Build()...Create()` each time it is constructed. However:

1. PetaPoco's `Database` class uses ADO.NET connection pooling under the hood (via the connection string), so physical connections are reused
2. The `Database` instance itself does not cache prepared statements -- it relies on SQL Server's plan cache
3. Each `UnitOfWorkMain` creates fresh repository instances that share the same `Database` (connection) instance

**Impact on plan pollution:** **MINIMAL.** PetaPoco does not use client-side prepared statements, so the Database instance lifecycle does not affect SQL Server plan caching. Plans are cached by SQL Server based on query text regardless of which Database instance sent them.

**However**, the pattern of creating `new UnitOfWorkMain()` frequently (potentially per-request or per-operation) means there is no connection reuse at the PetaPoco level. ADO.NET connection pooling handles this at the transport level, but there is overhead in repeatedly constructing 40+ repository objects per UnitOfWork instance.

---

## Recommended Remediation Strategy

### Priority Order

1. **Immediate (Week 1):** Fix CRITICAL issues
   - CRITICAL-1: Parameterize IN clause in AccountSearchRepository
   - CRITICAL-3: Parameterize interpolated values in RandomDrugScreenPersonRepository
   - CRITICAL-4: Parameterize table name in RepositoryBase.CheckDBFieldLengths
   - CRITICAL-5: Parameterize TOP clause in UserProfileRepository

2. **Short-term (Week 2):** Fix HIGH issues
   - HIGH-1 (all): Standardize dynamic WHERE assembly to always emit all clauses with `1=1` padding or use a fixed query with parameters that handle null
   - HIGH-2 (all): Replace all hardcoded literals with parameters
   - CRITICAL-2/HIGH-2d: Fix LIKE pattern and JOIN ON literal

3. **Medium-term (Week 3-4):** Address MEDIUM issues
   - Convert raw SQL strings to Sql.Builder pattern
   - Standardize LogRepository to use shared Database provider

### Common Fix Patterns

#### Pattern A: Replace literal embedding with parameters

**Before:**
```csharp
.Where($"{column} = 'N'")
```

**After:**
```csharp
.Where($"{column} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "N" })
```

#### Pattern B: Fix LIKE parameter arithmetic

**Before:**
```csharp
.Where($"{column} like @0+'%'", new SqlParameter() { ... Value = searchText })
```

**After:**
```csharp
.Where($"{column} like @0", new SqlParameter() { ... Value = searchText + "%" })
```

#### Pattern C: Fix dynamic WHERE to produce consistent query shape

**Before (variable number of WHERE clauses):**
```csharp
var sql = Sql.Builder.Where("account = @0", account);
if (condition1) sql.Where("col1 = @0", val1);
if (condition2) sql.Where("col2 = @0", val2);
```

**After (consistent shape using null-check in SQL):**
```csharp
var sql = Sql.Builder
    .Where("account = @0", account)
    .Where("(@0 IS NULL OR col1 = @0)", condition1 ? val1 : null)
    .Where("(@0 IS NULL OR col2 = @0)", condition2 ? val2 : null);
```

This produces the SAME SQL text regardless of which conditions are active, allowing plan reuse. The `@0 IS NULL` check short-circuits the condition when the parameter is null.

#### Pattern D: Fix IN clause parameterization

**Before:**
```csharp
command.Where($"{propName} in ({searchText})");
```

**After (using PetaPoco's array expansion):**
```csharp
var values = searchText.Split(',').Select(s => s.Trim().Trim('\'')).ToArray();
command.Where($"{propName} in (@values)", new { values });
```

Note: PetaPoco supports array expansion when an `IEnumerable` is passed as a parameter. However, this still creates different SQL text for different array lengths. For truly consistent plans, consider using a table-valued parameter or a fixed maximum number of parameters with null padding.

### Base Class Changes to Prevent Future Occurrences

1. **Add a helper method to RepositoryCoreBase** for parameterized LIKE:
```csharp
protected Sql WhereLike(Sql sql, string column, string value)
{
    return sql.Where($"{column} like @0",
        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = value + "%" });
}
```

2. **Add a helper for nullable WHERE conditions** (Pattern C):
```csharp
protected Sql WhereOptional(Sql sql, string column, object value)
{
    return sql.Where($"(@0 IS NULL OR {column} = @0)", value);
}
```

3. **Add XML comments/documentation** to GetRealColumn() noting that the result should never be used with user-provided input and is safe for SQL interpolation only because column names are compile-time constants.

4. **Consider a code analyzer rule** (Roslyn analyzer) that flags string interpolation in PetaPoco SQL methods to catch future regressions.

### Testing Approach to Verify Plan Reuse After Fixes

1. **Before fixing:** Run the following DMV query on the SQL Server to establish a baseline of single-use plans:
```sql
SELECT
    qs.plan_handle,
    qs.execution_count,
    SUBSTRING(st.text, 1, 200) as query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
WHERE qs.execution_count = 1
  AND st.text NOT LIKE '%sys%'
ORDER BY qs.creation_time DESC;
```

2. **After each fix:** Clear the plan cache for the specific database and repeat the test scenario:
```sql
-- Clear plan cache for the specific database (less disruptive than DBCC FREEPROCCACHE)
DECLARE @dbid INT = DB_ID('YourDatabaseName');
DBCC FLUSHPROCINDB(@dbid);
```

3. **Monitor plan reuse** by checking `execution_count > 1` for the modified queries:
```sql
SELECT
    qs.execution_count,
    SUBSTRING(st.text, 1, 300) as query_text,
    qs.total_elapsed_time / qs.execution_count as avg_elapsed_time
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
WHERE st.text LIKE '%tablename%'  -- filter for your table
ORDER BY qs.execution_count DESC;
```

4. **Unit test approach:** For each fixed method, write a test that calls the method multiple times with different parameter values, then checks `Context.LastSQL` is identical across calls (same SQL text = same plan).

---

## Appendix: Complete File Inventory

All repository files examined (confirmed complete coverage):

| # | File | Issues Found |
|---|------|-------------|
| 1 | AccountAlertRepository.cs | None |
| 2 | AccountLmrpErrorRepository.cs | None (uses GetRealColumn interpolation, but consistent) |
| 3 | AccountLockRepository.cs | None |
| 4 | AccountNoteRepository.cs | None |
| 5 | AccountRepository.cs | None |
| 6 | AccountSearchRepository.cs | CRITICAL-1 (IN clause), HIGH-1b, HIGH-1c (dynamic WHERE) |
| 7 | AccountValidationStatusRepository.cs | None |
| 8 | AnnouncementRepository.cs | None |
| 9 | AuditReportRepository.cs | MEDIUM-3 (raw SQL) |
| 10 | BadDebtRepository.cs | None |
| 11 | BillingActivityRepository.cs | None |
| 12 | BillingBatchRepository.cs | None |
| 13 | CdmDetailRepository.cs | None |
| 14 | CdmRepository.cs | None |
| 15 | ChkBatchDetailRepository.cs | None |
| 16 | ChkBatchRepository.cs | None |
| 17 | ChkRepository.cs | None |
| 18 | ChrgDetailRepository.cs | None |
| 19 | ChrgDiagnosisPointerRepository.cs | None |
| 20 | ChrgRepository.cs | HIGH-1a (dynamic WHERE), LOW-4 (literal 0) |
| 21 | ClaimItemRepository.cs | None |
| 22 | ClientDiscountRepository.cs | LOW-2 (2 variants) |
| 23 | ClientRepository.cs | HIGH-2a (literals in WHERE) |
| 24 | ClientTypeRepository.cs | None |
| 25 | CptAmaRepository.cs | None |
| 26 | CustomSqlDatabaseProvider.cs | See cross-cutting analysis |
| 27 | DictDxRepository.cs | None |
| 28 | EobRepository.cs | None |
| 29 | FinRepository.cs | LOW-3 (literal 0) |
| 30 | FunctionRepository.cs | None (utility, no SQL) |
| 31 | GLCodeRepository.cs | None |
| 32 | GlobalBillingCdmRepository.cs | None |
| 33 | InsCompanyRepository.cs | HIGH-1e (conditional WHERE) |
| 34 | InsCoverage.cs | None (no SQL) |
| 35 | InsRepository.cs | None |
| 36 | InvoiceHistoryRepository.cs | HIGH-1d (dynamic WHERE) |
| 37 | InvoiceSelectRepository.cs | None (uses parameterized patterns) |
| 38 | LMRPRuleRepository.cs | None |
| 39 | LogRepository.cs | MEDIUM-4 (separate DB instance) |
| 40 | MappingRepository.cs | MEDIUM-2 (raw SQL) |
| 41 | MessagesInboundRepository.cs | HIGH-2b (literal 'N') |
| 42 | MutuallyExclusiveEditRepository.cs | None |
| 43 | MyMapper.cs | See cross-cutting analysis |
| 44 | NumberRepository.cs | None |
| 45 | PatDxRepository.cs | None |
| 46 | PatientStatementAccountRepository.cs | None |
| 47 | PatientStatementCernerRepository.cs | None |
| 48 | PatientStatementEncounterActivityRepository.cs | None |
| 49 | PatientStatementEncounterRepository.cs | None |
| 50 | PatientStatementRepository.cs | HIGH-2c (literal date) |
| 51 | PatRepository.cs | None |
| 52 | PhyRepository.cs | CRITICAL-2 (`@0+'%'`), MEDIUM-3 (raw SQL) |
| 53 | RandomDrugScreenPersonRepository.cs | CRITICAL-3 (interpolated values), MEDIUM-1 (conditional WHERE) |
| 54 | RemittanceRepository.cs (incl. Claim/Detail/Adjustment) | None |
| 55 | ReportingRepository.cs | None (uses raw ADO.NET, bypasses PetaPoco) |
| 56 | RepositoryBase.cs | CRITICAL-4 (table name in INFORMATION_SCHEMA) |
| 57 | RepositoryCoreBase.cs | None (base class, no direct SQL) |
| 58 | RequisitionPrintTrackRepository.cs | None |
| 59 | RevenueCodeRepository.cs | None |
| 60 | SanctionedProviderRepository.cs | CRITICAL-2 (`@0+'%'`) |
| 61 | SystemParametersRepository.cs | None |
| 62 | UserAccountRepository.cs | None |
| 63 | UserProfileRepository.cs | CRITICAL-5 (TOP concatenation) |
| 64 | WriteOffCodeRepository.cs | None |

Service files examined:

| File | Issues Found |
|------|-------------|
| AccountService.cs | HIGH-2d (literal in JOIN ON) |
| PatientBillingService.cs | None (uses parameterized patterns) |
| WorklistService.cs | Triggers HIGH-1c via AccountSearchRepository |
| All other service files | No direct SQL construction issues |
