<objective>
Thoroughly analyze all stored procedures and functions defined in `LabPatientAccounting DB Build.sql` to identify performance optimizations, anti-patterns, and improvements. The goal is to produce optimized versions of underperforming stored procedures along with a detailed report explaining each optimization and its expected impact.
</objective>

<context>
This is a medical billing application (Lab Patient Accounting) for outreach laboratories running on SQL Server 2014+. The stored procedures handle critical operations including:
- Patient account management and lookups
- Charge and payment processing
- Insurance claim generation
- HL7 message processing (high-volume batch operations)
- Reporting and data aggregation
- Data maintenance and cleanup operations

The application uses PetaPoco as its ORM, but many operations are delegated to stored procedures for complex business logic or performance-critical operations.

Read the project conventions from `CLAUDE.md` before starting.
</context>

<research>
Phase 1 - Extract and catalog all stored procedures and functions:
1. Read `LabPatientAccounting DB Build.sql` and identify all:
   - Stored procedures (CREATE PROCEDURE)
   - Scalar functions (CREATE FUNCTION returning scalar)
   - Table-valued functions (CREATE FUNCTION returning TABLE)
   - Their approximate line counts and complexity

Phase 2 - Analyze each stored procedure for common anti-patterns:
Review each stored procedure and function for the following SQL Server performance anti-patterns:

**Query-Level Issues:**
- SELECT * instead of explicit column lists
- Missing or inappropriate use of NOLOCK/transaction isolation hints
- Cursors where set-based operations would work
- Row-by-row processing (RBAR) instead of set-based logic
- Scalar functions in WHERE clauses or SELECT lists (causes row-by-row evaluation)
- Implicit type conversions in JOIN or WHERE conditions
- Non-sargable WHERE clauses (functions on indexed columns, LIKE with leading wildcard)
- Missing SET NOCOUNT ON (causes extra network roundtrips)
- Unnecessary ORDER BY in subqueries or intermediate results
- SELECT DISTINCT used to mask duplicate join issues
- UNION instead of UNION ALL when duplicates aren't possible

**Structural Issues:**
- Missing error handling (TRY/CATCH blocks)
- Missing transaction management where needed
- Temp tables vs. table variables vs. CTEs used inappropriately
- Dynamic SQL without parameterization (SQL injection risk + plan cache bloat)
- Parameter sniffing vulnerabilities in complex procedures
- Missing or stale statistics hints
- Excessive recompilation (unnecessary OPTION(RECOMPILE))

**Design Issues:**
- Procedures doing too much (should be split)
- Redundant logic across multiple procedures
- Hard-coded values that should be parameters
- Missing input validation
- Unnecessary complexity

Phase 3 - Identify the most impactful procedures to optimize:
Focus optimization efforts on:
1. Procedures called frequently by the application (cross-reference with codebase repository/service calls)
2. Procedures that process large datasets
3. Procedures with the most anti-patterns
4. Procedures involved in critical business operations (claim generation, HL7 processing, payment posting)

For maximum efficiency, whenever you need to perform multiple independent search operations, invoke all relevant tools simultaneously rather than sequentially.

After receiving search results, carefully reflect on their quality and determine optimal next steps before proceeding.
</research>

<analysis_requirements>
For each stored procedure and function, assess:

1. **Severity Rating**: Critical / High / Medium / Low based on:
   - How frequently the procedure is called
   - How much data it processes
   - How many anti-patterns it contains
   - Whether it's in a critical code path

2. **Anti-Patterns Found**: List each specific issue with:
   - Line number or location within the procedure
   - What the current code does
   - Why it's a problem (explain the performance impact)
   - What the optimized version should look like

3. **Optimization Recommendations**: For each procedure with issues:
   - Specific code changes with before/after comparisons
   - Expected performance improvement
   - Any risks or behavioral changes from the optimization
   - Whether the optimization requires testing of dependent application code

Group findings by priority:
- **Critical**: Procedures with severe performance issues in high-traffic code paths
- **High**: Procedures with multiple anti-patterns or significant inefficiencies
- **Medium**: Procedures with minor optimization opportunities
- **Low**: Style improvements or minor best-practice violations
</analysis_requirements>

<output>
Create two output files:

1. `./analyses/003-stored-procedure-optimization-report.md` - Detailed analysis report containing:
   - Executive summary with key findings
   - Statistics: total procedures/functions analyzed, issues found by severity
   - Detailed findings for each procedure with issues, organized by severity
   - Before/after code comparisons for top recommendations
   - Common anti-patterns summary with counts of occurrences across all procedures
   - Risk assessment for each recommended change
   - Recommended implementation order

2. `./analyses/003-stored-procedure-optimization.sql` - SQL script containing:
   - Commented header explaining the script's purpose
   - Optimized CREATE OR ALTER PROCEDURE/FUNCTION statements for all procedures with Critical or High severity issues
   - Each optimized procedure preceded by a comment block explaining:
     - What was changed and why
     - Expected performance impact
     - Any behavioral differences from the original
   - Original procedure definitions preserved as comments for reference
   - Script organized by priority (Critical first, then High)
</output>

<verification>
Before declaring complete, verify your work:
- Ensure optimized procedures maintain the same input/output contract as originals
- Verify that SET NOCOUNT ON is included where recommended
- Confirm that error handling additions don't change existing error behavior
- Check that any cursor-to-set-based conversions produce equivalent results
- Validate that the optimized SQL script is syntactically correct
- Cross-reference critical procedures with application code to confirm they're actively used
</verification>

<success_criteria>
- All stored procedures and functions in the schema reviewed
- Each procedure categorized by severity with specific anti-patterns identified
- Optimized SQL code provided for all Critical and High severity procedures
- Clear explanation of each optimization and its expected impact
- No optimizations that would change the functional behavior of procedures
- Executable SQL script for implementing all Critical and High priority optimizations
</success_criteria>
