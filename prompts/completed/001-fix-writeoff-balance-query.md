<objective>
Fix the account balance query in WriteOffRemainingBalances.cs that is returning incorrect results.

The current implementation returns 252,288 accounts with balances when only approximately 600 accounts should have outstanding balances. This indicates a fundamental flaw in the query logic.
</objective>

<context>
File to modify: @LabBillingConsole/WriteOffRemainingBalances.cs

The `GetAccountsWithPositiveBalance()` method (lines 133-193) queries accounts and calculates balances using:
- Charges: `SUM(qty * retail)` from `chrg` table where `credited = 0`
- Payments: `SUM(amt_paid + contractual + write_off)` from `chk` table
- Balance = Charges - Payments

The query has two known issues:
1. It does not exclude accounts with status "PAID_OUT" (closed/zeroed accounts)
2. The balance calculation logic may be incorrect, returning far too many accounts
</context>

<requirements>
1. **Add status filter**: Exclude accounts where `acc.status = 'PAID_OUT'` from the query

2. **Verify balance calculation logic**:
   - Examine how other parts of the codebase calculate account balances
   - Search for existing balance calculation patterns in AccountService or AccountRepository
   - Ensure the charge filtering logic (CBILL, CAP, N/A statuses) is correct
   - Verify the payment/write-off summation includes all relevant payment records

3. **Cross-reference with existing logic**:
   - Check @LabBilling Library/Services/AccountService.cs for balance calculation methods
   - Check @LabBilling Library/Repositories/AccountRepository.cs for existing balance queries
   - Use consistent logic with the rest of the application
</requirements>

<research>
Before modifying, search the codebase for:
- Existing balance calculation methods (grep for "balance" in AccountService, AccountRepository)
- How other reports or features calculate account balances
- Any stored procedures or views that calculate balances
</research>

<implementation>
1. Add `acc.status <> 'PAID_OUT'` to the WHERE clause in GetAccountsWithPositiveBalance()

2. Review and potentially refactor the balance subqueries to match existing patterns

3. Consider whether there are additional status values that should be excluded (e.g., 'CLOSED', 'VOID')

4. Ensure the query handles NULL values correctly in all calculations
</implementation>

<output>
Modify: `./LabBillingConsole/WriteOffRemainingBalances.cs`
- Update the GetAccountsWithPositiveBalance() method with corrected query logic
</output>

<verification>
After making changes:
1. The query should return approximately 600 accounts (not 252,288)
2. All returned accounts should have status values other than 'PAID_OUT'
3. Run a manual spot-check: verify a few returned accounts actually have outstanding balances
</verification>

<success_criteria>
- Accounts with status 'PAID_OUT' are excluded from results
- Balance calculation matches existing application logic
- Query returns a reasonable number of accounts (~600 as expected)
- No regression in the accuracy of balance calculations
</success_criteria>
