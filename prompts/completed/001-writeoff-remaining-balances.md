<objective>
Create a single-use C# console script to add write-off transactions for all accounts with a balance greater than zero.

This is for end-of-period cleanup to write off small remaining balances. The script must support a dry-run mode that previews affected accounts before making any changes.
</objective>

<context>
This is a medical billing application using:
- .NET 8.0 / C#
- PetaPoco ORM with SQL Server
- Repository pattern with UnitOfWork (UnitOfWorkMain)
- Existing services: AccountService, DictionaryService

Review CLAUDE.md for project conventions and architecture.

Examine these files to understand the existing patterns:
- `LabBilling Library/Services/AccountService.cs` - Account operations and transaction handling
- `LabBilling Library/Models/` - Account and transaction models
- `LabBilling Library/Repositories/` - Repository patterns for data access
- `LabBillingConsole/` - Existing console app patterns for reference
</context>

<requirements>
1. **Query all accounts** with balance > 0
2. **Write-off type**: Small balance write-off (use appropriate write-off code from the system)
3. **Dry run mode** (default behavior):
   - List all accounts that would be affected
   - Show account number, patient name, current balance
   - Display total count and sum of balances
   - Prompt user for confirmation before proceeding
4. **Execution mode** (after confirmation):
   - Create write-off transaction for each account's full remaining balance
   - Log each transaction created
   - Report summary on completion (accounts processed, total written off, any errors)
5. **Error handling**:
   - Continue processing remaining accounts if one fails
   - Collect and report all errors at the end
   - Use transactions appropriately for data integrity
</requirements>

<implementation>
1. First, research the existing codebase to understand:
   - How write-off transactions are typically created (look for existing write-off code in AccountService)
   - What write-off codes/types exist in the system
   - The correct model and properties for creating transactions
   - How the console apps connect to the database

2. Create the script following existing patterns in LabBillingConsole

3. Use dependency injection and services where available rather than direct SQL

4. Include command-line argument support:
   - `--execute` flag to skip dry-run and execute immediately (optional, for scripted use)
   - Default without flags = dry run mode

5. WHY dry run is critical: This is a bulk financial operation that cannot easily be undone. The preview step protects against accidentally writing off accounts that shouldn't be affected.
</implementation>

<output>
Create or modify:
- `./LabBillingConsole/WriteOffRemainingBalances.cs` - The main script logic
- Update `./LabBillingConsole/Program.cs` if needed to invoke the new functionality

Or if a separate executable is more appropriate based on the existing patterns, create it in a suitable location.
</output>

<verification>
Before declaring complete:
1. Build the solution to ensure no compilation errors: `dotnet build "Lab Billing.sln"`
2. Verify the dry-run output format is clear and includes all required information
3. Confirm the write-off transaction creation follows the same patterns used elsewhere in the codebase
4. Test that the `--execute` flag properly bypasses the confirmation prompt
</verification>

<success_criteria>
- Script compiles without errors
- Dry run mode shows preview of all accounts with balance > 0
- Execution mode creates valid write-off transactions
- Errors are handled gracefully without stopping the entire batch
- Summary report shows accounts processed and total amount written off
</success_criteria>
