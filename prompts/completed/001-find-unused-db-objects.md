<objective>
Thoroughly analyze the database schema defined in `LabPatientAccounting DB Build.sql` and compare it against the entire codebase to identify unused stored procedures, functions, and tables. The goal is to produce a comprehensive inventory of database objects that are no longer referenced by application code, so they can be safely removed to reduce schema complexity and maintenance burden.
</objective>

<context>
This is a medical billing application (Lab Patient Accounting) for outreach laboratories. The database schema contains 500+ tables and numerous stored procedures and functions accumulated over years of development.

The application uses PetaPoco as its ORM with attribute-based mapping. Database objects are referenced in:
- Repository classes in `LabBilling Library/Repositories/`
- Service classes in `LabBilling Library/Services/`
- Model classes in `LabBilling Library/Models/` (table/column mappings via PetaPoco attributes)
- Raw SQL queries throughout the codebase
- The Windows service (`LabBillingService/`)
- The job scheduler (`Lab Patient Accounting Job Scheduler/`)
- Console tools (`LabBillingConsole/`)
- WinForms UI (`Lab PA WinForms UI/`)
- Blazor UI (`LabOutreachUI/`)

Read the project conventions from `CLAUDE.md` before starting.
</context>

<research>
Phase 1 - Extract all database objects from the schema:
1. Read `LabPatientAccounting DB Build.sql` and extract a complete list of:
   - All table names (CREATE TABLE statements)
   - All stored procedure names (CREATE PROCEDURE statements)
   - All function names (CREATE FUNCTION statements)
   - All view names (CREATE VIEW statements)

Phase 2 - Search the codebase for references:
For each database object identified in Phase 1, search the entire codebase (excluding the SQL build script itself and any migration files) for references. Search patterns should include:
- Direct name references in C# code (SQL strings, PetaPoco queries, stored procedure calls)
- PetaPoco `[TableName("...")]` attributes in model classes
- `Sql.Builder` and raw SQL construction
- `db.Execute`, `db.Query`, `db.Fetch`, `db.SingleOrDefault`, `db.FirstOrDefault` calls referencing the object
- `EXEC` or `EXECUTE` statements in any embedded SQL
- String literals containing the object name
- References in other SQL files, config files, or resource files

Phase 3 - Cross-reference stored procedures and functions internally:
Some stored procedures or functions may only be called by other stored procedures or functions within the schema itself. Check for internal references within `LabPatientAccounting DB Build.sql` to distinguish between:
- Objects referenced by application code
- Objects referenced only by other database objects
- Objects with no references anywhere (completely orphaned)

For maximum efficiency, whenever you need to perform multiple independent search operations, invoke all relevant tools simultaneously rather than sequentially.

After receiving search results, carefully reflect on their quality and determine if additional searches are needed before concluding an object is unused.
</research>

<analysis_requirements>
Categorize findings into:

1. **Completely Unused** - No references in application code OR other database objects
2. **Only Referenced by Other DB Objects** - Called by stored procedures/functions but not by application code (note which objects reference them)
3. **Potentially Unused** - Only referenced in comments, dead code paths, or disabled features (explain why you believe the reference is inactive)

For each unused object, provide:
- Object type (table, stored procedure, function, view)
- Object name
- Brief description of what the object appears to do (based on its definition)
- Confidence level (high/medium/low) that it is truly unused
- Any caveats or risks in removing it

Pay special attention to:
- Tables that might be referenced via dynamic SQL construction
- Objects that might be used by external systems, SSRS reports, or ETL processes (flag these as uncertain)
- Audit/logging tables that may be written to by triggers but never queried by the application
</analysis_requirements>

<output>
Create two output files:

1. `./analyses/001-unused-db-objects-report.md` - Detailed analysis report containing:
   - Executive summary with counts of unused objects by type
   - Full inventory tables organized by object type
   - Risk assessment for removal
   - Recommended removal order (remove dependent objects first)

2. `./analyses/001-unused-db-objects-cleanup.sql` - SQL script containing:
   - Commented header explaining the script's purpose
   - DROP statements organized by dependency order (functions/procedures first, then views, then tables)
   - Each DROP wrapped in an IF EXISTS check
   - Comments noting confidence level and any caveats
   - The script should be safe to run incrementally (each DROP is independent)
</output>

<verification>
Before declaring complete, verify your work:
- Spot-check at least 10 objects marked as "unused" by performing additional targeted searches to confirm no references were missed
- Verify that no objects in the "unused" list are referenced by PetaPoco model attributes
- Confirm that dependency ordering in the cleanup script is correct (no DROP of an object still referenced by another object in the schema)
- Ensure the cleanup script would not break any foreign key relationships with tables still in use
</verification>

<success_criteria>
- Complete inventory of all stored procedures, functions, tables, and views in the schema
- Each object categorized as used, unused, or uncertain with supporting evidence
- Cleanup SQL script that can be safely executed
- No false positives in the "high confidence" unused category
</success_criteria>
