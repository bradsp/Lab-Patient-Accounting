<objective>
Thoroughly analyze all indexes defined in `LabPatientAccounting DB Build.sql` and cross-reference them with the application's actual query patterns to identify index optimizations that will improve overall application performance. This includes finding missing indexes, redundant indexes, suboptimal index definitions, and opportunities for covering indexes.
</objective>

<context>
This is a medical billing application (Lab Patient Accounting) for outreach laboratories running on SQL Server 2014+. The database schema contains 500+ tables with numerous indexes accumulated over years of development.

The application uses PetaPoco as its ORM with attribute-based mapping. Query patterns are found in:
- Repository classes in `LabBilling Library/Repositories/` - primary location of database queries
- Service classes in `LabBilling Library/Services/` - business logic that constructs queries
- Model classes in `LabBilling Library/Models/` - entity definitions with PetaPoco attributes
- Raw SQL throughout the codebase using `Sql.Builder`, `db.Query`, `db.Fetch`, `db.Execute`
- Stored procedures and functions within the SQL schema itself

Key high-traffic areas of the application:
- Patient account lookup and search (AccountService)
- Claim generation and processing (Billing837Service, ClaimGeneratorService)
- HL7 message processing (HL7ProcessorService) - high-volume batch operations
- Charge and payment processing
- Insurance verification and billing

Read the project conventions from `CLAUDE.md` before starting.
</context>

<research>
Phase 1 - Extract current index definitions:
1. Read `LabPatientAccounting DB Build.sql` and catalog all indexes:
   - Index name
   - Table name
   - Index type (clustered, nonclustered, unique, filtered)
   - Indexed columns (in order)
   - Included columns (if any)
   - Filter predicate (if any)
   - Whether it's a primary key or unique constraint

Phase 2 - Analyze application query patterns:
1. Search all repository classes in `LabBilling Library/Repositories/` for:
   - WHERE clause patterns (which columns are filtered on)
   - JOIN conditions (which columns are used for joins)
   - ORDER BY clauses (which columns are sorted on)
   - GROUP BY clauses
   - Frequently queried column combinations
2. Search service classes for additional query construction
3. Examine stored procedures in the schema for their query patterns

Phase 3 - Cross-reference indexes with queries:
For each table with significant query activity, compare:
- What columns are actually queried/filtered/joined on
- What indexes currently exist
- Whether existing indexes support the query patterns efficiently

For maximum efficiency, whenever you need to perform multiple independent search operations, invoke all relevant tools simultaneously rather than sequentially.

After receiving search results, carefully reflect on their quality and determine optimal next steps before proceeding.
</research>

<analysis_requirements>
Identify and categorize the following:

1. **Redundant Indexes** - Indexes that are subsets of other indexes on the same table (the broader index can serve both purposes)
   - Example: If index A covers (col1) and index B covers (col1, col2), index A may be redundant
   - Note: A narrower index may still be preferred for specific queries if it's significantly smaller

2. **Missing Indexes** - Query patterns that would benefit from indexes that don't exist
   - Focus on high-frequency queries (repositories called from main services)
   - Identify WHERE clause columns that lack supporting indexes
   - Identify JOIN columns without indexes
   - Identify common multi-column filter combinations that could benefit from composite indexes

3. **Suboptimal Index Definitions** - Existing indexes that could be improved
   - Wrong column order in composite indexes (most selective column should typically be first)
   - Indexes missing INCLUDE columns that would make them covering indexes for common queries
   - Indexes that could benefit from filter predicates to reduce size

4. **Over-Indexed Tables** - Tables with too many indexes causing write performance degradation
   - Identify tables with more than 8-10 nonclustered indexes
   - Assess whether all indexes are necessary based on query patterns

5. **Clustered Index Issues** - Tables where the clustered index choice may not be optimal
   - Identity columns vs. natural keys
   - Wide clustered keys causing page splits
   - Tables missing clustered indexes entirely (heaps)

For each recommendation, provide:
- The specific table and current index state
- The query pattern(s) that drive the recommendation
- The expected performance impact (high/medium/low)
- Any trade-offs (e.g., write performance impact of adding indexes)
</analysis_requirements>

<output>
Create two output files:

1. `./analyses/002-index-optimization-report.md` - Detailed analysis report containing:
   - Executive summary with key findings and estimated impact
   - Current index inventory summary (counts by type, tables with most indexes)
   - Redundant indexes (with DROP recommendations)
   - Missing indexes (with CREATE recommendations)
   - Suboptimal indexes (with modification recommendations)
   - Over-indexed tables analysis
   - Clustered index review
   - Priority-ordered recommendation list (highest impact first)

2. `./analyses/002-index-optimization.sql` - SQL script containing:
   - Commented header explaining the script's purpose
   - Organized into sections: DROP redundant indexes, CREATE missing indexes, ALTER existing indexes
   - Each statement with a comment explaining the rationale and expected impact
   - Wrapped in transactions where appropriate
   - Safe to run incrementally (each operation independent)
   - IF EXISTS checks on all DROP statements
   - IF NOT EXISTS checks on all CREATE statements
</output>

<verification>
Before declaring complete, verify your work:
- Confirm that no "redundant" index is actually required for a unique constraint
- Verify that recommended composite index column orders match actual query patterns
- Check that DROP recommendations don't remove indexes required by foreign key constraints
- Ensure new index recommendations don't duplicate existing indexes
- Validate that the SQL script is syntactically correct
</verification>

<success_criteria>
- Complete catalog of all current indexes in the schema
- At least 5 actionable index optimization recommendations with supporting evidence from query patterns
- Clear priority ordering of recommendations by expected impact
- Executable SQL script for implementing all recommendations
- No recommendations that would break existing functionality
</success_criteria>
