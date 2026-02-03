<objective>
Diagnose and fix a regression bug causing `System.ArgumentException: The SqlParameter is already contained by another SqlParameterCollection` when searching for patients by lastname and firstname.

This error was introduced by recent plan cache pollution fixes that refactored conditional WHERE clauses in repository methods to always-present clauses using the `(@0 IS NULL OR column = @0)` pattern with `DBNull.Value` or `SqlParameter` objects.
</objective>

<context>
Read `CLAUDE.md` for project conventions.

The error occurs when a user searches for a patient by entering a lastname and firstname. The stack trace points to `Microsoft.Data.SqlClient.SqlParameterCollection.Validate`.

**Root cause hypothesis:** The `SqlParameter` is already contained by another `SqlParameterCollection` error happens when the same `SqlParameter` **object instance** is passed to multiple PetaPoco `.Where()` calls. PetaPoco internally adds each parameter to a `SqlParameterCollection`, and SQL Server's ADO.NET does not allow the same `SqlParameter` instance to belong to multiple collections.

This likely affects the `AccountSearchRepository.GetBySearch` method (the overload with 7 string parameters: lastName, firstName, mrn, ssn, dob, sex, account) which was recently refactored to always include all WHERE clauses using `(@0 IS NULL OR column op @0)` pattern.

However, the bug could also exist in ANY of the recently modified files. Check all files that were changed in commit `82c5bb2` (the plan cache pollution fix commit).
</context>

<research>
1. Read `LabBilling Library/Repositories/AccountSearchRepository.cs` — focus on the `GetBySearch` method with 7 string parameters. Look for any `SqlParameter` instances that might be shared or reused.

2. The specific anti-pattern to look for is:
   - A `SqlParameter` object created once and passed to multiple `.Where()` calls
   - OR a conditional expression like `condition ? (object)someValue : DBNull.Value` where `someValue` is a `SqlParameter` that gets reused

3. Also check these files that were recently modified (any could have the same bug):
   - `LabBilling Library/Repositories/ChrgRepository.cs` (GetByAccount)
   - `LabBilling Library/Repositories/InvoiceHistoryRepository.cs` (GetWithSort)
   - `LabBilling Library/Repositories/InsCompanyRepository.cs` (GetAll)
   - `LabBilling Library/Repositories/RandomDrugScreenPersonRepository.cs` (multiple methods)
   - `LabBilling Library/Repositories/ClientRepository.cs` (GetUnbilledAccounts)
   - `LabBilling Library/Repositories/MessagesInboundRepository.cs`
   - `LabBilling Library/Repositories/PatientStatementRepository.cs`
   - `LabBilling Library/Services/AccountService.cs`
</research>

<requirements>
1. **Diagnose** the exact cause by reading the affected files and identifying where `SqlParameter` instances are being reused across multiple parameter collections.

2. **Fix** every instance of the bug across ALL recently modified files, not just the one that triggered the error. The same pattern was applied to many files, so the same bug likely exists in multiple places.

3. **The fix pattern:** Each `.Where()` call needs its OWN `SqlParameter` instance. Never share a `SqlParameter` object between multiple `.Where()` calls. If the same value needs to appear in multiple parameters, create a new `SqlParameter` for each usage.

   Common fix: Instead of creating a `SqlParameter` once and referencing it in a ternary, create a new one inline for each `.Where()` call. Or, if PetaPoco supports it, pass the raw value instead of a `SqlParameter` when using `DBNull.Value` as the alternative.

4. **Verify logical equivalence** — the fix must not change the query behavior, only how parameters are constructed.
</requirements>

<constraints>
- Do NOT revert the plan cache pollution fixes. The `(@0 IS NULL OR column = @0)` pattern is correct and should be preserved.
- Do NOT change the query shapes — only fix how SqlParameter objects are instantiated.
- Use `Microsoft.Data.SqlClient.SqlParameter` (not `System.Data.SqlClient`).
</constraints>

<verification>
After making changes:
1. Run `dotnet build "Lab Billing.sln"` — must compile without errors.
2. Run `dotnet test "LabBillingCore.UnitTests/LabBillingCore.UnitTests.csproj"` — all tests must pass.
3. Mentally trace through a patient search with lastname="Smith" and firstname="John" (all other params empty/null) to confirm:
   - Each `.Where()` call gets its own unique `SqlParameter` instance (or `DBNull.Value`)
   - No `SqlParameter` instance is shared between multiple `.Where()` calls
   - The SQL produced is logically equivalent to the pre-fix version
</verification>

<success_criteria>
- The `SqlParameter is already contained by another SqlParameterCollection` exception no longer occurs during patient search.
- All recently modified repository files are checked and fixed if they have the same issue.
- Solution builds cleanly.
- All tests pass.
</success_criteria>
